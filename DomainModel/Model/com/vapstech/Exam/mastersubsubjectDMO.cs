using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_Master_SubSubject", Schema = "Exm") ]
    public class mastersubsubjectDMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int EMSS_Id { get; set; }
        public long MI_Id { get; set; }
        public string EMSS_SubSubjectName { get; set; }
        public string EMSS_SubSubjectCode { get; set; }
        public int EMSS_Order { get; set; }
        public bool EMSS_ActiveFlag { get; set; }

    }
}
