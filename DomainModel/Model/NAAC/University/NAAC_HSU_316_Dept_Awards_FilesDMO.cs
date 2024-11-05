using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.University
{
    [Table("NAAC_HSU_316_Dept_Awards_Files")]
    public class NAAC_HSU_316_Dept_Awards_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NMC316DAF_Id { get; set; }
        public long NMC316DA_Id { get; set; }
        public string NMC316DAF_FileName { get; set; }
        public string NMC316DAF_FilePath { get; set; }
        public string NMC316DAF_FileDescription { get; set; }
        public DateTime? NMC316DAF__CreatedDate { get; set; }
        public DateTime? NMC316DAF__UpdatedDate { get; set; }
    }
}
