using Quiz.DataAccessLayer.Models;
using Quiz.DataAccessLayer.Repositories;
using Xunit;

namespace Quiz.Tests.Repositories;

public class TelegramUsersRepositoryTests : TestBase
{
    [Fact]
    public async Task GetByTelegramUserIdAsync_ExistingUser_ReturnsUser()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new TelegramUsersRepository(context);
        
        var user = new TelegramUsersModel
        {
            TelegramUserId = "123456789",
            Username = "testuser",
            CustomName = "Test User"
        };
        
        await context.TelegramUsers.AddAsync(user);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByTelegramUserIdAsync("123456789");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("123456789", result.TelegramUserId);
        Assert.Equal("testuser", result.Username);
        Assert.Equal("Test User", result.CustomName);
    }

    [Fact]
    public async Task GetByTelegramUserIdAsync_NonExistingUser_ReturnsNull()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new TelegramUsersRepository(context);

        // Act
        var result = await repository.GetByTelegramUserIdAsync("999999999");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByUsernameAsync_ExistingUser_ReturnsUser()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new TelegramUsersRepository(context);
        
        var user = new TelegramUsersModel
        {
            TelegramUserId = "123456789",
            Username = "testuser",
            CustomName = "Test User"
        };
        
        await context.TelegramUsers.AddAsync(user);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByUsernameAsync("testuser");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("testuser", result.Username);
        Assert.Equal("123456789", result.TelegramUserId);
    }

    [Fact]
    public async Task GetByUsernameAsync_NonExistingUser_ReturnsNull()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new TelegramUsersRepository(context);

        // Act
        var result = await repository.GetByUsernameAsync("nonexistent");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ExistsByTelegramUserIdAsync_ExistingUser_ReturnsTrue()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new TelegramUsersRepository(context);
        
        var user = new TelegramUsersModel
        {
            TelegramUserId = "123456789",
            Username = "testuser",
            CustomName = "Test User"
        };
        
        await context.TelegramUsers.AddAsync(user);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.ExistsByTelegramUserIdAsync("123456789");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExistsByTelegramUserIdAsync_NonExistingUser_ReturnsFalse()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new TelegramUsersRepository(context);

        // Act
        var result = await repository.ExistsByTelegramUserIdAsync("999999999");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ExistsByUsernameAsync_ExistingUser_ReturnsTrue()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new TelegramUsersRepository(context);
        
        var user = new TelegramUsersModel
        {
            TelegramUserId = "123456789",
            Username = "testuser",
            CustomName = "Test User"
        };
        
        await context.TelegramUsers.AddAsync(user);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.ExistsByUsernameAsync("testuser");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExistsByUsernameAsync_NonExistingUser_ReturnsFalse()
    {
        // Arrange
        using var context = CreateContext();
        var repository = new TelegramUsersRepository(context);

        // Act
        var result = await repository.ExistsByUsernameAsync("nonexistent");

        // Assert
        Assert.False(result);
    }
} 