using Microsoft.EntityFrameworkCore;
using Quiz.DataAccessLayer.Interfaces;
using Quiz.DataAccessLayer.Models;

namespace Quiz.DataAccessLayer.Repositories;

public class TelegramUsersRepository : Repository<TelegramUsersModel>, IRepository<TelegramUsersModel>
{
    public TelegramUsersRepository(QuizContext context) : base(context)
    {
    }

    public async Task<TelegramUsersModel> GetByTelegramUserIdAsync(string telegramUserId)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.TelegramUserId == telegramUserId);
    }

    public async Task<TelegramUsersModel> GetByUsernameAsync(string username)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<bool> ExistsByTelegramUserIdAsync(string telegramUserId)
    {
        return await _dbSet.AnyAsync(u => u.TelegramUserId == telegramUserId);
    }

    public async Task<bool> ExistsByUsernameAsync(string username)
    {
        return await _dbSet.AnyAsync(u => u.Username == username);
    }
} 