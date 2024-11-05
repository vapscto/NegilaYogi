using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_343_StudentActivities")]
    public class NAAC_AC_343_StudentActivities_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACSA343_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCACSA343_NoOfTeachers { get; set; }
        public string NCACSA343_TypeOfActivity { get; set; }
        public DateTime? NCACSA343_ActivityDate { get; set; }
        public string NCACSA343_OrgAgency { get; set; }
        public string NCACSA343_Place { get; set; }
        public string NCACSA343_Duration { get; set; }
        public long NCACSA343_Year { get; set; }
        public long NCACSA343_NoOfStudents { get; set; }
        //public string NCACSA343_FileName { get; set; }
        //public string NCACSA343_FilePath { get; set; }
        public bool NCACSA343_ActiveFlg { get; set; }
        public long NCACSA343_CreatedBy { get; set; }
        public long NCACSA343_UpdatedBy { get; set; }
        public DateTime NCACSA343_CreatedDate { get; set; }
        public DateTime NCACSA343_UpdatedDate { get; set; }

        public List<NAAC_AC_343_StudentActivities_Files_DMO> NAAC_AC_343_StudentActivities_Files_DMO { get; set; }
        public List<NAAC_AC_343_SA_Students_DMO> NAAC_AC_343_SA_Students_DMO { get; set; }
        public List<NAAC_AC_343_SA_Employee_DMO> NAAC_AC_343_SA_Employee_DMO { get; set; }

    }
}
