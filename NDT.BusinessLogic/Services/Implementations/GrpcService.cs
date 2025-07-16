using NDT.BusinessLogic.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Stockdata;

namespace NDT.BusinessLogic.Services.Implementations
{
    public class GrpcService : IGrpcService
    {
        private readonly StockService.StockServiceClient _client;

        public GrpcService(string grpcEndpoint)
        {
            var channel = GrpcChannel.ForAddress(grpcEndpoint);
            _client = new StockService.StockServiceClient(channel);
        }

        public async Task<HistoricalDataResponse> GetHistoricalDataAsync(HistoricalDataRequest request)
            => await _client.GetHistoricalDataAsync(request);



        public async Task<MajorShareholdersResponse> GetMajorShareholdersAsync(CompanyRequest request)
            => await _client.GetMajorShareholdersAsync(request);


        public async Task<NewsResponse> GetNewsAsync(CompanyRequest request)
            => await _client.GetNewsAsync(request);

        public async Task<EventsResponse> GetEventsAsync(CompanyRequest request)
            => await _client.GetEventsAsync(request);

        public async Task<OfficersResponse> GetOfficersAsync(CompanyRequest request)
            => await _client.GetOfficersAsync(request);

        public async Task<TradingStatsResponse> GetTradingStatsAsync(CompanyRequest request)
            => await _client.GetTradingStatsAsync(request);

        public async Task<IncomeStatementResponse> GetIncomeStatementAsync(IncomeStatementRequest request)
            => await _client.GetIncomeStatementAsync(request);

        public async Task<FinanceRatiosResponse> GetFinanceRatiosAsync(IncomeStatementRequest request)
            => await _client.GetFinanceRatiosAsync(request);

        public async Task<PriceBoardResponse> GetPriceBoardAsync(PriceBoardRequest request)
            => await _client.GetPriceBoardAsync(request);
    }
}
