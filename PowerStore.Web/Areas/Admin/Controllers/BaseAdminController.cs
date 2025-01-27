﻿using PowerStore.Framework.Controllers;
using PowerStore.Framework.Mvc.Filters;
using PowerStore.Web.Areas.Admin.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace PowerStore.Web.Areas.Admin.Controllers
{
    [ValidateIpAddress]
    [AuthorizeAdmin]
    [AutoValidateAntiforgeryToken]
    [Area(Constants.AreaAdmin)]
    [ValidateVendor]
    public abstract partial class BaseAdminController : BaseController
    {
        /// <summary>
        /// Save selected TAB index
        /// </summary>
        /// <param name="index">Idnex to save; null to automatically detect it</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected async Task SaveSelectedTabIndex(int? index = null, bool persistForTheNextRequest = true)
        {
            //keep this method synchronized with
            //"GetSelectedTabIndex" method of \PowerStore.Framework\ViewEngines\Razor\WebViewPage.cs
            if (!index.HasValue)
            {
                int tmp;
                var form = await HttpContext.Request.ReadFormAsync();
                var tabindex = form["selected-tab-index"];
                if (tabindex.Count > 0)
                {
                    if (int.TryParse(tabindex[0], out tmp))
                    {
                        index = tmp;
                    }
                }
                else
                    index = 1;
            }
            if (index.HasValue)
            {
                string dataKey = "PowerStore.selected-tab-index";
                if (persistForTheNextRequest)
                {
                    TempData[dataKey] = index;
                }
                else
                {
                    ViewData[dataKey] = index;
                }
            }
        }
        /// <summary>
        /// Creates a <see cref="T:System.Web.Mvc.JsonResult"/> object that serializes the specified object to JavaScript Object Notation (JSON) format using the content type, content encoding, and the JSON request behavior.
        /// </summary>
        /// 
        /// <returns>
        /// The result object that serializes the specified object to JSON format.
        /// </returns>
        /// <param name="data">The JavaScript object graph to serialize.</param>
        /// <param name="contentType">The content type (MIME type).</param>
        /// <param name="contentEncoding">The content encoding.</param>
        /// <param name="behavior">The JSON request behavior</param>
        public override JsonResult Json(object data)
        {
            var serializerSettings = new JsonSerializerSettings {
                DateFormatHandling = DateFormatHandling.IsoDateFormat 
            };
            return base.Json(data, serializerSettings);
        }
    }
}