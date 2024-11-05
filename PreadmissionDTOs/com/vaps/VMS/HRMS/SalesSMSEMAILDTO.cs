using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
    public class SalesSMSEMAILDTO 
    {
        public long HRMPR_Id { get; set; }
        public string HRMP_Name { get; set; }
        public int HRMP_Order { get; set; }
        public bool HRMP_ActiveFlag { get; set; }
        public long ISES_Id { get; set; }
        public bool snd_email { get; set; }
        public bool snd_sms { get; set; }
        public string ISES_Template_Name { get; set; }
        public string ISES_MailHTMLTemplate { get; set; }
        public string ISES_SMSMessage { get; set; }

        public string ISMSMSO_SourceName { get; set; }
        public string contactname { get; set; }
        public string mobilesearch { get; set; }
        public string emailsearch { get; set; }
        public DateTime? start_Date { get; set; }
        public DateTime? end_Date { get; set; }
        public string FHEAD { get; set; }
        public string retrunMsg { get; set; }
        public Array templatelist { get; set; }
        public Array PriorityList { get; set; }
        public Array categorylist { get; set; }
        public Array sourcelist { get; set; }
        public Array statuslist { get; set; }
        public Array productlist { get; set; }
        public Array countrylist { get; set; }
        public Array statelist { get; set; }
        public Array leadlist { get; set; }
        public long MI_Id { get; set; }
        public long HRMP_CreatedBy { get; set; }
        public long HRMP_UpdatedBy { get; set; }
        public bool athflag { get; set; }
        public long userid { get; set; }
        public stateids[] stateids { get; set; }
        public string searchstring { get; set; }
        public string statussidss { get; set; }
        public string contryidss { get; set; }
        public string catIds { get; set; }
        public string soursIds { get; set; }
        public string esubject { get; set; }
        public string Footer { get; set; }
        public string statidss { get; set; }
        public string prodidss { get; set; }
        public string ISMSLE_LeadName { get; set; }
        public string ISMSLE_ContactPerson { get; set; }
        public long ISMSLE_ContactNo { get; set; }
        public string ISMSLE_EmailId { get; set; }
        public string type { get; set; }
        public string template { get; set; }
        public string msg { get; set; }
        public string smsmsg { get; set; }
        public long ISMSLE_Id { get; set; }
        public LeadNamedata[] selected { get; set; }
        public NAACCriteriaFivefileDTO[] filelist { get; set; }

    }

    public class stateids
    {
        public long ivrmmC_Id { get; set; }
    }
    public class LeadNamedata
    {
        public string ISMSLE_LeadName { get; set; }
        public string ISMSLE_ContactPerson { get; set; }
        public long ISMSLE_ContactNo { get; set; }
        public string ISMSLE_EmailId { get; set; }
        public long ISMSLE_Id { get; set; }
    }

}