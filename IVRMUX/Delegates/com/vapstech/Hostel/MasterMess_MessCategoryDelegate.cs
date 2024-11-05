using CommonLibrary;
using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Hostel
{
    public class MasterMess_MessCategoryDelegate
    {
        CommonDelegate<MasterMess_MessCategoryDTO, MasterMess_MessCategoryDTO> COMMmess = new CommonDelegate<MasterMess_MessCategoryDTO, MasterMess_MessCategoryDTO>();
        CommonDelegate<HL_Master_Mess_DTO, HL_Master_Mess_DTO> COMM = new CommonDelegate<HL_Master_Mess_DTO, HL_Master_Mess_DTO>();
        public MasterMess_MessCategoryDTO get_Mmessdata(MasterMess_MessCategoryDTO data)
        {
            return COMMmess.Post_Hostel(data, "MasterMess_MessCategoryFacade/get_Mmessdata/");
        }
        public HL_Master_Mess_DTO save_Mmessdata(HL_Master_Mess_DTO data)
        {
            return COMM.Post_Hostel(data, "MasterMess_MessCategoryFacade/save_Mmessdata/");
        }
        public MasterMess_MessCategoryDTO edit_Mmessdata(MasterMess_MessCategoryDTO data)
        {
            return COMMmess.Post_Hostel(data, "MasterMess_MessCategoryFacade/edit_Mmessdata/");
        }
        public MasterMess_MessCategoryDTO deactiveY_Mmessdata(MasterMess_MessCategoryDTO data)
        {
            return COMMmess.Post_Hostel(data, "MasterMess_MessCategoryFacade/deactiveY_Mmessdata/");
        }
        //
    }
}
