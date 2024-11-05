using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Alumni
{
    public class CLGAlumniStudentDTO : CommonParamDTO
    {
        public long MI_Id { get; set; }


        public Array fillyear { get; set; }
       
        public Array fillclass { get; set; }

        public long? ASMAY_Id { get; set; }

        public long? ALCMST_Id { get; set; }

        public bool alumnitrue { get; set; }

        public long? AMCST_ID { get; set; }

        public long? AMCO_Id { get; set; }

        public long UserId { get; set; }
        public string ASMAY_Year { get; set; }
        public Array batch { get; set; }

        public Array alumnilist { get; set; }

        public Array alumnibirthday { get; set; }
        public long? roleid { get; set; }

        public DateTime? COEE_EStartDate { get; set; }
        public DateTime? COEE_EEndDate { get; set; }
        public string COEME_EventName { get; set; }
        public string COEME_EventDesc { get; set; }
        public DateTime? COEE_ReminderDate { get; set; }

        public string ALCMST_PerAdd3 { get; set; }
        public Array calenderlist { get; set; }
        public long IVRMUL_Id { get; set; }
        public long? AMCO_JOIN_Id { get; set; }
        public long? AMB_JOIN_Id { get; set; }
        public long? AMSE_JOIN_Id { get; set; }
        public long? ASMAY_Id_Join { get; set; }
        public long? AMB_Id_Left { get; set; }
        public long? ASMAY_Id_Left { get; set; }
        public long? AMCO_Left_Id { get; set; }
        public long? AMSE_Id_Left { get; set; }
        public long? AMB_Id { get; set; }

        public long? AMSE_Id { get; set; }

        public long? ALCSREG_Id { get; set; }

        public string ALCMST_FirstName { get; set; }

        public string ALCMST_MiddleName { get; set; }

        public string ALCMST_LastName { get; set; }

        public string rolename { get; set; }

        public long ALSREG_Id { get; set; }

        public Array stu_name { get; set; }

        public Array fillstudent { get; set; }

        public string AMCST_FirstName { get; set; }

        public string searchfilter { get; set; }

        public long roleId { get; set; }

        public long userid { get; set; }

        public Array studentDetails { get; set; }

        public Array studentproffession { get; set; }

        public Array studentachievement { get; set; }

        public Array studentqualification { get; set; }

        public Array memebersetails { get; set; }

        public string AMCST_CLASS_LEFT { get; set; }

        public string AMCST_CLASS_JOIN { get; set; }

        public string AMCST_JOIN_YEAR { get; set; }

        public string AMCST_JOIN_LEFT { get; set; }

        public long CourseJoin { get; set; }
        public long BranchJoin { get; set; }
        public long SemJoin { get; set; }
        public long CourseLeft { get; set; }
        public long BranchLeft { get; set; }
        public long SemLeft { get; set; }

        public long ASMCL_Id_Join { get; set; }

        public string ALCMST_AdmNo { get; set; }

        public long? ALCMST_MobileNo { get; set; }

        public string ALCMST_emailId { get; set; }

        public string ALCMST_PerStreet { get; set; }

        public string ALCMST_PerArea { get; set; }

        public long? ALCMST_PerCountry { get; set; }

        public string ALCMST_PerCity { get; set; }

        public DateTime? ALCMST_DOB { get; set; }

        public string ALCMST_FatherName { get; set; }

        public string membername { get; set; }

        public string courseadmitted { get; set; }

        public string branchadmitted { get; set; }

        public string courseleft { get; set; }

        public string branchleft { get; set; }

        public string classnameadmitted { get; set; }

        public string classnameleft { get; set; }

        public string yeardmitted { get; set; }

        public string yearleft { get; set; }

        public long? ALCMST_PerPincode { get; set; }


        public long? ALCMST_PerState { get; set; }



        public Array countryDrpDown { get; set; }

        public Array statedropdown { get; set; }


        public long ALCMST_DETAILS_ID { get; set; }
        public string ALCMST_PUC_QS_DETAILS { get; set; }
        public string ALCMST_PUC_INS_NAME { get; set; }
        public string ALCMST_PUC_PASSED_OUT { get; set; }
        public string ALCMST_PUC_PERCENTAGE { get; set; }
        public string ALCMST_PUC_PLACE { get; set; }
        public string ALCMST_PUC_STATE { get; set; }
        public string ALCMST_UG_QS_DETAILS { get; set; }
        public string ALCMST_UG_INS_NAME { get; set; }
        public string ALCMST_UG_PASSED_OUT { get; set; }
        public string ALCMST_UG_PERCENTAGE { get; set; }
        public string ALCMST_UG_PLACE { get; set; }
        public string ALCMST_UG_STATE { get; set; }
        public string ALCMST_PG_QS_DETAILS { get; set; }

        public string ALCMST_PG_INS_NAME { get; set; }
        public string ALCMST_PG_PASSED_OUT { get; set; }
        public string ALCMST_PG_PERCENTAGE { get; set; }
        public string ALCMST_PG_PLACE { get; set; }
        public string ALCMST_PG_STATE { get; set; }
        public string ALCMST_ACH_DET { get; set; }
        public string ALCMST_ACH_REMARKS { get; set; }
        public string ALCMST_PRO_COMPANY_NAME { get; set; }

        public string ALCMST_PRO_DESIGNATION { get; set; }
        public string ALCMST_PRO_OFFICE_NO { get; set; }
        public string ALCMST_PRO_ADDRESS { get; set; }
        public string ALCMST_PRO_REMARKS { get; set; }

        public Array saveddata { get; set; }

        public string returnval { get; set; }


        public int ALCMST_AmountPaid { get; set; }
        public string ALCMST_AppDownloadedDeviceId { get; set; }
        public string ALCMST_ApplStatus { get; set; }
        public int ALCMST_BPLCardFlag { get; set; }
        public string ALCMST_BPLCardNo { get; set; }
        public string ALCMST_ConAdd3 { get; set; }
        public long ALCMST_ConCountryId { get; set; }
        public long ALCMST_CreatedBy { get; set; }
        public string ALCMST_District { get; set; }
        public string ALCMST_DOBinwords { get; set; }
        public int ALCMST_ECSFlag { get; set; }
        public string ALCMST_EMSINo { get; set; }
        public string ALCMST_FatherBankIFSCCode { get; set; }
        public string ALCMST_FatherFingerprint { get; set; }
        public long ALCMST_FatherPANCardNo { get; set; }
        public string ALCMST_FatherSign { get; set; }
        public bool ALCMST_FinalpaymentFlag { get; set; }
        public long ALCMST_GPSTrackingId { get; set; }
        public int ALCMST_GymReqdFlag { get; set; }
        public string ALCMST_HomeLaguage { get; set; }
        public int ALCMST_HostelReqdFlag { get; set; }
        public string ALCMST_MOInstruction { get; set; }
        public string ALCMST_MotherBankIFSCCode { get; set; }
        public string ALCMST_MotherFingerprint { get; set; }
        public long ALCMST_MotherMobleNo { get; set; }
        public string ALCMST_MotherPANCardNo { get; set; }
        public string ALCMST_MotherSign { get; set; }
        public int ALCMST_NoOfBrothers { get; set; }
        public int ALCMST_NoofDependencies { get; set; }
        public int ALCMST_NoofSiblings { get; set; }
        public int ALCMST_NoofSiblingsSchool { get; set; }
        public int ALCMST_NoOfSisters { get; set; }
        public DateTime? ALCMST_PaymentDate { get; set; }
        public int ALCMST_PaymentFlag { get; set; }
        public string ALCMST_PaymentType { get; set; }
        public string ALCMST_ReceiptNo { get; set; }
        public string ALCMST_StuBankIFSCCode { get; set; }
        public string ALCMST_StudentFingerprint { get; set; }
        public long ALCMST_StudentPANCard { get; set; }
        public string ALCMST_StudentPhoto { get; set; }
        public string ALCMST_StudentSign { get; set; }
        public long ALCMST_StudentSubCaste { get; set; }
        public string ALCMST_Taluk { get; set; }
        public string ALCMST_TPINNO { get; set; }
        public int ALCMST_TransportReqdFlag { get; set; }
        public long ALCMST_UpdatedBy { get; set; }
        public string ALCMST_Village { get; set; }
        public long IVRMMB_Id { get; set; }
        public long IVRMMC_Id { get; set; }

        public string ALCMST_Marital_Status { get; set; }

        public long? ALCMST_PhoneNo { get; set; }

        public CLGAlumniApproveStudentDTO approvalarray { get; set; }

        public string ALCSAC_Achievement { get; set; }
        public string ALCSAC_Remarks { get; set; }
        public Boolean ALCSAC_ActiveFlg { get; set; }

        public string ALCSQU_Qulification { get; set; }
        public long ALCSQU_YearOfPassing { get; set; }
        public string ALCSQU_University { get; set; }
        public string ALCSQU_OtherDetails { get; set; }
        public Boolean ALCSQU_ActiveFlg { get; set; }

        public string ALCSPR_CompanyName { get; set; }
        public string ALCSPR_CompanyAddress { get; set; }

        public string ALCSPR_Designation { get; set; }
        public string ALCSPR_EmailId { get; set; }
        public long? ALCSPR_Phone { get; set; }
        public string ALCSPR_WorkingSince { get; set; }

        public string ALCSPR_OtherDetails { get; set; }
        public string stuStatus { get; set; }
        public List<Object> field = new List<Object>();
        public List<Object> Operator = new List<Object>();

        public List<Object> value = new List<Object>();
        public List<Object> condition = new List<Object>();
        public Array searchResult { get; set; }

        public Array savetmpdata { get; set; }

        public Array AllAchivement { get; set; }
        public long ASMC_Id { get; set; }

        public long AMSTEC_Id { get; set; }

        public Array studentlist { get; set; }
        public string HFC_Flag { get; set; }
        public CLGAlumniStudentDTO[] professionalDetails { get; set; }
        public CLGAlumniStudentDTO[] achievementsDetails { get; set; } 
        public CLGAlumniStudentDTO[] qualificationDetails { get; set;}
        public CLGAlumniStudentDTO[] selectedstudent { get; set; }
        public Array fillstudlist { get; set; }
        public int count { get; set; }
        public CourselistDTO[] courselist { get; set; }
        public BranchlistDTO[] branchlist { get; set; }
        public SemlistDTO[] semlist { get; set; }
        public check_list221[] check_list22 { get; set; }

        public string message { get; set; }
        public long ALCSPR_Id { get; set; }
        //=======================================
        public Array almdetails { get; set; }
        public string ALCSREG_MemberName { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMB_BranchName { get; set; }
        public long ALCSREG_AdmittedYear { get; set; }
        public long ALCSREG_LeftYear { get; set; }
        public long ALCSREG_AdmittedCourse { get; set; }
        public long ALCSREG_LeftCourse { get; set; }
        public long ALCSREG_AdmisstedBranch { get; set; }
        public long ALCSREG_LeftBranch { get; set; }
       
    }

    public class CLGqualificationDetails
    {
        public string ALCSQU_Qulification { get; set; }
        public long? ALCSQU_YearOfPassing { get; set; }
        public string ALCSQU_University { get; set; }
        public string ALCSQU_OtherDetails { get; set; }
        public Boolean ALCSQU_ActiveFlg { get; set; }
    }
    public class CLGprofessionalDetails 
    {
        public string ALCSPR_CompanyName { get; set; }
        public string ALCSPR_CompanyAddress { get; set; }

        public string ALCSPR_Designation { get; set; }
        public string ALCSPR_EmailId { get; set; }
        public long? ALCSPR_Phone { get; set; }
        public string ALCSPR_WorkingSince { get; set; }

        public string ALCSPR_OtherDetails { get; set; }
    }
    public class CLGachievementsDetails
    {
        public string ALCSAC_Achievement { get; set; }
        public string ALCSAC_Remarks { get; set; }
        public Boolean ALCSAC_ActiveFlg { get; set; }
    }

    public class CLGAlumniApproveStudentDTO
    {
        public long ALCSREG_Id { get; set; }
    }

    public class CourselistDTO
    {
        public long? AMCO_Id { get; set; }
    }
    public class BranchlistDTO
    {
        public long? AMB_Id { get; set; }
    }
    public class SemlistDTO
    {
        public long? AMSE_Id { get; set; }
    }
    public class check_list221
    {
        public long ALCSREG_Id { get; set; }
    }
}



