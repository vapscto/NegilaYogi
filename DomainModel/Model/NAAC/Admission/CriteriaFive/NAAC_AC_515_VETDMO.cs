using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_515_VET")]
    public class NAAC_AC_515_VETDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC515VET_Id { get; set; }
        public long  MI_Id { get; set; }
        public long  NCAC515VET_Year { get; set; }
        public string  NCAC515VET_VETProgramName { get; set; }
        public long  NCAC515VET_NoOfStudents { get; set; }
        public bool  NCAC515VET_ActiveFlg { get; set; }
        public long  NCAC515VET_CreatedBy { get; set; }
        public long  NCAC515VET_UpdatedBy { get; set; }
        public DateTime  NCAC515VET_CreatedDate { get; set; }
        public DateTime NCAC515VET_UpdatedDate { get; set; }
        public string  NCAC515VET_StatusFlg { get; set; }
        public List<NAAC_AC_515_VETFilesDMO> NAAC_AC_515_VETFilesDMO { get; set; }

    }
}
