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
    }
}
