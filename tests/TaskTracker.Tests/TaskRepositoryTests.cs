using System;
using System.IO;
using System.Threading.Tasks;
using TodoCodexPoc.Models;
using TodoCodexPoc.Services;
using Xunit;

namespace TaskTracker.Tests;

public class TaskRepositoryTests
{
    [Fact]
    public async Task AddAndRetrieveItem()
    {
        // Arrange
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");
        var repo = new FileTodoRepository(path);
        var item = new TodoItem
        {
            Id = Guid.NewGuid(),
            Title = "Test item",
            Priority = 1
        };

        try
        {
            // Act
            await repo.AddAsync(item);
            var retrieved = await repo.GetAsync(item.Id);

            // Assert
            Assert.NotNull(retrieved);
            Assert.Equal(item.Title, retrieved!.Title);
        }
        finally
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
