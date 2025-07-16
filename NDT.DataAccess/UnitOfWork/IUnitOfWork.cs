using NDT.BusinessModels.Entities;
using NDT.DataAccess.Repositories;
using System;
using System.Security.Principal;
using System.Threading.Tasks;

namespace NDT.DataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<AppUser, Guid> AppUser { get; }
        IGenericRepository<Category, int> Categories { get; }
        IGenericRepository<News, int> News { get; }
        IGenericRepository<Tags, int> Tags { get; }
        IGenericRepository<Advertisement, int> Advertisements { get; }
        IGenericRepository<WatchList, int> WatchLists { get; }
        IGenericRepository<Stock, int> Stocks { get; }
        IGenericRepository<FrontPageNews, int> FrontPageNews { get; }

        Task<int> SaveChangesAsync();
    }
} 