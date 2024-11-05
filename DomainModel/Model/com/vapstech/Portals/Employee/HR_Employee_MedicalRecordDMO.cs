using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("HR_Employee_MedicalRecord")]
    public class HR_Employee_MedicalRecordDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      
        public long HREMR_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime HREMR_TestDate { get; set; }
        public string HREMR_TestName { get; set; }
        public string HREMR_Remarks { get; set; }
        public bool HREMR_ActiveFlag { get; set; }
        public DateTime? HREMR_CreatedDate { get; set; }
        public DateTime? HREMR_UpdatedDate { get; set; }
        public long HREMR_CreatedBy { get; set; }
        public long HREMR_UpdatedBy { get; set; }
        public List<HR_Employee_MedicalRecord_FileDMO> HR_Employee_MedicalRecord_FileDMO { get; set; }
    }
}
