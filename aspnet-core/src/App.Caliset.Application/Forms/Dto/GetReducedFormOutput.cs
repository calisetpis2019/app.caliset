﻿using Abp.AutoMapper;
using App.Caliset.Models.Forms;

namespace App.Caliset.Forms
{
    [AutoMapFrom(typeof(Form))]
    public class GetReducedFormOutput
    {
        public int Id { get; set; }
        public string Name { get; set; }
    
        public string PathCompleto { get; set; }
    }
}