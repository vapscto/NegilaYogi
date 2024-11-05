using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("INV_Stock", Schema = "INV")]
    public class INV_StockDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INVSTO_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMST_Id { get; set; }
        public long INVMI_Id { get; set; }
        public DateTime? INVSTO_PurchaseDate { get; set; }
        public decimal? INVSTO_PurchaseRate { get; set; }
        public string INVSTO_BatchNo { get; set; }
        public decimal? INVSTO_SalesRate { get; set; }
        public decimal? INVSTO_PurOBQty { get; set; }
        public decimal? INVSTO_PurRetQty { get; set; }
        public decimal? INVSTO_SalesQty { get; set; }
        public decimal? INVSTO_SalesRetQty { get; set; }
        public decimal? INVSTO_ItemConQty { get; set; }
        public decimal? INVSTO_MatIssPlusQty { get; set; }
        public decimal? INVSTO_MatIssMinusQty { get; set; }
        public decimal? INVSTO_PhyPlusQty { get; set; }
        public decimal? INVSTO_PhyMinQty { get; set; }
        public decimal? INVSTO_AvaiableStock { get; set; }
        public decimal? INVSTO_CheckedOutQty { get; set; }
        public decimal? INVSTO_DisposedQty { get; set; }
        public long IMFY_Id { get; set; }
        



    }
}
