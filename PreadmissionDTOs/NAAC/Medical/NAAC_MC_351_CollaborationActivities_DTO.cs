using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Medical
{
   public class NAAC_MC_351_CollaborationActivities_DTO
    {
        public long NCMC351CA_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public long NCMC351CA_Year { get; set; }
        public string NCMC351CA_AgencyName { get; set; }
        public string NCMC351CA_ActivityTitle { get; set; }
        public string NCMC351CA_AgencyContactDetails { get; set; }
        public string NCMC351CA_ParticipantsNames { get; set; }
        public string NCMC351CA_SourceOfFinacialSupport { get; set; }
        public string NCMC351CA_Duration { get; set; }
        public string NCMC351CA_NatureOfActivity { get; set; }
        public string NCMC351CA_LinkDocument { get; set; }
        public bool NCMC351CA_ActiveFlag { get; set; }
        public long NCMC351CA_CreatedBy { get; set; }
        public long NCMC351CA_UpdatedBy { get; set; }
        public DateTime? NCMC351CA_CreatedDate { get; set; }
        public DateTime? NCMC351CA_UpdatedDate { get; set; }

        public string ASMAY_Year { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }       
        public bool returnval { get; set; }
        public bool duplicate { get; set; }
        public long NCMC351CAF_Id { get; set; }
        public string msg { get; set; }
        public Array institutionlist { get; set; }
        public Array yearlist { get; set; }     
        public Array viewuploadflies { get; set; }
        public Array editFileslist { get; set; }
        public Array alldata { get; set; }
        public Array mappedstudentlist { get; set; }
        public Array editlist { get; set; }
        public Array reportlist { get; set; }

        public Naac_CommonFiles_DTO[] filelist { get; set; }

    }
}
