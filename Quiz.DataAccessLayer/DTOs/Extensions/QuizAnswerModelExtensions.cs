using Quiz.DataAccessLayer.Models;

namespace Quiz.DataAccessLayer.DTOs.Extensions;

public static class QuizAnswerModelExtensions
{
    public static QuizAnswerDto ToDto(this QuizAnswerModel model) =>
        new(model.Id, model.QuizQuestionId, model.IsCorrect, model.Answer, model.MediaContentId, model.MediaContent?.ToDto(), model.Order);

    public static QuizAnswerModel FromDto(this QuizAnswerDto dto) => new()
    {
        Id = dto.Id,
        QuizQuestionId = dto.QuizQuestionId,
        IsCorrect = dto.IsCorrect,
        Answer = dto.Answer,
        MediaContentId = dto.MediaContentId,
        MediaContent = dto.MediaContent?.FromDto(),
        Order = dto.Order
    };
}
