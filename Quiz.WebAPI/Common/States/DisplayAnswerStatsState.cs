using Quiz.DataAccessLayer.Interfaces;
using Quiz.DataAccessLayer.Models;
using Quiz.WebAPI.Interfaces;

namespace Quiz.WebAPI.Common.States;

public sealed class DisplayAnswerStatsState(IQuizStateRepository repository,
    IEnumerable<IQuizStateChangedHandler> handlers) : BaseState(repository, handlers), IQuizState
{
    public override QuizStateEnum State => QuizStateEnum.DisplayAnswerStats;

    public override async Task OnExitAsync()
    {
        throw new NotImplementedException();
    }
}
