using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.admission
{
    [Table("Adm_Master_Student_Achivements")]
    public class StudentAchivementDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AMSTEC_Id { get; set; }
        public long MI_Id { get; set; }
        [ForeignKey("AMST_Id")]
        public long AMST_Id { get; set; }
        public string AMSTEC_Extracurricular { get; set; }
        public long? AMSTEC_CreatedBy { get; set; }
        public long? AMSTEC_UpdatedBy { get; set; }
    }
}