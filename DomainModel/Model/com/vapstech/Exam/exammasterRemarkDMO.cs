using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_ProgressCard_Remarks", Schema = "Exm")]
    public class exammasterRemarkDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]     
        public int EPCR_Id { get; set; }
        public long MI_Id { get; set; }
        public string EPCR_RemarksName { get; set; }
        public int EPCR_RemarksOrder { get; set; }
        public bool EPCR_ActiveFlag { get; set; }
    }
}
