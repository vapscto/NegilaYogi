using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class JSHSExamReportsDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public long EMGR_Id { get; set; }
        public long ECT_Id { get; set; }
        public long Userid { get; set; }
        public long Roleid { get; set; }
        public long Emp_Code { get; set; }
        public long HRME_Id { get; set; }
        public string UserName { get; set; }
        public string flag { get; set; }
        public string rolename { get; set; }
        public string studentname { get; set; }
        public string AMST_Sex { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public Array getyearlist { get; set; }
        public Array getclasslist { get; set; }
        public Array getsectionlist { get; set; }
        public Array getstudentlist { get; set; }
        public Array getgradelist { get; set; }
        public Array getallgrade { get; set; }
        public Array gettermlist { get; set; }
        public Array getgradedetails { get; set; }
        public Array getgradetermlist { get; set; }
        public Array gettermdetails { get; set; }
        public Array gettermexamdetails { get; set; }
        public Array getstudentmarksdetails { get; set; }
        public Array getstudentmarksindidetails { get; set; }
        public Array getstudentdetails { get; set; }
        public Array getstudentwisesubjectlist { get; set; }
        public Array getstudentwisesubjectsubsubjectlist { get; set; }
        public Array getstudentwisesubjectlistnew { get; set; }
        public Array getstudentwiseskillslist { get; set; }
        public Array getstudentwiseactiviteslist { get; set; }
        public Array getstudentwisesportsdetails { get; set; }
        public Array getstudentwiseattendancedetails { get; set; }
        public Array exam_promotion_groupwise_remarks { get; set; }
        public Array getstudentwisetermwisedetails { get; set; }
        public Array getcumulativereportdetails { get; set; }
        public Array getsubjectslist { get; set; }
        public Array clstchname { get; set; }
        public Array examwiseremarks { get; set; }
        public Array getsudentwisehousedetails { get; set; }
        public Array studentwisemarks { get; set; }
        public Array getsubjectwisetotaldetails { get; set; }
        public temp_JSHSExamReportsDTO[] termlist { get; set; }
        public temp_ExamlistDTO[] examlist { get; set; }
        public temp_groupDTO[] temp_groupDTO { get; set; }
        public Temp_AmstIds[] Temp_AmstIds { get; set; }

        // Term Exam Details 
        public long EME_Id { get; set; }
        public long ISMS_Id { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string EME_ExamName { get; set; }
        public string EME_ExamCode { get; set; }
        public int EME_ExamOrder { get; set; }
        public decimal? ECTEX_MarksPercentValue { get; set; }
        public decimal? EYCES_MaxMarks { get; set; }
        public Array getinstitution { get; set; }
        public Array getexam { get; set; }
        public Array getselectedexamlist { get; set; }
        public Array getsubjects { get; set; }
        public Array getexamsubjectwisereport { get; set; }
        public Array getexamwisesubsubjectlist { get; set; }
        public string EMSS_SubSubjectName { get; set; }
        public string EME_ExamDescription { get; set; }
        public int EMSS_Order { get; set; }
        public int EMSS_Id { get; set; }       
        public decimal? EYCESSS_MaxMarks { get; set; }
        public string EYCESSS_Grade { get; set; }
        public string institutionname { get; set; }
        public string institutionaddress { get; set; }
        public Array getgradereport { get; set; }
        public int? institutionpincode { get; set; }
        public string checkoruncheckflag { get; set; }
        public Array getmultipleexamcumulativereport { get; set; }
        public Array getexamdetails { get; set; }
        public Array getgroupdetails { get; set; }
        public Array getgroupdetailsnew { get; set; }
        public Array getgroupexamdetails { get; set; }
        public Array getpromotionmarksdetails { get; set; }
        public Array getpromotionremarksdetails { get; set; }
        public Array getparticipatedetails { get; set; }
        public Array getexamwisetotaldetails { get; set; }
        public Array getexamsubjectwisemarksdetails { get; set; }
        public int EMPSG_Id { get; set; }
        public int EMPS_Id { get; set; }
        public string EMPG_GroupName { get; set; }
        public decimal? EMPSG_PercentValue { get; set; }
        public decimal? EMPSGE_ForMaxMarkrs { get; set; }
        public string EMPG_DistplayName { get; set; }
        public string reporttype { get; set; }
        public int? EMPSG_Order { get; set; }
        public Array getclassteacher { get; set; }
        public string classteachername { get; set; }
        public string flagtype { get; set; }
        public decimal? EMPSG_MarksValue { get; set; }
        public Array getexammaxmarks { get; set; }
        public Array Present_attendence { get; set; }
        public Array Work_attendence { get; set; }
        public Array getstudent_examwisemarks { get; set; }
        public Array nonapplicablesubject_examwisemarks { get; set; }
        public Array ExamWise_PaperType { get; set; }
        public bool? ECTEX_NotApplToTotalFlg { get; set; }
        public int EMPATY_Id { get; set; }
        public string EMPATY_PaperTypeName { get; set; }
        public string EMPATY_Color { get; set; }
        public int ECS_Id { get; set; }
        public int ECSA_Id { get; set; }
        public string ECS_SkillName { get; set; }
        public string ECSA_SkillArea { get; set; }
        public int ECSA_SkillOrder { get; set; }
        public Array YearlySkillArea { get; set; }
        public Array YearlySkillAreaStudentWise { get; set; }
        public Array savelisttot { get; set; }

        public Array St_ThomosTotal { get; set; }

    }
    public class temp_JSHSExamReportsDTO
    {
        public long ECT_Id { get; set; }
        public string ECT_TermName { get; set; }
    }
    public class temp_ExamlistDTO
    {
        public long EME_Id { get; set; }
        public string EME_ExamName { get; set; }
    }
    public class temp_groupDTO
    {
        public int EMPS_Id { get; set; }
        public string EMPG_GroupName { get; set; }
    }
    public class Temp_AmstIds
    {
        public long AMST_Id { get; set; }
    }
}
