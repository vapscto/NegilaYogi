using DomainModel.Model.com.vapstech.admission;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Portals.IVRM
{
    [Table("IVRM_Gallery_Class")]
    public class IVRM_Gallery_ClassDMO : CommonParamDMO
    {
        [Key]
        public long IGACL_Id { get; set; }
        public long IGA_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public bool IGACL_ActiveFlag { get; set; }
        public long IGACL_CreatedBy { get; set; }
        public long IGACL_UpdatedBy { get; set; }

        public IVRM_GalleryDMO IVRM_GalleryDMO { get; set; }
    }
}

