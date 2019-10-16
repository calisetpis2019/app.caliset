using System;
using System.Collections.Generic;
using System.Text;

namespace App.Caliset.UserFiles.Dto
{
    public class GetUserFileOutput
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int UserId { get; set; }
    }
}
