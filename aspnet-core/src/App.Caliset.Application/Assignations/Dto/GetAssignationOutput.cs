using App.Caliset.Models.Operations;
using App.Caliset.Operations.Dto;
using App.Caliset.Users.Dto;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Caliset.Assignations.Dto
{
    public class GetAssignationOutput
    {
        public int Id { get; set; }
        public int InspectorId { get; set; }
        public UserDtoOperation Inspector { get; set; }
        public int OperationId { get; set; }
        public GetOperationOutput Operation { get; set; }
        public DateTime Date { get; set; }
        public bool? Aware { get; set; }
    }
}
