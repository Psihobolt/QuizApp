using LightResults;
using Quiz.DataAccessLayer.DTOs;

namespace Quiz.DataAccessLayer.Interfaces;

public interface IQuizQuestionRepository : IRepository<QuizQuestionDto>
{
    /// <summary>
    /// Получить вопрос по порядковому номеру.
    /// </summary>
    Task<Result<QuizQuestionDto>> GetByOrderAsync(int order);

    /// <summary>
    /// Получить следующий вопрос после указанного порядкового номера.
    /// </summary>
    Task<Result<QuizQuestionDto>> GetNextQuestionAsync(int currentOrder);

    /// <summary>
    /// Получить самый первый вопрос.
    /// </summary>
    Task<Result<QuizQuestionDto>> GetFirstQuestionAsync();

    /// <summary>
    /// Получить все вопросы с ответами и медиа.
    /// </summary>
    Task<Result<List<QuizQuestionDto>>> GetAllWithAnswersAsync();
}
