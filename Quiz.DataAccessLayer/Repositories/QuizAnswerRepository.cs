using Microsoft.EntityFrameworkCore;
using Quiz.DataAccessLayer.Interfaces;
using Quiz.DataAccessLayer.Models;

namespace Quiz.DataAccessLayer.Repositories;

public class QuizAnswerRepository : Repository<QuizAnswerModel>, IRepository<QuizAnswerModel>
{
    public QuizAnswerRepository(QuizContext context) : base(context)
    {
    }

    public override async Task<QuizAnswerModel?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(a => a.QuizQuestion)
            .Include(a => a.MediaContent)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public override async Task<IEnumerable<QuizAnswerModel>> GetAllAsync()
    {
        return await _dbSet
            .Include(a => a.QuizQuestion)
            .Include(a => a.MediaContent)
            .OrderBy(a => a.QuizQuestionId)
            .ThenBy(a => a.Order)
            .ToListAsync();
    }

    public async Task<IEnumerable<QuizAnswerModel>> GetByQuestionIdAsync(Guid questionId)
    {
        return await _dbSet
            .Include(a => a.MediaContent)
            .Where(a => a.QuizQuestionId == questionId)
            .OrderBy(a => a.Order)
            .ToListAsync();
    }

    public async Task<IEnumerable<QuizAnswerModel>> GetCorrectAnswersByQuestionIdAsync(Guid questionId)
    {
        return await _dbSet
            .Include(a => a.MediaContent)
            .Where(a => a.QuizQuestionId == questionId && a.IsCorrect)
            .OrderBy(a => a.Order)
            .ToListAsync();
    }
}
