using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_Committee_Members_File_Comments")]
   public class NAAC_AC_Committee_Members_File_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public long NCACCOMMMFC_Id { get; set; }
        public string NCACCOMMMFC_Remarks { get; set; }
        public long? NCACCOMMMFC_RemarksBy { get; set; }
        public bool? NCACCOMMMFC_ActiveFlag { get; set; }
        public long? NCACCOMMMFC_CreatedBy { get; set; }
        public DateTime? NCACCOMMMFC_CreatedDate { get; set; }
        public long? NCACCOMMMFC_UpdatedBy { get; set; }
        public DateTime? NCACCOMMMFC_UpdatedDate { get; set; }
        public string NCACCOMMMFC_StatusFlg { get; set; }
        public long NCACCOMMMF_Id { get; set; }
    }
}
