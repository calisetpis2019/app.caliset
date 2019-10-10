using Abp.AutoMapper;
using App.Caliset.Authorization.Users;
using App.Caliset.Roles.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Caliset.Users.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserDtoOperation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
    }
}
