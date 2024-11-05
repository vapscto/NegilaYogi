using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Hostel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Hostel;

namespace IVRMUX.Controllers.com.vapstech.Hostel
{

    [Route("api/[controller]")]
    public class HS_MasterController : Controller
    {
        HS_Master_Delegate TMD = new HS_Master_Delegate();

        #region ================== MATSER FLOOR ===============

        [Route("get_floordata/{id:int}")]
        public HR_Master_Floor_DTO get_floordata(int id)
        {
            HR_Master_Floor_DTO data = new HR_Master_Floor_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return TMD.get_floordata(data);
        }

        [Route("save_Floordata")]
        public HR_Master_Floor_DTO save_Floordata([FromBody] HR_Master_Floor_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.save_Floordata(data);
        }

        [Route("edit_floordata")]
        public HR_Master_Floor_DTO details_F([FromBody]HR_Master_Floor_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return TMD.edit_floordata(data);
        }

        [Route("deactivate_Floordata")]
        public HR_Master_Floor_DTO deactivate_Floordata([FromBody] HR_Master_Floor_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.deactivate_Floordata(data);
        }

        [Route("get_Mappedfacility")]
        public HR_Master_Floor_DTO get_Mappedfacility([FromBody] HR_Master_Floor_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.get_Mappedfacility(data);
        }
        #endregion 

        #region ================== MASTER ROOM ================ 
        [Route("get_Roomloaddata/{id:int}")]
        public HR_Master_Room_DTO get_Roomloaddata(int id)
        {
            HR_Master_Room_DTO data = new HR_Master_Room_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return TMD.get_Roomloaddata(data);
        }

        [Route("save_Roomdata")]
        public HR_Master_Room_DTO save_Roomdata([FromBody] HR_Master_Room_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.save_Roomdata(data);
        }

        [Route("deactive_Roomdata")]
        public HR_Master_Room_DTO deactive_Roomdata([FromBody] HR_Master_Room_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.deactive_Roomdata(data);
        }

        [Route("edit_Roomdata")]
        public HR_Master_Room_DTO edit_Roomdata([FromBody] HR_Master_Room_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return TMD.edit_Roomdata(data);
        }

        [Route("get_MappedfacilityforRoom")]
        public HR_Master_Room_DTO get_MappedfacilityforRoom([FromBody] HR_Master_Room_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return TMD.get_MappedfacilityforRoom(data);
        }

        [Route("floor")]
        public HR_Master_Room_DTO floor([FromBody] HR_Master_Room_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.floor(data);
        }

        [Route("floordetails")]
        public HR_Master_Room_DTO floordetails([FromBody] HR_Master_Room_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.floordetails(data);
        }

        [Route("categorydetails")]
        public HR_Master_Room_DTO categorydetails([FromBody] HR_Master_Room_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.categorydetails(data);
        }
        #endregion

        #region ================== MASTER FACALITY ============
        [Route("get_facilitydata/{id:int}")]
        public MasterFacility_DTO get_facilitydata(int id)
        {
            MasterFacility_DTO data = new MasterFacility_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.get_facilitydata(data);
        }

        [Route("save_facilitydata")]
        public MasterFacility_DTO save_facilitydata([FromBody]MasterFacility_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.save_facilitydata(data);
        }

        [Route("edit_faclitydata")]
        public MasterFacility_DTO edit_faclitydata([FromBody]MasterFacility_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.edit_faclitydata(data);
        }

        [Route("deactiveY_faclitydata")]
        public MasterFacility_DTO deactiveY_faclitydata([FromBody]MasterFacility_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.deactiveY_faclitydata(data);
        }

        #endregion

        #region ================== MASTER MESS CATEGORY =======
        [Route("get_messCategorydata/{id:int}")]
        public HL_Master_MessCategory_DTO get_messCategorydata(int id)
        {
            HL_Master_MessCategory_DTO data = new HL_Master_MessCategory_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.get_messCategorydata(data);
        }

        [Route("save_messCategorydata")]
        public HL_Master_MessCategory_DTO save_messCategorydata([FromBody]HL_Master_MessCategory_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.save_messCategorydata(data);
        }

        [Route("deactiveY_messCategorydata")]
        public HL_Master_MessCategory_DTO deactiveY_messCategorydata([FromBody]HL_Master_MessCategory_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.deactiveY_messCategorydata(data);
        }
        #endregion

        #region ================== MASTER MESS ================
        [Route("get_Mmessdata/{id:int}")]
        public HL_Master_Mess_DTO get_Mmessdata(int id)
        {
            HL_Master_Mess_DTO data = new HL_Master_Mess_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.get_Mmessdata(data);
        }

        [Route("save_Mmessdata")]
        public HL_Master_Mess_DTO save_Mmessdata([FromBody]HL_Master_Mess_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.save_Mmessdata(data);
        }

        [Route("edit_Mmessdata")]
        public HL_Master_Mess_DTO edit_Mmessdata([FromBody]HL_Master_Mess_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.edit_Mmessdata(data);
        }

        [Route("deactiveY_Mmessdata")]
        public HL_Master_Mess_DTO deactiveY_Mmessdata([FromBody]HL_Master_Mess_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.deactiveY_Mmessdata(data);
        }

        #endregion

        #region ================== MESS MENU ==================
        [Route("get_MessMenudata/{id:int}")]
        public HL_Master_MessMenu_DTO get_MessMenudata(int id)
        {
            HL_Master_MessMenu_DTO data = new HL_Master_MessMenu_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.get_MessMenudata(data);
        }

        [Route("save_MessMenudata")]
        public HL_Master_MessMenu_DTO save_MessMenudata([FromBody]HL_Master_MessMenu_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.save_MessMenudata(data);
        }

        [Route("edit_MessMenudata")]
        public HL_Master_MessMenu_DTO edit_MessMenudata([FromBody]HL_Master_MessMenu_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.edit_MessMenudata(data);
        }

        [Route("deactiveY_MessMenudata")]
        public HL_Master_MessMenu_DTO deactiveY_MessMenudata([FromBody]HL_Master_MessMenu_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.deactiveY_MessMenudata(data);
        }

        #endregion

        #region ================== ROOM CATEGORY ==============
        [Route("get_roomcatdata/{id:int}")]
        public HL_Master_Room_Category_DTO get_roomcatdata(int id)
        {
            HL_Master_Room_Category_DTO data = new HL_Master_Room_Category_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.get_roomcatdata(data);
        }

        [Route("save_roomcatdata")]
        public HL_Master_Room_Category_DTO save_roomcatdata([FromBody]HL_Master_Room_Category_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.save_roomcatdata(data);
        }

        [Route("edit_roomcatdata")]
        public HL_Master_Room_Category_DTO edit_roomcatdata([FromBody]HL_Master_Room_Category_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.edit_roomcatdata(data);
        }

        [Route("deactiveY_roomcatdata")]
        public HL_Master_Room_Category_DTO deactiveY_roomcatdata([FromBody]HL_Master_Room_Category_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.deactiveY_roomcatdata(data);
        }

        #endregion

    }
}