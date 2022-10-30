using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CStore.Models
{
    [Table("Products")]
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public string? SKU { get; set; }

        public double Price { get; set; }

        public Brand Brand { get; set; }

        public Category Category { get; set; }
    }

    [Table("Brands")]
    public class Brand
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }
    }

    [Table("Categories")]
    public class Category
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }
    }
}
