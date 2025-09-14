using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORDService.Domain.Entities
{
    public class ResearchProject
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Leader { get; set; }
        public int Year { get; set; }
        public decimal Budget { get; set; }
        public string Status { get; set; }
    }
}
