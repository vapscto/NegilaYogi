using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Portals.IVRS
{
   public class CallDashboardDTO
    {

public long MI_Id { get; set; }
public Array reportlist { get; set; }
public long inboundcall { get; set; }
public long outboundcall { get; set; }
public Array subdomain_name { get; set; }
public string IVRM_Sub_Domain_Name { get; set; }
public string IIVRSC_VirtualNo { get; set; }
public string flag { get; set; }
public long received { get; set; }
public long NotConnectedCount { get; set; }
public long sum { get; set; }
public long ConnectedCount { get; set; }
public string sub_name { get; set; }
        public long IMCS_AssignedCall { get; set; }
        public long IMCS_InboundCalls { get; set; }
        public long IMCS_OutboundCalls { get; set; }
        public long IMCS_AvailableCalls { get; set; }
        public Array reportdatelist { get; set; }
        public Array reportdatelist2 { get; set; }

    }
}
