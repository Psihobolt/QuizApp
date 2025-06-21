using Quiz.DataAccessLayer.Models;

namespace Quiz.DataAccessLayer.DTOs;

public readonly record struct QuizStateDto(Guid Id, QuizStateEnum State, Guid QuizQuestionId, bool IsActive = true);
