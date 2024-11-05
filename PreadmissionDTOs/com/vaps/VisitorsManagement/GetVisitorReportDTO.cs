using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VisitorsManagement
{
    public class GetVisitorReportDTO
    {
        public long AMVM_Id { get; set; }
        public long MI_Id { get; set; }
        public string AMVM_Name { get; set; }
        public string AMVM_Contact_No { get; set; }
        public string AMVM_Emailid { get; set; }
        public string AMVM_Card_No { get; set; }
        public string AMVM_Identity_Type { get; set; }
        public string AMVM_Address { get; set; }
        public string AMVM_To_Meet { get; set; }
        public string AMVM_Purpose { get; set; }
        public DateTime? Date_Visit { get; set; }
        public string Time_Visit { get; set; }
        public string AMVM_Type { get; set; }
        public string AMVM_Status { get; set; }
        public string AMVM_Out_Time { get; set; }
        public DateTime? AMVM_Entry_Date { get; set; }
        public string AMVM_Remarks { get; set; }
        public bool AMVM_ActiveFlag { get; set; }
        public Array viewlist { get; set; }
        public string searchby { get; set; }
        public string txtbox { get; set; }
        public DateTime? Date_vist { get; set; }


        public string month_id { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public string all1 { get; set; }

    }
}
