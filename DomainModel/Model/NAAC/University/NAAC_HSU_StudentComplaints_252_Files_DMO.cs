using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.University
{
    [Table("NAAC_HSU_StudentComplaints_252_Files")]
    public class NAAC_HSU_StudentComplaints_252_Files_DMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCHSU252SCF_Id { get; set; }
        public long NCHSU252SC_Id { get; set; }
        public string NCHSU252SCF_FileName { get; set; }
        public string NCHSU252SCF_FilePath { get; set; }
        public string NCHSU252SCF_Filedesc { get; set; }
    }
}
