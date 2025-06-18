using Microsoft.EntityFrameworkCore;
using Quiz.DataAccessLayer.Interfaces;
using Quiz.DataAccessLayer.Models;

namespace Quiz.DataAccessLayer.Repositories;

public class QuizStateRepository : Repository<QuizStateModel>, IRepository<QuizStateModel>
{
    public QuizStateRepository(QuizContext context) : base(context)
    {
    }

    public override async Task<QuizStateModel> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(s => s.QuizQuestion)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public override async Task<IEnumerable<QuizStateModel>> GetAllAsync()
    {
        return await _dbSet
            .Include(s => s.QuizQuestion)
            .ToListAsync();
    }

    public async Task<QuizStateModel> GetActiveStateAsync()
    {
        return await _dbSet
            .Include(s => s.QuizQuestion)
            .FirstOrDefaultAsync(s => s.IsActive);
    }

    public async Task<IEnumerable<QuizStateModel>> GetStatesByTypeAsync(QuizStateEnum state)
    {
        return await _dbSet
            .Include(s => s.QuizQuestion)
            .Where(s => s.State == state)
            .ToListAsync();
    }
} 