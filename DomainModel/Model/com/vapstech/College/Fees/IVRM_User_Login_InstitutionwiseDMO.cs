using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("IVRM_User_Login_Institutionwise")]
    public class IVRM_User_Login_InstitutionwiseDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMULI_Id { get; set; }
        public long MI_Id { get; set; }
        public int Id { get; set; }
        public int Activeflag { get; set; }
    }
}
