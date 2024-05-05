﻿using AutoMapper;
using PowerStore.Api.DTOs.Catalog;
using PowerStore.Domain.Catalog;
using PowerStore.Core.Mapper;

namespace PowerStore.Api.Infrastructure.Mapper
{
    public class CategoryProfile : Profile, IAutoMapperProfile
    {

        public CategoryProfile()
        {
            CreateMap<CategoryDto, Category>()
                .ForMember(dest => dest.SubjectToAcl, mo => mo.Ignore())
                .ForMember(dest => dest.CustomerRoles, mo => mo.Ignore())
                .ForMember(dest => dest.LimitedToStores, mo => mo.Ignore())
                .ForMember(dest => dest.Stores, mo => mo.Ignore())
                .ForMember(dest => dest.CreatedOnUtc, mo => mo.Ignore())
                .ForMember(dest => dest.UpdatedOnUtc, mo => mo.Ignore())
                .ForMember(dest => dest.Locales, mo => mo.Ignore())
                .ForMember(dest => dest.AppliedDiscounts, mo => mo.Ignore())
                .ForMember(dest => dest.GenericAttributes, mo => mo.Ignore());

            CreateMap<Category, CategoryDto>();

        }

        public int Order => 1;
    }
}
