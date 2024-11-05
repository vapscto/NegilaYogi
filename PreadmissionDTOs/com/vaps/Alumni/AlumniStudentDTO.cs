using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Alumni
{
    public class AlumniStudentDTO : CommonParamDTO
    {
        public long MI_Id { get; set; }
        public long ALMDON_Id { get; set; }
        public long ALDON_Id { get; set; }
        public long ALNTB_Id { get; set; }
        public long FPGD_Id { get; set; }
        public string imagenew { get; set; }
        public string flag { get; set; }
        public string ALMDON_DonationName { get; set; }
        public string message { get; set; }
        public string IMPG_PGFlag { get; set; }
        public string FPGD_Image { get; set; }
        public string ALMST_MembershipId { get; set; }
        public string FPGD_PGName { get; set; }
        public string ALGA_Id { get; set; }
        public string orderId { get; set; }
        public string trans_id { get; set; }
        public decimal ALMDON_Amount { get; set; }
        public long pageid { get; set; }
        public long check { get; set; }
        public bool ALMST_ActiveFlag { get; set; }
        public bool ALMDON_ActiveFlag { get; set; }
        public string fromdate { get; set; }
        public string alumniregister { get; set; }
        public string todate { get; set; }
        public string Template { get; set; }
        public bool ALMDON_RegistrationFeeFlag { get; set; }
        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }

        public Array fillalumnidonationdetails { get; set; }
        public Array donationlist { get; set; }
        public Array alumnidetails { get; set; }
        public Array totaldonation { get; set; }
        public Array edit_donation_list { get; set; }
        public Array paymentgateway { get; set; }
        public Array institution { get; set; }
        public Array student_details { get; set; }
        public Array getamountlist { get; set; }
        public Array fillpaymentgateway { get; set; }
        public Array fillyear { get; set; }
        public Array alumninoticeboardlist { get; set; }
        public string almst { get; set; }
        public Array placelist { get; set; }
        public Array placelistqua { get; set; }
        public Array citylist { get; set; }
        public Array occupationlist { get; set; }
        public Array statelist { get; set; }
        public Array districtlist { get; set; }
        public Array countrylist { get; set; }
        public Array statelistall { get; set; }
        public Array fillclass { get; set; }
        public Array attachementlist { get; set; }
        public Array qualification { get; set; }
        public Array achivement { get; set; }
        public Array profession { get; set; }
        public Array birthdaylist { get; set; }
        public Array alumnidonationlist { get; set; }
        public Array friendrequestlist { get; set; }
        public long ASMAY_Id { get; set; }

        public long ALMST_Id { get; set; }
        public string ALMST_NickName { get; set; }
        public string ALMST_SpouseName { get; set; }
        public string ALMST_SpouseEmailId { get; set; }
        public string ALMST_SpouseQulaification { get; set; }
        public string ALMST_SpouseProfession { get; set; }
        public string ALMST_SpouseContactNo { get; set; }

        public string ASMAY_Year { get; set; }
        public string ALDON_DonorName { get; set; }
        public decimal ALDON_Amount { get; set; }
        public DateTime ALDON_Date { get; set; }


        public string ReceiptNo { get; set; }
        public string Designation { get; set; }

        public long IVRMUL_Id { get; set; }

        public Array batch { get; set; }

        public Array SearchstudentDetails { get; set; }

        public bool alumnitrue { get; set; }

        public long? AMST_ID { get; set; }

        public Array alumnilist { get; set; }
        public Array alumnilistnew { get; set; }

        public Array calenderlist { get; set; }

        public Array yearwiselist { get; set; }
        public Array viewgallery { get; set; }

        public Array classwiselist { get; set; }

        public int count { get; set; }

        public long? ASMAY_Id_Left { get; set; }

        public long? ASMAY_Id_Join { get; set; }

        public string ALSQU_Qulification { get; set; }
        public string APP_FLG { get; set; }
        public string ALSQU_University { get; set; }
        public string ALSQU_OtherDetails { get; set; }
        public string IVRMMS_Name { get; set; }
        public string IVRMMC_CountryName { get; set; }
        public decimal? ALSQU_Percentage { get; set; }
        public string ALSQU_Place { get; set; }
        public long? ALSQU_YearOfPassing { get; set; }
        public string ALMST_FirstName { get; set; }

        public string ALMST_MiddleName { get; set; }

        public string ALMST_LastName { get; set; }

        public string rolename { get; set; }

        public long ALSREG_Id { get; set; }

        public Array stu_name { get; set; }
        public Array Alumnigallerygrid { get; set; }

        public Array fillstudent { get; set; }

        public string AMST_FirstName { get; set; }

        public string searchfilter { get; set; }

        public long roleId { get; set; }

        public Array YearList { get; set; }

        public Array classList { get; set; }

        public long userid { get; set; }

        public Array studentDetails { get; set; }
        public Array studentachive { get; set; }
        public Array studentprof { get; set; }
        public Array studentquali { get; set; }

        public Array memebersetails { get; set; }

        public string AMST_CLASS_LEFT { get; set; }

        public string AMST_CLASS_JOIN { get; set; }

        public string AMST_JOIN_YEAR { get; set; }

        public string AMST_JOIN_LEFT { get; set; }

        public long? ASMCL_Id_Left { get; set; }

        public string ALSAC_Achievement { get; set; }
        public string ALSAC_Remarks { get; set; }
        public string ALSPR_CompanyName { get; set; }
        public string ALSPR_Designation { get; set; }

        public long? ASMCL_Id_Join { get; set; }
        public long ASMCL_Id { get; set; }
        public string ASMCL_Left { get; set; }
        public string ASMCL_Join { get; set; }

        public string ALMST_AdmNo { get; set; }

        public long? ALMST_MobileNo { get; set; }

        public string ALMST_emailId { get; set; }

        public string ALMST_PerStreet { get; set; }

        public string ALMST_PerArea { get; set; }
        public string ALMST_PerAdd3 { get; set; }

        public long? ALMST_PerCountry { get; set; }
        public long? ALMST_PerDistrict { get; set; }
        public long? ALMST_ConDistrict { get; set; }

        public string ALMST_PerCity { get; set; }

        public DateTime ALMST_DOB { get; set; }
        public DateTime? ALMST_WeddingAnniversaryDate { get; set; }

        public string ALMST_FatherName { get; set; }

        public string membername { get; set; }

        public string classnameadmitted { get; set; }

        public string classnameleft { get; set; }

        public string yeardmitted { get; set; }

        public string yearleft { get; set; }

        public long? ALMST_PerPincode { get; set; }


        public long? ALMST_PerState { get; set; }
        public long? ALMST_ConState { get; set; }
        public string ALMST_ConStreet { get; set; }
        public string ALMST_BloodGroup { get; set; }
        public string ALMST_ConCity { get; set; }
        public string ALMST_ConArea { get; set; }



        public Array countryDrpDown { get; set; }
        public Array membercategory { get; set; }

        public Array statedropdown { get; set; }
        public Array districtdropdown { get; set; }


        public long ALMST_DETAILS_ID { get; set; }
        public string ALMST_PUC_QS_DETAILS { get; set; }
        public string ALMST_PUC_INS_NAME { get; set; }
        public string ALMST_PUC_PASSED_OUT { get; set; }
        public string ALMST_PUC_PERCENTAGE { get; set; }
        public string ALMST_PUC_PLACE { get; set; }
        public string ALMST_PUC_STATE { get; set; }
        public string ALMST_UG_QS_DETAILS { get; set; }
        public string ALMST_UG_INS_NAME { get; set; }
        public string ALMST_UG_PASSED_OUT { get; set; }
        public string ALMST_UG_PERCENTAGE { get; set; }
        public string ALMST_UG_PLACE { get; set; }
        public string ALMST_UG_STATE { get; set; }
        public string ALMST_PG_QS_DETAILS { get; set; }

        public string ALMST_PG_INS_NAME { get; set; }
        public string ALMST_PG_PASSED_OUT { get; set; }
        public string ALMST_PG_PERCENTAGE { get; set; }
        public string ALMST_PG_PLACE { get; set; }
        public string ALMST_PG_STATE { get; set; }
        public string ALMST_ACH_DET { get; set; }
        public string ALMST_ACH_REMARKS { get; set; }
        public string ALMST_PRO_COMPANY_NAME { get; set; }

        public string ALMST_PRO_DESIGNATION { get; set; }
        public string ALSPR_EmailId { get; set; }
        public long ALSPR_WorkingSince { get; set; }
        public string ALSPR_CompanyAddress { get; set; }
        public string ALMST_PRO_OFFICE_NO { get; set; }
        public string ALMST_PRO_ADDRESS { get; set; }
        public string ALSPR_OtherDetails { get; set; }
        public string ALMST_PRO_REMARKS { get; set; }
        public Array saveddata { get; set; }
        public string returnval { get; set; }
        public int? ALMST_AmountPaid { get; set; }
        public string ALMST_AppDownloadedDeviceId { get; set; }
        public string ALMST_ApplStatus { get; set; }
        public string ALMST_FullAddess { get; set; }
        public int ALMST_BPLCardFlag { get; set; }
        public string ALMST_BPLCardNo { get; set; }
        public string ALMST_ConAdd3 { get; set; }
        public long ALMST_ConCountryId { get; set; }
        public long? ALMST_MembershipCategory { get; set; }
        public long ALMST_CreatedBy { get; set; }
        public string ALMST_District { get; set; }
        public string ALMST_DOBinwords { get; set; }
        public Array academicList { get; set; }
        public int ALMST_ECSFlag { get; set; }
        public string ALMST_EMSINo { get; set; }
        public string ALMST_FatherBankIFSCCode { get; set; }
        public string ALMST_FatherFingerprint { get; set; }
        public long ALMST_FatherPANCardNo { get; set; }
        public string ALMST_FatherSign { get; set; }
        public bool ALMST_FinalpaymentFlag { get; set; }
        public long ALMST_GPSTrackingId { get; set; }
        public int ALMST_GymReqdFlag { get; set; }
        public string ALMST_HomeLaguage { get; set; }
        public int ALMST_HostelReqdFlag { get; set; }
        public string ALMST_MOInstruction { get; set; }
        public string ALMST_MotherBankIFSCCode { get; set; }
        public string ALMST_MotherFingerprint { get; set; }
        public long ALMST_MotherMobleNo { get; set; }
        public string ALMST_MotherPANCardNo { get; set; }
        public string ALMST_MotherSign { get; set; }
        public int ALMST_NoOfBrothers { get; set; }
        public int ALMST_NoofDependencies { get; set; }
        public int ALMST_NoofSiblings { get; set; }
        public int ALMST_NoofSiblingsSchool { get; set; }
        public int ALMST_NoOfSisters { get; set; }
        public DateTime? ALMST_PaymentDate { get; set; }
        public int ALMST_PaymentFlag { get; set; }
        public string ALMST_PaymentType { get; set; }
        public string ALMST_ReceiptNo { get; set; }
        public string ALMST_StuBankIFSCCode { get; set; }
        public string ALMST_StudentFingerprint { get; set; }
        public string ALMST_StudentPANCard { get; set; }
        public bool ALDON_NRIFlg { get; set; }
        public string ALDON_Towards { get; set; }
        public string ALMST_StudentPhoto { get; set; }
        public string ALMST_FamilyPhoto { get; set; }
        public string ALMST_StudentSign { get; set; }
        public long ALMST_StudentSubCaste { get; set; }
        public string ALMST_Taluk { get; set; }
        public string ALMST_TPINNO { get; set; }
        public int ALMST_TransportReqdFlag { get; set; }
        public long ALMST_UpdatedBy { get; set; }
        public string ALMST_Village { get; set; }
        public string city { get; set; }
        public string Occupation { get; set; }
        public long IVRMMB_Id { get; set; }
        public long IVRMMC_Id { get; set; }
        public long IVRMMS_Id { get; set; }
        public long IVRMMD_Id { get; set; }
        public long? ALMST_ConPincode { get; set; }
        public string ALMST_Marital_Status { get; set; }

        public string ALMST_PhoneNo { get; set; }
        public qualification_array1[] qualification_array { get; set; }
        public alumnistudentarray1[] alumnistudentarray { get; set; }
        public AlumniApproveStudentDTO approvalarray { get; set; }

        public DateTime? COEE_EStartDate { get; set; }
        public DateTime? COEE_EEndDate { get; set; }
        public string COEME_EventName { get; set; }
        public string COEME_EventDesc { get; set; }
        public DateTime? COEE_ReminderDate { get; set; }

        public Array alumnibirthday { get; set; }

        public string stuStatus { get; set; }
        public List<Object> field = new List<Object>();
        public List<Object> Operator = new List<Object>();

        public List<Object> value = new List<Object>();
        public List<Object> condition = new List<Object>();
        public Array searchResult { get; set; }
        public Array almdetails { get; set; }
        public string ALSREG_MemberName { get; set; }
        public long ALSREG_AdmittedClass { get; set; }
        public long ALSREG_LeftClass { get; set; }
        public long ALSREG_AdmittedYear { get; set; }
        public long ALSREG_LeftYear { get; set; }
        public string classadmitted { get; set; }
        public string ALMST_RegistrationNo { get; set; }
        public string classleft { get; set; }
        public string admittedyear { get; set; }
        public string leftyear { get; set; }

        public classlistnew1[] classlistnew { get; set; }

        public cityslist1[] cityslist { get; set; }
        public occupationslist1[] occupationslist { get; set; }

        public yearslist1[] yearslist { get; set; }
        public bool Readmitted_student { get; set; }
        public long studentCunt { get; set; }
        public long? Readmitted_ASMAY_Id_Left { get; set; }
        public long? Readmitted_ASMAY_Id_Join { get; set; }
        public long? Readmitted_ASMCL_Id_Join { get; set; }
        public long? Readmitted_ASMCL_Id_Left { get; set; }

        public long? ALMSTRADM_Id { get; set; }
        public ReadmittedStudentDTOO[] ReadmittedStudentDTO { get; set; }
        //public long? ALMSTRADM_YearJoined { get; set; }
        //public long? ALMSTRADM_ClassJoined { get; set; }
        //public long? ALMSTRADM_YearLeft { get; set; }
        //public long? ALMSTRADM_ClassLeft { get; set; }
        public Array regid { get; set; }
        public long tempid { get; set; }

        //Akash
        public Array editdatalist { get; set; }
        public bool returnvals { get; set; }
        public bool deactiveactivelist { get; set; }
        public Array qualificationAlmStlist { get; set; }
        public Array achivementALMSTDetails { get; set; }
        public Array professionaldetailslist { get; set; }
        public countrylist1[] countrylistarray { get; set; }

        public statelistarray1[] statelistarray { get; set; }

        public districtlistarray1[] districtlistarray { get; set; }

        public multipleBatch[] multipleBatchs { get; set; }

        //
        public class multipleBatch
        {
            public long ASMAY_Id { get; set; }

        }
        public class districtlistarray1
        {
            public long IVRMMD_Id { get; set; }
        }
        public class countrylist1
        {
            public long IVRMMC_Id { get; set; }
        }
        public class statelistarray1
        {
            public long IVRMMS_Id { get; set; }
        }

        public class ReadmittedStudentDTOO
        {

            public long? ALMSTRADM_Id { get; set; }
            public long ALMST_Id { get; set; }

            public long? ALMSTRADM_YearJoined { get; set; }
            public long? ALMSTRADM_ClassJoined { get; set; }
            public long? ALMSTRADM_YearLeft { get; set; }
            public long? ALMSTRADM_ClassLeft { get; set; }
            public long ALMSTRADM_ActiveFlg { get; set; }
            public DateTime ALMSTRADM_CreatedDate { get; set; }
            public DateTime ALMSTRADM_UpdatredDate { get; set; }
            public long ALMSTRADM_CreatedBy { get; set; }
            public long ALMSTRADM_UpdatedBy { get; set; }

        }

        public class yearslist1
        {
            public long ASMAY_Id { get; set; }
        }
        public class cityslist1
        {
            public long id { get; set; }
        }
        public class occupationslist1
        {
            public long id { get; set; }
        }
        public class classlistnew1
        {
            public long ASMCL_Id { get; set; }
        }
        public class qualification_array1
        {
            public string Qualification { get; set; }
            public string ALMST_PUC_INS_NAME { get; set; }
            public string ALMST_PLACE { get; set; }

            public string ALSQU_OtherDetails { get; set; }

            public long? ALMST_PerState { get; set; }
            public long? ALMST_PUC_PASSED_OUT { get; set; }
            public string ALSQU_University { get; set; }
            public decimal? ALSQU_Percentage { get; set; }
        }
    }

    public class AlumniApproveStudentDTO
    {
        public long ALSREG_Id { get; set; }
    }
    public class alumnistudentarray1
    {
        public long ALSREG_Id { get; set; }
    }
}



