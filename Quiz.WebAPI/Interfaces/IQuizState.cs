using Quiz.DataAccessLayer.DTOs;
using Quiz.DataAccessLayer.Models;
using Quiz.WebAPI.Common.Dtos;

namespace Quiz.WebAPI.Interfaces;

/// <summary>
/// Интерфейс для конкретных состояний викторины
/// </summary>
public interface IQuizState
{
    QuizStateEnum State { get; }
    Task OnEnterAsync(HandleDto handle, CancellationToken ct = default);
}
