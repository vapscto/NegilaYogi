using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_343_SA_Students_Files")]
    public class NAAC_AC_343_SA_Students_Files_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACSA343SF_Id { get; set; }
        public string NCACSA343SF_FileName { get; set; }
        public string NCACSA343SF_Filedesc { get; set; }
        public string NCACSA343SF_FilePath { get; set; }
        public long NCACSA343S_Id { get; set; }

    }
}
