using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_522_HrEducation_Files")]
    public class NAAC_AC_522_HrEducationFilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       
        public long NCAC522HREDF_Id { get; set; }
        public long NCAC522HRED_Id { get; set; }
      
      
        public string NCAC522HREDF_FileName { get; set; }
        public string NCAC522HREDF_Filedesc { get; set; }
        public string NCAC522HREDF_FilePath { get; set; }
        public string NCAC522HREDF_StatusFlg { get; set; }
        public bool? NCAC522HREDF_ActiveFlg { get; set; }
        
 

    }
}
