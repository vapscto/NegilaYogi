using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace DomainModel.Model.com.vapstech.College.Exam
{
    [Table("Exm_Col_Yearly_Scheme_Group", Schema = "CLG")]
    public class Exm_Col_Yearly_Scheme_GroupDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ECYSG_Id { get; set; }
        public long ECYS_Id { get; set; }
        public int EMG_Id { get; set; }
        public bool ECYSG_ActiveFlag { get; set; }
    }
}
