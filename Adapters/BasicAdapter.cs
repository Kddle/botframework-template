using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Builder.TraceExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public class BasicAdapter : BotFrameworkHttpAdapter
{
    private readonly IConfiguration configuration;
    private readonly ILogger logger;
    private readonly ConversationState conversationState;

    public BasicAdapter(IConfiguration configuration, ILogger<BotFrameworkHttpAdapter> logger, ConversationState conversationState) : base(configuration, logger)
    {
        this.configuration = configuration;
        this.logger = logger;
        this.conversationState = conversationState;
        OnTurnError = ProcessTurnError;
    }

    private async Task ProcessTurnError(ITurnContext turnContext, Exception ex)
    {
        if (logger != null)
        {
            logger.LogError(ex, $"[OnTurnError] Error : {ex.Message}");
        }

        await turnContext.SendActivityAsync("The bot encountered an error");

        await turnContext.TraceActivityAsync("OnTurnError Trace", ex.Message, "https://www.botframework.com/schemas/error", "TurnError");

        if (conversationState != null)
        {
            await conversationState.DeleteAsync(turnContext);
        }
    }
}