using PowerStore.Domain.Catalog;
using PowerStore.Domain.Common;
using PowerStore.Domain.Customers;

namespace PowerStore.Services.Tax
{
    /// <summary>
    /// Represents a request for tax calculation
    /// </summary>
    public partial class CalculateTaxRequest
    {
        // <summary>
        /// Gets or sets a product
        /// </summary>
        public Product Product { get; set; }

        /// <summary>
        /// Gets or sets a customer
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// Gets or sets an address
        /// </summary>
        public Address Address { get; set; }

        /// <summary>
        /// Gets or sets a tax category identifier
        /// </summary>
        public string TaxCategoryId { get; set; }

        /// <summary>
        /// Gets or sets a price
        /// </summary>
        public decimal Price { get; set; }
    }
}
