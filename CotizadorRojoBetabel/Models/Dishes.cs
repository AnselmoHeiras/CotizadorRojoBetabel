using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CotizadorRojoBetabel.Models
{
    public class Dishes
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public DishesLine Line { get; set; }

        public int Portions { get; set; }

        public Products[] Ingredients { get; set; }

        public decimal Quantity { get; set; }

        [CustomField("BLOB")]
        public byte[] Draw { get; set; }

        public string Notes { get; set; }

        public string Instructions { get; set; }
    }

    public enum DishesLine
    {
        Fria,
        Caliente
    }
}
