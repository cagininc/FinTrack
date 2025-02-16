using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stock;
using api.Models;

namespace api.Mappers
{
    public static class StockMappers
    {
        public static StockDto ToStockDto(this Stock stockModel)
        {
            return new  StockDto
            {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Purchase = stockModel.Purchase,
                LastDiv = stockModel.LastDiv,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap,
               Comments = stockModel.Comments.Select(c => c.ToCommentDto()).ToList()
            };
        }

        public static Stock ToStockFromCreateDTO(this CreateStockRequestDto  fmpStock)
        {
            return new Stock
            {
                Symbol =  fmpStock.Symbol,
                CompanyName =  fmpStock.CompanyName,
                Purchase =  fmpStock.Purchase,
                LastDiv =  fmpStock.LastDiv,
                Industry =  fmpStock.Industry,
                MarketCap =  fmpStock.MarketCap
            };
        }

public static Stock ToStockFromFMP(this FMPStock fmpStock)
        {
            return new Stock
            {
                Symbol =  fmpStock.symbol,
                CompanyName =  fmpStock.companyName,
                Purchase =   (decimal)fmpStock.price,
                LastDiv =  (decimal)fmpStock.lastDiv,
                Industry =  fmpStock.industry,
                MarketCap =  fmpStock.mktCap
            };
        }




    }
}