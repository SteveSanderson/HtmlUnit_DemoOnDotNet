using System;
using System.Linq;
using com.gargoylesoftware.htmlunit;
using com.gargoylesoftware.htmlunit.html;
using NUnit.Framework;

namespace SampleTests
{
    [TestFixture]
    public class GoogleTests
    {
        private WebClient webClient;
        [SetUp] public void Setup() {
            webClient = new WebClient();
        }


        [Test]
        public void Can_Load_Google_Homepage()
        {
            var page = (HtmlPage)webClient.getPage("http://www.google.com");
            Assert.AreEqual("Google", page.getTitleText());
        }

        [Test]
        public void Google_Search_For_AspNetMvc_Yields_Link_To_Codeplex()
        {
            var searchPage = (HtmlPage)webClient.getPage("http://www.google.com");
            ((HtmlInput)searchPage.getElementByName("q")).setValueAttribute("asp.net mvc");
            var resultsPage = (HtmlPage)searchPage.getElementByName("btnG").click();

            var linksToCodeplex = from tag in resultsPage.getElementsByTagName("a").toArray().Cast<HtmlAnchor>()
                                  let href = tag.getHrefAttribute()
                                  where href.StartsWith("http://")
                                  let uri = new Uri(href)
                                  where uri.Host.ToLower().EndsWith("codeplex.com")
                                  select uri;
            CollectionAssert.IsNotEmpty(linksToCodeplex);
        }

        [Test]
        public void Runs_JavaScript()
        {
            // Arrange: Load the demo page
            var autocompleteDemoPage = (HtmlPage)webClient.getPage("http://jquery.bassistance.de/autocomplete/demo/");

            // Act: Type "lon" into the suggestions box
            autocompleteDemoPage.getElementById("suggest1").type("lon");
            webClient.waitForBackgroundJavaScript(1000);

            // Assert: Suggestions should include "London"
            var suggestions = autocompleteDemoPage.getByXPath("//div[@class='ac_results']/ul/li").toArray().Cast<HtmlElement>().Select(x => x.asText());
            CollectionAssert.Contains(suggestions, "London");
        }
    }
}
