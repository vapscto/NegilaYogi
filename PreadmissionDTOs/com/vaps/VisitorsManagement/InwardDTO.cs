using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VisitorsManagement
{
    public class InwardDTO : CommonParamDTO
    {
        #region old data
        //public long MI_Id { get; set; }
        //public long IW_Id { get; set; }
        //public string IW_Discription { get; set; }
        //public string IW_From { get; set; }
        //public string IW_To { get; set; }
        //public int IW_No { get; set; }
        //public DateTime? IW_Date { get; set; }
        //public string IW_Remarks { get; set; }
        //public string IW_Action_By { get; set; }
        //public string Ass_To { get; set; }
        //public Array gridoptions { get; set; }
        //public int count { get; set; }
        //public Array editDetails { get; set; }
        //public string returnVal { get; set; }
        //public bool value { get; set; }
        //public string msg { get; set; }
        //public bool IW_ActiveFlag { get; set; }
        //public Array inward { get; set; }
        //public Array institution { get; set; }
        #endregion


        public long FOIN_Id { get; set; }
        public long MI_Id { get; set; }
        public string FOIN_InwardNo { get; set; }
        public DateTime? FOIN_DateTime { get; set; }
        public string FOIN_From { get; set; }
        public string FOIN_Adddress { get; set; }
        public string FOIN_ContactPerson { get; set; }
        public string FOIN_PhoneNo { get; set; }
        public string FOIN_EmailId { get; set; }
        public string FOIN_Discription { get; set; }
        public long FOIN_To { get; set; }
        public long FOIN_ReceivedBy { get; set; }
        public long FOIN_HandedOverTo { get; set; }
        public bool FOIN_ActiveFlag { get; set; }
        public long FOIN_CreatedBy { get; set; }
        public long FOIN_UpdatedBy { get; set; }
        public long ASMAY_Id { get; set; }
        public long UserId { get; set; }

        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public long HRME_Id { get; set; }

        public bool returnval2 { get; set; }
        public string returnval { get; set; }
        public Array emplist { get; set; }
        public Array emplist2 { get; set; }
        public Array emplist3 { get; set; }


        public string searchfilter { get; set; }
        public long empid2 { get; set; }
        public bool duplicate { get; set; }
        public string trans_id { get; set; }
        public long empid3 { get; set; }
        public Array getdataall { get; set; }
        public string firstName1 { get; set; }
        public string firstName2 { get; set; }      
        public string firstName3 { get; set; }
        public string secnam1 { get; set; }
        public string secnam2 { get; set; }
        public string secnam3 { get; set; }
        public Array editDetails { get; set; }
        public string employeename1 { get; set; }
        public long hrmid1 { get; set; }
        public string employeename12 { get; set; }
        public long hrmid12 { get; set; }
        public Array curntInwardrec { get; set; }
        public Array institution { get; set; }

        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }

    }
}
