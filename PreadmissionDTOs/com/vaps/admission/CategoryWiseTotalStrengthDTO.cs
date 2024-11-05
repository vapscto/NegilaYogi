using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class CategoryWiseTotalStrengthDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public Array accyear { get; set; }
        public Array accclasss { get; set; }
        public Array accsec { get; set; }
        public Array acccaste { get; set; }
        public Array report { get; set; }
        public CategoryWiseTotalStrengthDTO[] TempararyArrayheadList { get; set; }
        public string columnName { get; set; }
        public string columnID { get; set; }
        public string tcallorindi { get; set; }
        public string tcperortemp { get; set; }
        public string castecategoryid { get; set; }
        public string castecategoryname { get; set; }
        public Array castecategory { get; set; }
        public CategoryWiseTotalStrengthDTO[] TempararyArrayheadListcastecategory { get; set; }
        public string casteorcategory { get; set; }
        public string categorystudent { get; set; }

    }
}
