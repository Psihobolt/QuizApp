using LightResults;
using Quiz.DataAccessLayer.DTOs;
using Quiz.DataAccessLayer.Interfaces;
using Quiz.DataAccessLayer.Models;
using Quiz.WebAPI.Exceptions;
using Quiz.WebAPI.Interfaces;

namespace Quiz.WebAPI.Common.States;

public abstract class BaseState(
    IQuizStateRepository repository,
        IEnumerable<IQuizStateChangedHandler> handlers) : IQuizState
{
    public abstract QuizStateEnum State { get; }

    protected virtual async Task SetStateAsync(Guid quizQuestionId)
    {
        var newState = await repository.SetNewStateAsync(new QuizStateDto()
        {
            Id = Guid.Empty,
            State = State,
            QuizQuestionId = quizQuestionId,
        });

        if (newState.IsFailure(out var errors))
            throw new RepositoryException(errors);
    }

    public virtual async Task OnEnterAsync(Guid quizQuestionId)
    {
        await SetStateAsync(quizQuestionId);
        foreach (var handler in handlers)
            await handler.HandleAsync(State);
    }

    public abstract Task OnExitAsync();
}
