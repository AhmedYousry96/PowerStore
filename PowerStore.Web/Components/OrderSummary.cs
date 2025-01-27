﻿using PowerStore.Core;
using PowerStore.Domain.Orders;
using PowerStore.Framework.Components;
using PowerStore.Services.Orders;
using PowerStore.Web.Features.Models.ShoppingCart;
using PowerStore.Web.Models.ShoppingCart;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace PowerStore.Web.ViewComponents
{
    public class OrderSummaryViewComponent : BaseViewComponent
    {
        private readonly IMediator _mediator;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IStoreContext _storeContext;
        private readonly IWorkContext _workContext;
        public OrderSummaryViewComponent(IMediator mediator, IShoppingCartService shoppingCartService, IStoreContext storeContext, IWorkContext workContext)
        {
            _mediator = mediator;
            _shoppingCartService = shoppingCartService;
            _storeContext = storeContext;
            _workContext = workContext;
        }

        public async Task<IViewComponentResult> InvokeAsync(bool? prepareAndDisplayOrderReviewData, ShoppingCartModel overriddenModel)
        {
            //use already prepared (shared) model
            if (overriddenModel != null)
                return View(overriddenModel);

            var cart = _shoppingCartService.GetShoppingCart(_storeContext.CurrentStore.Id, ShoppingCartType.ShoppingCart, ShoppingCartType.Auctions);

            var model = await _mediator.Send(new GetShoppingCart() {
                Cart = cart,
                IsEditable = false,
                PrepareAndDisplayOrderReviewData = prepareAndDisplayOrderReviewData.GetValueOrDefault(),
                Customer = _workContext.CurrentCustomer,
                Currency = _workContext.WorkingCurrency,
                Language = _workContext.WorkingLanguage,
                Store = _storeContext.CurrentStore,
                TaxDisplayType = _workContext.TaxDisplayType
            });

            return View(model);

        }
    }
}
