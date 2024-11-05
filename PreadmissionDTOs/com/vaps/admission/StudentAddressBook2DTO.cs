using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class StudentAddressBook2DTO
    {
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public long ASMC_Id { get; set; }
        public int AMST_Id { get; set; }
        public string AMST_AdmNo { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_Sex { get; set; }
        public DateTime? AMST_DOB { get; set; }
        public string AMST_BloodGroup { get; set; }
        public string AMST_emailId { get; set; }
        public string AMST_FatherName { get; set; }
        public string AMST_PerStreet { get; set; }
        public string AMST_PerArea { get; set; }
        public string AMST_PerCity { get; set; }
        public string AMST_PerAdd3 { get; set; }
        public long AMST_PerState { get; set; }
        public long AMST_PerCountry { get; set; }
        public int AMST_PerPincode { get; set; }
        public string AMST_ConStreet { get; set; }
        public string AMST_ConArea { get; set; }
        public string AMST_ConCity { get; set; }
        public string IVRMMS_Name { get; set; }
        public string IVRMMC_CountryName { get; set; }
        public int AMST_ConPincode { get; set; }
        public string flag { get; set; }
        public string flag1 { get; set; }
        public string AMST_SOL { get; set; }
        public string all { get; set; }
        public string sall { get; set; }
        public string asmcL_ClassName { get; set; }
        public long AMST_MobileNo { get; set; }
        public DateTime? ASTC_TCIssueDate { get; set; }

        public Array getdetails;
        public Array accyear;
        public Array classlist;
        public Array sectionlist;
        public Array studentlist { get; set; }
        public long MI_id { get; set; }
        public Array schooldetails { get; set; }
        public Array classteacher { get; set; }
        public string empname { get; set; }
        public studentaddressbokktemp[] studentlisttemp { get; set; }
    }
    public class studentaddressbokktemp
    {
        public long AMST_Id { get; set; }
        public string studentname { get; set; }
    }
}
