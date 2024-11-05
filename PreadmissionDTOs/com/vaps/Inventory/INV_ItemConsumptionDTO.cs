using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class INV_ItemConsumptionDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool? returnval { get; set; }
        public bool? already_cnt { get; set; }
        public string message { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long INVMI_Id { get; set; }
        public long INVMUOM_Id { get; set; }
        public string INVMUOM_UOMName { get; set; }
        public string INVMP_ProductName { get; set; }
        public string INVMI_ItemName { get; set; }
        public string INVMI_ItemCode { get; set; }
        public string employeename { get; set; }
        public string studentname { get; set; }
        public long HRME_Id { get; set; }
        public int? HRME_EmployeeOrder { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public int? AMST_ActiveFlag { get; set; }
        public string AMST_SOL { get; set; }
        public string AMST_AdmNo { get; set; }

        public long INVMIC_Id { get; set; }
        public long INVMST_Id { get; set; }
        public string INVMS_StoreName { get; set; }
        public string INVMIC_StuOtherFlg { get; set; }
        public string INVMIC_ICNo { get; set; }
        public DateTime INVMIC_ICDate { get; set; }
        public string INVMIC_Remarks { get; set; }
        public bool INVMIC_ActiveFlg { get; set; }
        public long INVTIC_Id { get; set; }
        public long INVMP_Id { get; set; }
        public decimal INVTIC_ICPrice { get; set; }
        public string INVTIC_BatchNo { get; set; }
        public decimal INVTIC_ICQty { get; set; }
        public string INVTIC_Naration { get; set; }
        public bool INVTIC_ActiveFlg { get; set; }
        public long INVMICS_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public long ASMS_Id { get; set; }
        public string ASMC_SectionName { get; set; }
        public bool INVMICS_ActiveFlg { get; set; }
        public long INVMICST_Id { get; set; }
        public bool INVMICST_ActiveFlg { get; set; }
        public long INVMICD_Id { get; set; }
        public long HRMD_Id { get; set; }
        public bool INVMICD_ActiveFlg { get; set; }
        public string userflag { get; set; }
        public decimal INVSTO_SalesRate { get; set; }

        //==========================Reports
        public Array get_ICreportdetails { get; set; }
        public Array get_ICReport { get; set; }
        public stockItemArrayDTO[] ICStaffArray { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string typeflag { get; set; }
        public string optionflag { get; set; }

        public staffarrayDTO[] staffarray { get; set; }
        public arrayStudentname1[] arrayStudentname { get; set; }

        //=======================================

        public Array get_sectionlist { get; set; }
        public Array get_Store { get; set; }
        public Array get_item { get; set; }
        public Array get_employee { get; set; }
        public Array get_Student_Cls_Sec { get; set; }
        public Array get_Department { get; set; }
        public Array get_Product { get; set; }
        public Array get_Student { get; set; }
        public Array get_itemconsumption { get; set; }
        public Array get_ICdetails { get; set; }
        public Array get_ICuser { get; set; }
        public arrayICDTO[] arrayIC { get; set; }
        public arrayStaffDTO[] arrayStaff { get; set; }
        public string  FlagsC { get; set; }
        public Array semesterlist { get; set; }
        public Array sectionClglist { get; set; }
        public long ACMS_Id { get; set; }
        public long AMSE_Id { get; set; }
    }
    public class arrayICDTO
    {
        public long INVTIC_Id { get; set; }
        public long INVMI_Id { get; set; }
        public long INVMUOM_Id { get; set; }
        public long INVMIC_Id { get; set; }
        public long INVMP_Id { get; set; }
        public string INVTIC_BatchNo { get; set; }
        public decimal INVTIC_ICQty { get; set; }
        public string INVTIC_Naration { get; set; }
        public bool INVTIC_ActiveFlg { get; set; }
        public decimal INVTIC_ICPrice { get; set; }
        public decimal INVSTO_SalesRate { get; set; }
    }
    public class arrayStaffDTO
    {
        public long INVMICST_Id { get; set; }
        public long INVMIC_Id { get; set; }
        public long HRME_Id { get; set; }
        public bool INVMICST_ActiveFlg { get; set; }
    }
    public class staffarrayDTO
    {
        public long HRME_Id { get; set; }
    } public class arrayStudentname1
    {
        public long AMST_Id { get; set; }
        public long AMSE_Id { get; set; }
    }

}
