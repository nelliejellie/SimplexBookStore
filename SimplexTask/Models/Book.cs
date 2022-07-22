using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimplexTask.Models
{
    public class Book
    {
        public Book()
        {
            CreatedOn = DateTime.Now;
        }
        [Key]
        public Guid Id { get; set; }
        [Range(1,5)]
        public string Rating { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedOn { get; set; }
        
    }
}
