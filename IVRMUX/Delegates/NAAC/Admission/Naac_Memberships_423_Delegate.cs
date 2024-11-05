using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class Naac_Memberships_423_Delegate
    {

        CommonDelegate<Naac_Memberships_423_DTO, Naac_Memberships_423_DTO> COMMM = new CommonDelegate<Naac_Memberships_423_DTO, Naac_Memberships_423_DTO>();
        public Naac_Memberships_423_DTO deactiveStudent(Naac_Memberships_423_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacMemberships423Facade/deactiveStudent");
        }
        public Naac_Memberships_423_DTO save(Naac_Memberships_423_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacMemberships423Facade/save");

        }
        public Naac_Memberships_423_DTO loaddata(Naac_Memberships_423_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacMemberships423Facade/loaddata");

        }
        public Naac_Memberships_423_DTO EditData(Naac_Memberships_423_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacMemberships423Facade/EditData");

        }
        public Naac_Memberships_423_DTO getcomment(Naac_Memberships_423_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacMemberships423Facade/getcomment");

        }
        public Naac_Memberships_423_DTO getfilecomment(Naac_Memberships_423_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacMemberships423Facade/getfilecomment");

        }
        public Naac_Memberships_423_DTO savemedicaldatawisecomments(Naac_Memberships_423_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacMemberships423Facade/savemedicaldatawisecomments");

        }
        public Naac_Memberships_423_DTO savefilewisecomments(Naac_Memberships_423_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacMemberships423Facade/savefilewisecomments");

        }
        public Naac_Memberships_423_DTO viewuploadflies(Naac_Memberships_423_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacMemberships423Facade/viewuploadflies");
        }
        public Naac_Memberships_423_DTO deleteuploadfile(Naac_Memberships_423_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacMemberships423Facade/deleteuploadfile");
        }
    }
}
