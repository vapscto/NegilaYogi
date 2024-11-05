using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_351_Linkage")]
  public  class NAAC_AC_351_Linkage_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

      public long NCAC351LIN_Id { get; set; }
       public long MI_Id { get; set; }
       public string NCAC351LIN_LinkageTitle { get; set; }
       public string NCAC351LIN_PartnerName { get; set; }
       public long NCAC351LIN_CommYear { get; set; }
       public DateTime NCAC351LIN_From { get; set; }
       public DateTime NCAC351LIN_To { get; set; }
       public string NCAC351LIN_LinkageNature { get; set; }
       public string NCAC351LIN_LinkOfDocument { get; set; }
       public bool NCAC351LIN_ActiveFlg { get; set; }
       public long NCAC351LIN_CreatedBy { get; set; }
       public long NCAC351LIN_UpdatedBy { get; set; }
       public DateTime NCAC351LIN_CreatedDate { get; set; }
       public DateTime NCAC351LIN_UpdatedDate { get; set; }
       public string NCAC351LIN_StatusFlg { get; set; }
    public List<NAAC_AC_351_Linkage_Files_DMO> NAAC_AC_351_Linkage_Files_DMO { get; set; }


    }
}
