using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace RPGInterviewTask.DTOs
{
    public class Character
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string Race { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public int Damage { get; set; }
        public int Strength { get; set; }
        public int Agility { get; set; }
        public int Intelligence { get; set; }
        public int Range { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
