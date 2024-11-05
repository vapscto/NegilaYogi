using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PreadmissionDTOs.com.vaps.VMS.Exit
{
   public class ISM_Resignation_Master_CheckLists_DTO
    {
        public long ISMRESGMCL_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long HRMDES_Id { get; set; }
        public string ISMRESGMCL_CheckListName { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public string ISMRESGMCL_Remarks { get; set; }
        public bool ISMRESGMCL_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long ISMRESGMCL_CreatedBy { get; set; }
        public long ISMRESGMCL_UpdatedBy { get; set; }
        public string ISMRESGMCL_Template { get; set; }
        public long userId { get; set; }
        public string returnval { get; set; }
        //================================

        public Array department_lisd_dd { get; set; }
        public Array designation_list_dd { get; set; }
        public Array check_list { get; set; }
        public Array get_details_check_list { get; set; }



    }
}
