using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_Master_Cycle")]
    public class NAAC_Master_CycleDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMACY_Id { get; set; }
        public long MI_Id { get; set; }
        public string NCMACY_NAACCycle { get; set; }
        public DateTime? NCMACY_FromDate { get; set; }
        public DateTime? NCMACY_TODate { get; set; }
        public bool NCMACY_ActiveFlg { get; set; }
        public int NCMACY_Order { get; set; }
        public long NCMACY_CreatedBy { get; set; }
        public long NCMACY_UpdatedBy { get; set; }
        public DateTime? NCMACY_CreatedDate { get; set; }
        public DateTime? NCMACY_UpdatedDate { get; set; }
    }
}
