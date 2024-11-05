using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_AC_SProjects_133_Comments")]
    public class NAAC_AC_SProjects_133_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACSPR133C_Id { get; set; }
        public long NCACSPR133_Id { get; set; }
        public string NCACSPR133C_Remarks { get; set; }
        public long NCACSPR133C_RemarksBy { get; set; }
        public string NCACSPR133C_StatusFlg { get; set; }
        public long? NCACSPR133C_CreatedBy { get; set; }
        public long? NCACSPR133C_UpdatedBy { get; set; }
        public DateTime? NCACSPR133C_CreatedDate { get; set; }
        public DateTime? NCACSPR133_UpdatedDate { get; set; }
        public bool? NCACSPR133C_ActiveFlag { get; set; }
        
    }
}
