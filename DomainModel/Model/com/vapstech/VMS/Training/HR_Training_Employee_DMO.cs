using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Training
{
    [Table("HR_Training_Employee")]
    public class HR_Training_Employee_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRTE_Id { get; set; }
        public long HRME_Id { get; set; }
        public long HRTCR_Id { get; set; }

       
    }
}
