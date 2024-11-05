using DomainModel.Model.com.vapstech.admission;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Portals.IVRM
{
    [Table("IVRM_Gallery")]
    public class IVRM_GalleryDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IGA_Id { get; set; }
        public long MI_Id { get; set; }
        public string IGA_GalleryName { get; set; }
        public DateTime IGA_Date { get; set; }
        public long IGA_CreatedBy { get; set; }
        public long IGA_UpdatedBy { get; set; }
        public bool IGA_ActiveFlag { get; set; }
        public string IGA_Time { get; set; }
        public bool IGA_CommonGalleryFlg { get; set; }
        public List<IVRM_Gallery_PhotosDMO> IVRM_Gallery_PhotosDMO { get; set; }
        public List<IVRM_Gallery_ClassDMO> IVRM_Gallery_ClassDMO { get; set; }


    }
}

