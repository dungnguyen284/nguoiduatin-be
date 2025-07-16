using NDT.BusinessLogic.DTOs.RequestDTOs;
using NDT.BusinessLogic.DTOs.ResponseDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NDT.BusinessLogic.Services.Interfaces
{
    public interface IStockInforService
    {
        Task<IEnumerable<StockResponseDTO>> GetAllStocksAsync();
        Task<StockResponseDTO?> GetStockByCodeAsync(string code);
        Task<StockFullInfoResponseDTO?> GetFullStockInfoAsync(string code);
        Task<StockResponseDTO> CreateStockAsync(StockRequestDTO dto);
        Task<StockResponseDTO?> UpdateStockAsync(int id, StockRequestDTO dto);
        Task<bool> DeleteStockAsync(int id);

        Task<bool> AddStockToWatchListAsync(WatchListStockRequestDTO dto);
        Task<bool> RemoveStockFromWatchListAsync(WatchListStockRequestDTO dto);
    }
}
