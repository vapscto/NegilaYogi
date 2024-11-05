using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class StudentHelthcertificateDTO:CommonParamDTO
    {
        public long PASHD_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime? PASHD_EntryDate { get; set; }
        public DateTime? PASHD_UpdateDate { get; set; }
        public long ASMAY_Id { get; set; }
        public long PASR_Id { get; set; }
        public DateTime? PASHD_VaccinationDate { get; set; }
        public int PASHD_FitsFlag { get; set; }
        public DateTime? PASHD_FitsDate { get; set; }
        public int PASHD_Illness { get; set; }
        public DateTime? PASHD_HepatitisB { get; set; }
        public DateTime? PASHD_TyphoidFever { get; set; }
        public string PASHD_Ailment { get; set; }
        public string PASHD_Allergy { get; set; }
        public DateTime? PASR_Date { get; set; }

        public string PASHD_HealthProblem { get; set; }
        public string PASHD_BloodGroup { get; set; }
        public bool returnval { get; set; }

        public bool updated { get; set; }
        public long Id { get; set; }
        public long roleId { get; set; }
        public Array studentDetailsTEmp { get; set; }

        public Array transport  { get; set; }
        public Array studentDetails { get; set; }
        public string PASR_FirstName { get; set; }
        public string PASR_MiddleName { get; set; }
        public string PASR_LastName { get; set; }
        public string PASR_EMAIL { get; set; }
        public Array studenthelthDetails { get; set; }
        public Array studenthelth_DTObj { get; set; }

        public Array vaccines { get; set; }
        public string PASR_FatherName { get; set; }
        public int? PASR_Age { get; set; }
        public Array studenthelthDTO { get; set; }

        public string pasR_RegistrationNo { get; set; }

        public string pasR_Student_Pic_Path { get; set; }

        public Array prospectusPaymentlist { get; set; }

        public long pasr_id { get; set; }


        public long PAMVA_Id { get; set; }
        public string PAMVA_Vaccines { get; set; }
        public bool PAMVA_YesNoFlg { get; set; }

        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }

        public MasterConfigurationDTO configurationsettings { get; set; }

        public StudentVaccines[] StudentVaccines { get; set; }

        ///buss paass

        public Array studetailslist { get; set; }
        public string PASR_PerStreet { get; set; }
        public string PASR_PerArea { get; set; }
        public long PASR_PerPincode { get; set; }
        public long PASR_FatherMobleNo { get; set; }
        public long PASR_MotherMobleNo { get; set; }
        public string PASR_emailId { get; set; }
        public string PASR_FatherHomePhNo { get; set; }
        public string PASR_FatherOfficePhNo { get; set; }
        public string IVRMMC_CountryCode { get; set; }
        public long IVRMMS_Id { get; set; }
        public string IVRMMS_Name { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string PASR_ConCountry { get; set; }
        public long IVRMMC_Id { get; set; }
        public string IVRMMC_CountryName { get; set; }
        public Array studentpercountry { get; set; }
        public Array studentconstate { get; set; }

        public long TRMA_Id { get; set; }
        public string TRMA_AreaName { get; set; }
        public string TRMA_AliasName { get; set; }
        public bool TRMA_ActiveFlg { get; set; }


        public long TRMR_Id { get; set; }
        public string TRMR_RouteName { get; set; }
        public string TRMR_RouteNo { get; set; }
        public string TRMR_RouteDesc { get; set; }
        public bool TRMR_ActiveFlg { get; set; }

        public long TRML_Id { get; set; }
        public string TRML_Latitude { get; set; }
        public string TRML_Longitude { get; set; }
        public bool TRML_ActiveFlg { get; set; }
        public string TRML_LocationName { get; set; }
        public string TRML_PickLocationName { get; set; }
        public string TRML_DropLocationName { get; set; }


        public bool PASHD_AllergyFlg { get; set; }

        public string PASHD_CronicDesease { get; set; }

        public string PASHD_MEDetails { get; set; }

        public long PASHD_MEContactNo { get; set; }

        public long PASTA_Id { get; set; }

        public long PASTA_ApplicationNo { get; set; }
        public DateTime? PASTA_ApplicationDate { get; set; }
        public string PASTA_AreaZoneName { get; set; }

        public long? PASTA_PickupSMSMobileNo { get; set; }
        public long? PASTA_DropSMSMobileNo { get; set; }
        public DateTime? PASTA_PaymentDate { get; set; }

        public string PASTA_ReceiptNo { get; set; }

        public decimal PASTA_Amount { get; set; }
        public bool PASTA_ActiveFlag { get; set; }

        public string message { get; set; }
        public long TRML_Idp { get; set; }
        public long TRML_Idd { get; set; }
        public long? PASTA_PickUp_TRMR_Id { get; set; }
        public long? PASTA_PickUp_TRML_Id { get; set; }
        public long? PASTA_Drop_TRMR_Id { get; set; }
        public long? PASTA_Drop_TRML_Id { get; set; }
        public Array areaList { get; set; }
        public Array routelist { get; set; }
        public Array locationlist { get; set; }
        public Array routeDetails { get; set; }
        public Array buspassdatalist { get; set; }

        public long ASMCL_Id { get; set; }

        
        public long PASR_MobileNo { get; set; }

        public string paymentapplicable { get; set; }

        public int payementcheck { get; set; }

        public Array paydet { get; set; }


        public int PASHD_ChickenpoxFlag { get; set; }

        public DateTime? PASHD_ChickenpoxDate { get; set; }

        public int PASHD_DiptheriaFlag { get; set; }

        public DateTime? PASHD_DiptheriaDate { get; set; }

        public int PASHD_EpidemicFlag { get; set; }

        public DateTime? PASHD_EpidemicDate { get; set; }

        public int PASHD_MeaslesFlag { get; set; }

        public DateTime? PASHD_MeaslesDate { get; set; }

        public int PASHD_MumpsFlag { get; set; }

        public DateTime? PASHD_MumpsDate { get; set; }

        public int PASHD_RingwormFlag { get; set; }

        public DateTime? PASHD_RingwormDate { get; set; }

        public int PASHD_ScarletFlag { get; set; }

        public DateTime? PASHD_ScarletDate { get; set; }

        public int PASHD_SmallPoxFlag { get; set; }

        public DateTime? PASHD_SmallPoxDate { get; set; }

        public int PASHD_WhoopingFlag { get; set; }

        public DateTime? PASHD_WhoopingDate { get; set; }

        public string manualAdmFlag { get; set; }
        public string ApplicationNo { get; set; }
        public bool countrole { get; set; }
    }
}
