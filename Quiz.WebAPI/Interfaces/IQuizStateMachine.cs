using Quiz.DataAccessLayer.DTOs;
using Quiz.DataAccessLayer.Models;
using Quiz.WebAPI.Common.Dtos;

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
    public Task ChangeStateAsync(HandleDto handle);
}
