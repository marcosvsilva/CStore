using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CStore.Context;
using CStore.Models;

namespace CStore.Controllers
{
    public class FavoriteController
    {
        private readonly CStoreContext _context;

        public FavoriteController(CStoreContext context)
        {
            _context = context;
        }

        public async Task<List<Product>?> GetProductsByUser(int userId)
        {
            List<Favorite>? favorites = GetFavoritesByUser(userId);

            if ((favorites != null) && (favorites.Count > 0))
            {
                List<Product>? products = new List<Product>();

                foreach (var favorite in favorites)
                {
                    Product? product = await _context.
                        Products.FindAsync(favorite.ProductId);

                    if (product != null)
                    {
                        product.Brand = await _context.
                            Brands.FindAsync(product.BrandId);

                        product.Category = await _context.
                            Categories.FindAsync(product.CategoryId);

                        products.Add(product);
                    }
                }

                return products;
            }

            return null;
        }

        public List<Favorite>? GetFavoritesByUser(int userId)
        {
            IQueryable<Favorite> favoritesQuery = _context
                .Favorites.Where(f => f.UserId == userId);

            if (favoritesQuery != null)
            {
                return favoritesQuery.ToList();
            }

            return null;
        }

        public async Task<List<Favorite>?> PostFavorites(List<Favorite> favorites)
        {
            if (favorites.Count <= 0)
            {
                return null;
            }

            List<Favorite> favoritesInserted = new List<Favorite>();
            foreach (var favorite in favorites)
            {
                if (!FavoriteExists(favorite.UserId, favorite.ProductId))
                {
                    if (favorite != null)
                    {
                        _context.Favorites.Add(favorite);
                        await _context.SaveChangesAsync();

                        favoritesInserted.Add(favorite);
                    }
                }
            }

            return favoritesInserted;
        }

        public async Task<List<Favorite>?> DeleteFavorites(List<Favorite> favorites)
        {
            if (favorites.Count <= 0)
            {
                return null;
            }

            List<Favorite> favoritesDeleted = new List<Favorite>();
            foreach (var item in favorites)
            {
                if (item != null)
                {
                    var favorite = await _context.Favorites.FindAsync(item.Id);
                    if (favorite != null)
                    {
                        _context.Favorites.Remove(favorite);
                        await _context.SaveChangesAsync();
                        favoritesDeleted.Add(item);
                    }
                }

            }

            return favoritesDeleted;
        }

        private bool FavoriteExists(int userId, int productId)
        {
            return (_context.Favorites?
                .Any(e => (e.ProductId == productId) && (e.UserId == userId)))
                .GetValueOrDefault();
        }
    }
}
