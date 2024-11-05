using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Preadmssion_Student_App_Points")]
    public class PointsDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PASAP_ID { get; set; }      
        public long PASR_Id { get; set; }
        public int PASAP_AGE { get; set; }
        public int PASAP_INCOME { get; set; }
        public int PASAP_CASTE { get; set; }
        public int PASAP_ADRESS { get; set; }
        public int PASAP_QA { get; set; }
        public int PASAP_TOTAL { get; set; }


    }
}
