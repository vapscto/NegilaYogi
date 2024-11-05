using CommonLibrary;
using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Hostel
{
    public class MasterHostelDelegate
    {
        CommonDelegate<HL_Master_Hostel_DTO, HL_Master_Hostel_DTO> _commnbranch = new CommonDelegate<HL_Master_Hostel_DTO, HL_Master_Hostel_DTO>();

        public HL_Master_Hostel_DTO Page_loaddata(HL_Master_Hostel_DTO data)
        {
            return _commnbranch.Post_Hostel(data, "MasterHostelFacade/Page_loaddata/");
        }
        public HL_Master_Hostel_DTO Get_StateData( HL_Master_Hostel_DTO data)
        {
            return _commnbranch.Post_Hostel(data, "MasterHostelFacade/Get_StateData/");
        }
        public HL_Master_Hostel_DTO Save_Hostel_Data(HL_Master_Hostel_DTO data)
        {
            return _commnbranch.Post_Hostel(data, "MasterHostelFacade/Save_Hostel_Data/");
        }
        public HL_Master_Hostel_DTO Edit_Hostel_Row(HL_Master_Hostel_DTO data)
        {
            return _commnbranch.Post_Hostel(data, "MasterHostelFacade/Edit_Hostel_Row/");
        }
        public HL_Master_Hostel_DTO Deactive_Hostel_Row(HL_Master_Hostel_DTO data)
        {
            return _commnbranch.Post_Hostel(data, "MasterHostelFacade/Deactive_Hostel_Row/");
        }
        public HL_Master_Hostel_DTO Get_MappedFacility(HL_Master_Hostel_DTO data)
        {
            return _commnbranch.Post_Hostel(data, "MasterHostelFacade/Get_MappedFacility/");
        }
        public HL_Master_Hostel_DTO Get_MappedEmpl(HL_Master_Hostel_DTO data)
        {
            return _commnbranch.Post_Hostel(data, "MasterHostelFacade/Get_MappedEmpl/");
        }
        public HL_Master_Hostel_DTO viewuploadflies(HL_Master_Hostel_DTO data)
        {
            return _commnbranch.Post_Hostel(data, "MasterHostelFacade/viewuploadflies/");
        }
        public HL_Master_Hostel_DTO deleteuploadfile(HL_Master_Hostel_DTO data)
        {
            return _commnbranch.Post_Hostel(data, "MasterHostelFacade/deleteuploadfile/");
        }
        
    }
}
