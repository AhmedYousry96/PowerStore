﻿using PowerStore.Services.Localization;

namespace PowerStore.Web.Models.Common
{
    public partial class PagerModel
    {
        #region Constructors

        public PagerModel(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        #endregion Constructors

        #region Fields

        private readonly ILocalizationService _localizationService;
        private int individualPagesDisplayedCount;
        private int pageIndex = -2;
        private int pageSize;

        private bool? showFirst;
        private bool? showIndividualPages;
        private bool? showLast;
        private bool? showNext;
        private bool? showPagerItems;
        private bool? showPrevious;
        private bool? showTotalSummary;

        private string firstButtonText;
        private string lastButtonText;
        private string nextButtonText;
        private string previousButtonText;
        private string currentPageText;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the current page index
        /// </summary>
        public int CurrentPage
        {
            get
            {
                return (PageIndex + 1);
            }
        }

        /// <summary>
        /// Gets or sets a count of individual pages to be displayed
        /// </summary>
        public int IndividualPagesDisplayedCount
        {
            get
            {
                if (individualPagesDisplayedCount <= 0)
                    return 5;
                
                return individualPagesDisplayedCount;
            }
            set
            {
                individualPagesDisplayedCount = value;
            }
        }

        /// <summary>
        /// Gets the current page index
        /// </summary>
        public int PageIndex
        {
            get
            {
                if (pageIndex < 0)
                {
                    return 0;
                }
                return pageIndex;
            }
            set
            {
                pageIndex = value;
            }
        }

        /// <summary>
        /// Gets or sets a page size
        /// </summary>
        public int PageSize
        {
            get
            {
                return (pageSize <= 0) ? 10 : pageSize;
            }
            set
            {
                pageSize = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show "first"
        /// </summary>
        public bool ShowFirst
        {
            get
            {
                return showFirst ?? true;
            }
            set
            {
                showFirst = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show "individual pages"
        /// </summary>
        public bool ShowIndividualPages
        {
            get
            {
                return showIndividualPages ?? true;
            }
            set
            {
                showIndividualPages = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show "last"
        /// </summary>
        public bool ShowLast
        {
            get
            {
                return showLast ?? true;
            }
            set
            {
                showLast = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show "next"
        /// </summary>
        public bool ShowNext
        {
            get
            {
                return showNext ?? true;
            }
            set
            {
                showNext = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show pager items
        /// </summary>
        public bool ShowPagerItems
        {
            get
            {
                return showPagerItems ?? true;
            }
            set
            {
                showPagerItems = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show "previous"
        /// </summary>
        public bool ShowPrevious
        {
            get
            {
                return showPrevious ?? true;
            }
            set
            {
                showPrevious = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show "total summary"
        /// </summary>
        public bool ShowTotalSummary
        {
            get
            {
                return showTotalSummary ?? false;
            }
            set
            {
                showTotalSummary = value;
            }
        }

        /// <summary>
        /// Gets a total pages count
        /// </summary>
        public int TotalPages
        {
            get
            {
                if ((TotalRecords == 0) || (PageSize == 0))
                {
                    return 0;
                }
                int num = TotalRecords / PageSize;
                if ((TotalRecords % PageSize) > 0)
                {
                    num++;
                }
                return num;
            }
        }

        /// <summary>
        /// Gets or sets a total records count
        /// </summary>
        public int TotalRecords { get; set; }

        /// <summary>
        /// Gets or sets the first button text
        /// </summary>
        public string FirstButtonText
        {
            get
            {
                return (!string.IsNullOrEmpty(firstButtonText)) ?
                    firstButtonText :
                    _localizationService.GetResource("Pager.First");
            }
            set
            {
                firstButtonText = value;
            }
        }

        /// <summary>
        /// Gets or sets the last button text
        /// </summary>
        public string LastButtonText
        {
            get
            {
                return (!string.IsNullOrEmpty(lastButtonText)) ?
                    lastButtonText :
                    _localizationService.GetResource("Pager.Last");
            }
            set
            {
                lastButtonText = value;
            }
        }

        /// <summary>
        /// Gets or sets the next button text
        /// </summary>
        public string NextButtonText
        {
            get
            {
                return (!string.IsNullOrEmpty(nextButtonText)) ?
                    nextButtonText :
                    _localizationService.GetResource("Pager.Next");
            }
            set
            {
                nextButtonText = value;
            }
        }

        /// <summary>
        /// Gets or sets the previous button text
        /// </summary>
        public string PreviousButtonText
        {
            get
            {
                return (!string.IsNullOrEmpty(previousButtonText)) ?
                    previousButtonText :
                    _localizationService.GetResource("Pager.Previous");
            }
            set
            {
                previousButtonText = value;
            }
        }

        /// <summary>
        /// Gets or sets the current page text
        /// </summary>
        public string CurrentPageText
        {
            get
            {
                return (!string.IsNullOrEmpty(currentPageText)) ?
                    currentPageText :
                    _localizationService.GetResource("Pager.CurrentPage");
            }
            set
            {
                currentPageText = value;
            }
        }

        /// <summary>
        /// Gets or sets the route name or action name
        /// </summary>
        public string RouteActionName { get; set; }

        /// <summary>
        /// Gets or sets whether the links are created using RouteLink instead of Action Link 
        /// (for additional route values such as slugs or page numbers)
        /// </summary>
        public bool UseRouteLinks { get; set; }

        /// <summary>
        /// Gets or sets the RouteValues object. Allows for custom route values other than page.
        /// </summary>
        public IRouteValues RouteValues { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets first individual page index
        /// </summary>
        /// <returns>Page index</returns>
        public int GetFirstIndividualPageIndex()
        {
            if ((TotalPages < IndividualPagesDisplayedCount) ||
                ((PageIndex - (IndividualPagesDisplayedCount / 2)) < 0))
            {
                return 0;
            }
            if ((PageIndex + (IndividualPagesDisplayedCount / 2)) >= TotalPages)
            {
                return (TotalPages - IndividualPagesDisplayedCount);
            }
            return (PageIndex - (IndividualPagesDisplayedCount / 2));
        }

        /// <summary>
        /// Get last individual page index
        /// </summary>
        /// <returns>Page index</returns>
        public int GetLastIndividualPageIndex()
        {
            int num = IndividualPagesDisplayedCount / 2;
            if ((IndividualPagesDisplayedCount % 2) == 0)
            {
                num--;
            }
            if ((TotalPages < IndividualPagesDisplayedCount) ||
                ((PageIndex + num) >= TotalPages))
            {
                return (TotalPages - 1);
            }
            if ((PageIndex - (IndividualPagesDisplayedCount / 2)) < 0)
            {
                return (IndividualPagesDisplayedCount - 1);
            }
            return (PageIndex + num);
        }

        #endregion Methods
    }

    #region Classes

    /// <summary>
    /// Interface for custom RouteValues objects
    /// </summary>
    public interface IRouteValues
    {
        int pageNumber { get; set; }
    }

    /// Class that has only page for route value. Used for (My Account) Back in stock subscriptions pagination
    /// </summary>
    public partial class BackInStockSubscriptionsRouteValues : IRouteValues
    {
        public int pageNumber { get; set; }
    }

    #endregion Classes
}