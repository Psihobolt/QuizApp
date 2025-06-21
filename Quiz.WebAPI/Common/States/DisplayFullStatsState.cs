using Quiz.DataAccessLayer.DTOs;
using Quiz.DataAccessLayer.Interfaces;
using Quiz.DataAccessLayer.Models;
using Quiz.WebAPI.Interfaces;

namespace Quiz.WebAPI.Common.States;

public sealed class DisplayFullStatsState(IQuizStateRepository repository,
    IEnumerable<IQuizStateChangedHandler> handlers) : BaseState(repository, handlers), IQuizState
{
    public override QuizStateEnum State => QuizStateEnum.DisplayFullStats;

    public override Task OnExitAsync() => Task.CompletedTask;
}
