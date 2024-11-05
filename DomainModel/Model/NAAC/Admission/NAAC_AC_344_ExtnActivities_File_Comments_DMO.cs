using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_344_ExtnActivities_File_Comments")]
   public class NAAC_AC_344_ExtnActivities_File_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public long NCACET343FC_Id { get; set; }
        public string NCACET343FC_Remarks { get; set; }
        public long NCACET343FC_RemarksBy { get; set; }
        public bool NCACET343FC_ActiveFlag { get; set; }
        public long NCACET343FC_CreatedBy { get; set; }
        public DateTime NCACET343FC_CreatedDate { get; set; }
        public long NCACET343FC_UpdatedBy { get; set; }
        public DateTime NCACET343FC_UpdatedDate { get; set; }
        public string NCACET343FC_StatusFlg { get; set; }
        public long NCACET343F_Id { get; set; }
    }
}
