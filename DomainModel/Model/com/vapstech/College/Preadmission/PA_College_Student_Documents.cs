using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.College.Preadmission
{
    [Table("PA_College_Student_Documents", Schema = "CLG")]
    public class PA_College_Student_Documents : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long PACSTD_Id { get; set; }
        public long PACA_Id { get; set; }
        public long AMSMD_Id { get; set; }
        public string ACSTD_Doc_Path { get; set; }
        public string ACSTD_Doc_Name { get; set; }

    }
}
