using CommonLibrary;
using PreadmissionDTOs.SeatingArrangment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.SeatingArrangment
{
    public class SAMasterSuperintendentDelegate
    {
        CommonDelegate<SAMasterSuperintendent, SAMasterSuperintendent> comm = new CommonDelegate<SAMasterSuperintendent, SAMasterSuperintendent>();

        public SAMasterSuperintendent load_sup(SAMasterSuperintendent data)
        {
            return comm.SeatingArrangmentPOST(data, "SAMasterSuperintendentFacade/load_sup");
        }
        public SAMasterSuperintendent Save_sup(SAMasterSuperintendent data)
        {
            return comm.SeatingArrangmentPOST(data, "SAMasterSuperintendentFacade/Save_sup");
        }
        public SAMasterSuperintendent Edit_sup(SAMasterSuperintendent data)
        {
            return comm.SeatingArrangmentPOST(data, "SAMasterSuperintendentFacade/Edit_sup");
        }
        public SAMasterSuperintendent ActiveDeactive_sup(SAMasterSuperintendent data)
        {
            return comm.SeatingArrangmentPOST(data, "SAMasterSuperintendentFacade/ActiveDeactive_sup");
        }

        //=========Absent Student======================
        public SAMasterSuperintendent load_AS(SAMasterSuperintendent data)
        {
            return comm.SeatingArrangmentPOST(data, "SAMasterSuperintendentFacade/load_AS");
        }
        public SAMasterSuperintendent Save_AS(SAMasterSuperintendent data)
        {
            return comm.SeatingArrangmentPOST(data, "SAMasterSuperintendentFacade/Save_AS");
        }
        public SAMasterSuperintendent Edit_AS(SAMasterSuperintendent data)
        {
            return comm.SeatingArrangmentPOST(data, "SAMasterSuperintendentFacade/Edit_AS");
        }
        public SAMasterSuperintendent DeleteAbsentStudent(SAMasterSuperintendent data)
        {
            return comm.SeatingArrangmentPOST(data, "SAMasterSuperintendentFacade/DeleteAbsentStudent");
        }      
        
        //=========Malpractice Student======================
        public SAMasterSuperintendent load_MPS(SAMasterSuperintendent data)
        {
            return comm.SeatingArrangmentPOST(data, "SAMasterSuperintendentFacade/load_MPS");
        }
        public SAMasterSuperintendent Save_MPS(SAMasterSuperintendent data)
        {
            return comm.SeatingArrangmentPOST(data, "SAMasterSuperintendentFacade/Save_MPS");
        }
        public SAMasterSuperintendent Edit_MPS(SAMasterSuperintendent data)
        {
            return comm.SeatingArrangmentPOST(data, "SAMasterSuperintendentFacade/Edit_MPS");
        }
        public SAMasterSuperintendent DeleteMalPraticeStudent(SAMasterSuperintendent data)
        {
            return comm.SeatingArrangmentPOST(data, "SAMasterSuperintendentFacade/DeleteMalPraticeStudent");
        }
        

        //=========Chief coordinator======================
        public SAMasterSuperintendent load_CC(SAMasterSuperintendent data)
        {
            return comm.SeatingArrangmentPOST(data, "SAMasterSuperintendentFacade/load_CC");
        }
        public SAMasterSuperintendent Save_CC(SAMasterSuperintendent data)
        {
            return comm.SeatingArrangmentPOST(data, "SAMasterSuperintendentFacade/Save_CC");
        }
        public SAMasterSuperintendent Edit_CC(SAMasterSuperintendent data)
        {
            return comm.SeatingArrangmentPOST(data, "SAMasterSuperintendentFacade/Edit_CC");
        }
        public SAMasterSuperintendent ActiveDeactive_CC(SAMasterSuperintendent data)
        {
            return comm.SeatingArrangmentPOST(data, "SAMasterSuperintendentFacade/ActiveDeactive_CC");
        }


        // *************** General Selections ************** //
        public SAMasterSuperintendent GetCourse(SAMasterSuperintendent data)
        {
            return comm.SeatingArrangmentPOST(data, "SAMasterSuperintendentFacade/GetCourse");
        }
        public SAMasterSuperintendent GetBranch(SAMasterSuperintendent data)
        {
            return comm.SeatingArrangmentPOST(data, "SAMasterSuperintendentFacade/GetBranch");
        }
        public SAMasterSuperintendent GetSemester(SAMasterSuperintendent data)
        {
            return comm.SeatingArrangmentPOST(data, "SAMasterSuperintendentFacade/GetSemester");
        }
        public SAMasterSuperintendent GetSubject(SAMasterSuperintendent data)
        {
            return comm.SeatingArrangmentPOST(data, "SAMasterSuperintendentFacade/GetSubject");
        }
        public SAMasterSuperintendent GetStudent(SAMasterSuperintendent data)
        {
            return comm.SeatingArrangmentPOST(data, "SAMasterSuperintendentFacade/GetStudent");
        }
    }
}
