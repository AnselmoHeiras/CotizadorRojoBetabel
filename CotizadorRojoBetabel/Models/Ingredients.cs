using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CotizadorRojoBetabel.Models
{
    public class Ingredients
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(Dishes))]
        public int Dish { get; set; }

        public Products Ingredient { get; set; }

        public decimal Quantity { get; set; }
    }
}
