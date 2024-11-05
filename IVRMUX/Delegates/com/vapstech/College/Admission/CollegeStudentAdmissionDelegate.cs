using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.College.Admission
{
    public class CollegeStudentAdmissionDelegate
    {
        CommonDelegate<AdmMasterCollegeStudentDTO, AdmMasterCollegeStudentDTO> common = new CommonDelegate<AdmMasterCollegeStudentDTO, AdmMasterCollegeStudentDTO>();

        public AdmMasterCollegeStudentDTO Getdetails(AdmMasterCollegeStudentDTO obj)
        {
            return common.clgadmissionbypost(obj, "CollegeStudentAdmissionFacade/Getdetails/");
        }
        public AdmMasterCollegeStudentDTO getCourse(AdmMasterCollegeStudentDTO data)
        {
            return common.clgadmissionbypost(data, "CollegeStudentAdmissionFacade/getCourse/");
        }
        public AdmMasterCollegeStudentDTO getBranch(AdmMasterCollegeStudentDTO dt)
        {
            return common.clgadmissionbypost(dt, "CollegeStudentAdmissionFacade/getBranch/");
        }
        public AdmMasterCollegeStudentDTO getSemester(AdmMasterCollegeStudentDTO dto)
        {
            return common.clgadmissionbypost(dto, "CollegeStudentAdmissionFacade/getSemester/");
        }
        public AdmMasterCollegeStudentDTO getCaste(AdmMasterCollegeStudentDTO dto)
        {
            return common.clgadmissionbypost(dto, "CollegeStudentAdmissionFacade/getCaste/");
        }
        public AdmMasterCollegeStudentDTO getQuotaCategory(AdmMasterCollegeStudentDTO dto)
        {
            return common.clgadmissionbypost(dto, "CollegeStudentAdmissionFacade/getQuotaCategory/");
        }
        public save_firsttab_details saveStudentDetails(save_firsttab_details dto)
        {    
            //return common.clgadmissionbypost(dto, "CollegeStudentAdmissionFacade/saveStudentDetails/");     
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50790/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {

                var myContent = JsonConvert.SerializeObject(dto);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/CollegeStudentAdmissionFacade/saveStudentDetails/", byteContent).Result;


                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    dto = JsonConvert.DeserializeObject<save_firsttab_details>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return dto;
        }
        public AdmMasterCollegeStudentDTO Edit(AdmMasterCollegeStudentDTO data)
        {
            return common.clgadmissionbypost(data, "CollegeStudentAdmissionFacade/Edit/");
        }
        public AdmMasterCollegeStudentDTO checkDuplicate(AdmMasterCollegeStudentDTO data)
        {
            return common.clgadmissionbypost(data, "CollegeStudentAdmissionFacade/checkDuplicate/");
        }
        public AdmMasterCollegeStudentDTO getdpstate(AdmMasterCollegeStudentDTO data)
        {
            return common.clgadmissionbypost(data, "CollegeStudentAdmissionFacade/getdpstate/");
        }
        public AdmMasterCollegeStudentDTO saveAddress(AdmMasterCollegeStudentDTO add)
        {
            return common.clgadmissionbypost(add, "CollegeStudentAdmissionFacade/saveAddress/");
        }
        public AdmMasterCollegeStudentDTO saveParentsDetails(AdmMasterCollegeStudentDTO ParentsData)
        {
            return common.clgadmissionbypost(ParentsData, "CollegeStudentAdmissionFacade/saveParentsDetails/");
        }
        public AdmMasterCollegeStudentDTO StateByCountryName(AdmMasterCollegeStudentDTO country)
        {
            return common.clgadmissionbypost(country, "CollegeStudentAdmissionFacade/StateByCountryName/");
        }
        public AdmMasterCollegeStudentDTO saveOthersDetails(AdmMasterCollegeStudentDTO others)
        {
            return common.clgadmissionbypost(others, "CollegeStudentAdmissionFacade/saveOthersDetails/");
        }
        public AdmMasterCollegeStudentDTO saveDocuments(AdmMasterCollegeStudentDTO docs)
        {
            return common.clgadmissionbypost(docs, "CollegeStudentAdmissionFacade/saveDocuments/");
        }
        public AdmMasterCollegeStudentDTO SearchByColumn(AdmMasterCollegeStudentDTO docs)
        {
            return common.clgadmissionbypost(docs, "CollegeStudentAdmissionFacade/SearchByColumn/");
        }
        public AdmMasterCollegeStudentDTO DeleteEntry(AdmMasterCollegeStudentDTO docs)
        {
            return common.clgadmissionbypost(docs, "CollegeStudentAdmissionFacade/DeleteEntry/");
        }
        public AdmMasterCollegeStudentDTO ViewStudentProfile(AdmMasterCollegeStudentDTO data)
        {
            return common.clgadmissionbypost(data, "CollegeStudentAdmissionFacade/ViewStudentProfile/");
        }

        //master competitive exam
        public AdmMasterCollegeStudentDTO compExamName(AdmMasterCollegeStudentDTO country)
        {
            return common.clgadmissionbypost(country, "CollegeStudentAdmissionFacade/compExamName/");
        }
        //document view
        public AdmMasterCollegeStudentDTO getprintdata(AdmMasterCollegeStudentDTO data)
        {
            return common.clgadmissionbypost(data, "CollegeStudentAdmissionFacade/getprintdata/");
        }

        public AdmMasterCollegeStudentDTO checkbiometriccode(AdmMasterCollegeStudentDTO data)
        {
            return common.clgadmissionbypost(data, "CollegeStudentAdmissionFacade/checkbiometriccode");
        }
        public AdmMasterCollegeStudentDTO checkrfcardduplicate(AdmMasterCollegeStudentDTO data)
        {
            return common.clgadmissionbypost(data, "CollegeStudentAdmissionFacade/checkrfcardduplicate");
        }
    }
}
