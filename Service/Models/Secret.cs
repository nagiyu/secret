using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    public class Secret
    {
        public int Id { get; set; }
        public string? Key { get; set; }
        public string? Value { get; set; }
    }
}
