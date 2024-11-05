using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModel.Model.NAAC.Medical;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_SParticipation_123")]
    public class NAAC_AC_SParticipation_123_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACSP123_Id { get; set; }
        public long NCACSP123_Year { get; set; }
        public long MI_Id { get; set; }        
        public string NCACSP123_AddOnProgramName { get; set; }
        public long NCACSP123_NoOfStudParticipated { get; set; }
        public bool NCACSP123_ActiveFlg { get; set; }
        public long NCACSP123_CreatedBy { get; set; }
        public long NCACSP123_UpdatedBy { get; set; }
        public DateTime? NCACSP123_CreatedDate { get; set; }
        public DateTime? NCACSP123_UpdatedDate { get; set; }
        public DateTime NCACSP123_Date { get; set; }
        public string NCACSP123_StatusFlg { get; set; }
        public bool? NCACSP123_ApprovedFlg { get; set; }
        public string NCACSP123_Remarks { get; set; }
        

        public List<NAAC_AC_SParticipation_123_Students_DMO> NAAC_AC_SParticipation_123_Students_DMO { get; set; }
        public List<NAAC_AC_SParticipation_123_FilesDMO> NAAC_AC_SParticipation_123_FilesDMO { get; set; }
        //public List<NAAC_AC_SParticipation_123_File_Comments_DMO> NAAC_AC_SParticipation_123_File_Comments_DMO { get; set; }
        public List<NAAC_AC_SParticipation_123_Comments_DMO> NAAC_AC_SParticipation_123_Comments_DMO { get; set; }
    }
}
