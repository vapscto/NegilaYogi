using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Library
{
    public class AddELibraryLinksDTO : CommonParamDTO
    {
        
        public long MI_Id { get; set; }
        public long LELS_Id { get; set; }
        public string LELS_Name { get; set; }
        public string LELS_BookType { get; set; }
        public string LELS_Genre { get; set; }
        public string LELS_PriceRange { get; set; }
        public string LELS_FilePath { get; set; }
        public string LELS_Url { get; set; }
        public bool LELS_ActiveFlag { get; set; }
        public bool returnval { get; set; }
        public bool duplicate { get; set; }
        public Array linklist { get; set; }
        public Array editlist { get; set; }

    }
}
