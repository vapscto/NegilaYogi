using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_Master_Grade_Details", Schema = "Exm")]
    public class Exm_Master_Grade_DetailsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]     

        public int EMGD_Id { get; set; }
        [ForeignKey("EMGR_Id")]
        public int EMGR_Id { get; set; }
        public string EMGD_Name { get; set; }
        public decimal EMGD_From { get; set; }
        public decimal EMGD_To { get; set; }
        public string EMGD_Remarks { get; set; }
        public decimal? EMGD_GradePoints { get; set; }
        public bool EMGD_ActiveFlag { get; set; }
       
    }
}
