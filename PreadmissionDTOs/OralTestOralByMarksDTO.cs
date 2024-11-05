using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class OralTestOralByMarksDTO : CommonParamDTO
    {
        public long PAOTM_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime PAOTM_EntryDate { get; set; }
        public int PAOTM_OralBy { get; set; }
        public string OralTestScheduleAppFlag { get; set; }
        public long PAOTS_Id { get; set; }
        public List<StudentDetailsDTO> SelectedStudentData { get; set; }
        public List<OralTestMarksBindDataDTO> SelectedStudentMarksData { get; set; }
        public Array studentDetails { get; set; }


    }
}
