using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library
{
    public class Staff_BookTranasctionDelegate
    {


        CommonDelegate<Staff_BookTranasctionDTO, Staff_BookTranasctionDTO> _commnbranch = new CommonDelegate<Staff_BookTranasctionDTO, Staff_BookTranasctionDTO>();

        public Staff_BookTranasctionDTO getdetails(Staff_BookTranasctionDTO data)
        {
            return _commnbranch.PostLibrary(data, "Staff_BookTranasctionFacade/getdetails/");
        }
        public Staff_BookTranasctionDTO get_Staffdetails(Staff_BookTranasctionDTO data)
        {
            return _commnbranch.PostLibrary(data, "Staff_BookTranasctionFacade/get_Staffdetails/");
        }

        public Staff_BookTranasctionDTO get_bookdetails(Staff_BookTranasctionDTO data)
        {
            return _commnbranch.PostLibrary(data, "Staff_BookTranasctionFacade/get_bookdetails/");
        }
        public Staff_BookTranasctionDTO Savedata(Staff_BookTranasctionDTO data)
        {
            return _commnbranch.PostLibrary(data, "Staff_BookTranasctionFacade/Savedata/");
        }

        public Staff_BookTranasctionDTO renewaldata(Staff_BookTranasctionDTO data)
        {
            return _commnbranch.PostLibrary(data, "Staff_BookTranasctionFacade/renewaldata/");
        }

        public Staff_BookTranasctionDTO Editdata(Staff_BookTranasctionDTO data)
        {
            return _commnbranch.PostLibrary(data, "Staff_BookTranasctionFacade/Editdata/");
        }

        public Staff_BookTranasctionDTO returndata(Staff_BookTranasctionDTO data)
        {
            return _commnbranch.PostLibrary(data, "Staff_BookTranasctionFacade/returndata/");
        }

    }
}
