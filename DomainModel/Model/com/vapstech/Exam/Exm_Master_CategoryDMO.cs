using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_Master_Category",Schema = "Exm")]
    public class Exm_Master_CategoryDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]     

        public int EMCA_Id { get; set; }
        public long MI_Id { get; set; }
        public string EMCA_CategoryName { get; set; }
        public bool? EMCA_CCECheckingFlg { get; set; }
        public bool EMCA_ActiveFlag { get; set; }
       
    }
}
