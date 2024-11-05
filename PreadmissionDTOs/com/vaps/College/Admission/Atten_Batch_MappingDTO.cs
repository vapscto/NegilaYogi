using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class Atten_Batch_MappingDTO
    {
        public long ACAB_Id { get; set; }
        public long MI_Id { get; set; }
        public string ACAB_BatchName { get; set; }
        public int ACAB_StudentStrength { get; set; }
        public long ACABS_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long ISMS_Id { get; set; }
        public int ACABS_StudentStrength { get; set; }
        public long ACABSS_Id { get; set; }
        public long AMCST_Id { get; set; }
        public bool ACABSS_ActiveFlg { get; set; }
        public Array yearlist { get; set; }
        public Array batchlist { get; set; }
        public Array courselist { get; set; }
        public Array branchlist { get; set; }
        public Array semisterlist { get; set; }
        public Array sectionlist { get; set; }
        public Array subjectlist { get; set; }
        public Array saveddata { get; set; }
        public bool returnval { get; set; }
        public bool returnduplicatestatus { get; set; }
        public Array studentlist { get; set; }
        public string AMCST_AdmNo { get; set; }
        public string AMCST_FirstName { get; set; }
        public long ACYST_RollNo { get; set; }
        public DateTime? AMCST_DOB { get; set; }
        public string AMCST_StudentPhoto { get; set; }
        public long[] sub_data { get; set; }
    } 
}
