using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetApi.Models
{
    [Table("Portfolios")]
    public class Portfolio
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int StockId { get; set; }
        public AppUser AppUser { get; set; } 
        public Stock Stock { get; set; }
    }
}