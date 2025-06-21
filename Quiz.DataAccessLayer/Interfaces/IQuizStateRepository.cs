using LightResults;
using Quiz.DataAccessLayer.DTOs;

namespace Quiz.DataAccessLayer.Interfaces;

public interface IQuizStateRepository : IRepository<QuizStateDto>
{
    /// <summary>
    /// Получить текущее активное состояние викторины.
    /// </summary>
    Task<Result<QuizStateDto>> GetActiveStateAsync(CancellationToken ct = default);

    /// <summary>
    /// Создать и установить новое активное состояние викторины.
    /// Предыдущие состояния автоматически деактивируются.
    /// </summary>
    Task<Result<QuizStateDto>> SetNewStateAsync(QuizStateDto newState, CancellationToken ct = default);

    /// <summary>
    /// Завершает текущую активную сессию состояния викторины (IsActive = false).
    /// </summary>
    Task<Result> DeactivateCurrentAsync(CancellationToken ct = default);
}
