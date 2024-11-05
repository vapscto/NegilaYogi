using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.HRMS
{
    [Table("HR_Employee_BOSBOE")]
    public class HR_Employee_BOSBOEDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HREBOS_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HREBOS_Subject { get; set; }
        public string HREBOS_BOSBOEFlg { get; set; }
        public string HREBOS_UnvCollegeFlg { get; set; }
        public DateTime HREBOS_FromToDate { get; set; }
        public DateTime HREBOS_ToDate { get; set; }
        public string HREBOS_Document { get; set; }
        public bool HREBOS_ActiveFlg { get; set; }
        public long HREBOS_CreatedBy { get; set; }
        public long HREBOS_UpdatedBy { get; set; }
        public long HREBOS_Year { get; set; }
        public string HREBOS_Role { get; set; }
        public bool? HREBOS_ApprovedFlg { get; set; }
        public string HREBOS_Remarks { get; set; }
        public string HREBOS_StatusFlg { get; set; }
    }
}
