using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Exam
{
    [Table("Exm_Col_Yearly_Scheme", Schema = "CLG")]
    public class Exm_Col_Yearly_SchemeDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ECYS_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACSS_Id { get; set; }
        public long ACST_Id { get; set; }
        public bool ECYS_ActiveFlag { get; set; }
    }
}
