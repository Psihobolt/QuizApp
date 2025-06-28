using LightResults;
using Quiz.DataAccessLayer.DTOs;

namespace Quiz.DataAccessLayer.Interfaces;

public interface IQuizUserAnswerRepository : IRepository<QuizUserAnswerDto>
{
    /// <summary>
    /// Возвращает список ответов игрока по его идентификатору.
    /// </summary>
    Task<Result<List<QuizUserAnswerDto>>> GetAnswersByPlayerIdAsync(Guid playerId, CancellationToken ct = default);

    /// <summary>
    /// Возвращает количество ответов игроков, сгруппированных по ID ответа для конкретного вопроса.
    /// </summary>
    Task<Result<Dictionary<Guid, int>>> GetAnswerStatisticsByQuestionIdAsync(Guid questionId, CancellationToken ct = default);

    /// <summary>
    /// Возвращает количество правильных ответов игрока.
    /// </summary>
    Task<Result<int>> GetCorrectAnswerCountByPlayerIdAsync(Guid playerId, CancellationToken ct = default);

    /// <summary>
    /// Проверяет, был ли уже дан ответ игроком на данный вопрос.
    /// </summary>
    Task<Result<bool>> HasPlayerAnsweredAsync(Guid playerId, Guid questionId, CancellationToken ct = default);

    Task<Result<Dictionary<TelegramUserDto, int>>> GetListCountCorrectAnswersGroupedByPlayerIdAsync(CancellationToken ct = default);
}
