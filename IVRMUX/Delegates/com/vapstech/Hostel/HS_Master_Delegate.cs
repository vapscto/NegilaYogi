using CommonLibrary;
using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Hostel
{
    public class HS_Master_Delegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";      

        CommonDelegate<HR_Master_Floor_DTO, HR_Master_Floor_DTO> COMMF = new CommonDelegate<HR_Master_Floor_DTO, HR_Master_Floor_DTO>();
        CommonDelegate<HR_Master_Room_DTO, HR_Master_Room_DTO> COMMR = new CommonDelegate<HR_Master_Room_DTO, HR_Master_Room_DTO>();
        CommonDelegate<MasterFacility_DTO, MasterFacility_DTO> COMMFAC = new CommonDelegate<MasterFacility_DTO, MasterFacility_DTO>();        
        CommonDelegate<HL_Master_MessCategory_DTO, HL_Master_MessCategory_DTO> COMMMCat = new CommonDelegate<HL_Master_MessCategory_DTO, HL_Master_MessCategory_DTO>();
        CommonDelegate<HL_Master_Mess_DTO, HL_Master_Mess_DTO> COMMmess = new CommonDelegate<HL_Master_Mess_DTO, HL_Master_Mess_DTO>();
        CommonDelegate<HL_Master_MessMenu_DTO, HL_Master_MessMenu_DTO> COMMmessMenu = new CommonDelegate<HL_Master_MessMenu_DTO, HL_Master_MessMenu_DTO>();
        CommonDelegate<HL_Master_Room_Category_DTO, HL_Master_Room_Category_DTO> COMMRoomCat = new CommonDelegate<HL_Master_Room_Category_DTO, HL_Master_Room_Category_DTO>();

        #region ========================= MASTER FLOOR ==================

        public HR_Master_Floor_DTO get_floordata(HR_Master_Floor_DTO data)
        {
            return COMMF.Post_Hostel(data, "HS_MasterFacade/get_floordata/");
        }
        public HR_Master_Floor_DTO save_Floordata(HR_Master_Floor_DTO data)
        {
            return COMMF.Post_Hostel(data, "HS_MasterFacade/save_Floordata/");
        }
        public HR_Master_Floor_DTO edit_floordata(HR_Master_Floor_DTO data)
        {
            return COMMF.Post_Hostel(data, "HS_MasterFacade/edit_floordata/");
        }
        public HR_Master_Floor_DTO deactivate_Floordata(HR_Master_Floor_DTO data)
        {
            return COMMF.Post_Hostel(data, "HS_MasterFacade/deactivate_Floordata/");
        }
        public HR_Master_Floor_DTO get_Mappedfacility(HR_Master_Floor_DTO data)
        {
            return COMMF.Post_Hostel(data, "HS_MasterFacade/get_Mappedfacility/");
        }

        #endregion

        #region ========================= MASTER ROOM ===================
        public HR_Master_Room_DTO get_Roomloaddata(HR_Master_Room_DTO id)
        {
            return COMMR.Post_Hostel(id, "HS_MasterFacade/get_Roomloaddata/");
        }
        public HR_Master_Room_DTO save_Roomdata(HR_Master_Room_DTO dto)
        {
            return COMMR.Post_Hostel(dto, "HS_MasterFacade/save_Roomdata/");
        }

        public HR_Master_Room_DTO deactive_Roomdata(HR_Master_Room_DTO dto)
        {
            return COMMR.Post_Hostel(dto, "HS_MasterFacade/deactive_Roomdata/");
        }
        public HR_Master_Room_DTO edit_Roomdata(HR_Master_Room_DTO id)
        {
            return COMMR.Post_Hostel(id, "HS_MasterFacade/edit_Roomdata/");
        }
        public HR_Master_Room_DTO get_MappedfacilityforRoom(HR_Master_Room_DTO id)
        {
            return COMMR.Post_Hostel(id, "HS_MasterFacade/get_MappedfacilityforRoom/");
        }
        public HR_Master_Room_DTO floor(HR_Master_Room_DTO dto)
        {
            return COMMR.Post_Hostel(dto, "HS_MasterFacade/floor/");
        }
        public HR_Master_Room_DTO floordetails(HR_Master_Room_DTO dto)
        {
            return COMMR.Post_Hostel(dto, "HS_MasterFacade/floordetails/");
        }
        public HR_Master_Room_DTO categorydetails(HR_Master_Room_DTO dto)
        {
            return COMMR.Post_Hostel(dto, "HS_MasterFacade/categorydetails/");
        }
        

        #endregion 

        #region ========================= MASTER FACILITY ===============
        public MasterFacility_DTO get_facilitydata(MasterFacility_DTO data)
        {
            return COMMFAC.Post_Hostel(data, "HS_MasterFacade/get_facilitydata/");
        }
        public MasterFacility_DTO save_facilitydata(MasterFacility_DTO data)
        {
            return COMMFAC.Post_Hostel(data, "HS_MasterFacade/save_facilitydata/");
        }
        public MasterFacility_DTO edit_faclitydata(MasterFacility_DTO data)
        {
            return COMMFAC.Post_Hostel(data, "HS_MasterFacade/edit_faclitydata/");
        }
        public MasterFacility_DTO deactiveY_faclitydata(MasterFacility_DTO data)
        {
            return COMMFAC.Post_Hostel(data, "HS_MasterFacade/deactiveY_faclitydata/");
        }
        #endregion

        #region ========================= MASTER MESS CATEGORY ==========
        public HL_Master_MessCategory_DTO get_messCategorydata(HL_Master_MessCategory_DTO data)
        {
            return COMMMCat.Post_Hostel(data, "HS_MasterFacade/get_messCategorydata/");
        }
        public HL_Master_MessCategory_DTO save_messCategorydata(HL_Master_MessCategory_DTO data)
        {
            return COMMMCat.Post_Hostel(data, "HS_MasterFacade/save_messCategorydata/");
        }       
        public HL_Master_MessCategory_DTO deactiveY_messCategorydata(HL_Master_MessCategory_DTO data)
        {
            return COMMMCat.Post_Hostel(data, "HS_MasterFacade/deactiveY_messCategorydata/");
        }
        #endregion

        #region ========================= Master MESS ===================
        public HL_Master_Mess_DTO get_Mmessdata(HL_Master_Mess_DTO data)
        {
            return COMMmess.Post_Hostel(data, "HS_MasterFacade/get_Mmessdata/");
        }
        public HL_Master_Mess_DTO save_Mmessdata(HL_Master_Mess_DTO data)
        {
            return COMMmess.Post_Hostel(data, "HS_MasterFacade/save_Mmessdata/");
        }       
        public HL_Master_Mess_DTO edit_Mmessdata(HL_Master_Mess_DTO data)
        {
            return COMMmess.Post_Hostel(data, "HS_MasterFacade/edit_Mmessdata/");
        }
        public HL_Master_Mess_DTO deactiveY_Mmessdata(HL_Master_Mess_DTO data)
        {
            return COMMmess.Post_Hostel(data, "HS_MasterFacade/deactiveY_Mmessdata/");
        }

        #endregion

        #region ========================= MESS MENU =====================
        public HL_Master_MessMenu_DTO get_MessMenudata(HL_Master_MessMenu_DTO data)
        {
            return COMMmessMenu.Post_Hostel(data, "HS_MasterFacade/get_MessMenudata/");
        }
        public HL_Master_MessMenu_DTO save_MessMenudata(HL_Master_MessMenu_DTO data)
        {
            return COMMmessMenu.Post_Hostel(data, "HS_MasterFacade/save_MessMenudata/");
        }
        public HL_Master_MessMenu_DTO edit_MessMenudata(HL_Master_MessMenu_DTO data)
        {
            return COMMmessMenu.Post_Hostel(data, "HS_MasterFacade/edit_MessMenudata/");
        }
        public HL_Master_MessMenu_DTO deactiveY_MessMenudata(HL_Master_MessMenu_DTO data)
        {
            return COMMmessMenu.Post_Hostel(data, "HS_MasterFacade/deactiveY_MessMenudata/");
        }

        #endregion

        #region ========================= ROOM CATEGORY =================
        public HL_Master_Room_Category_DTO get_roomcatdata(HL_Master_Room_Category_DTO data)
        {
            return COMMRoomCat.Post_Hostel(data, "HS_MasterFacade/get_roomcatdata/");
        }
        public HL_Master_Room_Category_DTO save_roomcatdata(HL_Master_Room_Category_DTO data)
        {
            return COMMRoomCat.Post_Hostel(data, "HS_MasterFacade/save_roomcatdata/");
        }
        public HL_Master_Room_Category_DTO edit_roomcatdata(HL_Master_Room_Category_DTO data)
        {
            return COMMRoomCat.Post_Hostel(data, "HS_MasterFacade/edit_roomcatdata/");
        }
        public HL_Master_Room_Category_DTO deactiveY_roomcatdata(HL_Master_Room_Category_DTO data)
        {
            return COMMRoomCat.Post_Hostel(data, "HS_MasterFacade/deactiveY_roomcatdata/");
        }

        #endregion

    }
}

