//Namespaces
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Example.KeyBoards;
using CommandCore;
using System.Collections.Generic;
//Example NameSpace

namespace Example.MainProgram
{
    //Main class

    class Program
    {
        private static string token = "5050607112:AAGwii7PIDXQGDHOaRoTVUYb3DqWVRs2jB4";

        public static TelegramBotClient botClient;

        private static Command cmd;
        private static long chatId;

        static async Task Main()
        {
            CommandEngine engine = new CommandEngine();
            botClient = new TelegramBotClient(token);
            using var cts = new CancellationTokenSource();

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }
            };
            botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken: cts.Token);

            var me = await botClient.GetMeAsync();

            Console.WriteLine($"Бот {me.Username}, запущен на {Environment.MachineName}, на пользователе {Environment.UserName}");

            Console.ReadLine();

            // Send cancellation request to stop bot
            cts.Cancel();

            async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
            {
                if (update.Type != UpdateType.Message)
                {
                    return;
                }
                if (update.Message.Type != MessageType.Text)
                {
                    return;
                }
                var msg = update.Message;

                chatId = msg.Chat.Id;

                var messageText = msg.Text;

                if (messageText == null || messageText.Length <= 0) return;

                cmd = new Command(messageText, 3, new List<Type> { typeof(long), typeof(string), typeof(bool) });
                cmd.onError = OnCommandError;
                cmd.onDevelopError = OnCommandDevelopError;
                cmd.OnInitializeOnErrors();

                if (engine.CheckCommand("/test", cmd))
                {

                    if (engine.CheckErrors(cmd))
                    {
                        for (int i = 0; i < cmd.Args.Count; i++)
                        {
                            if (cmd.Args[i].GetType() == typeof(bool))
                            {
                                await botClient.SendTextMessageAsync(chatId, cmd.GetValueFromArgs(i).ToString());
                            }
                            else if (cmd.Args[i].GetType() == typeof(decimal))
                            {
                                await botClient.SendTextMessageAsync(chatId, ((decimal)cmd.GetValueFromArgs(i) + 5).ToString());
                            }
                            else
                            {
                                await botClient.SendTextMessageAsync(chatId, cmd.GetValueFromArgs(i).ToString());
                            }

                        }
                        for (int i = 0; i < cmd.stringArgs.Count; i++)
                        {
                            Console.WriteLine(cmd.stringArgs[i]);

                        }
                        for (int i = 0; i < cmd.decimalArgs.Count; i++)
                        {
                            Console.WriteLine(cmd.decimalArgs[i]);

                        }
                        for (int i = 0; i < cmd.booleanArgs.Count; i++)
                        {
                            Console.WriteLine(cmd.booleanArgs[i]);
                        }
                    }
                }

                /*                
                                if (messageText.ToLower() == "стикер")
                                {
                                    var sticker = await botClient.SendStickerAsync(chatId: msg.Chat.Id, "https://tlgrm.ru/_/stickers/cc6/84d/cc684d05-d7c5-4749-9a88-b31756e5169a/1.webp", replyToMessageId: msg.MessageId, replyMarkup: KeyBoard.testKeyboard);
                                }
                                if (messageText.ToLower() == "/клавиатура") {
                                    var keyboard = await botClient.SendTextMessageAsync(chatId, "Выберете действие", replyMarkup: KeyBoard.testKeyboard);
                                }

                                if (messageText.ToLower() == "сообщение")
                                {
                                    var message = await botClient.SendTextMessageAsync(chatId, "Вот ваше сообщение" , replyMarkup: KeyBoard.testKeyboard);
                                }
                                if (messageText.ToLower() == "музыка")
                                {
                                    var message = await botClient.SendAudioAsync(
                                        chatId: chatId,
                                        audio: "https://github.com/TelegramBots/book/raw/master/src/docs/audio-guitar.mp3"
                                     );
                                }*/

            }

            Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
            {
                var ErrorMessage = exception switch
                {
                    ApiRequestException apiRequestException
                        => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                    _ => exception.ToString()
                };

                Console.WriteLine(ErrorMessage);
                return Task.CompletedTask;
            }
        }
        private static void OnCommandError(Error error)
        {
            error.WriteError("Command error:");

            error.SendBotMessage(botClient, chatId);
        }
        private static void OnCommandDevelopError(Error error)
        {
            error.WriteError("Develop command error:");
        }
    }
}