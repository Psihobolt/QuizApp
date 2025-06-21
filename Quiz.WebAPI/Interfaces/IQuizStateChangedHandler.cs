using Quiz.DataAccessLayer.DTOs;
using Quiz.DataAccessLayer.Models;

namespace Quiz.WebAPI.Interfaces;

/// <summary>
/// Обработчик изменения состояния (Mediator pattern)
/// </summary>
public interface IQuizStateChangedHandler
{
    Task HandleAsync(QuizStateEnum newState, CancellationToken ct = default);
}
