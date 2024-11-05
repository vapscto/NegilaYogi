using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Library
{
   public class LIB_Master_Accessories_DTO:CommonParamDTO
    {

        public long LMAC_Id { get; set; }
        public long MI_Id { get; set; }
        public string LMAC_AccessoriesName { get; set; }
        public string LMAC_AccessoriesDesc { get; set; }
        public bool LMAC_ActiveFlg { get; set; }

        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public Array accessoriselist { get; set; }
        public Array alldata { get; set; }


    }
}
