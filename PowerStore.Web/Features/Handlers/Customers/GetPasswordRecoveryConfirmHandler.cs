﻿using PowerStore.Domain.Customers;
using PowerStore.Services.Customers;
using PowerStore.Services.Localization;
using PowerStore.Web.Features.Models.Customers;
using PowerStore.Web.Models.Customer;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace PowerStore.Web.Features.Handlers.Customers
{
    public class GetPasswordRecoveryConfirmHandler : IRequestHandler<GetPasswordRecoveryConfirm, PasswordRecoveryConfirmModel>
    {
        private readonly ILocalizationService _localizationService;
        private readonly CustomerSettings _customerSettings;

        public GetPasswordRecoveryConfirmHandler(
            ILocalizationService localizationService,
            CustomerSettings customerSettings)
        {
            _localizationService = localizationService;
            _customerSettings = customerSettings;
        }

        public async Task<PasswordRecoveryConfirmModel> Handle(GetPasswordRecoveryConfirm request, CancellationToken cancellationToken)
        {
            var model = new PasswordRecoveryConfirmModel();

            //validate token
            if (!(request.Customer.IsPasswordRecoveryTokenValid(request.Token)))
            {
                model.DisablePasswordChanging = true;
                model.Result = _localizationService.GetResource("Account.PasswordRecovery.WrongToken");
            }

            //validate token expiration date
            if (request.Customer.IsPasswordRecoveryLinkExpired(_customerSettings))
            {
                model.DisablePasswordChanging = true;
                model.Result = _localizationService.GetResource("Account.PasswordRecovery.LinkExpired");
            }
            return await Task.FromResult(model);
        }
    }
}
