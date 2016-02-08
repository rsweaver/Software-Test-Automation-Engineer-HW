
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using System.Threading;
using OpenQA.Selenium;

namespace Software_Test_Automation_Engineer_HW
{
    [TestClass]
    public class UnitTest
    {
        WebDriver Driver = null;
        string url = "http://adam.goucher.ca/parkcalc/index.php";
        SelectElement ParkingLot = null;
        IWebElement EntryTime = null;
        IWebElement ExitTime = null;
        IWebElement EntryDate = null;
        IWebElement ExitDate = null;
        IWebElement EntryCalendar = null;
        IWebElement ExitCalendar = null;
        IWebElement EntryPM = null;
        IWebElement ExitPM = null;
        IWebElement Calculate = null;


        [TestInitialize]
        public void TestSetup()
        {
            //Replace path with chromedriver.exe path
            Driver = new WebDriver(@"C:\Users\Rebecca\Documents\Visual Studio 2015\Projects\Software Test Automation Engineer HW");
            Driver.Navigate().GoToUrl(url);
            Thread.Sleep(2000);

            ParkingLot = new SelectElement(Driver.FindElementById("Lot"));
            EntryTime = Driver.FindElementById("EntryTime");
            ExitTime = Driver.FindElementById("ExitTime");
            EntryDate = Driver.FindElementById("EntryDate");
            ExitDate = Driver.FindElementById("ExitDate");
            EntryCalendar = Driver.FindElementByXPath("/html/body/form/table/tbody/tr[2]/td[2]/font/a/img");
            ExitCalendar = Driver.FindElementByXPath("/html/body/form/table/tbody/tr[3]/td[2]/font/a/img");
            EntryPM = Driver.FindElementByXPath("/html/body/form/table/tbody/tr[2]/td[2]/font/input[3]");
            ExitPM = Driver.FindElementByXPath("/html/body/form/table/tbody/tr[3]/td[2]/font/input[3]");
            Calculate = Driver.FindElementByXPath("/html/body/form/input[2]");

        }

        [TestCleanup]
        public void Cleanup()
        {
            this.Driver.Quit();
        }

        [TestMethod]
        public void CheckParkCostandTimeCase1()
        {

            // Select Short-term Parking from Lot drop down menu
            ParkingLot.SelectByValue("STP");

            // Enter 10:00 PM in Entry Time, Enter date as 01/01/2014 In Entry Date
            EntryTime.Clear();
            EntryTime.SendKeys("10:00");

            EntryPM.Click();

            EntryDate.Clear();
            EntryDate.SendKeys("01/01/2014");

            // Enter 11:00 PM in Exit Time, Enter date as 01/01/2014 In Exit Date
            ExitTime.Clear();
            ExitTime.SendKeys("11:00");

            ExitPM.Click();

            ExitDate.Clear();
            ExitDate.SendKeys("01/01/2014");

            //Click calculate button, Check cost is equal to $ 2.00, Check duration of stay is (0 Days, 1 Hours, 0 Minutes)
            Calculate.Click();
            Thread.Sleep(2000);

            try
            {
                Assert.AreEqual("$ 2.00", Driver.FindElementByXPath("/html/body/form/table/tbody/tr[4]/td[2]/span[1]/font/b").Text);
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }

            try
            {
                Assert.AreEqual("(0 Days, 1 Hours, 0 Minutes)", Driver.FindElementByXPath("/html/body/form/table/tbody/tr[4]/td[2]/span[2]/font/b").Text.Trim());
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }


        }

