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
    public class OrderController : ControllerBase
    {
        private readonly CStoreContext _context;

        public OrderController(CStoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }

            List<Order> orders = await _context.Orders.ToListAsync();

            foreach (var order in orders)
            {
                if (order != null)
                {
                    order.User = await _context.Users.FindAsync(order.UserId);
                    order.Payment = await _context.Payments.FindAsync(order.PaymentId);

                    IQueryable<Item>? itemsQuery =
                        _context.Items.Where(i => i.OrderId == order.Id);

                    if (itemsQuery != null)
                    {
                        List<Item>? items = itemsQuery.ToList();
                        if (items != null)
                        {
                            order.Items = items;
                        }
                    }
                }
            }

            return orders;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);

            if (order != null)
            {
                order.User = await _context.Users.FindAsync(order.UserId);
                order.Payment = await _context.Payments.FindAsync(order.PaymentId);

                IQueryable<Item>? itemsQuery =
                    _context.Items.Where(i => i.OrderId == order.Id);

                if (itemsQuery != null)
                {
                    List<Item>? items = itemsQuery.ToList();
                    if (items != null)
                    {
                        order.Items = items;
                    }
                }
            }

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'CStoreContext.Orders'  is null.");
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
