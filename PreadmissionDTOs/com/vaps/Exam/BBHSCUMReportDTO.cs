﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class BBHSCUMReportDTO
    {
        public int EME_Id { get; set; }
        public long MI_Id { get; set; }
        public string EME_ExamName { get; set; }
        public string EME_ExamCode { get; set; }
        public int EME_ExamOrder { get; set; }
        public bool EME_FinalExamFlag { get; set; }
        public bool EME_ActiveFlag { get; set; }
        public Array exammastername { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public Array editlist { get; set; }
        public Array amstlist { get; set; }
        public string retrunMsg { get; set; }
        public exammasterDTO[] examDTO { get; set; }

        public Array Work_attendence { get; set; }
        public Array Present_attendence { get; set; }
        public long AMST_Id { get; set; }
        public int ELP_Id { get; set; }

        public long ASMAY_Id { get; set; }
        public long Login_Id { get; set; }
        public string ELP_Flg { get; set; }
        public bool ELP_ActiveFlg { get; set; }

        public int ELPS_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long ISMS_Id { get; set; }
        public bool ELPs_ActiveFlg { get; set; }
        public int EYCES_SubjectOrder { get; set; }
        public int ELPSS_Id { get; set; }
        public int EMSS_Id { get; set; }
        public bool EYCES_AplResultFlg { get; set; }
        public Array savelisttot { get; set; }


        public string graderemark { get; set; }
        public long User_Id { get; set; }
        public string UserName { get; set; }


        public Array yearlist { get; set; }
        public Array ctlist { get; set; }
        public Array grouplist { get; set; }
        public Array classlist { get; set; }
        public Array seclist { get; set; }
        public Array subjlist { get; set; }
        public Array grade_details { get; set; }
        public Array subsubject { get; set; }
        public Array studlist { get; set; }
        public Array studmaplist { get; set; }
        public Array gtdetailsview { get; set; }
        public Array edclasslist { get; set; }
        public Array emplist { get; set; }
        public Array userlist { get; set; }
        public Array pllist { get; set; }
        public Array clastechlt { get; set; }
        public Array editlist1 { get; set; }
        public List<BaldwinAllReportDTO> savelist { get; set; }
        public Array clstchname { get; set; }

        public tempPrivilagesDTO[] selectedclass { get; set; }



        public int EMG_Id { get; set; }
        public string EMG_GroupName { get; set; }
        public int EMCA_Id { get; set; }
        public string EMCA_CategoryName { get; set; }
        public int ECAC_Id { get; set; }
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
        public long HRME_Id { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public long IVRMULF_Id { get; set; }
        public long Emp_Code { get; set; }

        //ExmStudentMarksProcessSubjectwiseDMO
        public int ESTMPS_Id { get; set; }
        public decimal? ESTMPS_MaxMarks { get; set; }
        public decimal? ESTMPS_ObtainedMarks { get; set; }
        public string ESTMPS_ObtainedGrade { get; set; }
        public decimal? ESTMPS_ClassAverage { get; set; }
        public decimal? ESTMPS_SectionAverage { get; set; }
        public decimal? ESTMPS_ClassHighest { get; set; }
        public decimal? ESTMPS_SectionHighest { get; set; }
        public string ESTMPS_PassFailFlg { get; set; }
        //ExmStudentMarksProcessDMO
        public int ESTMP_Id { get; set; }
        public decimal ESTMP_TotalMaxMarks { get; set; }
        public decimal ESTMP_TotalObtMarks { get; set; }
        public decimal ESTMP_Percentage { get; set; }
        public string ESTMP_TotalGrade { get; set; }
        public int ESTMP_ClassRank { get; set; }
        public int ESTMP_SectionRank { get; set; }
        public string ESTMP_TotalGradeRemark { get; set; }
        public string ESTMP_Result { get; set; }
        public Array exmstdlist { get; set; }
        public decimal? classheld { get; set; }

        public decimal? classatt { get; set; }

        public bool EYCES_MarksDisplayFlg { get; set; }

        public bool EYCES_GradeDisplayFlg { get; set; }
        public Array fillsection { get; set; }
        public Array fillclass { get; set; }
        public Array fillstudents { get; set; }

        public Array studpersnoldetails { get; set; }

        public string AMST_RegistrationNo { get; set; }
        public string AMST_FatherName { get; set; }

        public string address { get; set; }
        public Array subjectslist { get; set; }
        public int EYC_Id { get; set; }
        public Array examlist { get; set; }
        public Array student_marklist { get; set; }

        public Array subsubjectlist { get; set; }

        public Array subsub_marks { get; set; }
        public Array subsub_marks1 { get; set; }
        public Array subsubmainmark { get; set; }
        public Array exmrank { get; set; }
        public Array subjects_details { get; set; }

        public int EMSE_Id { get; set; }
        public Array it_subsubmainmark { get; set; }
        public Array subexmlist { get; set; }
        public string EMSE_SubExamName { get; set; }


        public Array hhstudlist { get; set; }
        public Array stu_details { get; set; }
        public Array eam_sub_mrks_list { get; set; }
        public Array exmTPR { get; set; }
        public Array exam_subexam_W { get; set; }
        public Array exam_subexam { get; set; }

        public Array marksprocess_Pro_markscal { get; set; }
        public Array marksprocess_Pro_subwise { get; set; }

        public Array personality_status { get; set; }
        public Array co_curricular_activity { get; set; }
       public int EYCE_Id { get; set; }
        public int EYCESSE_Id { get; set; }
        public int EYCES_Id { get; set; }

        public string Stuname { get; set; }
        public string AMST_MotherName { get; set; }
        public string AMST_PerStreet { get; set; }
        public string AMST_PerArea { get; set; }
        public string AMST_PerCity { get; set; }
        public string IVRMMS_Name { get; set; }

        public string IVRMMC_CountryName { get; set; }
        public int? AMST_PerPincode { get; set; }
        public decimal? MaxMarks { get; set; }
        public decimal? ObtainedMarks { get; set; }

        public int EP_Id { get; set; }
        public string EP_PersonlaityName { get; set; }
        public string IVRM_Month_Name { get; set; }
        public string EPCR_RemarksName { get; set; }
        public int month_Id { get; set; }
        public int EPCR_Id { get; set; }

        public string ECC_CoCurricularName { get; set; }
        public int ECC_Id { get; set; }


        public Array halfyearatt { get; set; }
        public Array fullyearatt { get; set; }

        public decimal totalworkingday { get; set; }
        public decimal totalpresentday { get; set; }

        public DateTime strtdate { get; set; }
        public DateTime enddate { get; set; }
        public DateTime middledate { get; set; }
        public Array yearlistdate { get; set; }


        //04/09/2018
        public Array grade_list { get; set; }
        public Array grade_detailslist { get; set; }
        public Array termlist { get; set; }
        public Array instname { get; set; }
        public Array attendencelist { get; set; }


    }
}


