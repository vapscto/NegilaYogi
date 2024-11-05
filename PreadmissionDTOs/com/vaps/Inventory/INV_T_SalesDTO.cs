using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class INV_T_SalesDTO
    {
        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }
        public string trans_id { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool? returnval { get; set; }
        public bool? already_cnt { get; set; }
        public string message { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long IMFY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public int ASMCL_Order { get; set; }
        public long ASMS_Id { get; set; }
        public long INVMGRN_Id { get; set; }
        public long INVMS_Id { get; set; }
        public string ASMC_SectionName { get; set; }
        public int ASMC_Order { get; set; }
        public long AMST_Id { get; set; }
        public string membername { get; set; }
        public string AMST_AdmNo { get; set; }
        public string type { get; set; }
        public string saletype { get; set; }
        public string employeename { get; set; }
        public long HRME_Id { get; set; }
        public int? HRME_EmployeeOrder { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public long INVMI_Id { get; set; }
        public long INVMUOM_Id { get; set; }
        public string INVMUOM_UOMName { get; set; }
        public string INVMUOM_UOMAliasName { get; set; }
        public long INVSTO_Id { get; set; }
        public DateTime? INVSTO_PurchaseDate { get; set; }
        public string INVSTO_BatchNo { get; set; }
        public decimal? INVSTO_SalesRate { get; set; }
        public string INVMI_ItemName { get; set; }
        public string INVMI_ItemCode { get; set; }
        public long INVMT_Id { get; set; }
        public decimal? INVMIT_TaxValue { get; set; }
        public string INVMT_TaxName { get; set; }
        public string INVMT_TaxAliasName { get; set; }
        public string INVTSL_Naration { get; set; }
        public decimal? INVSTO_AvaiableStock { get; set; }

        //=======================INV_M_Sales
        public long INVMSL_Id { get; set; }
        public long INVMST_Id { get; set; }
        public string INVMS_StoreName { get; set; }
        public string INVMSL_StuOtherFlg { get; set; }
        public string INVMSL_SalesNo { get; set; }
        public DateTime? INVMSL_SalesDate { get; set; }
        public decimal? INVMSL_SalesValue { get; set; }
        public decimal? INVMSL_TotDiscount { get; set; }
        public decimal? INVMSL_TotTaxAmt { get; set; }
        public decimal? INVMSL_TotalAmount { get; set; }
        public string INVMSL_Remarks { get; set; }
        public string INVMSL_ReturnFlg { get; set; }
        public string INVMSL_PaidFlg { get; set; }
        public bool? INVMSL_CreditFlg { get; set; }
        public bool? INVMSL_ActiveFlg { get; set; }
        public long INVMP_Id { get; set; }
        public string Student_flag { get; set; }
        public string INVC_LIFOFIFOFlg { get; set; }


        //=======================INV_Sales
        public long INVMSLS_Id { get; set; }
        public long INVMSLST_Id { get; set; }
        public long INVMSLC_Id { get; set; }
        public long INVMC_Id { get; set; }
        public string INVMC_CustomerName { get; set; }
        public long INVTSL_Id { get; set; }
        public long INVTSLT_Id { get; set; }

        //=======================================
        //=======================================REPORT
        public Array get_salesdetails { get; set; }
        public Array get_class { get; set; }
        public Array get_Section { get; set; }
        public Array get_staff_customer { get; set; }
        public Array get_SalesReport { get; set; }
        public Array get_editdata { get; set; }
        public Array get_editmember { get; set; }
        public Array grnlist { get; set; }
    
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string optionflag { get; set; }
        public string typeflag { get; set; }
        public string individualflag { get; set; }

        public long? invmsl_id { get; set; }
        public long? invmi_id { get; set; }
        public long? amst_id { get; set; }
        public string studentid { get; set; }
        public long? hrme_id { get; set; }
        public long? invmc_id { get; set; }
        public string selectionflag { get; set; }

        public clsarrayDTO[] clsarray { get; set; }
        public secarrayDTO[] secarray { get; set; }
        public itemarrayDTO[] itemarray { get; set; }
        public salenoarrayDTO[] salenoarray { get; set; }
        public storenoarrayDTO[] storenoarray { get; set; }

        //=======================================
        public string INVTSL_BatchNo { get; set; }
        public decimal? INVTSL_SalesQty { get; set; }
        public decimal? INVTSL_SalesPrice { get; set; }
        public decimal? INVTSL_DiscountAmt { get; set; }
        public decimal? INVTSL_TaxAmt { get; set; }
        public decimal? INVTSL_Amount { get; set; }
        public bool? INVTSL_ActiveFlg { get; set; }
        public decimal? INVTSLT_TaxPer { get; set; }
        public decimal INVTSLT_TaxAmt { get; set; }
        public bool? INVTSLT_ActiveFlg { get; set; }
        public Array get_Student_Cls_Sec { get; set; }
        public Array get_Student_Cls_Sec_Item_SN { get; set; }
        public Array get_sectionlist { get; set; }
        public Array get_Studentlist { get; set; }
        public Array get_employee { get; set; }
        public Array get_customer { get; set; }
        public Array get_Product { get; set; }
        public Array get_itemTax { get; set; }
        public Array get_item { get; set; }
        public Array get_Store { get; set; }
        public Array get_itemDetail { get; set; }
        public Array availablestock { get; set; }
        public Array get_Sale { get; set; }
        public Array get_Saletypes { get; set; }
        public Array get_SaleItemDetails { get; set; }
        public Array get_SaleItemTax { get; set; }

        public SaleItemDTO[] SaleItem { get; set; }
        public StudentListDTO[] studentlist { get; set; }
        public clgstuarrayDTO[] clgstuarray { get; set; }

        public SaleProductDTO[] Saleproduct { get; set; }

        public vendorpaymentdto[] paymentdto { get; set; }
        //==================================================== COLLEGE DTO
        public long AMCO_Id { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMCO_CourseCode { get; set; }
        public string AMCO_CourseInfo { get; set; }
        public bool AMCO_CourseFlag { get; set; }
        public bool AMCO_ActiveFlag { get; set; }
        public int AMCO_Order { get; set; }

        public long AMB_Id { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMB_BranchCode { get; set; }
        public string AMB_BranchInfo { get; set; }
        public int AMB_Order { get; set; }
        public bool AMB_ActiveFlag { get; set; }

        public long AMSE_Id { get; set; }
        public string AMSE_SEMName { get; set; }
        public string AMSE_SEMCode { get; set; }
        public int AMSE_SEMOrder { get; set; }

        public long INVMSLCS_Id { get; set; }    
        public long AMCST_Id { get; set; }     
        public bool INVMSLCS_ActiveFlg { get; set; }

        public Array course_list { get; set; }
        public Array branch_list { get; set; }
        public Array sem_list { get; set; }
        public clgStudentListDTO[] clgStudentList { get; set; }

        //product

        public decimal? INVMP_ProductPrice { get; set; }
        public long DCSTSL_Id { get; set; }
        public long DCSMSL_Id { get; set; }
        public string DCS_Vehicleno { get; set; }
        public string INVMP_ProductName { get; set; }
        public string GSTNO { get; set; }
        public long DCSSPT_Id { get; set; }
        public DateTime INVSPT_PaymentDate { get; set; }
        public string INVSPT_ModeOfPayment { get; set; }
        public string INVSPT_PaymentReference { get; set; }
        public string INVSPT_ChequeDDNo { get; set; }
        public string INVSPT_BankName { get; set; }
        public DateTime? INVSPT_ChequeDDDate { get; set; }
        public decimal INVSPT_Amount { get; set; }
        public string INVSPT_Remarks { get; set; }
        public bool INVSPT_ActiveFlg { get; set; }
        public long userid { get; set; }
        public long DCSSPTGRN_Id { get; set; }
        public string INVMS_SupplierName { get; set; }
        public long DCSSPTD_Id { get; set; }

    }

    //=======================INV_T_Sales
    public class SaleItemDTO
    {
        public long INVTSL_Id { get; set; }
        public long INVMSL_Id { get; set; }
        public long INVMI_Id { get; set; }
        public long INVMUOM_Id { get; set; }
        public long INVSTO_Id { get; set; }
        public string INVTSL_BatchNo { get; set; }
        public decimal? INVTSL_SalesQty { get; set; }
        public decimal? INVTSL_SalesPrice { get; set; }
        public decimal? INVTSL_DiscountAmt { get; set; }
        public decimal? INVTSL_TaxAmt { get; set; }
        public decimal? INVTSL_Amount { get; set; }
        public string INVTSL_Naration { get; set; }
        public bool? INVTSL_ReturnFlg { get; set; }
        public decimal? INVTSL_ReturnQty { get; set; }
        public DateTime INVTSL_ReturnDate { get; set; }
        public string INVTSL_ReturnNo { get; set; }
        public string INVTSL_ReturnNaration { get; set; }
        public bool? INVTSL_ActiveFlg { get; set; }
        public Array get_itemTax { get; set; }

        public SaleItemTaxDTO[] saleItemTax { get; set; }
    }


    public class SaleProductDTO
    {
        public long INVTSL_Id { get; set; }
        public long INVMSL_Id { get; set; }
        public long INVMP_Id { get; set; }
        public long INVMUOM_Id { get; set; }
        public long INVSTO_Id { get; set; }
        public string INVTSL_BatchNo { get; set; }
        public decimal? INVTSL_SalesQty { get; set; }
        public decimal? INVTSL_SalesPrice { get; set; }
        public decimal? INVTSL_DiscountAmt { get; set; }
        public decimal? INVTSL_TaxAmt { get; set; }
        public decimal? INVTSL_Amount { get; set; }
        public string INVTSL_Naration { get; set; }
        public bool? INVTSL_ReturnFlg { get; set; }
        public decimal? INVTSL_ReturnQty { get; set; }
        public DateTime INVTSL_ReturnDate { get; set; }
        public string INVTSL_ReturnNo { get; set; }
        public string INVTSL_ReturnNaration { get; set; }
        public bool? INVTSL_ActiveFlg { get; set; }
        public Array get_itemTax { get; set; }

        public SaleItemTaxDTO[] saleItemTax { get; set; }
    }


    public class vendorpaymentdto
    {
      //  public long DCSSPT_Id { get; set; }
        public long INVMI_Id { get; set; }
        public decimal INVSPTGRN_Amount { get; set; }
        public string INVSPTGRN_Remarks { get; set; }
      //  public bool INVSPTGRN_ActiveFlg { get; set; }
       
    }






    //=======================INV_T_Sales_Tax
    public class SaleItemTaxDTO
    {
        public long INVTSLT_Id { get; set; }
        public long INVTSL_Id { get; set; }
        public long INVMT_Id { get; set; }
        public decimal? INVTSLT_TaxPer { get; set; }
        public decimal INVTSLT_TaxAmt { get; set; }
        public bool? INVTSLT_ActiveFlg { get; set; }
    }
    //=======================INV_M_Sales_Students
    public class StudentListDTO
    {
        public long INVMSLS_Id { get; set; }
        public long INVMSL_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public bool? INVMSLS_ActiveFlg { get; set; }
    }
       //=======================INV_M_Sales_Students
    public class clgStudentListDTO
    {
        public long INVMSLCS_Id { get; set; }
        public long INVMSL_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
    }

    //=======================Sales  Report
    public class clsarrayDTO
    {
        public long? amst_id { get; set; }
        public long? asmcl_id { get; set; }
        public long? asms_id { get; set; }

    }
    public class secarrayDTO
    {
        public long? amst_id { get; set; }
        public long? asmcl_id { get; set; }
        public long? asms_id { get; set; }
    }
    public class itemarrayDTO
    {
        public long? amst_id { get; set; }
        public long? invmi_id { get; set; }
    }
    public class salenoarrayDTO
    {
        public long? invmsl_id { get; set; }
        public long? invmi_id { get; set; }
    }
    public class storenoarrayDTO
    {
        public long? invmst_id { get; set; }
    }

    public class clgstuarrayDTO
    {
        public long AMCST_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
    }

}
