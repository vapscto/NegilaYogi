using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    
    [Table("Preadmission_Subjectwise_Written_Marks")]
    public class WIrttenTestSubjectWiseMarksDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PASWM_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime PASWM_Date { get; set; }
        public long ISMS_ID { get; set; }
       
    }
}
