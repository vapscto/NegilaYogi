using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace PreadmissionDTOs
{
    public class InstitutionImageDTO : CommonParamDTO
    {
        public int MI_Id { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public string ImageType { get; set; }

        public IFormFile File { get; set; }

       // public ICollection<IFormFile> Logo { get; set; }
    }
}
