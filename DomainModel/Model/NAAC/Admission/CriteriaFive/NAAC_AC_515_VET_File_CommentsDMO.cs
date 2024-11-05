using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_515_VET_File_Comments")]
    public class NAAC_AC_515_VET_File_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      
       
        public long NCAC515VETFC_Id { get; set; }
        public string NCAC515VETFC_Remarks { get; set; }
        public long NCAC515VETFC_RemarksBy { get; set; }
        public bool NCAC515VETFC_ActiveFlag { get; set; }
        public long NCAC515VETFC_CreatedBy { get; set; }
        public DateTime NCAC515VETFC_CreatedDate { get; set; }
        public long NCAC515VETFC_UpdatedBy { get; set; }
        public DateTime NCAC515VETFC_UpdatedDate { get; set; }
        public string NCAC515VETFC_StatusFlg { get; set; }
        public long NCAC515VETF_Id { get; set; }

    }
}
