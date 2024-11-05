using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_7115_ProfessionalEthics")]
    public class NAAC_AC_7115_ProfessionalEthicsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC7115PROETH_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC7115PROETH_Year { get; set; }
        public string NCAC7115PROETH_URL { get; set; }
        public bool NCAC7115PROETH_ActiveFlg { get; set; }
        public long NCAC7115PROETH_CreatedBy { get; set; }
        public long NCAC7115PROETH_UpdatedBy { get; set; }
        public DateTime NCAC7115PROETH_CreatedDate { get; set; }
        public DateTime NCAC7115PROETH_UpdatedDate { get; set; }
        public string NCAC7115PROETH_StatusFlg { get; set; }
    }
}
