using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_653_IQAC")]
    public class NAAC_AC_653_IQAC_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC653IQAC_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC653IQAC_Year { get; set; }
        public string NCAC653IQAC_QualityName { get; set; }
        public DateTime NCAC653IQAC_Date { get; set; }
        public long NCAC653IQAC_Duration { get; set; }
        public long NCAC653IQAC_NoOfParticipants { get; set; }
        public bool NCAC653IQAC_ActiveFlg { get; set; }
        public long NCAC653IQAC_CreatedBy { get; set; }
        public long NCAC653IQAC_UpdatedBy { get; set; }
        public DateTime NCAC653IQAC_CreatedDate { get; set; }
        public DateTime NCAC653IQAC_UpdatedDate { get; set; }
        public bool NCAC653IQAC_RegIQACFlg { get; set; }
        public bool NCAC653IQAC_FeedbackClgImprts { get; set; }
        public bool NCAC653IQAC_PrepOfDocAccBodiesFlg { get; set; }
        public string NCAC653IQAC_Venue { get; set; }
        public long NCAC653IQAC_NoOfTeacher { get; set; }
        public string NCAC653IQAC_StatusFlg { get; set; }
        public bool? NCAC653IQAC_ApprovedFlg { get; set; }
        public string NCAC653IQAC_Remarks { get; set; }


        public List<NAAC_AC_653_IQAC_files_DMO> NAAC_AC_653_IQAC_files_DMO { get; set; }
    }
}
