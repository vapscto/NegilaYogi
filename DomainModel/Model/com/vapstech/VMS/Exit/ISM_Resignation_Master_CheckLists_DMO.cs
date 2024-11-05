using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Exit
{
    [Table("ISM_Resignation_Master_CheckLists")]
    public class ISM_Resignation_Master_CheckLists_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMRESGMCL_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long? HRMDES_Id { get; set; }
        public string ISMRESGMCL_CheckListName { get; set; }
        public string ISMRESGMCL_Remarks { get; set; }
        public bool ISMRESGMCL_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long ISMRESGMCL_CreatedBy { get; set; }
        public long ISMRESGMCL_UpdatedBy { get; set; }
        public string ISMRESGMCL_Template { get; set; }
    }
}
