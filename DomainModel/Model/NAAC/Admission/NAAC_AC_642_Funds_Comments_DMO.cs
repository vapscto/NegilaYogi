using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_642_Funds_Comments")]
    public class NAAC_AC_642_Funds_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long NCAC642FUNDC_Id { get; set; }
        public string NCAC642FUNDC_Remarks { get; set; }
        public long? NCAC642FUNDC_RemarksBy { get; set; }
        public string NCAC642FUNDC_StatusFlg { get; set; }
        public bool? NCAC642FUNDC_ActiveFlag { get; set; }
        public long? NCAC642FUNDC_CreatedBy { get; set; }
        public DateTime? NCAC642FUNDC_CreatedDate { get; set; }
        public long? NCAC642FUNDC_UpdatedBy { get; set; }
        public DateTime? NCAC642FUNDC_UpdatedDate { get; set; }
        public long NCAC642FUND_Id { get; set; }
    }
}
