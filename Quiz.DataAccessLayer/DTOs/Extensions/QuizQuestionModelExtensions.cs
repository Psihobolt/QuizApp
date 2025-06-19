using Quiz.DataAccessLayer.Models;

namespace Quiz.DataAccessLayer.DTOs.Extensions;

public static class QuizQuestionModelExtensions
{
    public static QuizQuestionDto ToDto(this QuizQuestionModel model) =>
        new(model.Id, model.Question, model.MediaContent?.ToDto(), model.Order,
            model.Answers.Select(a => a.ToDto()).ToList());

    public static QuizQuestionModel FromDto(this QuizQuestionDto dto) => new()
    {
        Id = dto.Id,
        Question = dto.Question,
        MediaContent = dto.MediaContent?.FromDto(),
        Order = dto.Order,
        Answers = dto.Answers.Select(a => a.FromDto()).ToList()
    };
}
