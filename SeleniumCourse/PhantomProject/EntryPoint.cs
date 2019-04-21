using System;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System.Collections.Generic; //Needed for the list below.
using System.IO;
using System.Threading;

class EntryPoint
{
    static void Main()
    {
        //Headless option for ChromeDriver and instantiating the driver
        ChromeOptions options = new ChromeOptions();
        options.AddArgument("--headless");
        IWebDriver driver = new ChromeDriver(options);

        List<string> extractedLinks = new List<string>();
        List<string> extractedTitles = new List<string>();
        List<string> extractedContents = new List<string>();

        string sitemapURL = "http://testing.todvachev.com/sitemap-posttype-post.xml";
        string titleSelector = "#main-content > article > header > h1";
        string contentSelector = "#main-content > article > div";

        string[] pageSource;

        IWebElement titleElement;
        IWebElement contentElement;

        int startIndex = 0;
        int linkLength = 0;

        driver.Navigate().GoToUrl(sitemapURL);

        pageSource = driver.PageSource.Split(' ');

        // Extract all of the post links from the Sitemap.xml.
        foreach (var item in pageSource)
        {
            if (item.Contains(@"href=""http://testing.todvachev.com"))
            {
                startIndex = item.IndexOf(@"href=\", StringComparison.CurrentCulture) + 7;
                linkLength = item.LastIndexOf(@""">", StringComparison.CurrentCulture) - startIndex;

                //extractedLinks.Add(item.Substring(item.IndexOf(@"href=""http://testing.todvachev.com")));
                //Console.WriteLine(item.Substring(item.IndexOf(@"href=""http://testing.todvachev.com")));

                extractedLinks.Add(item.Substring(startIndex, linkLength));
            }
        }

        // Open each of the posts and extract the title and the content.
        foreach (var item in extractedLinks)
        {
            try
            {
                driver.Navigate().GoToUrl(item);

                titleElement = driver.FindElement(By.CssSelector(titleSelector));
                contentElement = driver.FindElement(By.CssSelector(contentSelector));

                Console.WriteLine(titleElement.Text);
                Console.WriteLine(contentElement.Text);

                //extractedTitles.Add(titleElement.Text);
                //extractedContents.Add(contentElement.Text);
            }
            catch (OpenQA.Selenium.UnhandledAlertException)
            {
                Console.WriteLine("The post located at " + item + " was unable to be read.");
            }
        }

        ////Create directory to store files in
        //Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\ExtractedContent");

        //using (StreamWriter sw = File.CreateText(Directory.GetCurrentDirectory() + @"\ExtractedContent\ExtractedTest.txt"))
        //{
        //    sw.WriteLine("TITLE: {0}", extractedTitles[0]);
        //    sw.WriteLine("CONTENT:");
        //    sw.Write(extractedContents[0]);
        //}
    }
}


