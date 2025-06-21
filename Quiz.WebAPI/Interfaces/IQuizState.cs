using Quiz.DataAccessLayer.DTOs;
using Quiz.DataAccessLayer.Models;

namespace Quiz.WebAPI.Interfaces;

/// <summary>
/// Интерфейс для конкретных состояний викторины
/// </summary>
public interface IQuizState
{
    QuizStateEnum State { get; }
    Task OnEnterAsync(Guid quizQuestionId);
    Task OnExitAsync();
}
