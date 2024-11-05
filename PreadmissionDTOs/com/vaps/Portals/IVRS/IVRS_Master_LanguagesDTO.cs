using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Portals.IVRS
{
    public class IVRS_Master_LanguagesDTO
    {
        public long IMLA_Id { get; set; }
        public long MI_Id { get; set; }
        public string IMLA_VirtualNo { get; set; }
        public string IMLA_SchoolURL { get; set; }
        public string IMLA_SchoolName { get; set; }
        public string IMLA_Language { get; set; }
        public int IMLA_LanguageOrder { get; set; }
        public bool IMLA_ActiveFlg { get; set; }
        public DateTime? IMLA_CreatedDate { get; set; }
        public DateTime? IMLA_UpdatedDate { get; set; }
        public string IMLA_CreatedBy { get; set; }
        public string IMLA_UpdatedBy { get; set; }
        public Array institute { get; set; }
        public Array maindata { get; set; }
        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; }
        public Array maindata_grid { get; set; }
    }
}
