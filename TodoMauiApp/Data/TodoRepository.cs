using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TodoMauiApp.Models;

namespace TodoMauiApp.Data
{
    public class TodoRepository
    {
        private readonly TaskDbContext _context;

        public TodoRepository(TaskDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<TodoItem>> GetAllAsync()
        {
            try
            {
                if (_context.TodoItems == null)
                {
                    Debug.WriteLine("TodoItems DbSet is null");
                    return new List<TodoItem>();
                }
                
                return await _context.TodoItems.ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetAllAsync error: {ex.Message}");
                return new List<TodoItem>();
            }
        }

        public async Task<TodoItem?> GetByIdAsync(int id)
        {
            try
            {
                if (_context.TodoItems == null)
                {
                    Debug.WriteLine("TodoItems DbSet is null");
                    return null;
                }
                
                return await _context.TodoItems.FindAsync(id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetByIdAsync error: {ex.Message}");
                return null;
            }
        }

        public async Task<int> AddAsync(TodoItem item)
        {
            if (item == null)
            {
                Debug.WriteLine("Cannot add null TodoItem");
                return 0;
            }
            
            try
            {
                if (_context.TodoItems == null)
                {
                    Debug.WriteLine("TodoItems DbSet is null");
                    return 0;
                }
                
                _context.TodoItems.Add(item);
                await _context.SaveChangesAsync();
                return item.Id;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"AddAsync error: {ex.Message}");
                throw; // Rethrow to handle in ViewModel
            }
        }

        public async Task UpdateAsync(TodoItem item)
        {
            if (item == null)
            {
                Debug.WriteLine("Cannot update null TodoItem");
                return;
            }
            
            try
            {
                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"UpdateAsync error: {ex.Message}");
                throw; // Rethrow to handle in ViewModel
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                if (_context.TodoItems == null)
                {
                    Debug.WriteLine("TodoItems DbSet is null");
                    return;
                }
                
                var item = await _context.TodoItems.FindAsync(id);
                if (item != null)
                {
                    _context.TodoItems.Remove(item);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"DeleteAsync error: {ex.Message}");
                throw; // Rethrow to handle in ViewModel
            }
        }
    }
} 