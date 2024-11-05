using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.University
{
   public class HSU_346_EMPApprovedJournalList_DTO
    {

        public long NCHSU346EAJL_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCHSU346EAJL_year { get; set; }
        public string NCHSU346EAJL_EmpName { get; set; }
        public string NCHSU346EAJL_BookTitle { get; set; }
        public string NCHSU346EAJL_PaperTitle { get; set; }
        public string NCHSU346EAJL_TitleOfProcConference { get; set; }
        public string NCHSU346EAJL_NameOfConference { get; set; }
        public string NCHSU346EAJL_NationalORInternational { get; set; }
        public string NCHSU346EAJL_ISBNNo { get; set; }
        public string NCHSU346EAJL_AffiliatingInsttimeOfPublication { get; set; }
        public string NCHSU346EAJL_PublisherName { get; set; }
        public bool NCHSU346EAJL_ActiveFlag { get; set; }
        public long NCHSU346EAJL_CreatedBy { get; set; }
        public long NCHSU346EAJL_UpdatedBy { get; set; }
        public DateTime NCHSU346EAJL_CreatedDate { get; set; }
        public DateTime NCHSU346EAJL_UpdatedDate { get; set; }
        public decimal NCHSU346EAJL_NoOfCitationsExcludeCitations { get; set; }
        public decimal NCHSU346EAJL_ISTHIndex { get; set; }
        public string NCHSU346EAJL_InstAffMenPub { get; set; }
        public long NCHSU346EAJL_NoOfCitationsScopus { get; set; }
        public long NCHSU346EAJL_NoOfCitationsWebOfScience { get; set; }
        public string NCHSU346EAJL_JournalTitle { get; set; }
        public long NCHSU346EAJLF_Id { get; set; }




        public Array list { get; set; }
        public Array allacademicyear { get; set; }
        public Array alldata1 { get; set; }
        public string ASMAY_Year { get; set; }
        public string msg { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public bool ret { get; set; }
        public Array editlist { get; set; }
        public HSU_346_EMPApprovedJournalList_DTO[] filelist { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public Array editFileslist { get; set; }
        public Array viewuploadflies { get; set; }
        public Array institutionlist { get; set; }


    }
}
