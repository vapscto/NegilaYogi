using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Library
{
   public class LIB_Master_Author_DTO:CommonParamDTO
    {
        public long LMAU_Id { get; set; }
        public long MI_Id { get; set; }
        public string LMAU_AuthorFirstName { get; set; }
        public string LMAU_AuthorMiddleName { get; set; }
        public string LMAU_AuthorLastName { get; set; }
        public long? LMAU_MobileNo { get; set; }
        public string LMAU_PhoneNo { get; set; }
        public string LMAU_EmailId { get; set; }
        public string LMAU_Address { get; set; }
        public bool LMAU_ActiveFlg { get; set; }

        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public Array authorlist { get; set; }


    }
}
