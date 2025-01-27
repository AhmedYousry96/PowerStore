﻿using PowerStore.Core;
using PowerStore.Core.Configuration;
using PowerStore.Domain.Localization;
using PowerStore.Services.Localization;
using PowerStore.Web.Features.Models.Common;
using MediatR;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PowerStore.Web.Features.Handlers.Common
{
    public class GetRobotsTextFileHandler : IRequestHandler<GetRobotsTextFile, string>
    {

        private readonly IStoreContext _storeContext;
        private readonly ILanguageService _languageService;
        private readonly IWebHelper _webHelper;

        private readonly PowerStoreConfig _config;

        public GetRobotsTextFileHandler(IStoreContext storeContext,
            ILanguageService languageService,
            IWebHelper webHelper,
            PowerStoreConfig config)
        {
            _storeContext = storeContext;
            _languageService = languageService;
            _webHelper = webHelper;
            _config = config;
        }

        public async Task<string> Handle(GetRobotsTextFile request, CancellationToken cancellationToken)
        {
            return await PrepareRobotsTextFile();
        }

        private async Task<string> PrepareRobotsTextFile()
        {
            var sb = new StringBuilder();

            //if robots.txt exists, let's use it
            string robotsFile = System.IO.Path.Combine(CommonHelper.WebRootPath, "robots.custom.txt");
            if (System.IO.File.Exists(robotsFile))
            {
                //the robots.txt file exists
                string robotsFileContent = System.IO.File.ReadAllText(robotsFile);
                sb.Append(robotsFileContent);
            }
            else
            {
                //doesn't exist. Let's generate it (default behavior)

                var disallowPaths = new List<string>
                {
                    "/admin",
                    "/bin/",
                    "/content/files/",
                    "/content/files/exportimport/",
                    "/country/getstatesbycountryid",
                    "/install",
                    "/upgrade",
                    "/setproductreviewhelpfulness",
                };
                var localizableDisallowPaths = new List<string>
                {
                    "/addproducttocart/catalog/",
                    "/addproducttocart/details/",
                    "/backinstocksubscriptions/manage",
                    "/cart",
                    "/changelanguage/*",
                    "/checkout",
                    "/checkout/billingaddress",
                    "/checkout/completed",
                    "/checkout/confirm",
                    "/checkout/shippingaddress",
                    "/checkout/shippingmethod",
                    "/checkout/paymentinfo",
                    "/checkout/paymentmethod",
                    "/clearcomparelist",
                    "/compareproducts",
                    "/compareproducts/add/*",
                    "/customer/activation",
                    "/customer/addresses",
                    "/customer/changepassword",
                    "/customer/checkusernameavailability",
                    "/customer/downloadableproducts",
                    "/customer/info",
                    "/customer/auctions",
                    "/common/customeractioneventurl",
                    "/common/getactivepopup",
                    "/common/removepopup",
                    "/deletepm",
                    "/emailwishlist",
                    "/login/*",
                    "/newsletter/subscriptionactivation",
                    "/onepagecheckout",
                    "/order/history",
                    "/orderdetails",
                    "/passwordrecovery/confirm",
                    "/poll/vote",
                    "/popupinteractiveform",
                    "/register/*",
                    "/returnrequest",
                    "/returnrequest/history",
                    "/rewardpoints/history",
                    "/search?",
                    "/shoppingcart/*",
                    "/storeclosed",
                    "/subscribenewsletter",
                    "/subscribenewsletter/SaveCategories",
                    "/topic/authenticate",
                    "/uploadfileproductattribute",
                    "/uploadfilecheckoutattribute",
                    "/wishlist",
                };


                const string newLine = "\r\n"; //Environment.NewLine
                sb.Append("User-agent: *");
                sb.Append(newLine);
                //sitemaps
                if (_config.SeoFriendlyUrlsForLanguagesEnabled)
                {
                    //URLs are localizable. Append SEO code
                    foreach (var language in await _languageService.GetAllLanguages(storeId: _storeContext.CurrentStore.Id))
                    {
                        sb.AppendFormat("Sitemap: {0}{1}/sitemap.xml", _storeContext.CurrentStore.Url, language.UniqueSeoCode);
                        sb.Append(newLine);
                    }
                }
                else
                {
                    //localizable paths (without SEO code)
                    sb.AppendFormat("Sitemap: {0}sitemap.xml", _storeContext.CurrentStore.Url);
                    sb.Append(newLine);
                }
                //host
                sb.AppendFormat("Host: {0}", _webHelper.GetStoreLocation());
                sb.Append(newLine);

                //usual paths
                foreach (var path in disallowPaths)
                {
                    sb.AppendFormat("Disallow: {0}", path);
                    sb.Append(newLine);
                }
                //localizable paths (without SEO code)
                foreach (var path in localizableDisallowPaths)
                {
                    sb.AppendFormat("Disallow: {0}", path);
                    sb.Append(newLine);
                }
                if (_config.SeoFriendlyUrlsForLanguagesEnabled)
                {
                    //URLs are localizable. Append SEO code
                    foreach (var language in await _languageService.GetAllLanguages(storeId: _storeContext.CurrentStore.Id))
                    {
                        foreach (var path in localizableDisallowPaths)
                        {
                            sb.AppendFormat("Disallow: /{0}{1}", language.UniqueSeoCode, path);
                            sb.Append(newLine);
                        }
                    }
                }

                //load and add robots.txt additions to the end of file.
                string robotsAdditionsFile = System.IO.Path.Combine(CommonHelper.WebRootPath, "robots.additions.txt");
                if (System.IO.File.Exists(robotsAdditionsFile))
                {
                    string robotsFileContent = System.IO.File.ReadAllText(robotsAdditionsFile);
                    sb.Append(robotsFileContent);
                }

            }
            return sb.ToString();
        }

    }
}
