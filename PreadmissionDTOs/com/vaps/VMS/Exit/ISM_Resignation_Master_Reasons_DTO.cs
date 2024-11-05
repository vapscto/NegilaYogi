using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PreadmissionDTOs.com.vaps.VMS.Exit
{
    public class ISM_Resignation_Master_Reasons_DTO
    {
        public long ISMRESGMRE_Id { get; set; }
        public long MI_Id { get; set; }
        public string ISMRESGMRE_ResignationReasons { get; set; }
        public bool ISMRESGMRE_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long ISMRESGMRE_CreatedBy { get; set; }
        public long ISMRESGMRE_UpdatedBy { get; set; }
        public long userId { get; set; }
        public string returnval { get; set; }
        //==================================================
        public Array get_reason_list { get; set; }
        public Array get_details_reason_list { get; set; }
        
    }
}
