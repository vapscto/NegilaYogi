using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_642_Funds")]
    public class NAAC_AC_642_Funds_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC642FUND_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC642FUND_Year { get; set; }
        public string NCAC642FUND_AgencyName { get; set; }
        public decimal NCAC642FUND_Amount { get; set; }
        public string NCAC642FUND_Initiative { get; set; }
        public string NCAC642FUND_GovORNongovFlag { get; set; }
        public bool NCAC642FUND_ActiveFlg { get; set; }
        public long NCAC642FUND_CreatedBy { get; set; }
        public long NCAC642FUND_UpdatedBy { get; set; }
        public DateTime NCAC642FUND_CreatedDate { get; set; }
        public DateTime NCAC642FUND_UpdatedDate { get; set; }
        public string NCAC642FUND_StatusFlg { get; set; }
        public bool? NCAC642FUND_ApprovedFlg { get; set; }
        public string NCAC642FUND_Remarks { get; set; }
        public List<NAAC_AC_642_Funds_files_DMO> NAAC_AC_642_Funds_files_DMO { get; set; }
    }
}
