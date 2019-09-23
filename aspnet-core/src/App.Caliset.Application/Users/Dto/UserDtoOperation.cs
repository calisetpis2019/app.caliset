using Abp.AutoMapper;
using App.Caliset.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Caliset.Users.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserDtoOperation
    {
        public int Id { get; set; }

    }
}
