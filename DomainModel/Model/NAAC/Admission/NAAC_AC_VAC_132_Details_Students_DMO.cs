using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_VAC_132_Details_Students")]
   public class NAAC_AC_VAC_132_Details_Students_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long NCACVAC132DS_Id { get; set; }
        public long NCACVAC132D_Id { get; set; }
        public long AMCST_Id { get; set; }
        public bool NCACVAC132DS_CompletedFlg { get; set; }
        public long NCACVAC132DS_CompletedYear { get; set; }
        public bool NCACVAC132DS_ActiveFlg { get; set; }
        public long NCACVAC132DS_CreatedBy { get; set; }
        public long NCACVAC132DS_UpdatedBy { get; set; }
        public DateTime? NCACVAC132DS_CreatedDate { get; set; }
        public DateTime? NCACVAC132DS_UpdatedDate { get; set; }
        public string NCACVAC132DS_StatusFlg { get; set; }

    }
}
