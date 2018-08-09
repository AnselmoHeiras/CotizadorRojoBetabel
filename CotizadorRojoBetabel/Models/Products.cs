using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CotizadorRojoBetabel.Models
{
    internal class Products
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public ProductCategory Category { get; set; }

        public decimal Price { get; set; }

        public PackageUnit Unit { get; set; }

        public decimal InitialWeight { get; set; }

        public decimal Waste { get; set; }

        public decimal Yield { get; set; }

        public decimal YieldFactor { get; set; }

        public decimal FinalWeight { get; set; }

        public decimal DrainWeight { get; set; }

        public decimal Cost { get; set; }

    }

    internal enum ProductCategory
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

    internal enum PackageUnit
    {
        Kilogramos,
        Gramos,
        Mililitros,
        Piezas
    }
}
