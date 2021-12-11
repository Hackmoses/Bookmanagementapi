

using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;


namespace Bookmanagementapi.Models 
{
    public class BookList
    {
        public int Id { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]

        //[Index(IsUnique = true)]
        public string Book { get; set; }
        [Required]
        public DateTime DueDate { get; set; }

    }

   
}