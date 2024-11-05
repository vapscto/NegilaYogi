using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class SchoolYearWiseStudentDTO : CommonParamDTO
    {
        public long ASYST_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMAY_Id { get; set; }
        public long AMAY_RollNo { get; set; }
        public int AMAY_PassFailFlag { get; set; }
        public long LoginId { get; set; }
        public DateTime AMAY_DateTime { get; set; }
        public int AMAY_ActiveFlag { get; set; }
        public Array StudentList { get; set; }
        public Array StudentListYear { get; set; }
        public Array YearList { get; set; }
        public Array classList { get; set; }
        public Array sectionList { get; set; }
        public SchoolYearWiseStudentDTO[] resultData { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string ASMAY_Year { get; set; }
        public string SectionAllotmentType { get; set; }
        public int NoOfYears { get; set; }
        public int MI_Id { get; set; }
        public string message { get; set; }
        public AbsentManualsms[] absentlist { get; set; }
        public TempclassDTO[] TempararyArrayListclass { get; set; }
        public TempcoloumnDTO[] TempararyArrayListcoloumn { get; set; }
        public string AMST_SOL { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime ASA_FromDate { get; set; }
        public Array SearchstudentDetails { get; set; }
        public string returnMsg { get; set; }
        public Array sectionAllotedStudentList { get; set; }
        public bool returnVal { get; set; }
        public bool returnValattendance { get; set; }
        
        public int count { get; set; }
        public int studentlistCount { get; set; }
        public int studentListYearCount { get; set; }
        public Array newstudlist { get; set; }
        public long AMAY_Id_New { get; set; }
        public long ASMCL_Id_New { get; set; }
        public string AMST_AdmNo { get; set; }
        public long ASMS_Id_New { get; set; }
        public string StudentName { get; set; }
        public long ASMAY_Id_Previous { get; set; }
        public string username { get; set; }
        public long Emp_Code { get; set; }
        public long ASALU_Id { get; set; }
        public long userId { get; set; }
        public string flag { get; set; }
        public long roleId { get; set; }
        public string rolename { get; set; }
        public Array stddto { get; set; }
        public Array defalutYearList { get; set; }    
        public Array tempstudentlist { get; set; }
        public Student_Update_RollNumber[] studentlisturn { get; set; }
        public Student_Update_RollNumber[] studentlisturn1 { get; set; }
        public Array student1 { get;set;}
        public Array newclasslist { get;set;}
        public Array studentdetails { get;set;}
        public long ASMCL_Id_CLS_New { get; set; }
        public long ASMCL_Id_CLS_Old { get; set; }
        public long Attendancecount { get; set; }
        public long Examcount { get; set; }
        public long Feecount { get; set; }
        public Array FeeCategoryDetails { get; set; }
        public string FMCC_ClassCategoryName { get; set; }
        public string Remarks { get; set; }
        public long FeeDeleteFlag { get; set; }

        public bool categoryflag { get; set; }

        public Array category_list { get; set; }
        public long AMC_Id { get; set; }
        public Array AMC_logo { get; set; }

        public Array section_update { get; set; }

        public bool attendanceDone { get; set; }
        public bool examMarksDone { get; set; }
        public DateTime? FromDate { get; set; }

    }
    public class AbsentManualsms
    {
        public DateTime? FromDate { get; set; }
        public long AMST_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public string AMST_FirstName { get; set; }
        public long AMST_MobileNo { get; set; }
    }

    public class Temp_SchoolYearWiseStudentDTO
    {
        public long Amst_Id { get; set; }
    }
}
