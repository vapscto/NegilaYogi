using CommonLibrary;
using PreadmissionDTOs.com.vaps.ClubManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.ClubManagement
{
    public class CMS_Master_MemberDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<CMS_MastermemberDTO, CMS_MastermemberDTO> COMMM = new CommonDelegate<CMS_MastermemberDTO, CMS_MastermemberDTO>();

        CommonDelegate<CMS_Master_Member_QualificationDTO, CMS_Master_Member_QualificationDTO> COMMQ = new CommonDelegate<CMS_Master_Member_QualificationDTO, CMS_Master_Member_QualificationDTO>();

        CommonDelegate<CMS_Master_Member_ExperienceDTO, CMS_Master_Member_ExperienceDTO> COMMEX = new CommonDelegate<CMS_Master_Member_ExperienceDTO, CMS_Master_Member_ExperienceDTO>();

        CommonDelegate<CMS_Master_MemberMobileNoDTO, CMS_Master_MemberMobileNoDTO> COMMNUM = new CommonDelegate<CMS_Master_MemberMobileNoDTO, CMS_Master_MemberMobileNoDTO>();
        //CMS_Master_Member_EmailDTO
        CommonDelegate<CMS_Master_Member_EmailDTO, CMS_Master_Member_EmailDTO> COMMEMAIL = new CommonDelegate<CMS_Master_Member_EmailDTO, CMS_Master_Member_EmailDTO>();
        //CMS_MasterMember_DocumentsDTO
        CommonDelegate<CMS_MasterMember_DocumentsDTO, CMS_MasterMember_DocumentsDTO> COMMEDOC = new CommonDelegate<CMS_MasterMember_DocumentsDTO, CMS_MasterMember_DocumentsDTO>();

        public CMS_MastermemberDTO loaddata(int id)
        {
            return COMMM.GetDataByClubManagement(id, "CMS_Master_MemberFacade/loaddata/");
        }
        //POSTDataClubManagement
        public CMS_MastermemberDTO savedetail1(CMS_MastermemberDTO data)
        {
            return COMMM.POSTDataClubManagement(data, "CMS_Master_MemberFacade/savedetail1/");
        }
        //deactive
        public CMS_MastermemberDTO deactive(CMS_MastermemberDTO data)
        {
            return COMMM.POSTDataClubManagement(data, "CMS_Master_MemberFacade/deactive/");
        }
        //editmember
        public CMS_MastermemberDTO editmember(CMS_MastermemberDTO data)
        {
            return COMMM.POSTDataClubManagement(data, "CMS_Master_MemberFacade/editmember/");
        }
        //CMS_Master_Member_QualificationDTO
        public CMS_Master_Member_QualificationDTO savedetail2(CMS_Master_Member_QualificationDTO data)
        {
            return COMMQ.POSTDataClubManagement(data, "CMS_Master_MemberFacade/savedetail2/");
        }
        //CMS_Master_Member_ExperienceDTO
        public CMS_Master_Member_ExperienceDTO savedetail3(CMS_Master_Member_ExperienceDTO data)
        {
            return COMMEX.POSTDataClubManagement(data, "CMS_Master_MemberFacade/savedetail3/");
        }
        //CMS_Master_MemberMobileNoDTO
        public CMS_Master_MemberMobileNoDTO savedetail5(CMS_Master_MemberMobileNoDTO data)
        {
            return COMMNUM.POSTDataClubManagement(data, "CMS_Master_MemberFacade/savedetail5/");
        }
        //CMS_Master_Member_EmailDTO
        public CMS_Master_Member_EmailDTO savedetail6(CMS_Master_Member_EmailDTO data)
        {
            return COMMEMAIL.POSTDataClubManagement(data, "CMS_Master_MemberFacade/savedetail6/");
        }
        //savedetail7
        public CMS_MasterMember_DocumentsDTO savedetail7(CMS_MasterMember_DocumentsDTO data)
        {
            return COMMEDOC.POSTDataClubManagement(data, "CMS_Master_MemberFacade/savedetail7/");
        }
    }
}
