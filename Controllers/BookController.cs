
using System;
using System.Collections.Generic;
using System.Linq;
using Bookmanagementapi.data;
using Bookmanagementapi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookmanagementapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BookController : ControllerBase
    {

        private readonly AppDbContext _context;

        public BookController (AppDbContext context) 
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<BookList>> GetAll()
        {
            List<BookList> items = _context.BookLists.ToList();
            return Ok(items);
        }
         [HttpPost]
    
        public IActionResult Add(BookList model) 
        {
           

                if(model == null) throw new ArgumentNullException( message: "Todo cannot be null", null);

               _context.BookLists.Add(model);
               _context.SaveChanges();

               
               return CreatedAtAction("Add", model);
            
        }
        
        [HttpGet("{Id}")]
        public IActionResult SingleItem(int Id)
        {
        

            if (Id <= 0) return NotFound();
            
            var item = _context.BookLists.FirstOrDefault(x=> x.Id == Id);

            if (item == null )
            {
                
                return NotFound();
            }
            
            return Ok(item);
           
        }

        [HttpPut("{Id}")]
         public IActionResult EditItem(int Id, [FromBody]BookList model)
        {
            
            if (Id <= 0) return NotFound();
            if(model == null) throw new ArgumentNullException( message: "Todo cannot be null", null);
            var item = _context.BookLists.FirstOrDefault(x=> x.Id == Id);

            if (item == null )
            {
                
                return NotFound();
            }
            
            
                item.Author = model.Book;
                item.Book = model.Book;
                item.DueDate = model.DueDate;

               _context.BookLists.Update(item);
               _context.SaveChanges();

               return Ok(item);
            
        }

        [HttpDelete("{Id}")]
        public IActionResult DeleteItem(int Id) 
        {
            

                if (Id <= 0) return NotFound();

                var item = _context.BookLists.FirstOrDefault(x=> x.Id == Id);

            if (item == null )
            {
                return NotFound();
        
            }
                

               _context.BookLists.Remove(item);
               _context.SaveChanges();

               return Ok(item);
            }
    }
}