﻿using PowerStore.Domain.Customers;
using PowerStore.Domain.Localization;
using PowerStore.Domain.Messages;
using PowerStore.Domain.Tax;
using PowerStore.Services.Authentication;
using PowerStore.Services.Common;
using PowerStore.Services.Customers;
using PowerStore.Services.Helpers;
using PowerStore.Services.Messages;
using PowerStore.Services.Tax;
using PowerStore.Web.Commands.Models.Customers;
using PowerStore.Web.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PowerStore.Web.Commands.Handler.Customers
{
    public class UpdateCustomerInfoCommandHandler : IRequestHandler<UpdateCustomerInfoCommand, bool>
    {
        private readonly ICustomerRegistrationService _customerRegistrationService;
        private readonly IPowerStoreAuthenticationService _authenticationService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IVatService _checkVatService;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;
        private readonly ICustomerService _customerService;
        private readonly IMediator _mediator;

        private readonly DateTimeSettings _dateTimeSettings;
        private readonly CustomerSettings _customerSettings;
        private readonly TaxSettings _taxSettings;
        private readonly LocalizationSettings _localizationSettings;

        public UpdateCustomerInfoCommandHandler(
            ICustomerRegistrationService customerRegistrationService,
            IPowerStoreAuthenticationService authenticationService,
            IGenericAttributeService genericAttributeService,
            IVatService checkVatService,
            IWorkflowMessageService workflowMessageService,
            INewsLetterSubscriptionService newsLetterSubscriptionService,
            ICustomerService customerService,
            IMediator mediator,
            DateTimeSettings dateTimeSettings,
            CustomerSettings customerSettings,
            TaxSettings taxSettings,
            LocalizationSettings localizationSettings)
        {
            _customerRegistrationService = customerRegistrationService;
            _authenticationService = authenticationService;
            _genericAttributeService = genericAttributeService;
            _checkVatService = checkVatService;
            _workflowMessageService = workflowMessageService;
            _newsLetterSubscriptionService = newsLetterSubscriptionService;
            _customerService = customerService;
            _mediator = mediator;
            _dateTimeSettings = dateTimeSettings;
            _customerSettings = customerSettings;
            _taxSettings = taxSettings;
            _localizationSettings = localizationSettings;
        }

        public async Task<bool> Handle(UpdateCustomerInfoCommand request, CancellationToken cancellationToken)
        {
            //username 
            if (_customerSettings.UsernamesEnabled && _customerSettings.AllowUsersToChangeUsernames)
            {
                if (!request.Customer.Username.Equals(request.Model.Username.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    //change username
                    await _customerRegistrationService.SetUsername(request.Customer, request.Model.Username.Trim());
                    //re-authenticate
                    if (request.OriginalCustomerIfImpersonated == null)
                        await _authenticationService.SignIn(request.Customer, true);
                }
            }
            //email
            if (!request.Customer.Email.Equals(request.Model.Email.Trim(), StringComparison.OrdinalIgnoreCase) && _customerSettings.AllowUsersToChangeEmail)
            {
                //change email
                await _customerRegistrationService.SetEmail(request.Customer, request.Model.Email.Trim());
                //re-authenticate (if usernames are disabled)
                //do not authenticate users in impersonation mode
                if (request.OriginalCustomerIfImpersonated == null)
                {
                    //re-authenticate (if usernames are disabled)
                    if (!_customerSettings.UsernamesEnabled)
                        await _authenticationService.SignIn(request.Customer, true);
                }
            }

            //VAT number
            if (_taxSettings.EuVatEnabled)
            {
                await UpdateTax(request);
            }

            //form fields
            await UpdateGenericAttributeFields(request);

            //newsletter
            if (_customerSettings.NewsletterEnabled)
            {
                await UpdateNewsletter(request);
            }

            //save customer attributes
            await _customerService.UpdateCustomerField(request.Customer, x => x.Attributes, request.CustomerAttributes);

            //notification
            await _mediator.Publish(new CustomerInfoEvent(request.Customer, request.Model, request.Form, request.CustomerAttributes));

            return true;

        }

        private async Task UpdateTax(UpdateCustomerInfoCommand request)
        {
            var prevVatNumber = await request.Customer.GetAttribute<string>(_genericAttributeService, SystemCustomerAttributeNames.VatNumber);

            await _genericAttributeService.SaveAttribute(request.Customer, SystemCustomerAttributeNames.VatNumber, request.Model.VatNumber);

            if (prevVatNumber != request.Model.VatNumber)
            {
                var vat = (await _checkVatService.GetVatNumberStatus(request.Model.VatNumber));
                await _genericAttributeService.SaveAttribute(request.Customer,
                        SystemCustomerAttributeNames.VatNumberStatusId,
                        (int)vat.status);

                //send VAT number admin notification
                if (!String.IsNullOrEmpty(request.Model.VatNumber) && _taxSettings.EuVatEmailAdminWhenNewVatSubmitted)
                    await _workflowMessageService.SendNewVatSubmittedStoreOwnerNotification(request.Customer, request.Store, request.Model.VatNumber, vat.address,
                        _localizationSettings.DefaultAdminLanguageId);
            }
        }

        private async Task UpdateGenericAttributeFields(UpdateCustomerInfoCommand request)
        {
            //properties
            if (_dateTimeSettings.AllowCustomersToSetTimeZone)
            {
                await _genericAttributeService.SaveAttribute(request.Customer, SystemCustomerAttributeNames.TimeZoneId, request.Model.TimeZoneId);
            }

            if (_customerSettings.GenderEnabled)
                await _genericAttributeService.SaveAttribute(request.Customer, SystemCustomerAttributeNames.Gender, request.Model.Gender);

            await _genericAttributeService.SaveAttribute(request.Customer, SystemCustomerAttributeNames.FirstName, request.Model.FirstName);
            await _genericAttributeService.SaveAttribute(request.Customer, SystemCustomerAttributeNames.LastName, request.Model.LastName);
            if (_customerSettings.DateOfBirthEnabled)
            {
                DateTime? dateOfBirth = request.Model.ParseDateOfBirth();
                await _genericAttributeService.SaveAttribute(request.Customer, SystemCustomerAttributeNames.DateOfBirth, dateOfBirth);
            }
            if (_customerSettings.CompanyEnabled)
                await _genericAttributeService.SaveAttribute(request.Customer, SystemCustomerAttributeNames.Company, request.Model.Company);
            if (_customerSettings.StreetAddressEnabled)
                await _genericAttributeService.SaveAttribute(request.Customer, SystemCustomerAttributeNames.StreetAddress, request.Model.StreetAddress);
            if (_customerSettings.StreetAddress2Enabled)
                await _genericAttributeService.SaveAttribute(request.Customer, SystemCustomerAttributeNames.StreetAddress2, request.Model.StreetAddress2);
            if (_customerSettings.ZipPostalCodeEnabled)
                await _genericAttributeService.SaveAttribute(request.Customer, SystemCustomerAttributeNames.ZipPostalCode, request.Model.ZipPostalCode);
            if (_customerSettings.CityEnabled)
                await _genericAttributeService.SaveAttribute(request.Customer, SystemCustomerAttributeNames.City, request.Model.City);
            if (_customerSettings.CountryEnabled)
                await _genericAttributeService.SaveAttribute(request.Customer, SystemCustomerAttributeNames.CountryId, request.Model.CountryId);
            if (_customerSettings.CountryEnabled && _customerSettings.StateProvinceEnabled)
                await _genericAttributeService.SaveAttribute(request.Customer, SystemCustomerAttributeNames.StateProvinceId, request.Model.StateProvinceId);
            if (_customerSettings.PhoneEnabled)
                await _genericAttributeService.SaveAttribute(request.Customer, SystemCustomerAttributeNames.Phone, request.Model.Phone);
            if (_customerSettings.FaxEnabled)
                await _genericAttributeService.SaveAttribute(request.Customer, SystemCustomerAttributeNames.Fax, request.Model.Fax);
        }

        private async Task UpdateNewsletter(UpdateCustomerInfoCommand request)
        {
            var categories = new List<string>();
            foreach (string formKey in request.Form.Keys)
            {
                if (formKey.Contains("customernewsletterCategory_"))
                {
                    try
                    {
                        var category = formKey.Split('_')[1];
                        categories.Add(category);
                    }
                    catch { }
                }
            }
            //save newsletter value
            var newsletter = await _newsLetterSubscriptionService.GetNewsLetterSubscriptionByEmailAndStoreId(request.Customer.Email, request.Store.Id);

            if (newsletter != null)
            {
                newsletter.Categories.Clear();
                categories.ForEach(x => newsletter.Categories.Add(x));

                if (request.Model.Newsletter)
                {
                    newsletter.Active = true;
                    await _newsLetterSubscriptionService.UpdateNewsLetterSubscription(newsletter);
                }
                else
                {
                    newsletter.Active = false;
                    await _newsLetterSubscriptionService.UpdateNewsLetterSubscription(newsletter);
                }
            }
            else
            {
                if (request.Model.Newsletter)
                {
                    var newsLetterSubscription = new NewsLetterSubscription {
                        NewsLetterSubscriptionGuid = Guid.NewGuid(),
                        Email = request.Customer.Email,
                        CustomerId = request.Customer.Id,
                        Active = true,
                        StoreId = request.Store.Id,
                        CreatedOnUtc = DateTime.UtcNow
                    };
                    categories.ForEach(x => newsLetterSubscription.Categories.Add(x));
                    await _newsLetterSubscriptionService.InsertNewsLetterSubscription(newsLetterSubscription);
                }
            }
        }
    }
}
