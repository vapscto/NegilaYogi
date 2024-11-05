using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_AC_SProjects_133_File_Comments")]
    public class NAAC_AC_SProjects_133_File_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACSPR133FC_Id { get; set; }
        public long NCACSPR133F_Id { get; set; }
        public string NCACSPR133FC_Remarks { get; set; }
        public long? NCACSPR133FC_RemarksBy { get; set; }
        public bool NCACSPR133FC_ActiveFlag { get; set; }
        public long? NCACSPR133FC_CreatedBy { get; set; }
        public long? NCACSPR133FC_UpdatedBy { get; set; }
        public DateTime? NCACSPR133FC_CreatedDate { get; set; }
        public DateTime? NCACSPR133FC_UpdatedDate { get; set; }
        public string NCACSPR133FC_StatusFlg { get; set; }
    }
}
