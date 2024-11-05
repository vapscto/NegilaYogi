using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class ImportStudentWrapperDTO
    {
        public Int64 MI_Id { get; set; }
        public Int64 ASMAY_Id { get; set; }
        public long User_Id { get; set; }
        public string stuStatus { get; set; }
        public List<ImportStudentDTO> newlstget { get; set; }
        public ImportStudentDTO[] newlstget1 { get; set; }
        public string returnMsg { get; set; }
        public string resp { get; set; }
        public long useridapp { get; set; }
        public long useridappfat { get; set; }
        public long useridappmot { get; set; }
        public Array failedlist { get; set; }
    }
}