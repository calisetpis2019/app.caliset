using Abp.AutoMapper;
using App.Caliset.Models.Forms;

namespace App.Caliset.Forms
{
    [AutoMapFrom(typeof(Form))]
    public class CreateFormInput 
    {
        public string Name { get; set; }

        public byte[] Photo { get; set; }
    }
}