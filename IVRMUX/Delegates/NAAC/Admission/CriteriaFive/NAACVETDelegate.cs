using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class NAACVETDelegate
    {
        CommonDelegate<NAACVETDTO, NAACVETDTO> comm = new CommonDelegate<NAACVETDTO, NAACVETDTO>();
        public NAACVETDTO loaddata(NAACVETDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACVETFacade/loaddata");
        }
        public NAACVETDTO save(NAACVETDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACVETFacade/save");
        }
        public NAACVETDTO deactiveStudent(NAACVETDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACVETFacade/deactiveStudent");
        }

        public NAACVETDTO EditData(NAACVETDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACVETFacade/EditData");
        }
        public NAACVETDTO viewuploadflies(NAACVETDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACVETFacade/viewuploadflies");
        }
        public NAACVETDTO deleteuploadfile(NAACVETDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACVETFacade/deleteuploadfile");
        }

        public NAACVETDTO savemedicaldatawisecomments(NAACVETDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACVETFacade/savemedicaldatawisecomments");
        }
        public NAACVETDTO getcomment(NAACVETDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACVETFacade/getcomment");
        }
        public NAACVETDTO getfilecomment(NAACVETDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACVETFacade/getfilecomment");
        }
        public NAACVETDTO savefilewisecomments(NAACVETDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACVETFacade/savefilewisecomments");
        }
    }
}
