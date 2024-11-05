using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HostelServiceHub.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Hostel;


namespace HostelServiceHub.Facade
{
    [Route("api/[controller]")]
    public class HS_MasterFacadeController : Controller
    {
        public HS_Master_Interface _Interface;


        public HS_MasterFacadeController(HS_Master_Interface parameter)
        {
            _Interface = parameter;
        }

        #region =================== MASTER FLOOR =======================
        [Route("get_floordata")]
        public HR_Master_Floor_DTO get_floordata([FromBody]HR_Master_Floor_DTO id)
        {
            return _Interface.get_floordata(id);
        }

        [Route("save_Floordata")]
        public HR_Master_Floor_DTO save_Floordata([FromBody] HR_Master_Floor_DTO data)
        {
            return _Interface.save_Floordata(data);
        }

        [Route("edit_floordata")]
        public HR_Master_Floor_DTO edit_floordata([FromBody] HR_Master_Floor_DTO data)
        {
            return _Interface.edit_floordata(data);
        }

        [Route("deactivate_Floordata")]
        public HR_Master_Floor_DTO deactivate_Floordata([FromBody] HR_Master_Floor_DTO data)
        {
            return _Interface.deactivate_Floordata(data);
        }

        [Route("get_Mappedfacility")]
        public HR_Master_Floor_DTO get_Mappedfacility([FromBody] HR_Master_Floor_DTO data)
        {
            return _Interface.get_Mappedfacility(data);
        }

        #endregion

        #region ==================== MASTER ROOM =======================
        [Route("get_Roomloaddata")]
        public HR_Master_Room_DTO get_Roomloaddata([FromBody] HR_Master_Room_DTO data)
        {
            return _Interface.get_Roomloaddata(data);
        }

        [Route("save_Roomdata")]
        public HR_Master_Room_DTO save_Roomdata([FromBody] HR_Master_Room_DTO data)
        {
            return _Interface.save_Roomdata(data);
        }

        [Route("deactive_Roomdata")]
        public HR_Master_Room_DTO deactivate_R([FromBody] HR_Master_Room_DTO data)
        {
            return _Interface.deactive_Roomdata(data);
        }

        [Route("edit_Roomdata")]
        public HR_Master_Room_DTO edit_Roomdata([FromBody] HR_Master_Room_DTO data)
        {
            return _Interface.edit_Roomdata(data);
        }

        [Route("get_MappedfacilityforRoom")]
        public HR_Master_Room_DTO get_MappedfacilityforRoom([FromBody] HR_Master_Room_DTO data)
        {
            return _Interface.get_MappedfacilityforRoom(data);
        }
        [Route("floor")]
        public HR_Master_Room_DTO floor([FromBody] HR_Master_Room_DTO data)
        {
            return _Interface.floor(data);
        }
        [Route("floordetails")]
        public HR_Master_Room_DTO floordetails([FromBody] HR_Master_Room_DTO data)
        {
            return _Interface.floordetails(data);
        }
        [Route("categorydetails")]
        public HR_Master_Room_DTO categorydetails([FromBody] HR_Master_Room_DTO data)
        {
            return _Interface.categorydetails(data);
        }
        
        #endregion 

        #region ==================== MASTER FACILITY ===================
        [Route("get_facilitydata")]
        public MasterFacility_DTO get_facilitydata([FromBody] MasterFacility_DTO data)
        {
            return _Interface.get_facilitydata(data);
        }

        [Route("save_facilitydata")]
        public MasterFacility_DTO save_facilitydata([FromBody] MasterFacility_DTO data)
        {
            return _Interface.save_facilitydata(data);
        }

        [Route("edit_faclitydata")]
        public MasterFacility_DTO edit_faclitydata([FromBody] MasterFacility_DTO data)
        {
            return _Interface.edit_faclitydata(data);
        }

