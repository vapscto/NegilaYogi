using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_413_ICT")]
    public class NAAC_AC_413_ICT_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC413ICT_Id { get; set; }
        public long MI_Id { get; set; }
        public string NCAC413ICT_RoomNo { get; set; }
        public string NCAC413ICT_ICTFacility { get; set; }
        //public string NCAC413ICT_FileName { get; set; }
        //public string NCAC413ICT_FilePath { get; set; }
        public Nullable<bool> NCAC413ICT_ActiveFlg { get; set; }
        public Nullable<long> NCAC413ICT_CreatedBy { get; set; }
        public Nullable<long> NCAC413ICT_UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string NCAC413ICT_StatusFlg { get; set; }

        public List<NAAC_AC_413_ICT_FilesDMO> NAAC_AC_413_ICT_FilesDMO { get; set; }
    }
}
