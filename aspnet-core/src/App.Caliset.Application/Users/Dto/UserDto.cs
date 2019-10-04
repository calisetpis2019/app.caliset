using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using App.Caliset.Authorization.Users;

namespace App.Caliset.Users.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserDto : EntityDto<long>
    {
        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        public bool IsActive { get; set; }

        public string FullName { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public DateTime CreationTime { get; set; }

        public string[] RoleNames { get; set; }



        public int Document { get; set; }
        public string Phone { get; set; }
        public DateTime? BirthDate { get; set; }
        public string City { get; set; }
        public string Adress { get; set; }
        public bool FirstLogin { get; set; }

    }
}
