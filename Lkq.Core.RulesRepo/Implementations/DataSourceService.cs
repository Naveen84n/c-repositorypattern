using System;
using System.Net;
using System.Threading.Tasks;
using Polly;
using RestSharp;
using ERPProcessing;
using Lkq.Core.RulesRepo.Interfaces;
using Lkq.Models.RulesRepo.CompNine;
using Lkq.Core.RulesRepo.Common;
using System.Diagnostics.CodeAnalysis;

namespace Lkq.Core.RulesRepo.Implementations
{
    [ExcludeFromCodeCoverage]
    public class DataSourceService : IDataSourceService
    {
        public async Task<VINDecodeResponseEntity> GetDataOneData(string vin)
        {
            return await WrapCall(GetDataOneVinData, vin);
        }

        private T WrapCall<T>(Func<string, T> fun, string vin)
        {
            var policy = Policy.Handle<AggregateException>()
                .Or<WebException>()
                .Or<TaskCanceledException>()
                .WaitAndRetry(new[]
                {
                        TimeSpan.FromSeconds(1),
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(15),
                        TimeSpan.FromSeconds(30)
                });
            var response = policy.Execute(() => fun(vin));
            return response;
        }

        public CompNineRequestedData GetCompNineData(string vin)
        {
            return WrapCall(GetCompNineVinData, vin);
        }

        public CompNineRequestedData GetCompNineVinData(string vin)
        {
            var client = new RestClient(Constants.COMPNINE_URL);
            var request = new RestRequest("vehicleinformation/", Method.POST);
            request.AddParameter("DataRequested", "CompNine");
            request.AddParameter("VinRequested", vin);

            return client.Execute<CompNineRequestedData>(request).Data;
        }

        private async Task<VINDecodeResponseEntity> GetDataOneVinData(string vin)
        {
            var client = new ERPProcessingClient();
            var vinReponse = await client.GetVINDataAsync(new VINDecodeRequestEntity
            {
                Display = "full",
                Styles = true,
                StyleBasicData = true,
                StyleSpecifications = true,
                StyleEngines = true,
                StyleTransmissions = true,
                StyleInstalled = true,
                StyleSafety = true,
                StyleOptional = true,
                StyleGeneric = true,
                StyleColors = true,
                StyleFuel = true,
                StyleWarranties = true,
                StylePricing = true,
                StyleSingleStock = true,
                StyleMultiStock = true,
                StyleRecalls = true,
                StyleService = true,
                StyleAwards = true,
                StyleVideo = true,
                StyleCrashTest = true,
                StyleEvox = true,
                StyleGreen = true,
                StyleOverview = true,
                CommonData = true,
                CommonDataAwards = true,
                CommonDataBasicData = true,
                CommonDataColors = true,
                CommonDataCrashTest = true,
                CommonDataEngines = true,
                CommonDataEvox = true,
                CommonDataFuel = true,
                CommonDataGeneric = true,
                CommonDataGreen = true,
                CommonDataInstalled = true,
                CommonDataMultiStock = true,
                CommonDataOptional = true,
                CommonDataOverview = true,
                CommonDataPricing = true,
                CommonDataRecalls = true,
                CommonDataSafety = true,
                CommonDataService = true,
                CommonDataSingleStock = true,
                CommonDataSpecifications = true,
                CommonDataTransmissions = true,
                CommonDataVideo = true,
                CommonDataWarranties = true,
                QueryRequest = new[]
                {
                    new QueryRequestEntity {Identifier = vin, VIN = vin}
                }
            });

            return vinReponse;
        }
    }
}