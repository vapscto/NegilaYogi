using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Placement
{
   public class PL_Master_Company_ContactDTO
    {
        public long MI_Id { get; set; }
        public long User_Id { get; set; }

        public Array get_Company { get; set; }
        public Array get_contact { get; set; }

        public long PLMCOMPCON_Id { get; set; }
        public long PLMCOMP_Id { get; set; }
        public string PLMCOMPCON_ContactPersonName { get; set; }
        public string PLMCOMPCON_Designation { get; set; }
        public string PLMCOMPCON_EmailId { get; set; }
        public long PLMCOMPCON_ContactNo { get; set; }
        public bool PLMCOMPCON_ActiveFlag { get; set; }
        public DateTime? PLMCOMPCON_CreatedDate { get; set; }
        public DateTime? PLMCOMPCON_UpdatedDate { get; set; }
        public long PLMCOMPCON_CreatedBy { get; set; }
        public long PLMCOMPCON_UpdatedBy { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public string message { get; set; }
        public string PLMCOMP_CompanyName { get; set; }
    }
}
