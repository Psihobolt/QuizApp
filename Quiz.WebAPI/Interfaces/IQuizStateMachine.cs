using Quiz.DataAccessLayer.Models;

namespace Quiz.WebAPI.Interfaces;

public interface IQuizStateMachine
{
    /// <summary>
    /// Инициализация из БД. Вызывать при старте приложения.
    /// </summary>
    public Task InitializeAsync();

    /// <summary>
    /// Переход в новое состояние
    /// </summary>
    public Task ChangeStateAsync(QuizStateEnum next);
}
