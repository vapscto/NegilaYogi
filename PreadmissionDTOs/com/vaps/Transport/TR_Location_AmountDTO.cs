using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class TR_Location_AmountDTO
    {
        public long TRMLAMT_Id { get; set; }
        public long TRML_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public bool TRMLAMT_ActiveFlg { get; set; }
        public DateTime TRMLAMT_CreatedDate { get; set; }
        public DateTime TRMLAMT_UpdatedDate { get; set; }
        public long TRMLAMT_CreatedBy { get; set; }
        public long TRMLAMT_UpdatedBy { get; set; }
        public Decimal TRMLAMT_OneWayAmount { get; set; }
        public Decimal TRMLAMT_TwoWayAmount { get; set; }
        public long MI_Id { get; set; }

        public string TRML_LocationName { get; set; }

        public string message { get; set; }

        public bool retrval { get; set; }

        public long user_id { get; set; }

        public Array editdatadetails { get; set; }

      
    }
}
