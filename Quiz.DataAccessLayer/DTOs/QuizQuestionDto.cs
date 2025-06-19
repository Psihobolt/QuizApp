namespace Quiz.DataAccessLayer.DTOs;

public readonly record struct QuizQuestionDto(
    Guid Id,
    string Question,
    MediaContentDto? MediaContent,
    int Order,
    IReadOnlyList<QuizAnswerDto> Answers);