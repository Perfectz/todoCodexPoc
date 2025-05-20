using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using TodoMauiApp.Data;

namespace TodoMauiApp.Services
{
    public class DatabaseService
    {
        private readonly string _databasePath;
        private TaskDbContext? _context;

        public DatabaseService()
        {
            var databaseName = "todos.db";
            var basePath = FileSystem.AppDataDirectory;
            _databasePath = Path.Combine(basePath, databaseName);
        }

        public TaskDbContext GetContext()
        {
            if (_context == null)
            {
                var options = new DbContextOptionsBuilder<TaskDbContext>()
                    .UseSqlite($"Filename={_databasePath}")
                    .Options;

                _context = new TaskDbContext(options);
            }

            return _context;
        }

        public void Initialize()
        {
            GetContext().Database.EnsureCreated();
        }
    }
} 