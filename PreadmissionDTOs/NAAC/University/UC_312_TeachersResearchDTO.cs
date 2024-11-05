using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.University
{
    public class UC_312_TeachersResearchDTO
    {
        public long NCMCTR312_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCMCTR312F_Id { get; set; }
        public long HRME_Id { get; set; }
        public long NCMCTR312_Year { get; set; }
        public long UserId { get; set; }
        public long NCMC8110IMMF_Id { get; set; }
        public long cycleid { get; set; }
        public long HRMD_Id { get; set; }
        public long HRMDES_Id { get; set; }
        public long ASMAY_Id { get; set; }

        public string NCMCTR312_ProjectName { get; set; }
        public string NCMCTR312_Duration { get; set; }
        public string ASMAY_Year { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public string msg { get; set; }
        public string NAACSL_InstitutionTypeFlg { get; set; }
        public string empname { get; set; }
        public string HRME_EmployeeCode { get; set; }

        public int? HRME_EmployeeOrder { get; set; }

        public decimal NCMCTR312_ProjReceivingSeedMoney { get; set; }
        public decimal NCMCTR312_amountOfSeedMoneyProvided { get; set; }

        public bool NCMCTR312_ActiveFlag { get; set; }
        public bool returnval { get; set; }
        public bool duplicate { get; set; }

        public Array getinstitutioncycle { get; set; }
        public Array getinstitution { get; set; }
        public Array editlist { get; set; }
        public Array editFileslist { get; set; }
        public Array viewuploadflies { get; set; }
        public Array allacademicyear { get; set; }
        public Array alldata1 { get; set; }
        public Array institutionlist { get; set; }
        public Array reportlist { get; set; }
        public Array reportlist2 { get; set; }
        public Array yearlist { get; set; }
        public Array yearlist1 { get; set; }
        public Array govtsclist { get; set; }
        public Array govtsclistfiles { get; set; }
        public Array filldepartment { get; set; }
        public Array filldesignation { get; set; }
        public Array emplist { get; set; }

        public UC_312_TeachersResearchDTO[] selected_Inst { get; set; }
        public UC_312_TeachersResearchDTO[] filelist { get; set; }

    }
}
