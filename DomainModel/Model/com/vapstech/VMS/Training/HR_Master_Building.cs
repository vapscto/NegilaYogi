using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Training
{
    [Table("HR_Master_Building")]
    public class HR_Master_Building : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMB_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMB_BuildingName { get; set; }
        public string HRMB_Desc { get; set; }
        public bool HRMB_ActiveFlag { get; set; }
        public long  HRMB_CreatedBy { get; set; }
        public long HRMB_UpdatedBy { get; set; }

    }
}
