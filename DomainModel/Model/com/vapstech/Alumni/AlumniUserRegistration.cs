using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Alumni
{
    [Table("Alumni_Student_Registration", Schema = "ALU")]
    public class AlumniUserRegistrationDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        //public long ALUR_ID { get; set; }
        //public long MI_ID { get; set; }
        //public string ALUR_Name { get; set; }

        //public string ALUR_Email { get; set; }
        //public long ALUR_Mobile { get; set; }

        //public int Id  { get; set; }


        //public long ALUR_AdmitId { get; set; }
        //public long ALUR_LeftId { get; set; }
        //public long ALUR_LeftClass { get; set; }
        //public int ALUR_Active { get; set; }
        //public DateTime CreatedDate { get; set; }

        //public DateTime UpdatedDate  { get; set; }


        public long ALSREG_Id { get; set; }
        public long MI_Id { get; set; }
        public string ALSREG_MembershipNo { get; set; }
        public string ALSREG_MemberName { get; set; }
        public string ALSREG_Photo { get; set; }
        public long ALSREG_MembershipCategory { get; set; }
        public string ALSREG_EmailId { get; set; }
        public long ALSREG_MobileNo { get; set; }
        public long ALSREG_AdmittedYear { get; set; }
        public long ALSREG_AdmittedClass { get; set; }
        public long ALSREG_LeftYear { get; set; }
        public long ALSREG_LeftClass { get; set; }
        public string ALSREG_AdmissionNo { get; set; }
        public DateTime? ALSREG_Date { get; set; }
        public bool ALSREG_ApprovedFlag { get; set; }
        public long? ALMST_Id { get; set; }
        public bool ALSREG_ActiveFlg { get; set; }
        public long ALSREG_CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long ALSREG_UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }


    }
}
