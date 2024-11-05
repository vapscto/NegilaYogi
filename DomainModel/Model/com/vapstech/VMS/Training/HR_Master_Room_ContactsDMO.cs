using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Training
{
    [Table("HR_Master_Room_Contacts")]
    public class HR_Master_Room_ContactsDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMRCO_Id { get; set; }
        public long HRMR_Id { get; set; }
        public long HRME_Id { get; set; }
        public bool HRMRCO_ActiveFlag { get; set; }
        public DateTime HRMRCO_CreatedDate { get; set; }
        public DateTime HRMRCO_UpdatedDate { get; set; }
       
        public long HRMRCO_CreatedBy { get; set; }
        public long HRMRCO_UpdatedBy { get; set; }
  

    }
}
