﻿using PowerStore.Core;
using PowerStore.Domain.Messages;
using PowerStore.Services.Commands.Models.Common;
using PowerStore.Services.Messages;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PowerStore.Services.Commands.Handlers.Common
{
    public class InsertContactUsCommandHandler : IRequestHandler<InsertContactUsCommand, bool>
    {
        private readonly IContactUsService _contactUsService;
        private readonly IWebHelper _webHelper;

        public InsertContactUsCommandHandler(
            IContactUsService contactUsService,
            IWebHelper webHelper)
        {
            _contactUsService = contactUsService;
            _webHelper = webHelper;
        }

        public async Task<bool> Handle(InsertContactUsCommand request, CancellationToken cancellationToken)
        {
            var contactus = new ContactUs() {
                CreatedOnUtc = DateTime.UtcNow,
                CustomerId = request.CustomerId,
                StoreId = request.StoreId,
                VendorId = request.VendorId,
                Email = request.Email,
                FullName = request.FullName,
                Subject = request.Subject,
                Enquiry = request.Enquiry,
                EmailAccountId = request.EmailAccountId,
                Attributes = request.ContactAttributes,
                ContactAttributeDescription = request.ContactAttributeDescription,
                IpAddress = _webHelper.GetCurrentIpAddress()
            };
            await _contactUsService.InsertContactUs(contactus);

            return true;
        }
    }
}
