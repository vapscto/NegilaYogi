using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Portals.IVRM
{
    public class ClgIVRMGalleryDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long AMCST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long IVRMRT_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long igaId { get; set; }
        public long IGAP_Id { get; set; }
        public long IGA_Id { get; set; }


        public string Role_flag { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMB_BranchName { get; set; }
        public string ACMS_SectionName { get; set; }
        public string AMSE_SEMName { get; set; }
        public string roleflg { get; set; }
        public string IGA_GalleryName { get; set; }
        public string IGA_Time { get; set; }
        public string mediatype { get; set; }
        public string message { get; set; }

        public int AMCO_Order { get; set; }
        public int AMB_Order { get; set; }
        public int AMSE_SEMOrder { get; set; }
        public int ACMS_Order { get; set; }

        public DateTime IGA_Date { get; set; }

        public bool IGA_CommonGalleryFlg { get; set; }
        public bool returnval { get; set; }

        public Array roletype { get; set; }
        public Array get_galleryimg { get; set; }
        public Array courselist { get; set; }
        public Array branchlist { get; set; }
        public Array covermodel { get; set; }
        public Array semesterlist { get; set; }
        public Array sectionlist { get; set; }

        public MasterSectionDTO[] arraySection { get; set; }

        public string[] images_list { get; set; }
    }

    public class MasterSectionDTO
    {
        public long ACMS_Id { get; set; }
        public string ACMS_SectionName { get; set; }
        public string ACMS_SectionCode { get; set; }
        public int ACMS_Order { get; set; }
    }
}
