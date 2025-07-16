using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NDT.BusinessModels.Entities;
using NDT.DataAccess.Data;
using NDT.DataAccess.Repositories;
using System;
using System.Threading.Tasks;

namespace NDT.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IGenericRepository<AppUser, Guid> AppUser { get; }
        public IGenericRepository<Category, int> Categories { get; }
        public IGenericRepository<News, int> News { get; }
        public IGenericRepository<Tags, int> Tags { get; }
        public IGenericRepository<Advertisement, int> Advertisements { get; }
        public IGenericRepository<WatchList, int> WatchLists { get; }
        public IGenericRepository<Stock, int> Stocks { get; }
        public IGenericRepository<FrontPageNews, int> FrontPageNews { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            AppUser = new GenericRepository<AppUser, Guid>(_context);
            Categories = new GenericRepository<Category, int>(_context);
            News = new GenericRepository<News, int>(_context);
            Tags = new GenericRepository<Tags, int>(_context);
            Advertisements = new GenericRepository<Advertisement, int>(_context);
            WatchLists = new GenericRepository<WatchList, int>(_context);
            Stocks = new GenericRepository<Stock, int>(_context);
            FrontPageNews = new GenericRepository<FrontPageNews, int>(_context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
} 