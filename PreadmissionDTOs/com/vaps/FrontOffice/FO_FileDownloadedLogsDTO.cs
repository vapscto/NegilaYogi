using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.FrontOffice
{
    public class FO_FileDownloadedLogsDTO
    {
        public long FODLL_Id { get; set; }
        public long MI_Id { get; set; }
        public string FODLL_Date { get; set; }
        public string FODLL_time { get; set; }
        public string FODLL_JSONData { get; set; }
    }
}
