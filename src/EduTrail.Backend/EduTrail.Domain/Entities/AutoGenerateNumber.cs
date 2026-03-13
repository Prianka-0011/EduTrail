using System.ComponentModel.DataAnnotations;
using EduTrail.Domain.Interfaces;
namespace EduTrail.Domain.Entities
{
    public class AutoGenerateNumber 
    {
        [Key]
        public Guid Id { get; set; }
        public int Number { get; set; }
        public int Year { get; set; }
        public string Prefix { get; set; }

      
    }

}