using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria8
{
    [Table("NAAC_MC_8110_Immunisation_Files")]
   public class NAAC_MC_8110_Immunisation_Files_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
public long NCMC8110IMMF_Id { get; set; }
public long MI_Id { get; set; }
public long NCMC8110IMM_Id { get; set; }
public string NCMC8110IMMF_FileDesc { get; set; }
public string NCMC8110IMMF_FileName { get; set; }
public string NCMC8110IMMF_FilePath { get; set; }
public string NCMC8110IMMF_StatusFlg { get; set; }
public bool NCMC8110IMMF_ActiveFlg { get; set; }
public long NCMC8110IMMF_CreatedBy{ get; set; }
public long NCMC8110IMMF_UpdatedBy { get; set; }
public DateTime NCMC8110IMMF_CreatedDate { get; set; }
public DateTime NCMC8110IMMF_UpdatedDate { get; set; }
    }
}
