using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Location_Amount", Schema = "TRN")]
    public class TR_Location_AmountDMO
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
    }
}
