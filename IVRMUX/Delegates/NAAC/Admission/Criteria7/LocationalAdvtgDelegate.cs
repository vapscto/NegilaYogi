using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using PreadmissionDTOs.NAAC.Admission.Criteria7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission.Criteria7
{
    public class LocationalAdvtgDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<LocationalAdvtgDTO, LocationalAdvtgDTO> COMMM = new CommonDelegate<LocationalAdvtgDTO, LocationalAdvtgDTO>();

        public LocationalAdvtgDTO loaddata(LocationalAdvtgDTO data)
        {
            return COMMM.naacdetailsbypost(data, "LocationalAdvtgFacade/loaddata/");
        }
        public LocationalAdvtgDTO getdata(LocationalAdvtgDTO data)
        {
            return COMMM.naacdetailsbypost(data, "LocationalAdvtgFacade/getdata/");
        }
        public LocationalAdvtgDTO savedatatab1(LocationalAdvtgDTO data)
        {
            return COMMM.naacdetailsbypost(data, "LocationalAdvtgFacade/savedatatab1/");
        }
        public LocationalAdvtgDTO edittab1(LocationalAdvtgDTO data)
        {
            return COMMM.naacdetailsbypost(data, "LocationalAdvtgFacade/edittab1/");
        }
        public LocationalAdvtgDTO deactivYTab1(LocationalAdvtgDTO data)
        {
            return COMMM.naacdetailsbypost(data, "LocationalAdvtgFacade/deactivYTab1/");
        }

        public LocationalAdvtgDTO deleteuploadfile(LocationalAdvtgDTO data)
        {
            return COMMM.naacdetailsbypost(data, "LocationalAdvtgFacade/deleteuploadfile");
        }
        public LocationalAdvtgDTO viewuploadflies(LocationalAdvtgDTO data)
        {
            return COMMM.naacdetailsbypost(data, "LocationalAdvtgFacade/viewuploadflies");
        }
        public LocationalAdvtgDTO getcomment(LocationalAdvtgDTO data)
        {
            return COMMM.naacdetailsbypost(data, "LocationalAdvtgFacade/getcomment");
        }
        public LocationalAdvtgDTO getfilecomment(LocationalAdvtgDTO data)
        {
            return COMMM.naacdetailsbypost(data, "LocationalAdvtgFacade/getfilecomment");
        }
        public LocationalAdvtgDTO savecomments(LocationalAdvtgDTO data)
        {
            return COMMM.naacdetailsbypost(data, "LocationalAdvtgFacade/savecomments");
        }
        public LocationalAdvtgDTO savefilewisecomments(LocationalAdvtgDTO data)
        {
            return COMMM.naacdetailsbypost(data, "LocationalAdvtgFacade/savefilewisecomments");
        }
    }
}

