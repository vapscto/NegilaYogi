using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class RouteTermFeeDetailsDTO
    {
        public long MI_Id { get; set; }
        public Array YearList { get; set; }
        public long ASMAY_Id { get; set; }
        public bool Is_Active { get; set; }

        public long TRMR_Id { get; set; }
        public long TRMA_Id { get; set; }
        public string TRMR_RouteName { get; set; }
        public string TRMR_RouteNo { get; set; }
        public string TRMR_RouteDesc { get; set; }
        public bool TRMR_ActiveFlg { get; set; }

        public long FMT_Id { get; set; }
     
        public string FMT_Name { get; set; }
        public bool FMT_ActiveFlag { get; set; }

        public string FromMonth { get; set; }
        public string ToMonth { get; set; }

        public int? FMT_Order { get; set; }

        public string FMT_Year { get; set; }

        public string Transport_FromMonth { get; set; }
        public string Transport_ToMonth { get; set; }

        public Array messagelist { get; set; }
        public Array termlist { get; set; }
        public Array classdata { get; set; }
        public Array cdeposit { get; set; }
        
        public long stud_count { get; set; }
        public Array griddata { get; set; }
        public long ASMCL_Id { get; set; }
        public long AMST_Id { get; set; }


        public long FSS_Id { get; set; }
      
        public long FMG_Id { get; set; }
        public long FMH_Id { get; set; }
        public long FTI_Id { get; set; }
        public long FMA_Id { get; set; }
        public long FSS_OBArrearAmount { get; set; }
        public long FSS_OBExcessAmount { get; set; }
        public long FSS_CurrentYrCharges { get; set; }
        public long FSS_TotalToBePaid { get; set; }
        public long FSS_ToBePaid { get; set; }
        public long FSS_PaidAmount { get; set; }
        public long FSS_ExcessPaidAmount { get; set; }
        public long FSS_ExcessAdjustedAmount { get; set; }
        public long FSS_RunningExcessAmount { get; set; }

        public long FSS_ConcessionAmount { get; set; }
        public long FSS_AdjustedAmount { get; set; }
        public long FSS_WaivedAmount { get; set; }
        public long FSS_RebateAmount { get; set; }
        public decimal FSS_FineAmount { get; set; }
        public long FSS_RefundAmount { get; set; }

        public long FSS_RefundAmountAdjusted { get; set; }
        public decimal FSS_NetAmount { get; set; }
        public bool FSS_ChequeBounceFlag { get; set; }
        public bool FSS_ArrearFlag { get; set; }
        public bool FSS_RefundOverFlag { get; set; }
        public bool FSS_ActiveFlag { get; set; }

        public long User_Id { get; set; }
        //praveen
        public DateTime frmdate { get; set; }
        public DateTime todate { get; set; }
        public bool checkdate { get; set; }
    }
}
