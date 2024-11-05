using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.NAAC;

namespace IVRMUX.Delegates.NAAC
{
    public class NAAC_User_PrivilegesDelegate
    {
        CommonDelegate<NAAC_User_PrivilegesDTO, NAAC_User_PrivilegesDTO> _comm = new CommonDelegate<NAAC_User_PrivilegesDTO, NAAC_User_PrivilegesDTO>();

        public NAAC_User_PrivilegesDTO Getdetails(NAAC_User_PrivilegesDTO data)
        {
            return _comm.naacdetailsbypost(data, "NAAC_User_PrivilegesFacade/Getdetails");
        }
        public NAAC_User_PrivilegesDTO onchangeemployee(NAAC_User_PrivilegesDTO data)
        {
            return _comm.naacdetailsbypost(data, "NAAC_User_PrivilegesFacade/onchangeemployee");
        }
        public NAAC_User_PrivilegesDTO onchangecriteria(NAAC_User_PrivilegesDTO data)
        {
            return _comm.naacdetailsbypost(data, "NAAC_User_PrivilegesFacade/onchangecriteria");
        }
        public NAAC_User_PrivilegesDTO savedata(NAAC_User_PrivilegesDTO data)
        {
            return _comm.naacdetailsbypost(data, "NAAC_User_PrivilegesFacade/savedata");
        }
        public NAAC_User_PrivilegesDTO viewrecord(NAAC_User_PrivilegesDTO data)
        {
            return _comm.naacdetailsbypost(data, "NAAC_User_PrivilegesFacade/viewrecord");
        }
        public NAAC_User_PrivilegesDTO deactivate(NAAC_User_PrivilegesDTO data)
        {
            return _comm.naacdetailsbypost(data, "NAAC_User_PrivilegesFacade/deactivate");
        }

        // Master Naac Criteria
        public NAAC_User_PrivilegesDTO OnChangeInstituionType(NAAC_User_PrivilegesDTO data)
        {
            return _comm.naacdetailsbypost(data, "NAAC_User_PrivilegesFacade/OnChangeInstituionType");
        }
        public NAAC_User_PrivilegesDTO SaveTab1(NAAC_User_PrivilegesDTO data)
        {
            return _comm.naacdetailsbypost(data, "NAAC_User_PrivilegesFacade/SaveTab1");
        }
        public NAAC_User_PrivilegesDTO EditTab1(NAAC_User_PrivilegesDTO data)
        {
            return _comm.naacdetailsbypost(data, "NAAC_User_PrivilegesFacade/EditTab1");
        }
        public NAAC_User_PrivilegesDTO OnChangeCriteriaName(NAAC_User_PrivilegesDTO data)
        {
            return _comm.naacdetailsbypost(data, "NAAC_User_PrivilegesFacade/OnChangeCriteriaName");
        }
        public NAAC_User_PrivilegesDTO SaveTab2(NAAC_User_PrivilegesDTO data)
        {
            return _comm.naacdetailsbypost(data, "NAAC_User_PrivilegesFacade/SaveTab2");
        }
        public NAAC_User_PrivilegesDTO EditTab2(NAAC_User_PrivilegesDTO data)
        {
            return _comm.naacdetailsbypost(data, "NAAC_User_PrivilegesFacade/EditTab2");
        }
        public NAAC_User_PrivilegesDTO SaveTab3(NAAC_User_PrivilegesDTO data)
        {
            return _comm.naacdetailsbypost(data, "NAAC_User_PrivilegesFacade/SaveTab3");
        }
        public NAAC_User_PrivilegesDTO EditTab3(NAAC_User_PrivilegesDTO data)
        {
            return _comm.naacdetailsbypost(data, "NAAC_User_PrivilegesFacade/EditTab3");
        }
        public NAAC_User_PrivilegesDTO OnChangeCriteriaNameLevelOne(NAAC_User_PrivilegesDTO data)
        {
            return _comm.naacdetailsbypost(data, "NAAC_User_PrivilegesFacade/OnChangeCriteriaNameLevelOne");
        }
    }
}
