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
    class CurrencyLabelButtonTest
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

        [TestCase("USD", @"^\$ Currency")]
        [TestCase("EUR", @"^€ Currency")]
        [TestCase("GBP", @"^\£ Currency")]
        public void CheckCurrencyLabel(string currency, string puttern)
        {
            MyDriver.FindElement(By.CssSelector("button.btn.btn-link.dropdown-toggle")).Click();
            MyDriver.FindElement(By.Name(currency)).Click();
            string Actual = MyDriver.FindElement(By.CssSelector("button.btn.btn-link.dropdown-toggle")).Text;
            Regex Puttern = new Regex(puttern);
            Assert.IsTrue(Puttern.IsMatch(Actual));
        }
    }
}
