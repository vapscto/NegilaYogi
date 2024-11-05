using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_7111_LocalCommunity")]
    public class NAAC_AC_7111_LocalCommunityDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC7111LOCCOM_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC7111LOCCOM_Year { get; set; }
        public string NCAC7111LOCCOM_NoOfAddress { get; set; }
        public long NCAC7111LOCCOM_NoOfEngage { get; set; }
        public DateTime? NCAC7111LOCCOM_Date { get; set; }
        public long NCAC7111LOCCOM_Duration { get; set; }
        public string NCAC7111LOCCOM_InitiativeName { get; set; }
        public string NCAC7111LOCCOM_IssuesAddressed { get; set; }
        public string NCAC7111LOCCOM_StatusFlg { get; set; }
        public long NCAC7111LOCCOM_NoOfParticipant { get; set; }
        public bool NCAC7111LOCCOM_ActiveFlg { get; set; }
        public long NCAC7111LOCCOM_CreatedBy { get; set; }
        public long NCAC7111LOCCOM_UpdatedBy { get; set; }
        public DateTime NCAC7111LOCCOM_CreatedDate { get; set; }
        public DateTime NCAC7111LOCCOM_UpdatedDate { get; set; }

       
    }
}
