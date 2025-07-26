using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NDT.BusinessLogic.Services.Interfaces;
using NDT.BusinessLogic.DTOs.RequestDTOs;
using NDT.BusinessLogic.DTOs.ResponseDTOs;


using NDT.API.CustomedResponses;
using Stockdata;
using System.Collections.Generic;
using Microsoft.AspNetCore.OData.Query;

namespace NDT.API.Controllers
{
    public class PriceBoardRequestDto
    {
        public List<string> Symbols { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IGrpcService _grpcService;
        private readonly IStockInforService _stockService;
        public StockController(IGrpcService grpcService, IStockInforService stockService)
        {
            _grpcService = grpcService;
            _stockService = stockService;
        }

        [HttpGet("historical/{symbol}")]
        public async Task<ActionResult<ApiResponse<HistoricalDataResponse>>> GetHistoricalData(string symbol, [FromQuery] string startDate, [FromQuery] string endDate)
        {
            try
            {
                var result = await _grpcService.GetHistoricalDataAsync(new HistoricalDataRequest { Symbol = symbol, StartDate = startDate, EndDate = endDate });
                return Ok(new ApiResponse<HistoricalDataResponse>(200, "Success", result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<HistoricalDataResponse>(400, ex.Message));
            }
        }

        [HttpGet("news/{symbol}")]
        public async Task<ActionResult<ApiResponse<NewsResponse>>> GetNews(string symbol)
        {
            try
            {
                var result = await _grpcService.GetNewsAsync(new CompanyRequest { Symbol = symbol });
                return Ok(new ApiResponse<NewsResponse>(200, "Success", result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<NewsResponse>(400, ex.Message));
            }
        }

        [HttpGet("events/{symbol}")]
        public async Task<ActionResult<ApiResponse<EventsResponse>>> GetEvents(string symbol)
        {
            try
            {
                var result = await _grpcService.GetEventsAsync(new CompanyRequest { Symbol = symbol });
                return Ok(new ApiResponse<EventsResponse>(200, "Success", result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<EventsResponse>(400, ex.Message));
            }
        }

        [HttpGet("shareholders/{symbol}")]
        public async Task<ActionResult<ApiResponse<MajorShareholdersResponse>>> GetMajorShareholders(string symbol)
        {
            try
            {
                var result = await _grpcService.GetMajorShareholdersAsync(new CompanyRequest { Symbol = symbol });
                return Ok(new ApiResponse<MajorShareholdersResponse>(200, "Success", result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<MajorShareholdersResponse>(400, ex.Message));
            }
        }

        [HttpGet("officers/{symbol}")]
        public async Task<ActionResult<ApiResponse<OfficersResponse>>> GetOfficers(string symbol)
        {
            try
            {
                var result = await _grpcService.GetOfficersAsync(new CompanyRequest { Symbol = symbol });
                return Ok(new ApiResponse<OfficersResponse>(200, "Success", result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<OfficersResponse>(400, ex.Message));
            }
        }

        [HttpGet("tradingstats/{symbol}")]
        public async Task<ActionResult<ApiResponse<TradingStatsResponse>>> GetTradingStats(string symbol)
        {
            try
            {
                var result = await _grpcService.GetTradingStatsAsync(new CompanyRequest { Symbol = symbol });
                return Ok(new ApiResponse<TradingStatsResponse>(200, "Success", result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<TradingStatsResponse>(400, ex.Message));
            }
        }

        [HttpGet("incomestatement/{symbol}")]
        public async Task<ActionResult<ApiResponse<IncomeStatementResponse>>> GetIncomeStatement(string symbol, [FromQuery] string period)
        {
            try
            {
                var result = await _grpcService.GetIncomeStatementAsync(new IncomeStatementRequest { Symbol = symbol, Period = period });
                return Ok(new ApiResponse<IncomeStatementResponse>(200, "Success", result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<IncomeStatementResponse>(400, ex.Message));
            }
        }

        [HttpGet("financeratios/{symbol}")]
        public async Task<ActionResult<ApiResponse<FinanceRatiosResponse>>> GetFinanceRatios(string symbol, [FromQuery] string period)
        {
            try
            {
                var result = await _grpcService.GetFinanceRatiosAsync(new IncomeStatementRequest { Symbol = symbol, Period = period });
                return Ok(new ApiResponse<FinanceRatiosResponse>(200, "Success", result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<FinanceRatiosResponse>(400, ex.Message));
            }
        }

        [HttpPost("priceboard")]
        public async Task<ActionResult<ApiResponse<PriceBoardResponse>>> GetPriceBoard([FromBody] PriceBoardRequestDto dto)
        {
            try
            {
                var request = new PriceBoardRequest();
                if (dto.Symbols != null)
                    request.Symbols.AddRange(dto.Symbols);
                var result = await _grpcService.GetPriceBoardAsync(request);
                return Ok(new ApiResponse<PriceBoardResponse>(200, "Success", result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<PriceBoardResponse>(400, ex.Message));
            }
        }

        // CRUD Stock
        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<StockResponseDTO>>> GetAllStocks()
        {
            var result = await _stockService.GetAllStocksAsync();
            return Ok(result);
        }

        [HttpGet("{symbol}")]
        public async Task<ActionResult<ApiResponse<StockFullInfoResponseDTO>>> GetStockBySymbol(string symbol)
        {
            var result = await _stockService.GetFullStockInfoAsync(symbol);
            if (result == null) return NotFound(new ApiResponse<StockFullInfoResponseDTO>(404, "Not found"));
            return Ok(new ApiResponse<StockFullInfoResponseDTO>(200, "Success", result));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<StockResponseDTO>>> CreateStock([FromBody] StockRequestDTO dto)
        {
            var existingStock = await _stockService.GetStockByCodeAsync(dto.Code);
            if (existingStock != null)
            {
                return BadRequest(new ApiResponse<StockResponseDTO>(400, "Stock with this code already exists"));
            }
            var result = await _stockService.CreateStockAsync(dto);
            return Ok(new ApiResponse<StockResponseDTO>(201, "Created", result));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<StockResponseDTO>>> UpdateStock(int id, [FromBody] StockRequestDTO dto)
        {
            var result = await _stockService.UpdateStockAsync(id, dto);
            if (result == null) return NotFound(new ApiResponse<StockResponseDTO>(404, "Not found"));
            return Ok(new ApiResponse<StockResponseDTO>(200, "Updated", result));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteStock(int id)
        {
            var success = await _stockService.DeleteStockAsync(id);
            if (!success) return NotFound(new ApiResponse<object>(404, "Not found"));
            return Ok(new ApiResponse<object>(200, "Deleted"));
        }

        /// <summary>
        /// Thêm nhiều stock vào watchlist của user
        /// </summary>
        /// <remarks>
        /// POST /api/Stock/watchlist/add
        /// Body: { "userId": "user-id", "stockIds": [1,2,3] }
        [HttpGet("watchlist/{userId}")]
        public async Task<ActionResult<ApiResponse<List<string>>>> GetWatchList(string userId)
        {
            var watchlists = await _stockService.GetWatchListAsync(userId);
           
            return Ok(new ApiResponse<List<string>> (200, $"Successfully",watchlists ?? new List<string>()));
        }
        /// </remarks>
        [HttpPost("watchlist/add")]
        public async Task<ActionResult<ApiResponse<object>>> AddStockToWatchList([FromBody] WatchListStockRequestDTO dto)
        {
            var added = await _stockService.AddStockToWatchListAsync(dto);
            if (!added) return BadRequest(new ApiResponse<object>(400, "No stock added to watchlist"));
            return Ok(new ApiResponse<object>(200, $"Added {added} stock(s) to watchlist"));
        }

        [HttpPost("watchlist/remove")]
        public async Task<ActionResult<ApiResponse<object>>> RemoveStockFromWatchList([FromBody] WatchListStockRequestDTO dto)
        {
            var success = await _stockService.RemoveStockFromWatchListAsync(dto);
            if (!success) return BadRequest(new ApiResponse<object>(400, "Failed to remove stock from watchlist"));
            return Ok(new ApiResponse<object>(200, "Removed from watchlist"));
        }

        /// </remarks>
        [HttpPost("batch")]
        public async Task<ActionResult<ApiResponse<IEnumerable<StockResponseDTO>>>> CreateStocks([FromBody] List<StockRequestDTO> dtos)
        {
            var results = new List<StockResponseDTO>();
            foreach (var dto in dtos)
            {
                var result = await _stockService.CreateStockAsync(dto);
                results.Add(result);
            }
            return Ok(new ApiResponse<IEnumerable<StockResponseDTO>>(201, $"Created {results.Count} stocks", results));
        }
    }
}
