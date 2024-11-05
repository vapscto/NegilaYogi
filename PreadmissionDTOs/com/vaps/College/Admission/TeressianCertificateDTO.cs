using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
   public class TeressianCertificateDTO
    {
        public long AMCST_ID { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_ID { get; set; }
        public long AMSE_ID { get; set; }
        public long ACMS_ID { get; set; }
        public long ACQ_ID { get; set; }
        public Array acdlist { get; set; }
        public Array courselist { get; set; }
        public Array branchlist { get; set; }
        public Array semlist { get; set; }
        public Array seclist { get; set; }
        public Array check_list { get; set; }
        public Array studentlist { get; set; }
        public Array talukalist { get; set; }      
        public Array districtlist { get; set; }      
        public string coursename { get; set; }
        public string syslabus { get; set; }
        public long coustart { get; set; }
        public long couend { get; set; }
        public string studentname { get; set; }
        public Array getreportdata { get; set; }
        public string report_name { get; set; }
        public string param { get; set; }
        public string admissionno { get; set; }
        public DateTime? Dob { get; set; }
        public string dobw { get; set; }
        public string nationality { get; set; }
        public string fathername { get; set; }
        public string mothername { get; set; }
        public string religion { get; set; }
        public string caste { get; set; }
        public DateTime? doj { get; set; }
        public string languages { get; set; }
        public string optionals { get; set; }
        public string feedue { get; set; }
        public string AMCST_Taluk { get; set; }
        public string AMCST_District { get; set; }
        public string fatheredu { get; set; }
        public string motheredu { get; set; }
        public string gendar { get; set; }
        public string address { get; set; }
        public string mobile { get; set; }
        public string district { get; set; }
        public string taluk { get; set; }
        public string imgnames { get; set; }

    }
}
