using Quiz.DataAccessLayer.DTOs;
using Quiz.DataAccessLayer.Interfaces;
using Quiz.DataAccessLayer.Models;
using Quiz.WebAPI.Exceptions;
using Quiz.WebAPI.Interfaces;

namespace Quiz.WebAPI.Common.States;

public sealed class DisplayQuestionState(IQuizStateRepository stateRepository,
    IEnumerable<IQuizStateChangedHandler> handlers,
    IQuizQuestionRepository questionRepository) : BaseState(stateRepository, handlers), IQuizState
{
    private readonly IQuizStateRepository _stateRepository = stateRepository;
    public override QuizStateEnum State => QuizStateEnum.DisplayQuestion;

    public override async Task OnEnterAsync(Guid quizQuestionId, CancellationToken ct = default)
    {
        var previousState = await _stateRepository.GetActiveStateAsync(ct);
        if (!previousState.IsSuccess(out var dto))
            throw new RepositoryException(previousState.Errors);

        var nextQuestion = dto.State is QuizStateEnum.WaitingForStart
            ? await questionRepository.GetFirstQuestionAsync()
            : await questionRepository.GetNextQuestionAsync(dto.QuizQuestionId);

        if (!nextQuestion.IsSuccess(out var nextState))
            throw new RepositoryException(nextQuestion.Errors);

        await base.OnEnterAsync(nextState.Id, ct);
    }
}
