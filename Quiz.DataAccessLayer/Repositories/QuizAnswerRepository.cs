using LightResults;
using Microsoft.EntityFrameworkCore;
using Quiz.DataAccessLayer.DTOs;
using Quiz.DataAccessLayer.DTOs.Extensions;
using Quiz.DataAccessLayer.Interfaces;

namespace Quiz.DataAccessLayer.Repositories;

public sealed class QuizAnswerRepository(QuizContext context) : IQuizAnswerRepository
{
    public async Task<Result<List<QuizAnswerDto>>> GetAllAsync(CancellationToken ct = default)
    {
        try
        {
            var items = await context.QuizAnswers
                .AsNoTracking()
                .ToListAsync(ct);

            return items.Select(x => x.ToDto()).ToList();
        }
        catch (Exception ex)
        {
            return Result.Failure<List<QuizAnswerDto>>(ex.Message);
        }
    }

    public async Task<Result<QuizAnswerDto>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        try
        {
            var entity = await context.QuizAnswers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, ct);

            return entity is not null
                ? Result.Success(entity.ToDto())
                : Result.Failure<QuizAnswerDto>("Ответ не найден");
        }
        catch (Exception ex)
        {
            return Result.Failure<QuizAnswerDto>(ex.Message);
        }
    }

    public async Task<Result> AddAsync(QuizAnswerDto item, CancellationToken ct = default)
    {
        try
        {
            var entity = item.FromDto();
            await context.QuizAnswers.AddAsync(entity, ct);
            await context.SaveChangesAsync(ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    public async Task<Result> UpdateAsync(QuizAnswerDto item, CancellationToken ct = default)
    {
        try
        {
            var existing = await context.QuizAnswers
                .FirstOrDefaultAsync(x => x.Id == item.Id, ct);

            if (existing is null)
                return Result.Failure("Ответ не найден");

            existing.QuizQuestionId = item.QuizQuestionId;
            existing.Answer = item.Answer;
            existing.IsCorrect = item.IsCorrect;
            existing.Order = item.Order;
            existing.MediaContentId = item.MediaContentId;

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
        try
        {
            var entity = await context.QuizAnswers
                .FirstOrDefaultAsync(x => x.Id == id, ct);

            if (entity is null)
                return Result.Failure("Ответ не найден");

            context.QuizAnswers.Remove(entity);
            await context.SaveChangesAsync(ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    public async Task<Result<List<QuizAnswerDto>>> GetByQuestionIdAsync(Guid quizQuestionId, CancellationToken ct = default)
    {
        try
        {
            var items = await context.QuizAnswers
                .AsNoTracking()
                .Where(x => x.QuizQuestionId == quizQuestionId)
                .OrderBy(x => x.Order)
                .ToListAsync(ct);

            return items.Select(x => x.ToDto()).ToList();
        }
        catch (Exception ex)
        {
            return Result.Failure<List<QuizAnswerDto>>(ex.Message);
        }
    }

    public async Task<Result<List<QuizAnswerDto>>> GetCorrectAnswersByQuestionIdAsync(Guid quizQuestionId, CancellationToken ct = default)
    {
        try
        {
            var items = await context.QuizAnswers
                .AsNoTracking()
                .Where(x => x.QuizQuestionId == quizQuestionId && x.IsCorrect)
                .ToListAsync(ct);

            return items.Select(x => x.ToDto()).ToList();
        }
        catch (Exception ex)
        {
            return Result.Failure<List<QuizAnswerDto>>(ex.Message);
        }
    }
}
