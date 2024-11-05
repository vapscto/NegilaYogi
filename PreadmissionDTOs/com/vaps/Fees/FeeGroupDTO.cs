using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeGroupDTO : CommonParamDTO
    {

        public int FMG_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMG_GroupName { get; set; }
        public string FMG_Remarks { get; set; }
        public string FMG_CompulsoryFlag { get; set; }
        public bool FMG_ActiceFlag { get; set; }
        public long user_id { get; set; }
        public bool returnval { get; set; }
        public long IVRMR_Id { get; set; }

        public bool Is_Active { get; set; }
        public Array GroupData { get; set; }
        public Array feeGroupname { get; set; }
        public string returnduplicatestatus { get; set; }

        //for yearly 

        public Array academicdrp { get; set; }
        public Array arraychkgrp { get; set; }
        public Array retriveYearlyGrpdata { get; set; }


        public List<FeeYearlyGroupDTO> dtoyrgroup { set; get; }
        public List<AcademicDTO> acyrdto { set; get; }
        public Array newary { get; set; }
        public Array getarray { get; set; }
        public Array activegrpnames { get; set; }

        // added on 06/03/2017
        public bool retvalue { get; set; }

        public long ASMAY_Id { get; set; }

        public string message { get; set; }

        public string FMG_PRE_FLAG { get; set; }

        public string FMG_RegNewFlg { get; set; }
        public bool FMG_HostelFlg { get; set; }
        public bool FMG_TransportFlg { get; set; }
        public bool FMG_BatchwiseFeeApplFlg { get; set; }
    }
}
