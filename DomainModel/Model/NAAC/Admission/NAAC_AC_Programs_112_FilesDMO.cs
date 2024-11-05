using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_Programs_112_Files")]
    public class NAAC_AC_Programs_112_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACPR112F_Id { get; set; }
        public long NCACPR112_Id { get; set; }
        public string NCACPR112F_FileName { get; set; }
        public string NCACPR112F_FilePath { get; set; }
        public string NCACPR112F_Filedesc { get; set; }
        public bool? NCACPR112F_ApprovedFlg { get; set; }       
        public string NCACPR112F_Remarks { get; set; }
        public string NCACPR112F_StatusFlg { get; set; }
        public bool? NCACPR112F_ActiveFlag { get; set; }
    }
}
