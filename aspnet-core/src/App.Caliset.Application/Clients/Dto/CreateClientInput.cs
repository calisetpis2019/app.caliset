using Abp.AutoMapper;
using App.Caliset.Models.Clients;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace App.Caliset.Clients.Dto
{
    [AutoMapFrom(typeof(Client))]
    public class CreateClientInput
    {
      
        public string Name { get; set; }
    }
}
