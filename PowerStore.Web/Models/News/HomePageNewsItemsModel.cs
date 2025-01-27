﻿using PowerStore.Core.Models;
using PowerStore.Web.Models.Media;
using System;
using System.Collections.Generic;

namespace PowerStore.Web.Models.News
{
    public partial class HomePageNewsItemsModel : BaseModel
    {
        public HomePageNewsItemsModel()
        {
            NewsItems = new List<NewsItemModel>();
        }

        public IList<NewsItemModel> NewsItems { get; set; }

        public class NewsItemModel : BaseModel
        {
            public NewsItemModel()
            {
                PictureModel = new PictureModel();
            }

            public string Id { get; set; }
            public string SeName { get; set; }
            public string Title { get; set; }
            public PictureModel PictureModel { get; set; }
            public string Short { get; set; }
            public string Full { get; set; }
            public DateTime CreatedOn { get; set; }
        }
    }
}