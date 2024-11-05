using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("Adm_M_Student_EmployeeDetails")]
    public class Adm_M_Employee_StudentDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AMSTE_Id { get; set; }
        public long HRME_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public int AMSTE_Left { get; set; }
        public decimal AMSTE_Concessionpercentage { get; set; }
        public int AMSTE_SiblingsOrder { get; set; }
    }
}
