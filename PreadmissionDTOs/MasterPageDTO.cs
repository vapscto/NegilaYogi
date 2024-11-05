using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class MasterPageDTO : CommonParamDTO
    {
        public long IVRMP_Id { get; set; }
        public string IVRMMP_PageName { get; set; }
        public string IVRMP_PageDesc { get; set; }
        public Array pagesdata { get; set; }
        //public Arra
        public string returnval { get; set; }
        public string IVRMP_PageURL { get; set; }
        public bool IVRMP_TemplateFlag { get; set; }
        public bool? IVRMP_MandatoryFlag { get; set; }

        public string IVRMP_PageDisplayName { get; set; }
        public long userid { get; set; }



        public long IVRMMAP_Id { get; set; }
        public string IVRMMAP_AppPageName { get; set; }

        public string IVRMMAP_Displayname { get; set; }
        public string IVRMMAP_AppPageDesc { get; set; }
        public string IVRMMAP_AppPageURL { get; set; }
        public bool IVRMMAP_ActiveFlg { get; set; }
    }
}
