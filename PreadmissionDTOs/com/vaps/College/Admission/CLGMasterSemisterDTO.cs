using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class CLGMasterSemisterDTO
    {
        public long AMSE_Id { get; set; }
        // [ForeignKey("MI_Id")]
        public long MI_Id { get; set; }
        public string AMSE_SEMName { get; set; }
        public string AMSE_SEMInfo { get; set; }
        public string AMSE_SEMCode { get; set; }
        public string AMSE_SEMTypeFlag { get; set; }
        public int AMSE_SEMOrder { get; set; }
        public int AMSE_Year { get; set; }
        public string AMSE_EvenOdd { get; set; }
        public bool AMSE_ActiveFlg { get; set; }
        public string message { get; set; }
        public bool returnval { get; set; }
        public Array semlist { get; set; }
        public Array semlistorder { get; set; }
        public Array editdetails { get; set; }
        public sem_temp[] sem_temp { get; set; }
    }
    public class sem_temp
    {
        public long AMSE_Id { get; set; }
        public string AMSE_SEMName { get; set; }
    }
}
