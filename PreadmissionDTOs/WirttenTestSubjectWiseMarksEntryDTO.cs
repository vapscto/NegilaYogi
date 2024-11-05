using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class WirttenTestSubjectWiseMarksEntryDTO : CommonParamDTO
    {
        public long PASWM_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime PASWM_Date { get; set; }
        public long PAMS_Id { get; set; }
        public string WrittenTestScheduleAppFlag { get; set; }
        public long PAWTS_Id { get; set; }
        public List<StudentDetailsDTO> SelectedStudentData { get; set; }
        public List<WrittenTestMarksBindDataDTO> SelectedStudentMarksData { get; set; }

        public Array studentDetails { get; set; }


       

    }
}
