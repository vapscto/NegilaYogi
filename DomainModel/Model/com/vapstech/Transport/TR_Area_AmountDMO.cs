using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Area_Amount", Schema = "TRN")]
    public class TR_Area_AmountDMO
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
    }
}
