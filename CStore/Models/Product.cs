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

    [Table("Favorites")]
    public class Favorite
    {
        [Key]
        public int Id { get; set; }

        public User User { get; set; }

        public List<Product> Products { get; set } = List<Product>();
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
