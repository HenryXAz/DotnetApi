using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetApi.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public String Symbol {get; set;} = String.Empty;
        public String CompanyName { get; set; } = String.Empty;

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Purchase {get; set;}

        [Column(TypeName = "decimal(10, 2)")]
        public decimal LastDiv {get; set;}
        public String Industry {get; set;} = String.Empty;
        public long  MarketCap {get; set;}

        public List<Comment> Comments {get; set;} = [];
    }
}
