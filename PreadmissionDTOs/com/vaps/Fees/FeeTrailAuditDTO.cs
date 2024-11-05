using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeTrailAuditDTO
    {
        public List<Adm_M_StudentDTO> admstudto { set; get; }

       // public List<IVRM_Fee_Master_Transaction_LogsDTO> admstudtotm { set; get; }
       // public List<IVRM_Fee_T_Transaction_LogsDTO> admstudtomt { set; get; }

        public Array admsudentslist { get; set; }
        public Array usersnameslist { get; set; }
        public Array feereceiptNolistDB { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public long Amst_Id { get; set; }
        public long MI_ID { get; set; }
        public string NormalizedUserName { get; set; }
        public int userId { get; set; }
        public string receiptNo { get; set; }
        public long paymentid { get; set; }
        public Array newreplist { get; set; }
        // for dto
        public long useridpassing { get; set; }
        public long asmay_id { get; set; }
        public long amst { get; set; }
      
        public DateTime? fromdate { get; set; }
        public DateTime? todate { get; set; }
        public string saveflg { get; set; }
        public string updateflg { get; set; }
        public string deleteflg { get; set; }
        public Array reportdatelist { get; set; }

        //report

        public string receiptnorpt { get; set; }
        public string userrpt { get; set; }
        public string studentrpt { get; set; }
        public string transtyperpt { get; set; }
        public DateTime daterpt { get; set; }
        public DateTime timerpt { get; set; }
        public string feedetailsrpt { get; set; }
        public double amountrpt { get; set; }
        public double connectoionrpt { get; set; }
        public double waiverpt { get; set; }
        public double finerpt { get; set; }
        public string statusrpt { get; set; }
        public string ipaddressrpt { get; set; }
        public Array TempararyArrayheadList { get; set; }
        


        //MB
        public bool Save_flag { get; set; }
        public bool Update_flag { get; set; }
        public bool Delete_flag { get; set; }

        public string User_Name { get; set; }
        public string Name { get; set; }
        public string FYP_Receipt_No { get; set; }
        public string FYP_Bank_Or_Cash { get; set; }
        public DateTime ITAT_Date { get; set; }
        public TimeSpan ITAT_Time { get; set; }
        public string FYP_Remarks { get; set; }
        public decimal FYP_Tot_Amount { get; set; }
        public decimal FYP_Tot_Waived_Amt { get; set; }
        public decimal FYP_Tot_Fine_Amt { get; set; }
        public decimal FYP_Tot_Concession_Amt { get; set; }
        public string ITAT_Operation { get; set; }
        public string ITAT_IPV4Address { get; set; }
        public string ITAT_MAACAddress { get; set; }
        public long ITAT_Id { get; set; }
        public long HRME_Id { get; set; }
        public long FMOST_Id { get; set; }
        public Array Main_Details { get; set; }
        public string Report_Type { get; set; }
        public Array D_Students { get; set; }
        public Array D_Receipts { get; set; }
        public string Status_IU_D { get; set; }
        public long pasr_id { get; set; }

        public Array yearlist { get; set; }
        public Array searcharray { get; set; }

        public string searchfilter { get; set; }

        public long userid { get; set; }
        public string filterinitialdata { get; set; }

        public Array fillstud { get; set; }

        //MB
        public bool User_Status { get; set; }
        public long FYP_Id { get; set; }
        public Array getviewdetails { get; set; }
    }
}
