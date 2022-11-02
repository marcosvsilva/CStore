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
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly CStoreContext _context;
        private FavoriteController _favoriteController;

        public UserController(CStoreContext context)
        {
            _context = context;
            _favoriteController = new FavoriteController(_context);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            if (_context.Users == null)
            {
                return NotFound();
            }

            List<User> users = await _context.Users.ToListAsync();

            foreach (var user in users)
            {
                if (user != null)
                {
                    List<Product>? products =
                        await _favoriteController.GetProductsByUser(user.Id);

                    if (products != null)
                    {
                        user.Favorites = products;
                    }
                }
            }

            return users;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            List<Product>? products =
                await _favoriteController.GetProductsByUser(user.Id);

            if (products != null)
            {
                user.Favorites = products;
            }

            return user;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                await InsertFavorites(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'CStoreContext.Users' is null.");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            await InsertFavorites(user);

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<ActionResult> InsertFavorites(User user)
        {
            if (user.Id > 0 && user.Favorites.Count > 0)
            {
                List<Favorite>? favoritesDelete =
                    _favoriteController.GetFavoritesByUser(user.Id);

                if (favoritesDelete != null && favoritesDelete.Count > 0)
                {
                    await _favoriteController.DeleteFavorites(favoritesDelete);
                }

                List<Favorite> favoritesInsert = new List<Favorite>();

                foreach (var product in user.Favorites)
                {
                    Favorite favorite = new Favorite();
                    favorite.UserId = user.Id;
                    favorite.ProductId = product.Id;
                    favoritesInsert.Add(favorite);
                }

                List<Favorite>? valid =
                    await _favoriteController.PostFavorites(favoritesInsert);

                if (valid == null || valid.Count <= 0)
                {
                    return Problem("Problem with insert favorites.");
                }
            }

            return Ok();
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
