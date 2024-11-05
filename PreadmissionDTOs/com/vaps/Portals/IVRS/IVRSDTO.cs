using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.IVRS
{
    public class IVRSDTO
    {
        public long userid { get; set; }
        public long AMST_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public DateTime AMST_Date { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public string AMST_AdmNo { get; set; }
        public long? AMC_Id { get; set; }
        public DateTime AMST_DOB { get; set; }
        public string AMST_DOB_Words { get; set; }
        public long? ASMCL_Id { get; set; }
        public long? AMST_PerState { get; set; }
        public long AMST_MobileNo { get; set; }
        public string AMST_emailId { get; set; }
        public string AMST_ReceiptNo { get; set; }
        public int? AMST_ActiveFlag { get; set; }
        public string AMST_Photoname { get; set; }
        public string AMST_SOL { get; set; }
        public string HttpStatusCode { get; set; }

        //-----------------------------------------//

        public Array AllAcademicYear { get; set; }
        public Array AllClass { get; set; }

        public Array AllSection { get; set; }

        public Array AllSubject { get; set; }

        public Array StudentAchivementDetails { get; set; }
        public Array StudentActivityDetails { get; set; }
        public Array adm_m_student { get; set; }

        // public List<MasterActivityDTO> SelectedActivityDetails { get; set; }

       

        public Array fillacademicyr { get; set; }
        public Array fillclass { get; set; }

        public Array preAdmtoAdmStuList { get; set; }

        public string SelectedAchivementDetails { get; set; }

        //Added for Admission Student Search by Sripad Joshi.
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

        public Array fillstudlist { get; set; }


        // added on 21/02/2017
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string AMSTRegistrationNo { get; set; }
        public string AMSTAdmNo { get; set; }  //  public long? AMC_Id { get; set; }
        public string Gender { get; set; } //AMST_Sex
        public DateTime DOB { get; set; }  //  public int? PASR_Age { get; set; }
        public string AMSTDOBWords { get; set; }
        public string Class { get; set; }  //long?

        public long MobileNo { get; set; }
        public string EmailID { get; set; }
        public string AmountPaid { get; set; }
        public string PaymentType { get; set; }
        public DateTime PaymentDate { get; set; }
        public string ReceiptNo { get; set; }
        public string ActiveFlag { get; set; }
        public string Applicationstatus { get; set; }
        public string FinalPaymentFlag { get; set; }
        public bool returnval { get; set; } = true;
        public Master_NumberingDTO transnumconfigsettings { get; set; }
        public Array admTransNumSetting { get; set; }
        public Master_NumberingDTO admissionNumbering { get; set; }
        public MasterConfigurationDTO configurationsettings { get; set; }

        public int payementcheck { get; set; }
        public string returnMsg { get; set; }


        public long asms_id { get; set; }
        public Array studentlist123 { get; set; }



      
        public Array adm_m_stud_cat { get; set; }




        public Array stud_catg_edit { get; set; }
        public string flag123 { get; set; }
        public Array achievement { get; set; }

        public long AMSMD_Id { get; set; }

        public int duplicateMobNo { get; set; }
        public int duplicateEmailId { get; set; }
        public int duplicateAdharNo { get; set; }
        public Array admissioncongigurationList { get; set; }
        public bool Edit_flag { get; set; }
        public Array activityIds { get; set; }
        public Array referenceIds { get; set; }
        public Array sourceIds { get; set; }
        public string IMC_CasteName { get; set; }
        public Array academicYearOnLoad { get; set; }
        public Array StudentList1 { get; set; }
        public Array academicyearforreadmit { get; set; }
        public string Status_Flag { get; set; }
        public string companyname { get; set; }
        public Array MasterCompany { get; set; }
        public Array academicList1 { get; set; }
        public string SearchColumn { get; set; }
        public string EnteredData { get; set; }
        public string message { get; set; }
        public Array govtBondList { get; set; }
        public Array bondIds { get; set; }
        public Array boardList { get; set; }
        public Array concessionList { get; set; }
        public string admRegManualFlag { get; set; }
        public string admAdmManualFlag { get; set; }
        public string preventdupRegNo { get; set; }
        public string preventdupAdmNo { get; set; }
        public int duplicateRegNo { get; set; }
        public int duplicateAdmNo { get; set; }
        //added by vishnu 
        public string classname { get; set; }
        public string sectionname { get; set; }
        public Array classsection { get; set; }
        public int totalgender { get; set; }
        public string gender1 { get; set; }
        public string stdmobilenumber { get; set; }
        //
        public string operation { get; set; }
        public string IVRA_TPIN { get; set; }
        public Array filldata { get; set; }
        public Array stud_data { get; set; }
        public Array Fees_data { get; set; }
        public Array operation_list { get; set; }
        public string exeId { get; set; }
        public int inst_id { get; set; }

        // newly added for get branch names
        public long vno { get; set; }
        public string INS_Name { get; set; }
        public string Url { get; set; }
        public bool Activeflag { get; set; }
        public Array BranchData { get; set; }


        //For Updates in our database
        public long IMCS_Id { get; set; }
        public string IMCS_VirtualNo { get; set; }
        public long IMCS_MI_Id { get; set; }
        public string IMCS_SchoolName { get; set; }
        public string IMCS_URL { get; set; }
        public long IMCS_PerMonthCall { get; set; }
        public long IMCS_ActiveFlg { get; set; }
        public long IMCS_CreatedBy { get; set; }
        public long IMCS_UpdatedBy { get; set; }
        public long IMCS_MonthYear { get; set; }
        public long IMCS_AssignedCall { get; set; }
        public long IMCS_InboundCalls { get; set; }
        public long IMCS_OutboundCalls { get; set; }
        public long IMCS_AvailableCalls { get; set; }
        public long IMCD_Id { get; set; }
        public long IMCD_MobileNo { get; set; }
        public string IMCD_InOutFlg { get; set; }
        public DateTime? IMCD_DateTime { get; set; }
        public string IMCD_CallStatus { get; set; }
        public string IMCD_CallDuration { get; set; }
        public long IMCD_PulseCount { get; set; }
        public bool IMCD_ActiveFlg { get; set; }
        public long IMCD_CreatedBy { get; set; }
        public long IMCD_UpdatedBy { get; set; }
        public string TypeOfMobile { get; set; }

        public Array institute { get; set; }
        public Array monthdropdown { get; set; }
        public Array yearlist { get; set; }
        public Array maindata { get; set; }
        public string IMCD_IVRSText { get; set; }
        public string IMCD_IVRSVoiceURL { get; set; }
        public int IIVRSC_SchoolFlg { get; set; }

        public string getcurdueamount { get; set; }

        public long IIVRSC_MI_Id { get; set; }
        public string IIVRSC_VirtualNo { get; set; }

    }
}
