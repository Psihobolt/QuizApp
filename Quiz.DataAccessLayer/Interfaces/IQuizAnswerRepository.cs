using LightResults;
using Quiz.DataAccessLayer.DTOs;

namespace Quiz.DataAccessLayer.Interfaces;

public interface IQuizAnswerRepository : IRepository<QuizAnswerDto>
{
    /// <summary>
    /// Получить список всех ответов для указанного вопроса
    /// </summary>
    Task<Result<List<QuizAnswerDto>>> GetByQuestionIdAsync(Guid quizQuestionId, CancellationToken ct = default);

    /// <summary>
    /// Получить список правильных ответов для вопроса
    /// </summary>
    Task<Result<List<QuizAnswerDto>>> GetCorrectAnswersByQuestionIdAsync(Guid quizQuestionId, CancellationToken ct = default);
}
