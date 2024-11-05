using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class TR_Area_AmountDTO
    {
        public long TRMAAMT_Id { get; set; }
        public long TRMA_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public bool TRMAAMT_ActiveFlg { get; set; }
        public DateTime TRMAAMT_CreatedDate { get; set; }
        public DateTime TRMAAMT_UpdatedDate { get; set; }
        public long TRMAAMT_CreatedBy { get; set; }
        public long TRMAAMT_UpdatedBy { get; set; }
        public decimal TRMAAMT_OneWayAmount { get; set; }
        public decimal TRMAAMT_TwoWayAmount { get; set; }

        public long user_id { get; set; }

        public string message { get; set; }
        public bool retrval { get; set; }

        public Array editdatadetails { get; set; }

        public long MI_Id { get; set; }
        public Array areadata { get; set; }
    }
}
