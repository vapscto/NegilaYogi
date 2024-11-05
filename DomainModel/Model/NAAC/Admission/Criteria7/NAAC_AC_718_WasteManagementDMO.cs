using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_718_WasteManagement")]
    public class NAAC_AC_718_WasteManagementDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC718WAMAN_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC718WAMAN_Year { get; set; }
        public decimal NCAC718WAMAN_Expenditure { get; set; }
        public bool NCAC718WAMAN_ActiveFlg { get; set; }
        public long NCAC718WAMAN_CreatedBy { get; set; }
        public long NCAC718WAMAN_UpdatedBy { get; set; }
        public DateTime? NCAC718WAMAN_CreatedDate { get; set; }
        public DateTime? NCAC718WAMAN_UpdatedDate { get; set; }
        public string NCAC718WAMAN_StatusFlg { get; set; }
    }
}
