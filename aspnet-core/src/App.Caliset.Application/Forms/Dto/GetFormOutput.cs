using Abp.AutoMapper;
using App.Caliset.Models.Forms;

namespace App.Caliset.Forms
{
    [AutoMapFrom(typeof(Form))]
    public class GetFormOutput
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public byte[] Photo { get; set; }

        public string PathCompleto { get; set; }
    }
}