using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_Master_Programs_112")]
    public class NAAC_AC_Master_Programs_112_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACMPR112_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public string NCACMPR112_DiplomaCertName { get; set; }
        public long NCACMPR112_IntroYear { get; set; }
        public bool NCACMPR112_DiscontinuedFlg { get; set; }
        public long NCACPMR112_DiscontinuedYear { get; set; }
        public string NCACMPR112_DiscontinuedReason { get; set; }
        public bool NCACMPR112_ActiveFlg { get; set; }
        public long NCACMPR112_CreatedBy { get; set; }
        public long NCACMPR112_UpdatedBy { get; set; }
        public DateTime NCACMPR112_CreatedDate { get; set; }
        public DateTime NCACMPR112_UpdatedDate { get; set; }
        public bool? NCACMPR112_FromExelImportFlag { get; set; }
        public bool? NCACMPR112_FreezeFlag { get; set; }
        public List<NAAC_AC_Programs_112_DMO> NAAC_AC_Programs_112_DMO { get; set; }       

    }
}
