using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using TheWineSociety.FunctionalTests.Core.PageObjects;

namespace TheWineSociety.FunctionalTests.Core.Steps
{
    [Binding]
    public class UserCanBuyWines_Steps : PageInitializer
    {


        private readonly ScenarioContext context;
        PageInitializer pages = new PageInitializer();


        [Given(@"I am on the products listing page")]
        public void GivenIAmOnTheProductsListingPage()
        {
            commonMethods.NavigateToUrl();
        }

        [When(@"I search for wines with '(.*)' using the refine search")]
        public void WhenISearchForWinesWithUsingTheRefineSearch(string wineName)
        {
            pages.productsListingPage.RefineSearch(wineName);
        }

        [Then(@"I see the '(.*)', '(.*)', '(.*)', '(.*)', '(.*)' of the wine")]
        public void ThenISeeTheDetailsofWine(string WineName, string country, string description, string bottlePrice, string bottleCasePrice)
        {
            pages.productsListingPage.VerifyWineDetails(WineName, country, description, bottlePrice, bottleCasePrice);
        }



        [When(@"I '(.*)' the wine list by price Low to High")]
        public void WhenITheWineListByPriceLowToHigh(string sortType)
        {

            pages.productsListingPage.SortResultsList(sortType);
        }

        [When(@"I select a product to see the Regional Content")]  
        [When(@"I select a product to see the product details")]
        [When(@"I select a out of stock product to see the details")]
        [When(@"I select a product to see the producer details")]
        public void WhenISelectAProductToSeeTheProducerDetails()
        {
            pages.productsListingPage.selectProduct();
        }

        [Then(@"I see the '(.*)', '(.*)' and Avatar")]
        public void ThenISeeProducerDetails(string producerName, string producerDescription)
        {
            pages.productsDetailsPage.VerifyProducerDetails(producerName, producerDescription);
        }

        [Then(@"I see the '(.*)', '(.*)' and Avatar of the product")]
        public void ThenISeeTheAndAvatarOfTheProduct(string regionalProfile, string regionalDetails)
        {
            pages.productsDetailsPage.VerifyRegionalProfileDetails(regionalProfile, regionalDetails);
        }


        [Then(@"I see wine listing ordered by price Low to High")]
        public void ThenISeeWineListingOrderedByPriceLowToHigh()
        {
            pages.productsListingPage.VerifyListItemsLowToHigh();
        }

        [Then(@"I see wine listing ordered by A to Z")]
        public void ThenISeeWineListingOrderedByAToZ()
        {
            pages.productsListingPage.VerifyListItemsAToZ();
        }


        [Then(@"I see the '(.*)', '(.*)', '(.*)', '(.*)' and '(.*)'")]
        public void ThenISeeIseeTheProductDetails(string ProductName, string BottlePrice, string CasePriceFor12, string CasePriceFor6, string Description)
        {
            pages.productsDetailsPage.VerifyProductDetails(ProductName, BottlePrice, CasePriceFor12, CasePriceFor6, Description);
        }

        [Then(@"I see the Product Image")]
        public void ThenISeeTheProductImage()
        {
            pages.productsDetailsPage.VerifyProductImage();
        }

        [Then(@"I see out of stock product in the product details page")]
        public void ThenISeeOutOfStockProductInTheProductDetailsPage()
        {
            pages.productsDetailsPage.VerifyOutOfStockProduct();
        }

        [Then(@"I see the Alternative product dispalyed with '(.*)', '(.*)' and Image")]
        public void ThenISeeTheAlternativeProductDispalyedWithAndImage(string productName, string description)
        {
            pages.productsDetailsPage.VerifyAlternativeProductDetails(productName, description);
        }


    }

}
