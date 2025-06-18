using Quiz.DataAccessLayer.Models;
using Quiz.DataAccessLayer.Repositories;
using Xunit;

namespace Quiz.Tests.Repositories;

public class QuizUserAnswerRepositoryTests : TestBase
{
    [Fact]
    public async Task GetByIdAsync_WithIncludes_ReturnsEntityWithAllRelations()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new QuizUserAnswerRepository(context);
        
        var user = new TelegramUsersModel
        {
            TelegramUserId = "123456789",
            Username = "testuser",
            CustomName = "Test User"
        };
        
        var question = new QuizQuestionModel { Question = "Test Question", Order = 1 };
        var answer = new QuizAnswerModel { Answer = "Test Answer", IsCorrect = true, Order = 1, QuizQuestion = question };
        
        await context.TelegramUsers.AddAsync(user);
        await context.QuizQuestions.AddAsync(question);
        await context.QuizAnswers.AddAsync(answer);
        
        var userAnswer = new QuizUserAnswerModel
        {
            Player = user,
            QuizQuestion = question,
            Answer = answer
        };
        
        await context.QuizUserAnswers.AddAsync(userAnswer);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(userAnswer.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userAnswer.Id, result.Id);
        Assert.NotNull(result.Player);
        Assert.Equal("testuser", result.Player.Username);
        Assert.NotNull(result.QuizQuestion);
        Assert.Equal("Test Question", result.QuizQuestion.Question);
        Assert.NotNull(result.Answer);
        Assert.Equal("Test Answer", result.Answer.Answer);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllUserAnswersWithIncludes()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new QuizUserAnswerRepository(context);
        
        var user = new TelegramUsersModel
        {
            TelegramUserId = "123456789",
            Username = "testuser",
            CustomName = "Test User"
        };
        
        var question = new QuizQuestionModel { Question = "Test Question", Order = 1 };
        var answer = new QuizAnswerModel { Answer = "Test Answer", IsCorrect = true, Order = 1, QuizQuestion = question };
        
        await context.TelegramUsers.AddAsync(user);
        await context.QuizQuestions.AddAsync(question);
        await context.QuizAnswers.AddAsync(answer);
        
        var userAnswers = new List<QuizUserAnswerModel>
        {
            new() { Player = user, QuizQuestion = question, Answer = answer },
            new() { Player = user, QuizQuestion = question, Answer = answer }
        };
        
