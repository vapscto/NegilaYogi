using CommonLibrary;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.University
{
    public class NAAC_HSU_Course_StaffMapping_122Delegate
    {
        CommonDelegate<NAAC_HSU_Course_StaffMapping_122DTO, NAAC_HSU_Course_StaffMapping_122DTO> comm = new CommonDelegate<NAAC_HSU_Course_StaffMapping_122DTO, NAAC_HSU_Course_StaffMapping_122DTO>();

        public NAAC_HSU_Course_StaffMapping_122DTO loaddata(NAAC_HSU_Course_StaffMapping_122DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_HSU_Course_StaffMapping_122Facade/loaddata");
        }
        public NAAC_HSU_Course_StaffMapping_122DTO save(NAAC_HSU_Course_StaffMapping_122DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_HSU_Course_StaffMapping_122Facade/save");
        }
        public NAAC_HSU_Course_StaffMapping_122DTO deactive(NAAC_HSU_Course_StaffMapping_122DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_HSU_Course_StaffMapping_122Facade/deactive");
        }
        public NAAC_HSU_Course_StaffMapping_122DTO EditData(NAAC_HSU_Course_StaffMapping_122DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_HSU_Course_StaffMapping_122Facade/EditData");
        }
        public NAAC_HSU_Course_StaffMapping_122DTO viewuploadflies(NAAC_HSU_Course_StaffMapping_122DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_HSU_Course_StaffMapping_122Facade/viewuploadflies");
        }
        public NAAC_HSU_Course_StaffMapping_122DTO deleteuploadfile(NAAC_HSU_Course_StaffMapping_122DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_HSU_Course_StaffMapping_122Facade/deleteuploadfile");
        }
        public NAAC_HSU_Course_StaffMapping_122DTO get_course(NAAC_HSU_Course_StaffMapping_122DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_HSU_Course_StaffMapping_122Facade/get_course");
        }
        public NAAC_HSU_Course_StaffMapping_122DTO get_designation(NAAC_HSU_Course_StaffMapping_122DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_HSU_Course_StaffMapping_122Facade/get_designation");
        }
        public NAAC_HSU_Course_StaffMapping_122DTO get_employee(NAAC_HSU_Course_StaffMapping_122DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_HSU_Course_StaffMapping_122Facade/get_employee");
        }
    }
}
