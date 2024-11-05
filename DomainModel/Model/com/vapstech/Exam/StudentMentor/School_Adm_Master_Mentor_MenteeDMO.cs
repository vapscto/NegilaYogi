using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Exam.StudentMentor
{
    [Table("Adm_Master_Mentor_Mentee")]
    public class School_Adm_Master_Mentor_MenteeDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AMMEM_Id { get; set; }
        public long AMME_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public long AMMEM_CreatedBy { get; set; }
        public long AMMEM_UpdatedBy { get; set; }
    }
}
