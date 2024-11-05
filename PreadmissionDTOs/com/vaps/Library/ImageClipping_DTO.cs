using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Library
{
   public class ImageClipping_DTO:CommonParamDTO
    {
        public long LNPCL_Id { get; set; }
        public long MI_Id { get; set; }
        public string LNPCL_ClipName { get; set; }
        public string LNPCL_ClipImage { get; set; }
        public string LNPCL_FilePath { get; set; }
        public string LNPCL_ClipDetails { get; set; }
        public bool LNPCL_ActiveFlg { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }
        public long UserId { get; set; }

        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public Array editdetails { get; set; }
        public Array alldata { get; set; }


    }
}
