﻿using PowerStore.Api.Commands.Models.Catalog;
using PowerStore.Api.DTOs.Catalog;
using PowerStore.Api.Extensions;
using PowerStore.Services.Catalog;
using PowerStore.Services.Localization;
using PowerStore.Services.Logging;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace PowerStore.Api.Commands.Handlers.Catalog
{
    public class UpdateProductAttributeCommandHandler : IRequestHandler<UpdateProductAttributeCommand, ProductAttributeDto>
    {
        private readonly IProductAttributeService _productAttributeService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly ILocalizationService _localizationService;

        public UpdateProductAttributeCommandHandler(
            IProductAttributeService productAttributeService,
            ICustomerActivityService customerActivityService,
            ILocalizationService localizationService)
        {
            _productAttributeService = productAttributeService;
            _customerActivityService = customerActivityService;
            _localizationService = localizationService;
        }

        public async Task<ProductAttributeDto> Handle(UpdateProductAttributeCommand request, CancellationToken cancellationToken)
        {
            var productAttribute = await _productAttributeService.GetProductAttributeById(request.Model.Id);

            productAttribute = request.Model.ToEntity(productAttribute);
            await _productAttributeService.UpdateProductAttribute(productAttribute);

            //activity log
            await _customerActivityService.InsertActivity("EditProductAttribute", productAttribute.Id, 
                _localizationService.GetResource("ActivityLog.EditProductAttribute"), productAttribute.Name);

            return productAttribute.ToModel();
        }
    }
}
