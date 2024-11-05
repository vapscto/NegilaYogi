using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class ClasswisestudentdetailsDTO
    {
        public long ASYST_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMC_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long UserId { get; set; }
        public long IVRMRT_Id { get; set; }
        public long HRME_Id { get; set; }
        public Array fillyear { get; set; }
        public Array fillclass { get; set; }
        public Array fillsection { get; set; }
        public ClasswisestudentdetailsDTO[] TempararyArrayheadList { get; set; }
        public string tcperortemp { get; set; }
        public string tcallorindi { get; set; }
        public Array alldatagridreport { get; set; }
        public long mid { get; set; }              
        public string allorindiflag { get; set;}
        public string columnName { get; set; }
        public string columnID { get; set; }
        public string flag { get; set; }
        public string photopathname { get; set; }

        public Array category_list { get; set; }
        public long AMC_Id { get; set; }
        public Array AMC_logo { get; set; }
        public bool categoryflag { get; set; }
        public categorylistarray1[] categorylistarray { get; set; }
        //added By Roopa
        public sectionlistarray[] sectionlistarray { get; set; }
        public classlsttwoo[] classlsttwo { get; set; }
    }
    public class classlsttwoo
    {
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
    }
    public class sectionlistarray
    {
        public long ASMS_Id { get; set; }
        public long ASMCL_Id { get; set; }
    }

}
