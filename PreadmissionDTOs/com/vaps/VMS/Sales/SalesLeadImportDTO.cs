using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.Sales
{
    public class SalesLeadImportDTO : CommonParamDTO
    {
        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }
       
        public long ISMTCRASTO_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long suscnt { get; set; }
        public long failcnt { get; set; }
        public long ASMAY_Id { get; set; }
        public string ISMTPLTA_EffortInHrsnew { get; set; }
        public string message { get; set; }
        public bool returnval { get; set; }

        public long ISMTPLTA_Id { get; set; }
        public long ISMTPL_Id { get; set; }
        public string ISMTPLTA_Remarks { get; set; }
        public DateTime? ISMTPLTA_StartDate { get; set; }
        public DateTime? ISMTPLTA_EndDate { get; set; }
        public long ISMTPLTA_EffortInHrs { get; set; }
        public salesdto[] advimppln { get; set; }
      

    }
   
    public class salesdto
    {
           public string LeadName          { get; set; }
           public string ContactPerson    { get; set; }
           public string Designation      { get; set; }
           public string ContactNo        { get; set; }
           public string Email            { get; set; }
           public string Category         { get; set; }
           public string Source           { get; set; }
           public string Reference        { get; set; }
           public string Health           { get; set; }
           public string Product          { get; set; }
           public string StaffStrength    { get; set; }
           public string StudentStrength  { get; set; }
           public string Pincode { get; set; }
           public string TotalInstitutions{ get; set; }
           public string Remark           { get; set; }
           public string Address1         { get; set; }
           public string Address2         { get; set; }
           public string Address3         { get; set; }
           public string Country          { get; set; }
           public string State            { get; set; }
           public string VisitedDay       { get; set; }
           public string VisitedMonth     { get; set; }
           public string VisitedYear { get; set; }


    }


}
