using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("HR_Employee_MedicalRecord_File")]
    public class HR_Employee_MedicalRecord_FileDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HREMRF_Id { get; set; }
        public long HREMR_Id { get; set; }
        public string HREMRF_FileName { get; set; }
        public string HREMRF_FilePath { get; set; }
        public bool HREMRF_ActiveFlag { get; set; }
        public DateTime? HREMRF_CreatedDate { get; set; }
        public DateTime? HREMRF_UpdatedDate { get; set; }
        public long HREMRF_CreatedBy { get; set; }
        public long HREMRF_UpdatedBy { get; set; }
    }
}


