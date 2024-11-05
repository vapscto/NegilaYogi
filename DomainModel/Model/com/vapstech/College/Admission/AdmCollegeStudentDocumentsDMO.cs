using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_Student_Documents",Schema ="CLG")]
    public class AdmCollegeStudentDocumentsDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACSTD_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long ACSMD_Id { get; set; }
        public string ACSTD_Doc_Path { get; set; }
        public string ACSTD_Doc_Name { get; set; }
       
    }
}
