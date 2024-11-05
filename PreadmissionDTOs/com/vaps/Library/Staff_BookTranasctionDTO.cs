using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Library
{
   public class Staff_BookTranasctionDTO:CommonParamDTO
    {
        public long Book_Trans_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMST_Id { get; set; }
        public long LMBANO_Id { get; set; }
        public DateTime Issue_Date { get; set; }
        public DateTime Due_Date { get; set; }
        public DateTime Return_Date { get; set; }
        public decimal Fine_Amount { get; set; }
        public decimal Waived_Amount { get; set; }
        public int Renewal_Counter { get; set; }
        public string Book_Trans_Status { get; set; }
        public long FODM_Id { get; set; }
        public long Guest_Id { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public string HRME_EmailId { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public long? HRME_MobileNo { get; set; }
        public string HRME_Photo { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public long HRMD_Id { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public long HRMDES_Id { get; set; }
        public Array stafftlist { get; set; }
        public Array configdata { get; set; }



        public Array booktitle { get; set; }
        public Array alldata { get; set; }
        public string LMB_BookTitle { get; set; }
        public string LMB_BookType { get; set; }
        public string LMB_ClassNo { get; set; }
        public decimal LMB_Price { get; set; }
        public string LMB_VolNo { get; set; }
        public string LMBANO_AccessionNo { get; set; }
        public DateTime LMB_EntryDate { get; set; }
        public Array selctstaffdata { get; set; }
        public Array bookdetails { get; set; }
        public long LMB_Id { get; set; }
        public long LMBA_Id { get; set; }
        public long LMS_Id { get; set; }
        public long LMD_Id { get; set; }
        public long HRME_Id { get; set; }
        public long? LMP_Id { get; set; }
        public long? LML_Id { get; set; }
        public long Donor_Id { get; set; }
        public long LMV_Id { get; set; }
        public long LMC_Id { get; set; }
        public long Rack_Id { get; set; }
        public Array getalldata { get; set; }
        public Array editlist { get; set; }
       
        public int Max_Issue_Items { get; set; }
        public int Max_Issue_Days { get; set; }
        public int Max_No_Renewals { get; set; }

        public bool renew { get; set; }
    }
}