        [TestMethod]
        public void CheckParkCostandTimeCase2()
        {


            String OriginalHandle = Driver.CurrentWindowHandle;

            // Select Long Term Surface Parking from Lot drop down menu
            ParkingLot.SelectByValue("LTS");

            //Click calendar icon to enter entry date
            EntryCalendar.Click();
            Thread.Sleep(2000);

            Driver.SwitchTo().Window(Driver.FindCalendarHandle(OriginalHandle));


            //Click the year descend button twice to get the year to be 2014 for entry date, click 1 for 01/01/2014
            Driver.FindElementByXPath("/html/body/form/table/tbody/tr[1]/td/table/tbody/tr/td[2]/a[1]").Click();
            Driver.FindElementByXPath("/html/body/form/table/tbody/tr[1]/td/table/tbody/tr/td[2]/a[1]").Click();

            SelectElement EntryMonth = new SelectElement(Driver.FindElementByXPath("/html/body/form/table/tbody/tr[1]/td/table/tbody/tr/td[1]/select"));
            EntryMonth.SelectByText("January");

            Driver.FindElementByXPath("/html/body/form/table/tbody/tr[4]/td[4]/font/a").Click();

            Driver.SwitchTo().Window(OriginalHandle);

            //Click calendar icon to enter exit date
            ExitCalendar.Click();
            Thread.Sleep(2000);

            Driver.SwitchTo().Window(Driver.FindCalendarHandle(OriginalHandle));

            //Click the year descend button twice to get the year to be 2014 for exit date, click on the 1st to get 02/01/2014 for exit date
            Driver.FindElementByXPath("/html/body/form/table/tbody/tr[1]/td/table/tbody/tr/td[2]/a[1]").Click();
            Driver.FindElementByXPath("/html/body/form/table/tbody/tr[1]/td/table/tbody/tr/td[2]/a[1]").Click();

            SelectElement ExitMonth = new SelectElement(Driver.FindElementByXPath("/html/body/form/table/tbody/tr[1]/td/table/tbody/tr/td[1]/select"));
            ExitMonth.SelectByText("February");

            Driver.FindElementByXPath("/html/body/form/table/tbody/tr[4]/td[7]/font/a").Click();

            Driver.SwitchTo().Window(OriginalHandle);

            //Click calculate button
            Calculate.Click();
            Thread.Sleep(2000);

            //Check cost is equal to $ 270.00
            try
            {
                Assert.AreEqual("$ 270.00", Driver.FindElementByXPath("/html/body/form/table/tbody/tr[4]/td[2]/span[1]/font/b").Text);
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }

            //Check duration of stay is (0 Days, 1 Hours, 0 Minutes)
            try
            {
                Assert.AreEqual("(31 Days, 0 Hours, 0 Minutes)", Driver.FindElementByXPath("/html/body/form/table/tbody/tr[4]/td[2]/span[2]/font/b").Text.Trim());
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }

        }
        [TestMethod]
        public void CheckParkCostandTimeCase3()
        {

            ParkingLot.SelectByValue("STP");

            //01/02/2014 In Entry Date
            EntryDate.Clear();
            EntryDate.SendKeys("01/02/2014");

            //01/01/2014 In Exit Date
            ExitDate.Clear();
            ExitDate.SendKeys("01/01/2014");

            //Click calculate button, check error message appears
            Calculate.Click();
            Thread.Sleep(2000);

            try
            {
                Assert.AreEqual("ERROR! YOUR EXIT DATE OR TIME IS BEFORE YOUR ENTRY DATE OR TIME", Driver.FindElementByXPath("/html/body/form/table/tbody/tr[4]/td[2]/span/font/b").Text.Trim());
            }

            catch (Exception ex)
            {
                Assert.Fail();
            }

        }
        [TestMethod]
        public void CheckParkCostandTimeCase4()
        {

            ParkingLot.SelectByValue("VP");

            // Enter 10:00PM in Entry Time, Enter date as 01/01/2016 In Entry Date
            EntryTime.Clear();
            EntryTime.SendKeys("10:00");

            EntryPM.Click();

            EntryDate.Clear();
            EntryDate.SendKeys("01/01/2016");

            // Enter 11:00 AM in Exit Time, Enter date as 01/01/2016 In Exit Date
            ExitTime.Clear();
            ExitTime.SendKeys("11:00");

            ExitDate.Clear();
            ExitDate.SendKeys("01/01/2016");

            //Click calculate button, Check error message appears
            Calculate.Click();
            Thread.Sleep(2000);

            try
            {
                Assert.AreEqual("ERROR! YOUR EXIT DATE OR TIME IS BEFORE YOUR ENTRY DATE OR TIME", Driver.FindElementByXPath("/html/body/form/table/tbody/tr[4]/td[2]/span/font/b").Text.Trim());

            }

            catch (Exception ex)
            {
                Assert.Fail();
            }

        }

        [TestMethod]
        public void CheckParkCostandTimeCase5()
        {

            ParkingLot.SelectByValue("LTG");

            // Enter 9:00 AM in Entry Time, Enter date as 1999 In Entry Date
            EntryTime.Clear();
            EntryTime.SendKeys("9:00");

            EntryDate.Clear();
            EntryDate.SendKeys("1999");

            // Enter 3:00 PM in Exit Time, Enter date as 2000 In Exit Date
            ExitTime.Clear();
            ExitTime.SendKeys("3:00");

            ExitPM.Click();

            ExitDate.Clear();
            ExitDate.SendKeys("2000");

            //Click calculate button, check for error
            Calculate.Click();
            Thread.Sleep(2000);

            try
            {
                Assert.AreEqual("ERROR! ENTER A CORRECTLY FORMATTED DATE", Driver.FindElementByXPath("/html/body/form/table/tbody/tr[4]/td[2]/span[2]/font/b").Text.Trim());
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }

        }

