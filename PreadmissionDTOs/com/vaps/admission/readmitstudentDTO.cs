using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class readmitstudentDTO
    {

        public int MI_Id { get; set; }
        public long ARS_Id { get; set; }
        public long AMST_Id { get; set; }
        public long AMCL_ID_OLD { get; set; }
        public long AMAY_ID_OLD { get; set; }
        public long AMCL_Id_New { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? userid { get; set; }
        public SchoolYearWiseStudentDTO[] resultData { get; set; }

        public long AMAY_Id_New { get; set; }
        public bool returnval { get; set; }
        public long AMST_Id_New { get; set; }


    }
}
