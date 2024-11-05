using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Preadmission_School_Registration")]

    public class StudentDetailsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PASR_Id { get; set; }
        public string PASR_FirstName { get; set; }
        public string PASR_MiddleName { get; set; }
        public string PASR_LastName { get; set; }
        public string PASR_RegistrationNo { get; set; }
        public string PASR_Sex { get; set; }
        public long PASR_MobileNo { get; set; }
        public string PASR_emailId { get; set; }

        public int PASR_Age { get; set; }
        public string Remark { get; set; }
        public long Caste_id { get; set; }

        public string PASR_Medium { get; set; }

        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }

        public long ASMCL_Id { get; set; }

        public long? CasteCategory_Id { get; set; }
        public string PASR_ConDistrict { get; set; }

    }
}
