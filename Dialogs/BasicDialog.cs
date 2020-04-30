using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

public class BasicDialog : ComponentDialog
{
    public BasicDialog(BotState conversationState)
    {
        var steps = new WaterfallStep[] { EchoStep };

        AddDialog(new WaterfallDialog(nameof(BasicDialog), steps));
        InitialDialogId = nameof(BasicDialog);
    }

    private async Task<DialogTurnResult> EchoStep(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    {
        await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Vous avez dit : {stepContext.Context.Activity.Text}"), cancellationToken: cancellationToken);
        return await stepContext.EndDialogAsync(null, cancellationToken);
    }
}