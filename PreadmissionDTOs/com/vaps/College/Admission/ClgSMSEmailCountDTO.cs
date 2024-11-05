using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
   public class ClgSMSEmailCountDTO
    {
        public long MI_Id { get; set; }
        public string rdbbutton { get; set; }
        public bool studChecked { get; set; }
        public bool staffChecked { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public Array sms_mail_count { get; set; }
        public int count { get; set; }
        public Array mail_count_list { get; set; }
        public string SearchColumn { get; set; }
        public string EnteredData { get; set; }
        public string message { get; set; }
        public Array Modulelist { get; set; }
        public modulenameslist[] modulenameslist { get; set; }
        public To_FLagList[] To_FLagList { get; set; }

        
    }
    public class To_FLagList
    {
        public string To_FLag { get; set; }
    }
    public class modulenameslist
    {
        public string IVRMM_ModuleName { get; set; }
    }
}
