using Quiz.DataAccessLayer.Models;

namespace Quiz.DataAccessLayer.DTOs.Extensions;

public static class QuizStateModelExtensions
{
    public static QuizStateDto ToDto(this QuizStateModel model) =>
        new(model.Id, model.IsActive, model.State, model.QuizQuestionId);

    public static QuizStateModel FromDto(this QuizStateDto dto) => new()
    {
        Id = dto.Id,
        IsActive = dto.IsActive,
        State = dto.State,
        QuizQuestionId = dto.QuizQuestionId
    };
}
