﻿using PowerStore.Core.Mapper;
using PowerStore.Domain.Shipping;
using PowerStore.Web.Areas.Admin.Models.Shipping;

namespace PowerStore.Web.Areas.Admin.Extensions
{
    public static class WarehouseMappingExtensions
    {
        public static WarehouseModel ToModel(this Warehouse entity)
        {
            return entity.MapTo<Warehouse, WarehouseModel>();
        }

        public static Warehouse ToEntity(this WarehouseModel model)
        {
            return model.MapTo<WarehouseModel, Warehouse>();
        }

        public static Warehouse ToEntity(this WarehouseModel model, Warehouse destination)
        {
            return model.MapTo(destination);
        }
    }
}