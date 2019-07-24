﻿using System;
using System.Text;
using NUnit.Framework;
using System.Linq;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Opencart.Tests
{
    [TestFixture]
    class CurrencyFormatTest
    {
        IWebDriver MyDriver;
        [SetUp]
        public void Setup()
        {
            MyDriver = new ChromeDriver(@"C:\Users\Oleg\Source\Repos\Opencart.Tests\Opencart.Tests\bin\Debug\netcoreapp2.1");
            MyDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            MyDriver.Navigate().GoToUrl(@"http://192.168.17.128/opencart/upload/");
            MyDriver.Manage().Window.Maximize();
            MyDriver.FindElement(By.CssSelector(".owl-wrapper-outer")).Click();
        }

        [TearDown]
        public void TearDown()
        {
            MyDriver.Quit();
        }

        [Test]
        public void CheckPriceIsNumber()
        {
            MyDriver.FindElement(By.CssSelector("button.btn.btn-link.dropdown-toggle")).Click();
            MyDriver.FindElement(By.Name("USD")).Click();
            string Price = MyDriver.FindElements(By.XPath("//div[@id='content']//div[@class='col-sm-4']/ul[count(*)=2]/li"))[0]
                .Text.Replace('.', ',').Substring(1);
            decimal result;
            bool actual = Decimal.TryParse(Price, out result);
            Assert.IsTrue(actual);
        }

        [Test]
        public void CheckPriceIsPositiveNumber()
        {
            MyDriver.FindElement(By.CssSelector("button.btn.btn-link.dropdown-toggle")).Click();
            MyDriver.FindElement(By.Name("USD")).Click();
            string Price = MyDriver.FindElements(By.XPath("//div[@id='content']//div[@class='col-sm-4']/ul[count(*)=2]/li"))[0]
                .Text.Replace('.', ',').Substring(1);
            decimal result = Decimal.Parse(Price);
            Assert.IsTrue(result >= 0);
        }

        [TestCase("USD", @"^\$\d+\.?\d*")]
        [TestCase("EUR", @"\d+\.?\d*€$")]
        [TestCase("GBP", @"^£\d+\.?\d*")]
        public void CheckCurrencyFormat(string currency, string pattern)
        {
            MyDriver.FindElement(By.CssSelector("button.btn.btn-link.dropdown-toggle")).Click();
            MyDriver.FindElement(By.Name(currency)).Click();
            string Price = MyDriver.FindElements(By.XPath("//div[@id='content']//div[@class='col-sm-4']/ul[count(*)=2]/li"))[0].Text;
            Regex Pattern = new Regex(pattern);
            Assert.IsTrue(Pattern.IsMatch(Price));
        }
    }
}
