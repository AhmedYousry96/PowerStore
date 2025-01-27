﻿using PowerStore.Domain.Catalog;
using PowerStore.Framework.Components;
using PowerStore.Services.Configuration;
using PowerStore.Services.Stores;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace PowerStore.Web.Areas.Admin.Components
{
    public class CommonMultistoreDisabledWarningViewComponent : BaseAdminViewComponent
    {
        private readonly ISettingService _settingService;
        private readonly IStoreService _storeService;
        private readonly CatalogSettings _catalogSettings;

        public CommonMultistoreDisabledWarningViewComponent(ISettingService settingService, IStoreService storeService, CatalogSettings catalogSettings)
        {
            _settingService = settingService;
            _storeService = storeService;
            _catalogSettings = catalogSettings;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //action displaying notification (warning) to a store owner that "limit per store" feature is ignored
            //default setting
            bool enabled = _catalogSettings.IgnoreStoreLimitations;
            if (!enabled)
            {
                //overridden settings
                var stores = await _storeService.GetAllStores();
                foreach (var store in stores)
                {
                    if (!enabled)
                    {
                        var catalogSettings = _settingService.LoadSetting<CatalogSettings>(store.Id);
                        enabled = catalogSettings.IgnoreStoreLimitations;
                    }
                }
            }

            //This setting is disabled. No warnings.
            if (!enabled)
                return Content("");

            return View();
        }
    }
}