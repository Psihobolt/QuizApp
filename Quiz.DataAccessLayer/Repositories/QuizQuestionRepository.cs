using LightResults;
using Microsoft.EntityFrameworkCore;
using Quiz.DataAccessLayer.DTOs;
using Quiz.DataAccessLayer.DTOs.Extensions;
using Quiz.DataAccessLayer.Interfaces;

namespace Quiz.DataAccessLayer.Repositories;

public sealed class QuizQuestionRepository(QuizContext context)
    : IQuizQuestionRepository
{
    public async Task<Result<QuizQuestionDto>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await context.QuizQuestions
            .AsNoTracking()
            .Include(q => q.Answers)
            .ThenInclude(a => a.MediaContent)
            .Include(q => q.MediaContent)
            .FirstOrDefaultAsync(q => q.Id == id, ct);

        return entity is null
            ? Result.Failure<QuizQuestionDto>("Вопрос не найден")
            : Result.Success(entity.ToDto());
    }

    public async Task<Result> AddAsync(QuizQuestionDto dto, CancellationToken ct = default)
    {
        var entity = dto.FromDto();
        try
        {
            await context.QuizQuestions.AddAsync(entity, ct);
            await context.SaveChangesAsync(ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    public async Task<Result> UpdateAsync(QuizQuestionDto dto, CancellationToken ct = default)
    {
        var existing = await context.QuizQuestions
            .Include(q => q.Answers)
            .FirstOrDefaultAsync(q => q.Id == dto.Id, ct);

        if (existing is null)
            return Result.Failure("Вопрос не найден");

        existing.Question = dto.Question;
        existing.Order = dto.Order;
        existing.MediaContentId = dto.Id;

        try
        {
            context.QuizQuestions.Update(existing);
            await context.SaveChangesAsync(ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await context.QuizQuestions
            .FirstOrDefaultAsync(q => q.Id == id, ct);

        if (entity is null)
            return Result.Failure("Вопрос не найден");

        context.QuizQuestions.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Result.Success();
    }

    public async Task<Result<QuizQuestionDto>> GetByOrderAsync(int order)
    {
        var entity = await context.QuizQuestions
            .AsNoTracking()
            .Include(q => q.Answers)
            .ThenInclude(a => a.MediaContent)
            .Include(q => q.MediaContent)
            .FirstOrDefaultAsync(q => q.Order == order);

        return entity is null
            ? Result.Failure<QuizQuestionDto>("Вопрос не найден")
            : Result.Success(entity.ToDto());
    }

    public async Task<Result<QuizQuestionDto>> GetNextQuestionAsync(int currentOrder)
    {
        var entity = await context.QuizQuestions
            .AsNoTracking()
            .Include(q => q.Answers)
            .ThenInclude(a => a.MediaContent)
            .Include(q => q.MediaContent)
            .Where(q => q.Order > currentOrder)
            .OrderBy(q => q.Order)
            .FirstOrDefaultAsync();

        return entity is null
            ? Result.Failure<QuizQuestionDto>("Следующий вопрос не найден")
            : Result.Success(entity.ToDto());
    }

    public async Task<Result<QuizQuestionDto>> GetFirstQuestionAsync()
    {
        var entity = await context.QuizQuestions
            .AsNoTracking()
            .Include(q => q.Answers)
            .ThenInclude(a => a.MediaContent)
            .Include(q => q.MediaContent)
            .OrderBy(q => q.Order)
            .FirstOrDefaultAsync();

        return entity is null
            ? Result.Failure<QuizQuestionDto>("Вопросов пока нет")
            : Result.Success(entity.ToDto());
    }

    public async Task<Result<List<QuizQuestionDto>>> GetAllWithAnswersAsync()
    {
        var items = await context.QuizQuestions
            .AsNoTracking()
            .Include(q => q.Answers)
            .ThenInclude(a => a.MediaContent)
            .Include(q => q.MediaContent)
            .OrderBy(q => q.Order)
            .ToListAsync();

        var dtos = items.ConvertAll(q => q.ToDto());
        return Result.Success(dtos);
    }
}
