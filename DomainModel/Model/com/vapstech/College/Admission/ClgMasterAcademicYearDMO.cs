using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_Master_AcademicYear", Schema = "CLG")]
    public class ClgMasterAcademicYearDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ACMAY_Id { get; set; }
        [ForeignKey("MI_Id")]
        public long MI_Id { get; set; }
        public string ACMAY_AcademicYear { get; set; }
        public string ACMAY_AcademicYearCode { get; set; }
        public DateTime? ACMAY_AYFromDate { get; set; }
        public DateTime? ACMAY_AYToDate { get; set; }
        public int ACMAY_AYOrder { get; set; }      
        public bool ACMAB_PAActiveFlg { get; set; }
        public bool Is_Active { get; set; }


    }
}
