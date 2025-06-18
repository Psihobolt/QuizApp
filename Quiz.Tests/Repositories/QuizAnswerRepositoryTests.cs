using Quiz.DataAccessLayer.Models;
using Quiz.DataAccessLayer.Repositories;
using Xunit;

namespace Quiz.Tests.Repositories;

public class QuizAnswerRepositoryTests : TestBase
{
    [Fact]
    public async Task GetByIdAsync_WithIncludes_ReturnsEntityWithQuestionAndMedia()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new QuizAnswerRepository(context);
        
        var question = new QuizQuestionModel { Question = "Test Question", Order = 1 };
        var mediaContent = new MediaContentModel { Data = new byte[] { 1, 2, 3 } };
        
        await context.QuizQuestions.AddAsync(question);
        await context.MediaContents.AddAsync(mediaContent);
        
        var answer = new QuizAnswerModel
        {
            Answer = "Test Answer",
            IsCorrect = true,
            Order = 1,
            QuizQuestion = question,
            MediaContent = mediaContent
        };
        
        await context.QuizAnswers.AddAsync(answer);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(answer.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(answer.Id, result.Id);
        Assert.NotNull(result.QuizQuestion);
        Assert.Equal("Test Question", result.QuizQuestion.Question);
        Assert.NotNull(result.MediaContent);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsOrderedAnswersWithIncludes()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new QuizAnswerRepository(context);
        
        var question1 = new QuizQuestionModel { Question = "Question 1", Order = 1 };
        var question2 = new QuizQuestionModel { Question = "Question 2", Order = 2 };
        
        await context.QuizQuestions.AddRangeAsync(question1, question2);
        
        var answers = new List<QuizAnswerModel>
        {
            new() { Answer = "Answer 2", IsCorrect = false, Order = 2, QuizQuestion = question1 },
            new() { Answer = "Answer 1", IsCorrect = true, Order = 1, QuizQuestion = question1 },
            new() { Answer = "Answer 3", IsCorrect = true, Order = 1, QuizQuestion = question2 }
        };
        
        await context.QuizAnswers.AddRangeAsync(answers);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        var resultList = result.ToList();
        Assert.Equal(3, resultList.Count);
        
        // Проверяем порядок: сначала по QuizQuestionId, затем по Order
        Assert.Equal("Answer 1", resultList[0].Answer); // Question 1, Order 1
        Assert.Equal("Answer 2", resultList[1].Answer); // Question 1, Order 2
        Assert.Equal("Answer 3", resultList[2].Answer); // Question 2, Order 1
    }

    [Fact]
    public async Task GetByQuestionIdAsync_ReturnsAnswersForQuestion()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new QuizAnswerRepository(context);
        
        var question1 = new QuizQuestionModel { Question = "Question 1", Order = 1 };
        var question2 = new QuizQuestionModel { Question = "Question 2", Order = 2 };
        
        await context.QuizQuestions.AddRangeAsync(question1, question2);
        
        var answers = new List<QuizAnswerModel>
        {
            new() { Answer = "Answer 1", IsCorrect = true, Order = 1, QuizQuestion = question1 },
            new() { Answer = "Answer 2", IsCorrect = false, Order = 2, QuizQuestion = question1 },
            new() { Answer = "Answer 3", IsCorrect = true, Order = 1, QuizQuestion = question2 }
        };
        
        await context.QuizAnswers.AddRangeAsync(answers);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByQuestionIdAsync(question1.Id);

        // Assert
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);
        Assert.All(resultList, a => Assert.Equal(question1.Id, a.QuizQuestionId));
        Assert.Equal("Answer 1", resultList[0].Answer); // Order 1
        Assert.Equal("Answer 2", resultList[1].Answer); // Order 2
    }

    [Fact]
    public async Task GetByQuestionIdAsync_NonExistingQuestion_ReturnsEmpty()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new QuizAnswerRepository(context);

        // Act
        var result = await repository.GetByQuestionIdAsync(Guid.NewGuid());

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetCorrectAnswersByQuestionIdAsync_ReturnsOnlyCorrectAnswers()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new QuizAnswerRepository(context);
        
        var question = new QuizQuestionModel { Question = "Test Question", Order = 1 };
        await context.QuizQuestions.AddAsync(question);
        
        var answers = new List<QuizAnswerModel>
        {
            new() { Answer = "Correct Answer 1", IsCorrect = true, Order = 1, QuizQuestion = question },
            new() { Answer = "Wrong Answer", IsCorrect = false, Order = 2, QuizQuestion = question },
            new() { Answer = "Correct Answer 2", IsCorrect = true, Order = 3, QuizQuestion = question }
        };
        
        await context.QuizAnswers.AddRangeAsync(answers);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetCorrectAnswersByQuestionIdAsync(question.Id);

        // Assert
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);
        Assert.All(resultList, a => Assert.True(a.IsCorrect));
        Assert.Equal("Correct Answer 1", resultList[0].Answer);
        Assert.Equal("Correct Answer 2", resultList[1].Answer);
    }

    [Fact]
    public async Task GetCorrectAnswersByQuestionIdAsync_NoCorrectAnswers_ReturnsEmpty()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new QuizAnswerRepository(context);
        
        var question = new QuizQuestionModel { Question = "Test Question", Order = 1 };
        await context.QuizQuestions.AddAsync(question);
        
        var answers = new List<QuizAnswerModel>
        {
            new() { Answer = "Wrong Answer 1", IsCorrect = false, Order = 1, QuizQuestion = question },
            new() { Answer = "Wrong Answer 2", IsCorrect = false, Order = 2, QuizQuestion = question }
        };
        
        await context.QuizAnswers.AddRangeAsync(answers);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetCorrectAnswersByQuestionIdAsync(question.Id);

        // Assert
        Assert.Empty(result);
    }
} 