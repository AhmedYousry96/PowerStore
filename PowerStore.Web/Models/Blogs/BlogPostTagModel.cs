﻿using PowerStore.Core.Models;

namespace PowerStore.Web.Models.Blogs
{
    public partial class BlogPostTagModel : BaseModel
    {
        public string Name { get; set; }

        public int BlogPostCount { get; set; }
    }
}