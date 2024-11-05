using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class INV_Master_ItemDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
        public string INVMI_GroupItemNo { get; set; }

        public long INVMI_Id { get; set; }
        public long MI_Id { get; set; }
        public long INVMG_Id { get; set; }

        public long INVMUOM_Id { get; set; }
        public string INVMI_ItemName { get; set; }
        public decimal INVMI_MaxStock { get; set; }
        public bool INVMI_TaxAplFlg { get; set; }
        public string INVMI_ItemCode { get; set; }
        public bool INVMI_ActiveFlg { get; set; }
        public decimal INVMI_ReorderStock { get; set; }
        public bool INVMI_RawMatFlg { get; set; }
        public bool INVMI_ForSaleFlg { get; set; }
        public bool INVMI_MaintenanceAplFlg { get; set; }
        public string INVMI_HSNCode { get; set; }
        public string INVMUOM_UOMName { get; set; }
        public long INVMIT_Id { get; set; }
        public long INVMT_Id { get; set; }
        public decimal INVMIT_TaxValue { get; set; }
        public bool INVMIT_ActiveFlg { get; set; }
        public string INVMT_TaxName { get; set; }
        public string INVMT_TaxAliasName { get; set; }


        public string INVMG_GroupName { get; set; }
        public Array get_tax { get; set; }
        public Array get_UOM { get; set; }

        public Array get_itemgroup { get; set; }
        public Array get_item { get; set; }
        public Array get_itemTax { get; set; }

        public Array griditemTax { get; set; }
        //===================================== ITEM REPORT
        public string optionflag { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public Array get_itemreportdetails { get; set; }
        public Array get_itemreport { get; set; }



        public Tax_ApplicableDTO[] tax_Applicable { get; set; }
        public itemsArrayDTO[] itemsArray { get; set; }
    }
    public class Tax_ApplicableDTO
    {
        public long INVMIT_Id { get; set; }
        public long INVMI_Id { get; set; }
        public long INVMT_Id { get; set; }
        public decimal INVMIT_TaxValue { get; set; }
        public bool INVMIT_ActiveFlg { get; set; }

    }

    public class itemsArrayDTO
    {
        public long INVMI_Id { get; set; }
        public long INVMST_Id { get; set; }
        public long INVMG_Id { get; set; }
        public long AMST_Id { get; set; }
    }
}
