using Quiz.DataAccessLayer.DTOs;
using Quiz.DataAccessLayer.Models;

namespace Quiz.WebAPI.Common.Dtos;

public record struct HandleDto(QuizStateEnum State, QuizQuestionDto Question, Dictionary<Guid, int> Statistics);
