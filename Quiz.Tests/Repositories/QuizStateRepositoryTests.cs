using Quiz.DataAccessLayer.Models;
using Quiz.DataAccessLayer.Repositories;
using Xunit;

namespace Quiz.Tests.Repositories;

public class QuizStateRepositoryTests : TestBase
{
    [Fact]
    public async Task GetByIdAsync_WithIncludes_ReturnsEntityWithQuestion()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new QuizStateRepository(context);
        
        var question = new QuizQuestionModel { Question = "Test Question", Order = 1 };
        await context.QuizQuestions.AddAsync(question);
        
        var state = new QuizStateModel
        {
            IsActive = true,
            State = QuizStateEnum.Question,
            QuizQuestion = question
        };
        
        await context.QuizStates.AddAsync(state);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(state.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(state.Id, result.Id);
        Assert.NotNull(result.QuizQuestion);
        Assert.Equal("Test Question", result.QuizQuestion.Question);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsStatesWithIncludes()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new QuizStateRepository(context);
        
        var question = new QuizQuestionModel { Question = "Test Question", Order = 1 };
        await context.QuizQuestions.AddAsync(question);
        
        var states = new List<QuizStateModel>
        {
            new() { IsActive = true, State = QuizStateEnum.Question, QuizQuestion = question },
            new() { IsActive = false, State = QuizStateEnum.Waiting, QuizQuestion = question }
        };
        
        await context.QuizStates.AddRangeAsync(states);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);
        Assert.All(resultList, s => Assert.NotNull(s.QuizQuestion));
    }

    [Fact]
    public async Task GetActiveStateAsync_ActiveStateExists_ReturnsActiveState()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new QuizStateRepository(context);
        
        var question = new QuizQuestionModel { Question = "Test Question", Order = 1 };
        await context.QuizQuestions.AddAsync(question);
        
        var states = new List<QuizStateModel>
        {
            new() { IsActive = false, State = QuizStateEnum.Waiting, QuizQuestion = question },
            new() { IsActive = true, State = QuizStateEnum.Question, QuizQuestion = question },
            new() { IsActive = false, State = QuizStateEnum.Statistics, QuizQuestion = question }
        };
        
        await context.QuizStates.AddRangeAsync(states);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetActiveStateAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsActive);
        Assert.Equal(QuizStateEnum.Question, result.State);
    }

    [Fact]
    public async Task GetActiveStateAsync_NoActiveState_ReturnsNull()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new QuizStateRepository(context);
        
        var question = new QuizQuestionModel { Question = "Test Question", Order = 1 };
        await context.QuizQuestions.AddAsync(question);
        
        var states = new List<QuizStateModel>
        {
            new() { IsActive = false, State = QuizStateEnum.Waiting, QuizQuestion = question },
            new() { IsActive = false, State = QuizStateEnum.Statistics, QuizQuestion = question }
        };
        
        await context.QuizStates.AddRangeAsync(states);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetActiveStateAsync();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetStatesByTypeAsync_ReturnsStatesOfSpecificType()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new QuizStateRepository(context);
        
        var question = new QuizQuestionModel { Question = "Test Question", Order = 1 };
        await context.QuizQuestions.AddAsync(question);
        
        var states = new List<QuizStateModel>
        {
            new() { IsActive = true, State = QuizStateEnum.Question, QuizQuestion = question },
            new() { IsActive = false, State = QuizStateEnum.Question, QuizQuestion = question },
            new() { IsActive = true, State = QuizStateEnum.Waiting, QuizQuestion = question },
            new() { IsActive = false, State = QuizStateEnum.Statistics, QuizQuestion = question }
        };
        
        await context.QuizStates.AddRangeAsync(states);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetStatesByTypeAsync(QuizStateEnum.Question);

        // Assert
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);
        Assert.All(resultList, s => Assert.Equal(QuizStateEnum.Question, s.State));
    }

    [Fact]
    public async Task GetStatesByTypeAsync_NoStatesOfType_ReturnsEmpty()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new QuizStateRepository(context);
        
        var question = new QuizQuestionModel { Question = "Test Question", Order = 1 };
        await context.QuizQuestions.AddAsync(question);
        
        var states = new List<QuizStateModel>
        {
            new() { IsActive = true, State = QuizStateEnum.Waiting, QuizQuestion = question },
            new() { IsActive = false, State = QuizStateEnum.Statistics, QuizQuestion = question }
        };
        
        await context.QuizStates.AddRangeAsync(states);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetStatesByTypeAsync(QuizStateEnum.Question);

        // Assert
        Assert.Empty(result);
    }
} 