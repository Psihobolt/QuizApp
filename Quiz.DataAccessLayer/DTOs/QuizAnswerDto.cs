namespace Quiz.DataAccessLayer.DTOs;

public readonly record struct QuizAnswerDto(
    Guid Id,
    Guid QuizQuestionId,
    bool IsCorrect,
    string Answer,
    Guid? MediaContentId,
    MediaContentDto? MediaContent,
    int Order);