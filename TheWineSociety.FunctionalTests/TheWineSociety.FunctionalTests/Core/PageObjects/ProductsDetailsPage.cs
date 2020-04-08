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
    public class ProductsDetailsPage : CommonMethods
    {
             
        public ProductsDetailsPage()
        {
            PageFactory.InitElements(driver, this);

        }
        private static readonly Logger log = NLog.LogManager.GetLogger("CustomerJourneyPage");

        #region Locators

        [FindsBy(How = How.CssSelector, Using = "#productAccordion .row #producerContent")]
        public IWebElement ExpandProducerProfile { set; get; }

        [FindsBy(How = How.CssSelector, Using = "#productAccordion .row .accordion__header")]
        public IWebElement ProducerProfileHeading { set; get; }

        [FindsBy(How = How.CssSelector, Using = "#productAccordion #collapseProducerContent .h5")]
        public IWebElement ProducerName { set; get; }

        [FindsBy(How = How.CssSelector, Using = "#productAccordion #collapseProducerContent .critic-review__review")]
        public IWebElement ProducerDescription { set; get; }

        [FindsBy(How = How.CssSelector, Using = "#productAccordion .profile__image-container img")]
        public IWebElement ProducerProfileAvatar { set; get; }

        //Product Details
        [FindsBy(How = How.CssSelector, Using = ".product-details span")]
        public IWebElement ProductName { set; get; }

        [FindsBy(How = How.CssSelector, Using = ".product-details .product-details__details p")]
        public IWebElement ProductDescription { set; get; }

        [FindsBy(How = How.CssSelector, Using = ".product-details .product-details__pricing  .product-pricing__price:nth-child(2)")]
        public IWebElement ProductPrice { set; get; }

        [FindsBy(How = How.CssSelector, Using = ".product-details .product-pricing__unit:nth-child(2) .product-pricing__price:nth-child(2)")]
        public IWebElement casePrice12 { set; get; }

        [FindsBy(How = How.CssSelector, Using = ".product-details .product-pricing .product-pricing__unit:nth-child(3) span")]
        public IWebElement casePrice6 { set; get; }
        
        [FindsBy(How = How.CssSelector, Using = ".product-view .row .product-image img")]
        public IWebElement productImage { set; get; }

        //OutOfStock and Alternative Product
        [FindsBy(How = How.CssSelector, Using = ".product-details .product-stock-info span")]
        public IWebElement outOfStockText { set; get; }

        [FindsBy(How = How.CssSelector, Using = ".product-details .add-to-basket a")]
        public IWebElement viewAlternativeWineLink  { set; get; }

        [FindsBy(How = How.CssSelector, Using = ".recommended-product h3")]
        public IWebElement alternativeProductHeading { set; get; }

        [FindsBy(How = How.CssSelector, Using = ".recommended-product .row .product-tile__container .product-tile__description .product-tile__name")]
        public IWebElement alternativeProductName { set; get; }

        [FindsBy(How = How.CssSelector, Using = ".recommended-product .row .product-tile__container .product-tile__description .product-tile__detail")]
        public IWebElement alternativeProductDescription { set; get; }

        [FindsBy(How = How.CssSelector, Using = ".recommended-product .row .product-tile__container .product-tile__image-container .product-image__link")]
        public IWebElement alternativeProductImage { set; get; }


        //Regional Content
        [FindsBy(How = How.CssSelector, Using = "#productAccordion .row #regionalContent")]
        public IWebElement ExpandRegionalProfile { set;get; }

        [FindsBy(How = How.CssSelector, Using = "#productAccordion .row .accordion__item #regionalContent .accordion__title")]
        public IWebElement RegionalProfileHeading { set; get; }

        [FindsBy(How = How.CssSelector, Using = "#productAccordion .row #collapseRegionalContent .accordion__body .h5")]
        public IWebElement RegionalProfile { set; get; }

        [FindsBy(How = How.CssSelector, Using = "#productAccordion .row #collapseRegionalContent .accordion__body .read-more")]
        public IWebElement RegionalProfileDescription { set; get; }

        [FindsBy(How = How.CssSelector, Using = "#productAccordion .row #collapseRegionalContent .profile__image-container img")]
        public IWebElement RegionalProfileAvatar { set; get; }

        #endregion
        public void VerifyProducerDetails(string producerName, string producerDescription)
        {
            ClickElement(ExpandProducerProfile);
            VerifyText(ProducerProfileHeading, "Producer Profile");
            VerifyText(ProducerName, producerName);
            VerifyContainingText(ProducerDescription, producerDescription);
            IsElementExistsLocator(ProducerProfileAvatar, true);
        }

        public void VerifyProductDetails(string productNameExp, string bottlePriceExp, string casePriceFor12Exp, string casePriceFor6Exp, string descriptionExp)
        {
            
            VerifyText(ProductName, productNameExp);
            VerifyContainingText(ProductDescription, descriptionExp);
            VerifyText(ProductPrice,bottlePriceExp);
            VerifyText(casePrice12, casePriceFor12Exp);
            VerifyText(casePrice6, casePriceFor6Exp);         
        }
         
        public void VerifyProductImage()
        {
            IsElementExistsLocator(productImage, true);
        }

        public void VerifyOutOfStockProduct()
        {
            VerifyText(outOfStockText, "Out of stock");
            IsElementExistsLocator(viewAlternativeWineLink, true);
        }

        public void VerifyAlternativeProductDetails(string altProductName, string altProdDescription)
        {
            VerifyText(alternativeProductHeading, "Alternative products");
            VerifyContainingText(alternativeProductName, altProductName);
            VerifyText(alternativeProductDescription, altProdDescription);
            IsElementExistsLocator(alternativeProductImage, true);

        }

        public void VerifyRegionalProfileDetails(string altRegionalProfile, string altRegionalDescription)
        {
            ClickElement(ExpandRegionalProfile);
            VerifyText(RegionalProfileHeading, "Regional Content");
            VerifyText(RegionalProfile, altRegionalProfile);
            VerifyContainingText(RegionalProfileDescription, altRegionalDescription);
            IsElementExistsLocator(RegionalProfileAvatar, true);
        }

    }
}
