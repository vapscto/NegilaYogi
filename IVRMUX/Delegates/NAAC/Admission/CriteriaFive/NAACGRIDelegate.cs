using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class NAACGRIDelegate
    {
        CommonDelegate<NAACGRIDTO, NAACGRIDTO> comm = new CommonDelegate<NAACGRIDTO, NAACGRIDTO>();
        public NAACGRIDTO loaddata(NAACGRIDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACGRIFacade/loaddata");
        }
        public NAACGRIDTO loaddatamed(NAACGRIDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACGRIFacade/loaddatamed");
        }
        public NAACGRIDTO save(NAACGRIDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACGRIFacade/save");
        }
        public NAACGRIDTO deactiveStudent(NAACGRIDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACGRIFacade/deactiveStudent");
        }

        public NAACGRIDTO EditData(NAACGRIDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACGRIFacade/EditData");
        }
        public NAACGRIDTO deleteuploadfile(NAACGRIDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACGRIFacade/deleteuploadfile");
        }
        public NAACGRIDTO viewuploadflies(NAACGRIDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACGRIFacade/viewuploadflies");
        }

        public NAACGRIDTO savemedicaldatawisecomments(NAACGRIDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACGRIFacade/savemedicaldatawisecomments");
        }
        public NAACGRIDTO getcomment(NAACGRIDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACGRIFacade/getcomment");
        }
        public NAACGRIDTO getfilecomment(NAACGRIDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACGRIFacade/getfilecomment");
        }
        public NAACGRIDTO savefilewisecomments(NAACGRIDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACGRIFacade/savefilewisecomments");
        }
    }
}
