using CommonLibrary;
using PreadmissionDTOs.com.vaps.VidyaBharathi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VidyaBharathi
{
    public class VBSC_MasterCompetition_CategoryDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<VBSC_MasterCompetition_CategoryDTO, VBSC_MasterCompetition_CategoryDTO> COMMC = new CommonDelegate<VBSC_MasterCompetition_CategoryDTO, VBSC_MasterCompetition_CategoryDTO>();
        CommonDelegate<Master_Competition_Category_ClassesDTO, Master_Competition_Category_ClassesDTO> COMMD = new CommonDelegate<Master_Competition_Category_ClassesDTO, Master_Competition_Category_ClassesDTO>();
        CommonDelegate<VBSC_Master_Competition_Category_LevelsDTO, VBSC_Master_Competition_Category_LevelsDTO> COMML = new CommonDelegate<VBSC_Master_Competition_Category_LevelsDTO, VBSC_Master_Competition_Category_LevelsDTO>();
        public VBSC_MasterCompetition_CategoryDTO loaddata(int id)
        {
            return COMMC.GetDataByVidyaBharathi(id, "VBSC_MasterCompetition_CategoryFacade/loaddata/");
        }

       
        public VBSC_MasterCompetition_CategoryDTO savedata(VBSC_MasterCompetition_CategoryDTO data)
        {
            return COMMC.POSTDataVidyaBharathi(data, "VBSC_MasterCompetition_CategoryFacade/savedata/");
        }
        public VBSC_MasterCompetition_CategoryDTO Deactivate(VBSC_MasterCompetition_CategoryDTO data)
        {
            return COMMC.POSTDataVidyaBharathi(data, "VBSC_MasterCompetition_CategoryFacade/Deactivate/");
        }
        //
        //Organsation
        public VBSC_MasterCompetition_CategoryDTO Organsation(VBSC_MasterCompetition_CategoryDTO data)
        {
            return COMMC.POSTDataVidyaBharathi(data, "VBSC_MasterCompetition_CategoryFacade/Organsation/");
        }
        //Master_Competition_Category_ClassesDTO //savedataCl
        public Master_Competition_Category_ClassesDTO savedataCl(Master_Competition_Category_ClassesDTO data)
        {
            return COMMD.POSTDataVidyaBharathi(data, "VBSC_MasterCompetition_CategoryFacade/savedataCl/");
        }
        public Master_Competition_Category_ClassesDTO DeactivateCl(Master_Competition_Category_ClassesDTO data)
        {
            return COMMD.POSTDataVidyaBharathi(data, "VBSC_MasterCompetition_CategoryFacade/DeactivateCl/");
        }
        //categorlevel
        public VBSC_Master_Competition_Category_LevelsDTO getdata(int id)
        {
            return COMML.GetDataByVidyaBharathi(id, "VBSC_MasterCompetition_CategoryFacade/getdata/");
        }
        //savedataVCl
        public VBSC_Master_Competition_Category_LevelsDTO savedataVCl(VBSC_Master_Competition_Category_LevelsDTO data)
        {
            return COMML.POSTDataVidyaBharathi(data, "VBSC_MasterCompetition_CategoryFacade/savedataVCl/");
        }
        public VBSC_Master_Competition_Category_LevelsDTO DeactivateVCl(VBSC_Master_Competition_Category_LevelsDTO data)
        {
            return COMML.POSTDataVidyaBharathi(data, "VBSC_MasterCompetition_CategoryFacade/DeactivateVCl/");
        }
    }
}
