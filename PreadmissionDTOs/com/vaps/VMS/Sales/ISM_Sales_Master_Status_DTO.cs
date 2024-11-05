using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PreadmissionDTOs.com.vaps.VMS.Sales
{

    public class ISM_Sales_Master_Status_DTO
    {
        public long ISMSMST_Id { get; set; }
        public long MI_Id { get; set; }
        public string ISMSMST_StatusName { get; set; }
        public string ISMSMST_Remarks { get; set; }
        public bool ISMSMST_ActiveFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long ISMSMST_CreatedBy { get; set; }
        public long? ISMSMST_UpdatedBy { get; set; }
        public long userId { get; set; }
        public string returnvalue { get; set; }
        public bool retbool { get; set; }
        public Array status_list { get; set; }
        public Array edit_status_list { get; set; }
    }
}
