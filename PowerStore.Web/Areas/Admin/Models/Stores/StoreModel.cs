﻿using PowerStore.Framework.Localization;
using PowerStore.Core.ModelBinding;
using PowerStore.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace PowerStore.Web.Areas.Admin.Models.Stores
{
    public partial class StoreModel : BaseEntityModel, ILocalizedModel<StoreLocalizedModel>
    {
        public StoreModel()
        {
            Locales = new List<StoreLocalizedModel>();
            AvailableLanguages = new List<SelectListItem>();
            AvailableWarehouses = new List<SelectListItem>();
            AvailableCountries = new List<SelectListItem>();
            AvailableCurrencies = new List<SelectListItem>();
        }

        [PowerStoreResourceDisplayName("Admin.Configuration.Stores.Fields.Name")]
        public string Name { get; set; }

        [PowerStoreResourceDisplayName("Admin.Configuration.Stores.Fields.Shortcut")]
        public string Shortcut { get; set; }

        [PowerStoreResourceDisplayName("Admin.Configuration.Stores.Fields.Url")]
        public string Url { get; set; }

        [PowerStoreResourceDisplayName("Admin.Configuration.Stores.Fields.SslEnabled")]
        public virtual bool SslEnabled { get; set; }

        [PowerStoreResourceDisplayName("Admin.Configuration.Stores.Fields.SecureUrl")]
        public virtual string SecureUrl { get; set; }

        [PowerStoreResourceDisplayName("Admin.Configuration.Stores.Fields.Hosts")]
        public string Hosts { get; set; }

        [PowerStoreResourceDisplayName("Admin.Configuration.Stores.Fields.DefaultAdminTheme")]
        public string DefaultAdminTheme { get; set; }

        [PowerStoreResourceDisplayName("Admin.Configuration.Stores.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [PowerStoreResourceDisplayName("Admin.Configuration.Stores.Fields.CompanyName")]
        public string CompanyName { get; set; }

        [PowerStoreResourceDisplayName("Admin.Configuration.Stores.Fields.CompanyAddress")]
        public string CompanyAddress { get; set; }

        [PowerStoreResourceDisplayName("Admin.Configuration.Stores.Fields.CompanyPhoneNumber")]
        public string CompanyPhoneNumber { get; set; }

        [PowerStoreResourceDisplayName("Admin.Configuration.Stores.Fields.CompanyVat")]
        public string CompanyVat { get; set; }

        [PowerStoreResourceDisplayName("Admin.Configuration.Stores.Fields.CompanyEmail")]
        public string CompanyEmail { get; set; }

        [PowerStoreResourceDisplayName("Admin.Configuration.Stores.Fields.CompanyHours")]
        public string CompanyHours { get; set; }

        public IList<StoreLocalizedModel> Locales { get; set; }
        //default language
        [PowerStoreResourceDisplayName("Admin.Configuration.Stores.Fields.DefaultLanguage")]
        public string DefaultLanguageId { get; set; }
        public IList<SelectListItem> AvailableLanguages { get; set; }

        //default warehouse
        [PowerStoreResourceDisplayName("Admin.Configuration.Stores.Fields.DefaultWarehouse")]
        public string DefaultWarehouseId { get; set; }
        public IList<SelectListItem> AvailableWarehouses { get; set; }

        //default country
        [PowerStoreResourceDisplayName("Admin.Configuration.Stores.Fields.DefaultCountry")]
        public string DefaultCountryId { get; set; }
        public IList<SelectListItem> AvailableCountries { get; set; }

        [PowerStoreResourceDisplayName("Admin.Configuration.Stores.Fields.DefaultCurrency")]
        public string DefaultCurrencyId { get; set; }
        public IList<SelectListItem> AvailableCurrencies { get; set; }
    }

    public partial class StoreLocalizedModel : ILocalizedModelLocal
    {
        public string LanguageId { get; set; }

        [PowerStoreResourceDisplayName("Admin.Configuration.Stores.Fields.Name")]
        public string Name { get; set; }

        [PowerStoreResourceDisplayName("Admin.Configuration.Stores.Fields.Shortcut")]
        public string Shortcut { get; set; }
    }
}