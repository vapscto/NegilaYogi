using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Medical
{
   public class NAAC_MC_422_Clinical_Laboratory_DTO
    {

        public long NCMC422CL_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCMC422CL_Year { get; set; }
        public long NCMC422CL_NoOfOutpatientsTreated { get; set; }
        public string NCMC422CL_OutStuPatientRatio { get; set; }
        public long NCMC422CL_NoofInPatientsTreated { get; set; }
        public string NCMC422CL_InStuPatientRatio { get; set; }
        public DateTime NCMC422CL_CreatedDate { get; set; }
        public DateTime NCMC422CL_UpdatedDate { get; set; }
        public long NCMC422CL_CreatedBy { get; set; }
        public long NCMC422CL_UpdatedBy { get; set; }
        public bool NCMC422CL_ActiveFlag { get; set; }
        public long NCMC422CLF_Id { get; set; }

        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }

        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public Array institutionlist { get; set; }
        public Array yearlist { get; set; }
        public Array viewuploadflies { get; set; }
        public Array editFileslist { get; set; }
        public Array editdata { get; set; }
        public Array alldata { get; set; }

        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }

        public Naac_CommonFiles_DTO[] filelist { get; set; }
    }
}
