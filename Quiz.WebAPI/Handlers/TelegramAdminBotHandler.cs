using Microsoft.Extensions.Options;
using Quiz.DataAccessLayer.Models;
using Quiz.WebAPI.Common;
using Quiz.WebAPI.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Quiz.WebAPI.Handlers;

interface IMenuConst
{
    public static string MenuText { get; }
    public static string MenuKey { get; }
}

#region ConstNamingMenu

public abstract class MainMenu: IMenuConst
{
    public static string MenuText => "Главное меню";
    public static string MenuKey => "mainMenu";
}

public abstract class ManageQuizMenu: IMenuConst
{
    public static string MenuText => "Управление викториной";
    public static string MenuKey => "manageQuiz";
}

public abstract class ManageQuestionsMenu: IMenuConst
{
    public static string MenuText => "Управление вопросами";
    public static string MenuKey => "manageQuestions";
}

public abstract class ManagePlayersMenu: IMenuConst
{
    public static string MenuText => "Управление игроками";
    public static string MenuKey => "managePlayers";
}

public abstract class BackMenu: IMenuConst
{
    public static string MenuText => "Назад";
    public static string MenuKey => "backMenu";
}

public abstract class StartQuizMenu: IMenuConst
{
    public static string MenuText => "Начать викторину";
    public static string MenuKey => "startQuiz";
}

public abstract class NextStepQuizMenu: IMenuConst
{
    public static string MenuText => "Следующий шаг";
    public static string MenuKey => "nextStep";
}

public abstract class PrevStepQuizMenu: IMenuConst
{
    public static string MenuText => "Предыдущий шаг";
    public static string MenuKey => "prevStep";
}

public abstract class TopFiveQuizMenu: IMenuConst
{
    public static string MenuText => "Топ 5 результатов";
    public static string MenuKey => "top5Step";
}

public abstract class FullStatisticsQuizMenu: IMenuConst
{
    public static string MenuText => "Полная статистика";
    public static string MenuKey => "fullStatsStep";
}

public abstract class AddQuestionQuizMenu: IMenuConst
{
    public static string MenuText => "Добавить вопрос";
    public static string MenuKey => "addQuestion";
}

public abstract class DeleteQuestionQuizMenu: IMenuConst
{
    public static string MenuText => "Удалить вопрос";
    public static string MenuKey => "deleteQuestion";
}

public abstract class ShowListQuestionQuizMenu: IMenuConst
{
    public static string MenuText => "Показать список вопросов";
    public static string MenuKey => "showListQuestion";
}

public abstract class NextPageListQuestionQuizMenu: IMenuConst
{
    public static string MenuText => "Следующая страница";
    public static string MenuKey => "nextPage";
}

public abstract class PrevPageListQuestionQuizMenu: IMenuConst
{
    public static string MenuText => "Предыдущая страница";
    public static string MenuKey => "prevPage";
}

public abstract class ShowPlayersQuizMenu: IMenuConst
{
    public static string MenuText => "Показать список игроков";
    public static string MenuKey => "showPlayersQuiz";
}

public abstract class UpdatePlayerQuizMenu: IMenuConst
{
    public static string MenuText => "Обновить игрока";
    public static string MenuKey => "updatePlayerQuiz";
}

public abstract class DeletePlayerQuizMenu: IMenuConst
{
    public static string MenuText => "Удалить игрока";
    public static string MenuKey => "deletePlayerQuiz";
}

#endregion


