using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("StudentsOtherState_temp")]
    public class Naac_Temp_OTState_OTCntry_Report21_DMO
    {
        [Key]        
        public long ASMAY_Id { get; set; }
        public string studentname { get; set; }
        public long AMCST_ConState { get; set; }
        public string IVRMMS_Name { get; set; }
        public string IVRMMC_CountryName { get; set; }
        public string EnrYear { get; set; }
        public string ASMAY_Year { get; set; }       
        public long NoofOTCStudents { get; set; }

    }
}
