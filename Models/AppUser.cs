using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    public class AppUser : IdentityUser
    {

        
        //  public ClaimsIdentity? GivenUserName { get; internal set; }

        public List<Portfolio> Portfolios{get;set;}= new List<Portfolio>();



    }
}