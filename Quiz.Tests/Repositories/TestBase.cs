using Microsoft.EntityFrameworkCore;
using Quiz.DataAccessLayer;

namespace Quiz.Tests.Repositories;

public abstract class TestBase
{
    protected QuizContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<QuizContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new QuizContext(options);
    }

    protected async Task<QuizContext> CreateContextWithDataAsync()
    {
        var context = CreateContext();
        await SeedDataAsync(context);
        return context;
    }

    protected virtual async Task SeedDataAsync(QuizContext context)
    {
        // Базовая реализация - переопределяется в наследниках
        await context.SaveChangesAsync();
    }
} 