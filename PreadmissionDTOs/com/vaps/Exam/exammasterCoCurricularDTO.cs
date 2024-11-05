using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class exammasterCoCurricularDTO
    {
        public int ECC_Id { get; set; }
        public long MI_Id { get; set; }
        public string ECC_CoCurricularName { get; set; }
        public string ECC_CoCurricularCode { get; set; }
        public long ECC_CoCurricularOrder { get; set; }
        public bool ECC_ActiveFlag { get; set; }
        public Array exammasterCoCurricularname { get; set; }
        public Array exammCoCurricularname { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public string retrunMsg { get; set; }
        public bool already_cnt { get; set; }
        public exammasterCoCurricularDTO[] examCoCurricularDTO { get; set; }


        // Student Mapping 
        public int ESCO_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public int Month_Id { get; set; }
        public bool ESCOM_ActiveFlag { get; set; }
        public string ESCOM_Remarks { get; set; }
        public string flag { get; set; }
        public int roleid { get; set; }
        public string rolename { get; set; }
        public long User_Id { get; set; }
        public string UserName { get; set; }
        public string monthname { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string ASMAY_Year { get; set; }
        public string studentname { get; set; }
        public string AMST_AdmNo { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public long AMAY_RollNo { get; set; }
        public int Monthid { get; set; }
        public long Emp_Code { get; set; }
        public int ASMCL_ClassOrder { get; set; }
        public int ASMC_Order { get; set; }
        public int EME_Id { get; set; }
        public temp_cocurr_mapping[] Temp_studentdata { get; set; }
        public Array configuration { get; set; }
        public Array classlist { get; set; }
        public Array sectionlist { get; set; }
        public Array yearlist { get; set; }
        public Array loaddata { get; set; }
        public Array personlaitylist { get; set; }
        public Array studentList { get; set; }
        public Array savedata { get; set; }
        public Array monthlist { get; set; }
        public Array examlist { get; set; }
    }

    public class temp_cocurr_mapping
    {
        public long AMST_Id { get; set; }
        public string ESCO_Remarks { get; set; }
    }
}
