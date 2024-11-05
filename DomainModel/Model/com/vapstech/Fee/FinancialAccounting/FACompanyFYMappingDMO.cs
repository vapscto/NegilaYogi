using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Fee.FinancialAccounting
{
    [Table("FA_Company_FY_Mapping")]
    public class FACompanyFYMappingDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public long FACFYM_Id { get; set; }

        public long MI_Id { get; set; }
        public long FAMCOMP_Id { get; set; }

        public long IMFY_Id { get; set; }

        public String FACFYM_RefNo { get; set; }
        public bool FACFYM_FinancialYearCloseFlg { get; set; }
        public bool FACFYM_ActiveFlg { get; set; }
        public decimal? FACFYM_Budget { get; set; }

        public DateTime? FACFYM_BBDate { get; set; }
        public DateTime? FACFYM_CreatedDate { get; set; }
        public DateTime? FACFYM_UpdatedDate { get; set; }

        public long FACFYM_UpdatedBY { get; set; }
        public long FACFYM_CreatedBy { get; set; }




    }
}
