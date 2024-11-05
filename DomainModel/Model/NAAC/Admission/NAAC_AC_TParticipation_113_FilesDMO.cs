using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_TParticipation_113_Files")]
    public class NAAC_AC_TParticipation_113_FilesDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long NCACTP113F_Id { get; set; }
        public long NCACTP113_Id { get; set; }
        public string NCACTP113F_FileName { get; set; }
        public string NCACTP113F_Filedesc { get; set; }
        public string NCACTP113F_FilePath { get; set; }
        public string NCACTP113F_StatusFlg { get; set; }
        public bool NCACTP113F_ActiveFlg { get; set; }
        public bool? NCACTP113F_ApprovedFlg { get; set; }
        public string NCACTP113F_Remarks { get; set; }

    }
}

