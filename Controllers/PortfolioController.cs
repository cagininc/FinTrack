using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions;
using api.interfaces;
using api.Models;
using api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepo;

        private readonly IPortfolioRepository _portfolioRepo;
        public PortfolioController(UserManager<AppUser> userManager, IStockRepository
        stockRepo, IPortfolioRepository portfolioRepo)
        {
            _userManager = userManager;
            _stockRepo = stockRepo;
            _portfolioRepo = portfolioRepo;

        }

        [HttpGet]
       [Authorize]

        public async Task<IActionResult> GetUserPortfolio()
        {
            //CHECK
            var allClaims = User.Claims.Select(c => $"{c.Type} = {c.Value}");
Console.WriteLine("DEBUG: Claims in the current token:");
foreach (var c in allClaims)
{
    Console.WriteLine($"    {c}");
}

            var username = User.GetUsername();
            Console.WriteLine($"DEBUG:given_name={username}");
            var AppUser = await _userManager.FindByNameAsync(username);

            if(AppUser==null)
            {
                    return NotFound("User not found in DB");

            }


            var userPortfolio = await _portfolioRepo.GetUserPortfolio(AppUser);
           
            return Ok(userPortfolio);

        }

    }
}