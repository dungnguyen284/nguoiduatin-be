using Stockdata;

namespace NDT.BusinessLogic.DTOs.ResponseDTOs
{
    public class StockResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string CompanyDescription { get; set; }
        public string LogoUrl { get; set; }
    }

    public class StockFullInfoResponseDTO
    {
        public StockResponseDTO Stock { get; set; }
        public HistoricalDataResponse? Historical { get; set; }
        public NewsResponse? News { get; set; }
        public EventsResponse? Events { get; set; }
        public MajorShareholdersResponse? Shareholders { get; set; }
        public OfficersResponse? Officers { get; set; }
        public TradingStatsResponse? TradingStats { get; set; }
        public IncomeStatementResponse? IncomeStatement { get; set; }
        public FinanceRatiosResponse? FinanceRatios { get; set; }
        // Có thể bổ sung thêm các trường khác nếu cần
    }
} 