using Quiz.DataAccessLayer.Models;

namespace Quiz.DataAccessLayer.DTOs.Extensions;

public static class QuizUserAnswerModelExtensions
{
    public static QuizUserAnswerDto ToDto(this QuizUserAnswerModel model) =>
        new(model.Id, model.PlayerId, model.QuizQuestionId, model.AnswerId);

    public static QuizUserAnswerModel FromDto(this QuizUserAnswerDto dto) => new()
    {
        Id = dto.Id,
        PlayerId = dto.PlayerId,
        QuizQuestionId = dto.QuizQuestionId,
        AnswerId = dto.AnswerId
    };
}
