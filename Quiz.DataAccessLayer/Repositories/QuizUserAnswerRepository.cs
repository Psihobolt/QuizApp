using Microsoft.EntityFrameworkCore;
using Quiz.DataAccessLayer.Interfaces;
using Quiz.DataAccessLayer.Models;

namespace Quiz.DataAccessLayer.Repositories;

public class QuizUserAnswerRepository : Repository<QuizUserAnswerModel>
{
    public QuizUserAnswerRepository(QuizContext context) : base(context)
    {
    }

    public override async Task<QuizUserAnswerModel?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(ua => ua.Player)
            .Include(ua => ua.QuizQuestion)
            .Include(ua => ua.Answer)
            .FirstOrDefaultAsync(ua => ua.Id == id);
    }

    public override async Task<IEnumerable<QuizUserAnswerModel>> GetAllAsync()
    {
        return await _dbSet
            .Include(ua => ua.Player)
            .Include(ua => ua.QuizQuestion)
            .Include(ua => ua.Answer)
            .ToListAsync();
    }

    public async Task<IEnumerable<QuizUserAnswerModel>> GetByPlayerIdAsync(Guid playerId)
    {
        return await _dbSet
            .Include(ua => ua.QuizQuestion)
            .Include(ua => ua.Answer)
            .Where(ua => ua.PlayerId == playerId)
            .ToListAsync();
    }

    public async Task<IEnumerable<QuizUserAnswerModel>> GetByQuestionIdAsync(Guid questionId)
    {
        return await _dbSet
            .Include(ua => ua.Player)
            .Include(ua => ua.Answer)
            .Where(ua => ua.QuizQuestionId == questionId)
            .ToListAsync();
    }

    public async Task<QuizUserAnswerModel> GetByPlayerAndQuestionAsync(Guid playerId, Guid questionId)
    {
        return await _dbSet
            .Include(ua => ua.Player)
            .Include(ua => ua.QuizQuestion)
            .Include(ua => ua.Answer)
            .FirstOrDefaultAsync(ua => ua.PlayerId == playerId && ua.QuizQuestionId == questionId);
    }

    public async Task<bool> HasPlayerAnsweredQuestionAsync(Guid playerId, Guid questionId)
    {
        return await _dbSet.AnyAsync(ua => ua.PlayerId == playerId && ua.QuizQuestionId == questionId);
    }
}
