using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class TransportStatusReportDTO
    {

        public string onclickloaddata { get; set; }
        public Array YearList { get; set; }
        public Array messagelist { get; set; }
        public Array countlist { get; set; }
        public string[] mdata { get; set; }
        public long stud_count { get; set; }
    


        public long ASTA_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long TRML_Id { get; set; }

        public string ASTA_ApplicationNo { get; set; }
        public long? ASTA_FatherMobileNo { get; set; }
        public DateTime? ASTA_ApplicationDate { get; set; }
        public DateTime FMCB_fromDATE { get; set; }
        public DateTime FMCB_toDATE { get; set; }
        public DateTime? ASTAA_Date { get; set; }

        public string ASTA_AreaZoneName { get; set; }

        public long TRMA_Id { get; set; }

        public long ASTA_PickupSMSMobileNo { get; set; }

        public long ASTA_DropSMSMobileNo { get; set; }

        public string ASTA_ApplStatus { get; set; }
        public DateTime ASTA_PaymentDate { get; set; }
        public string ASTA_ReceiptNo { get; set; }

        public string AMST_BloodGroup { get; set; }
        public decimal ASTA_Amount { get; set; }

        public bool ASTA_ActiveFlag { get; set; }


        public long ASTA_PickUp_TRMR_Id { get; set; }
        public long ASTA_PickUp_TRML_Id { get; set; }
        public long ASTA_Drop_TRMR_Id { get; set; }
        public long ASTA_Drop_TRML_Id { get; set; }

        public long ASTA_CurrentAY { get; set; }
        public long ASTA_CurrentClass { get; set; }
        public long ASTA_FutureAY { get; set; }
        public long ASTA_FutureClass { get; set; }

        public long ASTAA_Id { get; set; }

        public long IVRMUL_Id { get; set; }




        public long ASMAY_Id { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public string routename { get; set; }
        public string locationname { get; set; }
        public long TRSR_DropLocation { get; set; }
        public string ASTA_Regnew { get; set; }
        public long TRSR_PickUpLocation { get; set; }
        public string TRML_PickLocationName { get; set; }
        public string TRML_DropLocationName { get; set; }
        public string TRMR_PickRouteName { get; set; }
        public string TRMR_DropRouteName { get; set; }
        public long TRMR_Idp { get; set; }
        public long TRMR_Idd { get; set; }
        public bool cnt12 { get; set; }

        public bool updatedstu  { get; set; }
        public string cnt11 { get; set; }
        public string regorname_map { get; set; }
        public string TRMR_RouteName { get; set; }

        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string AMST_AdmNo { get; set; }

        public string paymentoption { get; set; }
        public long ASMCL_Id { get; set; }
        public long TRMR_Id { get; set; }
        public Array masterclass { get; set; }
        public Array routendetails { get; set; }

        public long ASTAU_PickUp_TRMR_Id { get; set; }
        public long ASTAU_PickUp_TRML_Id { get; set; }
        public long ASTAU_Drop_TRMR_Id { get; set; }
        public long ASTAU_Drop_TRML_Id { get; set; }
        public string TRMLU_PickLocationName { get; set; }
        public string TRMLU_DropLocationName { get; set; }
        public string TRMRU_PickRouteName { get; set; }
        public string TRMRU_DropRouteName { get; set; }
        public long TRMRU_Idp { get; set; }
        public long TRMRU_Idd { get; set; }

        public DateTime lastupdated { get; set; }

        public DateTime newlyupdated { get; set; }

        public string updatedtype { get; set; }
        public string AMST_Photoname { get; set; }


    }
}
