﻿using PowerStore.Core;
using PowerStore.Domain.Catalog;
using PowerStore.Framework.Components;
using PowerStore.Services.Customers;
using PowerStore.Services.Queries.Models.Catalog;
using PowerStore.Web.Features.Models.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace PowerStore.Web.Components
{
    public class RecommendedProductsViewComponent : BaseViewComponent
    {

        #region Fields

        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IMediator _mediator;
        private readonly CatalogSettings _catalogSettings;

        #endregion

        #region Constructors

        public RecommendedProductsViewComponent(
            IWorkContext workContext,
            IStoreContext storeContext,
            IMediator mediator,
            CatalogSettings catalogSettings)
        {
            _workContext = workContext;
            _storeContext = storeContext;
            _mediator = mediator;
            _catalogSettings = catalogSettings;
        }

        #endregion

        #region Invoker

        public async Task<IViewComponentResult> InvokeAsync(int? productThumbPictureSize)
        {
            if (!_catalogSettings.RecommendedProductsEnabled)
                return Content("");

            var products = await _mediator.Send(new GetRecommendedProductsQuery() { CustomerRoleIds = _workContext.CurrentCustomer.GetCustomerRoleIds(), StoreId = _storeContext.CurrentStore.Id });

            if (!products.Any())
                return Content("");

            var model = await _mediator.Send(new GetProductOverview() {
                PreparePictureModel = true,
                PreparePriceModel = true,
                ProductThumbPictureSize = productThumbPictureSize,
                Products = products,
            });

            return View(model);
        }

        #endregion

    }
}
