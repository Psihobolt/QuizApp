namespace Quiz.WebAPI.Common;

public class TelegramBotOptions
{
    public const string Section = "TelegramBot";

    public string Token { get; set; } = String.Empty;

    public string Name { get; set; } = String.Empty;

    public long AdminId { get; set; } = 0;
}
