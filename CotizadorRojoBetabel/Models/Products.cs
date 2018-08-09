using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CotizadorRojoBetabel.Models
{
    public class Products
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public ProductCategory Category { get; set; }

        public decimal Price { get; set; }

        public PackageUnit Unit { get; set; }

        public decimal Weight { get; set; }

        public decimal Waste { get; set; }

        public decimal Yield { get; set; }

        public decimal Cost { get; set; }

    }

    public enum ProductCategory
    {
        Frutas,
        Verduras,
        Carnes,
        Cerdo,
        Aves,
        Pescados,
        Mariscos,
        Lacteos,
        Abarrotes,
        Especias,
        Embalaje,
        Varios
    }

    public enum PackageUnit
    {
        Kilogramos,
        Gramos,
        Mililitros,
        Piezas
    }
}
