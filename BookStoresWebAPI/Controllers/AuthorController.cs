using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoresWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoresWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Author> Get()
        {
            using (var context = new BookStoresDBContext())
            {


                // Get all authors ******************
                return context.Authors.ToList();


                // Get author by id ******************
                //return context.Authors.Where(auth => auth.AuthorId == 1).ToList();


                // To add an author ******************
                //Author author = new Author();
                //author.FirstName = "John";
                //author.LastName = "Smith";

                //context.Authors.Add(author);

                //context.SaveChanges();
                // return context.Authors.Where(auth => auth.FirstName == "John").ToList();


                // To update the author ******************
                //Author author = context.Authors.Where(auth => auth.FirstName == "John").FirstOrDefault();
                //author.Phone = "777-777-7777";
                //context.SaveChanges();
                // return context.Authors.Where(auth => auth.FirstName == "John").ToList();


                // To remove author
                //Author author = context.Authors.Where(auth => auth.FirstName == "John").FirstOrDefault();
                //context.Authors.Remove(author);
                //context.SaveChanges();
                //return context.Authors.Where(auth => auth.FirstName == "John").ToList();


            }
        }
    }
}
