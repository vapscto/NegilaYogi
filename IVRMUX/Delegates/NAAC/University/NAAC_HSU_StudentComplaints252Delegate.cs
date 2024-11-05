using CommonLibrary;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.University
{
    public class NAAC_HSU_StudentComplaints252Delegate
    {

        CommonDelegate<NAAC_HSU_StudentComplaints252_DTO, NAAC_HSU_StudentComplaints252_DTO> comm = new CommonDelegate<NAAC_HSU_StudentComplaints252_DTO, NAAC_HSU_StudentComplaints252_DTO>();
        public NAAC_HSU_StudentComplaints252_DTO loaddata(NAAC_HSU_StudentComplaints252_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_HSU_StudentComplaints252Facade/loaddata");

        }
        public NAAC_HSU_StudentComplaints252_DTO save(NAAC_HSU_StudentComplaints252_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_HSU_StudentComplaints252Facade/save");
        }
        public NAAC_HSU_StudentComplaints252_DTO deactive(NAAC_HSU_StudentComplaints252_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_HSU_StudentComplaints252Facade/deactive");
        }
        public NAAC_HSU_StudentComplaints252_DTO EditData(NAAC_HSU_StudentComplaints252_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_HSU_StudentComplaints252Facade/EditData");
        }
        public NAAC_HSU_StudentComplaints252_DTO viewuploadflies(NAAC_HSU_StudentComplaints252_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_HSU_StudentComplaints252Facade/viewuploadflies");
        }
        public NAAC_HSU_StudentComplaints252_DTO deleteuploadfile(NAAC_HSU_StudentComplaints252_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_HSU_StudentComplaints252Facade/deleteuploadfile");
        }

    }
}
