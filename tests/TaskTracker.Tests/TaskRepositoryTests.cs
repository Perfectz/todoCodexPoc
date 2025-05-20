using System;
using System.IO;
using System.Threading.Tasks;
using TodoCodexPoc.Models;
using TodoCodexPoc.Services;
using Xunit;
using Xunit.Abstractions;

namespace TaskTracker.Tests;

public class TaskRepositoryTests
{
    private readonly ITestOutputHelper _output;

    public TaskRepositoryTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task AddAndRetrieveItem()
    {
        // Arrange
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");
        using var repo = new FileTodoRepository(path);
        var item = new TodoItem
        {
            Id = Guid.NewGuid(),
            Title = "Test item",
            Priority = 1
        };

        _output.WriteLine($"Test item ID: {item.Id}");

        try
        {
            // Act
            await repo.AddAsync(item);
            _output.WriteLine("Item added");
            
            var items = await repo.GetAllAsync();
            _output.WriteLine($"Total items: {items.Count}");
            
            foreach (var i in items)
            {
                _output.WriteLine($"Item in repo: {i.Id} - {i.Title}");
            }
            
            var retrieved = await repo.GetAsync(item.Id);
            _output.WriteLine($"Retrieved item: {retrieved?.Id} - {retrieved?.Title}");

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
