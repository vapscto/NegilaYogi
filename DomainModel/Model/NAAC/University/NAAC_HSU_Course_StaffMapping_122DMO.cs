using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.University
{
    [Table("NAAC_HSU_Course_StaffMapping_122")]
    public class NAAC_HSU_Course_StaffMapping_122DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCHSUSM122_Id { get; set; }
        public long ACAYC_Id { get; set; }
        public long HRME_Id { get; set; }
        public long NCHSUSM122_CreatedBy { get; set; }
        public long NCHSUSM122_UpdatedBy { get; set; }
        public bool NCHSUSM122_ActiveFlag { get; set; }
        public DateTime? NCHSUSM122_CreatedDate { get; set; }
        public DateTime? NCHSUSM122_UpdatedDate { get; set; }
        public List<NAAC_HSU_Course_StaffMapping_122_FilesDMO> NAAC_HSU_Course_StaffMapping_122_FilesDMO { get; set; }
    }
}
