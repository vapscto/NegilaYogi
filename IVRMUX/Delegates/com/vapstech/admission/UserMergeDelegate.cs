using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.admission
{
    public class UserMergeDelegate
    {
        CommonDelegate<UserMergeDTO, UserMergeDTO> _comm = new CommonDelegate<UserMergeDTO, UserMergeDTO>();

        public UserMergeDTO getalldetails(UserMergeDTO data)
        {
            return _comm.POSTDataADM(data, "UserMergeFacade/getalldetails");
        }
        public UserMergeDTO onstudentnamechange(UserMergeDTO data)
        {
            return _comm.POSTDataADM(data, "UserMergeFacade/onstudentnamechange");
        }
        public UserMergeDTO mergeuserdetails(UserMergeDTO data)
        {
            return _comm.POSTDataADM(data, "UserMergeFacade/mergeuserdetails");
        }
    }
}
