using Microsoft.Playwright.NUnit;
using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace UITests;

[Parallelizable(ParallelScope.Self)] /* https://playwright.dev/dotnet/docs/test-runners#running-nunit-tests-in-parallel */
[TestFixture]
public class ChatTest : PageTest /* https://playwright.dev/dotnet/docs/test-runners#base-nunit-classes-for-playwright */
{
    const string namePlaceholder = "Your name";
    const string messagePlaceholder = "Type message here";
    const string userName = "Rob";
    const string separator = " :";

    [SetUp]
    public async Task SetUp()
    {
        await Page.GotoAsync("http://localhost/");
    }

    [Test]
    public async Task ShouldHaveTheCorrectTitle()
    {
        var title = Page.Locator(".ud-header .text-center");
        await Expect(title).ToHaveTextAsync("Public Chat");
    }

    [Test]
    public async Task ShouldSendTheMessage()
    {
        var userInput = "Ey mate";

        await Page.GetByPlaceholder(namePlaceholder).FillAsync(userName);
        await Page.GetByPlaceholder(messagePlaceholder).FillAsync(userInput);
        await Page.GetByRole(AriaRole.Button, new() { Name = "" }).ClickAsync();

        var msg = Page.Locator(".msg-box .in-msg");
        await Expect(msg).ToHaveTextAsync(userName + separator + userInput);
    }

    [Test]
    public async Task ShouldHaveAIResponses()
    {
        var AIuser = "AI";
        var userInput = "AI: hey chatGPT";
        var expectedAIresponse = "Hello! How can I assist you today?";

        await Page.GetByPlaceholder(namePlaceholder).FillAsync(userName);
        await Page.GetByPlaceholder(messagePlaceholder).FillAsync(userInput);
        await Page.GetByRole(AriaRole.Button, new() { Name = "" }).ClickAsync();

        var myMsg = Page.Locator(".msg-box .in-msg");
        await Expect(myMsg).ToHaveTextAsync(userName + separator + userInput);

        var response = Page.Locator(".msg-box .ex-msg");
        await Expect(response).ToHaveTextAsync(AIuser + separator + expectedAIresponse);
    }
}