using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class StudentRouteMappingReportDTO
    {
        public string onclickloaddata { get; set; }
        public Array YearList { get; set; }
        public Array messagelist { get; set; }
        public Array countlist { get; set; }
        public string[] mdata { get; set; }
        public long stud_count { get; set; }


        public long ASTA_Id { get; set; }
        public long FMG_Id { get; set; }

        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long TRML_Id { get; set; }
        public long TRMS_Id { get; set; }

        public string ASTA_ApplicationNo { get; set; }
        public DateTime ASTA_ApplicationDate { get; set; }

        public string ASTA_AreaZoneName { get; set; }

        public long TRMA_Id { get; set; }

        public long ASTA_PickupSMSMobileNo { get; set; }

        public long ASTA_DropSMSMobileNo { get; set; }

        public string ASTA_ApplStatus { get; set; }
        public DateTime ASTA_PaymentDate { get; set; }
        public string ASTA_ReceiptNo { get; set; }


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
        public string TRMR_RouteName { get; set; }


        public long ASMAY_Id { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public Array routename { get; set; }
        public Array sessionlist { get; set; }
        public Array grouplist { get; set; }
        public long TRMR_Id { get; set; }
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
        public string regorname_map { get; set; }

        public string Paidnotpaid  { get; set; }
        public Array scheduledata { get; set; }
        public Array seclist { get; set; }
        public Array classlist { get; set; }

        public long ASMCL_Id { get; set; }

        public long ASMS_Id { get; set; }
        public bool feeflag { get; set; }


    }
}
