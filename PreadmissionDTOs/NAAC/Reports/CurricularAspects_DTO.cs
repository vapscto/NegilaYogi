using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Reports
{
    public class CurricularAspects_DTO : CommonParamDTO
    {

        public long ASMAY_Id { get; set; }
        public long NCAC533SPCAA_Id { get; set; }
        public long NCAC521PLA_Id { get; set; }
        public long MI_Id { get; set; }
        public Array yearlist { get; set; }
        public Array reportlist { get; set; }
        public Array govtsclist { get; set; }
        public Array govtsclistfiles { get; set; }
        public Array instsclist { get; set; }
        public Array instsclistfiles { get; set; }
        public long NCACPR112_Id { get; set; }
        public long UserId { get; set; }
       
        public Array reportlist2 { get; set; }
        public CurricularAspects_DTO[] selectedYear { get; set; }
        public CurricularAspects_DTO[] yerlistdata { get; set; }
        public long NCAC513INSCH_Id { get; set; }
        public string NCAC513INSCH_DevSchemeName { get; set; }
        public long noofstd { get; set; }
        public string NCAC513INSCH_AgencyDetails { get; set; }
        public string ASMAY_Year { get; set; }
        public string institutionname { get; set; }
        public string NCAC516GRI_GRIAPP { get; set; }
        public string NCAC516GRI_GRIRED { get; set; }
        public string NCAC516GRI_AvgTime { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMCO_CourseName { get; set; }
        public string NCAC521PLA_EmployerName { get; set; }
        public string amount { get; set; }
        public string AMCST_FirstName { get; set; }
        public long NCAC522HRED_Id { get; set; }
        public string atudentname { get; set; }
        public string department { get; set; }
        public string program { get; set; }
        public string awardname { get; set; }
        public string NCAC531SPCAS_NatOrInterNatFlg { get; set; }
        public string AMCST_RegistrationNo { get; set; }
        public string NCAC531SPCAS_SportsCAIEEEFlg { get; set; }
        public long? AMCST_AadharNo { get; set; }
        public long NCAC531SPCA_Id { get; set; }
        public long NCAC531SPCAS_Id { get; set; }
        public int ASMAY_Order { get; set; }
        public long NCACVAC132_Id { get; set; }
        public long NCACVAC132DF_Id { get; set; }
        public long NCACVAC132D_Id { get; set; }
        public string NCACVAC132DF_FileName { get; set; }
        public string NCACVAC132DF_FilePath { get; set; }
        public string NCACVAC132DF_Filedesc { get; set; }

        public string naactype { get; set; }
        public long MO_Id { get; set; }

        public Array getinstitutioncycle { get; set; }
        public long cycleid { get; set; }
        public string cyclename { get; set; }
        public int cycleorder { get; set; }
        public long NAACSL_Id { get; set; }

        public Array getparentidzero { get; set; }
        public Array getalldata { get; set; }
        public Array getinstitution { get; set; }

        public CurricularAspects_DTO[] selected_Inst{get;set;}
        public string NAACSL_InstitutionTypeFlg { get; set; }

        public long NCMCMPR112_Id { get; set; }
        public long NCMCMPR112DF_Id { get; set; }
        public string NCMCMPR112DF_FileDesc { get; set; }
        public string NCMCMPR112DF_FileName { get; set; }
        public string NCMCMPR112DF_FilePath { get; set; }
    }
}
