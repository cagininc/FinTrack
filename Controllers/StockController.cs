using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using api.Data;
using api.Mappers; 
using api.Dtos.Stock;


namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController: ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public StockController (ApplicationDBContext context)
{
    _context = context;

}
[HttpGet]

public IActionResult GetAll()
{
    var stocks = _context.Stocks.ToList()
    .Select(s=>s.ToStockDto());
    return Ok(stocks);
}
[HttpGet("{id}")]
public IActionResult GetById([FromRoute] int id)
{
    var stock= _context.Stocks.Find(id);
    if (stock ==null){

        return NotFound();
    }
        return Ok(stock.ToStockDto());
}



[HttpPost]
public IActionResult Create( [FromBody] CreateStockRequestDto stockDto)
{
    

    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }
    var StockModel= stockDto.ToStockFromCreateDTO();
    _context.Stocks.Add(StockModel);
    _context.SaveChanges();
    return CreatedAtAction(nameof(GetById),new {id = StockModel.Id},StockModel.ToStockDto());

}

    }
}