namespace MusacaWebApp.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Product
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        [RegularExpression("^([0-9]{3,12})$", ErrorMessage = "Barcode may contain exact 12 numbers.")]
        public string Barcode { get; set; }

        public string Picture { get; set; }

    }
}
