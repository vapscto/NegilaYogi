using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_Master_Cycle_Year")]
    public class NAAC_Master_Cycle_YearDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMACYYR_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCMACY_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public bool NCMACYYR_ActiveFlg { get; set; }
        public long NCMACYYR_CreatedBy { get; set; }
        public long NCMACYYR_UpdatedBy { get; set; }
        public DateTime? NCMACYYR_CreatedDate { get; set; }
        public DateTime? NCMACYYR_UpdatedDate { get; set; }
    }
}
