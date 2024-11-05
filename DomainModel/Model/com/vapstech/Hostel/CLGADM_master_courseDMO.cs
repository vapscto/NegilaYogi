using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Hostel
{
    [Table("ADM_master_course", Schema = "CLG")]
    public class CLGADM_master_courseDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long AMCO_Id { get; set; }
       public long MI_Id { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMCO_CourseCode { get; set; }
        public string AMCO_CourseInfo { get; set; }
        public bool AMCO_CourseFlag { get; set; }
        public int AMCO_NoOfYears { get; set; }
        public int AMCO_NoOfSemesters { get; set; }
        public float AMCO_MinAttPer { get; set; }
        public bool AMCO_FeeAplFlg { get; set; }
        public int AMCO_Order { get; set; }
        public bool AMCO_RegFeeFlg { get; set; }
        public bool AMCO_ActiveFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long AMCO_CreatedBy { get; set; }
        public long AMCO_UpdatedBy { get; set; }
    }
}
