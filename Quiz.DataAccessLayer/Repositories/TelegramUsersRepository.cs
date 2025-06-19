using LightResults;
using Microsoft.EntityFrameworkCore;
using Quiz.DataAccessLayer.DTOs;
using Quiz.DataAccessLayer.Interfaces;
using Quiz.DataAccessLayer.Models;

namespace Quiz.DataAccessLayer.Repositories;

public class TelegramUsersRepository(QuizContext db) : ITelegramUsersRepository
{
    public async Task<Result<TelegramUserDto>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var model = await db.TelegramUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, ct);

        return model is null
            ? Result.Failure<TelegramUserDto>($"User with Id {id} not found")
            : Result.Success(new TelegramUserDto(model.Id, model.TelegramUserId, model.Username, model.CustomName));
    }

    public async Task<Result> AddAsync(TelegramUserDto dto, CancellationToken ct = default)
    {
        var entity = new TelegramUsersModel
        {
            Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id,
            TelegramUserId = dto.TelegramUserId,
            Username = dto.Username,
            CustomName = dto.CustomName
        };

        try
        {
            await db.TelegramUsers.AddAsync(entity, ct);
            await db.SaveChangesAsync(ct);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex);
        }
    }

    public async Task<Result> UpdateAsync(TelegramUserDto dto, CancellationToken ct = default)
    {
        var entity = await db.TelegramUsers
            .FirstOrDefaultAsync(u => u.Id == dto.Id, ct);

        if (entity is null) return Result.Failure($"User with Id {dto.Id} not found");

        entity.Username = dto.Username;
        entity.CustomName = dto.CustomName;

        try
        {
            db.TelegramUsers.Update(entity);
            await db.SaveChangesAsync(ct);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex);
        }
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await db.TelegramUsers
            .FirstOrDefaultAsync(u => u.Id == id, ct);

        if (entity is null) return Result.Failure($"User with Id {id} not found");

        try
        {
            db.TelegramUsers.Remove(entity);
            await db.SaveChangesAsync(ct);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex);
        }
    }

    public async Task<Result<TelegramUserDto>> GetByTelegramIdAsync(string telegramId, CancellationToken ct = default)
    {
        var model = await db.TelegramUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.TelegramUserId == telegramId, ct);

        return model is null
            ? Result.Failure<TelegramUserDto>($"User with TelegramId {telegramId} not found")
            : Result.Success(new TelegramUserDto(model.Id, model.TelegramUserId, model.Username, model.CustomName));
    }
}
