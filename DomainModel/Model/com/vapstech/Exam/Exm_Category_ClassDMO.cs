using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_Category_Class", Schema = "Exm")]
    public class Exm_Category_ClassDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]     

        public int ECAC_Id { get; set; }    
        public int EMCA_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }                                                                                                                          
        public long ASMCL_Id { get; set; }   
        public long ASMS_Id { get; set; }
        public bool ECAC_ActiveFlag { get; set; }
       
    }
}
