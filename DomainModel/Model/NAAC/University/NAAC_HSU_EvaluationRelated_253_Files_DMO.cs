using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.University
{
    [Table("NAAC_HSU_EvaluationRelated_253_Files")]
    public class NAAC_HSU_EvaluationRelated_253_Files_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCHSU253ERF_Id { get; set; }
        public long NCHSU253ER_Id { get; set; }
        public string NCHSU253ERF_FileName { get; set; }
        public string NCHSU253ERF_FilePath { get; set; }
        public string NCHSU253ERF_Filedesc { get; set; }
    }
}
