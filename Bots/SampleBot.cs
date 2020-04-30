using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;

public class SampleBot : ActivityHandler
{
    public readonly IConfiguration Configuration;
    private Dialog dialog;
    private readonly BotState conversationState;

    public SampleBot(IConfiguration configuration, ConversationState conversationState)
    {
        Configuration = configuration;
        this.conversationState = conversationState;
        dialog = new BasicDialog(conversationState);
    }

    protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
    {
        foreach (var member in membersAdded)
            if (member.Id != turnContext.Activity.Recipient.Id)
                await turnContext.SendActivityAsync(MessageFactory.Text("Bonjour !"), cancellationToken);
    }

    protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
    {
        await dialog.RunAsync(turnContext, conversationState.CreateProperty<DialogState>(nameof(BasicDialog)), cancellationToken);
    }

    public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
    {
        await base.OnTurnAsync(turnContext, cancellationToken);
        await conversationState.SaveChangesAsync(turnContext, false, cancellationToken);
    }
}