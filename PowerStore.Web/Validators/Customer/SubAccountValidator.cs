﻿using FluentValidation;
using PowerStore.Domain.Customers;
using PowerStore.Core.Validators;
using PowerStore.Services.Localization;
using PowerStore.Web.Models.Customer;
using System.Collections.Generic;

namespace PowerStore.Web.Validators.Customer
{
    public class SubAccountValidator : BasePowerStoreValidator<SubAccountModel>
    {
        public SubAccountValidator(
            IEnumerable<IValidatorConsumer<SubAccountModel>> validators,
            ILocalizationService localizationService,
            CustomerSettings customerSettings)
            : base(validators)
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage(localizationService.GetResource("Account.Fields.Email.Required"));
            RuleFor(x => x.Email).EmailAddress().WithMessage(localizationService.GetResource("Common.WrongEmail"));

            RuleFor(x => x.FirstName).NotEmpty().WithMessage(localizationService.GetResource("Account.Fields.FirstName.Required"));
            RuleFor(x => x.LastName).NotEmpty().WithMessage(localizationService.GetResource("Account.Fields.LastName.Required"));

            RuleFor(x => x.Password).NotEmpty().WithMessage(localizationService.GetResource("Account.Fields.Password.Required"))
                .When(subaccount => (string.IsNullOrEmpty(subaccount.Id)) || (!string.IsNullOrEmpty(subaccount.Id) && !string.IsNullOrEmpty(subaccount.Password)));
            RuleFor(x => x.Password).Length(customerSettings.PasswordMinLength, 999).WithMessage(string.Format(localizationService.GetResource("Account.Fields.Password.LengthValidation"), customerSettings.PasswordMinLength))
                .When(subaccount => (string.IsNullOrEmpty(subaccount.Id)) || (!string.IsNullOrEmpty(subaccount.Id) && !string.IsNullOrEmpty(subaccount.Password)));
        }
    }
}