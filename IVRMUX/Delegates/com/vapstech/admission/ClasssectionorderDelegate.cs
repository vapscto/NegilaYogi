using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.admission
{
    public class ClasssectionorderDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ClasssectionorderDTO, ClasssectionorderDTO> COMMM = new CommonDelegate<ClasssectionorderDTO, ClasssectionorderDTO>();
        public ClasssectionorderDTO getdetails(int id)
        {
            return COMMM.GetDataByIdADM(id, "ClasssectionorderFacade/getdetails/");
        }
        public ClasssectionorderDTO save(ClasssectionorderDTO data)
        {
            return COMMM.POSTDataADM(data, "ClasssectionorderFacade/save/");
        }
    }
}
