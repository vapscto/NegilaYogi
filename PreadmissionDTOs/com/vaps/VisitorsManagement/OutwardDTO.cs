using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VisitorsManagement
{
   public class OutwardDTO:CommonParamDTO
    {
        
        #region   old Line
        //public long OW_Id { get; set; }
        //public long MI_Id { get; set; }
        //public string OW_Discription { get; set; }
        //public string OW_From { get; set; }
        //public string OW_To { get; set; }
        //public string OW_add { get; set; }
        //public string OW_Date { get; set; }
        //public string OW_Remarks { get; set; }
        //public string OW_Action_By { get; set; }
        //public bool OW_ActiveFlag { get; set; }
        //public Array gridoptions { get; set; }
        //public int count { get; set; }
        //public Array editDetails { get; set; }
        //public string returnVal { get; set; }
        //public bool value { get; set; }
        //public string msg { get; set; }
        //public Array institution { get; set; }
        //public Array outward { get; set; }
        #endregion


        public long FOOUT_Id { get; set; }
        public long MI_Id { get; set; }
        public string FOOUT_OutwardNo { get; set; }
        public DateTime FOOUT_DateTime { get; set; }
        public string FOOUT_Discription { get; set; }
        public string FOOUT_From { get; set; }
        public string FOOUT_To { get; set; }
        public string FOOUT_Address { get; set; }
        public long? FOOUT_PhoneNo { get; set; }
        public string FOOUT_EmailId { get; set; }
        public long FOOUT_DispatachedBy { get; set; }
        public string FOOUT_DispatchedThrough { get; set; }
        public string FOOUT_DispatchedDeatils { get; set; }
        public string FOOUT_DispatchedPhNo { get; set; }
        public bool FOOUT_ActiveFlag { get; set; }
        public long FOOUT_CreatedBy { get; set; }
        public long FOOUT_UpdatedBy { get; set; }
        
        public Array getdata { get; set; }
        public Array editDetails { get; set; }
        public string returnval { get; set; }
        public bool duplicate { get; set; }
        public Array emplist { get; set; }

        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public long HRME_Id { get; set; }

        public Array institution { get; set; }
        public Array outward { get; set; }
        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }
        public long ASMAY_Id { get; set; }
        public long UserId { get; set; }
        public string trans_id { get; set; }
        public bool returnval2 { get; set; }

    }
}
