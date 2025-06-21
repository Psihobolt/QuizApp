using Quiz.DataAccessLayer.Interfaces;
using Quiz.DataAccessLayer.Models;
using Quiz.WebAPI.Interfaces;

namespace Quiz.WebAPI.Common.States;

public sealed class DisplayAnswerStatsState(IQuizStateRepository stateRepository,
    IEnumerable<IQuizStateChangedHandler> handlers, IQuizAnswerRepository quizAnswerRepository) : BaseState(stateRepository, handlers), IQuizState
{
    public override QuizStateEnum State => QuizStateEnum.DisplayAnswerStats;
}
