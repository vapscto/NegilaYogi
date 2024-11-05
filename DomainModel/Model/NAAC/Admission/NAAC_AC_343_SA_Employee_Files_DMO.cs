using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_343_SA_Employee_Files")]
    public class NAAC_AC_343_SA_Employee_Files_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACSA343EF_Id { get; set; }
        public string NCACSA343EF_FileName { get; set; }
        public string NCACSA343EF_Filedesc { get; set; }
        public string NCACSA343EF_FilePath { get; set; }
        public long NCACSA343E_Id { get; set; }

    }
}
