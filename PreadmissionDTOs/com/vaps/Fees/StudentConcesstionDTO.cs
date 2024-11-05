using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class StudentConcesstionDTO
    {
        public long MI_Id { get; set; }
        public Array acayear { get; set; }
        public Array fgrp { get; set; }
        public Array classlist { get; set; }
        public Array sectionlist { get; set; }
        public long FMG_Id { get; set; }

        public long Class { get; set; }
        public long Type { get; set; }

        public long FMCC_Id { get; set; }
        public string typeofrpt { get; set; }
        public long StaffWiseMflag { get; set; }
        public long GeneralMflag { get; set; }
        public long TotalMflag { get; set; }
        public long CDAdjustMflag { get; set; }
        public long asmyid { get; set; }
        public long clsid { get; set; }
        public long setid { get; set; }
        public Array reportdatelist { get; set; }
        public TempDTO[] TempararyArrayListnew { get; set; }

        public TempDTO[] fmcC_ConcessionName { get; set; }

        public Array concategory { get; set; }

        public object grpidsget;

        public string reporttype { get; set; }

        public Array fillmastergroup { get; set; }
        public Array customlist { get; set; }
        public Array grouplist { get; set; }
        public long userid { get; set; }
        public string customflag { get; set; }

        public string groupflag { get; set; }
        public string termflag { get; set; }
        public long[] FMGG_Ids { get; set; }
        public long[] FMG_Ids { get; set; }
        public long[] FMT_Ids { get; set; }
        public string term_group { get; set; }
        public string report { get; set; }
    }
}
