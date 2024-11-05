using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.University
{
    [Table("NAAC_HSU_324_CampusStartUps_Files")]
    public class HSU_334_CampusStartUps_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCHSU324CSF_Id { get; set; }
        public long NCHSU324CS_Id { get; set; }
        public string NCHSU324CSF_FileName { get; set; }
        public string NCHSU324CSF_Filedesc { get; set; }
        public string NCHSU324CSF_FilePath { get; set; }
    }
}
