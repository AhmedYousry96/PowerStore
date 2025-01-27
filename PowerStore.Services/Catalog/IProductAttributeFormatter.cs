using PowerStore.Domain.Catalog;
using PowerStore.Domain.Common;
using PowerStore.Domain.Customers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PowerStore.Services.Catalog
{
    /// <summary>
    /// Product attribute formatter interface
    /// </summary>
    public partial interface IProductAttributeFormatter
    {
        /// <summary>
        /// Formats attributes
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="customAttributes">Attributes</param>
        /// <returns>Attributes</returns>
        Task<string> FormatAttributes(Product product, IList<CustomAttribute> customAttributes);

        /// <summary>
        /// Formats attributes
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="customAttributes">Attributes</param>
        /// <param name="customer">Customer</param>
        /// <param name="serapator">Serapator</param>
        /// <param name="htmlEncode">A value indicating whether to encode (HTML) values</param>
        /// <param name="renderPrices">A value indicating whether to render prices</param>
        /// <param name="renderProductAttributes">A value indicating whether to render product attributes</param>
        /// <param name="renderGiftCardAttributes">A value indicating whether to render gift card attributes</param>
        /// <param name="allowHyperlinks">A value indicating whether to HTML hyperink tags could be rendered (if required)</param>
        /// <returns>Attributes</returns>
        Task<string> FormatAttributes(Product product, IList<CustomAttribute> customAttributes,
            Customer customer, string serapator = "<br />", bool htmlEncode = true, bool renderPrices = true,
            bool renderProductAttributes = true, bool renderGiftCardAttributes = true,
            bool allowHyperlinks = true, bool showInAdmin = false);
    }
}
