using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace api.Models
{ [Table("Stocks")]

    public class Stock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Symbol { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,2)")]

        public decimal Purchase { get; set; }
        public decimal LastDiv { get; set; }

        public string Industry { get; set; } = string.Empty;

        public long MarketCap { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();

        public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();


    }
}