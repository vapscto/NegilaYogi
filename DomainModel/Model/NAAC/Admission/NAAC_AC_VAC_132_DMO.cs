using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_VAC_132")]
    public class NAAC_AC_VAC_132_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACVAC132_Id { get; set; }
        public long MI_Id { get; set; }
        public string NCACVAC132_CourseName { get; set; }
        public string NCACVAC132_CourseCode { get; set; }
        public long NCACVAC132_IntroYear { get; set; }
        public bool NCACVAC132_DiscontinuedFlg { get; set; }
        public long NCACVAC132_DiscontinuedYear { get; set; }
        
        //public string NCACVAC132F_Filedesc { get; set; }
        public bool NCACVAC132_ActiveFlg { get; set; }
        public long NCACVAC132_CreatedBy { get; set; }
        public long NCACVAC132_UpdatedBy { get; set; }
        public DateTime? NCACVAC132_CreatedDate { get; set; }
        public DateTime? NCACVAC132_UpdatedDate { get; set; }
        public string NCACVAC132_StatusFlg { get; set; }
        public bool? NCACVAC132_FromExelImportFlag { get; set; }
        public bool? NCACVAC132_FreezeFlag { get; set; }
        public List<NAAC_AC_VAC_132_Details_DMO> NAAC_AC_VAC_132_Details_DMO { get; set; }
        public List<NAAC_AC_VAC_132_Files_DMO> NAAC_AC_VAC_132_Files_DMO { get; set; }

    }
}
