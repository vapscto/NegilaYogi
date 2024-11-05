using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_M_Personality", Schema = "Exm")]
    public class exammasterpersonalityDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]     
        public long Per_Id { get; set; }
        public long MI_Id { get; set; }
        public string Per_Name { get; set; }
        public string Per_Code { get; set; }
        public int Per_Order { get; set; }
        public bool Per_ActiveFlag { get; set; }
    }
}
