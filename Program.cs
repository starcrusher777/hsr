using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bots.Http;
using static Telegram.Bot.Types.Enums.UpdateType;
using static train.Commands;
using CallbackQuery = Telegram.Bot.Types.CallbackQuery;

namespace train
{
    class Program
    {
        private static Dictionary<long, int> _userAnswers = new();
        static ITelegramBotClient bot = new TelegramBotClient("6528515375:AAGH2grbOdX0Iee12YfePk0Ihbh51W_O95I");
        static List<(long, string)> stages = new List<(long, string)>();
        private static Dictionary<long, List<string>> Answers = new();

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            long? numbersChatId = null;
            long? daysChatId = null;
            var message = update.Message;
            long chatId = message.Chat.Id;
            
            
            
            if (message == null || message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return;
            
            if (!Answers.ContainsKey(chatId))
            {
                Answers[chatId] = new List<string>();
            }
            
            if (_userAnswers.ContainsKey(chatId))
            {
                var currentQuestion = _userAnswers[chatId];
                switch (currentQuestion)
                {
                    case 0:
                        await SendQuestion1(chatId, update);
                        break;
                    case 1:
                        await ProcessAnswer1(chatId, update);
                        await SendQuestion2(chatId, update);
                        break;
                    case 2:
                        await ProcessAnswer2(chatId, update);
                        await SendQuestion3(chatId, update);
                        break;
                    case 3:
                        await ProcessAnswer3(chatId,update);
                        _userAnswers.Remove(chatId);
                        break;
                }
            }
            
            
            var chatStage = await getChatStage(message.Chat.Id);
            if (!string.IsNullOrEmpty(chatStage))
            {
                switch (chatStage)
                {
                    case CountJumps: await HandleReceivedNumbersAsync(message);
                        break;
                    case MainMenu: await toMainMenu(message);
                        break;
                    case CountJade: 
                        break;
                    case hasPass : await ProcessAnswer1(chatId, update); await SendQuestion2(chatId, update);
                        break;
                    case noPass : await ProcessAnswer1(chatId, update); await SendQuestion2(chatId, update);
                        break;
                    case hasBP : await ProcessAnswer2(chatId, update); await SendQuestion3(chatId, update);
                        break;
                    case noBP : await ProcessAnswer2(chatId, update); await SendQuestion3(chatId, update);
                        break;

                }
                return;
            }

            if (message.Text == CountJumps)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Вы выбрали посчитать прыжки, введите колличество нефрита");
                await setChatInStage(message.Chat.Id, CountJumps);
                return;
            }
            
            if (message.Text == CountJade)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Вы выбрали посчитать колличество нефрита через X дней");
                _userAnswers[chatId] = 0;
                await setChatInStage(message.Chat.Id, CountJade);
                return;
            }

            if (message.Text == hasPass)
            {
                _userAnswers[chatId] = 1;
                await setChatInStage(chatId, hasPass);
            }

            if (message.Text == noPass)
            {
                _userAnswers[chatId] = 1;
                await setChatInStage(chatId, noPass);
            }
            if (message.Text == hasBP)
            {
                _userAnswers[chatId] = 2;
                await setChatInStage(chatId, hasBP);
            }

            if (message.Text == noBP)
            {
                _userAnswers[chatId] = 2;
                await setChatInStage(chatId, noBP);
            }
            
            
            
            
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
           
