using Quiz.DataAccessLayer.Interfaces;
using Quiz.DataAccessLayer.Models;
using Quiz.WebAPI.Interfaces;

namespace Quiz.WebAPI.Common.States;

public sealed class WaitingForStartState(IQuizStateRepository repository,
    IEnumerable<IQuizStateChangedHandler> handlers) : BaseState(repository, handlers), IQuizState
{
    public override QuizStateEnum State => QuizStateEnum.WaitingForStart;

    public override async Task OnExitAsync()
    {
        throw new NotImplementedException();
    }
}
