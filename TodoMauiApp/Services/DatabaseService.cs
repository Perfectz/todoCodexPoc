using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Diagnostics;
using TodoMauiApp.Data;

namespace TodoMauiApp.Services
{
    public class DatabaseService
    {
        private readonly string _databasePath;
        private TaskDbContext? _context;

        public DatabaseService()
        {
            try
            {
                var databaseName = "todos.db";
                var basePath = FileSystem.AppDataDirectory;
                Debug.WriteLine($"Database base path: {basePath}");
                
                // Ensure directory exists
                Directory.CreateDirectory(basePath);
                
                _databasePath = Path.Combine(basePath, databaseName);
                Debug.WriteLine($"Database full path: {_databasePath}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error setting up database path: {ex.Message}");
                // Fallback to a safe location
                _databasePath = "todos.db";
            }
        }

        public TaskDbContext GetContext()
        {
            if (_context == null)
            {
                try
                {
                    var options = new DbContextOptionsBuilder<TaskDbContext>()
                        .UseSqlite($"Filename={_databasePath}")
                        .LogTo(message => Debug.WriteLine(message))
                        .Options;

                    _context = new TaskDbContext(options);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error creating database context: {ex.Message}");
                    throw; // Rethrow to indicate failure
                }
            }

            return _context;
        }

        public bool Initialize()
        {
            try
            {
                Debug.WriteLine("Initializing database...");
                GetContext().Database.EnsureCreated();
                Debug.WriteLine("Database initialized successfully");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error initializing database: {ex.Message}");
                return false;
            }
        }
    }
} 