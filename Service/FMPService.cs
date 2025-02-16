using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using api.interfaces;
using api.Models;
using Newtonsoft.Json;
using api.Dtos.Stock;
using api.Mappers;

namespace api.Service
{
    public class FMPService : IFMPService
    {
        private HttpClient _httpClient;

        private IConfiguration _config;


        public FMPService(HttpClient httpClient, IConfiguration config)

        {
            _httpClient=httpClient;
            _config=config;
            

        }


        public async Task<Stock> FindStockBySymbolAsync(string symbol)
        {

            try{
                var result =await _httpClient.GetAsync($"https://financialmodelingprep.com/api/v3/profile/{symbol}?apikey={_config["FMPKey"]}");
                if(result.IsSuccessStatusCode)
                {
                    var content=await result.Content.ReadAsStringAsync();
                    //task
                    var finData=JsonConvert.DeserializeObject<FMPStock[]>(content);
                    var stock=finData[0];
                    if(stock!=null)
                    {
                        return stock.ToStockFromFMP();

                    }
                return null;

                }
                return null;
            }

            catch(Exception e)
            
            {

            Console.WriteLine(e);
                return null;
            }

        }
    }
}