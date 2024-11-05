using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_Staff_User_Privileges")]
    public class StaffLoginDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMSTAUP_Id { get; set; }
        public long MI_Id { get; set; }
        public int IVRMSTAUL_Id { get; set; }
        public long IVRMIMP_Id { get; set; }
        public bool IVRMSTAUP_AddFlag { get; set; }
        public bool IVRMSTAUP_UpdateFlag { get; set; }
        public bool IVRMSTAUP_DeleteFlag { get; set; }
        public bool IVRMSTAUP_ReportFlag { get; set; }
        public bool IVRMSTAUP_ActiveFlag { get; set; }
        public bool IVRMSTAUP_ProcessFlag { get; set; }
        public long? amc_id { get; set; }
        public string User_Name { get; set; }
        public long IVRMRT_Id { get; set; }
    }
}
