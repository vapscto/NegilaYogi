using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS;
using PreadmissionDTOs.com.vaps.VMS.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS
{
    public class Sales_Lead_Delegate
    {
        CommonVMSDelegate<ISM_Sales_Master_Category_DTO, ISM_Sales_Master_Category_DTO> COMMC = new CommonVMSDelegate<ISM_Sales_Master_Category_DTO, ISM_Sales_Master_Category_DTO>();
        CommonVMSDelegate<ISM_Sales_Master_Product_DTO, ISM_Sales_Master_Product_DTO> COMMP = new CommonVMSDelegate<ISM_Sales_Master_Product_DTO, ISM_Sales_Master_Product_DTO>();
        CommonVMSDelegate<ISM_Sales_Master_Source_DTO, ISM_Sales_Master_Source_DTO> COMMS = new CommonVMSDelegate<ISM_Sales_Master_Source_DTO, ISM_Sales_Master_Source_DTO>();
        CommonVMSDelegate<ISM_Sales_Lead_DTO, ISM_Sales_Lead_DTO> COMML = new CommonVMSDelegate<ISM_Sales_Lead_DTO, ISM_Sales_Lead_DTO>();
        CommonVMSDelegate<ISM_Sales_Master_Status_DTO, ISM_Sales_Master_Status_DTO> COMMSTS = new CommonVMSDelegate<ISM_Sales_Master_Status_DTO, ISM_Sales_Master_Status_DTO>();
        CommonVMSDelegate<ISM_Sales_Lead_Demo_DTO, ISM_Sales_Lead_Demo_DTO> COMMLD = new CommonVMSDelegate<ISM_Sales_Lead_Demo_DTO, ISM_Sales_Lead_Demo_DTO>();

        //----------------------------category------------------------------------
        public ISM_Sales_Master_Category_DTO get_load_Cat(int id)
        {
            return COMMC.GetDataById(id, "Sales_Lead_Facade/get_load_Cat/");
        }
        public ISM_Sales_Master_Category_DTO SaveEdit_Cat(ISM_Sales_Master_Category_DTO dto)
        {
            return COMMC.POSTData(dto, "Sales_Lead_Facade/SaveEdit_Cat/");
        }
        public ISM_Sales_Master_Category_DTO Edit_details_Cat(ISM_Sales_Master_Category_DTO dto)
        {
            return COMMC.POSTData(dto, "Sales_Lead_Facade/Edit_details_Cat/");
        }
        public ISM_Sales_Master_Category_DTO deactivate_Cat(ISM_Sales_Master_Category_DTO dto)
        {
            return COMMC.POSTData(dto, "Sales_Lead_Facade/deactivate_Cat/");
        }

        //=======================Product==============
        public ISM_Sales_Master_Product_DTO get_load_pro(int id)
        {
            return COMMP.GetDataById(id, "Sales_Lead_Facade/get_load_pro/");
        }
        public ISM_Sales_Master_Product_DTO SaveEdit_pro(ISM_Sales_Master_Product_DTO dto)
        {
            return COMMP.POSTData(dto, "Sales_Lead_Facade/SaveEdit_pro/");
        }
        public ISM_Sales_Master_Product_DTO Edit_details_pro(ISM_Sales_Master_Product_DTO dto)
        {
            return COMMP.POSTData(dto, "Sales_Lead_Facade/Edit_details_pro/");
        }
        public ISM_Sales_Master_Product_DTO deactivate_pro(ISM_Sales_Master_Product_DTO dto)
        {
            return COMMP.POSTData(dto, "Sales_Lead_Facade/deactivate_pro/");
        }

        //===================Source===================
              public ISM_Sales_Master_Source_DTO get_load_src(int id)
        {
            return COMMS.GetDataById(id, "Sales_Lead_Facade/get_load_src/");
        }
        public ISM_Sales_Master_Source_DTO SaveEdit_src(ISM_Sales_Master_Source_DTO dto)
        {
            return COMMS.POSTData(dto, "Sales_Lead_Facade/SaveEdit_src/");
        }
        public ISM_Sales_Master_Source_DTO Edit_details_src(ISM_Sales_Master_Source_DTO dto)
        {
            return COMMS.POSTData(dto, "Sales_Lead_Facade/Edit_details_src/");
        }
        public ISM_Sales_Master_Source_DTO deactivate_src(ISM_Sales_Master_Source_DTO dto)
        {
            return COMMS.POSTData(dto, "Sales_Lead_Facade/deactivate_src/");
        }
        //===================Status===================
              public ISM_Sales_Master_Status_DTO get_load_sts(int id)
        {
            return COMMSTS.GetDataById(id, "Sales_Lead_Facade/get_load_sts/");
        }
        public ISM_Sales_Master_Status_DTO SaveEdit_sts(ISM_Sales_Master_Status_DTO dto)
        {
            return COMMSTS.POSTData(dto, "Sales_Lead_Facade/SaveEdit_sts/");
        }
        public ISM_Sales_Master_Status_DTO Edit_details_sts(ISM_Sales_Master_Status_DTO dto)
        {
            return COMMSTS.POSTData(dto, "Sales_Lead_Facade/Edit_details_sts/");
        }
        public ISM_Sales_Master_Status_DTO deactivate_sts(ISM_Sales_Master_Status_DTO dto)
        {
            return COMMSTS.POSTData(dto, "Sales_Lead_Facade/deactivate_sts/");
        }
        //======================Lead====================

        public ISM_Sales_Lead_DTO load_all_lead(ISM_Sales_Lead_DTO dto)
        {
            return COMML.POSTData(dto, "Sales_Lead_Facade/load_all_lead/");
        }
        

        public ISM_Sales_Lead_DTO Save_Edit_SaleLead(ISM_Sales_Lead_DTO dto)
        {
            return COMML.POSTData(dto, "Sales_Lead_Facade/Save_Edit_SaleLead/");
        }
        public ISM_Sales_Lead_DTO Sales_lead_edit(ISM_Sales_Lead_DTO dto)
        {
            return COMML.POSTData(dto, "Sales_Lead_Facade/Sales_lead_edit/");
        }
        public ISM_Sales_Lead_DTO deactiv_prde(ISM_Sales_Lead_DTO dto)
        {
            return COMML.POSTData(dto, "Sales_Lead_Facade/deactiv_prde/");
        }
        public ISM_Sales_Lead_DTO Sales_lead_view(ISM_Sales_Lead_DTO dto)
        {
            return COMML.POSTData(dto, "Sales_Lead_Facade/Sales_lead_view/");
        }
        public ISM_Sales_Lead_DTO select_state(ISM_Sales_Lead_DTO dto)
        {
            return COMML.POSTData(dto, "Sales_Lead_Facade/select_state/");
        }
        public ISM_Sales_Lead_DTO checkemailtemplet(ISM_Sales_Lead_DTO dto)
        {
            return COMML.POSTData(dto, "Sales_Lead_Facade/checkemailtemplet/");
        }
        //===============Lead Demo===================

        public ISM_Sales_Lead_Demo_DTO get_load_lead_demo(ISM_Sales_Lead_Demo_DTO dto)
        {
            return COMMLD.POSTData(dto, "Sales_Lead_Facade/get_load_lead_demo/");
        }
        public ISM_Sales_Lead_Demo_DTO Edit_details_lead_demo(ISM_Sales_Lead_Demo_DTO dto)
        {
            return COMMLD.POSTData(dto, "Sales_Lead_Facade/Edit_details_lead_demo/");
        }
        public ISM_Sales_Lead_Demo_DTO Edit_response_lead_demo(ISM_Sales_Lead_Demo_DTO dto)
        {
            return COMMLD.POSTData(dto, "Sales_Lead_Facade/Edit_response_lead_demo/");
        }
        public ISM_Sales_Lead_Demo_DTO SaveEdit_lead_demo(ISM_Sales_Lead_Demo_DTO dto)
        {
            return COMMLD.POSTData(dto, "Sales_Lead_Facade/SaveEdit_lead_demo/");
        }
        public ISM_Sales_Lead_Demo_DTO Save_response_lead_demo(ISM_Sales_Lead_Demo_DTO dto)
        {
            return COMMLD.POSTData(dto, "Sales_Lead_Facade/Save_response_lead_demo/");
        }
        public ISM_Sales_Lead_Demo_DTO view_lead_demo(ISM_Sales_Lead_Demo_DTO dto)
        {
            return COMMLD.POSTData(dto, "Sales_Lead_Facade/view_lead_demo/");
        }
    }
}
