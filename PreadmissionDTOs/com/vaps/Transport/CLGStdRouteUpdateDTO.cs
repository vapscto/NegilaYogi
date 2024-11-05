 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class CLGStdRouteUpdateDTO
    {
        public long? PASHD_Id { get; set; }
        public long MI_Id { get; set; }
        public int TRMR_order { get; set; }
        public DateTime? PASHD_EntryDate { get; set; }
        public DateTime? PASHD_UpdateDate { get; set; }
        public long ASMAY_Id { get; set; }

        public long Session_ASMAY_Id { get; set; }
        public long PASR_Id { get; set; }
        public DateTime? PASHD_VaccinationDate { get; set; }
        public int? PASHD_FitsFlag { get; set; }
        public DateTime? PASHD_FitsDate { get; set; }
        public int? PASHD_Illness { get; set; }
        public DateTime? PASHD_HepatitisB { get; set; }
        public DateTime? PASHD_TyphoidFever { get; set; }
        public string PASHD_Ailment { get; set; }
        public int? PASHD_Allergy { get; set; }
        public string PASHD_HealthProblem { get; set; }
        public string PASHD_BloodGroup { get; set; }
        public string returnval { get; set; }
        public long? Id { get; set; }
        public long? roleId { get; set; }
        public long FCMA_Id { get; set; }
        public string trans_id { get; set; }
        public Array studentDetailsTEmp { get; set; }
        public Array studentDetails { get; set; }
        public Array instidet { get; set; }
        public string PASR_FirstName { get; set; }
        public string PASR_MiddleName { get; set; }
        public string PASR_LastName { get; set; }
        public string PASR_EMAIL { get; set; }
        public Array studenthelthDetails { get; set; }
        public Array studenthelth_DTObj { get; set; }
        public string PASR_FatherName { get; set; }
        public int? PASR_Age { get; set; }

        public Array paydet { get; set; }
        public Array studenthelthDTO { get; set; }
        public Array fillpaymentgateway { get; set; }

        public string pasR_RegistrationNo { get; set; }

        public string pasR_Student_Pic_Path { get; set; }

        public Array prospectusPaymentlist { get; set; }

        public long? pasr_id { get; set; }

        public long AMCST_Id { get; set; }

        public string paymentapplicable { get; set; }

        public int payementcheck { get; set; }

        public string AMST_AdmNo { get; set; }
        public long? AMST_MobileNo { get; set; }
        public string AMST_emailId { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public DateTime? AMST_DOB { get; set; }
        public string AMST_Photoname { get; set; }
        public long? ASMCL_Id { get; set; }
        public long? ASMS_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long AMB_Id { get; set; }
        public string ASMC_SectionName { get; set; }
        public string ASMAY_Year { get; set; }
        public string AMCST_FirstName { get; set; }
        public string AMCST_MiddleName { get; set; }
        public string AMCST_LastName { get; set; }
        public int? ASMAY_Order { get; set; }

        public long? PASR_MobileNo { get; set; }
        ///buss paass

        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }

        public MasterConfigurationDTO configurationsettings { get; set; }

        public Array studetailslist { get; set; }
        public string PASR_PerStreet { get; set; }
        public string paytype { get; set; }
        public string PASR_PerArea { get; set; }
        public long? PASR_PerPincode { get; set; }
        public long? PASR_FatherMobleNo { get; set; }
        public long? PASR_MotherMobleNo { get; set; }
        public string PASR_emailId { get; set; }
        public string PASR_FatherHomePhNo { get; set; }
        public string PASR_FatherOfficePhNo { get; set; }
        public string IVRMMC_CountryCode { get; set; }
        public long? IVRMMS_Id { get; set; }
        public string IVRMMS_Name { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string PASR_ConCountry { get; set; }
        public long? IVRMMC_Id { get; set; }
        public string IVRMMC_CountryName { get; set; }
        public Array studentpercountry { get; set; }
        public Array studentconstate { get; set; }

        public long? TRMA_Id { get; set; }
        public string TRMA_AreaName { get; set; }
        public string TRMA_AliasName { get; set; }
        public bool TRMA_ActiveFlg { get; set; }

        public long? TRMR_Id { get; set; }
        public long? TRMR_Idp { get; set; }
        public long? TRMR_Idd { get; set; }
        public string TRMR_RouteName { get; set; }
        public string TRMR_RouteNo { get; set; }
        public string TRMR_RouteDesc { get; set; }
        public bool TRMR_ActiveFlg { get; set; }

        public long? TRML_Id { get; set; }
        public string TRML_Latitude { get; set; }
        public string TRML_Longitude { get; set; }
        public bool TRML_ActiveFlg { get; set; }
        public string TRML_LocationName { get; set; }
        public string TRML_PickLocationName { get; set; }
        public string TRML_DropLocationName { get; set; }
        public string TRMR_PickRouteName { get; set; }
        public string TRMR_DropRouteName { get; set; }

        public Master_NumberingDTO transnumconfigsettings { get; set; }


        public long? PASTA_Id { get; set; }

        public long? PASTA_ApplicationNo { get; set; }
        public DateTime? PASTA_ApplicationDate { get; set; }
        public string ASTA_AreaZoneName { get; set; }

        public long? PASTA_PickupSMSMobileNo { get; set; }
        public long? PASTA_DropSMSMobileNo { get; set; }
        public DateTime? PASTA_PaymentDate { get; set; }

        public string PASTA_ReceiptNo { get; set; }

        public decimal PASTA_Amount { get; set; }
        public bool PASTA_ActiveFlag { get; set; }

        public string message { get; set; }
        public long? TRML_Idp { get; set; }
        public long? TRML_Idd { get; set; }

        public string AMST_PerStreet { get; set; }
        public string AMST_PerCity { get; set; }
        public string AMST_PerArea { get; set; }
        public int? AMST_PerPincode { get; set; }
        public string AMST_FatherName { get; set; }
        public string AMST_MotherName { get; set; }
        public long? AMST_FatherMobleNo { get; set; }
        public long? AMST_MotherMobileNo { get; set; }

        public long ASTA_FatherMobileNo { get; set; }
        public long ASTA_MotherMobileNo { get; set; }
        public string AMST_FatherOfficeAdd { get; set; }
        public string AMST_BloodGroup { get; set; }
        public Array areaList { get; set; }
        public Array routelist { get; set; }
        public Array locationlist { get; set; }
        public Array routeDetails { get; set; }
        public Array buspassdatalist { get; set; }

        public long? ASTACO_Id { get; set; }
        public long? ASTA_PickUp_TRMR_Id { get; set; }
        public long? ASTA_PickUp_TRML_Id { get; set; }
        public long? ASTA_Drop_TRMR_Id { get; set; }
        public long? ASTA_Drop_TRML_Id { get; set; }
        public Array countryid { get; set; }
        public Array stateid { get; set; }
        public int? year_Order { get; set; }
        public long year_Id { get; set; }
        public int? cls_Order { get; set; }
        public long? cls_Id { get; set; }
        public long? Class_Id { get; set; }
        public Array stu_cls_Id { get; set; }
        public Array stutransapp { get; set; }
        public Array stu_name { get; set; }

        public Array regularnew { get; set; }

        public Array regularnewff { get; set; }

        public string newregular { get; set; }
        public Array routeList { get; set; }
        public Array locaList { get; set; }
        public Array trans_amstid { get; set; }

        public string ASTA_Landmark { get; set; }

        public long UserId { get; set; }

        public string logopath { get; set; }

        public Array logoheader { get; set; }

        public long classnextid { get; set; }

        public string approvenot { get; set; }

        public string ASTA_ApplStatus { get; set; }

        public bool ASTA_ActiveFlag { get; set; }

        public string approoved { get; set; }


        public long FMG_Id { get; set; }

        public long openingbalance { get; set; }

        public bool openba { get; set; }

        public long? ASTA_Phoneoff { get; set; }

        public long? ASTA_PhoneRes { get; set; }

        public Array fillyear { get; set; }

        public Array currfillyear { get; set; }

        public long transportyear { get; set; }

        public long studentcurrentyear { get; set; }

        public long studentaccyear { get; set; }

        public long? studentsem { get; set; }

        public long? studentfuturesem { get; set; }

        public long ASTA_FutureAY { get; set; }

        public string trnsportcutoffdate { get; set; }

        public string paymentoffdate { get; set; }

        public string studentstatus { get; set; }

        public string studentTrstatus { get; set; }

        public bool transappfillTrNew { get; set; }

        public bool transappfillTrRegular { get; set; }

        public bool transappfillAdmissionNew { get; set; }

        public long studentid { get; set; }

        public string searchfilter { get; set; }

        public Array fillstudent { get; set; }

        public string htmldata { get; set; }

        public string oneortwoway { get; set; }

        public string oneortwowayupdate { get; set; }

        public string newcurrent { get; set; }
        public string merchantkey { get; set; }
        public decimal FMA_Amount { get; set; }
        public string splitpayinformation { get; set; }
    }

}
