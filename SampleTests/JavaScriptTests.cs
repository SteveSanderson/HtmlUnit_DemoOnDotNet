using System.Linq;
using com.gargoylesoftware.htmlunit;
using com.gargoylesoftware.htmlunit.html;
using NUnit.Framework;

namespace SampleTests
{
    public class JavaScriptTests
    {
        private WebClient webClient;
        [SetUp] public void Setup()
        {
            webClient = new WebClient();
        }

        [Test]
        public void jQuery_Autocomplete_Lon_Suggests_London()
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