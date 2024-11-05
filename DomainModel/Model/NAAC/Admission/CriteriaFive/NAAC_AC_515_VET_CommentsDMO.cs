using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_515_VET_Comments")]
    public class NAAC_AC_515_VET_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      
        public long NCAC515VETC_Id { get; set; }
        public string NCAC515VETC_Remarks { get; set; }
        public long NCAC515VETC_RemarksBy { get; set; }
        public string NCAC515VETC_StatusFlg { get; set; }
        public bool NCAC515VETC_ActiveFlag { get; set; }
        public long NCAC515VETC_CreatedBy { get; set; }
        public DateTime NCAC515VETC_CreatedDate { get; set; }
        public long NCAC515VETC_UpdatedBy { get; set; }
        public DateTime NCAC515VETC_UpdatedDate { get; set; }
        public long NCAC515VET_Id { get; set; }

    }
}
