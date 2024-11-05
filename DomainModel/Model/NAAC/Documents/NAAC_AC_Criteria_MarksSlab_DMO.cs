using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Documents
{
    [Table("NAAC_AC_Criteria_MarksSlab")]
    public class NAAC_AC_Criteria_MarksSlab_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACCRMRSLB_Id { get; set; }
        public long MI_Id { get; set; }
        public long NAACSL_Id { get; set; }
        public decimal NCACCRMRSLB_FromSlab { get; set; }
        public decimal NCACCRMRSLB_ToSlab { get; set; }
        public decimal NCACCRMRSLB_Marks { get; set; }
        public bool NCACCRMRSLB_ActiveFlg { get; set; }
        public long NCACCRMRSLB_CreatedBy { get; set; }
        public long NCACCRMRSLB_UpdatedBy { get; set; }
        public DateTime NCACCRMRSLB_CreatedDate { get; set; }
        public DateTime NCACCRMRSLB_UpdatedDate { get; set; }
        public long MT_Id { get; set; }

    }
}
