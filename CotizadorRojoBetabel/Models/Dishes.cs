using LibreR.Controllers;
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

        public Ingredients[] Ingredients { get; set; }

        [CustomField("BLOB")]
        public byte[] Draw { get; set; }

        public string Notes { get; set; }

        public string Instructions { get; set; }

        public override string ToString()
        {
            var draw = (Draw == null) ? false : true;
            return $"ID: {Id}, Name: {Name},  Line: {Line.ToString()}, Portions: {Portions.ToString()}, " +
                $"Ingredients: {Ingredients.Serialize(LibreR.Models.Enums.Serializer.OneLine)}, Draw: {draw}, Notes: {Notes}, Instructions: {Instructions}";
        }
    }

    public enum DishesLine
    {
        Fria,
        Caliente
    }
}
