using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VidyaBharathi
{
    public class VBSC_Master_SportsCCName_UOMDTO
    {
        public long VBSCMSCCUOM_Id { get; set; }
        public long VBSCMSCC_Id { get; set; }
        public long VBCCMUOM_Id { get; set; }
        public long MT_Id { get; set; }
        public Array uomList { get; set; }
        public Array sportsCCNameUOMList { get; set; }
        public Array sportsCCNameList { get; set; }
        public Array editDetails { get; set; }
        public Array deactivate { get; set; }
        public Array save{get;set;}
        public string sportsCCName { get; set; }
        public string returnVal { get; set; }
        public string uom { get; set; }
        public string sport { get; set; }
        public string uomName { get; set; }
        public bool VBSCMSCCUOM_ActiveFlag { get; set; }
        public bool retval { get; set; }
        public bool VBSCMSCC_ActiveFlag { get; set; }
        public int count { get; set; }
        
    }
}
