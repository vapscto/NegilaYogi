using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class AdmSchoolAttendanceSubjectBatchDTO
    {
        public long ASASB_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMSU_Id { get; set; }
        public string ASASB_BatchName { get; set; }
        public long ASASB_StudentStrenth { get; set; }

        public Array YearList { get; set; }
        public Array classList { get; set; }
        public Array sectionList { get; set; }
        public Array SubjectList { get; set; }

        public Array SubjectBatchList { get; set; }
        public Array StudentList { get; set; }

        public string FormType { get; set; }

        public AdmSchoolAttendanceSubjectBatchStudentsDTO[] selectedstudents { get; set; }

        public long AMST_Id { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string AMS_SubjectName { get; set; }
        public string MI_Name { get; set; }
        public string ASMAY_Year { get; set; }
        public long ASASBS_Id { get; set; }
        public Array EditedStudentList { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? CreatedDate { get; set; }

        public Array batchList { get; set; }

        public string message { get; set; }
        public string AMSU_Name { get; set; }
        public int batchcount { get; set; }
        public int studentCount { get; set; }
        public bool returnVal { get; set; }
        public Array batchwisestdlist { get; set; }
        public string regno { get; set; }
        public string studentname { get; set; }
        public string admno { get; set; }
        public int countbatchlist { get; set; }
        //public long amsT_Id { get; set; }
        public Array activityIds { get; set; }
        public long ASASB_Id1 { get; set; }
        public long type12 { get; set; }
        public string amsT_AdmNo { get; set; }
        public string amsT_RegistrationNo { get; set; }
        
    }
}
