using Quiz.DataAccessLayer.DTOs;
using Quiz.DataAccessLayer.Interfaces;
using Quiz.DataAccessLayer.Models;
using Quiz.WebAPI.Exceptions;
using Quiz.WebAPI.Interfaces;

namespace Quiz.WebAPI.Services;

public class QuizService(IEnumerable<IQuizStateChangedHandler> handlers,
    IQuizStateMachine stateMachine,
    IQuizStateRepository stateRepository,
    IQuizQuestionRepository questionRepository,
    IQuizUserAnswerRepository userAnswerRepository) : IQuizService
{
    private async Task<QuizQuestionDto> GetNextQuestionAsync(QuizStateDto stateDto)
    {
        var nextQuestion = stateDto.State is QuizStateEnum.WaitingForStart
            ? await questionRepository.GetFirstQuestionAsync()
            : await questionRepository.GetNextQuestionAsync(stateDto.QuizQuestionId);

        if (!nextQuestion.IsSuccess(out var nextState))
            throw new RepositoryException(nextQuestion.Errors);

        return nextState;
    }

    public async Task<(QuizStateEnum state, int orderQuestion)> StartQuizAsync() => await NextStepQuizAsync();

    public async Task<(QuizStateEnum state, int orderQuestion)> NextStepQuizAsync()
    {
        var stepResult = await stateRepository.GetActiveStateAsync(CancellationToken.None);
        if (!stepResult.IsSuccess(out var step)) throw new RepositoryException(stepResult.Errors);
        QuizStateEnum nextState = QuizStateEnum.WaitingForStart;
        int nextOrder = step.QuizQuestion.Order;
        var question = await GetNextQuestionAsync(step);

        switch (step.State)
        {
            case QuizStateEnum.WaitingForStart:
                await QuestionHandleAsync(question, CancellationToken.None);

                nextState = QuizStateEnum.DisplayQuestion;
                break;
            case QuizStateEnum.DisplayQuestion:
                await QuestionHandleAsync(question, CancellationToken.None);

                nextState = QuizStateEnum.DisplayAnswerStats;
                nextOrder = step.QuizQuestion.Order;
                break;
            case QuizStateEnum.DisplayAnswerStats:
                var statisticsResult = await userAnswerRepository.GetAnswerStatisticsByQuestionIdAsync(question.Id);
                if (!statisticsResult.IsSuccess(out var statistics)) throw new RepositoryException(statisticsResult.Errors);

                await StatisticsHandleAsync(question, statistics, CancellationToken.None);
                nextState = QuizStateEnum.DisplayCorrectAnswer;
                break;
            case QuizStateEnum.DisplayCorrectAnswer:
                await AnswerHandleAsync(question, CancellationToken.None);

                nextState = QuizStateEnum.DisplayQuestion;
                break;
            case QuizStateEnum.DisplayFullStats:
                var playerFullStatisticsResult = await userAnswerRepository.GetListCountCorrectAnswersGroupedByPlayerIdAsync(CancellationToken.None);
                if (!playerFullStatisticsResult.IsSuccess(out var playerFullStatistics)) throw new RepositoryException(playerFullStatisticsResult.Errors);
                await PlayerStatisticsHandleAsync(question, playerFullStatistics, CancellationToken.None);

                nextState = QuizStateEnum.DisplayQuestion;
                break;
            case QuizStateEnum.DisplayTop5:
                var playerStatisticsResult = await userAnswerRepository.GetListCountCorrectAnswersGroupedByPlayerIdAsync(CancellationToken.None);
                if (!playerStatisticsResult.IsSuccess(out var playerStatistics)) throw new RepositoryException(playerStatisticsResult.Errors);
                var topFive = playerStatistics.OrderByDescending(x=>x.Value).Take(5).ToDictionary(x=>x.Key,x=>x.Value);
                await TopFiveHandleAsync(question, topFive, CancellationToken.None);

                nextState = QuizStateEnum.DisplayQuestion;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return (nextState, nextOrder);
    }

    public async Task<(QuizStateEnum state, int orderQuestion)> PreviousStepQuizAsync()
    {
        throw new NotImplementedException();
    }

    public async Task ShowTopFiveResultsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task ShowFullStatsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<QuizStateDto> GetActiveStateAsync()
    {
        var stepResult = await stateRepository.GetActiveStateAsync(CancellationToken.None);
        if (!stepResult.IsSuccess(out var step)) throw new RepositoryException(stepResult.Errors);

        return step;
    }

    private async Task QuestionHandleAsync(QuizQuestionDto question, CancellationToken cancellationToken)
    {
        await stateRepository.SetNewStateAsync(new QuizStateDto(Guid.NewGuid(), QuizStateEnum.DisplayQuestion, question.Id, question), cancellationToken);
        foreach (var handler in handlers)
            await handler.QuestionHandleAsync(question, cancellationToken);
    }

    private async Task StatisticsHandleAsync(QuizQuestionDto question, Dictionary<Guid, int> statistics, CancellationToken cancellationToken)
    {
        await stateRepository.SetNewStateAsync(new QuizStateDto(Guid.NewGuid(), QuizStateEnum.DisplayAnswerStats, question.Id, question), cancellationToken);
        foreach (var handler in handlers)
            await handler.StatisticsHandleAsync(question, statistics, cancellationToken);
    }

    private async Task AnswerHandleAsync(QuizQuestionDto question, CancellationToken cancellationToken)
    {
        await stateRepository.SetNewStateAsync(new QuizStateDto(Guid.NewGuid(), QuizStateEnum.DisplayCorrectAnswer, question.Id, question), cancellationToken);
        foreach (var handler in handlers)
            await handler.AnswerHandleAsync(question, cancellationToken);
    }

    private async Task PlayerStatisticsHandleAsync(QuizQuestionDto question, Dictionary<TelegramUserDto, int> userStat, CancellationToken cancellationToken)
    {
        await stateRepository.SetNewStateAsync(new QuizStateDto(Guid.NewGuid(), QuizStateEnum.DisplayFullStats, question.Id, question), cancellationToken);
        foreach (var handler in handlers)
            await handler.PlayerStatisticsHandleAsync(question, userStat, cancellationToken);
    }

    private async Task TopFiveHandleAsync(QuizQuestionDto question, Dictionary<TelegramUserDto, int> userStat, CancellationToken cancellationToken)
    {
        await stateRepository.SetNewStateAsync(new QuizStateDto(Guid.NewGuid(), QuizStateEnum.DisplayTop5, question.Id, question), cancellationToken);
        foreach (var handler in handlers)
            await handler.TopFiveHandleAsync(question, userStat, cancellationToken);
    }
}

public interface IQuizService
{
    /// <summary>
    /// Начинает викторину
    /// <remarks>Регистрация после начала игры прекращается</remarks>
    /// </summary>
    public Task<(QuizStateEnum state, int orderQuestion)> StartQuizAsync();

    /// <summary>
    /// Переход к следующему шагу
    /// </summary>
    public Task<(QuizStateEnum state, int orderQuestion)> NextStepQuizAsync();

    /// <summary>
    /// Переход к предыдущему шагу
    /// </summary>
    public Task<(QuizStateEnum state, int orderQuestion)> PreviousStepQuizAsync();

    /// <summary>
    /// Переход к топ 5 результатов на текущий момент
    /// </summary>
    public Task ShowTopFiveResultsAsync();

    /// <summary>
    /// Переход к статистике за игру на текущий момент
    /// </summary>
    public Task ShowFullStatsAsync();

    public Task<QuizStateDto> GetActiveStateAsync();
}
