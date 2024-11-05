using DomainModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
{
    [Table("HR_Master_Location")]
    public class HR_Master_LocationDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMLO_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMLO_LocationName { get; set; }
        public string HRMLO_LocationDesc { get; set; }
        public long HRMLO_CreatedBy { get; set; }
        public long HRMLO_UpdatedBy { get; set; }
        public bool HRMLO_ActiveFlg { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
    }

}