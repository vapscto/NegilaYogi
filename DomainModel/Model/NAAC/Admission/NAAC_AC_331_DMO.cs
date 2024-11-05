using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_331")]
   public class NAAC_AC_331_DMO
    {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
public long NCAC331_Id { get; set; }
public long MI_Id { get; set; }
public string NCAC331_EthicsURL { get; set; }
public bool NCAC331_PDSFlg { get; set; }
public string NCAC331_PDMecanism { get; set; }

public bool NCAC331_ActiveFlg { get; set; }
public long NCAC331_CreatedBy { get; set; }
public long NCAC331_UpdatedBy { get; set; }
public DateTime NCAC331_CreatedDate { get; set; }
public DateTime NCAC331_UpdatedDate { get; set; }
public string NCAC331_StatusFlg { get; set; }
 public List<NAAC_AC_331_Files_DMO> NAAC_AC_331_Files_DMO { get; set; }
    }
}
