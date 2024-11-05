using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Library
{
   public class MasterAuthorDTO:CommonParamDTO
    {
        public long LMBA_Id { get; set; }
        public long MI_Id { get; set; }
        public string LMBA_AuthorFirstName { get; set; }
        public string LMBA_AuthorMiddleName { get; set; }
        public string LMBA_AuthorLastName { get; set; }
        public string Author_Address { get; set; }
        public bool LMBA_ActiveFlg { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public Array authorlist { get; set; }
    }
}
