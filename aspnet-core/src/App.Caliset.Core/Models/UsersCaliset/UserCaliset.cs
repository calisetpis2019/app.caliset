using Abp.Authorization.Users;
using App.Caliset.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Caliset.Models.UsersCaliset
{
    public class UserCaliset : AbpUser<User>
    {
        public string LastName { get; set; }
        public int Document { get; set; }
        public String Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public String City { get; set; }
        public String Adress { get; set; }

    }
}
