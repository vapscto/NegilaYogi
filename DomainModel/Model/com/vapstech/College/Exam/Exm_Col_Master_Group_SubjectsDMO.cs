using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Exam
{
    [Table("Exm_Col_Master_Group_Subjects", Schema = "CLG")]
    public class Exm_Col_Master_Group_SubjectsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ECMGS_Id { get; set; }
        public int EMG_Id { get; set; }
        public long ISMS_Id { get; set; }
        public bool ECMGS_ActiveFlag { get; set; }

    }
}
