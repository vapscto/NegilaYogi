using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_343_StudentActivities_Files")]
    public class NAAC_AC_343_StudentActivities_Files_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACSA343F_Id { get; set; }
        public string NCACSA343F_FileName { get; set; }
        public string NCACSA343F_Filedesc { get; set; }
        public string NCACSA343F_FilePath { get; set; }
        public long NCACSA343_Id { get; set; }

    }
}
