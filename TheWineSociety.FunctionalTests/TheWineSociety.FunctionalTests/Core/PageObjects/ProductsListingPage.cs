using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using NLog.Fluent;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace TheWineSociety.FunctionalTests.Core.PageObjects
{
    public class ProductsListingPage : CommonMethods
    {
        public ProductsListingPage()
        {
            PageFactory.InitElements(driver, this);

        }
        private static readonly Logger log = NLog.LogManager.GetLogger("CustomerJourneyPage");

        #region Locators

        /// <summary>
        /// Locators 
        /// </summary>

        [FindsBy(How = How.CssSelector, Using = "#filter-search")]
        public IWebElement RefineSearchEditLocator { set; get; }

        [FindsBy(How = How.CssSelector, Using = ".product-listing--isList .product-tile__container .product-tile__description .product-tile__name")]
        public IWebElement ProductTitleTextLocator { set; get; }

        [FindsBy(How = How.CssSelector, Using = ".product-listing--isList .product-tile__container .product-tile__description .product-tile__facets")]
        public IWebElement CountryNameTextLocator { set; get; }

        [FindsBy(How = How.CssSelector, Using = ".product-listing--isList .product-tile__container .product-tile__description .product-tile__detail")]
        public IWebElement ProductDescriptionTextLocator { set; get; }

        [FindsBy(How = How.CssSelector, Using = ".product-listing--isList .product-tile__container .product-tile__description .product-pricing .product-tile__price span")]
        public IWebElement BottlePriceTextLocator { set; get; }

        [FindsBy(How = How.CssSelector, Using = ".product-listing--isList .product-tile__container .product-tile__description .product-pricing .product-tile__price:nth-child(2) span")]
        public IWebElement CasePriceTextLocator { set; get; }

        [FindsBy(How = How.CssSelector, Using = ".product-listing--isList .product-tile__container .product-tile__description .product-pricing .product-tile__price:nth-child(2) span")]
        public IWebElement parentList { set; get; }

        [FindsBy(How = How.CssSelector, Using = ".Price Low - High")]
        public IWebElement sortByPriceLowToHigh { set; get; }

        [FindsBy(How = How.CssSelector, Using = "#sort-control")]
        public IWebElement sortByDropDown { set; get; }

        [FindsBy(How = How.CssSelector, Using = ".product-listing--isList .product-tile__name")]
        public IWebElement ProductLink { set; get; }

        #endregion
        /// <summary>
        /// Search for wines using refine search
        /// </summary>
        /// <param

        public void RefineSearch(string wineName)
        {
            EnterTextInTextField(RefineSearchEditLocator, wineName);
            WaitForPageToLoad();

        }

        /// <summary>
        /// Assertions on refine search functionality
        /// </summary>
        /// <param

        public void VerifyWineDetails(string wineName, string country, string description, string bottlePrice, string casePrice)
        {
            VerifyText(ProductTitleTextLocator, wineName);
            VerifyText(CountryNameTextLocator, country);
            VerifyText(ProductDescriptionTextLocator, description);
            VerifyText(BottlePriceTextLocator, bottlePrice);
            VerifyText(CasePriceTextLocator, casePrice);
        }

        public void selectProduct()
        {
            ClickElement(ProductLink);
        }

        public void SortResultsList(string sortType)
        {

            switch (sortType)
            {
                case "A-Z":
                    SelectDropdownPosition(sortByDropDown, 2);
                    WaitForPageLoad();
                    break;
                case "Low TO High":
                    SelectDropdownPosition(sortByDropDown, 4);
                    WaitForPageLoad();
                    break;
            }

        }

        public List<IWebElement> GetListElemnts()
        {
            WaitForPageToLoad();
            IWebElement products = driver.FindElement(By.CssSelector(".product-listing--isList"));
            List<IWebElement> allProducts = new List<IWebElement>();
            List<IWebElement> productPriceList = new List<IWebElement>();

            allProducts = products.FindElements(By.CssSelector("div.product-tile__container")).ToList();
           
                foreach (var product in allProducts)
                {

                var partialLinkText = product.FindElement(By.CssSelector(".product-tile__price"));
                string oos = partialLinkText.Text;
                if (oos != "Out of stock")
                {

                    if (product.FindElements(By.CssSelector(".product-tile__price--per-bottle:nth-child(1) span")).Count() == 1)

                    {
                        productPriceList.Add(product.FindElement(By.CssSelector(".product-tile__price--per-bottle:nth-child(1) .product-pricing__price")));

                    }
                    else if (product.FindElements(By.CssSelector(".product-tile__price--per-bottle:nth-child(1) span")).Count() == 2)
                    {
                        productPriceList.Add(product.FindElement(By.CssSelector(".product-tile__price--per-bottle:nth-child(1) .product-pricing__price:nth-child(2)")));
                    }
                }
                
            }

            return productPriceList;

        }

        public bool IsTestElementPresent(IWebElement product)
        {
            try
            {
                product.FindElement(By.CssSelector(".product-tile__price--per-bottle:nth-child(1) span2"));
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public void VerifyListItemsLowToHigh()
        {
            List<IWebElement> resultsList = GetListElemnts();
            List<string> item = new List<string>();
            foreach (IWebElement cell in resultsList)
            {
                item.Add(cell.Text.Replace("£", ""));
            }
            List<decimal> listA = item.Select(s =>
            decimal.Parse(s)).ToList();
            List<decimal> listB = listA.OrderBy(x => x).ToList();
            Assert.IsTrue(listA.SequenceEqual(listB));
        }

        public List<IWebElement> GetListElemntsProductName()
        {
            WaitForPageToLoad();
            IWebElement products = driver.FindElement(By.CssSelector(".product-listing--isList"));
            List<IWebElement> allProducts = new List<IWebElement>();
            List<IWebElement> productNamesList = new List<IWebElement>();

            allProducts = products.FindElements(By.CssSelector("div.product-tile__container")).ToList();
            
                foreach (var product in allProducts)
                { 
                        productNamesList.Add(product.FindElement(By.CssSelector(".product-tile__description .product-tile__name")));
                }
            return productNamesList;

        }

        public void VerifyListItemsAToZ()
        {
            List<IWebElement> resultsList = GetListElemntsProductName();
            List<string> item = new List<string>();
            foreach (IWebElement cell in resultsList)
            {
                item.Add(cell.Text);
            }
            List<string> listA = item.Select(s =>
            s).ToList();
            List<string> listB = listA.OrderBy(x => x).ToList();
            Assert.IsTrue(listA.SequenceEqual(listB));
        }
    }
}

