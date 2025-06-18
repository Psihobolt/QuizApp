using Quiz.DataAccessLayer.Models;
using Quiz.DataAccessLayer.Repositories;
using Xunit;

namespace Quiz.Tests.Repositories;

public class QuizQuestionRepositoryTests : TestBase
{
    [Fact]
    public async Task GetByIdAsync_WithIncludes_ReturnsEntityWithAnswersAndMedia()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new QuizQuestionRepository(context);
        
        var mediaContent = new MediaContentModel { Data = new byte[] { 1, 2, 3 } };
        await context.MediaContents.AddAsync(mediaContent);
        
        var question = new QuizQuestionModel
        {
            Question = "Test Question",
            Order = 1,
            MediaContent = mediaContent,
            Answers = new List<QuizAnswerModel>
            {
                new() { Answer = "Answer 1", IsCorrect = true, Order = 1 },
                new() { Answer = "Answer 2", IsCorrect = false, Order = 2 }
            }
        };
        
        await context.QuizQuestions.AddAsync(question);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(question.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(question.Id, result.Id);
        Assert.NotNull(result.Answers);
        Assert.Equal(2, result.Answers.Count);
        Assert.NotNull(result.MediaContent);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsOrderedQuestionsWithIncludes()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new QuizQuestionRepository(context);
        
        var questions = new List<QuizQuestionModel?>
        {
            new() { Question = "Question 3", Order = 3 },
            new() { Question = "Question 1", Order = 1 },
            new() { Question = "Question 2", Order = 2 }
        };
        
        await context.QuizQuestions.AddRangeAsync(questions);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        var resultList = result.ToList();
        Assert.Equal(3, resultList.Count);
        Assert.Equal("Question 1", resultList[0].Question);
        Assert.Equal("Question 2", resultList[1].Question);
        Assert.Equal("Question 3", resultList[2].Question);
    }

    [Fact]
    public async Task GetByOrderAsync_ExistingOrder_ReturnsQuestion()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new QuizQuestionRepository(context);
        
        var question = new QuizQuestionModel
        {
            Question = "Test Question",
            Order = 5,
            Answers = new List<QuizAnswerModel>
            {
                new() { Answer = "Answer 2", IsCorrect = false, Order = 2 },
                new() { Answer = "Answer 1", IsCorrect = true, Order = 1 }
            }
        };
        
        await context.QuizQuestions.AddAsync(question);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByOrderAsync(5);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.Order);
        Assert.Equal(2, result.Answers.Count);
        // Проверяем, что ответы отсортированы по Order
        Assert.Equal("Answer 1", result.Answers.First().Answer);
        Assert.Equal("Answer 2", result.Answers.Last().Answer);
    }

    [Fact]
    public async Task GetByOrderAsync_NonExistingOrder_ReturnsNull()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new QuizQuestionRepository(context);

        // Act
        var result = await repository.GetByOrderAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetQuestionsWithAnswersAsync_ReturnsQuestionsWithOrderedAnswers()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new QuizQuestionRepository(context);
        
        var questions = new List<QuizQuestionModel?>
        {
            new()
            {
                Question = "Question 1",
                Order = 1,
                Answers = new List<QuizAnswerModel>
                {
                    new() { Answer = "Answer 2", IsCorrect = false, Order = 2 },
                    new() { Answer = "Answer 1", IsCorrect = true, Order = 1 }
                }
            },
            new()
            {
                Question = "Question 2",
                Order = 2,
                Answers = new List<QuizAnswerModel>
                {
                    new() { Answer = "Answer 3", IsCorrect = true, Order = 1 }
                }
            }
        };
        
        await context.QuizQuestions.AddRangeAsync(questions);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetQuestionsWithAnswersAsync();

        // Assert
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);
        
        // Проверяем порядок вопросов
        Assert.Equal("Question 1", resultList[0].Question);
        Assert.Equal("Question 2", resultList[1].Question);
        
        // Проверяем порядок ответов в первом вопросе
        var firstQuestionAnswers = resultList[0].Answers.ToList();
        Assert.Equal("Answer 1", firstQuestionAnswers[0].Answer);
        Assert.Equal("Answer 2", firstQuestionAnswers[1].Answer);
    }
} 