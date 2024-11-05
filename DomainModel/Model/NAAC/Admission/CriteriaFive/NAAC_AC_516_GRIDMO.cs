using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_516_GRI")]
    public class NAAC_AC_516_GRIDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
         
        public long NCAC516GRI_Id { get; set; }
        public long  MI_Id { get; set; }
        public long  NCAC516GRI_Year { get; set; }
        public string  NCAC516GRI_GRIAPP { get; set; }
        public string NCAC516GRI_GRIRED { get; set; }
        public string NCAC516GRI_AvgTime { get; set; }
        public string NCAC516GRI_StatusFlg { get; set; }
        public bool  NCAC516GRI_ActiveFlg { get; set; }
        public long  NCAC516GRI_CreatedBy { get; set; }
        public long  NCAC516GRI_UpdatedBy { get; set; }
        public DateTime  NCAC516GRI_CreatedDate { get; set; }
        public DateTime NCAC516GRI_UpdatedDate { get; set; }

        public bool NCAC516GRI_AdpOfguidelinesofRegbodiesFlg { get; set; }
        public bool  NCAC516GRI_StusgrvOnline_OR_offlineFlg { get; set; }
        public bool  NCAC516GRI_CommitteewithminutesFlg { get; set; }
        public bool  NCAC516GRI_RecordOfActionTakenFlg { get; set; }
        public List<NAAC_AC_516_GRIFilesDMO> NAAC_AC_516_GRIFilesDMO { get; set; }

    }
}
