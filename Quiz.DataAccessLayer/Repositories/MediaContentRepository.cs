using LightResults;
using Microsoft.EntityFrameworkCore;
using Quiz.DataAccessLayer.DTOs;
using Quiz.DataAccessLayer.DTOs.Extensions;
using Quiz.DataAccessLayer.Interfaces;

namespace Quiz.DataAccessLayer.Repositories;

public sealed class MediaContentRepository(QuizContext context) : IMediaContentRepository
{
    public async Task<Result<MediaContentDto>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await context.MediaContents
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        return entity is null
            ? Result.Failure<MediaContentDto>("Media content not found.")
            : Result.Success(entity.ToDto());
    }

    public async Task<Result> AddAsync(MediaContentDto dto, CancellationToken ct = default)
    {
        var entity = dto.FromDto();

        await context.MediaContents.AddAsync(entity, ct);
        var changes = await context.SaveChangesAsync(ct);

        return changes > 0
            ? Result.Success()
            : Result.Failure("Failed to add media content.");
    }

    public async Task<Result> UpdateAsync(MediaContentDto dto, CancellationToken ct = default)
    {
        var existing = await context.MediaContents.FindAsync([dto.Id], ct);
        if (existing is null)
            return Result.Failure("Media content not found.");

        existing.Data = dto.Data;

        context.MediaContents.Update(existing);
        var changes = await context.SaveChangesAsync(ct);

        return changes > 0
            ? Result.Success()
            : Result.Failure("Failed to update media content.");
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await context.MediaContents.FindAsync([id], ct);
        if (entity is null)
            return Result.Failure("Media content not found.");

        context.MediaContents.Remove(entity);
        var changes = await context.SaveChangesAsync(ct);

        return changes > 0
            ? Result.Success()
            : Result.Failure("Failed to delete media content.");
    }
}
