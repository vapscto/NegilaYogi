using DomainModel.Model.com.vapstech.admission;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Portals.IVRM
{
    [Table("IVRM_Gallery_Photos")]
    public class IVRM_Gallery_PhotosDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IGAP_Id { get; set; }
        public long IGA_Id { get; set; }
        public string IGAP_Photos { get; set; }
        public bool IGAP_CoverPhotoFlag { get; set; }
        public bool IGAP_ActiveFlag { get; set; }
        public IVRM_GalleryDMO IVRM_GalleryDMO { get; set; }
    }
}

