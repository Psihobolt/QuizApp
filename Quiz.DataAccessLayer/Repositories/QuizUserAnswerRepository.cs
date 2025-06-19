using LightResults;
using Microsoft.EntityFrameworkCore;
using Quiz.DataAccessLayer.DTOs;
using Quiz.DataAccessLayer.DTOs.Extensions;
using Quiz.DataAccessLayer.Interfaces;

namespace Quiz.DataAccessLayer.Repositories;

public sealed class QuizUserAnswerRepository(QuizContext context) : IQuizUserAnswerRepository
{
    public async Task<Result<QuizUserAnswerDto>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        try
        {
            var entity = await context.QuizUserAnswers
                .AsNoTracking()
                .Include(x => x.Answer)
                .Include(x => x.QuizQuestion)
                .Include(x => x.Player)
                .FirstOrDefaultAsync(x => x.Id == id, ct);

            return entity is not null
                ? Result.Success(entity.ToDto())
                : Result.Failure<QuizUserAnswerDto>("Ответ игрока не найден");
        }
        catch (Exception ex)
        {
            return Result.Failure<QuizUserAnswerDto>(ex.Message);
        }
    }

    public async Task<Result<List<QuizUserAnswerDto>>> GetAllAsync(CancellationToken ct = default)
    {
        try
        {
            var data = await context.QuizUserAnswers
                .AsNoTracking()
                .Include(x => x.Answer)
                .Include(x => x.QuizQuestion)
                .Include(x => x.Player)
                .ToListAsync(ct);

            return Result.Success(data.Select(x => x.ToDto()).ToList());
        }
        catch (Exception ex)
        {
            return Result.Failure<List<QuizUserAnswerDto>>(ex.Message);
        }
    }

    public async Task<Result> AddAsync(QuizUserAnswerDto dto, CancellationToken ct = default)
    {
        try
        {
            var entity = dto.FromDto();
            context.QuizUserAnswers.Add(entity);
            await context.SaveChangesAsync(ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    public async Task<Result> UpdateAsync(QuizUserAnswerDto dto, CancellationToken ct = default)
    {
        try
        {
            var entity = dto.FromDto();
            context.QuizUserAnswers.Update(entity);
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
            var entity = await context.QuizUserAnswers.FindAsync([id], ct);
            if (entity is null)
                return Result.Failure("Ответ игрока не найден");

            context.QuizUserAnswers.Remove(entity);
            await context.SaveChangesAsync(ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    public async Task<Result<List<QuizUserAnswerDto>>> GetAnswersByPlayerIdAsync(Guid playerId, CancellationToken ct = default)
    {
        try
        {
            var data = await context.QuizUserAnswers
                .AsNoTracking()
                .Where(x => x.PlayerId == playerId)
                .Include(x => x.Answer)
                .Include(x => x.QuizQuestion)
                .ToListAsync(ct);

            return Result.Success(data.Select(x => x.ToDto()).ToList());
        }
        catch (Exception ex)
        {
            return Result.Failure<List<QuizUserAnswerDto>>(ex.Message);
        }
    }

    public async Task<Result<Dictionary<Guid, int>>> GetAnswerStatisticsByQuestionIdAsync(Guid questionId, CancellationToken ct = default)
    {
        try
        {
            var stats = await context.QuizUserAnswers
                .AsNoTracking()
                .Where(x => x.QuizQuestionId == questionId)
                .GroupBy(x => x.AnswerId)
                .Select(g => new { g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Key, x => x.Count, ct);

            return Result.Success(stats);
        }
        catch (Exception ex)
        {
            return Result.Failure<Dictionary<Guid, int>>(ex.Message);
        }
    }

    public async Task<Result<int>> GetCorrectAnswerCountByPlayerIdAsync(Guid playerId, CancellationToken ct = default)
    {
        try
        {
            var count = await context.QuizUserAnswers
                .AsNoTracking()
                .CountAsync(x => x.PlayerId == playerId && x.Answer.IsCorrect, ct);

            return Result.Success(count);
        }
        catch (Exception ex)
        {
            return Result.Failure<int>(ex.Message);
        }
    }

    public async Task<Result<bool>> HasPlayerAnsweredAsync(Guid playerId, Guid questionId, CancellationToken ct = default)
    {
        try
        {
            var exists = await context.QuizUserAnswers
                .AsNoTracking()
                .AnyAsync(x => x.PlayerId == playerId && x.QuizQuestionId == questionId, ct);

            return Result.Success(exists);
        }
        catch (Exception ex)
        {
            return Result.Failure<bool>(ex.Message);
        }
    }
}
