using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.admission
{
    public class ClassTeacherMappingDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ClassTeacherMappingDTO, ClassTeacherMappingDTO> COMMM = new CommonDelegate<ClassTeacherMappingDTO, ClassTeacherMappingDTO>();
        public ClassTeacherMappingDTO getdetails (int id)
        {          
            return COMMM.GetDataByIdADM(id, "ClassTeacherMappingFacade/getdetails/");            
        }
        public ClassTeacherMappingDTO save(ClassTeacherMappingDTO data)
        {
            return COMMM.POSTDataADM(data, "ClassTeacherMappingFacade/save/");
        }
        public ClassTeacherMappingDTO GetSelectedRowDetails(ClassTeacherMappingDTO data)
        {
            return COMMM.POSTDataADM(data, "ClassTeacherMappingFacade/GetSelectedRowDetails/");
        }
        public ClassTeacherMappingDTO onchangestaff1(ClassTeacherMappingDTO data)
        {
            return COMMM.POSTDataADM(data, "ClassTeacherMappingFacade/onchangestaff1/");
        }
        public ClassTeacherMappingDTO onchangestaff2(ClassTeacherMappingDTO data)
        {
            return COMMM.POSTDataADM(data, "ClassTeacherMappingFacade/onchangestaff2/");
        }
        public ClassTeacherMappingDTO exchangesave(ClassTeacherMappingDTO data)
        {
            return COMMM.POSTDataADM(data, "ClassTeacherMappingFacade/exchangesave/");
        }
        public ClassTeacherMappingDTO deleterecord (ClassTeacherMappingDTO data)
        {
            return COMMM.POSTDataADM(data, "ClassTeacherMappingFacade/deleterecord/");
        }
    }
}
