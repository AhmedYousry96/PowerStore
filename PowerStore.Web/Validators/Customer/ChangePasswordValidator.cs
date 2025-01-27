﻿using FluentValidation;
using PowerStore.Domain.Customers;
using PowerStore.Core.Validators;
using PowerStore.Services.Localization;
using PowerStore.Web.Models.Customer;
using System.Collections.Generic;

namespace PowerStore.Web.Validators.Customer
{
    public class ChangePasswordValidator : BasePowerStoreValidator<ChangePasswordModel>
    {
        public ChangePasswordValidator(
            IEnumerable<IValidatorConsumer<ChangePasswordModel>> validators,
            ILocalizationService localizationService, CustomerSettings customerSettings)
            : base(validators)
        {
            RuleFor(x => x.OldPassword).NotEmpty().WithMessage(localizationService.GetResource("Account.ChangePassword.Fields.OldPassword.Required"));
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage(localizationService.GetResource("Account.ChangePassword.Fields.NewPassword.Required"));
            RuleFor(x => x.NewPassword).Length(customerSettings.PasswordMinLength, 999).WithMessage(string.Format(localizationService.GetResource("Account.ChangePassword.Fields.NewPassword.LengthValidation"), customerSettings.PasswordMinLength));

            if (!string.IsNullOrEmpty(customerSettings.PasswordRegularExpression))
                RuleFor(x => x.NewPassword).Matches(customerSettings.PasswordRegularExpression).WithMessage(string.Format(localizationService.GetResource("Account.ChangePassword.Fields.NewPassword.Validation")));

            RuleFor(x => x.ConfirmNewPassword).NotEmpty().WithMessage(localizationService.GetResource("Account.ChangePassword.Fields.ConfirmNewPassword.Required"));
            RuleFor(x => x.ConfirmNewPassword).Equal(x => x.NewPassword).WithMessage(localizationService.GetResource("Account.ChangePassword.Fields.NewPassword.EnteredPasswordsDoNotMatch"));
        }
    }
}