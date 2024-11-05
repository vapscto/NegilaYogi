using DomainModel.Model.com.vapstech.admission;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Portals.IVRM
{
    [Table("IVRM_Gallery_Videos")]
    public class IVRM_Gallery_VideosDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IGAV_Id { get; set; }
        public long IGA_Id { get; set; }
        public string IGAV_Videos { get; set; }
        public bool IGAV_ActiveFlag { get; set; }
        public IVRM_GalleryDMO IVRM_GalleryDMO { get; set; }
    }
}

