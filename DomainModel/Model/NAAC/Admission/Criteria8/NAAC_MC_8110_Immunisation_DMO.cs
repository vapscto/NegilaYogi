using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria8
{
    [Table("NAAC_MC_8110_Immunisation")]
   public class NAAC_MC_8110_Immunisation_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
public long NCMC8110IMM_Id { get; set; }
public long MI_Id { get; set; }
public long NCMC8110IMM_Year { get; set; }
public long NCMC8110IMM_NoOfAdmStudents { get; set; }
public long NCMC8110IMM_NoOfImmuStudents { get; set; }
public bool NCMC8110IMM_ActiveFlg { get; set; }
public long NCMC8110IMM_CreatedBy { get; set; }
public long NCMC8110IMM_UpdatedBy { get; set; }
public string NCMC8110IMM_StatusFlg { get; set; }
public DateTime NCMC8110IMM_CreatedDate { get; set; }
public DateTime NCMC8110IMM_UpdatedDate { get; set; }
public List<NAAC_MC_8110_Immunisation_Files_DMO> NAAC_MC_8110_Immunisation_Files_DMO { get; set; }


    }
}
