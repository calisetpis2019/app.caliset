using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using App.Caliset.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Caliset.Users.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserUpdateRolesDto
    {
        public long IdUser { get; set; }
        public string Role { get; set; }
    }
    
    
}
