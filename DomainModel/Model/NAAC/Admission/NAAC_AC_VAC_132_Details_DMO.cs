using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_VAC_132_Details")]
  public class NAAC_AC_VAC_132_Details_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACVAC132D_Id { get; set; }
        public long NCACVAC132_Id { get; set; }
        public DateTime? NCACVAC132D_Date { get; set; }
        public long NCACVAC132D_Year { get; set; }
        public long NCACVAC132D_NoOfStudentsEnr { get; set; }
        public long NCACVAC132D_NoOfStdCompleted { get; set; }
       
        public bool NCACVAC132D_ActiveFlg { get; set; }
        public long NCACVAC132D_CreatedBy { get; set; }
        public long NCACVAC132D_UpdatedBy { get; set; }
        public DateTime? NCACVAC132D_CreatedDate { get; set; }
        public DateTime? NCACVAC132D_UpdatedDate { get; set; }
        public string NCACVAC132D_StatusFlg { get; set; }

        public List<NAAC_AC_VAC_132_Details_Students_DMO> NAAC_AC_VAC_132_Details_Students_DMO { get; set; }
        public List<NAAC_AC_VAC_132_Details_FilesDMO> NAAC_AC_VAC_132_Details_FilesDMO { get; set; }

    }
}
