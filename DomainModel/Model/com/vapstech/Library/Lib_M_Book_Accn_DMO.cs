using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_Master_Book_AccnNo", Schema ="LIB")]
    public class Lib_M_Book_Accn_DMO : CommonParamDMO
    {
        [Key]
        public long LMBANO_Id { get; set; }
        public long LMB_Id { get; set; }
        public string LMBANO_AccessionNo { get; set; }
        public string LMBANO_AvialableStatus { get; set; }
        public DateTime? LMBANO_LostDamagedDate { get; set; }
        public string LMBANO_LostDamagedReason { get; set; }
        public bool LMBANO_ActiveFlg { get; set; }
        public long LMRA_Id { get; set; }
        public bool LMBANO_LostDamagedFlg { get; set; }
        public decimal? LMBANO_AmountCollected { get; set; }
        public string LMBANO_ModeOfPayment { get; set; }
        public long? LMBANO_CreatedBy { get; set; }
        public long? LMBANO_UpdatedBy { get; set; }

    }
}
