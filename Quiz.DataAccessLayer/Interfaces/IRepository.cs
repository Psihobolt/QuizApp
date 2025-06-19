using LightResults;

namespace Quiz.DataAccessLayer.Interfaces;

public interface IRepository<TDto>
{
    Task<Result<TDto>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result> AddAsync(TDto dto, CancellationToken ct = default);
    Task<Result> UpdateAsync(TDto dto, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken ct = default);
}
