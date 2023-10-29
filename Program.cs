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
    internal class Program
    {
        private static Dictionary<long, int> _userAnswers = new();
        private static ITelegramBotClient bot = new TelegramBotClient("6528515375:AAGH2grbOdX0Iee12YfePk0Ihbh51W_O95I");
        private static List<(long, string)> stages = new List<(long, string)>();
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
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
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
                    case hasPass : 
                        break;
                    case noPass : 
                        break;
                    case hasBP : 
                        break;
                    case noBP : 
                        break;
                    case "I" : 
                        break;
                    case "II" : 
                        break;
                    case "III" : 
                        break;
                    case "IV" :
                        break;
                    case "V" : 
                        break;
                    case "VI" : 
                        break;
                    case "VII" : 
                        break;
                    case "VIII" : 
                        break;
                    case "IX" : 
                        break;
                    case "X" : 
                        break;
                    case "days":
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
                await SendQuestion1(chatId, update);
                return;
            }

            if (message.Text == hasPass)
            {
                _userAnswers[chatId] = 1;
                Console.WriteLine($"Установлено значение _userAnswers[{chatId}] = {_userAnswers[chatId]}");
                await setChatInStage(chatId, hasPass);
                await ProcessAnswer1(chatId, update);
                await SendQuestion2(chatId, update);
                
            }

            if (message.Text == noPass)
            {
                _userAnswers[chatId] = 1;
                Console.WriteLine($"Установлено значение _userAnswers[{chatId}] = {_userAnswers[chatId]}");
                await setChatInStage(chatId, noPass);
                await ProcessAnswer1(chatId, update);
                await SendQuestion2(chatId, update);
                
            }
            
            if (message.Text == hasBP)
            {
                _userAnswers[chatId] = 2;
                Console.WriteLine($"Установлено значение _userAnswers[{chatId}] = {_userAnswers[chatId]}");
                await setChatInStage(chatId, hasBP);
                await ProcessAnswer2(chatId, update);
                await SendQuestion3(chatId, update);
                
            }

            if (message.Text == noBP)
            {
                _userAnswers[chatId] = 2;
                Console.WriteLine($"Установлено значение _userAnswers[{chatId}] = {_userAnswers[chatId]}");
                await setChatInStage(chatId, noBP);
                await ProcessAnswer2(chatId, update);
                await SendQuestion3(chatId, update);
                
            }
            
            if (message.Text == "I")
            {
                _userAnswers[chatId] = 3;
                Console.WriteLine($"Установлено значение _userAnswers[{chatId}] = {_userAnswers[chatId]}");
                await setChatInStage(chatId, "1");
                await ProcessAnswer3(chatId, update);
                await SendQuestion4(chatId, update);
                
            }
            
            if (message.Text == "II")
            {
                _userAnswers[chatId] = 3;
                await setChatInStage(chatId, "2");
                await ProcessAnswer3(chatId, update);
            }
            
            if (message.Text == "III")
            {
                _userAnswers[chatId] = 3;
                await setChatInStage(chatId, "3");
                await ProcessAnswer3(chatId, update);
            }
            
            if (message.Text == "IV")
            {
                _userAnswers[chatId] = 3;
                await setChatInStage(chatId, "4");
                await ProcessAnswer3(chatId, update);
            }
            
            if (message.Text == "V")
            {
                _userAnswers[chatId] = 3;
                await setChatInStage(chatId, "5");
                await ProcessAnswer3(chatId, update);
            }
            
            if (message.Text == "VI")
            {
                _userAnswers[chatId] = 3;
                await setChatInStage(chatId, "6");
                await ProcessAnswer3(chatId, update);
            }
            
            if (message.Text == "VII")
            {
                _userAnswers[chatId] = 3;
                await setChatInStage(chatId, "7");
                await ProcessAnswer3(chatId, update);
            }
            
            if (message.Text == "VIII")
            {
                _userAnswers[chatId] = 3;
                await setChatInStage(chatId, "8");
                await ProcessAnswer3(chatId, update);
            }
            
            if (message.Text == "IX")
            {
                _userAnswers[chatId] = 3;
                await setChatInStage(chatId, "9");
                await ProcessAnswer3(chatId, update);
            }
            
            if (message.Text == "X")
            {
                _userAnswers[chatId] = 3;
                await setChatInStage(chatId, "10");
                await ProcessAnswer3(chatId, update);
            }

            if (message.Text != null)
            {
                _userAnswers[chatId] = 4;
                Console.WriteLine($"Установлено значение _userAnswers[{chatId}] = {_userAnswers[chatId]}");
                await setChatInStage(chatId, "days");
                await ProcessAnswer4(chatId, update);
                
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


        private static async Task HandleReceivedDaysAsync(Message message)
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

        private static async Task HandleReceivedNumbersAsync(Message message)
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

        private static async Task setChatInStage(long chatId, string stageName)
        {
            stages.Add(new(chatId, stageName));
        }

        private static async Task<string> getChatStage(long chatId)
        {
            return stages.FirstOrDefault(x => x.Item1 == chatId).Item2;
        }

        private static async Task removeChatFromStage(long chatId)
        {
            stages.RemoveAll(x => x.Item1 == chatId);
        }

        private static async Task toMainMenu(Message message)
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

        private static async Task SendQuestion1(long chatId, Update update)
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
            await removeChatFromStage(chatId);
        }

        private static async Task SendQuestion2(long chatId, Update update)
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
            await removeChatFromStage(chatId);
        }

        private static async Task SendQuestion3(long chatId, Update update)
        {
            var replyMarkup = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton("I"),
                new KeyboardButton("II"),
                new KeyboardButton("III"),
                new KeyboardButton("IV"),
                new KeyboardButton("V"),
                new KeyboardButton("VI"),
                new KeyboardButton("VII"),
                new KeyboardButton("VIII"),
                new KeyboardButton("IX"),
                new KeyboardButton("X"),
            })
            {
                ResizeKeyboard = true
            };
            
            await bot.SendTextMessageAsync(chatId, "Сколько этажей воспоминания хаоса вы проходите?", replyMarkup: replyMarkup);
            await ProcessAnswer3(chatId, update);
            await removeChatFromStage(chatId);
            
        }
        
        private static async Task SendQuestion4(long chatId, Update update)
        {
            await bot.SendTextMessageAsync(chatId, "Введите колличество дней");
            await removeChatFromStage(chatId);
        }


        private static async Task ProcessAnswer1(long chatId, Update update)
        {
            var answerText = update.Message.Text;
            var answer1 = answerText;
            Answers[chatId].Add(answer1);
            await removeChatFromStage(chatId);
            await bot.SendTextMessageAsync(chatId, $"Вы ответили на первый вопрос: {answerText}");
        }

        private static async Task ProcessAnswer2(long chatId, Update update)
        {
            var answerText = update.Message.Text;
            var answer2 = answerText;
            Answers[chatId].Add(answer2);
            await removeChatFromStage(chatId);
            await bot.SendTextMessageAsync(chatId, $"Вы ответили на второй вопрос: {answerText}");
        }

        private static async Task ProcessAnswer3(long chatId, Update update)
        {
            var answerText = update.Message.Text;
            var answer3 = answerText;
            Answers[chatId].Add(answer3);
            await removeChatFromStage(chatId);
            await bot.SendTextMessageAsync(chatId, $"Вы ответили на третий вопрос: {answerText}");
        }
        
        private static async Task ProcessAnswer4(long chatId, Update update)
        {
            var answerText = update.Message.Text;
            var answer4 = answerText;
            Answers[chatId].Add(answer4);
            await removeChatFromStage(chatId);
            await bot.SendTextMessageAsync(chatId, $"Вы выбрали посчитать нефрит через {answerText} дней");
        }

        private static async Task Start(long chatId, Update update)
        {
            var replyMarkup = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton("Начать"),
            })
            {
                ResizeKeyboard = true
            };
            await bot.SendTextMessageAsync(chatId, "Начинаем", replyMarkup: replyMarkup);
            await removeChatFromStage(chatId);
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