using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PreadmissionDTOs
{
    public class WrittenTestScheduleDTO : CommonParamDTO
    {

        public long PAWTS_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public int IVRMSTAUL_Id { get; set; }
         public string PAWTS_EntryDate { get; set; }
        public string PAWTS_ScheduleName { get; set; }
        public DateTime? PAWTS_ScheduleDate { get; set; }
        public string PAWTS_ScheduleTime { get; set; }
        public string PAWTS_ScheduleTimeTo { get; set; }
        public string PAWTS_AM_PM { get; set; }
        public string PAWTS_Remarks { get; set; }
        public Array WrittenTestSchedule { get; set; }
        public Array SelectedStudentDetails { get; set; }
        public long PASR_Id { get; set; }

        // public StudentDetailsDTOSave[] SelectedStudent { get; set; }
        public List<StudentDetailsDTO> SelectedStudentData { get; set; }
        public List<StudentDetailsDTO> SelectedStudentDataForEdit { get; set; }

        public string PAWTS_Superviser { get; set; }

        public string PAWTS_Skills { get; set; }

        public string returnvalue { get; set; }

    }
}
