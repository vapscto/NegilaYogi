using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.Portals.IVRS
{
    [Table("IVRS_Acc_Recharge")]
    public class IVRS_Acc_RechargeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IACRE_Id { get; set; }
        public string IACRE_VirtualNo { get; set; }
        public long MI_Id { get; set; }
        public string IACRE_Year { get; set; }
        public string IACRE_Month { get; set; }
        public long IACRE_RechargeAmt { get; set; }
        public string IACRE_PaymentMode { get; set; }
        public string IACRE_ReferneceNo { get; set; }
        public long IACRE_NoOfCalls { get; set; }
        public bool IACRE_ActiveFlg { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
