using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class StudentMappingDTO
    {
        public int ESTSU_Id { get; set; }
        public long UserId { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public int? EME_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ISMS_Id { get; set; }
        public bool ESTSU_ElecetiveFlag { get; set; }
        public bool ESTSU_ActiveFlg { get; set; }
        public Array exammastername { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public Array editlist {get;set;}
        public Array yearlist { get; set; }
        public Array ctlist { get; set; }
        public Array grouplist { get; set; }
        public Array classlist { get; set; }
        public Array seclist { get; set; }
        public Array examlist { get; set; }
        public Array subjlist { get; set; }
        public Array studlist { get; set; }
        public Array question_papertype_list { get; set; }
        public Array studmaplist { get; set; }
        public Array configuration { get; set; }
        public Array gtdetailsview { get; set; }
        public Array edclasslist { get; set; }
        public Array allstudent_details { get; set; }
        public tempDTO[] get_list { get; set; }
        public tempDTO[] get_Removed_list { get; set; }
        public int EMG_Id { get; set; }
        public string EMG_GroupName { get; set; }
        public int EMCA_Id { get; set; }
        public string EMCA_CategoryName { get; set; }
        public int ECAC_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public string AMST_FirstName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string ASMAY_Year { get; set; }
        public long AMAY_RollNo { get; set; }
        public string AMST_AdmNo { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public string AMST_SOL { get; set; }
        public DateTime AMST_DOB { get; set; }
        public bool? Left_Flag { get; set; }
        public bool? Deactive_Flag { get; set; }
        public Temp_SubjectList[] Temp_SubjectList { get; set; }
        public Selected_students[] Selected_students { get; set; }
        public Selected_RemoveRecordsList[] Selected_RemoveRecordsList { get; set; }
    }
    public class Temp_SubjectList
    {
        public long id { get; set; }
        public string name { get; set; }
    }
    public class Selected_students
    {
        public long AMST_Id { get; set; }
        public int ESEWPT_Id { get; set; }
        public int EMPATY_Id { get; set; }        
    }
    public class Selected_RemoveRecordsList
    {
        public long AMST_Id { get; set; }
        public int ESEWPT_Id { get; set; }
        public int EMPATY_Id { get; set; }
    }
}