using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class MasterEmployeeDTO :CommonParamDTO
    {
         public long HRME_Id { get; set; }
         public long MI_Id { get; set; }
         public long? HRMET_Id { get; set; }
         public long? HRMGT_Id { get; set; }
         public long? HRMD_Id { get; set; }
         public long? HRMDES_Id { get; set; }
         public long? HRMG_Id { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public string HRME_BiometricCode { get; set; }
        public string HRME_RFCardId { get; set; }
        public string HRME_PerStreet { get; set; }
        public string HRME_PerArea { get; set; }
        public string HRME_PerCity { get; set; }
        public string HRME_PerAdd4 { get; set; }
         public long? HRME_PerStateId { get; set; }
         public long? HRME_PerCountryId { get; set; }
         public long? HRME_PerPincode { get; set; }
        public string HRME_LocStreet { get; set; }
        public string HRME_LocArea { get; set; }
        public string HRME_LocCity { get; set; }
        public string HRME_LocAdd4 { get; set; }
         public long? HRME_LocStateId { get; set; }
         public long? HRME_LocCountryId { get; set; }
         public long? HRME_LocPincode { get; set; }
         public long? IVRMMMS_Id { get; set; }
         public long? IVRMMG_Id { get; set; }
         public long? CasteCategoryId { get; set; }
         public long? CasteId { get; set; }
         public long? ReligionId { get; set; }
        public string HRME_FatherName { get; set; }
        public string HRME_MotherName { get; set; }
        public string HRME_SpouseName { get; set; }
        public string HRME_SpouseOccupation { get; set; }
         public long? HRME_SpouseMobileNo { get; set; }
        public string HRME_SpouseEmailId { get; set; }
        public string HRME_SpouseAddress { get; set; }
        public DateTime? HRME_DOB { get; set; }
        public DateTime? HRME_DOJ { get; set; }
        public DateTime? HRME_ExpectedRetirementDate { get; set; }
        public DateTime? HRME_PFDate { get; set; }
        public DateTime? HRME_ESIDate { get; set; }
         public long? HRME_MobileNo { get; set; }
        public string HRME_EmailId { get; set; }
        public string HRME_BloodGroup { get; set; }
        public string HRME_PaymentType { get; set; }
        public string HRME_BankAccountNo { get; set; }
        public bool? HRME_PFApplicableFlag { get; set; }
       public bool? HRME_PFMaxFlag { get; set; }
        public bool? HRME_PFFixedFlag { get; set; }
        public string HRME_PFAccNo { get; set; }
        public string HRME_ESIAccNo { get; set; }
        public string HRME_GratuityAccNo { get; set; }
        public bool? HRME_ESIApplicableFlag { get; set; }
        public string HRME_Photo { get; set; }
        public bool? HRME_LeftFlag { get; set; }
        public DateTime? HRME_DOC { get; set; }
        public DateTime? HRME_DOL { get; set; }
        public string HRME_LeavingReason { get; set; }
        public string HRME_Height { get; set; }
        public string HRME_HeightUOM { get; set; }
        public int? HRME_Weight { get; set; }
        public string HRME_WeightUOM { get; set; }
        public string HRME_IdentificationMark { get; set; }
        public string HRME_ApprovalNo { get; set; }
        public string HRME_PANCardNo { get; set; }
         public long? HRME_AadharCardNo { get; set; }
        public bool? HRME_SubstituteFlag { get; set; }
        public string HRME_NationalSSN { get; set; }
        public string HRME_SalaryType { get; set; }
        public int? HRME_EmployeeOrder { get; set; }
        public bool? HRME_ActiveFlag { get; set; }
        public string HRME_SubjectsTaught { get; set; }
        public string HRME_UINumber { get; set; }
        public string HRME_AnyHealthIssue { get; set; }
        public string HRME_EyeSightIssue { get; set; }
        public long? HRME_TransfferedTo { get; set; }
        public bool? HRME_RecordTransferredFlg { get; set; }
        public bool? HRME_PHDResearchGuideFlg { get; set; }
        public string HRME_TechNonTeachingFlg { get; set; }
        public long? HRME_RGRecognitionYear { get; set; }

        public bool? HRME_ExcPunch { get; set; }
        public bool? HRME_RetiredFlg { get; set; }
        public DateTime? HRME_PensionStoppedDate { get; set; }
        public bool? HRME_FPFNotApplicableFlg { get; set; }
        public bool? HRME_EPFNotApplicableFlg { get; set; }
        public bool? HRME_AllowSoftwareAccessFlg { get; set; }

        public Array employeedetailList { get; set; }

        public Array experienceDetails { get; set; }

        public Array qualificationDetails { get; set; }

        public Array documentList { get; set; }
        public Array LeavingReasonList { get; set; }
        public Array institutiondropdownlist { get; set; }


        public string retrunMsg { get; set; }

         public long? roleId { get; set; }

        public MasterEmployeeDTO Employeedto { get; set; }


        //dropdown Array list
        public Array employeeTypedropdownlist { get; set; }
        public Array groupTypedropdownlist { get; set; }
        public Array departmentdropdownlist { get; set; }
        public Array designationdropdownlist { get; set; }
        public Array gradedropdownlist { get; set; }
        public Array genderdropdownlist { get; set; }
        public Array maritalStatusdropdownlist { get; set; }

        public Array coursedropdownlist { get; set; }
        public Array SpecialisationList { get; set; }
        public Array countrydropdownlist { get; set; }
        public Array castedropdownlist { get; set; }
        public Array casteCategorydropdownlist { get; set; }
        public Array religiondropdownlist { get; set; }

        public Array AllState { get; set; }

        //Experiance details Array object

        public Master_Employee_ExperienceDTO[] EmployeeExperienceDTO { get; set; }

        public Master_Employee_QulaificationDTO[] EmployeeQulaificationDTO { get; set; }

        public Master_Employee_DocumentsDTO[] EmployeeDocumentDTO { get; set; }

        //EmployeeDocumentDTO

        //Type
        public string Type { get; set; }

        public Array earningList { get; set; }
        public Array detectionList { get; set; }
        public HR_Master_Employee_IncrementDetailsDTO IncrementDetailsDTO { get; set; }

        public HR_Employee_EarningsDeductionsDTO[] EarningDTO { get; set; }
        public HR_Employee_EarningsDeductionsDTO[] DeductionDTO { get; set; }
        
            public HR_Employee_EarningsDeductionsDTO[] ArrearDTO { get; set; }
        public Array incrementDetails { get; set; }


        public Array employeeEarningsDeductionsDetails { get; set; }

        public HR_ConfigurationDTO configurationDetails { get; set; }

        public MasterEmployeeDTO[] EmporderDTO { get; set; }

        //added by sudeep
        public Mobile_Number_DTO[] mobile_list_dto { get; set; }

        public Email_Id_DTO[] email_list_dto { get; set; }

         public long? HRMEMNO_MobileNo { get; set; }

        public string HRMEM_EmailId { get; set; }

         public long? HRMEMNO_Id { get; set; }
         public long? HRMEEM_Id { get; set; }


        public string tcflagexists { get; set; }
        public Array arrearList { get; set; }

        public string TabName { get; set; }

        public Array grossList { get; set; }

        public Array bankdropdownlist { get; set; }

        public Array employeebankDetails { get; set; }
        public HR_Master_Employee_BankDTO[] employeebankDTO { get; set; }
         public long IVRMUL_Id { get; set; }

        public Array selectedmobile_list_dto { get; set; }
        public Array selectedemail_list_dto { get; set; }

        public long LogInUserId { get; set; }
        public string username { get; set; }

        public string HRMD_DepartmentName { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public string IMC_CasteName { get; set; }
        public string HRME_QualificationName { get; set; }
        public long IMC_Id { get; set; }
        public long HRMEQ_Id { get; set; }
        public Array departmentlist { get; set; }
        public Array designationlist { get; set; }
        public Array castlist { get; set; }
        public Array qualificationlist { get; set; }
        public string MI_Logo { get; set; }
        public string HRME_MSTeamsUserId { get; set; }
        public string HRME_MSTeamsEmailId { get; set; }
        public string HRME_MSTeamsPassword { get; set; }

        public decimal? HREED_ApplicableMaxValue { get; set; }
        public bool? HREED_MaxApplicableFlg { get; set; }
    }
}
