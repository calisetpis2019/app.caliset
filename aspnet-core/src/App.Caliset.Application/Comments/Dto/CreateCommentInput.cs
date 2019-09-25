using System;
using System.Collections.Generic;
using System.Text;

namespace App.Caliset.Comments.Dto
{
    public class CreateCommentInput
    {
        public string Commentary { get; set; }
        public int OperationId { get; set; }
    }
}
