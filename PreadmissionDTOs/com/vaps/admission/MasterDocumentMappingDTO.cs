using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class MasterDocumentMappingDTO
    {
        public long PASCD_Id { get; set; }
        public long MI_Id { get; set; }
        public long PASMD_Id { get; set; }
        public long AMC_Id { get; set; }

        public Array GridviewDetails { get; set; }

        public Array SelectedRowDetails { get; set; }

        public Array DocumentList { get; set; }

        public Array CategoryList { get; set; }


        public List<MasterDocumentDTO> SelDocumentList { get; set; }

        public List<MasterCategoryDTO> SelCategoryList { get; set; }

        public string CategoryName { get; set; }

        public string DocumentName { get; set; }

    }
}
