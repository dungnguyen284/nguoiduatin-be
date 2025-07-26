using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NDT.BusinessLogic.DTOs.RequestDTOs;
using NDT.BusinessLogic.DTOs.ResponseDTOs;
using NDT.BusinessLogic.Services.Interfaces;
using NDT.BusinessModels.Entities;
using NDT.DataAccess.UnitOfWork;

namespace NDT.BusinessLogic.Services.Implementations
{
    public class StockInfoService : IStockInforService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IGrpcService _grpcService;
        public StockInfoService(IUnitOfWork unitOfWork, IMapper mapper, IGrpcService grpcService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _grpcService = grpcService;
        }

        public async Task<IEnumerable<StockResponseDTO>> GetAllStocksAsync()
        {
            var stocks = await _unitOfWork.Stocks.GetAllAsync();
            return stocks.Select(s => _mapper.Map<StockResponseDTO>(s));
        }

        public async Task<StockResponseDTO?> GetStockByCodeAsync(string code)
        {
            var stock = await _unitOfWork.Stocks.FirstOrDefaultAsync(s => s.Code == code);
            return stock == null ? null : _mapper.Map<StockResponseDTO>(stock);
        }

        public async Task<StockFullInfoResponseDTO?> GetFullStockInfoAsync(string code)
        {
            var stock = await _unitOfWork.Stocks.FirstOrDefaultAsync(s => s.Code == code);
            if (stock == null) return null;
            var stockDto = _mapper.Map<StockResponseDTO>(stock);
            var grpcCompanyReq = new Stockdata.CompanyRequest { Symbol = code };
            var grpcIncomeReq = new Stockdata.IncomeStatementRequest { Symbol = code, Period = "quarterly" };
            // Lấy 20 ngày gần nhất
            var endDate = DateTime.UtcNow.Date;
            var startDate = endDate.AddDays(-19); // 20 ngày tính cả hôm nay
            var historicalRequest = new Stockdata.HistoricalDataRequest
            {
                Symbol = code,
                StartDate = startDate.ToString("yyyy-MM-dd"),
                EndDate = endDate.ToString("yyyy-MM-dd")
            };
            var fullInfo = new StockFullInfoResponseDTO
            {
                Stock = stockDto,
                Historical = await _grpcService.GetHistoricalDataAsync(historicalRequest),
                News = await _grpcService.GetNewsAsync(grpcCompanyReq),
                Events = await _grpcService.GetEventsAsync(grpcCompanyReq),
                Shareholders = await _grpcService.GetMajorShareholdersAsync(grpcCompanyReq),
                Officers = await _grpcService.GetOfficersAsync(grpcCompanyReq),
                TradingStats = await _grpcService.GetTradingStatsAsync(grpcCompanyReq),
                IncomeStatement = await _grpcService.GetIncomeStatementAsync(grpcIncomeReq),
                FinanceRatios = await _grpcService.GetFinanceRatiosAsync(grpcIncomeReq)
            };
            return fullInfo;
        }

        public async Task<StockResponseDTO> CreateStockAsync(StockRequestDTO dto)
        {
            var stock = _mapper.Map<Stock>(dto);
            await _unitOfWork.Stocks.AddAsync(stock);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<StockResponseDTO>(stock);
        }

        public async Task<StockResponseDTO?> UpdateStockAsync(int id, StockRequestDTO dto)
        {
            var stock = await _unitOfWork.Stocks.GetByIdAsync(id);
            if (stock == null) return null;
            stock.Name = dto.Name;
            stock.Code = dto.Code;
            stock.CompanyDescription = dto.CompanyDescription;
            stock.LogoUrl = dto.LogoUrl;
            await _unitOfWork.Stocks.UpdateAsync(stock);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<StockResponseDTO>(stock);
        }

        public async Task<bool> DeleteStockAsync(int id)
        {
            var stock = await _unitOfWork.Stocks.GetByIdAsync(id);
            if (stock == null) return false;
            await _unitOfWork.Stocks.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddStockToWatchListAsync(WatchListStockRequestDTO dto)
        {
            var watchList = await _unitOfWork.WatchLists.FirstOrDefaultAsync(w => w.UserId == dto.UserId, s => s.Stocks);
            var stock = await _unitOfWork.Stocks.FirstOrDefaultAsync(c => c.Code == dto.Code);
            if (stock == null) return false;
            if (watchList == null)
            {
                watchList = new WatchList
                {
                    UserId = dto.UserId,
                    Stocks = new List<Stock> { stock }
                };
                await _unitOfWork.WatchLists.AddAsync(watchList);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            if (!watchList.Stocks.Any(s => s.Id == stock.Id))
            {
                watchList.Stocks.Add(stock);
                await _unitOfWork.WatchLists.UpdateAsync(watchList);
                await _unitOfWork.SaveChangesAsync();
            }
            return true;
        }

        public async Task<bool> RemoveStockFromWatchListAsync(WatchListStockRequestDTO dto)
        {
            var watchList = await _unitOfWork.WatchLists.FirstOrDefaultAsync(w => w.UserId == dto.UserId, s => s.Stocks);
            if (watchList == null) return false;
            var stock = watchList.Stocks.FirstOrDefault(s => s.Code == dto.Code);
            if (stock == null) return false;
            watchList.Stocks.Remove(stock);
            await _unitOfWork.WatchLists.UpdateAsync(watchList);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<List<string>> GetWatchListAsync(string userId)
        {
            var watchList = await _unitOfWork.WatchLists
                .FirstOrDefaultAsync(w => w.UserId == userId, w => w.Stocks);
            return watchList == null ? new List<string>() : watchList.Stocks
                .Select(s => s.Code)
                .ToList();
        }
    }
}
