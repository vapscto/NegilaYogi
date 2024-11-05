using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS
{
    [Table("HR_Master_DepartmentCode")]
    public class HR_Master_DepartmentCodeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMDC_ID { get; set; }
        public string HRMDC_Name  { get; set; }
        public int HRMDC_Order { get; set; }
        public string HRMDC_Code { get; set; }
    }
}
