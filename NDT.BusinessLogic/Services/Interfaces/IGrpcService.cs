using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stockdata;

namespace NDT.BusinessLogic.Services.Interfaces
{
    public interface IGrpcService
    {
        Task<HistoricalDataResponse> GetHistoricalDataAsync(HistoricalDataRequest request);
        Task<MajorShareholdersResponse> GetMajorShareholdersAsync(CompanyRequest request);
        Task<NewsResponse> GetNewsAsync(CompanyRequest request);
        Task<EventsResponse> GetEventsAsync(CompanyRequest request);
        Task<OfficersResponse> GetOfficersAsync(CompanyRequest request);
        Task<TradingStatsResponse> GetTradingStatsAsync(CompanyRequest request);
        Task<IncomeStatementResponse> GetIncomeStatementAsync(IncomeStatementRequest request);
        Task<FinanceRatiosResponse> GetFinanceRatiosAsync(IncomeStatementRequest request);
        Task<PriceBoardResponse> GetPriceBoardAsync(PriceBoardRequest request);
    }
}
