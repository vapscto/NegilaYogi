using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_352_MOU")]
   public class Naac_MOU_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC352MOU_Id { get; set; }
        public long MI_Id { get; set; }
        public string NCAC352MOU_OrganisationName { get; set; }
        public string NCAC352MOU_Name { get; set; }
        public long NCAC352MOU_SigningYear { get; set; }
        public string NCAC352MOU_Duration { get; set; }
        public string NCAC352MOU_ActivitiesList { get; set; }
        public long NCAC352MOU_NoOfStudents { get; set; }
        public long NCAC352MOU_NoOfStaff { get; set; }
        public string NCAC352MOU_LinkOfDocument { get; set; }
        public bool NCAC352MOU_ActiveFlg { get; set; }
        public long NCAC352MOU_CreatedBy { get; set; }
        public long NCAC352MOU_UpdatedBy { get; set; }
        public DateTime? NCAC352MOU_CreatedDate { get; set; }
        public DateTime? NCAC352MOU_UpdatedDate { get; set; }
        public string NCAC352MOU_StatusFlg { get; set; }
        public List<NAAC_AC_352_MOU_Files_DMO> NAAC_AC_352_MOU_Files_DMO { get; set; }
    }
}
