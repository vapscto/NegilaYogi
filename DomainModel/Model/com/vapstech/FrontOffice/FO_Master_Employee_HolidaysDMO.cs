using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.FrontOffice
{
    [Table("FO_Master_Employee_Holidays", Schema = "FO")]
    public class FO_Master_Employee_HolidaysDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FOMEH_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime? FOMEH_Date { get; set; }
        public string FOMEH_Day { get; set; }
        public bool FOMEH_ActiveFlg { get; set; }
        public long FOMEH_CreatedBy { get; set; }
        public long FOMEH_UpdatedBy { get; set; }
    }
}
