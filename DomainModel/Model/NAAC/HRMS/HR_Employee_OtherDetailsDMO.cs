using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.HRMS
{
    [Table("HR_Employee_OtherDetails")]
    public class HR_Employee_OtherDetailsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HREOTHDET_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HREOTHDET_OtherDetails { get; set; }
        public string HREOTHDET_Document { get; set; }
        public bool HREOTHDET_ActiveFlg { get; set; }
        public long HREOTHDET_CreatedBy { get; set; }
        public long HREOTHDET_UpdatedBy { get; set; }
        public string HREOTHDET_Year { get; set; }
        //public DateTime? CreatedDate { get; set; }
        //public DateTime? UpdatedDate { get; set; }
    }
}
