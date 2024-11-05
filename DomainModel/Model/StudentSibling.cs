using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Preadmission_School_Registration_SiblingsDetails")]
    public class StudentSibling : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PASRS_Id { get; set; }
        public long PASR_Id { get; set; }
        public string PASRS_SiblingsName { get; set; }
        public string PASRS_SiblingsClass { get; set; }
        public string PASRS_SiblingsAdmissionNo { get; set; }
        public string PASRS_SiblingsSection { get; set; }
        public string PASRS_Status { get; set; }
        public string PASRS_SchoolName { get; set; }
        public string PASRS_Age { get; set; }
        public string PASRS_Gender { get; set; }
        public string PASRS_Institution  { get; set; }
        public string PASRS_DOB { get; set; }
    }
}
