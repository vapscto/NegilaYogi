using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class NaacCommiteeDelegate
    {

        CommonDelegate<NAAC_AC_Committee_DTO, NAAC_AC_Committee_DTO> comm = new CommonDelegate<NAAC_AC_Committee_DTO, NAAC_AC_Committee_DTO>();
        public NAAC_AC_Committee_DTO loaddata(NAAC_AC_Committee_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCommiteeFacade/loaddata");
        }
       public NAAC_AC_Committee_DTO saverecord(NAAC_AC_Committee_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCommiteeFacade/saverecord");
        }
        public NAAC_AC_Committee_DTO get_Designation(NAAC_AC_Committee_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCommiteeFacade/get_Designation");
        }
        public NAAC_AC_Committee_DTO savemedicaldatawisecomments(NAAC_AC_Committee_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCommiteeFacade/savemedicaldatawisecomments");
        }
        public NAAC_AC_Committee_DTO getcomment(NAAC_AC_Committee_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCommiteeFacade/getcomment");
        }
        public NAAC_AC_Committee_DTO getfilecomment(NAAC_AC_Committee_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCommiteeFacade/getfilecomment");
        }
        public NAAC_AC_Committee_DTO savefilewisecomments(NAAC_AC_Committee_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCommiteeFacade/savefilewisecomments");
        }
        public NAAC_AC_Committee_DTO savefilewisecommentsmember(NAAC_AC_Committee_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCommiteeFacade/savefilewisecommentsmember");
        }
        public NAAC_AC_Committee_DTO getfilecommentmember(NAAC_AC_Committee_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCommiteeFacade/getfilecommentmember");
        }
        public NAAC_AC_Committee_DTO get_Employee(NAAC_AC_Committee_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCommiteeFacade/get_Employee");
        }
        public NAAC_AC_Committee_DTO savemedicaldatawisecommentsmember(NAAC_AC_Committee_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCommiteeFacade/savemedicaldatawisecommentsmember");
        }
        public NAAC_AC_Committee_DTO getcommentmember(NAAC_AC_Committee_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCommiteeFacade/getcommentmember");
        }
        public NAAC_AC_Committee_DTO deactiveStudent(NAAC_AC_Committee_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCommiteeFacade/deactiveStudent");
        }
        public NAAC_AC_Committee_DTO EditData(NAAC_AC_Committee_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCommiteeFacade/EditData");
        }
        public NAAC_AC_Committee_DTO get_MappedStaff(NAAC_AC_Committee_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCommiteeFacade/get_MappedStaff");
        }
        public NAAC_AC_Committee_DTO deactive_staff(NAAC_AC_Committee_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCommiteeFacade/deactive_staff");
        }
        public NAAC_AC_Committee_DTO viewdocument_MainActUploadFiles(NAAC_AC_Committee_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCommiteeFacade/viewdocument_MainActUploadFiles");
        }
        public NAAC_AC_Committee_DTO delete_MainActUploadFiles(NAAC_AC_Committee_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCommiteeFacade/delete_MainActUploadFiles");
        }
      
        public NAAC_AC_Committee_DTO viewdocument_StaffActUploadFiles(NAAC_AC_Committee_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCommiteeFacade/viewdocument_StaffActUploadFiles");
        }
        public NAAC_AC_Committee_DTO delete_StaffActUploadFiles(NAAC_AC_Committee_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCommiteeFacade/delete_StaffActUploadFiles");
        }
    }
}
