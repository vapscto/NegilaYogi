using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs
{
    public class UploadinhouseDTo
    {
        public long IMINF_Id { get; set; }
        public long MI_Id { get; set; }
        public string IMINF_Referenceno { get; set; }
        public string IMINF_Filename { get; set; }
        public string IMINF_filepath { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public ICollection<IFormFile> File { get; set; }
        public UploadinhouseDTo[] data { get; set; }
    }
}
