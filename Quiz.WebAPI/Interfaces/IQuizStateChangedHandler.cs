using Quiz.DataAccessLayer.DTOs;
using Quiz.DataAccessLayer.Models;
using Quiz.WebAPI.Common.Dtos;

namespace Quiz.WebAPI.Interfaces;

/// <summary>
/// Обработчик изменения состояния (Mediator pattern)
/// </summary>
public interface IQuizStateChangedHandler
{
    Task QuestionHandleAsync(QuizQuestionDto quizQuestionDto, CancellationToken ct = default);
    Task StatisticsHandleAsync(QuizQuestionDto quizQuestionDto, Dictionary<Guid, int> statistics, CancellationToken ct = default);
    Task AnswerHandleAsync(QuizQuestionDto quizQuestionDto, CancellationToken ct = default);
    Task TopFiveHandleAsync(QuizQuestionDto quizQuestionDto, Dictionary<TelegramUserDto, int> userStat, CancellationToken ct = default);
    Task PlayerStatisticsHandleAsync(QuizQuestionDto quizQuestionDto, Dictionary<TelegramUserDto, int> userStat, CancellationToken ct = default);
}
