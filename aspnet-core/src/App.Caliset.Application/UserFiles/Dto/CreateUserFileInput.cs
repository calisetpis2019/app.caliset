using Abp.AutoMapper;
using App.Caliset.Models.UserFile;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Caliset.UserFiles.Dto
{
    [AutoMapFrom(typeof(UserFile))]
    public class CreateUserFileInput
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public byte[] Photo { get; set; }
    }
}
