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
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;

namespace TheWineSociety.FunctionalTests.Core
{
    public class CommonMethods
    {
        private static readonly Logger log = NLog.LogManager.GetLogger("CommonMethods");
        private static int stalecounter = 0;

        protected static RemoteWebDriver driver

        {
            get { return ScenarioContext.Current.Get<RemoteWebDriver>(); }
            set { }
        }

     
        public void NavigateToUrl()
        {
            string envFromConfig = ScenarioContext.Current["URL"] as String;
            Log.Info("Navigate into site using: " + envFromConfig);
            driver.Navigate().GoToUrl(envFromConfig);
            WaitForPageToLoad();
        }
              

        public void EnterTextInTextField(IWebElement element, string txt)
        {
            log.Info("Started entering: " + txt);
            WaitForElementToBeVisible(element);
            element.Clear();
            element.SendKeys(txt);
            log.Info("Finished entering: " + txt);
            Thread.Sleep(10000);

        }

        /// <summary>
        /// Wait for an element to be visible using webelement as parameter
        /// </summary>
        /// <param name="element"></param>
        public void WaitForElementToBeVisible(IWebElement element)
        {
            // WaitForPageLoad();
            log.Info("Wait for element to be visible");
            //log.Info("TimeSpan"+TimeSpan.FromMinutes(TimeSpan.MaxValue.Minutes));

            DefaultWait<IWebElement> wait = new DefaultWait<IWebElement>(element)
            {
                Timeout = TimeSpan.FromSeconds(60),
                PollingInterval = TimeSpan.FromSeconds(1)
            };
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));

            Func<IWebElement, bool> WaitFunc = new Func<IWebElement, bool>((IWebElement ele) =>
            {
                try
                {
                    ScrollDownToElement(element);
                    Highlight(element);
                    return true;
                }
                catch
                {
                    return false;
                }
            });
            wait.Until(WaitFunc);
        }

        /// <summary>
        /// wait for page load with implicit wait
        /// </summary>
        public void WaitForPageLoad()
        {
            log.Info("Wait for implicit page to load ");
            Thread.Sleep(10000);


        }

        /// <summary>
        /// Verifies the Element is not on the page
        /// </summary>
        /// <param name="element"></param>
        public Boolean VerifyElementAbsence(IWebElement element)
        {
            WaitForPageLoad();
            log.Info("Start Validating Element Absence");

            Boolean Result = false;

            DefaultWait<IWebElement> wait = new DefaultWait<IWebElement>(element)
            {
                Timeout = TimeSpan.FromSeconds(30),
                PollingInterval = TimeSpan.FromSeconds(1)
            };
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));

            Func<IWebElement, bool> WaitFunc = new Func<IWebElement, bool>((IWebElement ele) =>
            {
                if (element.Displayed)
                {
                    Result = false;
                    return true;
                }
                return false;
            });
            try
            {
                wait.Until(WaitFunc);
            }
            catch
            {
                Result = true;
            }

           
            Assert.AreEqual(Result, true);
            log.Info("End Validating Element Absence");
            return Result;
        }

        public Boolean VerifyElementExistsTest(IWebElement element)
        {
            WaitForElementToBeVisible(element);
            return element.Displayed;
        }

        /// <summary>
        /// Scroll down to element using java script
        /// </summary>
        /// <param name="element"></param>
        public void ScrollDownToElement(IWebElement element)
        {
            try
            {
                var jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript("arguments[0].scrollIntoView(true);", element);
                stalecounter = 0;
            }
            catch (Exception e)
            {
                if (CheckIfStale(e) && stalecounter < 5)
                {
                    stalecounter++;
                    ScrollDownToElement(element);
                }
                else
                {
                    stalecounter = 0;
                    throw e;
                }
            }
        }

        /// <summary>
        /// Jquery Highlight the selected Eelement
        /// </summary>
        /// <param name="element">pass the element details</param>
        public void Highlight(IWebElement element)
        {
            var jsDriver = (IJavaScriptExecutor)driver;
            string highlightJavascript = @"arguments[0].style.cssText = ""border-width: 2px; border-style: solid; border-color: red"";";
            jsDriver.ExecuteScript(highlightJavascript, new object[] { element });
        }

        private bool CheckIfStale(Exception e)
        {
            if (e.Message.Contains("stale element reference"))
            {
                log.Info(e);
                return true;
            }
            else
            {
                try
                {
                    if (e.InnerException.Message.Contains("stale element reference"))
                    {
                        log.Info(e);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    log.Info(ex);
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Click element
        /// </summary>
        /// <param name="element"></param>
        public void ClickElement(IWebElement element)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromMinutes(1));
            try
            {
                WaitForElementToBeVisible(element);
                log.Info("Clicking on: ");
                //if (element.Enabled)
                element.Click();
                log.Info("Finished Clicked ");
                stalecounter = 0;
            }
            catch (Exception e)
            {
                if (CheckIfStale(e) && stalecounter < 5)
                {
                    stalecounter++;
                    ClickElement(element);
                }
                else
                {
                    stalecounter = 0;
                    throw e;
                }
            }
        }

        
        public void VerifyText(IWebElement element, string expectedText)
        {
            log.Info("Started validating text: " + expectedText);
            WaitForElementToBeVisible(element);
            Assert.IsTrue(element.Displayed);
            Assert.AreEqual(expectedText, element.Text, "Actual:" + element.Text + "  Expected:" + expectedText);
            log.Info("End validating text: " + expectedText);
        }

        /// <summary>
        /// Wait for a page to load completely
        /// </summary>
        public void WaitForPageToLoad()
        {
            log.Info("Wait for Javascript page to load ");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromMinutes(1));
            wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));

        }

        /// <summary>
        /// Verify whether element present on the page using By element
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public void IsElementExistsLocator(IWebElement element, Boolean value)
        {        
            log.Info("Start validate element exists by locator");
            WaitForElementToBeVisible(element);
            Assert.AreEqual(value, element.Displayed, "Element Not Exists =" + value);
            log.Info("End validate element exists by locator");

        }

        // <summary>
        /// Verify element contains text on webpage
        /// </summary>
        /// <param name="element"></param>
        /// <param name="expectedText"></param>
        public void VerifyContainingText(IWebElement element, string expectedText)
        {
            log.Info("Started validating containing text: " + expectedText);
            WaitForElementToBeVisible(element);
            Assert.IsTrue(element.Displayed);
            Assert.IsTrue(element.Text.Contains(expectedText), "Actual:" + element.Text + "  Expected:" + expectedText);
            log.Info("End validating containing text: " + expectedText);
        }

        /// <summary>
        /// Wait for an element to be displayed
        /// </summary>
        /// <param name="element"></param>
        public void WaitForAnElementToBeDisplayed(IWebElement element)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromMinutes(1));
            wait.Until(driver => element.Displayed);
            Highlight(element);
        }


        /// <summary>
        /// Select drop down option using webelement and position
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public void SelectDropdownPosition(IWebElement element, int value)
        {
            WaitForAnElementToBeDisplayed(element);
            SelectElement selectElement = new SelectElement(element);
            selectElement.SelectByIndex(value);
        }



    }
}
