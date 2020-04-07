using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OpenQA.Selenium.Remote;
using TechTalk.SpecFlow;
using NLog;
using System.IO;
using OpenQA.Selenium.Chrome;
using System.Diagnostics;
using System.CodeDom.Compiler;
using NUnit.Framework;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System.Configuration;
using OpenQA.Selenium;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Gherkin.Model;
using System.Reflection;
using System.Collections.Specialized;

namespace TheWineSociety.FunctionalTests.Core
{
    [GeneratedCode("SpecFlow", "3.1.86")]
    [SetUpFixture]
    [Binding]
    public class Hooks
    {

        private static readonly Logger log = LogManager.GetLogger("Hooks");
        public static RemoteWebDriver Driver = null;
        public string Env { get; set; } 
        public string browser { get; set; }
        public string executionMode { get; set; }
        public string tags { get; set; }
        FileReader fileReader = new FileReader();
        private static Boolean takeScreenshot = true;
        FirefoxProfile profile = new FirefoxProfile();
       
        //Extent Report Variables
        private static ExtentTest featureName;
        private static ExtentTest scenario;
        private static ExtentReports extent;

        public void ContextSetup()
        {

        }
        /// <summary>
        /// Hooks Construction with globalsetting parameter
        /// </summary>
        public Hooks()
        {            
            try
            {
                 Env = ConfigurationManager.AppSettings["environment"];
                 browser = ConfigurationManager.AppSettings["browser"].ToLower();

            }
            catch (NullReferenceException)
            {

                log.Info("RunSetting file is not used so defualt to run on SSU4");

            }
            setEnvironment(Env);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="environment">DEV</param>
        public void setEnvironment(string environment)
        {
            string path = fileReader.readFile(environment);
            // Open the file to read from.
            string writeToFile = "";
            foreach (String item in File.ReadLines(path))
            {
                //log.Info(item);
                String[] itemValue = item.Split('=');

                writeToFile = writeToFile + itemValue[1].Replace("http://", "").Replace("https://", "").Replace("/", "") + ",";
                //  setStandardProperties(itemValue[0], itemValue[1]);
               
                ScenarioContext.Current.Add("URL", itemValue[1]);
                ScenarioContext.Current.Add(itemValue[0], itemValue[1]);
            }

            Env = environment;
            ScenarioContext.Current.Add("Env", Env);
            ScenarioContext.Current.Add("Browser", browser);
            ScenarioContext.Current.Add("Tag", tags);
          
        }

        /// <summary>
        /// Webdriver open the Browser
        /// </summary>
        public RemoteWebDriver OpenBrowser()
        {
            switch (browser)
            {
                case "firefox":
                    try
                    {

                        string driverpath = fileReader.getCurrentDriverPath();
                        System.Environment.SetEnvironmentVariable("webdriver.gecko.driver", Environment.CurrentDirectory + "\\geckodriver.exe");
                        FirefoxOptions firefoxOptions = new FirefoxOptions();
                        profile.AcceptUntrustedCertificates = true;
                        firefoxOptions.Profile = profile;
                        firefoxOptions.SetPreference("javascript.enabled", true);
                        Driver = new FirefoxDriver(firefoxOptions);
                        Driver.Manage().Cookies.DeleteAllCookies();
                        Driver.Navigate().Refresh();
                        Driver.Manage().Window.Maximize();
                        //Driver.Manage().Window.Size = new System.Drawing.Size(1280, 900);

                    }
                    catch (Exception e)
                    {
                        log.Info(e);
                    }
                    break;

                case "chrome":
                    try
                    {
                        KillChromeDriver();
                        Environment.CurrentDirectory = fileReader.getCurrentDriverPath();
                        //string path = fileReader.getCurrentDriverPath();
                        log.Info("Driver Path" + Environment.CurrentDirectory);
                        ChromeOptions Options = new ChromeOptions();
                        //Options.AddArgument("--start-maximized");
                        Options.AddArgument("no-sandbox");
                        Driver = new ChromeDriver(Environment.CurrentDirectory, Options, TimeSpan.FromMinutes(1));
                        Driver.Manage().Window.Maximize();
                       // Driver.Manage().Window.Size = new System.Drawing.Size(1280, 900);
                    }
                    catch (Exception e)
                    {

                        log.Info(e);
                    }
                    break;
                    //default:
                    //    break;
            }
            log.Info("Browser Open");           
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMinutes(1);
            Driver.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(1);
            return Driver;
        }
        /// <summary>
        /// Kill any existing Chrome Driver process on the system.
        /// </summary>
        private static void KillChromeDriver()
        {
            Process[] chromeDriverProcesses = Process.GetProcessesByName("chromedriver");
            foreach (var chromeDriverProcess in chromeDriverProcesses)
            {
                chromeDriverProcess.Kill();
            }
        }

        [BeforeTestRun]
        public static void InitializeReport()
        {           
            var fileReader = new FileReader();
            string path = fileReader.getProjectRootPath() +@"\Reports\ExtentReport.html";
            var htmlReporter = new ExtentHtmlReporter(path);
            htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Dark;
            htmlReporter.Config.DocumentTitle = "The Wine Society End To End Test Report";
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
        }

        [AfterTestRun]
        public static void TearDownReport()
        {
            if (ConfigurationManager.AppSettings.Get("executionMode").ToLower() == "local")
            {
                extent.Flush();
            }
        }

        [BeforeFeature]

        public static void BeforeFeature()
        {
            featureName = extent.CreateTest<Feature>(FeatureContext.Current.FeatureInfo.Title);

        }

        [AfterStep]
        public static void InsertReportingSteps()
        {
            if (ConfigurationManager.AppSettings.Get("executionMode").ToLower() == "local")
            {
                var stepType = ScenarioStepContext.Current.StepInfo.StepDefinitionType.ToString();
                var Given = "<b>" + "Given: " + "</b>";
                var When = "<b>" + "When: " + "</b>";
                var Then = "<b>" + "Then: " + "</b>";
                var And = "<b> " + "And: " + "</b>";

                PropertyInfo PInfo = typeof(ScenarioContext).GetProperty("ScenarioExecutionStatus", BindingFlags.Instance | BindingFlags.Public);
                MethodInfo Getter = PInfo.GetGetMethod(nonPublic: true);
                Object TestResult = Getter.Invoke(ScenarioContext.Current, null);

                if (ScenarioContext.Current.TestError == null)
                {
                    if (stepType == "Given")
                        scenario.CreateNode<Given>(Given + ScenarioStepContext.Current.StepInfo.Text);
                    else if (stepType == "When")
                        scenario.CreateNode<When>(When + ScenarioStepContext.Current.StepInfo.Text);
                    else if (stepType == "Then")
                        scenario.CreateNode<Then>(Then + ScenarioStepContext.Current.StepInfo.Text);
                    else if (stepType == "And")
                        scenario.CreateNode<And>(And + ScenarioStepContext.Current.StepInfo.Text);
                }

                //Error State
                else if (ScenarioContext.Current.TestError != null)
                {
                    if (stepType == "Given")
                        scenario.CreateNode<Given>(Given + ScenarioStepContext.Current.StepInfo.Text).Fail(ScenarioContext.Current.TestError.Message);
                    else if (stepType == "When")
                        scenario.CreateNode<When>(When + ScenarioStepContext.Current.StepInfo.Text).Fail(ScenarioContext.Current.TestError.Message);
                    else if (stepType == "Then")
                        scenario.CreateNode<Then>(Then + ScenarioStepContext.Current.StepInfo.Text).Fail(ScenarioContext.Current.TestError.Message);
                    else if (stepType == "And")
                        scenario.CreateNode<And>(And + ScenarioStepContext.Current.StepInfo.Text).Fail(ScenarioContext.Current.TestError.Message);
                }

                //Pending Status
                if (TestResult.ToString() == "StepDefinitionPending")
                {
                    if (stepType == "Given")
                        scenario.CreateNode<Given>(Given + ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");
                    else if (stepType == "When")
                        scenario.CreateNode<When>(When + ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");
                    else if (stepType == "Then")
                        scenario.CreateNode<Then>(Then + ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");
                    else if (stepType == "And")
                        scenario.CreateNode<Then>(And + ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");

                }
            }
        }

        public void CaptureScreenShot(string title)
        {
            log.Info("TakeScreenshot: " + takeScreenshot);
                     
            if (takeScreenshot)
            {
                try
                {
                    Screenshot ss = ((ITakesScreenshot)Driver).GetScreenshot();
                    string path = fileReader.getScreenShotsPath() + title+".png";
                    ss.SaveAsFile(path, ScreenshotImageFormat.Png); //use any of the built in image formating
         
                }
                catch (Exception ex)
                {

                    log.Info(ex);
                }
            }
        }

        /// <summary>
        /// Before Scenario
        /// </summary>
        [BeforeScenario]
        public void Before()
        {          
                                  
            executionMode = ConfigurationManager.AppSettings["executionMode"].ToLower();
            //Browser Stack Execution 
            if(executionMode != "local")
            {
           
                Driver = BrowserStackDriver();
                ScenarioContext.Current.Set(Driver);

            }
            else
            {
             //Local execution
                Driver = Driver ?? OpenBrowser();
                log.Info("Before Test Run");
                log.Info("====================Scenario :- " + ScenarioContext.Current.ScenarioInfo.Title + "=====================");

                ScenarioContext.Current.Set(Driver);
                scenario = featureName.CreateNode<Feature>(ScenarioContext.Current.ScenarioInfo.Title);

            }

        }

        /// <summary>
        /// After Scenario
        /// </summary>
        [AfterScenario]
        public void After()
        
        {
            
            try
            {


                if (ScenarioContext.Current.TestError != null)
                {
                    log.Info("====================Failed: " + ScenarioContext.Current.ScenarioInfo.Title + "=====================");
                                     
                    CaptureScreenShot(ScenarioContext.Current.ScenarioInfo.Title);
                }

                if (ScenarioContext.Current.TestError == null)
                {
                    log.Info("====================Passed - " + ScenarioContext.Current.ScenarioInfo.Title + "=====================");
                                       
                }

            }
            finally
            {                
                profile.Clean();
                ScenarioContext.Current.Clear();
                Driver.Dispose();
                Driver.Quit();
                Driver = null;
                log.Info("Browser Closed Completely after test run");
            }
        }
        
        public RemoteWebDriver BrowserStackDriver()
        {

            string bsUsernName = ConfigurationManager.AppSettings.Get("bsUserName");
            string bsPassword = ConfigurationManager.AppSettings.Get("bsPassword");
            string bsBrowser = ConfigurationManager.AppSettings.Get("bsBrowser");
            RemoteWebDriver driver;
            NameValueCollection caps = ConfigurationManager.GetSection("capabilities/" + "single") as NameValueCollection;
            NameValueCollection settings = ConfigurationManager.GetSection("environments/" + bsBrowser) as NameValueCollection;
            DesiredCapabilities capability = new DesiredCapabilities();

            capability.SetCapability("browserName", bsBrowser);
            capability.SetCapability("browserstack.user", bsUsernName);
            capability.SetCapability("browserstack.key", bsPassword);
           
            driver = new RemoteWebDriver(new Uri("http://" + ConfigurationManager.AppSettings.Get("server") + "/wd/hub/"), capability);
            return driver;
        }
    }
      
    }