/// <summary>
/// Обработчик команд администратора через Inline-кнопки Telegram
/// </summary>
public class TelegramAdminBotHandler(
    IOptions<TelegramBotOptions> telegramBotOptions,
    ITelegramBotClient bot,
    QuizStateMachine stateMachine,
    IQuizService quizService)
{
    private string _prevCommand = string.Empty;

    private InlineKeyboardMarkup _mainMenu = new[]
    {
        new[] {  InlineKeyboardButton.WithCallbackData(ManageQuizMenu.MenuText, ManageQuizMenu.MenuKey) },
        new[] {  InlineKeyboardButton.WithCallbackData(ManageQuestionsMenu.MenuText, ManageQuestionsMenu.MenuKey) },
        new[] {  InlineKeyboardButton.WithCallbackData(ManagePlayersMenu.MenuText, ManagePlayersMenu.MenuKey) },
    };

    private InlineKeyboardMarkup _manageQuestions = new[]
    {
        new[] { InlineKeyboardButton.WithCallbackData(ShowListQuestionQuizMenu.MenuText, ShowListQuestionQuizMenu.MenuKey) },
        new[] { InlineKeyboardButton.WithCallbackData(AddQuestionQuizMenu.MenuText, AddQuestionQuizMenu.MenuKey) },
        new[] { InlineKeyboardButton.WithCallbackData(DeleteQuestionQuizMenu.MenuText, DeleteQuestionQuizMenu.MenuKey) },
        new[] { InlineKeyboardButton.WithCallbackData(BackMenu.MenuText, BackMenu.MenuKey) },
    };

    private InlineKeyboardMarkup _showListQuestionsMenu = new[]
    {
        new[] { InlineKeyboardButton.WithCallbackData(NextPageListQuestionQuizMenu.MenuText, NextPageListQuestionQuizMenu.MenuKey) },
        new[] { InlineKeyboardButton.WithCallbackData(PrevStepQuizMenu.MenuText, PrevStepQuizMenu.MenuKey) },
        new[] { InlineKeyboardButton.WithCallbackData(BackMenu.MenuText, BackMenu.MenuKey) },
    };

    private InlineKeyboardMarkup GenerateManageQuizMenu()
    {
        var currentState = stateMachine.Current;

        return currentState switch
        {
            QuizStateEnum.WaitingForStart => new[]
            {
                new[] { InlineKeyboardButton.WithCallbackData(StartQuizMenu.MenuText, StartQuizMenu.MenuKey) },
                new[] { InlineKeyboardButton.WithCallbackData(BackMenu.MenuText, BackMenu.MenuKey) },
            },
            QuizStateEnum.DisplayCorrectAnswer => new[]
            {
                new[] { InlineKeyboardButton.WithCallbackData(NextStepQuizMenu.MenuText, NextStepQuizMenu.MenuKey) },
                new[] { InlineKeyboardButton.WithCallbackData(PrevStepQuizMenu.MenuText, PrevStepQuizMenu.MenuKey) },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(TopFiveQuizMenu.MenuText, TopFiveQuizMenu.MenuKey),
                    InlineKeyboardButton.WithCallbackData(FullStatisticsQuizMenu.MenuText, FullStatisticsQuizMenu.MenuKey)
                },
                new[] { InlineKeyboardButton.WithCallbackData(BackMenu.MenuText, BackMenu.MenuKey) }
            },
            _ => new[]
            {
                new[] { InlineKeyboardButton.WithCallbackData(NextStepQuizMenu.MenuText, NextStepQuizMenu.MenuKey) },
                new[] { InlineKeyboardButton.WithCallbackData(PrevStepQuizMenu.MenuText, PrevStepQuizMenu.MenuKey) },
                new[] { InlineKeyboardButton.WithCallbackData(BackMenu.MenuText, BackMenu.MenuKey) },
            }
        };
    }

    public async Task HandleAsync(Update update)
    {
        if (update.Type == UpdateType.Message)
        {
            InlineKeyboardMarkup keyboard;
            // Отправка меню админа
            switch(_prevCommand)
            {
                default:
                    _prevCommand = MainMenu.MenuKey;
                    keyboard = _mainMenu;
                    break;
            };

            await bot.SendTextMessageAsync(telegramBotOptions.Value.AdminId, "Выберите действие:", replyMarkup: keyboard);
        }
        else if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery?.Message?.Chat.Id == telegramBotOptions.Value.AdminId)
        {
            var data = update.CallbackQuery.Data;
            await bot.AnswerCallbackQueryAsync(update.CallbackQuery.Id);

            switch (data)
            {
                case var _ when ManageQuizMenu.MenuKey == data && _prevCommand == MainMenu.MenuKey:
                    _prevCommand = ManageQuizMenu.MenuKey;
                    await bot.SendTextMessageAsync(telegramBotOptions.Value.AdminId, "Выберите действие:", replyMarkup: GenerateManageQuizMenu());
                    break;

                case var _ when ManageQuestionsMenu.MenuKey == data && _prevCommand == MainMenu.MenuKey:
                    _prevCommand = ManageQuestionsMenu.MenuKey;
                    await bot.SendTextMessageAsync(telegramBotOptions.Value.AdminId, "Выберите действие:", replyMarkup: _manageQuestions);
                    break;

                case var _ when ManagePlayersMenu.MenuKey == data && _prevCommand == MainMenu.MenuKey:
                    _prevCommand = MainMenu.MenuKey;
                    await bot.SendTextMessageAsync(telegramBotOptions.Value.AdminId, "Выберите действие:", replyMarkup: _mainMenu);
                    break;

                case var _ when BackMenu.MenuKey == data:
                    var newMenu = _prevCommand switch
                    {
                        _ when ShowListQuestionQuizMenu.MenuKey == _prevCommand => ManageQuestionsMenu.MenuKey,
                        _ when ShowPlayersQuizMenu.MenuKey == _prevCommand => ManagePlayersMenu.MenuKey,
                        _ => MainMenu.MenuKey
                    };
                    update.CallbackQuery.Data = newMenu;
                    break;

                case var _ when StartQuizMenu.MenuKey == data && _prevCommand == ManageQuizMenu.MenuKey:
                    await quizService.StartQuizAsync();
                    break;

                case var _ when NextStepQuizMenu.MenuKey == data && _prevCommand == ManageQuizMenu.MenuKey:
                    await quizService.NextStepAsync();
                    break;

                case var _ when PrevStepQuizMenu.MenuKey == data && _prevCommand == ManageQuizMenu.MenuKey:
                    await quizService.PreviousStepAsync();
                    break;

                case var _ when TopFiveQuizMenu.MenuKey == data && _prevCommand == ManageQuizMenu.MenuKey:
                    await quizService.ShowTopAsync(5);
                    break;

                case var _ when FullStatisticsQuizMenu.MenuKey == data && _prevCommand == ManageQuizMenu.MenuKey:
                    await quizService.ShowFullStatsAsync();
                    break;

                case "add_question" when _prevCommand == ManageQuestionsMenu.MenuKey:
                    // Запустить конверсацию для добавления вопроса
                    await quizService.InitiateQuestionCreationAsync(adminChatId);
                    break;

                case "get_question" when _prevCommand == ManageQuestionsMenu.MenuKey:
                    // Запустить конверсацию для добавления вопроса
                    await quizService.InitiateQuestionCreationAsync(adminChatId);
                    break;

                case "delete_question" when _prevCommand == ManageQuestionsMenu.MenuKey:
                    // Запустить конверсацию для добавления вопроса
                    await quizService.InitiateQuestionCreationAsync(adminChatId);
                    break;

                default:
                    await bot.SendTextMessageAsync(adminChatId, "Неизвестная команда.");
                    break;
            }
        }
    }
}
