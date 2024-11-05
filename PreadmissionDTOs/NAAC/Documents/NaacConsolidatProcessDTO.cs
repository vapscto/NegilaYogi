using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Documents
{
    public class NaacConsolidatProcessDTO
    {
        public long MI_Id { get; set; }
        public long userid { get; set; }
        public long MO_Id { get; set; }
        public long roleidd { get; set; }
        public long NAACSL_Id { get; set; }
        public long NCMACY_Id { get; set; }
        public string naactype { get; set; }
        public string naacschoolclguinversity { get; set; }
        public string NCMACY_NAACCycle { get; set; }
        public int NCMACY_Order { get; set; }
        public Array getcriteria { get; set; }
        public Array getparentidzero { get; set; }
        public Array getalldata { get; set; }
        public Array getsavealldata { get; set; }
        public Array getdocumentlist { get; set; }
        public Array getcommentslist { get; set; }
        public Array getgeneralcommentslist { get; set; }
        public Array viewhyperlinks { get; set; }
        public Array getinstitutioncycle { get; set; }
        public Array reportlist { get; set; }
        public Array reportlisthead { get; set; }
        public Array getcycle { get; set; }
        public Array reportlist2 { get; set; }
        public Array reportlist3 { get; set; }
        public Array reportlist4 { get; set; }
        public Array yearlist { get; set; }
        public Array getinstitutionlist { get; set; }
        public bool returnval { get; set; }
        public selecteddetails_temp[] selecteddetails_temp { get; set; }
        public temp_mi_id_list[] temp_mi_id_list { get; set; }
        public approvedata[] approvedlist { get; set; }
        public approvedocuments[] approvedfilelist { get; set; }
        public long cycleid { get; set; }
        public string cyclename { get; set; }
        public int cycleorder { get; set; }
        public long ASMAY_Id { get; set; }
        public string NAACSL_SLNo_level0 { get; set; }
        public string NAACSL_SLNo_level1 { get; set; }
        public string NAACSL_SLNo_level2 { get; set; }
        public string NAACSL_SLNo { get; set; }
        public string htmldata { get; set; }
        public string pagename { get; set; }
        public string ASMAY_Year { get; set; }
        public int Flag { get; set; }
        public long fileid { get; set; }
        public string filename { get; set; }
        public string filepath { get; set; }
        public long filefkid { get; set; }
        public bool? fileapprovedflag { get; set; }
        public string filestatus { get; set; }
        public string messagevalue { get; set; }
        public string approvalmessage { get; set; }
        public string approvedornot { get; set; }
        public bool approvalflag { get; set; }
        public bool? approvalflagnew { get; set; }

        // Approval Details 
        public long NAC_Id { get; set; }
        public string NAC_SNo { get; set; }
        public long NAC_CycleId { get; set; }
        public string NAC_Flag { get; set; }
        public long NAC_UserId { get; set; }
        public DateTime? NAC_CreatedDate { get; set; }
        public DateTime? NAC_UpdateDate { get; set; }
        public string Remarks { get; set; }
        public Array datawisecomments { get; set; }

        // COMMENTS VIEW PARAMETERS
        public string commentsremarks { get; set; }
        public long commentsid { get; set; }
        public long commentuserid { get; set; }
        public string commentuser { get; set; }
        public DateTime? commentcreatedate { get; set; }
        public Array approvedreportdetails { get; set; }
        public Array approveddocreportdetails { get; set; }

    }

    public class selecteddetails_temp
    {
        public long NCMACY_Id { get; set; }
        public string NCMACY_NAACCycle { get; set; }
        public long MI_Id { get; set; }
        public long cycleid { get; set; }
        public string cyclename { get; set; }
        public int cycleorder { get; set; }
        public long ASMAY_Id { get; set; }

    }
    public class temp_mi_id_list
    {
        public long MI_Id { get; set; }
    }
    public class approvedata
    {
        public long filefkid { get; set; }
        public long naacsL_Id { get; set; }
        public string naacsL_SLNo { get; set; }
        public long naacsL_ParentId { get; set; }
    }
    public class approvedocuments
    {
        public long filefkid { get; set; }
        public long naacsL_Id { get; set; }
        public string naacsL_SLNo { get; set; }
        public long naacsL_ParentId { get; set; }
        public long fileid { get; set; }

    }
}
