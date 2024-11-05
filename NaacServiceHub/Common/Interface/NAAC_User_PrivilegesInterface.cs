using PreadmissionDTOs.NAAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Common.Interface
{
    public interface NAAC_User_PrivilegesInterface
    {
        NAAC_User_PrivilegesDTO Getdetails(NAAC_User_PrivilegesDTO data);
        NAAC_User_PrivilegesDTO onchangeemployee(NAAC_User_PrivilegesDTO data);
        NAAC_User_PrivilegesDTO onchangecriteria(NAAC_User_PrivilegesDTO data);
        NAAC_User_PrivilegesDTO savedata(NAAC_User_PrivilegesDTO data);
        NAAC_User_PrivilegesDTO viewrecord(NAAC_User_PrivilegesDTO data);
        NAAC_User_PrivilegesDTO deactivate(NAAC_User_PrivilegesDTO data);

        // Master Naac Criteria
        NAAC_User_PrivilegesDTO OnChangeInstituionType(NAAC_User_PrivilegesDTO data);
        NAAC_User_PrivilegesDTO SaveTab1(NAAC_User_PrivilegesDTO data);
        NAAC_User_PrivilegesDTO EditTab1(NAAC_User_PrivilegesDTO data);
        NAAC_User_PrivilegesDTO OnChangeCriteriaName(NAAC_User_PrivilegesDTO data);
        NAAC_User_PrivilegesDTO SaveTab2(NAAC_User_PrivilegesDTO data);
        NAAC_User_PrivilegesDTO EditTab2(NAAC_User_PrivilegesDTO data);
        NAAC_User_PrivilegesDTO SaveTab3(NAAC_User_PrivilegesDTO data);
        NAAC_User_PrivilegesDTO EditTab3(NAAC_User_PrivilegesDTO data);
        NAAC_User_PrivilegesDTO OnChangeCriteriaNameLevelOne(NAAC_User_PrivilegesDTO data);
    }
}
