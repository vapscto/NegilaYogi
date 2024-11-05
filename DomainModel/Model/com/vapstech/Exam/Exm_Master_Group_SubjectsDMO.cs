using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_Master_Group_Subjects", Schema = "Exm")]
    public class Exm_Master_Group_SubjectsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]     

        public int EMGS_Id { get; set; }
        [ForeignKey("EMG_Id")]
        public int EMG_Id { get; set; }
        public long ISMS_Id { get; set; }
        public bool EMGS_ActiveFlag { get; set; }
       
    }
}
