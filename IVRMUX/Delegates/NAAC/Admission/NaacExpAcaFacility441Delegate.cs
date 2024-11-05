using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class NaacExpAcaFacility441Delegate
    {

        CommonDelegate<NaacExpAcaFacility441DTO, NaacExpAcaFacility441DTO> COMMM = new CommonDelegate<NaacExpAcaFacility441DTO, NaacExpAcaFacility441DTO>();
        public NaacExpAcaFacility441DTO loaddata(NaacExpAcaFacility441DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacExpAcaFacility441Facade/loaddata");
        }
        public NaacExpAcaFacility441DTO save(NaacExpAcaFacility441DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacExpAcaFacility441Facade/save");
        }
        public NaacExpAcaFacility441DTO deactiveStudent(NaacExpAcaFacility441DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacExpAcaFacility441Facade/deactiveStudent");
        }
        public NaacExpAcaFacility441DTO getfilecomment(NaacExpAcaFacility441DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacExpAcaFacility441Facade/getfilecomment");
        }
        public NaacExpAcaFacility441DTO getcomment(NaacExpAcaFacility441DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacExpAcaFacility441Facade/getcomment");
        }
        public NaacExpAcaFacility441DTO savefilewisecomments(NaacExpAcaFacility441DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacExpAcaFacility441Facade/savefilewisecomments");
        }
        public NaacExpAcaFacility441DTO savemedicaldatawisecomments(NaacExpAcaFacility441DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacExpAcaFacility441Facade/savemedicaldatawisecomments");
        }
        public NaacExpAcaFacility441DTO EditData(NaacExpAcaFacility441DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacExpAcaFacility441Facade/EditData");
        }
        public NaacExpAcaFacility441DTO viewuploadflies(NaacExpAcaFacility441DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacExpAcaFacility441Facade/viewuploadflies");
        }
        public NaacExpAcaFacility441DTO deleteuploadfile(NaacExpAcaFacility441DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacExpAcaFacility441Facade/deleteuploadfile");
        }
    }
}
