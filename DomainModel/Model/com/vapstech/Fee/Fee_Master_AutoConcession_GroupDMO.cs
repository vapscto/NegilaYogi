using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("Fee_Master_AutoConcession_Group")]
   public class Fee_Master_AutoConcession_GroupDMO
    {

[Key]
[DatabaseGenerated(DatabaseGeneratedOption.Identity)]


      public long FMACCG_Id { get; set; }
public long FMCC_Id { get; set; }
public long FMG_Id { get; set; }
public long FMH_Id { get; set; }
public bool FMACCG_ActiveFlg { get; set; }
public DateTime CreatedDate { get; set; }
public DateTime UpdatedDate { get; set; }




    }
}
