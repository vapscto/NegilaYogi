using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Exam
{
    public class Exm_Col_Master_Group_SubjectsDTO : CommonParamDTO
    {
        public long ECMGS_Id { get; set; }
        public int EMG_Id { get; set; }
        public long ISMS_Id { get; set; }
        public bool ECMGS_ActiveFlag { get; set; }
        public Array view_group_subjects { get; set; }
        public string EMG_GroupName { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string ISMS_SubjectCode { get; set; }
    }
}
