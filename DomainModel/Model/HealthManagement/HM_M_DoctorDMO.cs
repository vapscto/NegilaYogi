using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.HealthManagement
{
    [Table("HM_M_Doctor", Schema = "HM")]
    public class HM_M_DoctorDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HMMDOC_Id { get; set; }
        public long MI_Id { get; set; }
        public string HMMDOC_DoctorName { get; set; }
        public string HMMDOC_DoctorQualification { get; set; }
        public string HMMDOC_Specialisation { get; set; }
        public string HMMDOC_Address { get; set; }
        public string HMMDOC_Phoneno { get; set; }
        public string HMMDOC_EmailId { get; set; }
        public string HMMDOC_BloodGroup { get; set; }
        public bool HMMDOC_ActiveFlg { get; set; }
        public DateTime? HMMDOC_CreatedDate { get; set; }
        public DateTime? HMMDOC_UpdatedDate { get; set; }
        public long HMMDOC_CreatedBy { get; set; }
        public long HMMDOC_UpdatedBy { get; set; }
    }
}
