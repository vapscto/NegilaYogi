using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class ClgYearWiseStudentDTO : CommonParamDTO
    {
        //public long ASYST_Id { get; set; }
        //public long AMST_Id { get; set; }
        //public long ASMCL_Id { get; set; }
        //public long ASMS_Id { get; set; }
        //public long AMAY_Id { get; set; }
        //public long AMAY_RollNo { get; set; }
        //public int AMAY_PassFailFlag { get; set; }
        //public long LoginId { get; set; }
        //public DateTime AMAY_DateTime { get; set; }
        //public int AMAY_ActiveFlag { get; set; }
        public Array StudentList { get; set; }
        public Array StudentListYear { get; set; }

        public UpdateRollNo[] UpdateRollNo { get; set; }
        public UpdateRollNo1[] UpdateRollNo1 { get; set; }


        public Array YearList { get; set; }
        public Array classList { get; set; }
        public Array sectionList { get; set; }

        //public SchoolYearWiseStudentDTO[] resultData { get; set; }
        //public string AMST_FirstName { get; set; }
        //public string AMST_MiddleName { get; set; }
        //public string AMST_LastName { get; set; }
        //public string ASMCL_ClassName { get; set; }
        //public string ASMC_SectionName { get; set; }
        //public string ASMAY_Year { get; set; }
        public string SectionAllotmentType { get; set; }
        public int NoOfYears { get; set; }
        public int MI_Id { get; set; }
        public TempclassDTO[] TempararyArrayListclass { get; set; }
        public TempcoloumnDTO[] TempararyArrayListcoloumn { get; set; }
        public string AMST_SOL { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime ASA_FromDate { get; set; }
        public Array SearchstudentDetails { get; set; }
        public string returnMsg { get; set; }
        public Array sectionAllotedStudentList { get; set; }
        public bool returnVal { get; set; }
        public int count { get; set; }
        public int studentlistCount { get; set; }
        public int studentListYearCount { get; set; }
        public Array newstudlist { get; set; }
        public long AMAY_Id_New { get; set; }
        public long ASMCL_Id_New { get; set; }
        //  public string AMST_AdmNo { get; set; }
        public long ASMS_Id_New { get; set; }
        public string StudentName { get; set; }

        //added by vishnu
        public bool returnal { get; set; }
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

        //added by vishnu 
        //public Array studentlisturn { get; set; }
        public Array tempstudentlist { get; set; }
        //public Student_Update_RollNumber[] studentlisturn { get; set; }
        //public Student_Update_RollNumber[] studentlisturn1 { get; set; }
        public Array semlist { get; set; }
        public Array coursebranchlist { get; set; }
        public long AMB_Id { get; set; }
        public string AMB_BranchName { get; set; }
        public long AMCO_Id { get; set; }
        public string AMCO_CourseName { get; set; }
        public long AMCOBM_Id { get; set; }
        public Array courselist { get; set; }
        public ClgYearWiseStudentDTO[] resultData1 { get; set; }

        //public long ACMAY_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long? AMCOC_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long ACYST_RollNo { get; set; }
        public string AMSE_SEMName { get; set; }
        public string ACMS_SectionName { get; set; }
        public string AMCST_AdmNo { get; set; }
        public long ACYST_Id { get; set; }
        public string AMCST_FirstName { get; set; }
        public string AMCST_MiddleName { get; set; }
        public string AMCST_LastName { get; set; }
        public string ASMAY_Year { get; set; }
        public bool AYST_PassFailFlag { get; set; }
        public int ACYST_ActiveFlag { get; set; }
        public long LoginId { get; set; }
        public DateTime? ACYST_DateTime { get; set; }
        public long ACAYC_Id { get; set; }
        public long ACAYCB_Id { get; set; }
        public DateTime? ACAYCBS_SemEndDate { get; set; }
        public string AMSE_EvenOdd { get; set; }
        public Array prosemlist { get; set; }
        public Array promoyear { get; set; }
        public string promotedflag { get; set; }
        public Array studentList4ys { get; set; }
    }

    public class UpdateRollNo
    {
        public string StudentName { get; set; }
        public string ASMCL_ClassName { get; set; }
        public long AMSE_Id { get; set; }
        public long? AMCOC_Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long AMB_Id { get; set; }
        public string AMB_BranchName { get; set; }
        public long AMCO_Id { get; set; }
        public string AMCO_CourseName { get; set; }
        public long AMCOBM_Id { get; set; }
        public Array courselist { get; set; }


        public long ACMS_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long ACYST_RollNo { get; set; }
        public string AMSE_SEMName { get; set; }
        public string ACMS_SectionName { get; set; }
        public string AMCST_AdmNo { get; set; }
        public long ACYST_Id { get; set; }
        public string AMCST_FirstName { get; set; }
        public string AMCST_MiddleName { get; set; }
        public string AMCST_LastName { get; set; }
        public string ASMAY_Year { get; set; }
        public bool AYST_PassFailFlag { get; set; }
        public int ACYST_ActiveFlag { get; set; }
        public long LoginId { get; set; }
        public DateTime? ACYST_DateTime { get; set; }
        public long ACAYC_Id { get; set; }
        public long ACAYCB_Id { get; set; }
        public DateTime? ACAYCBS_SemEndDate { get; set; }
        public string AMSE_EvenOdd { get; set; }
        public Array prosemlist { get; set; }
        public Array promoyear { get; set; }
        public string promotedflag { get; set; }
        public Array studentList4ys { get; set; }

        public string ASMC_SectionName { get; set; }
        public string AMST_AdmNo { get; set; }
        public long? pdays { get; set; }
        public long? AMST_Id { get; set; }
        public long? MI_Id { get; set; }
        public bool returnal { get; set; }
        public long? ASMCL_Id { get; set; }
        public long? ASMS_Id { get; set; }
        public long? ASYST_Id { get; set; }
        public long? ASMAY_Id { get; set; }
        public long? AMAY_RollNo { get; set; }
        public long? UserId { get; set; }

    }

    public class UpdateRollNo1
    {
        public string StudentName { get; set; }
        public string ASMCL_ClassName { get; set; }
        public long AMSE_Id { get; set; }
        public long? AMCOC_Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long AMB_Id { get; set; }
        public string AMB_BranchName { get; set; }
        public long AMCO_Id { get; set; }
        public string AMCO_CourseName { get; set; }
        public long AMCOBM_Id { get; set; }
        public Array courselist { get; set; }


        public long ACMS_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long ACYST_RollNo { get; set; }
        public string AMSE_SEMName { get; set; }
        public string ACMS_SectionName { get; set; }
        public string AMCST_AdmNo { get; set; }
        public long ACYST_Id { get; set; }
        public string AMCST_FirstName { get; set; }
        public string AMCST_MiddleName { get; set; }
        public string AMCST_LastName { get; set; }
        public string ASMAY_Year { get; set; }
        public bool AYST_PassFailFlag { get; set; }
        public int ACYST_ActiveFlag { get; set; }
        public long LoginId { get; set; }
        public DateTime? ACYST_DateTime { get; set; }
        public long ACAYC_Id { get; set; }
        public long ACAYCB_Id { get; set; }
        public DateTime? ACAYCBS_SemEndDate { get; set; }
        public string AMSE_EvenOdd { get; set; }
        public Array prosemlist { get; set; }
        public Array promoyear { get; set; }
        public string promotedflag { get; set; }
        public Array studentList4ys { get; set; }

        public string ASMC_SectionName { get; set; }
        public string AMST_AdmNo { get; set; }
        public long? pdays { get; set; }
        public long? AMST_Id { get; set; }
        public long? MI_Id { get; set; }
        public bool returnal { get; set; }
        public long? ASMCL_Id { get; set; }
        public long? ASMS_Id { get; set; }
        public long? ASYST_Id { get; set; }
        public long? ASMAY_Id { get; set; }
        public long? AMAY_RollNo { get; set; }
        public long? UserId { get; set; }

    }
}

