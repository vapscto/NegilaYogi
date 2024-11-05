using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{

  
    public class MasterCategoryDTO : CommonParamDTO

    {
        public long AMC_Id { get; set; }
        public long MI_Id { get; set; }
        public string AMC_Name { get; set; }
        public string MI_Name { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string Line4 { get; set; }
        public string AMC_Type { get; set; }
        public string AMC_RegNoPrefix { get; set; }
        public int AMC_ActiveFlag { get; set; }
        public string returnval { get; set; }
        public Array categoryList { get; set; }
        public Array institutionDrpdwn { get; set; }

        public string AMC_Logo { get; set; }
        public string AMC_FilePath { get; set; }

    }
}
