using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_344_ExtnActivities")]
    public class NAAC_AC_344_ExtnActivities_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACET343_Id { get; set; }
        public long MI_Id { get; set; }
        public string NCACET343_TypeOfActivity { get; set; }
        public string NCACET343_SchemeName { get; set; }
        public DateTime NCACET343_ActivityDate { get; set; }
        public string NCACET343_OrgAgency { get; set; }
        public string NCACET343_Place { get; set; }
        public string NCACET343_Duration { get; set; }
        public long NCACET343_Year { get; set; }
        public long NCACET343_NoOfStudents { get; set; }
        public bool NCACET343_ActiveFlg { get; set; }
        public long NCACET343_CreatedBy { get; set; }
        public long NCACET343_UpdatedBy { get; set; }
        public DateTime NCACET343_CreatedDate { get; set; }
        public DateTime NCACET343_UpdatedDate { get; set; }
        public long NCACET343_NoOEmployee { get; set; }
        public string NCACET343_StatusFlg { get; set; }
        public List<NAAC_AC_344_ExtnActivities_Files_DMO> NAAC_AC_344_ExtnActivities_Files_DMO { get; set; }
        public List<NAAC_AC_344_ExtnActivities_Students_DMO> NAAC_AC_344_ExtnActivities_Students_DMO { get; set; }
        public List<NAAC_AC_344_ExtnActivities_Staff_DMO> NAAC_AC_344_ExtnActivities_Staff_DMO { get; set; }
    }
}
