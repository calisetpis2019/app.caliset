using Abp.AutoMapper;
using App.Caliset.Clients.Dto;
using App.Caliset.Comments.Dto;
using App.Caliset.Locations.Dto;
using App.Caliset.Models.Operations;
using App.Caliset.OperationStates.Dto;
using App.Caliset.OperationTypes.Dto;
using App.Caliset.Users.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace App.Caliset.Operations.Dto
{
    [AutoMapFrom(typeof(Operation))]
    public class CreateOperationInput
    {
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Commodity { get; set; }
        [Required]
        public string Package { get; set; }
        public string ShipName { get; set; }
        public string Destiny { get; set; }
        public string ClientReference { get; set; }
        public string Line { get; set; }
        public string BookingNumber { get; set; }
        public string Notes { get; set; }

        [Required]
        public  int LocationId { get; set; }
        [Required]
        public int OperationTypeId { get; set; }
        [Required]
        public int NominatorId { get; set; }
        [Required]
        public int ChargerId { get; set; }
        [Required]
        public long ManagerId { get; set; }
        
    }
}
