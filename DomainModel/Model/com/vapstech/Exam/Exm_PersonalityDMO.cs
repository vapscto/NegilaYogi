using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_Personality", Schema = "Exm")]
    public class Exm_PersonalityDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]     
        public int EP_Id { get; set; }
        public long MI_Id { get; set; }
        public string EP_PersonlaityName { get; set; }
        public string EP_PersonlaityCode { get; set; }
        public int EP_PersonlaityOrder { get; set; }
        public bool EP_ActiveFlag { get; set; }
    }
}
