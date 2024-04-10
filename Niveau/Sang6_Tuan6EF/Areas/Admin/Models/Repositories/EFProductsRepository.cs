﻿using Microsoft.EntityFrameworkCore;
using Niveau.Areas.Admin.Models.Products;
using Niveau.Areas.User.Models;

namespace Niveau.Areas.Admin.Models.Repositories
{
    public class EFProductsRepository : IProductsRepository
    {
        private readonly ApplicationDbContext _context;
        public EFProductsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        //các lệnh thực thi Interface
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }
        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }
        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
