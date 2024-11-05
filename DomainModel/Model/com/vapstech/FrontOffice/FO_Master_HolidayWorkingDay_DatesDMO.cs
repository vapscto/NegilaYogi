using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.FrontOffice
{
    [Table("FO_Master_HolidayWorkingDay_Dates", Schema = "FO")]
    public class FO_Master_HolidayWorkingDay_DatesDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FOMHWDD_Id { get; set; }
        public int FOHWDT_Id { get; set; }
        public long MI_Id { get; set; }
        public string FOMHWDD_Name { get; set; }
        public long HRMLY_Id { get; set; }
        public DateTime? FOMHWDD_FromDate { get; set; }
        public DateTime? FOMHWDD_ToDate { get; set; }
        public bool FOMHWD_ActiveFlg { get; set; }
        public long? HRMD_Id { get; set; }
        public long? FOMHWDD_UpdatedBy { get; set; }
        public long?  FOMHWDD_CreatedBy { get; set; }
}
}
