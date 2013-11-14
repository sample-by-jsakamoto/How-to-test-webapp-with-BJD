using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using TechTalk.SpecFlow;

namespace BehaviorTest
{
    [Binding]
    public class StepDefinition1
    {
        const string _BJDWebAPIEndPoint = "http://127.0.0.1:5050/mail/";

        [Given("There is no mails received")]
        public void GivenThereIsNoMailsReceived()
        {
            var url = _BJDWebAPIEndPoint + "message";
            try
            {
                new WebClient().UploadData(url, "DELETE", new byte[0]);
            }
            catch (WebException e)
            {
                if (e.Message.StartsWith("The server committed a protocol violation.") == false) throw;
            }
        }

        [When("Open URL (.+)")]
        public void OpenURL(string url)
        {
            this.WebDriver.Navigate().GoToUrl(url);
        }

        [When("Enter text \"(.+)\" into (.+)")]
        public void EnterTextInto(string text, string id)
        {
            var element = this.WebDriver.FindElementById(id.TrimStart('#'));
            element.Clear();
            element.Click();
            element.SendKeys(text);
        }

        [When("Click (.+)")]
        public void Click(string id)
        {
            var element = this.WebDriver.FindElementById(id.TrimStart('#'));
            element.Click();
        }

        [Then("The text \"(.+)\" is present at (.+)")]
        public void TheTextIsPresentAt(string text, string id)
        {
            var element = this.WebDriver.FindElementById(id.TrimStart('#'));
            Assert.AreEqual(text, element.Text);
        }

        public class MailMsg
        {
            public string From { get; set; }

            public string To { get; set; }
            
            public string Subject { get; set; }
            
            public string Body { get; set; }
        }

        public class ApiMsgPack
        {
            public IEnumerable<MailMsg> Data { get; set; }
        }

        [Then("One mail received as bellow")]
        public void OneMailReceivedAsBellow(string body, Table expected)
        {
            var url = _BJDWebAPIEndPoint + "message";
            var jsonString = default(string);
            try
            {
                jsonString = new WebClient().DownloadString(url);
            }
            catch (WebException e)
            {
                if (e.Message.StartsWith("The server committed a protocol violation.") == false) throw;
            }
            //ToDo: var apiMsgPack = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiMsgPack>(jsonString);
        }

        public RemoteWebDriver WebDriver
        {
            get
            {
                var webDriver = default(RemoteWebDriver);
                var featureContext = FeatureContext.Current;
                if (featureContext.TryGetValue<RemoteWebDriver>("WebDriver", out webDriver) == false)
                {
                    //webDriver = new InternetExplorerDriver();
                    webDriver = new ChromeDriver();
                    webDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(3));
                    featureContext.Add("WebDriver", webDriver);
                }
                return webDriver;
            }
        }

        [AfterFeature]
        public static void OnAfterFeature()
        {
            var webDriver = default(RemoteWebDriver);
            var featureContext = FeatureContext.Current;
            if (featureContext.TryGetValue<RemoteWebDriver>("WebDriver", out webDriver) == true)
            {
                featureContext.Remove("WebDriver");
                webDriver.Close();
                webDriver.Quit();
            }
        }
    }
}
