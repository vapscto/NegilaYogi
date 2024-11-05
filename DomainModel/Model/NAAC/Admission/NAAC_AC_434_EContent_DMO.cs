using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_434_EContent")]
    public class NAAC_AC_434_EContent_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC434ECT_Id { get; set; }
        public long MI_Id { get; set; }
        public string NCAC434ECT_DevFacilityName { get; set; }
        public string NCAC434ECT_LinkName { get; set; }
        //public string NCAC434ECT_FileName { get; set; }
        //public string NCAC434ECT_FilePath { get; set; }
        public Nullable<bool> NCAC434ECT_ActiveFlg { get; set; }
        public Nullable<long> NCAC434ECT_CreatedBy { get; set; }
        public Nullable<long> NCAC434ECT_UpdatedBy { get; set; }
        public string NCAC434ECT_StatusFlg { get; set; }
        public List<NAAC_AC_434_EContent_Files_DMO> NAAC_AC_434_EContent_Files_DMO { get; set; }

    }
}
