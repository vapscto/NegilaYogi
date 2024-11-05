using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Master_DepartmentCode")]
    public class HR_Master_DepartmentCode_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMDC_ID { get; set; }
        public string HRMDC_Name { get; set; }
        public long HRMDC_Order { get; set; }
    }
}
