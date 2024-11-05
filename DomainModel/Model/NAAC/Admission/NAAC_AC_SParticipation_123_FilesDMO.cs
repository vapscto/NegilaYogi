using DomainModel.Model.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_SParticipation_123_Files")]
    public class NAAC_AC_SParticipation_123_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACSP123F_Id { get; set; }      
        public long NCACSP123_Id { get; set; }
        public string NCACSP123F_FileName { get; set; }
        public string NCACSP123F_Filedesc { get; set; }
        public string NCACSP123F_FilePath { get; set; }
        public string NCACSP123F_StatusFlg { get; set; }
        public bool? NCACSP123F_ActiveFlg { get; set; }
        public bool? NCACSP123F_ApprovedFlg { get; set; }
        public string NCACSP123F_Remarks { get; set; }
        

        public List<NAAC_AC_SParticipation_123_File_Comments_DMO> NAAC_AC_SParticipation_123_File_Comments_DMO { get; set; }
    }
}
