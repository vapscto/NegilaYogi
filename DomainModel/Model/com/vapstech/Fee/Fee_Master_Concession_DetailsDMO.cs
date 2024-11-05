using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("Fee_Master_Concession_Details")]
   public class Fee_Master_Concession_DetailsDMO
    {
[Key]
[DatabaseGenerated(DatabaseGeneratedOption.Identity)]

public long FMCCD_Id { get; set; }
public long FMCC_Id { get; set; }
public string FMCCD_PerOrAmtFlag { get; set; }
public long FMCCD_FromNoSibblings { get; set; }
public long FMCCD_ToNoSibblings { get; set; }
public decimal FMCCD_PerOrAmt { get; set; }
public bool FMCCD_ActiveFlg { get; set; }
public DateTime CreatedDate { get; set; }
public DateTime UpdatedDate { get; set; }
    }
}
