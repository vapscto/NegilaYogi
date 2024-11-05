using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Alumni
{
    [Table("Alumni_College_Student_Registration", Schema = "CLG")]
    public class CLGAlumniUserRegistrationDMO 
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


        public long ALCSREG_Id { get; set; }
        public long MI_Id { get; set; }
        public string ALCSREG_MembershipNo { get; set; }
        public string ALCSREG_MemberName { get; set; }
        public string ALCSREG_Photo { get; set; }
        public string ALCSREG_MembershipCategory { get; set; }
        public string ALCSREG_EmailId { get; set; }
        public long ALCSREG_MobileNo { get; set; }
        public long ALCSREG_AdmittedYear { get; set; }
        public long ALCSREG_AdmittedCourse { get; set; }
        public long ALCSREG_LeftYear { get; set; }
        public long ALCSREG_LeftCourse { get; set; }
        public long ALCSREG_AdmisstedBranch { get; set; }
        public long ALCSREG_AdmittedSemester { get; set; }
        public long ALCSREG_LeftBranch { get; set; }
        public long ALCSREG_LeftSemester { get; set; }
        public string ALCSREG_AdmissionNo { get; set; }
        public DateTime ALCSREG_Date { get; set; }
        public bool ALCSREG_ApprovedFlag { get; set; }
        public long? AMCST_Id  { get; set; }
        public bool ALCSREG_ActiveFlg { get; set; }
        public long ALCSREG_CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long ALCSREG_UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }


    }
}
