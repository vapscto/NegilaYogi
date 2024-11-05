using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.HRMS
{
    [Table("HR_Employee_Committee")]
    public class HR_Employee_CommitteeDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRECOM_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HRECOM_CommitteeName { get; set; }
        public string HRECOM_Flg { get; set; }
        public string HRECOM_Role { get; set; }
        public string HRECOM_Document { get; set; }
        public bool HRECOM_ActiveFlg { get; set; }
        public long HRECOM_CreatedBy { get; set; }
        public long HRECOM_UpdatedBy { get; set; }
        public string HRECOM_Year { get; set; }
        //public DateTime? CreatedDate { get; set; }
        //public DateTime? UpdatedDate { get; set; }
    }
}
