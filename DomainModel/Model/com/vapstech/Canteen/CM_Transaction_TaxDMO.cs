using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace DomainModel.Model.com.vapstech.Canteen
{
    [Table("CM_Transaction_Tax")]
    public class CM_Transaction_TaxDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

     public long CMTRANST_Id { get; set; }
     public long CMTRANS_Id { get; set; }
     public long INVMT_Id { get; set; }
    public decimal CMTRANST_TaxAmount { get; set; }
    public bool CMTRANST_ActiveFlg { get; set; }
    public long? CMTRANST_CreatedBy { get; set; }
    public long? CMTRANST_UpdatedBy { get; set; }
    public DateTime? CMTRANST_CreatedDate { get; set; }
    public DateTime? CMTRANST_Updateddate { get; set; }
    }
}
