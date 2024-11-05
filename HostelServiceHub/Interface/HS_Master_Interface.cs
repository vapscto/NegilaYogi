
using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HostelServiceHub.Interface
{
    public interface HS_Master_Interface
    {

        #region ===================== MASTER FLOOR =================
        HR_Master_Floor_DTO get_floordata(HR_Master_Floor_DTO id);
        HR_Master_Floor_DTO save_Floordata(HR_Master_Floor_DTO dto);
        HR_Master_Floor_DTO edit_floordata(HR_Master_Floor_DTO dto);
        HR_Master_Floor_DTO deactivate_Floordata(HR_Master_Floor_DTO dto);
        HR_Master_Floor_DTO get_Mappedfacility(HR_Master_Floor_DTO dto);
        #endregion

        #region ===================== MATSER ROOM ==================
        HR_Master_Room_DTO get_Roomloaddata(HR_Master_Room_DTO data);
        HR_Master_Room_DTO save_Roomdata(HR_Master_Room_DTO data);
        HR_Master_Room_DTO deactive_Roomdata(HR_Master_Room_DTO data);
        HR_Master_Room_DTO edit_Roomdata(HR_Master_Room_DTO data);
        HR_Master_Room_DTO get_MappedfacilityforRoom(HR_Master_Room_DTO data);
        HR_Master_Room_DTO floor(HR_Master_Room_DTO data);
        HR_Master_Room_DTO floordetails(HR_Master_Room_DTO data);
        HR_Master_Room_DTO categorydetails(HR_Master_Room_DTO data);

        #endregion

        #region ===================== MASTER FACILITY ==============
        MasterFacility_DTO get_facilitydata(MasterFacility_DTO data);
        MasterFacility_DTO save_facilitydata(MasterFacility_DTO data);
        MasterFacility_DTO edit_faclitydata(MasterFacility_DTO data);
        MasterFacility_DTO deactiveY_faclitydata(MasterFacility_DTO data);
        #endregion

        #region ===================== MASTER FACILITY ============== 
        HL_Master_MessCategory_DTO get_messCategorydata(HL_Master_MessCategory_DTO data);
        HL_Master_MessCategory_DTO save_messCategorydata(HL_Master_MessCategory_DTO data);
        HL_Master_MessCategory_DTO deactiveY_messCategorydata(HL_Master_MessCategory_DTO data);
        #endregion

        #region ===================== MASTER MESS ==================
        HL_Master_Mess_DTO get_Mmessdata(HL_Master_Mess_DTO data);
        HL_Master_Mess_DTO save_Mmessdata(HL_Master_Mess_DTO data);
        HL_Master_Mess_DTO edit_Mmessdata(HL_Master_Mess_DTO data);
        HL_Master_Mess_DTO deactiveY_Mmessdata(HL_Master_Mess_DTO data);
        #endregion

        #region ===================== MASTER MENU ==================
        HL_Master_MessMenu_DTO get_MessMenudata(HL_Master_MessMenu_DTO data);
        HL_Master_MessMenu_DTO save_MessMenudata(HL_Master_MessMenu_DTO data);
        HL_Master_MessMenu_DTO edit_MessMenudata(HL_Master_MessMenu_DTO data);
        HL_Master_MessMenu_DTO deactiveY_MessMenudata(HL_Master_MessMenu_DTO data);
        #endregion

        #region ===================== ROOM CATEGORY ================
        HL_Master_Room_Category_DTO get_roomcatdata(HL_Master_Room_Category_DTO data);
        HL_Master_Room_Category_DTO save_roomcatdata(HL_Master_Room_Category_DTO data);
        HL_Master_Room_Category_DTO edit_roomcatdata(HL_Master_Room_Category_DTO data);
        HL_Master_Room_Category_DTO deactiveY_roomcatdata(HL_Master_Room_Category_DTO data);
        #endregion

    }
}
