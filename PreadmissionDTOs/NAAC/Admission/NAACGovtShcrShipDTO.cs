using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
    public class NAACGovtShcrShipDTO
    {
        public long NCAC511GSCH_Id { get; set; }
        public long NCAC511GSCHF_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC511GSCH_Year { get; set; }
        public string NCAC511GSCH_SchemeName { get; set; }
        public long NCAC511GSCH_NoOfStudents { get; set; }
        public string NCAC511GSCHF_FileName { get; set; }
        public string NCAC511GSCHF_FilePath { get; set; }
        public string Remarks { get; set; }
        public string NCAC511GSCH_StatusFlg { get; set; }
        public string NCAC511GSCHF_Filedesc { get; set; }
        public bool NCAC511GSCH_ActiveFlg { get; set; }
        public long NCAC511GSCH_CreatedBy { get; set; }
        public long NCAC511GSCH_UpdatedBy { get; set; }
        public long UserId { get; set; }
        public long filefkid { get; set; }

        public string ASMAY_Year { get; set; }

        public Array allacademicyear { get; set; }
        public Array institutionlist { get; set; }
        public Array editfiles { get; set; }
        public Array commentlist { get; set; }
        public Array alldatalist { get; set; }
        public Array editlist { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }

        public NAACCriteriaFivefileDTO[] filelist { get; set; }
        public commentsdto[] commentsdto { get; set; }
        public paidemn[] paidemn { get; set; }

    }


    public class NAACCriteriaFivefileDTO
    {
        public long gridid { get; set; }
        public long cfileid { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public string status { get; set; }
    }
    public class commentsdto
    {
        public long commentid { get; set; }
        public string remarks { get; set; }
        public string username { get; set; }
        public string status { get; set; }
        public DateTime createddate { get; set; }
        public bool activeflag { get; set; }
        
    }

    public class paidemn
    {
        public long gridid { get; set; }
        public long cfileid { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
    }
}
