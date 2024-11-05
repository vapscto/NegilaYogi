using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Library
{
   public class MasterDonorDTO:CommonParamDTO
    {
        public long Donor_Id { get; set; }
        public long MI_Id { get; set; }
        public string Donor_Name { get; set; }
        public string Donor_Address { get; set; }
        public bool Donor_ActiveFlag { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public Array donorlist { get; set; }
    }
}
