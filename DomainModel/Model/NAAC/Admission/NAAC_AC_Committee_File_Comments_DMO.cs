using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_Committee_File_Comments")]
  public class NAAC_AC_Committee_File_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public long NCACCOMMFC_Id { get; set; }
        public string NCACCOMMFC_Remarks { get; set; }
        public long? NCACCOMMFC_RemarksBy { get; set; }
        public bool? NCACCOMMFC_ActiveFlag { get; set; }
        public long? NCACCOMMFC_CreatedBy { get; set; }
        public DateTime? NCACCOMMFC_CreatedDate { get; set; }
        public long? NCACCOMMFC_UpdatedBy { get; set; }
        public DateTime? NCACCOMMFC_UpdatedDate { get; set; }
        public string NCACCOMMFC_StatusFlg { get; set; }
        public long NCACCOMMF_Id { get; set; }
    }
}