        [TestMethod]
        public void CheckParkCostandTimeCase6()
        {

            ParkingLot.SelectByValue("EP");

            // Enter 12:00 PM in Entry Time, Enter date as 01/01/2016 In Entry Date
            EntryTime.Clear();
            EntryTime.SendKeys("12:00");

            EntryPM.Click();

            EntryDate.Clear();
            EntryDate.SendKeys("01/01/2016");

            // Enter 3:00 PM in Exit Time, Leave Exit date as MM/DD/YYYY
            ExitTime.Clear();
            ExitTime.SendKeys("3:00");

            ExitPM.Click();

            //Click calculate button, check error message
            Calculate.Click();
            Thread.Sleep(2000);

            try
            {
                Assert.AreEqual("ERROR! ENTER A CORRECTLY FORMATTED DATE", Driver.FindElementByXPath("/html/body/form/table/tbody/tr[4]/td[2]/span/font/b").Text.Trim());
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }

        }

        [TestMethod]
        public void CheckParkCostandTimeCase7()
        {
            String OriginalHandle = Driver.CurrentWindowHandle;

            ParkingLot.SelectByValue("LTS");

            // Enter 4:00 PM in Entry Time
            EntryTime.Clear();
            EntryTime.SendKeys("4:00");

            EntryPM.Click();

            //Click calendar icon to enter entry date
            EntryCalendar.Click();
            Thread.Sleep(2000);

            //Switch Driver to new calendar frame for entry date 01/02/2016
            Driver.SwitchTo().Window(Driver.FindCalendarHandle(OriginalHandle));

            SelectElement EntryMonth = new SelectElement(Driver.FindElementByXPath("/html/body/form/table/tbody/tr[1]/td/table/tbody/tr/td[1]/select"));
            EntryMonth.SelectByText("January");

            Driver.FindElementByXPath("/html/body/form/table/tbody/tr[4]/td[7]/font/a").Click();

            Driver.SwitchTo().Window(OriginalHandle);

            //Check date is formatted correctly from calendar
            try
            {
                String DateValue = EntryDate.GetAttribute("value");
                Assert.AreEqual("1/2/2016", DateValue);
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }

            // Enter 3:00 AM in Exit Time, Enter date as 01/07/2016 In Exit Date
            ExitTime.Clear();
            ExitTime.SendKeys("3:00");

            ExitDate.Clear();
            ExitDate.SendKeys("01/07/2016");

            //Click calculate button  
            Calculate.Click();
            Thread.Sleep(2000);

            //Check cost is equal to $ 50.00
            try
            {
                Assert.AreEqual("$ 50.00", Driver.FindElementByXPath("/html/body/form/table/tbody/tr[4]/td[2]/span[1]/font/b").Text);
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }

            //Check duration of stay is (4 Days, 11 Hours, 0 Minutes)

            try
            {
                Assert.AreEqual("(4 Days, 11 Hours, 0 Minutes)", Driver.FindElementByXPath("/html/body/form/table/tbody/tr[4]/td[2]/span[2]/font/b").Text.Trim());
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }

        }
        [TestMethod]
        public void CheckParkCostandTimeCase8()
        {
            DateTime Time = DateTime.Now;

            ParkingLot.SelectByValue("LTG");

            // Enter right now for the time
            EntryTime.Clear();
            EntryTime.SendKeys(Time.ToString(@"hh:mm"));

            if (Time.ToString(@"tt").Equals("PM"))
            {
                EntryPM.Click();
            }

            //Enter today In Entry Date
            EntryDate.Clear();
            EntryDate.SendKeys(Time.ToString(@"MM\/ dd\/ yyyy"));

            Time = Time.AddHours(1);

            // Enter an hour from now in exit time
            ExitTime.Clear();
            ExitTime.SendKeys(Time.ToString(@"hh:mm"));

            if (Time.ToString(@"tt").Equals("PM"))
            {
                ExitPM.Click();
            }

            //Enter tomorrow in exit date
            ExitDate.Clear();
            ExitDate.SendKeys(Time.AddDays(1).ToString(@"MM\/ dd\/ yyyy"));

            //Click calculate button 
            Calculate.Click();
            Thread.Sleep(2000);

            //Check cost is equal to $ 14.00
            try
            {
                Assert.AreEqual("$ 14.00", Driver.FindElementByXPath("/html/body/form/table/tbody/tr[4]/td[2]/span[1]/font/b").Text);
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }

            //Check duration of stay is (1 Days, 1 Hours, 0 Minutes)

            try
            {
                Assert.AreEqual("(1 Days, 1 Hours, 0 Minutes)", Driver.FindElementByXPath("/html/body/form/table/tbody/tr[4]/td[2]/span[2]/font/b").Text.Trim());
            }
            catch (Exception ex)
            {

                Assert.Fail();
            }

        }

    }

}
