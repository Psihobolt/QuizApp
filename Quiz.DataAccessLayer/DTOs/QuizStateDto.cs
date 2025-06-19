using Quiz.DataAccessLayer.Models;

namespace Quiz.DataAccessLayer.DTOs;

public readonly record struct QuizStateDto(
    Guid Id,
    bool IsActive,
    QuizStateEnum State,
    Guid QuizQuestionId);