using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

using Newtonsoft.Json;
using CommonLibrary;
namespace corewebapi18072016.Delegates
{
    public class School_M_ClassDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<School_M_ClassDTO, School_M_ClassDTO> COMMM = new CommonDelegate<School_M_ClassDTO, School_M_ClassDTO>();
        public School_M_ClassDTO saveSchool_M_Classdetails(School_M_ClassDTO instute)
        {
            return COMMM.POSTData(instute, "School_M_ClassFacade/");

        }

        public School_M_ClassDTO getSchool_M_Classdata(School_M_ClassDTO id)
        {
            School_M_ClassDTO dto = null;
            return COMMM.POSTData(id, "School_M_ClassFacade/getalldetails");

        }

        //by id

        public School_M_ClassDTO getSchool_M_ClassDetailsbySchool_M_ClassId(int School_M_ClassId)
        {

            return COMMM.GetDataById(School_M_ClassId, "School_M_ClassFacade/getdetailsById/");

        }

        //delete record
        public School_M_ClassDTO deleterec(School_M_ClassDTO id)
        {

            return COMMM.POSTData(id, "School_M_ClassFacade/deletedetails/");

         
        }

        public School_M_ClassDTO searchByColumn(School_M_ClassDTO dto)
        {
            return COMMM.POSTData(dto, "School_M_ClassFacade/searchByColumn");

        }
    }
}
