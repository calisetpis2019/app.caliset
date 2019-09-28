﻿using App.Caliset.Users.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Caliset.Comments.Dto
{
    public class GetCommentOutput
    {
        public int Id { get; set; }
        public string Commentary { get; set; }
        public UserDtoOperation CreatorUser { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
