using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model
{
    [Table("Preadmission_School_Registration_AdmNoticeStudents")]
   public class PreadmissionSchoolRegistrationAdmNoticeStudentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PASRANS_ID { get; set; }
        [ForeignKey("PASRAN_ID")]
        public long PASRAN_ID { get; set; }
        public long PASR_ID { get; set; }
        public DateTime? PASRANS_ADMDATE { get; set; }
        public DateTime? Createddate { get; set; }
        public DateTime? Updateddate { get; set; }
        public string PASRANS_REMARKS { get; set; }
        public string PASRANS_TIME { get; set; }
        
    }
}
