using PreadmissionDTOs.com.vaps.VMS.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
    public interface Sales_Lead_Master_Interface
    {
        //---------------------- Category-----------------------
        ISM_Sales_Master_Category_DTO get_load_Cat(int id);
        ISM_Sales_Master_Category_DTO deactivate_Cat(ISM_Sales_Master_Category_DTO id);
        ISM_Sales_Master_Category_DTO SaveEdit_Cat(ISM_Sales_Master_Category_DTO dto);
        ISM_Sales_Master_Category_DTO Edit_details_Cat(ISM_Sales_Master_Category_DTO dto);
        //---------------------- Product-----------------------
        ISM_Sales_Master_Product_DTO get_load_pro(int id);
        ISM_Sales_Master_Product_DTO deactivate_pro(ISM_Sales_Master_Product_DTO id);
        ISM_Sales_Master_Product_DTO SaveEdit_pro(ISM_Sales_Master_Product_DTO dto);
        ISM_Sales_Master_Product_DTO Edit_details_pro(ISM_Sales_Master_Product_DTO dto); 
        //----------------------Source-----------------------
        ISM_Sales_Master_Source_DTO get_load_src(int id);
        ISM_Sales_Master_Source_DTO deactivate_src(ISM_Sales_Master_Source_DTO id);
        ISM_Sales_Master_Source_DTO SaveEdit_src(ISM_Sales_Master_Source_DTO dto);
        ISM_Sales_Master_Source_DTO Edit_details_src(ISM_Sales_Master_Source_DTO dto);
        //----------------------Status-----------------------
        ISM_Sales_Master_Status_DTO get_load_sts(int id);
        ISM_Sales_Master_Status_DTO deactivate_sts(ISM_Sales_Master_Status_DTO id);
        ISM_Sales_Master_Status_DTO SaveEdit_sts(ISM_Sales_Master_Status_DTO dto);
        ISM_Sales_Master_Status_DTO Edit_details_sts(ISM_Sales_Master_Status_DTO dto);
        //----------------------Lead-----------------
        ISM_Sales_Lead_DTO load_all_lead(ISM_Sales_Lead_DTO dTO);
        ISM_Sales_Lead_DTO Save_Edit_SaleLead(ISM_Sales_Lead_DTO dTO);
        ISM_Sales_Lead_DTO Sales_lead_edit(ISM_Sales_Lead_DTO dTO);
        ISM_Sales_Lead_DTO deactiv_prde(ISM_Sales_Lead_DTO dTO);
        ISM_Sales_Lead_DTO select_state(ISM_Sales_Lead_DTO dTO);
        ISM_Sales_Lead_DTO checkemailtemplet(ISM_Sales_Lead_DTO dTO);
        //----------------------Lead Demo-----------------
        ISM_Sales_Lead_Demo_DTO get_load_lead_demo(ISM_Sales_Lead_Demo_DTO dto);
        ISM_Sales_Lead_Demo_DTO Edit_details_lead_demo(ISM_Sales_Lead_Demo_DTO dto);
        ISM_Sales_Lead_Demo_DTO Edit_response_lead_demo(ISM_Sales_Lead_Demo_DTO dto);
        ISM_Sales_Lead_Demo_DTO SaveEdit_lead_demo(ISM_Sales_Lead_Demo_DTO dto);
        ISM_Sales_Lead_Demo_DTO Save_response_lead_demo(ISM_Sales_Lead_Demo_DTO dto);
        ISM_Sales_Lead_Demo_DTO view_lead_demo(ISM_Sales_Lead_Demo_DTO dto);
    }
}
