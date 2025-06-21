using LightResults;
using Microsoft.EntityFrameworkCore;
using Quiz.DataAccessLayer.DTOs;
using Quiz.DataAccessLayer.DTOs.Extensions;
using Quiz.DataAccessLayer.Interfaces;

namespace Quiz.DataAccessLayer.Repositories;

public sealed class QuizStateRepository(QuizContext context)
    : IQuizStateRepository
{
    public async Task<Result<QuizStateDto>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await context.QuizStates
            .AsNoTracking()
            .Include(q => q.QuizQuestion)
            .FirstOrDefaultAsync(q => q.Id == id, ct);

        return entity is null
            ? Result.Failure<QuizStateDto>("Состояние викторины не найдено.")
            : Result.Success(entity.ToDto());
    }

    public async Task<Result<QuizStateDto>> GetActiveStateAsync(CancellationToken ct = default)
    {
        var entity = await context.QuizStates
            .AsNoTracking()
            .Include(q => q.QuizQuestion)
            .FirstOrDefaultAsync(q => q.IsActive, ct);

        return entity is null
            ? Result.Failure<QuizStateDto>("Активное состояние викторины не найдено.")
            : Result.Success(entity.ToDto());
    }

    public async Task<Result> AddAsync(QuizStateDto dto, CancellationToken ct = default)
    {
        var model = dto.FromDto();
        context.QuizStates.Add(model);

        var count = await context.SaveChangesAsync(ct);
        return count == 0
            ? Result.Failure("Не удалось добавить состояние викторины.")
            : Result.Success();
    }

    public async Task<Result> UpdateAsync(QuizStateDto dto, CancellationToken ct = default)
    {
        var existing = await context.QuizStates.FindAsync([dto.Id], ct);
        if (existing is null)
            return Result.Failure("Состояние викторины не найдено.");

        existing.State = dto.State;
        existing.IsActive = dto.IsActive;
        existing.QuizQuestionId = dto.QuizQuestionId;

        var count = await context.SaveChangesAsync(ct);
        return count == 0
            ? Result.Failure("Не удалось обновить состояние викторины.")
            : Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await context.QuizStates.FindAsync([id], ct);
        if (entity is null)
            return Result.Failure("Состояние викторины не найдено.");

        context.QuizStates.Remove(entity);
        var count = await context.SaveChangesAsync(ct);
        return count == 0
            ? Result.Failure("Не удалось удалить состояние викторины.")
            : Result.Success();
    }

    public async Task<Result> DeactivateCurrentAsync(CancellationToken ct = default)
    {
        var current = await context.QuizStates.FirstOrDefaultAsync(q => q.IsActive, ct);
        if (current is null)
            return Result.Failure("Нет активного состояния для деактивации.");

        current.IsActive = false;
        var count = await context.SaveChangesAsync(ct);
        return count == 0
            ? Result.Failure("Не удалось деактивировать текущее состояние.")
            : Result.Success();
    }

    public async Task<Result<QuizStateDto>> SetNewStateAsync(QuizStateDto newState, CancellationToken ct = default)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(ct);

        var deactivateResult = await DeactivateCurrentAsync(ct);
        if (deactivateResult.IsFailure())
            return Result.Failure<QuizStateDto>(deactivateResult.Errors);

        var newEntity = newState.FromDto();
        newEntity.IsActive = true;
        context.QuizStates.Add(newEntity);

        var count = await context.SaveChangesAsync(ct);
        if (count == 0)
        {
            await transaction.RollbackAsync(ct);
            return Result.Failure<QuizStateDto>("Не удалось сохранить новое состояние викторины.");
        }

        await transaction.CommitAsync(ct);
        return Result.Success(newEntity.ToDto());
    }
}
