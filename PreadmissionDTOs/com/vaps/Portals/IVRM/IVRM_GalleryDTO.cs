using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.IVRM
{
    public class IVRM_GalleryDTO
    {
        public classlst[] Classlst { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long UserId { get; set; }
        public bool returnval { get; set; }
        public string message { get; set; }
        public string roleflg { get; set; }
        public long AMST_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public int ASMCL_Order { get; set; }
        public long ASMS_Id { get; set; }
        public string ASMC_SectionName { get; set; }
        public string ASMC_SectionCode { get; set; }
        public int ASMC_Order { get; set; }

        public long IGA_Id { get; set; }
        public long igaId { get; set; }
        public string IGA_GalleryName { get; set; }
        public DateTime IGA_Date { get; set; }
        public long IGA_CreatedBy { get; set; }
        public long IGA_UpdatedBy { get; set; }
        public bool IGA_ActiveFlag { get; set; }
        public string IGA_Time { get; set; }
        public bool IGA_CommonGalleryFlg { get; set; }

        public long IGAP_Id { get; set; }
        public string IGAP_Photos { get; set; }
        public bool IGAP_CoverPhotoFlag { get; set; }
        public bool IGAP_ActiveFlag { get; set; }

        public long IGACL_Id { get; set; }    
        public bool IGACL_ActiveFlag { get; set; }
        public long IGACL_CreatedBy { get; set; }
        public long IGACL_UpdatedBy { get; set; }


        public long IVRMRT_Id { get; set; }
        public int month { get; set; }
        public string year { get; set; }
        public string Role_flag { get; set; }
        public string countflag { get; set; }
        public string Moduleflag { get; set; }
        public string roleflag { get; set; }
        public string mediatype { get; set; }
        public string[] images_list { get; set; }
        public Array classlist { get; set; }
        public Array sectionlist { get; set; }
        public Array roletype { get; set; }
        public Array acayear { get; set; }
        public Array Month_array { get; set; }
        public Array get_monthendreport { get; set; }
        public Array get_galleryimg { get; set; }
        public Array covermodel { get; set; }
        public Array covermodel1 { get; set; }
        public arraySectionDTO[] arraySection { get; set; }

        public Array attachementlist { get; set; }
        public string IGAV_Videos { get; set; }
        public Array editclass { get; set; }
        public Array editdata { get; set; }
        

    }
    public class classlst
    {
        public long ASMCL_Id { get; set; }
    }
    public class arraySectionDTO
    {
        public long ASMS_Id { get; set; }
        public long ASMCL_Id { get; set; }
    }
}