using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_414_Budget")]
    public class NAAC_AC_414_Budget_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC414BD_Id { get; set; }
        public long MI_Id { get; set; }
        public string NCAC414BD_Budget { get; set; }
        public long NCAC414BD_AllotYear { get; set; }
        public Nullable<bool> NCAC414BD_ActiveFlg { get; set; }
        public Nullable<long> NCAC414BD_CreatedBy { get; set; }
        public Nullable<long> NCAC414BD_UpdatedBy { get; set; }       
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string NCAC414BD_BudgetAllotDev { get; set; }
        public decimal NCAC414BD_BudgetINfDev { get; set; }
        public decimal NCAC414BD_BudgetINfAugn { get; set; }
        public decimal NCAC414BD_TotalExpExcludeSal { get; set; }
        public string NCAC414BD_StatusFlg { get; set; }
        public List<NAAC_AC_414_Budget_Files_DMO> NAAC_AC_414_Budget_Files_DMO { get; set; }
    }
}