﻿using PowerStore.Core.Mapper;
using PowerStore.Domain.Knowledgebase;
using PowerStore.Web.Areas.Admin.Models.Knowledgebase;

namespace PowerStore.Web.Areas.Admin.Extensions
{
    public static class KnowledgebaseCategoryMappingExtensions
    {
        public static KnowledgebaseCategory ToEntity(this KnowledgebaseCategoryModel model)
        {
            return model.MapTo<KnowledgebaseCategoryModel, KnowledgebaseCategory>();
        }

        public static KnowledgebaseCategoryModel ToModel(this KnowledgebaseCategory entity)
        {
            return entity.MapTo<KnowledgebaseCategory, KnowledgebaseCategoryModel>();
        }

        public static KnowledgebaseCategory ToEntity(this KnowledgebaseCategoryModel model, KnowledgebaseCategory destination)
        {
            return model.MapTo(destination);
        }

        public static KnowledgebaseArticle ToEntity(this KnowledgebaseArticleModel model)
        {
            return model.MapTo<KnowledgebaseArticleModel, KnowledgebaseArticle>();
        }

        public static KnowledgebaseArticleModel ToModel(this KnowledgebaseArticle entity)
        {
            return entity.MapTo<KnowledgebaseArticle, KnowledgebaseArticleModel>();
        }

        public static KnowledgebaseArticle ToEntity(this KnowledgebaseArticleModel model, KnowledgebaseArticle destination)
        {
            return model.MapTo(destination);
        }
    }
}