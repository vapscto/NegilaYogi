using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model
{
    [Table("Preadmission_School_Registration_AdmissionNotice")]
   public class PreadmissionSchoolRegistrationAdmissionNoticeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PASRAN_ID { get; set; }
        public long MI_ID { get; set; }
        public string PASRAN_NAME { get; set; }
        public string PASRAN_PAYTIME { get; set; }
        public decimal PASRAN_FEEAMOUNT { get; set; }
        public DateTime? PASRAN_PAYDATE { get; set; }
        public DateTime? Createddate { get; set; }
        public DateTime? Updateddate { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }

        public List<PreadmissionSchoolRegistrationAdmNoticeStudentsDMO> PreadmissionSchoolRegistrationAdmNoticeStudentsDMO { get; set; }
        List<PreadmissionSchoolRegistrationAdmNoticeStudentsDMO> list = new List<PreadmissionSchoolRegistrationAdmNoticeStudentsDMO>();
    }
}
