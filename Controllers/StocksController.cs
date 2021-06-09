using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using los_api.Models;

namespace los_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly ApplicationDBContext _db;

        public StocksController(ApplicationDBContext context)
        {
            _db = context;
        
           
        }

       


        // GET: api/Stocks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Stock>>> Getstocks()
        {
            return await _db.stocks.ToListAsync();
        }

        // GET: api/Stocks/5
        [HttpGet("{id}")]
        public dynamic GetStock(string id)
        {
            var query = from p in _db.products
                       join st in _db.stocks on p.Id equals st.productId
                       where p.Id == id
                       select new { 
                           ProductID = p.Id,
                           ProductName = p.name,
                           ProductImg = p.imageurl,
                           PriceProduct = p.price,
                           Amount = st.amount

                       };

            if (query == null)
            {
                return NotFound();
            }

            return query;
        }

        // PUT: api/Stocks/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStock(string id, Stock stock)
        {
            if (id != stock.Id)
            {
                return BadRequest();
            }

            _db.Entry(stock).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StockExists(id))
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

        // POST: api/Stocks
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Stock>> PostStock(Stock stock)
        {
            _db.stocks.Add(stock);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StockExists(stock.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStock", new { id = stock.Id }, stock);
        }

        // DELETE: api/Stocks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Stock>> DeleteStock(string id)
        {
            var stock = await _db.stocks.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }

            _db.stocks.Remove(stock);
            await _db.SaveChangesAsync();

            return stock;
        }

        private bool StockExists(string id)
        {
            return _db.stocks.Any(e => e.Id == id);
        }
    }
}
