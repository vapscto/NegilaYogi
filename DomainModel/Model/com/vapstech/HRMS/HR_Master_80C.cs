using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Master_80C")]
    public class HR_Master_80C : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long HRMMM_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMMC_Name { get; set; }
        public string HRMMMC_Description { get; set; }

       
        public bool HRMMMC_ActiveFlag { get; set; }


    }
}
