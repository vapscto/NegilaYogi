using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
   [Table("ISM_Client_Project_BOM")]
  public  class ISM_Client_Project_BOM_DMO
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMCLTPRBOM_Id { get; set; }
        public long ISMMCLTPR_Id { get; set; }
        public long ISMCLTC_Id { get; set; }
        public decimal ISMCLTPRBOM_Qty { get; set; }
        public string ISMCLTPRBOM_Remarks { get; set; }
        public bool ISMCLTPRBOM_ActiveFlag { get; set; }
        public long ISMCLTPRBOM_CreatedBy { get; set; }
        public long ISMCLTPRBOM_UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
