using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class MasterHeaderDetailsDTO
    {
        public long IVRMIMP_Id { get; set; }
        public string IVRMMP_PageName { get; set; }
        public long IMP_Id { get; set; }
        public long MI_Id { get; set; }
        public long IVRMHE_Id { get; set; }
        public long IMPCM_Id { get; set; }
        public int IMP_PeriodName { get; set; }

        public int IMP_PeriodOrder { get; set; }

        public Array yeardropDown { get; set; }
        public Array headerdata { get; set; }
        public Array categorydropDown { get; set; }
        public string Half { get; set; }
        public string IVRMHE_Name { get; set; }
        public long IVRMIM_Id { get; set; }
        public string CategoryName { get; set; }

        public long AMC_Id { get; set; }

         public List<MasterCategoryDTO> SelectedCategoryDetails { get; set; }
      public pagedto[] pageids { get; set; }
        public Array parameterlist { get; set; }
        public Array GridviewDetails { get; set; }
        public Array institutionModuleList { get; set; }
        public Array institutionPageList { get; set; }
        public Array SelectedCategoryDetails123 { get; set; }
        public string IVRMM_ModuleName { get; set; }
        public string message { get; set; }
        public string messageupdate { get; set; }
        public bool returnVal { get; set; }


    }


    public class pagedto
    {

        public long ISMP_ID { get; set; }

    }
}
