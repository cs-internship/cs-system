using CrystaLearn.Shared.Dtos.Chatbot;

namespace CrystaLearn.Shared.Controllers.Chatbot;

[AuthorizedApi]
[Route("api/[controller]/[action]/")]
public interface IChatbotController : IAppController
{
    [HttpGet("{kind}")]
    Task<string> GetSystemPromptMarkdown(PromptKind kind, CancellationToken cancellationToken);

    [HttpPost]
    Task UpdateSystemPrompt(UpdateSystemPromptDto request, CancellationToken cancellationToken);
}
