using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class MasterDocumentDTO : CommonParamDTO
    {
        public long AMSTD_Id { get; set; }
        public long AMSMD_Id { get; set; }
        public long MI_Id { get; set; }
        public string AMSMD_DocumentName { get; set; }
        public string AMSMD_Description { get; set; }

        public Array GridviewDetails { get; set; }

        public Array SelectedRowDetails { get; set; }
        public string message { get; set; }
        public string messageupdate { get; set; }
        public bool returnVal { get; set; }
        public int count { get; set; }
        public string Document_Path { get; set; }
        public long AMST_Id { get; set; }
        public bool AMSMD_FLAG { get; set; }

      



    }
}
