using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class UserDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
