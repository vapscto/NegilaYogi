using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class NaacBudget_414_Delegate
    {
        CommonDelegate<NaacBudget_414_DTO, NaacBudget_414_DTO> comm = new CommonDelegate<NaacBudget_414_DTO, NaacBudget_414_DTO>();

        public NaacBudget_414_DTO loaddata(NaacBudget_414_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacBudget414Facade/loaddata");
        }
        public NaacBudget_414_DTO save(NaacBudget_414_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacBudget414Facade/save");
        }
        public NaacBudget_414_DTO getcomment(NaacBudget_414_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacBudget414Facade/getcomment");
        }
        public NaacBudget_414_DTO savefilewisecomments(NaacBudget_414_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacBudget414Facade/savefilewisecomments");
        }
        public NaacBudget_414_DTO getfilecomment(NaacBudget_414_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacBudget414Facade/getfilecomment");
        }
        public NaacBudget_414_DTO savemedicaldatawisecomments(NaacBudget_414_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacBudget414Facade/savemedicaldatawisecomments");
        }
        public NaacBudget_414_DTO EditData(NaacBudget_414_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacBudget414Facade/EditData");
        }
        public NaacBudget_414_DTO deactiveStudent(NaacBudget_414_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacBudget414Facade/deactiveStudent");
        }
        public NaacBudget_414_DTO viewuploadflies(NaacBudget_414_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacBudget414Facade/viewuploadflies");
        }
        public NaacBudget_414_DTO deleteuploadfile(NaacBudget_414_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacBudget414Facade/deleteuploadfile");
        }
        
    }
}
