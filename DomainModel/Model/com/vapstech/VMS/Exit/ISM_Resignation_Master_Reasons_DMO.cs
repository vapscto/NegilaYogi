using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Exit
{
    [Table("ISM_Resignation_Master_Reasons")]
    public class ISM_Resignation_Master_Reasons_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMRESGMRE_Id { get; set; }
        public long MI_Id { get; set; }
        public string ISMRESGMRE_ResignationReasons { get; set; }
        public bool ISMRESGMRE_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long ISMRESGMRE_CreatedBy { get; set; }
        public long ISMRESGMRE_UpdatedBy { get; set; }
    }
}
