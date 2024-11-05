using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Portals.Student
{
    public class StudentProgressCardReportDTO
    {
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long Userid { get; set; }
        public long roleid { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long EME_Id { get; set; }
        public long ECT_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ISMS_Id { get; set; }
        public string examorterm { get; set; }
        public string spflag { get; set; }
        public int? examflag { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string EME_ExamName { get; set; }
        public string EME_ExamCode { get; set; }
        public int EME_ExamOrder { get; set; }
        public bool EME_FinalExamFlag { get; set; }
        public bool EME_ActiveFlag { get; set; }
        public string EMER_Remarks { get; set; }
        public int EYCES_SubjectOrder { get; set; }       
        public bool EYCES_AplResultFlg { get; set; }
        public Array savelisttot { get; set; }
        public string graderemark { get; set; }        
        public Array grade_details { get; set; }       
        public Array studlist { get; set; }     
        public Array promotionstatus { get; set; }
        public List<StudentProgressCardReportDTO> savelist { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public string AMST_FirstName { get; set; }
        public DateTime AMST_DOB { get; set; }
        public string AMST_AdmNo { get; set; }
        public long AMAY_RollNo { get; set; }
        public decimal? EYCES_MaxMarks { get; set; }
        public decimal? EYCES_MinMarks { get; set; }
        public string ASMC_SectionName { get; set; }
        public string ASMAY_Year { get; set; }
        public string EMSS_SubSubjectName { get; set; }
        public int EMGR_Id { get; set; }
        public long Emp_Code { get; set; }       
        public int ESTMPS_Id { get; set; }
        public decimal? ESTMPS_MaxMarks { get; set; }
        public decimal? ESTMPS_ObtainedMarks { get; set; }
        public string ESTMPS_ObtainedGrade { get; set; }
        public decimal? ESTMPS_ClassAverage { get; set; }
        public decimal? ESTMPS_SectionAverage { get; set; }
        public decimal? ESTMPS_ClassHighest { get; set; }
        public decimal? ESTMPS_SectionHighest { get; set; }
        public string ESTMPS_PassFailFlg { get; set; }       
        public int ESTMP_Id { get; set; }
        public decimal ESTMP_TotalMaxMarks { get; set; }
        public decimal ESTMP_TotalObtMarks { get; set; }
        public decimal ESTMP_Percentage { get; set; }
        public string ESTMP_TotalGrade { get; set; }
        public int ESTMP_ClassRank { get; set; }
        public int ESTMP_SectionRank { get; set; }
        public string ESTMP_TotalGradeRemark { get; set; }
        public string ESTMP_Result { get; set; }       
        public decimal? classheld { get; set; }
        public decimal? classatt { get; set; }
        public bool EYCES_MarksDisplayFlg { get; set; }
        public bool EYCES_GradeDisplayFlg { get; set; }
        public string EMSE_SubExamName { get; set; }
        public decimal ESTMPSSS_ObtainedMarks { get; set; }
        public string AMST_FatherName { get; set; }
        public string AMST_MotherName { get; set; }       
        public string photoname { get; set; }
        public string htmlstring { get; set; }
        public Array examwiseremarks { get; set; }
        public Array Present_attendence { get; set; }
        public Array Work_attendence { get; set; }
        public Array getpublishmarks { get; set; }
        public Array getyear { get; set; }
        public Array getterm { get; set; }
        public Array getexam { get; set; }
        public Array getclass { get; set; }
        public Array getexamtermlist { get; set; }
        public Array getstudentdetails { get; set; }
        public Array instname { get; set; }
        public Array clstchname { get; set; }
        public Array subjlist { get; set; }
        public Array gettermexamdetails { get; set; }
        public Array gettermdetails { get; set; }
        public Array getstudentwisesubjectlist { get; set; }
        public Array getstudentdetailsreport { get; set; }
        public Array getstudentwiseskillslist { get; set; }
        public Array getstudentwiseactiviteslist { get; set; }
        public Array getstudentwisesportsdetails { get; set; }
        public Array getstudentwiseattendancedetails { get; set; }
        public Array getstudentwisetermwisedetails { get; set; }
        public Array getstudentmarksdetails { get; set; }
        public Array getgroupdetails { get; set; }
        public Array getpromotionmarksdetails { get; set; }
        public Array getsubjectlist { get; set; }
        public Array nonapplicablesubject_examwisemarks { get; set; }
        public Array getexamlist { get; set; }
        public Array getsavedlist { get; set; }
        public Array remarks { get; set; }
        public Array grade_detailslist { get; set; }
        public decimal? ECTEX_MarksPercentValue { get; set; }
        public string EMPG_GroupName { get; set; }
        public decimal? EMPSG_PercentValue { get; set; }
        public decimal? EMPSGE_ForMaxMarkrs { get; set; }
        public string EMPG_DistplayName { get; set; }
        public string EPCFT_ExamFlag { get; set; }

        //BGHS
        public Array getinstitution { get; set; }
        public Array getexamdetails { get; set; }
        public Array getgroupexamdetails { get; set; }
        public Array getclassteacher { get; set; }
        public Array getexammaxmarks { get; set; }
        public int? EMPSG_Order { get; set; }
        public decimal? EMPSG_MarksValue { get; set; }
        public string classteachername { get; set; }
        public string message { get; set; }

        //HHS
        public Array stu_details { get; set; }
        public Array eam_sub_mrks_list { get; set; }
        public Array exmTPR { get; set; }
        public Array promotionrank { get; set; }
        public Array marksprocess_Pro_markscal { get; set; }
        public Array exam_subexam_W { get; set; }
        public Array exam_subexam { get; set; }
        public Array remarkslist { get; set; }
        public Array personality_status { get; set; }
        public Array personalitymonth { get; set; }
        public Array co_curricular_activity { get; set; }
        public Array cocurillarymonth { get; set; }
        public Array yearlistdate { get; set; }
        public Array fullyearatt { get; set; }
        public Array halfyearatt { get; set; }
        public string Stuname { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public string AMST_PerStreet { get; set; }
        public string AMST_PerArea { get; set; }
        public string AMST_PerCity { get; set; }
        public string IVRMMS_Name { get; set; }
        public string IVRMMC_CountryName { get; set; }
        public long? AMST_PerPincode { get; set; }
        public string AMST_EmailId { get; set; }
        public string EP_PersonlaityName { get; set; }
        public string EPCR_RemarksName { get; set; }
        public string ECC_CoCurricularName { get; set; }
        public DateTime strtdate { get; set; }
        public DateTime enddate { get; set; }
        public DateTime middledate { get; set; }
        public long AMST_Mobile { get; set; }
        public long EYCE_Id { get; set; }
        public long EYCESSE_Id { get; set; }
        public long EMSE_Id { get; set; }
        public long EYCES_Id { get; set; }
        public long EP_Id { get; set; }
        public long ECC_Id { get; set; }
        public decimal? EYCESSS_MaxMarks { get; set; }
        public decimal totalworkingday { get; set; }
        public decimal totalpresentday { get; set; }
        public string AMST_Photoname { get; set; }

        //Stjames
        public Array studentwisemarks { get; set; }
        public Array getexamwisesubsubjectlist { get; set; }
        public Array getstudentwisesubjectlistnew { get; set; }

        //Notredame
        public Array getsubjectwisetotaldetails { get; set; }
        public Array getpromotionremarksdetails { get; set; }
        public Array getparticipatedetails { get; set; }
        public Array getstudentwisesubjectsubsubjectlist { get; set; }

    }
}
