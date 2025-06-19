using LightResults;
using Quiz.DataAccessLayer.DTOs;

namespace Quiz.DataAccessLayer.Interfaces;

public interface ITelegramUsersRepository : IRepository<TelegramUserDto>
{
    /// <summary>
    /// Получить пользователя по Telegram ID
    /// </summary>
    Task<Result<TelegramUserDto>> GetByTelegramIdAsync(string telegramId, CancellationToken ct = default);
}
