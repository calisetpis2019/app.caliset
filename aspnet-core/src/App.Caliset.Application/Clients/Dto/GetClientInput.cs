using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace App.Caliset.Clients.Dto
{
    public class GetClientInput
    {
        [Required]
        public int Id { get; set; }
    }
}
