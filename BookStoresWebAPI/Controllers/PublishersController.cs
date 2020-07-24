﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoresWebAPI.Models;

namespace BookStoresWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly BookStoresDBContext _context;

        public PublishersController(BookStoresDBContext context)
        {
            _context = context;
        }

        // GET: api/Publishers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Publisher>>> GetPublishers()
        {
            return await _context.Publishers.ToListAsync();
        }

        // GET: api/Publishers/5
        [HttpGet("PostPublisherDetails/")]
        public async Task<ActionResult<Publisher>> PostPublisherDetails()
        {

            var publisher = new Publisher();
            publisher.PublisherName = "Harper & Brothers";
            publisher.City = "New York City";
            publisher.State = "NY";
            publisher.Country = "USA";

            Book book1 = new Book();
            book1.Title = "Good Night Moon - 1";
            book1.PublishedDate = DateTime.Now;

            Book book2 = new Book();
            book2.Title = "Good Night Moon - 2";
            book2.PublishedDate = DateTime.Now;

            Sale sale1 = new Sale();
            sale1.Quantity = 2;
            sale1.StoreId = "8042";
            sale1.OrderNum = "XYZ";
            sale1.PayTerms = "Net 30";
            sale1.OrderDate = DateTime.Now;

            Sale sale2 = new Sale();
            sale2.Quantity = 2;
            sale2.StoreId = "7131";
            sale2.OrderNum = "QA879.1";
            sale2.PayTerms = "Net 20";
            sale2.OrderDate = DateTime.Now;

            book1.Sales.Add(sale1);
            book2.Sales.Add(sale2);

            publisher.Books.Add(book1);
            publisher.Books.Add(book2);

            _context.Publishers.Add(publisher);
            _context.SaveChanges();

            var publishers = await _context.Publishers
                .Include(pub => pub.Books)
                    .ThenInclude(book => book.Sales)
                .Include(pub => pub.Users)
                    .ThenInclude(user => user.Job)
                .Where(pub => pub.PubId == publisher.PubId)
                .FirstOrDefaultAsync();

            if (publishers == null)
            {
                return NotFound();
            }

            return publishers;
        }


        // GET: api/Publishers/5
        [HttpGet("GetPublisherDetails/{id}")]
        public async Task<ActionResult<Publisher>> GetPublisherDetails(int id)
        {
            // Eager Loading
            //var publisher = await _context.Publishers
            //    .Include(pub => pub.Books)
            //        .ThenInclude(book => book.Sales)
            //    .Include(pub => pub.Users)
            //        .ThenInclude(user => user.Job)
            //    .Where(pub => pub.PubId == id)
            //    .FirstOrDefaultAsync();

            // Explicit Loading
            var publisher = await _context.Publishers
                            .SingleAsync(pub => pub.PubId == id);

            _context.Entry(publisher)
                    .Collection(pub => pub.Users)
                    .Query()
                    //.Where(usr => usr.EmailAddress.Contains("karin")) 
                    .Load();

            _context.Entry(publisher)
                    .Collection(pub => pub.Books)
                    .Query()
                    .Include(book => book.Sales)
                    .Load();

            //var user = await _context.Users.SingleAsync(usr => usr.UserId == 1);

            //_context.Entry(user)
            //        .Reference(usr => usr.Role)
            //        .Load();


            if (publisher == null)
            {
                return NotFound();
            }

            return publisher;
        }

        // GET: api/Publishers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Publisher>> GetPublisher(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);

            if (publisher == null)
            {
                return NotFound();
            }

            return publisher;
        }

        // PUT: api/Publishers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPublisher(int id, Publisher publisher)
        {
            if (id != publisher.PubId)
            {
                return BadRequest();
            }

            _context.Entry(publisher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PublisherExists(id))
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

        // POST: api/Publishers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Publisher>> PostPublisher(Publisher publisher)
        {
            _context.Publishers.Add(publisher);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPublisher", new { id = publisher.PubId }, publisher);
        }

        // DELETE: api/Publishers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Publisher>> DeletePublisher(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher == null)
            {
                return NotFound();
            }

            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();

            return publisher;
        }

        private bool PublisherExists(int id)
        {
            return _context.Publishers.Any(e => e.PubId == id);
        }
    }
}
