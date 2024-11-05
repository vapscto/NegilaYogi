using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_Master_NonBook_AccnNo", Schema = "LIB")]
    public class LIB_Master_NonBook_AccnNo_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LMNBKANO_Id { get; set; }
        public long LMNBK_Id { get; set; }
        public long LMRA_Id { get; set; }
        public string LMNBKANO_AccnNo { get; set; }
        public string LMNBKANO_AvailableStatus { get; set; }
        public bool LMNBKANO_DeletedLostFlg { get; set; }
        public DateTime? LMNBKANO_DeletedLostDate { get; set; }
        public string LMNBKANO_DeletedLostReason { get; set; }
        public decimal LMNBKANO_AmountColleceted { get; set; }
        public string LMNBKANO_ModeOfPayment { get; set; }
        public bool LMNBKANO_ActiveFlg { get; set; }        

    }
}