        [Route("deactiveY_faclitydata")]
        public MasterFacility_DTO deactiveY_faclitydata([FromBody] MasterFacility_DTO data)
        {
            return _Interface.deactiveY_faclitydata(data);
        }

        #endregion

        #region ==================== MASTER MESS CATEGORY ==============
        [Route("get_messCategorydata")]
        public HL_Master_MessCategory_DTO get_messCategorydata([FromBody]HL_Master_MessCategory_DTO data)
        {            
            return _Interface.get_messCategorydata(data);
        }

        [Route("save_messCategorydata")]
        public HL_Master_MessCategory_DTO save_messCategorydata([FromBody]HL_Master_MessCategory_DTO data)
        {
            return _Interface.save_messCategorydata(data);
        }

        [Route("deactiveY_messCategorydata")]
        public HL_Master_MessCategory_DTO deactiveY_messCategorydata([FromBody]HL_Master_MessCategory_DTO data)
        {
            return _Interface.deactiveY_messCategorydata(data);
        }
        #endregion

        #region ==================== MASTER MESS =======================
        [Route("get_Mmessdata")]
        public HL_Master_Mess_DTO get_Mmessdata([FromBody] HL_Master_Mess_DTO data)
        {
            return _Interface.get_Mmessdata(data);
        }

        [Route("save_Mmessdata")]
        public HL_Master_Mess_DTO save_Mmessdata([FromBody] HL_Master_Mess_DTO data)
        {
            return _Interface.save_Mmessdata(data);
        }

        [Route("edit_Mmessdata")]
        public HL_Master_Mess_DTO edit_Mmessdata([FromBody] HL_Master_Mess_DTO data)
        {
            return _Interface.edit_Mmessdata(data);
        }

        [Route("deactiveY_Mmessdata")]
        public HL_Master_Mess_DTO deactiveY_Mmessdata([FromBody] HL_Master_Mess_DTO data)
        {
            return _Interface.deactiveY_Mmessdata(data);
        }

        #endregion

        #region ==================== MASTER MENU =======================
        [Route("get_MessMenudata")]
        public HL_Master_MessMenu_DTO get_MessMenudata([FromBody] HL_Master_MessMenu_DTO data)
        {
            return _Interface.get_MessMenudata(data);
        }

        [Route("save_MessMenudata")]
        public HL_Master_MessMenu_DTO save_MessMenudata([FromBody] HL_Master_MessMenu_DTO data)
        {
            return _Interface.save_MessMenudata(data);
        }

        [Route("edit_MessMenudata")]
        public HL_Master_MessMenu_DTO edit_MessMenudata([FromBody] HL_Master_MessMenu_DTO data)
        {
            return _Interface.edit_MessMenudata(data);
        }

        [Route("deactiveY_MessMenudata")]
        public HL_Master_MessMenu_DTO deactiveY_MessMenudata([FromBody] HL_Master_MessMenu_DTO data)
        {
            return _Interface.deactiveY_MessMenudata(data);
        }

        #endregion

        #region ==================== ROOM CATEGORY =====================
        [Route("get_roomcatdata")]
        public HL_Master_Room_Category_DTO get_roomcatdata([FromBody] HL_Master_Room_Category_DTO data)
        {
            return _Interface.get_roomcatdata(data);
        }

        [Route("save_roomcatdata")]
        public HL_Master_Room_Category_DTO save_roomcatdata([FromBody] HL_Master_Room_Category_DTO data)
        {
            return _Interface.save_roomcatdata(data);
        }

        [Route("edit_roomcatdata")]
        public HL_Master_Room_Category_DTO edit_roomcatdata([FromBody] HL_Master_Room_Category_DTO data)
        {
            return _Interface.edit_roomcatdata(data);
        }

        [Route("deactiveY_roomcatdata")]
        public HL_Master_Room_Category_DTO deactiveY_roomcatdata([FromBody] HL_Master_Room_Category_DTO data)
        {
            return _Interface.deactiveY_roomcatdata(data);
        }

        #endregion

    }
}