﻿using AutoMapper;
using PowerStore.Domain.News;
using PowerStore.Core.Mapper;
using PowerStore.Services.Seo;
using PowerStore.Web.Areas.Admin.Models.News;
using System.Collections.Generic;
using System.Linq;

namespace PowerStore.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class NewsItemProfile : Profile, IAutoMapperProfile
    {
        public NewsItemProfile()
        {
            CreateMap<NewsItem, NewsItemModel>()
                .ForMember(dest => dest.Locales, mo => mo.Ignore())
                .ForMember(dest => dest.SeName, mo => mo.MapFrom(src => src.GetSeName("", true)))
                .ForMember(dest => dest.Comments, mo => mo.Ignore())
                .ForMember(dest => dest.CreatedOn, mo => mo.Ignore())
                .ForMember(dest => dest.AvailableStores, mo => mo.Ignore())
                .ForMember(dest => dest.SelectedStoreIds, mo => mo.Ignore())
                .ForMember(dest => dest.AvailableCustomerRoles, mo => mo.Ignore())
                .ForMember(dest => dest.SelectedCustomerRoleIds, mo => mo.Ignore());

            CreateMap<NewsItemModel, NewsItem>()
                .ForMember(dest => dest.Locales, mo => mo.Ignore())
                .ForMember(dest => dest.Id, mo => mo.Ignore())
                .ForMember(dest => dest.NewsComments, mo => mo.Ignore())
                .ForMember(dest => dest.CommentCount, mo => mo.Ignore())
                .ForMember(dest => dest.CreatedOnUtc, mo => mo.Ignore())
                .ForMember(dest => dest.Stores, mo => mo.MapFrom(x => x.SelectedStoreIds != null ? x.SelectedStoreIds.ToList() : new List<string>()))
                .ForMember(dest => dest.CustomerRoles, mo => mo.MapFrom(x => x.SelectedCustomerRoleIds != null ? x.SelectedCustomerRoleIds.ToList() : new List<string>()));
        }

        public int Order => 0;
    }
}