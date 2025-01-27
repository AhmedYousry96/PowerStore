﻿using PowerStore.Core.Mapper;
using PowerStore.Domain.Stores;
using PowerStore.Web.Areas.Admin.Models.Stores;

namespace PowerStore.Web.Areas.Admin.Extensions
{
    public static class StoreMappingExtensions
    {
        public static StoreModel ToModel(this Store entity)
        {
            return entity.MapTo<Store, StoreModel>();
        }

        public static Store ToEntity(this StoreModel model)
        {
            return model.MapTo<StoreModel, Store>();
        }

        public static Store ToEntity(this StoreModel model, Store destination)
        {
            return model.MapTo(destination);
        }
    }
}