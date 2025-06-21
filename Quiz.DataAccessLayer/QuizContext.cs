using Microsoft.EntityFrameworkCore;
using Quiz.DataAccessLayer.Models;

namespace Quiz.DataAccessLayer;

public class QuizContext(DbContextOptions<QuizContext> options) : DbContext(options)
{
    public DbSet<QuizQuestionModel> QuizQuestions { get; set; }

    public DbSet<QuizAnswerModel> QuizAnswers { get; set; }

    public DbSet<MediaContentModel> MediaContents { get; set; }

    public DbSet<QuizStateModel> QuizStates { get; set; }

    public DbSet<TelegramUsersModel> TelegramUsers { get; set; }

    public DbSet<QuizUserAnswerModel> QuizUserAnswers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Конфигурация QuizQuestionModel
        modelBuilder.Entity<QuizQuestionModel>(entity =>
        {
            entity.ToTable("QuizQuestions");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(e => e.Question)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("Текст вопроса");

            entity.Property(e => e.Order)
                .IsRequired()
                .HasComment("Порядок вопроса. Нумерация сквозная");

            // Уникальный индекс для Order
            entity.HasIndex(e => e.Order)
                .IsUnique();

            // Связь один-ко-многим с QuizAnswerModel
            entity.HasMany(e => e.Answers)
                .WithOne(e => e.QuizQuestion)
                .HasForeignKey(e => e.QuizQuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь один-к-одному с MediaContentModel
            entity.HasOne(e => e.MediaContent)
                .WithMany()
                .HasForeignKey("MediaContentId")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Конфигурация QuizAnswerModel
        modelBuilder.Entity<QuizAnswerModel>(entity =>
        {
            entity.ToTable("QuizAnswers");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(e => e.QuizQuestionId)
                .IsRequired()
                .HasComment("Id вопроса, к которому относится ответ");

            entity.Property(e => e.IsCorrect)
                .IsRequired()
                .HasComment("Является ли ответ правильным для вопроса, к которому он относится. На некоторые вопросы может быть несколько правильных ответов, либо ни одного");

            entity.Property(e => e.Answer)
                .IsRequired()
                .HasComment("Текст ответа");

            entity.Property(e => e.MediaContentId)
                .IsRequired(false)
                .HasComment("Id картинки, которая относится к ответу");

            entity.Property(e => e.Order)
                .IsRequired()
                .HasDefaultValue(0)
                .HasComment("Порядок ответа. Нумерация только в рамках вопроса");

            // Составной уникальный индекс для QuizQuestionId и Order
            entity.HasIndex(e => new { e.QuizQuestionId, e.Order })
                .IsUnique();

            // Связь с QuizQuestionModel
            entity.HasOne(e => e.QuizQuestion)
                .WithMany(e => e.Answers)
                .HasForeignKey(e => e.QuizQuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь с MediaContentModel
            entity.HasOne(e => e.MediaContent)
                .WithMany()
                .HasForeignKey(e => e.MediaContentId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Конфигурация MediaContentModel
        modelBuilder.Entity<MediaContentModel>(entity =>
        {
            entity.ToTable("MediaContent");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(e => e.Data)
                .HasColumnType("bytea")
                .IsRequired()
                .HasComment("Хранит медиа файлы (картинки)");
        });

        // Конфигурация QuizStateModel
        modelBuilder.Entity<QuizStateModel>(entity =>
        {
            entity.ToTable("QuizState");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .HasComment("Какое активное состояние у викторины. Активным может быть только одно состояние");

            entity.Property(e => e.State)
                .IsRequired()
                .HasConversion<string>()
                .HasComment("Этап викторины, на котором сейчас викторина");

            entity.Property(e => e.QuizQuestionId)
                .IsRequired()
                .HasComment("Внешний ключ к таблице QuizQuestion. Если активное состояние - WaitingStart - то внешний ключ должен ссылаться на первый вопрос в первом туре");

            // Связь с QuizQuestionModel
            entity.HasOne(e => e.QuizQuestion)
                .WithMany()
                .HasForeignKey(e => e.QuizQuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Индекс для быстрого поиска активного состояния
            entity.HasIndex(e => e.IsActive)
                .IsUnique()
                .HasFilter("\"IsActive\" = true");
        });

        // Конфигурация TelegramUsersModel
        modelBuilder.Entity<TelegramUsersModel>(entity =>
        {
            entity.ToTable("TelegramUsers");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(e => e.TelegramUserId)
                .IsRequired()
                .HasComment("Id пользователя, получаемого из Telegram");

            entity.Property(e => e.Username)
                .IsRequired()
                .HasComment("Тег пользователя в telegram");

            entity.Property(e => e.CustomName)
                .IsRequired()
                .HasComment("Имя пользователя, которая устанавливается самим пользователем при регистрации на викторину");

            // Уникальный индекс для TelegramUserId
            entity.HasIndex(e => e.TelegramUserId)
                .IsUnique();

            // Уникальный индекс для Username
            entity.HasIndex(e => e.Username)
                .IsUnique();
        });

        // Конфигурация QuizUserAnswerModel
        modelBuilder.Entity<QuizUserAnswerModel>(entity =>
        {
            entity.ToTable("QuizUserAnswer");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(e => e.PlayerId)
                .IsRequired()
                .HasComment("Id игрока");

            entity.Property(e => e.QuizQuestionId)
                .IsRequired()
                .HasComment("Id вопроса, на который дал ответ игрок");

            entity.Property(e => e.AnswerId)
                .IsRequired()
                .HasComment("Id ответа, на который дал ответ игрок");

            // Связь с TelegramUsersModel
            entity.HasOne(e => e.Player)
                .WithMany()
                .HasForeignKey(e => e.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь с QuizQuestionModel
            entity.HasOne(e => e.QuizQuestion)
                .WithMany()
                .HasForeignKey(e => e.QuizQuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь с QuizAnswerModel
            entity.HasOne(e => e.Answer)
                .WithMany()
                .HasForeignKey(e => e.AnswerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Составной уникальный индекс для предотвращения дублирования ответов
            entity.HasIndex(e => new { e.PlayerId, e.QuizQuestionId })
                .IsUnique();
        });
    }
}