            if (update.Type == UpdateType.Message)
            {
                
                if (message.Text.ToLower() == "/start")
                {
                    await botClient.SendTextMessageAsync(message.Chat, "Выбери клавиатуру:\n" +
                                                                       "/reply\n");
                    return;
                }

                if (message.Text == "/reply")
                {
                    var replyKeyboard = new ReplyKeyboardMarkup(
                        new List<KeyboardButton[]>()
                        {
                            new KeyboardButton[]
                            {
                                new KeyboardButton(CountJumps),
                                new KeyboardButton(CountJade),
                            },
                        })
                    {
                        ResizeKeyboard = true,
                        };
                    await botClient.SendTextMessageAsync(message.Chat, "Что вас интересует?",
                        replyMarkup: replyKeyboard);    
                        
                }
            }
            else if (update.Type == UpdateType.CallbackQuery)
            {
                var callbackQuery = update.CallbackQuery;
            }
        }
        
        
        
        static async Task HandleReceivedDaysAsync(Message message)
        {
            var daysChatId = message.Chat.Id;

            if (message.Text != null)
            {
                if (double.TryParse(message.Text, out double inputNumber3))
                {
                    var not = inputNumber3 * 150;
                    var yes = inputNumber3 * 60;
                    await bot.SendTextMessageAsync(daysChatId, $"Колличество нефрита через {inputNumber3} дней Без пропуска - {yes} с пропуском снабжения - {not}");
                    await removeChatFromStage(daysChatId);
                }
                else
                {
                    await bot.SendTextMessageAsync(daysChatId, "Пожалуйста, введите число в правильном формате.");
                }
            }
        }
        static async Task HandleReceivedNumbersAsync(Message message)
        {
            var NumbersChatId = message.Chat.Id;

            if (message.Text != null)
            {
                if (double.TryParse(message.Text, out double inputNumber))
                {
                    var result1 = (int)Math.Floor(inputNumber / 160);
                    double result2 = inputNumber - (result1 * 160);
                    await bot.SendTextMessageAsync(NumbersChatId, $"Колличество прыжков: {result1} + {result2} нефрита остаток");
                    await removeChatFromStage(NumbersChatId);
                }
                else
                {
                    await bot.SendTextMessageAsync(NumbersChatId, "Пожалуйста, введите число в правильном формате.");
                }
            }
        }
        static async Task setChatInStage(long chatId, string stageName)
        {
            stages.Add(new(chatId, stageName));
        }
        static async Task<string> getChatStage(long chatId)
        {
            return stages.FirstOrDefault(x => x.Item1 == chatId).Item2;
        }
        static async Task removeChatFromStage(long chatId)
        {
            stages.RemoveAll(x => x.Item1 == chatId);
        }
        static async Task toMainMenu(Message message)
        {
            if (message.Text == "В главное меню")
            {
                var replyKeyboard = new ReplyKeyboardMarkup(
                    new List<KeyboardButton[]>()
                    {
                        new KeyboardButton[]
                        {
                            new KeyboardButton("В главное меню")
                        }
                    })
                {
                    ResizeKeyboard = true,
                };
                await bot.SendTextMessageAsync(message.Chat, "Что вас интересует?",
                    replyMarkup: replyKeyboard);    
                return;
            }
        }
        static async Task SendQuestion1(long chatId, Update update)
        {
            var replyMarkup = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton("Есть пропуск снабжения"),
                new KeyboardButton("Нет пропуска снабжения"),
            })
            {
                ResizeKeyboard = true
            };
            await bot.SendTextMessageAsync(chatId, "Есть ли у вас пропуск снабжения?", replyMarkup: replyMarkup);
        }

        static async Task SendQuestion2(long chatId, Update update)
        {
            var replyMarkup = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton("Есть боевой пропуск"),
                new KeyboardButton("Нет боевого пропуска"),
            })
            {
                ResizeKeyboard = true
            };
            await bot.SendTextMessageAsync(chatId, "Есть ли у вас боевой пропуск?", replyMarkup: replyMarkup);
        }

        static async Task SendQuestion3(long chatId, Update update)
        {
            var replyMarkup = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton("1"),
                new KeyboardButton("2"),
                new KeyboardButton("3"),
                new KeyboardButton("4"),
                new KeyboardButton("5"),
                new KeyboardButton("6"),
                new KeyboardButton("7"),
                new KeyboardButton("8"),
                new KeyboardButton("9"),
                new KeyboardButton("10"),
            })
            {
                ResizeKeyboard = true
            };
            await ProcessAnswer3(chatId, update);
            await bot.SendTextMessageAsync(chatId, "Сколько этажей воспоминания хаоса вы проходите?", replyMarkup: replyMarkup);
        }

        
        static async Task ProcessAnswer1(long chatId, Update update)
        {
            var answerText = update.Message.Text;
            var answer1 = answerText;
            Answers[chatId].Add(answer1);
            removeChatFromStage(chatId);
            await bot.SendTextMessageAsync(chatId, $"Вы ответили на первый вопрос: {answerText}");
        }

        static async Task ProcessAnswer2(long chatId, Update update)
        {
            var answerText = update.Message.Text;
            var answer2 = answerText;
            Answers[chatId].Add(answer2);
            removeChatFromStage(chatId);
            await bot.SendTextMessageAsync(chatId, $"Вы ответили на первый вопрос: {answerText}");
        }

        static async Task ProcessAnswer3(long chatId, Update update)
        {
            var answerText = update.Message.Text;
            var answer3 = answerText;
            Answers[chatId].Add(answer3);
            removeChatFromStage(chatId);
            await bot.SendTextMessageAsync(chatId, $"Вы ответили на первый вопрос: {answerText}");
        }

        
        
        
        public static void Main(string[] args) 
            { 
                Console.WriteLine("Let`s go " + bot.GetMeAsync().Result.FirstName);
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
                {
                    AllowedUpdates = {  }, 
                };

            bot.StartReceiving(
                (client, update, cancellationToken) => HandleUpdateAsync(client, update, cancellationToken),
                (client, exception, arg3) => HandleErrorAsync(client, exception, arg3),
                receiverOptions,
                cancellationToken
                              );

            Console.ReadLine();
        }

        private static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }
    }
}