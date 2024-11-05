using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Placement
{
    [Table("PL_Master_Course_ClassMarks_Mapping")]
    public class mappingDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PLMCLSMAP_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCO_Id { get; set; }
        public string PLMCLSMAP_ClassName { get; set; }
        public string PLMCLSMAP_ClassFlg { get; set; }
        public string PLMCLSMAP_Remarks { get; set; }
        public bool PLMCLSMAP_ActiveFlag { get; set; }
        public DateTime? PLMCLSMAP_CreatedDate { get; set; }
        public DateTime? PLMCLSMAP_UpdatedDate { get; set; }
        public  long PLMCLSMAP_CreatedBy { get; set; }
        public  long PLMCLSMAP_UpdatedBy { get; set; }
    }
}