        await context.QuizUserAnswers.AddRangeAsync(userAnswers);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);
        Assert.All(resultList, ua => Assert.NotNull(ua.Player));
        Assert.All(resultList, ua => Assert.NotNull(ua.QuizQuestion));
        Assert.All(resultList, ua => Assert.NotNull(ua.Answer));
    }

    [Fact]
    public async Task GetByPlayerIdAsync_ReturnsUserAnswersForPlayer()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new QuizUserAnswerRepository(context);
        
        var user1 = new TelegramUsersModel
        {
            TelegramUserId = "123456789",
            Username = "user1",
            CustomName = "User 1"
        };
        
        var user2 = new TelegramUsersModel
        {
            TelegramUserId = "987654321",
            Username = "user2",
            CustomName = "User 2"
        };
        
        var question = new QuizQuestionModel { Question = "Test Question", Order = 1 };
        var answer = new QuizAnswerModel { Answer = "Test Answer", IsCorrect = true, Order = 1, QuizQuestion = question };
        
        await context.TelegramUsers.AddRangeAsync(user1, user2);
        await context.QuizQuestions.AddAsync(question);
        await context.QuizAnswers.AddAsync(answer);
        
        var userAnswers = new List<QuizUserAnswerModel>
        {
            new() { Player = user1, QuizQuestion = question, Answer = answer },
            new() { Player = user1, QuizQuestion = question, Answer = answer },
            new() { Player = user2, QuizQuestion = question, Answer = answer }
        };
        
        await context.QuizUserAnswers.AddRangeAsync(userAnswers);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByPlayerIdAsync(user1.Id);

        // Assert
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);
        Assert.All(resultList, ua => Assert.Equal(user1.Id, ua.PlayerId));
    }

    [Fact]
    public async Task GetByQuestionIdAsync_ReturnsUserAnswersForQuestion()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new QuizUserAnswerRepository(context);
        
        var user = new TelegramUsersModel
        {
            TelegramUserId = "123456789",
            Username = "testuser",
            CustomName = "Test User"
        };
        
        var question1 = new QuizQuestionModel { Question = "Question 1", Order = 1 };
        var question2 = new QuizQuestionModel { Question = "Question 2", Order = 2 };
        var answer = new QuizAnswerModel { Answer = "Test Answer", IsCorrect = true, Order = 1, QuizQuestion = question1 };
        
        await context.TelegramUsers.AddAsync(user);
        await context.QuizQuestions.AddRangeAsync(question1, question2);
        await context.QuizAnswers.AddAsync(answer);
        
        var userAnswers = new List<QuizUserAnswerModel>
        {
            new() { Player = user, QuizQuestion = question1, Answer = answer },
            new() { Player = user, QuizQuestion = question1, Answer = answer },
            new() { Player = user, QuizQuestion = question2, Answer = answer }
        };
        
        await context.QuizUserAnswers.AddRangeAsync(userAnswers);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByQuestionIdAsync(question1.Id);

        // Assert
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);
        Assert.All(resultList, ua => Assert.Equal(question1.Id, ua.QuizQuestionId));
    }

    [Fact]
    public async Task GetByPlayerAndQuestionAsync_ExistingAnswer_ReturnsUserAnswer()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new QuizUserAnswerRepository(context);
        
        var user = new TelegramUsersModel
        {
            TelegramUserId = "123456789",
            Username = "testuser",
            CustomName = "Test User"
        };
        
        var question = new QuizQuestionModel { Question = "Test Question", Order = 1 };
        var answer = new QuizAnswerModel { Answer = "Test Answer", IsCorrect = true, Order = 1, QuizQuestion = question };
        
        await context.TelegramUsers.AddAsync(user);
        await context.QuizQuestions.AddAsync(question);
        await context.QuizAnswers.AddAsync(answer);
        
        var userAnswer = new QuizUserAnswerModel
        {
            Player = user,
            QuizQuestion = question,
            Answer = answer
        };
        
        await context.QuizUserAnswers.AddAsync(userAnswer);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByPlayerAndQuestionAsync(user.Id, question.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.PlayerId);
        Assert.Equal(question.Id, result.QuizQuestionId);
    }

    [Fact]
    public async Task GetByPlayerAndQuestionAsync_NonExistingAnswer_ReturnsNull()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new QuizUserAnswerRepository(context);

        // Act
        var result = await repository.GetByPlayerAndQuestionAsync(Guid.NewGuid(), Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task HasPlayerAnsweredQuestionAsync_PlayerAnswered_ReturnsTrue()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new QuizUserAnswerRepository(context);
        
        var user = new TelegramUsersModel
        {
            TelegramUserId = "123456789",
            Username = "testuser",
            CustomName = "Test User"
        };
        
        var question = new QuizQuestionModel { Question = "Test Question", Order = 1 };
        var answer = new QuizAnswerModel { Answer = "Test Answer", IsCorrect = true, Order = 1, QuizQuestion = question };
        
        await context.TelegramUsers.AddAsync(user);
        await context.QuizQuestions.AddAsync(question);
        await context.QuizAnswers.AddAsync(answer);
        
        var userAnswer = new QuizUserAnswerModel
        {
            Player = user,
            QuizQuestion = question,
            Answer = answer
        };
        
        await context.QuizUserAnswers.AddAsync(userAnswer);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.HasPlayerAnsweredQuestionAsync(user.Id, question.Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task HasPlayerAnsweredQuestionAsync_PlayerNotAnswered_ReturnsFalse()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new QuizUserAnswerRepository(context);

        // Act
        var result = await repository.HasPlayerAnsweredQuestionAsync(Guid.NewGuid(), Guid.NewGuid());

        // Assert
        Assert.False(result);
    }
} 