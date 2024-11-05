using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class INV_Master_ProductDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }

        public long INVMP_Id { get; set; }
        public long MI_Id { get; set; }
        public string INVMP_ProductName { get; set; }
        public string INVMP_ProductCode { get; set; }
        public decimal INVMP_ProductPrice { get; set; }
        public bool INVMP_TaxApplFlg { get; set; }
        public bool INVMP_ActiveFlg { get; set; }
        public string INVMI_ItemName { get; set; }

        public long INVMPT_Id { get; set; }
        public long INVMT_Id { get; set; }
        public decimal INVMPT_TaxValue { get; set; }
        public bool INVMPT_ActiveFlg { get; set; }

        public string INVMT_TaxName { get; set; }
        public string INVMT_TaxAliasName { get; set; }

        public long INVMPI_Id { get; set; }
        public long INVMI_Id { get; set; }
        public decimal INVMPI_ItemQty { get; set; }
        public bool INVMPI_ActiveFlg { get; set; }


        public Array get_tax { get; set; }
        public Array get_product { get; set; }
        public Array get_item { get; set; }
        public Array get_productlist { get; set; }
        public Array get_productItemlist { get; set; }
        public Array get_productTax { get; set; }
        public Array gridproductTax { get; set; }
        public Array get_store_product { get; set; }

        public ProductTaxDTO[] product_tax { get; set; }

        // production
        public string INVMPS_Stages { get; set; }
        public long INVMPS_Id { get; set; }
        public long INVMPSS_Id { get; set; }
        public bool INVMPS_ActiveFlg { get; set; }
        public ProductTaxDTO1[] product_stage { get; set; }
        public decimal INVMPSS_Status { get; set; }
        public bool INVMPSS_ActiveFlg { get; set; }
        public long DCSSP_Id { get; set; }
        public bool DCSSP_Activeflag { get; set; }
        public long INVMST_Id { get; set; }
        public string store_name { get; set; }
    }

    public class ProductTaxDTO
    {
        public long INVMPT_Id { get; set; }
        public long INVMP_Id { get; set; }
        public long INVMT_Id { get; set; }
        public decimal INVMPT_TaxValue { get; set; }
        public bool INVMPT_ActiveFlg { get; set; }

    }

    public class ProductTaxDTO1
    {
        public long INVMP_Id { get; set; }
        public long INVMPSS_Id { get; set; }
        public long INVMPS_Id { get; set; }
        public decimal INVMPSS_Status { get; set; }
        public bool INVMPSS_ActiveFlg { get; set; }

    }



}
