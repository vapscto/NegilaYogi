using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Inventory;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.Purchase.Inventory;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.Fee;
using DomainModel.Model.com.vapstech.Transport;
using DomainModel.Model.com.vapstech.VMS;
using DomainModel.Model.com.vapstech.VMS.Sales;

namespace DataAccessMsSqlServerProvider.com.vapstech.Inventory
{
    public class InventoryContext : DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options)
        { }
      
        public DbSet<INV_Master_CategoryDMO> INV_Master_CategoryDMO { get; set; }
        public DbSet<IVRM_Master_FinancialYear> IVRM_Master_FinancialYear { get; set; }
        public DbSet<INV_Master_GroupDMO> INV_Master_GroupDMO { get; set; }
        public DbSet<INV_Master_UOMDMO> INV_Master_UOMDMO { get; set; }
        public DbSet<INV_Master_TaxDMO> INV_Master_TaxDMO { get; set; }
        public DbSet<INV_Master_StoreDMO> INV_Master_StoreDMO { get; set; }
        public DbSet<INV_Master_CustomerDMO> INV_Master_CustomerDMO { get; set; }
        public DbSet<INV_Master_ItemDMO> INV_Master_ItemDMO { get; set; }
        public DbSet<INV_Master_Item_TaxDMO> INV_Master_Item_TaxDMO { get; set; }
        public DbSet<INV_Master_ProductDMO> INV_Master_ProductDMO { get; set; }
        public DbSet<INV_Master_Product_ItemDMO> INV_Master_Product_ItemDMO { get; set; }
        public DbSet<INV_Master_Product_TaxDMO> INV_Master_Product_TaxDMO { get; set; }
        public DbSet<INV_Master_SupplierDMO> INV_Master_SupplierDMO { get; set; }
        public DbSet<INV_OpeningBalanceDMO> INV_OpeningBalanceDMO { get; set; }
        public DbSet<INV_StockDMO> INV_StockDMO { get; set; }
        public DbSet<MasterEmployee> MasterEmployee { get; set; }
        public DbSet<HR_Master_Department> HR_Master_Department { get; set; }
        //===================================Purchase DMO ========================  
        public DbSet<INV_M_PurchaseRequisitionDMO> INV_M_PurchaseRequisitionDMO { get; set; }
        public DbSet<INV_T_PurchaseRequisitionDMO> INV_T_PurchaseRequisitionDMO { get; set; }
        public DbSet<INV_M_PurchaseIndentDMO> INV_M_PurchaseIndentDMO { get; set; }
        public DbSet<INV_T_PurchaseIndentDMO> INV_T_PurchaseIndentDMO { get; set; }
        public DbSet<INV_PurchaseIndent_ToSupplierDMO> INV_PurchaseIndent_ToSupplierDMO { get; set; }        
        public DbSet<INV_M_SupplierQuotationDMO> INV_M_SupplierQuotationDMO { get; set; }
        public DbSet<INV_T_SupplierQuotationDMO> INV_T_SupplierQuotationDMO { get; set; }
        public DbSet<INV_M_PurchaseOrderDMO> INV_M_PurchaseOrderDMO { get; set; }
        public DbSet<INV_T_PurchaseOrderDMO> INV_T_PurchaseOrderDMO { get; set; }
        public DbSet<INV_T_PurchaseOrder_TaxDMO> INV_T_PurchaseOrder_TaxDMO { get; set; }
        public DbSet<INV_M_GRNDMO> INV_M_GRNDMO { get; set; }
        public DbSet<INV_T_GRNDMO> INV_T_GRNDMO { get; set; }
        public DbSet<INV_T_GRN_TaxDMO> INV_T_GRN_TaxDMO { get; set; }
        public DbSet<INV_M_GRN_PODMO> INV_M_GRN_PODMO { get; set; }
        public DbSet<INV_M_GRN_StoreDMO> INV_M_GRN_StoreDMO { get; set; }
        public DbSet<IVRM_ModeOfPaymentDMO> IVRM_ModeOfPaymentDMO { get; set; }
        public DbSet<INV_Supplier_PaymentDMO> INV_Supplier_PaymentDMO { get; set; }
        public DbSet<INV_Supplier_Payment_GRNDMO> INV_Supplier_Payment_GRNDMO { get; set; }
        //===================================Sales DMO =======================       
        public DbSet<INV_M_SalesDMO> INV_M_SalesDMO { get; set; }
        public DbSet<INV_M_Sales_CustomerDMO> INV_M_Sales_CustomerDMO { get; set; }
        public DbSet<INV_M_Sales_StaffDMO> INV_M_Sales_StaffDMO { get; set; }
        public DbSet<INV_M_Sales_StudentDMO> INV_M_Sales_StudentDMO { get; set; }
        public DbSet<INV_T_SalesDMO> INV_T_SalesDMO { get; set; }
        public DbSet<INV_T_Sales_TaxDMO> INV_T_Sales_TaxDMO { get; set; }
        public DbSet<INV_M_ItemConsumptionDMO> INV_M_ItemConsumptionDMO { get; set; }
        public DbSet<INV_T_ItemConsumptionDMO> INV_T_ItemConsumptionDMO { get; set; }
        public DbSet<INV_M_IC_StaffDMO> INV_M_IC_StaffDMO { get; set; }
        public DbSet<INV_M_IC_DepartmentDMO> INV_M_IC_DepartmentDMO { get; set; }
        public DbSet<INV_M_IC_StudentDMO> INV_M_IC_StudentDMO { get; set; }
        public DbSet<INV_PhysicalStock_UpdationDMO> INV_PhysicalStock_UpdationDMO { get; set; }
        //=================================== College Sales DMO =======================       
        public DbSet<INV_M_Sales_College_StudentDMO> INV_M_Sales_College_StudentDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_CourseDMO> CLG_Adm_College_AY_CourseDMO { get; set; }
        public DbSet<MasterCourseDMO> MasterCourseDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_Course_BranchDMO> CLG_Adm_College_AY_Course_BranchDMO { get; set; }
        public DbSet<ClgMasterBranchDMO> ClgMasterBranchDMO { get; set; }
        public DbSet<CLG_Adm_College_AY_Course_Branch_SemesterDMO> CLG_Adm_College_AY_Course_Branch_SemesterDMO { get; set; }
        public DbSet<CLG_Adm_Master_SemesterDMO> CLG_Adm_Master_SemesterDMO { get; set; }
        //==============================ADDMISSION DMO========================
        public DbSet<Adm_M_Student> Adm_M_Student { get; set; }
        public DbSet<School_M_Section> School_M_Section { get; set; }
        public DbSet<School_M_Class> School_M_Class { get; set; }
        public DbSet<AcademicYear> AcademicYear { get; set; }
        public DbSet<School_Adm_Y_StudentDMO> School_Adm_Y_StudentDMO { get; set; }
        //===================================Inventory Configuration ========================  
        public DbSet<INV_ConfigurationDMO> INV_ConfigurationDMO { get; set; }
        public DbSet<Month> mnth { get; set; }
        public DbSet<MasterRoleType> IVRM_Role_Type { get; set; }
        //====================================================================

        public DbSet<INV_Master_Product_StagesDMO> INV_Master_Product_StagesDMO { get; set; }
        public DbSet<INV_Master_Product_Stages_StatusDMO> INV_Master_Product_Stages_StatusDMO { get; set; }
        public DbSet<DCS_PhysicalStock_UpdationDMO> DCS_PhysicalStock_UpdationDMO { get; set; }
        public DbSet<DCS_StockDMO> DCS_StockDMO { get; set; }
        public DbSet<DCS_M_Sales_CustomerDMO> DCS_M_Sales_CustomerDMO { get; set; }
        public DbSet<DCS_T_SalesDMO> DCS_T_SalesDMO { get; set; }
        public DbSet<DCS_M_SalesDMO> DCS_M_SalesDMO { get; set; }
        public DbSet<DCS_Supplier_PaymentDMO> DCS_Supplier_PaymentDMO { get; set; }
        public DbSet<DCS_Supplier_Payment_DetailsDMO> DCS_Supplier_Payment_DetailsDMO { get; set; }
        public DbSet<DCS_Store_ProductDMO> DCS_Store_ProductDMO { get; set; }
        public DbSet<INV_T_Sales_Tax_Return_DMO> INV_T_Sales_Tax_Return_DMO_con { get; set; }
        public DbSet<INV_M_Sales_Return_DMO> INV_M_Sales_Return_DMO_con { get; set; }
        public DbSet<INV_T_Sales_Return_DMO> INV_T_Sales_Return_DMO_con { get; set; }
        public DbSet<INV_M_Sales_Return_Apply_DMO> INV_M_Sales_Return_Apply_DMO { get; set; }
        public DbSet<INV_T_Sales_Return_Apply_DMO> INV_T_Sales_Return_Apply_DMO { get; set; }
        public DbSet<NV_M_ItemConsumptionCLGDMO> NV_M_ItemConsumptionCLGDMO { get; set; }
        public DbSet<Adm_College_Master_SectionDMO> Adm_College_Master_SectionDMO { get; set; }

        public DbSet<Institution> Institution { get; set; }
        public DbSet<ISM_Master_Client_DMO> clientTable { get; set; }
        public DbSet<ISM_Client_Master_Components_DMO> ISM_Client_Master_Components_DMO { get; set; }
        public DbSet<ISM_Client_Project_BOM_DMO> ISM_Client_Project_BOM_DMO { get; set; }
        public DbSet<ISM_InvoiceDMO> ISM_InvoiceDMO { get; set; }
        public DbSet<ISM_Invoice_DetailsDMO> ISM_Invoice_DetailsDMO { get; set; }
        public DbSet<ISM_Proforma_InvoiceDMO> ISM_Proforma_InvoiceDMO { get; set; }
        public DbSet<MastersProject_DMO> MastersProject_DMO { get; set; }
        public DbSet<ISM_Master_Client_ProjectDMO> ISM_Master_Client_ProjectDMO { get; set; }
        public DbSet<ISM_Invoice_TaxDMO> ISM_Invoice_TaxDMO { get; set; }
        public DbSet<ISM_Client_Project_Payment_DetailsDMO> ISM_Client_Project_Payment_DetailsDMO { get; set; }
        public DbSet<ISM_Client_Payment_ConfigurationDMO> ISM_Client_Payment_ConfigurationDMO { get; set; }
        public DbSet<ISM_Proforma_Invoice_DetailsDMO> ISM_Proforma_Invoice_DetailsDMO { get; set; }
        public DbSet<ISM_Proforma_Invoice_TaxDMO> ISM_Proforma_Invoice_TaxDMO { get; set; }
        public DbSet<ISM_Master_Client_IEMapping_DMO> clientMappingTable { get; set; }
        public DbSet<ISM_Master_Client_DMO> ISM_Master_Client_DMO { get; set; }
        public DbSet<MasterModules> MasterModules { get; set; }
        public DbSet<Staff_User_Login> Staff_User_Login { get; set; }
        public DbSet<MastersModule_DMO> MastersModule_DMO { get; set; }
        public DbSet<HR_Master_DepartmentCodeDMO> HR_Master_DepartmentCodeDMO { get; set; }
        public DbSet<ISM_Master_Module_DevelopersDMO> ISM_Master_Module_DevelopersDMO { get; set; }
        public DbSet<ISM_TaskCreationDMO> ISM_TaskCreationDMO { get; set; }
        public DbSet<ISM_Master_TaskGroupDMO> ISM_Master_TaskGroupDMO { get; set; }
        public DbSet<ISM_Master_TaskGroup_DeptDMO> ISM_Master_TaskGroup_DeptDMO { get; set; }





        public DbSet<Institution_Phone_No> Institution_Phone_No { get; set; }
        public DbSet<Institution_EmailId> Institution_EmailId { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<INV_M_PurchaseRequisitionDMO>().HasKey(m => m.INVMPR_Id);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            return base.SaveChanges();
        }
        private void updateUpdatedProperty<T>() where T : class
        {
            var modifiedSourceInfo =
                ChangeTracker.Entries<T>()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in modifiedSourceInfo)
            {

            }
        }
    }
}