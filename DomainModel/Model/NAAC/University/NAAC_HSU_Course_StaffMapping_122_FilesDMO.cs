using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.University
{
    [Table("NAAC_HSU_Course_StaffMapping_122_Files")]
    public  class NAAC_HSU_Course_StaffMapping_122_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCHSUSM122F_Id { get; set; }
        public long NCHSUSM122_Id { get; set; }
        public string NCHSUSM122F_FileName { get; set; }
        public string NCHSUSM122F_FilePath { get; set; }
        public string NCHSUSM122F_FileDesc { get; set; }
    }
}
