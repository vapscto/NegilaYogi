using DomainModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
{
    [Table("HR_MRF_List")]
    public class HR_MRF_ListDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMRFL_Id { get; set; }
        public long HRMP_Id { get; set; }
        public long HRMPT_Id { get; set; }
        public int HRMRFL_No { get; set; }
        public long HRCD_Id { get; set; }
        public string HRMRFL_Status { get; set; }
        public long IVRMMC_Id { get; set; }
        public long IVRMMS_Id { get; set; }
        public long HRMRFR_Id { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
    }

}