using LightResults;
using Quiz.DataAccessLayer.DTOs;
using Quiz.DataAccessLayer.Interfaces;
using Quiz.DataAccessLayer.Models;
using Quiz.WebAPI.Exceptions;
using Quiz.WebAPI.Interfaces;

namespace Quiz.WebAPI.Common.States;

public abstract class BaseState(
    IQuizStateRepository stateRepository,
    IEnumerable<IQuizStateChangedHandler> handlers) : IQuizState
{
    public abstract QuizStateEnum State { get; }

    protected virtual async Task SetStateAsync(Guid quizQuestionId, CancellationToken ct = default)
    {
        var newState = await stateRepository.SetNewStateAsync(new QuizStateDto()
        {
            Id = Guid.Empty,
            State = State,
            QuizQuestionId = quizQuestionId,
        }, ct);

        if (newState.IsFailure())
            throw new RepositoryException(newState.Errors);
    }

    public virtual async Task OnEnterAsync(Guid quizQuestionId, CancellationToken ct = default)
    {
        await SetStateAsync(quizQuestionId, ct);
        foreach (var handler in handlers)
            await handler.HandleAsync(State, ct);
    }
}
