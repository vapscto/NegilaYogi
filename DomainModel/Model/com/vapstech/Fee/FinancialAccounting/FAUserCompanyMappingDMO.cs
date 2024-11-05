using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Fee.FinancialAccounting
{
    [Table("FA_User_Company_Mapping")]
    public class FAUserCompanyMappingDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FAUCM_Id { get; set; }
        public long FAMCOMP_Id { get; set; }
        public long MI_Id { get; set; }
        public long User_Id { get; set; }
        public bool FAUCM_ActiveFlg { get; set; }
        public string FAUCM_Password { get; set; }
        public DateTime FAUCM_CreatedDate { get; set; }
        public DateTime FAUCM_UpdatedDate { get; set; }
       
        public long FAUCM_CreatedBy { get; set; }
        public long FAUCM_UpdatedBY { get; set; }
    }
}
