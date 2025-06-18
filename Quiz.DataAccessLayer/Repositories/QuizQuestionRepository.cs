using Microsoft.EntityFrameworkCore;
using Quiz.DataAccessLayer.Interfaces;
using Quiz.DataAccessLayer.Models;

namespace Quiz.DataAccessLayer.Repositories;

public class QuizQuestionRepository : Repository<QuizQuestionModel>, IRepository<QuizQuestionModel>
{
    public QuizQuestionRepository(QuizContext context) : base(context)
    {
    }

    public override async Task<QuizQuestionModel> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(q => q.Answers)
            .Include(q => q.MediaContent)
            .FirstOrDefaultAsync(q => q.Id == id);
    }

    public override async Task<IEnumerable<QuizQuestionModel>> GetAllAsync()
    {
        return await _dbSet
            .Include(q => q.Answers)
            .Include(q => q.MediaContent)
            .OrderBy(q => q.Order)
            .ToListAsync();
    }

    public async Task<QuizQuestionModel> GetByOrderAsync(int order)
    {
        return await _dbSet
            .Include(q => q.Answers.OrderBy(a => a.Order))
            .Include(q => q.MediaContent)
            .FirstOrDefaultAsync(q => q.Order == order);
    }

    public async Task<IEnumerable<QuizQuestionModel>> GetQuestionsWithAnswersAsync()
    {
        return await _dbSet
            .Include(q => q.Answers.OrderBy(a => a.Order))
            .Include(q => q.MediaContent)
            .OrderBy(q => q.Order)
            .ToListAsync();
    }
} 