namespace Quiz.DataAccessLayer.DTOs;

public readonly record struct QuizUserAnswerDto(
    Guid Id,
    Guid PlayerId,
    Guid QuizQuestionId,
    Guid AnswerId);