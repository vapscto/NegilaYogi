using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;

namespace HRMSServicesHub.com.vaps.Services
{
    public class EmployeeRegistrationService : Interfaces.EmployeeRegistrationInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        readonly ILogger<EmployeeRegistrationService> _logger;
        public EmployeeRegistrationService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext, ILogger<EmployeeRegistrationService> log)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;
            _logger = log;
        }

        public MasterEmployeeDTO getBasicData(MasterEmployeeDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }
        public MasterEmployeeDTO SaveUpdate(MasterEmployeeDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.TabName.Equals("FirstTab"))
                {
                    AddUpdateEmployeePersonalDetails(dto);
                }
                else if (dto.TabName.Equals("AddressTab"))
                {
                    AddUpdateEmployeeAddressDetails(dto);
                }
                else if (dto.TabName.Equals("QualificationTab"))
                {
                    AddUpdateEmployeeQualificationDetails(dto);
                }
                else if (dto.TabName.Equals("ExperienceTab"))
                {
                    AddUpdateEmployeeExperienceDetails(dto);
                }
                else if (dto.TabName.Equals("DocumentTab"))
                {
                    AddUpdateEmployeeDocumentDetails(dto);
                }
                else if (dto.TabName.Equals("MedicalTab"))
                {
                    AddUpdateEmployeeMedicalDetails(dto);
                }
                else if (dto.TabName.Equals("OtherTab"))
                {
                    AddUpdateEmployeeOtherDetails(dto);
                }
                else if (dto.TabName.Equals("SalaryTab"))
                {
                    //AddUpdateEmployeeEarningDetails(dto);
                    //AddUpdateEmployeeDeductionDetails(dto);
                    //AddUpdateEmployeeArrearDetails(dto);
                    dto.retrunMsg = "Update";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error";
            }
            return dto;
        }

        //Personal Details
        public MasterEmployeeDTO AddUpdateEmployeePersonalDetails(MasterEmployeeDTO dto)
        {

            dto.retrunMsg = "";
            try
            {
                //if (dto.HRME_Id == 0)
                //{
                //    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_EmployeeCode.Equals(dto.HRME_EmployeeCode)).Count();

                //    if (resultCout > 0)
                //    {
                //        dto.retrunMsg = "Duplicate";
                //        return dto;
                //    }
                //}
                //else
                //{
                //    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_EmployeeCode.Equals(dto.HRME_EmployeeCode) && t.HRME_Id == dto.HRME_Id).Count();

                //    if (resultCout > 0)
                //    {
                //        dto.retrunMsg = "Duplicate";
                //        return dto;
                //    }
                //}




                MasterEmployee dmoObj = Mapper.Map<MasterEmployee>(dto.Employeedto);

                if (dmoObj.HRME_Id > 0)
                {
                    var result = _HRMSContext.MasterEmployee.Single(t => t.HRME_Id == dmoObj.HRME_Id);

                    dto.Employeedto.UpdatedDate = DateTime.Now;

                    Mapper.Map(dto.Employeedto, result);

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        // dto.HRME_Id = dmoObj.HRME_Id;
                        dto.retrunMsg = "Update";
                        student_mobile_no(dto);
                        student_email_id(dto);
                        //EmployeeCodeDuplicateCheck(dto);
                        AddUpdateEmployeeBankDetails(dto);
                        Feeconcession(dto);

                        var currentEmp = _HRMSContext.MasterEmployee.Single(t => t.HRME_Id == dmoObj.HRME_Id);
                        Mapper.Map(currentEmp, dto.Employeedto);

                    }
                    else
                    {
                        dto.retrunMsg = "false";
                    }
                }
                else
                {
                    var duplicatecountresult = 0;
                    if (dmoObj.HRME_EmployeeCode != null)
                    {
                        duplicatecountresult = _HRMSContext.MasterEmployee.Where(t => t.MI_Id == dto.MI_Id && t.HRME_EmployeeCode.Equals(dmoObj.HRME_EmployeeCode)).Count();
                    }
                    if (duplicatecountresult == 0)
                    {
                        dmoObj.HRME_ActiveFlag = true;

                        dmoObj.HRME_LeftFlag = false;

                        dmoObj.MI_Id = dto.MI_Id;
                        dmoObj.UpdatedDate = DateTime.Now;
                        dmoObj.CreatedDate = DateTime.Now;


                        var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_EmployeeCode.Equals(dmoObj.HRME_EmployeeCode)).Count();

                        if (resultCout > 0)
                        {
                            dto.retrunMsg = "Duplicate";
                            return dto;
                        }


                        _HRMSContext.Add(dmoObj);
                        var flag = _HRMSContext.SaveChanges();
                        if (flag == 1)
                        {
                            MasterEmployeeDTO DTO = Mapper.Map<MasterEmployeeDTO>(dmoObj);
                            DTO.HRME_EmployeeOrder = Convert.ToInt32(DTO.HRME_Id);
                            var resultd = _HRMSContext.MasterEmployee.Single(t => t.HRME_Id == DTO.HRME_Id);
                            Mapper.Map(DTO, resultd);
                            _HRMSContext.Update(resultd);
                            _HRMSContext.SaveChanges();
                            dto.Employeedto.HRME_Id = dmoObj.HRME_Id;
                            dto.Employeedto.MI_Id = dto.MI_Id;
                            dto.retrunMsg = "Add";

                            student_mobile_no(dto);
                            student_email_id(dto);
                            AddUpdateEmployeeBankDetails(dto);
                            // EmployeeCodeDuplicateCheck(dto);
                            //jesus Client 
                            //Feeconcession(dto);
                           
                            var currentEmp = _HRMSContext.MasterEmployee.Single(t => t.HRME_Id == dmoObj.HRME_Id);
                            Mapper.Map(currentEmp, dto.Employeedto);



                         


                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }
                    }
                    else
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }

                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }


            return dto;
        }


        //Contact Details

        public MasterEmployeeDTO AddUpdateEmployeeAddressDetails(MasterEmployeeDTO dto)
        {

            dto.retrunMsg = "";
            try
            {

                MasterEmployee dmoObj = Mapper.Map<MasterEmployee>(dto.Employeedto);

                if (dmoObj.HRME_Id > 0)
                {
                    var result = _HRMSContext.MasterEmployee.Single(t => t.HRME_Id == dmoObj.HRME_Id);

                    dto.Employeedto.UpdatedDate = DateTime.Now;

                    Mapper.Map(dto.Employeedto, result);

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        dto.retrunMsg = "Update";
                        var currentEmp = _HRMSContext.MasterEmployee.Single(t => t.HRME_Id == dmoObj.HRME_Id);
                        Mapper.Map(currentEmp, dto.Employeedto);

                    }
                    else
                    {
                        dto.retrunMsg = "false";
                    }
                }
                else
                {
                    var duplicatecountresult = 0;
                    if (dmoObj.HRME_EmployeeCode != null)
                    {
                        duplicatecountresult = _HRMSContext.MasterEmployee.Where(t => t.MI_Id == dto.MI_Id && t.HRME_EmployeeCode.Equals(dmoObj.HRME_EmployeeCode)).Count();
                    }
                    if (duplicatecountresult == 0)
                    {
                        dmoObj.HRME_ActiveFlag = true;

                        dmoObj.HRME_LeftFlag = false;

                        dmoObj.MI_Id = dto.MI_Id;
                        dmoObj.UpdatedDate = DateTime.Now;
                        dmoObj.CreatedDate = DateTime.Now;
                        _HRMSContext.Add(dmoObj);
                        var flag = _HRMSContext.SaveChanges();
                        if (flag == 1)
                        {
                            MasterEmployeeDTO DTO = Mapper.Map<MasterEmployeeDTO>(dmoObj);
                            DTO.HRME_EmployeeOrder = Convert.ToInt32(DTO.HRME_Id);
                            var resultd = _HRMSContext.MasterEmployee.Single(t => t.HRME_Id == DTO.HRME_Id);
                            Mapper.Map(DTO, resultd);
                            _HRMSContext.Update(resultd);
                            _HRMSContext.SaveChanges();
                            dto.Employeedto.HRME_Id = dmoObj.HRME_Id;
                            dto.Employeedto.MI_Id = dto.MI_Id;
                            dto.retrunMsg = "Add";
                            var currentEmp = _HRMSContext.MasterEmployee.Single(t => t.HRME_Id == dmoObj.HRME_Id);
                            Mapper.Map(currentEmp, dto.Employeedto);


                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }
                    }
                    else
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }

                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }


            return dto;
        }

        //Qualification details
        public MasterEmployeeDTO AddUpdateEmployeeQualificationDetails(MasterEmployeeDTO dto)
        {
            //add/update Qulaification details
            try
            {
                if (dto.EmployeeQulaificationDTO.Count() > 0)
                {
                    foreach (Master_Employee_QulaificationDTO QulaificationDTO in dto.EmployeeQulaificationDTO)
                    {
                        QulaificationDTO.MI_Id = dto.Employeedto.MI_Id;
                        QulaificationDTO.HRME_Id = dto.Employeedto.HRME_Id;
                        Master_Employee_Qulaification Qulaification = Mapper.Map<Master_Employee_Qulaification>(QulaificationDTO);

                        if (Qulaification.HRMEQ_Id > 0)
                        {
                            var Qulaificationresult = _HRMSContext.Master_Employee_Qulaification.Single(t => t.HRMEQ_Id == QulaificationDTO.HRMEQ_Id);
                            //added by 20/05/2017   
                            Qulaificationresult.UpdatedDate = DateTime.Now;
                            Mapper.Map(QulaificationDTO, Qulaificationresult);
                            _HRMSContext.Update(Qulaificationresult);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Update";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }

                        }
                        else
                        {
                            Qulaification.CreatedDate = DateTime.Now;
                            Qulaification.UpdatedDate = DateTime.Now;
                            _HRMSContext.Add(Qulaification);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }

                        }
                    }
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }





        //Experiance details
        public MasterEmployeeDTO AddUpdateEmployeeExperienceDetails(MasterEmployeeDTO dto)
        {
            //add/update Experience details
            try
            {
                if (dto.EmployeeExperienceDTO.Count() > 0)
                {
                    foreach (Master_Employee_ExperienceDTO ExperienceDTO in dto.EmployeeExperienceDTO)
                    {
                        ExperienceDTO.MI_Id = dto.Employeedto.MI_Id;
                        ExperienceDTO.HRME_Id = dto.Employeedto.HRME_Id;
                        Master_Employee_Experience Experience = Mapper.Map<Master_Employee_Experience>(ExperienceDTO);

                        if (Experience.HRMEE_Id > 0)
                        {
                            var Experienceresult = _HRMSContext.Master_Employee_Experience.Single(t => t.HRMEE_Id == ExperienceDTO.HRMEE_Id);
                            //added by 20/05/2017   
                            Experienceresult.UpdatedDate = DateTime.Now;
                            Mapper.Map(ExperienceDTO, Experienceresult);
                            _HRMSContext.Update(Experienceresult);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Update";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }

                        }
                        else
                        {
                            Experience.CreatedDate = DateTime.Now;
                            Experience.UpdatedDate = DateTime.Now;
                            _HRMSContext.Add(Experience);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }


        //Document details
        public MasterEmployeeDTO AddUpdateEmployeeDocumentDetails(MasterEmployeeDTO dto)
        {
            //add/update Documents details
            try
            {
                if (dto.EmployeeDocumentDTO.Count() > 0)
                {
                    foreach (Master_Employee_DocumentsDTO DocumentsDTO in dto.EmployeeDocumentDTO)
                    {
                        DocumentsDTO.MI_Id = dto.Employeedto.MI_Id;
                        DocumentsDTO.HRME_Id = dto.Employeedto.HRME_Id;
                        Master_Employee_Documents Documents = Mapper.Map<Master_Employee_Documents>(DocumentsDTO);

                        if (Documents.HRMEDS_Id > 0)
                        {
                            var Documentsresult = _HRMSContext.Master_Employee_Documents.Single(t => t.HRMEDS_Id == DocumentsDTO.HRMEDS_Id);
                            //added by 20/05/2017   
                            Documentsresult.UpdatedDate = DateTime.Now;
                            Mapper.Map(DocumentsDTO, Documentsresult);
                            _HRMSContext.Update(Documentsresult);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Update";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }

                        }
                        else
                        {
                            Documents.CreatedDate = DateTime.Now;
                            Documents.UpdatedDate = DateTime.Now;
                            _HRMSContext.Add(Documents);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        
        public MasterEmployeeDTO AddUpdateEmployeeMedicalDetails(MasterEmployeeDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                MasterEmployee dmoObj = Mapper.Map<MasterEmployee>(dto.Employeedto);
                if (dmoObj.HRME_Id > 0)
                {
                    var result = _HRMSContext.MasterEmployee.Single(t => t.HRME_Id == dmoObj.HRME_Id);
                    dto.Employeedto.UpdatedDate = DateTime.Now;
                    Mapper.Map(dto.Employeedto, result);
                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        dto.retrunMsg = "Update";
                    }
                    else
                    {
                        dto.retrunMsg = "false";
                    }
                }
                else
                {
                    var duplicatecountresult = 0;
                    if (dmoObj.HRME_EmployeeCode != null)
                    {
                        duplicatecountresult = _HRMSContext.MasterEmployee.Where(t => t.MI_Id == dto.MI_Id && t.HRME_EmployeeCode.Equals(dmoObj.HRME_EmployeeCode)).Count();
                    }
                    if (duplicatecountresult == 0)
                    {
                        dmoObj.HRME_ActiveFlag = true;
                        dmoObj.HRME_Height = dto.HRME_Height;
                        dmoObj.HRME_HeightUOM = dto.HRME_HeightUOM;
                        dmoObj.HRME_Weight = dto.HRME_Weight;
                        dmoObj.HRME_WeightUOM = dto.HRME_WeightUOM;
                        dmoObj.HRME_IdentificationMark = dto.HRME_IdentificationMark;
                        dmoObj.HRME_EyeSightIssue = dto.HRME_EyeSightIssue;
                        dmoObj.HRME_AnyHealthIssue = dto.HRME_AnyHealthIssue;
                        dmoObj.MI_Id = dto.MI_Id;
                        dmoObj.UpdatedDate = DateTime.Now;
                        dmoObj.CreatedDate = DateTime.Now;
                        _HRMSContext.Add(dmoObj);
                        var flag = _HRMSContext.SaveChanges();
                        if (flag == 1)
                        {
                            dto.retrunMsg = "Add";
                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }
                    }
                    else
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }
        
         public string GetTeamsUserID(MasterEmployeeDTO dto)
        {
            string meetingid = "", meetingurl = "", accesstoken = "";

            var teamcredentials = _Context.Institution.Where(c => c.MI_Id == dto.MI_Id).ToList();

            
                //MS Teams Meeting schedule Integration

                var teamstaffcredentials = (from a in _Context.HR_Master_Employee_DMO
                                            from b in _Context.Staff_User_Login
                                            where (a.HRME_Id == b.Emp_Code && a.HRME_Id== dto.HRME_Id && a.MI_Id == dto.MI_Id)
                                            select a).Distinct().ToList();

                string useraccesstokenurl = teamcredentials.FirstOrDefault().MI_MSTeamsUserAceessTockenURL;
                if(useraccesstokenurl!=null)
                {
                    useraccesstokenurl = useraccesstokenurl.Replace("TenantID", teamcredentials.FirstOrDefault().MI_MSTeamsTenentId);
                var client = new RestClient(useraccesstokenurl);

                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

                request.AddParameter("grant_type", teamcredentials.FirstOrDefault().MI_MSTeamsGrantType);
                request.AddParameter("client_id", teamcredentials.FirstOrDefault().MI_MSTeamsClientId);
                request.AddParameter("client_secret", teamcredentials.FirstOrDefault().MI_MSTemasClinetSecretCode);
                request.AddParameter("scope", teamcredentials.FirstOrDefault().MI_MSTeamsScope);
                //request.AddParameter("userName", teamcredentials.FirstOrDefault().MI_MSTeamsAdminUsername);
                //request.AddParameter("password", teamcredentials.FirstOrDefault().MI_MSTeamsAdminPassword);

                request.AddParameter("userName", dto.HRME_MSTeamsEmailId);
                request.AddParameter("password", dto.HRME_MSTeamsPassword);

                try
                {
                    IRestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                    JObject joResponse = JObject.Parse(response.Content);
                    accesstoken = (string)joResponse["access_token"];
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    _logger.LogError("Error in " + dto.MI_Id + " - " + ex.InnerException);
                }

                var client1 = new RestClient("https://graph.microsoft.com/v1.0/me");
                client1.Timeout = -1;
                var request1 = new RestRequest(Method.GET);
                request1.AddHeader("Authorization", "Bearer " + accesstoken);
                request1.AddHeader("Content-Type", "application/json");
                DateTime dt = DateTime.UtcNow;

                try
                {
                    IRestResponse response1 = client1.Execute(request1);
                    Console.WriteLine(response1.Content);
                    JObject joResponse1 = JObject.Parse(response1.Content);
                    dto.HRME_MSTeamsUserId = (string)joResponse1["id"];
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    _logger.LogError("Error in " + dto.MI_Id + " - " + ex.InnerException);
                }

                }
            return dto.HRME_MSTeamsUserId;
        }

        public MasterEmployeeDTO AddUpdateEmployeeOtherDetails(MasterEmployeeDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                MasterEmployee dmoObj = Mapper.Map<MasterEmployee>(dto.Employeedto);

                if (dmoObj.HRME_Id > 0)
                {
                    var result = _HRMSContext.MasterEmployee.Single(t => t.HRME_Id == dmoObj.HRME_Id);
                    Feeconcession(dto);
                    UpdateLeftEmployee(dto);
                    dto.Employeedto.UpdatedDate = DateTime.Now;
                    dto.HRME_DOC = dto.Employeedto.HRME_DOC;
                    dto.HRME_RetiredFlg = dto.HRME_RetiredFlg;
                     GetTeamsUserID(dto.Employeedto);
                     
                    Mapper.Map(dto.Employeedto, result); 

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        dto.retrunMsg = "Update";

                    }
                    else
                    {
                        dto.retrunMsg = "false";
                    }
                    var resultss = _HRMSContext.MasterEmployee.Single(t => t.HRME_Id == dmoObj.HRME_Id);

                    var Paramaeters = _HRMSContext.Staff_User_Login.Where(i => i.Emp_Code == dmoObj.HRME_Id).Select(d => d.Id).ToList();
                    var result1 = _HRMSContext.UserRoleWithInstituteDMO.Where(j => Paramaeters.Contains(j.Id)).ToList();
                    if (result1.Count > 0)
                    {
                        if (dto.Employeedto.HRME_LeftFlag == true)
                        {
                            result1[0].Activeflag = 0;
                            result1[0].UpdatedDate = DateTime.Now;
                        }
                        else if (dto.Employeedto.HRME_LeftFlag == false)
                        {
                            result1[0].Activeflag = 1; result1[0].UpdatedDate = DateTime.Now;
                        }
                        _HRMSContext.Update(result1[0]);

                        var flags = _HRMSContext.SaveChanges();
                        if (flags > 0)
                        {
                            dto.retrunMsg = "Update";

                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }
                    }


                    //if (dmoObj.HRME_TransfferedTo > 0)
                    //{
                    //    var result2 = _HRMSContext.MasterEmployee.Single(t => t.HRME_Id == dmoObj.HRME_Id);
                    //    result2.CreatedDate = DateTime.Now;
                    //    result2.UpdatedDate = DateTime.Now;
                    //    result2.HRME_Id = 0;
                    //    result2.MI_Id = Convert.ToInt64(dmoObj.HRME_TransfferedTo);
                    //    //dto.HRME_DOC = dto.Employeedto.HRME_DOC;
                    //    //Mapper.Map(dto.Employeedto, result);
                    //    _HRMSContext.Add(result2);
                    //    var flag2 = _HRMSContext.SaveChanges();
                    //    if (flag2 > 0)
                    //    {
                    //        dto.retrunMsg = "Add";
                    //    }
                    //    else
                    //    {
                    //        dto.retrunMsg = "false";
                    //    }
                    //}
                }
                else
                {
                    var duplicatecountresult = 0;
                    if (dmoObj.HRME_EmployeeCode != null)
                    {
                        duplicatecountresult = _HRMSContext.MasterEmployee.Where(t => t.MI_Id == dto.MI_Id && t.HRME_EmployeeCode.Equals(dmoObj.HRME_EmployeeCode)).Count();
                    }
                    if (duplicatecountresult == 0)
                    {
                        dmoObj.HRME_ActiveFlag = true;
                        dmoObj.HRME_LeftFlag = false;
                        dmoObj.HRME_DOC = dto.Employeedto.HRME_DOC;
                        //dmoObj.HRME_DOC = dto.HRME_DOC;
                        //dmoObj.HRME_RetiredFlg = dto.HRME_RetiredFlg;
                        dmoObj.MI_Id = dto.MI_Id;
                        dmoObj.UpdatedDate = DateTime.Now;
                        dmoObj.CreatedDate = DateTime.Now;
                        _HRMSContext.Add(dmoObj);
                        var flag = _HRMSContext.SaveChanges();
                        if (flag == 1)
                        {
                            MasterEmployeeDTO DTO = Mapper.Map<MasterEmployeeDTO>(dmoObj);
                            DTO.HRME_EmployeeOrder = Convert.ToInt32(DTO.HRME_Id);
                            var resultd = _HRMSContext.MasterEmployee.Single(t => t.HRME_Id == DTO.HRME_Id);
                            dmoObj.HRME_DOC = dto.HRME_DOC;
                            Mapper.Map(DTO, resultd);
                            _HRMSContext.Update(resultd);
                            _HRMSContext.SaveChanges();
                            dto.Employeedto.HRME_Id = dmoObj.HRME_Id;
                            dto.Employeedto.MI_Id = dto.MI_Id;
                            dto.retrunMsg = "Add";

                            var resultss = _HRMSContext.MasterEmployee.Single(t => t.HRME_Id == dmoObj.HRME_Id);
                            var Paramaeters = _HRMSContext.Staff_User_Login.Where(i => i.Emp_Code == dmoObj.HRME_Id).Select(d => d.Id).ToList();
                            var result1 = _HRMSContext.UserRoleWithInstituteDMO.Where(j => Paramaeters.Contains(j.Id)).ToList();
                            if (result1.Count > 0)
                            {
                                if (dto.Employeedto.HRME_LeftFlag == true)
                                {
                                    result1[0].Activeflag = 0;
                                    result1[0].UpdatedDate = DateTime.Now;
                                }
                                else if (dto.Employeedto.HRME_LeftFlag == false)
                                {
                                    result1[0].Activeflag = 1; result1[0].UpdatedDate = DateTime.Now;
                                }
                                _HRMSContext.Update(result1[0]);
                            }
                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }
                    }
                    else
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }


        //Increament details
        public MasterEmployeeDTO AddUpdateEmployeeIncreamentDetails(MasterEmployeeDTO dto)
        {
            //add/update Documents details
            try
            {
                if (dto.IncrementDetailsDTO != null)
                {

                    dto.IncrementDetailsDTO.MI_Id = dto.Employeedto.MI_Id;
                    dto.IncrementDetailsDTO.HRME_Id = dto.Employeedto.HRME_Id;
                    HR_Master_Employee_IncrementDetails IncrementDetails = Mapper.Map<HR_Master_Employee_IncrementDetails>(dto.IncrementDetailsDTO);

                    if (dto.IncrementDetailsDTO.HRMEID_Id > 0)
                    {
                        var Incrementresult = _HRMSContext.HR_Master_Employee_IncrementDetails.Single(t => t.HRMEID_Id == dto.IncrementDetailsDTO.HRMEID_Id);
                        //added by 20/05/2017   
                        Mapper.Map(dto.IncrementDetailsDTO, Incrementresult);
                        _HRMSContext.Update(Incrementresult);
                        _HRMSContext.SaveChanges();

                    }
                    else
                    {
                        _HRMSContext.Add(IncrementDetails);
                        _HRMSContext.SaveChanges();

                    }
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.InnerException);
            }

            return dto;
        }
        //Employee Earning details
        public MasterEmployeeDTO AddUpdateEmployeeEarningDetails(MasterEmployeeDTO dto)
        {
            //add/update Documents details
            try
            {
                if (dto.EarningDTO.Count() > 0)
                {
                    foreach (HR_Employee_EarningsDeductionsDTO DocumentsDTO in dto.EarningDTO)
                    {
                        DocumentsDTO.MI_Id = dto.Employeedto.MI_Id;
                        DocumentsDTO.HRME_Id = dto.Employeedto.HRME_Id;
                        //HR_Employee_EarningsDeductions Documents = Mapper.Map<HR_Employee_EarningsDeductions>(DocumentsDTO);                    

                        if (DocumentsDTO.HREED_Id > 0)
                        {
                            var Documentsresult = _HRMSContext.HR_Employee_EarningsDeductions.Single(t => t.HREED_Id == DocumentsDTO.HREED_Id);

                            Documentsresult.HREED_Amount = DocumentsDTO.HREED_Amount;
                            Documentsresult.HREED_Percentage = DocumentsDTO.HREED_Percentage;
                            Documentsresult.HRMED_Id = DocumentsDTO.HRMED_Id;
                            // Mapper.Map(DocumentsDTO, Documentsresult);
                            _HRMSContext.Update(Documentsresult);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Update";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }

                        }
                        else
                        {
                            HR_Employee_EarningsDeductions Documents = new HR_Employee_EarningsDeductions();

                            Documents.MI_Id = DocumentsDTO.MI_Id;
                            Documents.HRME_Id = DocumentsDTO.HRME_Id;
                            Documents.HRMED_Id = DocumentsDTO.HRMED_Id;
                            Documents.HREED_Amount = DocumentsDTO.HREED_Amount;
                            Documents.HREED_Percentage = DocumentsDTO.HREED_Percentage;
                            Documents.HREED_ActiveFlag = true;

                            _HRMSContext.Add(Documents);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        //Employee Arrear details
        public MasterEmployeeDTO AddUpdateEmployeeArrearDetails(MasterEmployeeDTO dto)
        {
            //add/update Documents details
            try
            {
                if (dto.EarningDTO.Count() > 0)
                {
                    foreach (HR_Employee_EarningsDeductionsDTO DocumentsDTO in dto.ArrearDTO)
                    {
                        DocumentsDTO.MI_Id = dto.Employeedto.MI_Id;
                        DocumentsDTO.HRME_Id = dto.Employeedto.HRME_Id;
                        HR_Employee_EarningsDeductions Documents = Mapper.Map<HR_Employee_EarningsDeductions>(DocumentsDTO);

                        if (Documents.HREED_Id > 0)
                        {
                            var Documentsresult = _HRMSContext.HR_Employee_EarningsDeductions.Single(t => t.HREED_Id == DocumentsDTO.HREED_Id);
                            //added by 20/05/2017  
                            //  DocumentsDTO.HREED_ActiveFlag = true;
                            Mapper.Map(DocumentsDTO, Documentsresult);
                            _HRMSContext.Update(Documentsresult);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Update";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }

                        }
                        else
                        {
                            // Documents.HREED_ActiveFlag = true;
                            _HRMSContext.Add(Documents);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }

                        }
                    }
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }
        //Employee Deduction details
        public MasterEmployeeDTO AddUpdateEmployeeDeductionDetails(MasterEmployeeDTO dto)
        {
            //add/update Documents details
            try
            {
                if (dto.DeductionDTO.Count() > 0)
                {
                    foreach (HR_Employee_EarningsDeductionsDTO DocumentsDTO in dto.DeductionDTO)
                    {
                        DocumentsDTO.MI_Id = dto.Employeedto.MI_Id;
                        DocumentsDTO.HRME_Id = dto.Employeedto.HRME_Id;
                        HR_Employee_EarningsDeductions Documents = Mapper.Map<HR_Employee_EarningsDeductions>(DocumentsDTO);

                        if (Documents.HREED_Id > 0)
                        {
                            var Documentsresult = _HRMSContext.HR_Employee_EarningsDeductions.Single(t => t.HREED_Id == DocumentsDTO.HREED_Id);
                            //added by 20/05/2017   
                            // DocumentsDTO.HREED_ActiveFlag = true;
                            Mapper.Map(DocumentsDTO, Documentsresult);
                            _HRMSContext.Update(Documentsresult);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Update";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }

                        }
                        else
                        {
                            //Documents.HREED_ActiveFlag = true;
                            _HRMSContext.Add(Documents);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }

                        }
                    }
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }


        public MasterEmployeeDTO editData(int id)
        {

            MasterEmployeeDTO dto = new MasterEmployeeDTO();
            dto.retrunMsg = "";
            try
            {
                List<MasterEmployee> lorg = new List<MasterEmployee>();
                lorg = _HRMSContext.MasterEmployee.AsNoTracking().Where(t => t.HRME_Id.Equals(id)).ToList();
                dto.employeedetailList = lorg.ToArray();

                //Qualification details
                List<Master_Employee_Qulaification> Qulaificationlorg = new List<Master_Employee_Qulaification>();
                Qulaificationlorg = _HRMSContext.Master_Employee_Qulaification.AsNoTracking().Where(t => t.HRME_Id.Equals(id)).ToList();
                dto.qualificationDetails = Qulaificationlorg.ToArray();


                //Experiance details
                List<Master_Employee_Experience> Experiencelorg = new List<Master_Employee_Experience>();
                Experiencelorg = _HRMSContext.Master_Employee_Experience.AsNoTracking().Where(t => t.HRME_Id.Equals(id)).ToList();
                dto.experienceDetails = Experiencelorg.ToArray();

                //Documents details
                List<Master_Employee_Documents> Documentslorg = new List<Master_Employee_Documents>();
                Documentslorg = _HRMSContext.Master_Employee_Documents.AsNoTracking().Where(t => t.HRME_Id.Equals(id)).ToList();
                dto.documentList = Documentslorg.ToArray();


                //increament and Basic Amount details
                List<HR_Master_Employee_IncrementDetails> IncrementDetails = new List<HR_Master_Employee_IncrementDetails>();
                IncrementDetails = _HRMSContext.HR_Master_Employee_IncrementDetails.AsNoTracking().Where(t => t.HRME_Id.Equals(id)).ToList();
                dto.incrementDetails = IncrementDetails.ToArray();


                //Employee Earning & deduction details

                List<HR_Employee_EarningsDeductionsDTO> EarningsDeductionsDetails = new List<HR_Employee_EarningsDeductionsDTO>();
                //EarningsDeductionsDetails = _HRMSContext.HR_Employee_EarningsDeductions.AsNoTracking().Where(t => t.HRME_Id.Equals(id)).ToList();


                EarningsDeductionsDetails = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                                             from med in _HRMSContext.HR_Master_EarningsDeductions
                                             where emp.HRMED_Id == med.HRMED_Id && med.HRMED_ActiveFlag == true
                                             && emp.HRME_Id == id
                                             select new HR_Employee_EarningsDeductionsDTO
                                             {
                                                 HREED_Id = emp.HREED_Id,
                                                 HRMED_Id = emp.HRMED_Id,
                                                 HRME_Id = emp.HRME_Id,
                                                 MI_Id = emp.MI_Id,
                                                 HREED_Amount = emp.HREED_Amount,
                                                 HREED_Percentage = emp.HREED_Percentage,
                                                 HRMED_EarnDedFlag = med.HRMED_EarnDedFlag,
                                                 HREED_ActiveFlag = emp.HREED_ActiveFlag,
                                                 HREED_ApplicableMaxValue = emp.HREED_ApplicableMaxValue
                                             }
                                ).ToList();



                dto.employeeEarningsDeductionsDetails = EarningsDeductionsDetails.Distinct().ToArray();





                var mobile_list = (from a in _HRMSContext.Emp_MobileNo
                                   from b in _HRMSContext.MasterEmployee
                                   where (a.HRME_Id == b.HRME_Id && a.HRME_Id == id)
                                   select a).ToList();

                dto.selectedmobile_list_dto = mobile_list.ToArray();


                var email_list = (from a in _HRMSContext.MasterEmployee
                                  from b in _HRMSContext.Emp_Email_Id
                                  where (a.HRME_Id == b.HRME_Id && a.HRME_Id == id)
                                  select b).ToList();

                dto.selectedemail_list_dto = email_list.ToArray();




                var empdata = (from a in _HRMSContext.MasterEmployee
                               from b in _HRMSContext.HR_Master_Employee_Bank
                               where (a.HRME_Id == b.HRME_Id && a.HRME_Id == id)
                               select b).ToList();

                dto.employeebankDetails = empdata.ToArray();

                ////


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public MasterEmployeeDTO deactivate(MasterEmployeeDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRME_Id > 0)
                {
                    var result = _HRMSContext.MasterEmployee.Single(t => t.HRME_Id == dto.HRME_Id);


                   
                    var Paramaeters = _HRMSContext.Staff_User_Login.Where(i => i.Emp_Code == dto.HRME_Id).Select(d => d.Id).ToList();
                    var result1= _HRMSContext.UserRoleWithInstituteDMO.Where(j=> Paramaeters.Contains(j.Id)).ToList();
                    if (result1.Count > 0)
                    {
                        if (result.HRME_ActiveFlag == true)
                        {
                            result1[0].Activeflag = 0;
                            result1[0].UpdatedDate = DateTime.Now;

                        }
                        else if (result.HRME_ActiveFlag == false)
                        {
                            result1[0].Activeflag =1; result1[0].UpdatedDate = DateTime.Now;
                        }
                        _HRMSContext.Update(result1[0]);
                    }


                    


                    if (result.HRME_ActiveFlag == true)
                    {
                        result.HRME_ActiveFlag = false;
                        result.UpdatedDate = DateTime.Now;
                    }
                   
                    else if (result.HRME_ActiveFlag == false)
                    {
                        result.HRME_ActiveFlag = true;
                      
                        result.UpdatedDate = DateTime.Now;
                      
                    }
                  
                   
                    _HRMSContext.Update(result);
                  

                  
                   
                       






                        var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRME_ActiveFlag == true)
                        {

                            dto.retrunMsg = "Activated";
                        }
                        else
                        {
                            dto.retrunMsg = "Deactivated";
                        }
                    }
                    else
                    {
                        dto.retrunMsg = "Record Not Activated/Deactivated";
                    }

                    dto = GetAllDropdownAndDatatableDetails(dto);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public MasterEmployeeDTO GetAllDropdownAndDatatableDetails(MasterEmployeeDTO dto)
        {
            List<MasterEmployee> EmployeeList = new List<MasterEmployee>();
            List<HR_Master_EmployeeType> EmployeeTypelist = new List<HR_Master_EmployeeType>();
            List<HR_Master_GroupType> GroupTypelist = new List<HR_Master_GroupType>();
            List<HR_Master_Department> Departmentlist = new List<HR_Master_Department>();
            List<HR_Master_Designation> Designationlist = new List<HR_Master_Designation>();
            List<HR_Master_CourseDMO> Courselist = new List<HR_Master_CourseDMO>();
            List<HR_Master_Grade> Gradelist = new List<HR_Master_Grade>();
            List<masterSpecialisationDMO> SpecialisationList = new List<masterSpecialisationDMO>();
            List<masterLeavingReasonDMO> LeavingReasonList = new List<masterLeavingReasonDMO>();
            List<Institution> institutionlist = new List<Institution>();
            List<IVRM_Master_Gender> Genderlist = new List<IVRM_Master_Gender>();
            List<IVRM_Master_Marital_Status> MaritalStatuslist = new List<IVRM_Master_Marital_Status>();
            List<HR_Master_EarningsDeductions> earningdeductiondatalist = new List<HR_Master_EarningsDeductions>();
            List<HR_Master_EarningsDeductions> earningdatalist = new List<HR_Master_EarningsDeductions>();
            List<HR_Master_EarningsDeductions> deductiondatalist = new List<HR_Master_EarningsDeductions>();
            List<HR_Master_EarningsDeductionsDTO> DTOdatalistEarning = new List<HR_Master_EarningsDeductionsDTO>();
            List<HR_Master_EarningsDeductionsDTO> DTOdatalistDeduction = new List<HR_Master_EarningsDeductionsDTO>();
            List<HR_Master_EarningsDeductionsDTO> DTOdatalistArrear = new List<HR_Master_EarningsDeductionsDTO>();
            List<HR_Master_EarningsDeductions> arreardatalist = new List<HR_Master_EarningsDeductions>();
            List<HR_Master_EarningsDeductions> grossdatalist = new List<HR_Master_EarningsDeductions>();
            List<HR_Master_EarningsDeductionsDTO> DTOdatalistGross = new List<HR_Master_EarningsDeductionsDTO>();
            List<HR_Master_BankDeatils> banklist = new List<HR_Master_BankDeatils>();
            List<HR_PROCESSDMO> PROCESSList = new List<HR_PROCESSDMO>();

            try
            {
                // dto.LogInUserId = 10002;
                //Employee List
                //var empcode_check = (from a in _HRMSContext.Staff_User_Login
                //                     where (a.MI_Id == dto.MI_Id)
                //                     select new MasterEmployeeDTO
                //                     {
                //                         username = a.IVRMSTAUL_UserName
                //                     }).ToList();

                //var empcode_check1 = (from a in _HRMSContext.userLogin
                //                      where (a.MI_Id == dto.MI_Id && a.IVRMUL_UserName == empcode_check.FirstOrDefault().username)
                //                      select new MasterEmployeeDTO
                //                      {
                //                          IVRMUL_Id = a.IVRMUL_Id
                //                      }).ToList();



                //PROCESSList = (from ao in _HRMSContext.HR_Process_Auth_OrderNoDMO
                //               from pa in _HRMSContext.HR_PROCESSDMO
                //               from au in _HRMSContext.Staff_User_Login
                //               where (pa.HRPA_Id == ao.HRPA_Id && au.Id == dto.LogInUserId
                //               &&  pa.MI_Id == dto.MI_Id && au.IVRMSTAUL_Id== ao.IVRMUL_Id)
                //               select pa
                //          ).ToList();

                PROCESSList = (from ao in _HRMSContext.HR_Process_Auth_OrderNoDMO
                               from pa in _HRMSContext.HR_PROCESSDMO
                               from cc in _HRMSContext.Staff_User_Login
                               where (pa.HRPA_Id == ao.HRPA_Id && ao.IVRMUL_Id == cc.IVRMSTAUL_Id && cc.Id == dto.LogInUserId)


                               select pa
                          ).ToList();

                if (PROCESSList.Count() > 0)
                {

                    List<long> groupTypeIdList = PROCESSList.Select(t => t.HRMGT_Id).Distinct().ToList();
                    List<long> hrmD_IdList = PROCESSList.Select(t => t.HRMD_Id).Distinct().ToList();
                    List<long> hrmdeS_IdList = PROCESSList.Select(t => t.HRMDES_Id).Distinct().ToList();

                    EmployeeList = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id))).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                    dto.employeedetailList = EmployeeList.ToArray();

                    //GroupTypelist
                    GroupTypelist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id)) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                    dto.groupTypedropdownlist = GroupTypelist.ToArray();

                    //Departmentlist
                    Departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmD_IdList.Contains(Convert.ToInt64(t.HRMD_Id)) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                    dto.departmentdropdownlist = Departmentlist.ToArray();

                    //Designationlist
                    Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmdeS_IdList.Contains(Convert.ToInt64(t.HRMDES_Id)) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                    dto.designationdropdownlist = Designationlist.ToArray();
                }
                else
                {
                    EmployeeList = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id)).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                    dto.employeedetailList = EmployeeList.ToArray();

                    //GroupTypelist
                    GroupTypelist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                    dto.groupTypedropdownlist = GroupTypelist.ToArray();

                    //Departmentlist
                    Departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                    dto.departmentdropdownlist = Departmentlist.ToArray();

                    //Designationlist
                    Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                    dto.designationdropdownlist = Designationlist.ToArray();
                }

                banklist = _HRMSContext.HR_Master_BankDeatils.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMBD_ActiveFlag == true).ToList();
                dto.bankdropdownlist = banklist.ToArray();

                //emptype
                EmployeeTypelist = _HRMSContext.HR_Master_EmployeeType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMET_ActiveFlag == true).ToList();
                dto.employeeTypedropdownlist = EmployeeTypelist.ToArray();

                //Gradelist
                Gradelist = _HRMSContext.HR_Master_Grade.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMG_ActiveFlag == true).OrderBy(t => t.HRMG_Order).ToList();
                dto.gradedropdownlist = Gradelist.ToArray();

                //Genderlist
                Genderlist = _HRMSContext.IVRM_Master_Gender.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.IVRMMG_ActiveFlag == true).ToList();
                dto.genderdropdownlist = Genderlist.ToArray();

                //MaritalStatuslist
                MaritalStatuslist = _HRMSContext.IVRM_Master_Marital_Status.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.IVRMMMS_ActiveFlag == true).ToList();
                dto.maritalStatusdropdownlist = MaritalStatuslist.ToArray();

                //Courselist
                Courselist = _HRMSContext.HR_Master_Course.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMC_ActiveFlag == true).OrderBy(t => t.HRMC_Order).ToList();
                dto.coursedropdownlist = Courselist.ToArray();

                SpecialisationList = _HRMSContext.masterSpecialisationDMO.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMSPL_ActiveFlg == true).ToList();
                dto.SpecialisationList = SpecialisationList.ToArray();

                LeavingReasonList = _HRMSContext.masterLeavingReasonDMO.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMLREA_ActiveFlg == true).ToList();
                dto.LeavingReasonList = LeavingReasonList.ToArray();

                institutionlist = _HRMSContext.Institution.Where(t => t.MI_ActiveFlag == 1 && t.MI_Id != dto.MI_Id).ToList();
                dto.institutiondropdownlist = institutionlist.ToArray();

                HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                HR_ConfigurationDTO dmoObj = Mapper.Map<HR_ConfigurationDTO>(PayrollStandard);
                dto.configurationDetails = dmoObj;

                //List<mastercasteDMO> AllCaste = new List<mastercasteDMO>();
                //AllCaste = _HRMSContext.Caste.Where(c => c.MI_Id == dto.MI_Id && c.IMC_CasteName != null).ToList();
                //dto.castedropdownlist = AllCaste.ToArray();

                //var AllCastess = (from a in _HRMSContext.Caste
                //                  from b in _HRMSContext.CasteCategory
                //                  where (a.IMCC_Id == b.IMCC_Id && a.MI_Id == dto.MI_Id)
                //                  select b).ToArray();


                var AllCastess = (from a in _HRMSContext.Caste
                                  from b in _HRMSContext.CasteCategory
                                  where (a.IMCC_Id == b.IMCC_Id && a.MI_Id == dto.MI_Id)
                                  select a).ToArray();

                dto.castedropdownlist = AllCastess.Distinct().ToArray();


                //Earning list
                earningdatalist = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_EarnDedFlag.Equals("Earning") && t.HRMED_ActiveFlag == true).OrderBy(t=>t.HRMED_Order).ToList();

                if (earningdatalist.Count() > 0)
                {
                    foreach (HR_Master_EarningsDeductions ph in earningdatalist)
                    {
                        HR_Master_EarningsDeductionsDTO phdto = Mapper.Map<HR_Master_EarningsDeductionsDTO>(ph);

                        if (phdto.HRMED_AmountPercentFlag == "Percentage")
                        {
                            var EarningsDeductionsPerlist = _HRMSContext.HR_Master_EarningsDeductionsPer.Where(t => t.MI_Id.Equals(phdto.MI_Id) && t.HRMED_Id == phdto.HRMED_Id).ToList();

                            if (EarningsDeductionsPerlist.Count() > 0)
                            {
                                List<string> percentOff = new List<string>();

                                foreach (HR_Master_EarningsDeductionsPer headername in EarningsDeductionsPerlist)
                                {
                                    var percentOffHRMED_Name = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_Id.Equals(headername.HRMEDP_HRMED_Id)).Select(t => t.HRMED_Name).ToList();

                                    percentOff.Add(percentOffHRMED_Name.FirstOrDefault());
                                }
                                phdto.percentOff = String.Join(" + ", percentOff.ToArray());
                            }
                            else
                            {
                                phdto.percentOff = "";
                            }
                        }
                        else
                        {
                            phdto.percentOff = "";
                        }

                        DTOdatalistEarning.Add(phdto);

                    }

                }

                dto.earningList = DTOdatalistEarning.ToArray();

                //Deduction List
                deductiondatalist = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_EarnDedFlag.Equals("Deduction") && t.HRMED_ActiveFlag == true).OrderBy(t=>t.HRMED_Order).ToList();

                if (deductiondatalist.Count() > 0)
                {
                    foreach (HR_Master_EarningsDeductions ph in deductiondatalist)
                    {
                        HR_Master_EarningsDeductionsDTO phdto = Mapper.Map<HR_Master_EarningsDeductionsDTO>(ph);

                        if (phdto.HRMED_AmountPercentFlag == "Percentage")
                        {
                            var EarningsDeductionsPerlist = _HRMSContext.HR_Master_EarningsDeductionsPer.Where(t => t.MI_Id.Equals(phdto.MI_Id) && t.HRMED_Id == phdto.HRMED_Id).ToList();

                            if (EarningsDeductionsPerlist.Count() > 0)
                            {
                                List<string> percentOff = new List<string>();

                                foreach (HR_Master_EarningsDeductionsPer headername in EarningsDeductionsPerlist)
                                {
                                    var percentOffHRMED_Name = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_Id.Equals(headername.HRMEDP_HRMED_Id)).Select(t => t.HRMED_Name).ToList();

                                    percentOff.Add(percentOffHRMED_Name.FirstOrDefault());
                                }
                                phdto.percentOff = String.Join(" + ", percentOff.ToArray());
                            }
                            else
                            {
                                phdto.percentOff = "";
                            }
                        }
                        else
                        {
                            phdto.percentOff = "";
                        }

                        DTOdatalistDeduction.Add(phdto);

                    }

                }

                dto.detectionList = DTOdatalistDeduction.ToArray();



                //Arrear list
                arreardatalist = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_EarnDedFlag.Equals("Arrear") && t.HRMED_ActiveFlag == true).ToList();

                if (arreardatalist.Count() > 0)
                {
                    foreach (HR_Master_EarningsDeductions ph in arreardatalist)
                    {
                        HR_Master_EarningsDeductionsDTO phdto = Mapper.Map<HR_Master_EarningsDeductionsDTO>(ph);

                        if (phdto.HRMED_AmountPercentFlag == "Percentage")
                        {
                            var EarningsDeductionsPerlist = _HRMSContext.HR_Master_EarningsDeductionsPer.Where(t => t.MI_Id.Equals(phdto.MI_Id) && t.HRMED_Id == phdto.HRMED_Id).ToList();

                            if (EarningsDeductionsPerlist.Count() > 0)
                            {
                                List<string> percentOff = new List<string>();

                                foreach (HR_Master_EarningsDeductionsPer headername in EarningsDeductionsPerlist)
                                {
                                    var percentOffHRMED_Name = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_Id.Equals(headername.HRMEDP_HRMED_Id)).Select(t => t.HRMED_Name).ToList();

                                    percentOff.Add(percentOffHRMED_Name.FirstOrDefault());
                                }
                                phdto.percentOff = String.Join(" + ", percentOff.ToArray());
                            }
                            else
                            {
                                phdto.percentOff = "";
                            }
                        }
                        else
                        {
                            phdto.percentOff = "";
                        }

                        DTOdatalistArrear.Add(phdto);

                    }

                }

                dto.arrearList = DTOdatalistArrear.ToArray();



                //Gross list
                grossdatalist = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_EarnDedFlag.Equals("Gross") && t.HRMED_ActiveFlag == true).ToList();

                if (grossdatalist.Count() > 0)
                {
                    foreach (HR_Master_EarningsDeductions ph in grossdatalist)
                    {
                        HR_Master_EarningsDeductionsDTO phdto = Mapper.Map<HR_Master_EarningsDeductionsDTO>(ph);

                        if (phdto.HRMED_AmountPercentFlag == "Percentage")
                        {
                            var EarningsDeductionsPerlist = _HRMSContext.HR_Master_EarningsDeductionsPer.Where(t => t.MI_Id.Equals(phdto.MI_Id) && t.HRMED_Id == phdto.HRMED_Id).ToList();

                            if (EarningsDeductionsPerlist.Count() > 0)
                            {
                                List<string> percentOff = new List<string>();

                                foreach (HR_Master_EarningsDeductionsPer headername in EarningsDeductionsPerlist)
                                {
                                    var percentOffHRMED_Name = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_Id.Equals(headername.HRMEDP_HRMED_Id)).Select(t => t.HRMED_Name).ToList();

                                    percentOff.Add(percentOffHRMED_Name.FirstOrDefault());
                                }
                                phdto.percentOff = String.Join(" + ", percentOff.ToArray());
                            }
                            else
                            {
                                phdto.percentOff = "";
                            }
                        }
                        else
                        {
                            phdto.percentOff = "";
                        }
                        DTOdatalistGross.Add(phdto);
                    }
                }

                dto.grossList = DTOdatalistGross.ToArray();

                List<Country> Allcountry = new List<Country>();
                Allcountry = _Context.country.Where(c => c.IVRMMC_CountryName != null).ToList();
                dto.countrydropdownlist = Allcountry.ToArray();

                //caste category
                List<CasteCategory> AllcasteCategory = new List<CasteCategory>();
                AllcasteCategory = _HRMSContext.CasteCategory.Where(c => c.IMCC_CategoryName != null).ToList();
                dto.casteCategorydropdownlist = AllcasteCategory.ToArray();

                List<MasterReligionDMO> AllReligion = new List<MasterReligionDMO>();
                AllReligion = _HRMSContext.Religion.Where(r => r.Is_Active == true && r.IVRMMR_Name != null).ToList();
                dto.religiondropdownlist = AllReligion.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

        //get statelistByCountryId

        public MasterEmployeeDTO getstateDropdown(int id)//int IVRMM_Id
        {
            MasterEmployeeDTO dto = new MasterEmployeeDTO();
            List<State> AllState = new List<State>();
            if (id > 0)
            {
                AllState = _Context.State.Where(t => t.IVRMMC_Id.Equals(id)).ToList();
                dto.AllState = AllState.ToArray();
            }
            return dto;
        }

        //validate data  

        public MasterEmployeeDTO validateData(MasterEmployeeDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                // if (dto.HRME_Id == 0)
                //  {
                if (dto.Type.Equals("EmployeeOrder"))
                {
                    return EmployeeOrderDuplicateCheck(dto);
                }
                else if (dto.Type.Equals("mobileNo"))
                {
                    return MobileNoDuplicateCheck(dto);
                }
                else if (dto.Type.Equals("EmailID"))
                {
                    return EmailIdDuplicateCheck(dto);
                }
                else if (dto.Type.Equals("AadharCardNo"))
                {
                    return AdharcardNumberDuplicateCheck(dto);
                }
                else if (dto.Type.Equals("PANNumber"))
                {
                    return PANCardNumberDuplicateCheck(dto);
                }

                else if (dto.Type.Equals("SpouseEmailId"))
                {
                    return SpouseEmailIdDuplicateCheck(dto);
                }

                else if (dto.Type.Equals("SpouseMobileNo"))
                {
                    return SpouseMobileNoDuplicateCheck(dto);
                }

                else if (dto.Type.Equals("BiometricCode"))
                {
                    return BiometricCodeDuplicateCheck(dto);
                }
                else if (dto.Type.Equals("EmployeeCode"))
                {
                    return EmployeeCodeDuplicateCheck(dto);
                }
                else if (dto.Type.Equals("RFCardId"))
                {
                    return RFCardIdDuplicateCheck(dto);
                }
                else if (dto.Type.Equals("PFAccNo"))
                {
                    return PFAccNoDuplicateCheck(dto);
                }
                else if (dto.Type.Equals("UINumber"))
                {
                    return UINumberDuplicateCheck(dto);
                }
                else if (dto.Type.Equals("ESIAccNo"))
                {
                    return ESIAccNoDuplicateCheck(dto);
                }
                else if (dto.Type.Equals("GratuityAccNo"))
                {
                    return GratuityAccNoDuplicateCheck(dto);
                }
                else if (dto.Type.Equals("NationalSSN"))
                {
                    return NationalSSNDuplicateCheck(dto);
                }

                //  }

            }
            catch (Exception e)
            {
                dto.retrunMsg = "error";
                Console.WriteLine(e.Message);
            }
            return dto;
        }

        private MasterEmployeeDTO NationalSSNDuplicateCheck(MasterEmployeeDTO dto)
        {
            try
            {
                if (dto.HRME_Id == 0)
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_NationalSSN.Equals(dto.HRME_NationalSSN)).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }
                else
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_NationalSSN.Equals(dto.HRME_NationalSSN) && t.HRME_Id != dto.HRME_Id).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }


            }
            catch (Exception e)
            {
                dto.retrunMsg = "error";
                Console.WriteLine(e.Message);
            }

            return dto;
        }

        private MasterEmployeeDTO GratuityAccNoDuplicateCheck(MasterEmployeeDTO dto)
        {
            try
            {

                if (dto.HRME_Id == 0)
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_GratuityAccNo.Equals(dto.HRME_GratuityAccNo)).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }


                }
                else
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_GratuityAccNo.Equals(dto.HRME_GratuityAccNo) && t.HRME_Id != dto.HRME_Id).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }


            }
            catch (Exception e)
            {
                dto.retrunMsg = "error";
                Console.WriteLine(e.Message);
            }

            return dto;
        }

        private MasterEmployeeDTO ESIAccNoDuplicateCheck(MasterEmployeeDTO dto)
        {
            try
            {
                if (dto.HRME_Id == 0)
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ESIAccNo.Equals(dto.HRME_ESIAccNo)).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }

                }
                else
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ESIAccNo.Equals(dto.HRME_ESIAccNo) && t.HRME_Id != dto.HRME_Id).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }


            }
            catch (Exception e)
            {
                dto.retrunMsg = "error";
                Console.WriteLine(e.Message);
            }

            return dto;
        }

        private MasterEmployeeDTO PFAccNoDuplicateCheck(MasterEmployeeDTO dto)
        {
            try
            {
                if (dto.HRME_Id == 0)
                {

                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_PFAccNo.Equals(dto.HRME_PFAccNo)).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }
                else
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_PFAccNo.Equals(dto.HRME_PFAccNo) && t.HRME_Id != dto.HRME_Id).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }


            }
            catch (Exception e)
            {
                dto.retrunMsg = "error";
                Console.WriteLine(e.Message);
            }

            return dto;
        }

        private MasterEmployeeDTO UINumberDuplicateCheck(MasterEmployeeDTO dto)
        {
            try
            {
                if (dto.HRME_Id == 0)
                {

                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_UINumber.Equals(dto.HRME_UINumber)).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }
                else
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_UINumber.Equals(dto.HRME_UINumber) && t.HRME_Id != dto.HRME_Id).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }


            }
            catch (Exception e)
            {
                dto.retrunMsg = "error";
                Console.WriteLine(e.Message);
            }

            return dto;
        }

        private MasterEmployeeDTO RFCardIdDuplicateCheck(MasterEmployeeDTO dto)
        {
            try
            {
                if (dto.HRME_Id == 0)
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_RFCardId.Equals(dto.HRME_RFCardId)).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }
                else
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_RFCardId.Equals(dto.HRME_RFCardId) && t.HRME_Id != dto.HRME_Id).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }

            }
            catch (Exception e)
            {
                dto.retrunMsg = "error";
                Console.WriteLine(e.Message);
            }

            return dto;
        }

        private MasterEmployeeDTO EmployeeCodeDuplicateCheck(MasterEmployeeDTO dto)
        {
            try
            {
                if (dto.HRME_Id == 0)
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_EmployeeCode.Equals(dto.HRME_EmployeeCode)).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }
                else
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_EmployeeCode.Equals(dto.HRME_EmployeeCode)).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }


            }
            catch (Exception e)
            {
                dto.retrunMsg = "error";
                Console.WriteLine(e.Message);
            }

            return dto;
        }

        private MasterEmployeeDTO BiometricCodeDuplicateCheck(MasterEmployeeDTO dto)
        {
            try
            {
                if (dto.HRME_Id == 0)
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_BiometricCode.Equals(dto.HRME_BiometricCode)).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }
                else
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_BiometricCode.Equals(dto.HRME_BiometricCode) && t.HRME_Id != dto.HRME_Id).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }

            }
            catch (Exception e)
            {
                dto.retrunMsg = "error";
                Console.WriteLine(e.Message);
            }

            return dto;
        }

        //Employee Order validation 
        public MasterEmployeeDTO EmployeeOrderDuplicateCheck(MasterEmployeeDTO dto)
        {
            try
            {
                if (dto.HRME_Id == 0)
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_EmployeeOrder.Equals(dto.HRME_EmployeeOrder)).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }

                }
                else
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_EmployeeOrder.Equals(dto.HRME_EmployeeOrder) && t.HRME_Id != dto.HRME_Id).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }

                }



            }
            catch (Exception e)
            {
                dto.retrunMsg = "error";
                Console.WriteLine(e.Message);
            }

            return dto;
        }

        //mobile Number validation
        public MasterEmployeeDTO MobileNoDuplicateCheck(MasterEmployeeDTO dto)
        {
            try
            {
                var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_MobileNo.Equals(dto.HRME_MobileNo)).Count();

                if (resultCout > 0)
                {
                    dto.retrunMsg = "Duplicate";
                    return dto;
                }
            }
            catch (Exception e)
            {
                dto.retrunMsg = "error";
                Console.WriteLine(e.Message);
            }

            return dto;
        }

        //Email Id validation
        public MasterEmployeeDTO EmailIdDuplicateCheck(MasterEmployeeDTO dto)
        {
            try
            {
                var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_EmailId.Equals(dto.HRME_EmailId)).Count();

                if (resultCout > 0)
                {
                    dto.retrunMsg = "Duplicate";
                    return dto;
                }
            }
            catch (Exception e)
            {
                dto.retrunMsg = "error";
                Console.WriteLine(e.Message);
            }
            return dto;
        }

        //Adharcard Number validation
        public MasterEmployeeDTO AdharcardNumberDuplicateCheck(MasterEmployeeDTO dto)
        {
            try
            {
                if (dto.HRME_Id == 0)
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_AadharCardNo.Equals(dto.HRME_AadharCardNo)).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }
                else
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_AadharCardNo.Equals(dto.HRME_AadharCardNo) && t.HRME_Id != dto.HRME_Id).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }


            }
            catch (Exception e)
            {
                dto.retrunMsg = "error";
                Console.WriteLine(e.Message);
            }
            return dto;
        }

        //PAN Card Number validation
        public MasterEmployeeDTO PANCardNumberDuplicateCheck(MasterEmployeeDTO dto)
        {
            try
            {
                if (dto.HRME_Id == 0)
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_PANCardNo.Equals(dto.HRME_PANCardNo)).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }
                else
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_PANCardNo.Equals(dto.HRME_PANCardNo) && t.HRME_Id != dto.HRME_Id).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }

            }
            catch (Exception e)
            {
                dto.retrunMsg = "error";
                Console.WriteLine(e.Message);
            }
            return dto;
        }


        //Spouse Email Id validation
        public MasterEmployeeDTO SpouseEmailIdDuplicateCheck(MasterEmployeeDTO dto)
        {
            try
            {
                if (dto.HRME_Id == 0)
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_SpouseEmailId.Equals(dto.HRME_SpouseEmailId)).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }
                else
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_SpouseEmailId.Equals(dto.HRME_SpouseEmailId) && t.HRME_Id != dto.HRME_Id).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }

            }
            catch (Exception e)
            {
                dto.retrunMsg = "error";
                Console.WriteLine(e.Message);
            }
            return dto;
        }



        //
        //Spouse mobile Number validation
        public MasterEmployeeDTO SpouseMobileNoDuplicateCheck(MasterEmployeeDTO dto)
        {
            try
            {
                if (dto.HRME_Id == 0)
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_SpouseMobileNo.Equals(dto.HRME_SpouseMobileNo)).Count();

                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }
                else
                {
                    var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_SpouseMobileNo.Equals(dto.HRME_SpouseMobileNo) && t.HRME_Id != dto.HRME_Id).Count();
                    if (resultCout > 0)
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }
                }

            }
            catch (Exception e)
            {
                dto.retrunMsg = "error";
                Console.WriteLine(e.Message);
            }

            return dto;
        }


        //salary calculation

        //validate data  

        public MasterEmployeeDTO SalaryCalculation(HR_Master_Employee_IncrementDetailsDTO dto)
        {
            MasterEmployeeDTO EmpDTO = new MasterEmployeeDTO();
            List<HR_Master_EarningsDeductions> earningdatalist = new List<HR_Master_EarningsDeductions>();
            List<HR_Master_EarningsDeductionsDTO> DTOdatalistEarning = new List<HR_Master_EarningsDeductionsDTO>();
            try
            {


                //Earning list
                earningdatalist = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_EarnDedFlag.Equals("Earning")).ToList();

                if (earningdatalist.Count() > 0)
                {
                    foreach (HR_Master_EarningsDeductions ph in earningdatalist)
                    {
                        HR_Master_EarningsDeductionsDTO phdto = new HR_Master_EarningsDeductionsDTO();

                        phdto.HREED_Amount = 0;

                        if (ph.HRMED_AmountPercentFlag == "Percentage")
                        {
                            var EarningsDeductionsPerlist = _HRMSContext.HR_Master_EarningsDeductionsPer.Where(t => t.MI_Id.Equals(ph.MI_Id) && t.HRMED_Id == ph.HRMED_Id).ToList();

                            if (EarningsDeductionsPerlist.Count() > 0)
                            {
                                List<string> percentOff = new List<string>();

                                foreach (HR_Master_EarningsDeductionsPer headername in EarningsDeductionsPerlist)
                                {
                                    var percentOffHRMED_Name = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_Id.Equals(headername.HRMEDP_HRMED_Id)).ToList();

                                    percentOff.Add(percentOffHRMED_Name.FirstOrDefault().HRMED_Name);

                                    if (percentOffHRMED_Name.FirstOrDefault().HRMED_Name == "Basic Pay")
                                    {
                                        phdto.HREED_Amount = phdto.HREED_Amount + dto.HRMEID_Amount;
                                    }
                                    else
                                    {
                                        phdto.HREED_Amount = phdto.HREED_Amount + Convert.ToInt64(percentOffHRMED_Name.FirstOrDefault().HRMED_AmountPercent);
                                    }



                                }
                                phdto.percentOff = String.Join(" + ", percentOff.ToArray());
                                phdto.HREED_Amount = (Convert.ToInt64(phdto.HRMED_AmountPercent) / 100) * phdto.HREED_Amount;
                            }
                            else
                            {
                                phdto.percentOff = "";
                                phdto.HREED_Amount = Convert.ToDecimal(phdto.HRMED_AmountPercent);
                            }
                        }
                        else
                        {
                            phdto.percentOff = "";
                            phdto.HREED_Amount = Convert.ToDecimal(phdto.HRMED_AmountPercent);
                        }

                        DTOdatalistEarning.Add(phdto);

                    }

                }

                EmpDTO.earningList = DTOdatalistEarning.ToArray();






            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return EmpDTO;
        }

        //onchange into set order fuction 
        public MasterEmployeeDTO employeesetorder(MasterEmployeeDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                //Order updated
                if (dto.EmporderDTO.Count() > 0)
                {
                    foreach (MasterEmployeeDTO mob in dto.EmporderDTO)
                    {
                        if (mob.HRME_Id > 0)
                        {
                            var result = _HRMSContext.MasterEmployee.Single(t => t.HRME_Id == mob.HRME_Id);
                            Mapper.Map(mob, result);
                            _HRMSContext.Update(result);
                            _HRMSContext.SaveChanges();
                        }
                    }
                    dto.retrunMsg = "Order updated successfully";
                }
                else
                {
                    dto.retrunMsg = "No records found to set Order !!!";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }


        public Master_Employee_QulaificationDTO DeleteQualificationRecord(Master_Employee_QulaificationDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                //Delete Existing Record 
                List<Master_Employee_Qulaification> lorg = new List<Master_Employee_Qulaification>();
                lorg = _HRMSContext.Master_Employee_Qulaification.Where(t => t.HRME_Id.Equals(dto.HRME_Id) && t.MI_Id == dto.MI_Id && t.HRMC_Id == dto.HRMC_Id).ToList();

                foreach (Master_Employee_Qulaification ph1 in lorg)
                {

                    _HRMSContext.Remove(ph1);
                    _HRMSContext.SaveChanges();
                    dto.retrunMsg = "Deleted";
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;

        }


        public Master_Employee_ExperienceDTO DeleteExperienceRecord(Master_Employee_ExperienceDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                //Delete Existing Record 
                List<Master_Employee_Experience> lorg = new List<Master_Employee_Experience>();
                lorg = _HRMSContext.Master_Employee_Experience.Where(t => t.HRME_Id.Equals(dto.HRME_Id) && t.MI_Id == dto.MI_Id && t.HRMEE_Id == dto.HRMEE_Id).ToList();

                foreach (Master_Employee_Experience ph1 in lorg)
                {

                    _HRMSContext.Remove(ph1);
                    _HRMSContext.SaveChanges();
                    dto.retrunMsg = "Deleted";
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;

        }


        public Master_Employee_DocumentsDTO DeleteDocumentRecord(Master_Employee_DocumentsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                //Delete Existing Record 
                List<Master_Employee_Documents> lorg = new List<Master_Employee_Documents>();
                lorg = _HRMSContext.Master_Employee_Documents.Where(t => t.HRME_Id.Equals(dto.HRME_Id) && t.MI_Id == dto.MI_Id && t.HRMEDS_Id == dto.HRMEDS_Id).ToList();

                foreach (Master_Employee_Documents ph1 in lorg)
                {

                    _HRMSContext.Remove(ph1);
                    _HRMSContext.SaveChanges();

                    dto.retrunMsg = "Deleted";
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;

        }

        public MasterEmployeeDTO student_mobile_no(MasterEmployeeDTO datastdmobile)
        {
            try
            {
                List<long> temparr = new List<long>();
                List<long> temparr1 = new List<long>();
                //getting all mobilenumbers
                foreach (Mobile_Number_DTO ph in datastdmobile.mobile_list_dto)
                {
                    temparr.Add(ph.HRMEMNO_Id);
                }
                if (temparr.Count() > 0)
                {
                    //removing mobile number 
                    Array Phone_Noresultremove = _HRMSContext.Emp_MobileNo.Where(t => !temparr.Contains(t.HRMEMNO_Id) && t.HRME_Id == datastdmobile.Employeedto.HRME_Id).ToArray();
                    foreach (Multiple_Mobile_DMO ph1 in Phone_Noresultremove)
                    {
                        _HRMSContext.Remove(ph1);
                    }

                }



                //updating and saving 

                foreach (Mobile_Number_DTO ph in datastdmobile.mobile_list_dto)
                {
                    ph.HRME_Id = datastdmobile.Employeedto.HRME_Id;
                    Multiple_Mobile_DMO phone = Mapper.Map<Multiple_Mobile_DMO>(ph);
                    if (phone.HRMEMNO_Id > 0)
                    {

                        var tcnoduplicate23 = _HRMSContext.Emp_MobileNo.Where(d => d.HRMEMNO_MobileNo == ph.HRMEMNO_MobileNo).Count();
                        if (tcnoduplicate23 > 0)
                        {
                            datastdmobile.tcflagexists = "Mobile Number Already Exists";
                            datastdmobile.tcflagexists = "Duplicate";
                        }
                        else {

                        var Phone_Noresult = _HRMSContext.Emp_MobileNo.Single(t => t.HRMEMNO_Id == ph.HRMEMNO_Id);
                        ph.UpdatedDate = DateTime.Now;
                        ph.CreatedDate = Phone_Noresult.CreatedDate;
                        Phone_Noresult.HRMEMNO_MobileNo = ph.HRMEMNO_MobileNo;
                        Mapper.Map(ph, Phone_Noresult);
                        _HRMSContext.Update(Phone_Noresult);

                        foreach (var mob in datastdmobile.mobile_list_dto)
                        {
                            if (mob.HRMEMNO_DeFaultFlag == "default")
                            {
                                datastdmobile.HRMEMNO_MobileNo = mob.HRMEMNO_MobileNo;
                            }
                        }
                    }

                        foreach (var mail in datastdmobile.email_list_dto)
                        {
                            if (mail.HRMEM_DeFaultFlag == "default")
                            {
                                datastdmobile.HRMEM_EmailId = mail.HRMEM_EmailId;
                            }
                        }

                        //ApplicationUser objappuser = from data in _HRMSContext.app

                        //var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(datastdmobile.MI_Id) && t.HRME_MobileNo.Equals(datastdmobile.HRMEMNO_MobileNo)).Count();

                        //if (resultCout > 0)
                        //{
                        //    datastdmobile.retrunMsg = "Duplicate";
                        //    return datastdmobile;
                        //}

                        var tcnoduplicate = _HRMSContext.Emp_Email_Id.Where(d => d.HRMEM_EmailId == datastdmobile.HRMEM_EmailId).Count();
                        if (tcnoduplicate > 0)
                        {
                            datastdmobile.tcflagexists = "Email_Id Already Exists";

                        }

                       else if (datastdmobile.HRMEM_EmailId != null || datastdmobile.HRMEM_EmailId != "")
                        {
                            using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "ApplicationUser_UPDATE";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar)
                                {
                                    Value = Convert.ToString(datastdmobile.HRMEM_EmailId)
                                });

                                cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.BigInt)
                                {
                                    Value = Convert.ToInt64(ph.HRME_Id)
                                });

                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var retObject = new List<dynamic>();
                                try
                                {
                                    using (var dataReader = cmd.ExecuteReader())
                                    {
                                        while (dataReader.Read())
                                        {
                                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                            {
                                                dataRow.Add(
                                                    dataReader.GetName(iFiled),
                                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                                );
                                            }

                                            retObject.Add((ExpandoObject)dataRow);
                                        }
                                    }
                                    // retObject.Add((ExpandoObject)dataRow);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }

                        // var resultCout = _HRMSContext.MasterEmployee.Where(t => t.MI_Id.Equals(datastdmobile.MI_Id) && t.HRME_MobileNo.Equals(datastdmobile.HRMEMNO_MobileNo)).Count();
                        var tcnoduplicate2 = _HRMSContext.Emp_MobileNo.Where(d => d.HRMEMNO_MobileNo == datastdmobile.HRMEMNO_MobileNo).Count();
                        if (tcnoduplicate2 > 0)
                        {
                            datastdmobile.tcflagexists = "Mobile Number Already Exists";
                            datastdmobile.tcflagexists = "Duplicate";
                        }
                    

                        else if (datastdmobile.HRMEMNO_MobileNo != 0)
                        {
                            using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "ApplicationUser_UPDATE_mobile";
                                cmd.CommandType = CommandType.StoredProcedure;


                                cmd.Parameters.Add(new SqlParameter("@PhoneNo", SqlDbType.BigInt)
                                {
                                    Value = Convert.ToInt64(datastdmobile.HRMEMNO_MobileNo)
                                });

                                cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.BigInt)
                                {
                                    Value = Convert.ToInt64(ph.HRME_Id)
                                });

                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var retObject = new List<dynamic>();
                                try
                                {
                                    using (var dataReader = cmd.ExecuteReader())
                                    {
                                        while (dataReader.Read())
                                        {
                                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                            {
                                                dataRow.Add(
                                                    dataReader.GetName(iFiled),
                                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                                );
                                            }

                                            retObject.Add((ExpandoObject)dataRow);
                                        }
                                    }
                                    // retObject.Add((ExpandoObject)dataRow);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                    

                        if (datastdmobile.HRME_Photo != null || datastdmobile.HRME_Photo != "")
                        {
                            using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "ApplicationUser_UPDATE_photo";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@Photo", SqlDbType.NVarChar)
                                {
                                    Value = Convert.ToString(datastdmobile.Employeedto.HRME_Photo)
                                });

                                cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.BigInt)
                                {
                                    Value = Convert.ToInt64(ph.HRME_Id)
                                });

                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var retObject = new List<dynamic>();
                                try
                                {
                                    using (var dataReader = cmd.ExecuteReader())
                                    {
                                        while (dataReader.Read())
                                        {
                                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                            {
                                                dataRow.Add(
                                                    dataReader.GetName(iFiled),
                                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                                );
                                            }

                                            retObject.Add((ExpandoObject)dataRow);
                                        }
                                    }
                                    // retObject.Add((ExpandoObject)dataRow);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }

                      

                    }
                    else
                    {
                        ph.CreatedDate = DateTime.Now;
                        ph.UpdatedDate = DateTime.Now;
                        phone.HRMEMNO_MobileNo = ph.HRMEMNO_MobileNo;
                        Mapper.Map(ph, phone);
                        _HRMSContext.Add(phone);
                    }
                    var count = _HRMSContext.SaveChanges();
                    //var count = _HRMSContext.SaveChanges();
                    if (count > 0)
                    {
                        ApplUser objuser = new ApplUser();
                        objuser = (from a in _HRMSContext.Staff_User_Login
                                   from b in _HRMSContext.applicationuser
                                   where (a.Emp_Code == ph.HRME_Id && a.MI_Id == datastdmobile.MI_Id && a.Id == b.Id)
                                   select b).FirstOrDefault();
                        if (objuser != null)
                        {
                            objuser.PhoneNumber = ph.HRMEMNO_MobileNo.ToString();
                            _HRMSContext.Update(objuser);
                            _HRMSContext.SaveChanges();
                        }
                    }
                }

            }
            catch (Exception e)
            {
                // _log.LogInformation("Student_Mobile error");
                // _log.LogDebug(e.Message);
                Console.WriteLine(e.Message);
                _HRMSContext.Database.RollbackTransaction();
            }
            return datastdmobile;
        }



        //added by sudeep

        //public MasterEmployeeDTO student_mobile_no(MasterEmployeeDTO datastdmobile)
        //{
        //    try
        //    {
        //        List<long> temparr = new List<long>();
        //        List<long> temparr1 = new List<long>();
        //        //getting all mobilenumbers
        //        foreach (Mobile_Number_DTO ph in datastdmobile.mobile_list_dto)
        //        {
        //            temparr.Add(ph.HRMEMNO_Id);
        //        }
        //        if (temparr.Count() > 0)
        //        {
        //            //removing mobile number 
        //            Array Phone_Noresultremove = _HRMSContext.Emp_MobileNo.Where(t => !temparr.Contains(t.HRMEMNO_Id) && t.HRME_Id == datastdmobile.Employeedto.HRME_Id).ToArray();
        //            foreach (Multiple_Mobile_DMO ph1 in Phone_Noresultremove)
        //            {
        //                _HRMSContext.Remove(ph1);
        //            }

        //        }



        //        //updating and saving 

        //        foreach (Mobile_Number_DTO ph in datastdmobile.mobile_list_dto)
        //        {
        //            ph.HRME_Id = datastdmobile.Employeedto.HRME_Id;
        //            Multiple_Mobile_DMO phone = Mapper.Map<Multiple_Mobile_DMO>(ph);
        //            if (phone.HRMEMNO_Id > 0)
        //            {
        //                var Phone_Noresult = _HRMSContext.Emp_MobileNo.Single(t => t.HRMEMNO_Id == ph.HRMEMNO_Id);
        //                ph.UpdatedDate = DateTime.Now;
        //                ph.CreatedDate = Phone_Noresult.CreatedDate;
        //                Phone_Noresult.HRMEMNO_MobileNo = ph.HRMEMNO_MobileNo;
        //                Mapper.Map(ph, Phone_Noresult);
        //                _HRMSContext.Update(Phone_Noresult);
        //            }
        //            else
        //            {
        //                ph.CreatedDate = DateTime.Now;
        //                ph.UpdatedDate = DateTime.Now;
        //                phone.HRMEMNO_MobileNo = ph.HRMEMNO_MobileNo;
        //                Mapper.Map(ph, phone);
        //                _HRMSContext.Add(phone);
        //            }
        //            _HRMSContext.SaveChanges();
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        // _log.LogInformation("Student_Mobile error");
        //        // _log.LogDebug(e.Message);
        //        Console.WriteLine(e.Message);
        //        _HRMSContext.Database.RollbackTransaction();
        //    }
        //    return datastdmobile;
        //}

        // Employee Bank Details

        public MasterEmployeeDTO AddUpdateEmployeeBankDetails(MasterEmployeeDTO datastdmobile)
        {
            try
            {
                List<long> temparr = new List<long>();
                List<long> temparr1 = new List<long>();
                //getting all mobilenumbers
                foreach (HR_Master_Employee_BankDTO ph in datastdmobile.employeebankDTO)
                {
                    temparr.Add(ph.HRMEB_Id);
                }

                if (temparr.Count() > 0)
                {
                    //removing mobile number 
                    Array EmployeeBankDetailsresultremove = _HRMSContext.HR_Master_Employee_Bank.Where(t => !temparr.Contains(t.HRMEB_Id) && t.HRME_Id == datastdmobile.Employeedto.HRME_Id).ToArray();
                    foreach (HR_Master_Employee_Bank ph1 in EmployeeBankDetailsresultremove)
                    {
                        _HRMSContext.Remove(ph1);
                    }
                }



                //updating and saving 

                foreach (HR_Master_Employee_BankDTO ph in datastdmobile.employeebankDTO)
                {
                    ph.HRME_Id = datastdmobile.Employeedto.HRME_Id;
                    HR_Master_Employee_Bank EmployeeBankDetails = Mapper.Map<HR_Master_Employee_Bank>(ph);
                    if (EmployeeBankDetails.HRMEB_Id > 0)
                    {
                        var EmployeeBankDetailsresult = _HRMSContext.HR_Master_Employee_Bank.Single(t => t.HRMEB_Id == ph.HRMEB_Id);
                        ph.UpdatedDate = DateTime.Now;
                        Mapper.Map(ph, EmployeeBankDetailsresult);
                        _HRMSContext.Update(EmployeeBankDetailsresult);
                    }
                    else
                    {
                        ph.CreatedDate = DateTime.Now;
                        ph.UpdatedDate = DateTime.Now;
                        Mapper.Map(ph, EmployeeBankDetails);
                        _HRMSContext.Add(EmployeeBankDetails);
                    }
                    _HRMSContext.SaveChanges();
                }

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                _HRMSContext.Database.RollbackTransaction();
            }
            return datastdmobile;
        }

        public MasterEmployeeDTO student_email_id(MasterEmployeeDTO datastdemail)
        {
            try
            {
                if (datastdemail.email_list_dto != null)
                {
                    List<long> temparr = new List<long>();
                    List<long> temparr1 = new List<long>();
                    //getting all emails
                    foreach (Email_Id_DTO ph in datastdemail.email_list_dto)
                    {
                        temparr.Add(ph.HRMEEM_Id);
                    }

                    if (temparr.Count() > 0)
                    {
                        //removing email number 
                        Array Phone_Noresultremove = _HRMSContext.Emp_Email_Id.Where(t => !temparr.Contains(t.HRMEEM_Id) && t.HRME_Id == datastdemail.Employeedto.HRME_Id).ToArray();
                        foreach (Email_Id_DTO ph1 in Phone_Noresultremove)
                        {
                            _HRMSContext.Remove(ph1);
                        }
                    }


                    //updating and saving 

                    foreach (Email_Id_DTO ph in datastdemail.email_list_dto)
                    {
                        ph.HRME_Id = datastdemail.Employeedto.HRME_Id;

                        Multiple_Email_DMO phone = Mapper.Map<Multiple_Email_DMO>(ph);
                        if (phone.HRMEEM_Id > 0)
                        {
                            var tcnoduplicate = _HRMSContext.Emp_Email_Id.Where(d => d.HRMEM_EmailId == ph.HRMEM_EmailId).Count();
                            if (tcnoduplicate > 0)
                            {
                                datastdemail.tcflagexists = "Email_Id Already Exists";

                            }
                            else
                            {
                                var Phone_Noresult = _HRMSContext.Emp_Email_Id.Single(t => t.HRMEEM_Id == ph.HRMEEM_Id);
                                ph.UpdatedDate = DateTime.Now;
                                ph.CreatedDate = Phone_Noresult.CreatedDate;
                                Phone_Noresult.HRMEM_EmailId = ph.HRMEM_EmailId;
                                Mapper.Map(ph, Phone_Noresult);
                                _HRMSContext.Update(Phone_Noresult);
                            }
                        }
                        else
                        {
                            ph.CreatedDate = DateTime.Now;
                            ph.UpdatedDate = DateTime.Now;
                            //phone.CreatedDate = DateTime.Now;
                            //phone.UpdatedDate = DateTime.Now;
                            phone.HRMEM_EmailId = ph.HRMEM_EmailId;
                            Mapper.Map(ph, phone);
                            _HRMSContext.Add(phone);
                        }

                        var count = _HRMSContext.SaveChanges();
                        if (count > 0)
                        {
                            ApplUser objuser = new ApplUser();
                            objuser = (from a in _HRMSContext.Staff_User_Login
                                       from b in _HRMSContext.applicationuser
                                       where (a.Emp_Code == ph.HRME_Id && a.MI_Id == datastdemail.MI_Id && a.Id == b.Id)
                                       select b).FirstOrDefault();
                            if (objuser != null)
                            {
                                objuser.Email = ph.HRMEM_EmailId.ToString();
                                objuser.NormalizedEmail = ph.HRMEM_EmailId.ToString();
                                _HRMSContext.Update(objuser);
                                _HRMSContext.SaveChanges();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                _HRMSContext.Database.RollbackTransaction();
            }

            return datastdemail;
        }

        //checking duplicate mobile number
        public MasterEmployeeDTO Dupl_mob(Mobile_Number_DTO dto)
        {
            MasterEmployeeDTO dd = new MasterEmployeeDTO();

            try
            {
                if (dto.HRMEMNO_Id == 0)
                {
                    var tcnoduplicate = (from a in _HRMSContext.MasterEmployee
                                         from b in _HRMSContext.Emp_MobileNo
                                         where (a.HRME_Id == b.HRME_Id && a.MI_Id == dto.MI_Id && a.HRME_ActiveFlag == true && b.HRMEMNO_MobileNo == dto.HRMEMNO_MobileNo)
                                         select new Mobile_Number_DTO
                                         {
                                             HRME_Id = a.HRME_Id,
                                             HRMEMNO_MobileNo = b.HRMEMNO_MobileNo
                                         }).ToArray();
                        //_HRMSContext.Emp_MobileNo.Where(d => d.HRMEMNO_MobileNo == dto.HRMEMNO_MobileNo && d.mi_id).Count();
                    if (tcnoduplicate.Length > 0)
                    {
                        dd.tcflagexists = "Mobile Number Already Exists";
                    }
                }
                else
                {
                    var tcnoduplicate = (from a in _HRMSContext.MasterEmployee
                                         from b in _HRMSContext.Emp_MobileNo
                                         where (a.HRME_Id == b.HRME_Id && a.MI_Id == dto.MI_Id && a.HRME_ActiveFlag == true && b.HRMEMNO_MobileNo == dto.HRMEMNO_MobileNo)
                                         select new Mobile_Number_DTO
                                         {
                                             HRME_Id = a.HRME_Id,
                                             HRMEMNO_MobileNo = b.HRMEMNO_MobileNo
                                         }).ToArray();
                    //var tcnoduplicate = _HRMSContext.Emp_MobileNo.Where(d => d.HRMEMNO_MobileNo == dto.HRMEMNO_MobileNo && d.HRMEMNO_Id != dto.HRMEMNO_Id).Count();
                    if (tcnoduplicate.Length > 0)
                    {
                        dd.tcflagexists = "Mobile Number Already Exists";
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dd.retrunMsg = "Error occured";
            }
            return dd;
        }

        //Dupl_email
        public MasterEmployeeDTO Dupl_email(Email_Id_DTO dto)
        {
            MasterEmployeeDTO dd = new MasterEmployeeDTO();
            try
            {
                if (dto.HRMEEM_Id == 0)
                {
                    var tcnoduplicate = (from a in _HRMSContext.MasterEmployee
                                         from b in _HRMSContext.Emp_Email_Id
                                         where (a.HRME_Id == b.HRME_Id && a.MI_Id == dto.MI_Id && a.HRME_ActiveFlag == true && b.HRMEM_EmailId == dto.HRMEM_EmailId)
                                         select new Email_Id_DTO
                                         {
                                             HRME_Id = a.HRME_Id,
                                             HRMEM_EmailId = b.HRMEM_EmailId
                                         }).ToArray();
                    //var tcnoduplicate = _HRMSContext.Emp_Email_Id.Where(d => d.HRMEM_EmailId == dto.HRMEM_EmailId).Count();
                    if (tcnoduplicate.Length > 0)
                    {
                        dd.tcflagexists = "Email_Id Already Exists";

                    }
                }
                else
                {
                    var tcnoduplicate = (from a in _HRMSContext.MasterEmployee
                                         from b in _HRMSContext.Emp_Email_Id
                                         where (a.HRME_Id == b.HRME_Id && a.MI_Id == dto.MI_Id && a.HRME_ActiveFlag == true && b.HRMEM_EmailId == dto.HRMEM_EmailId)
                                         select new Email_Id_DTO
                                         {
                                             HRME_Id = a.HRME_Id,
                                             HRMEM_EmailId = b.HRMEM_EmailId
                                         }).ToArray();
                    //var tcnoduplicate = _HRMSContext.Emp_Email_Id.Where(d => d.HRMEM_EmailId == dto.HRMEM_EmailId && d.HRMEEM_Id != dto.HRMEEM_Id).Count();
                    if (tcnoduplicate.Length > 0)
                    {
                        dd.tcflagexists = "Email_Id Already Exists";
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dd.retrunMsg = "Error occured";
            }
            return dd;
        }
        //duplicate_bankAccountNo
        public MasterEmployeeDTO duplicate_bankAccountNo(HR_Master_Employee_BankDTO dto)
        {
            MasterEmployeeDTO dd = new MasterEmployeeDTO();
            try
            {
                if (dto.HRMEB_Id == 0)
                {
                    var tcnoduplicate = _HRMSContext.HR_Master_Employee_Bank.Where(d => d.HRMEB_AccountNo == dto.HRMEB_AccountNo).Count();
                    if (tcnoduplicate > 0)
                    {
                        dd.tcflagexists = "Account Number Already Exists";

                    }
                }
                else
                {
                    var tcnoduplicate = _HRMSContext.HR_Master_Employee_Bank.Where(d => d.HRMEB_AccountNo == dto.HRMEB_AccountNo && d.HRMEB_Id != dto.HRMEB_Id).Count();
                    if (tcnoduplicate > 0)
                    {
                        dd.tcflagexists = "Account Number Already Exists";

                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dd.retrunMsg = "Error occured";
            }
            return dd;
        }



        //del_mob
        public MasterEmployeeDTO del_mob(Mobile_Number_DTO dto)
        {

            MasterEmployeeDTO dd = new MasterEmployeeDTO();


            dd.retrunMsg = "";
            try
            {

                List<Multiple_Mobile_DMO> lorg = new List<Multiple_Mobile_DMO>();
                lorg = _HRMSContext.Emp_MobileNo.Where(t => t.HRMEMNO_Id.Equals(dto.HRMEMNO_Id)).ToList();

                foreach (Multiple_Mobile_DMO ph1 in lorg)
                {

                    _HRMSContext.Remove(ph1);
                    _HRMSContext.SaveChanges();
                    dd.retrunMsg = "Deleted";
                }
            }
            //Delete Existing Record 
            // List<Master_Employee_Qulaification> lorg = new List<Master_Employee_Qulaification>();

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dd.retrunMsg = "Error occured";
            }

            return dd;



        }
        //del_email

        public MasterEmployeeDTO del_email(Email_Id_DTO dto)
        {

            MasterEmployeeDTO dd = new MasterEmployeeDTO();


            dd.retrunMsg = "";
            try
            {

                List<Multiple_Email_DMO> lorg = new List<Multiple_Email_DMO>();
                lorg = _HRMSContext.Emp_Email_Id.Where(t => t.HRMEEM_Id.Equals(dto.HRMEEM_Id)).ToList();

                foreach (Multiple_Email_DMO ph1 in lorg)
                {

                    _HRMSContext.Remove(ph1);
                    _HRMSContext.SaveChanges();
                    dd.retrunMsg = "Deleted";
                }
            }
            //Delete Existing Record 
            // List<Master_Employee_Qulaification> lorg = new List<Master_Employee_Qulaification>();

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dd.retrunMsg = "Error occured";
            }

            return dd;



        }


        public MasterEmployeeDTO delete_bankAccountNo(HR_Master_Employee_BankDTO dto)
        {

            MasterEmployeeDTO dd = new MasterEmployeeDTO();


            dd.retrunMsg = "";
            try
            {

                List<HR_Master_Employee_Bank> lorg = new List<HR_Master_Employee_Bank>();
                lorg = _HRMSContext.HR_Master_Employee_Bank.Where(t => t.HRMEB_Id.Equals(dto.HRMEB_Id)).ToList();

                foreach (HR_Master_Employee_Bank ph1 in lorg)
                {

                    _HRMSContext.Remove(ph1);
                    _HRMSContext.SaveChanges();
                    dd.retrunMsg = "Deleted";
                }
            }
            //Delete Existing Record 
            // List<Master_Employee_Qulaification> lorg = new List<Master_Employee_Qulaification>();

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dd.retrunMsg = "Error occured";
            }

            return dd;



        }


        //OnLoadSaveEmployeeSalaryHeads
        public MasterEmployeeDTO OnLoadSaveEmployeeSalaryHeads(MasterEmployeeDTO dto)
        {
            //Earning list
            List<HR_Master_EarningsDeductions> masterheadlist = new List<HR_Master_EarningsDeductions>();
            try
            {
                var EmployeeHRMED_Idlist = _HRMSContext.HR_Employee_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_Id.Equals(dto.HRME_Id)).Select(t => t.HRMED_Id);
                if (EmployeeHRMED_Idlist.Count() > 0)
                {
                    masterheadlist = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && !EmployeeHRMED_Idlist.Contains(t.HRMED_Id) && t.HRMED_ActiveFlag == true).ToList();
                }
                else
                {
                    masterheadlist = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_ActiveFlag == true).ToList();
                }

                if (masterheadlist.Count > 0)
                {

                    foreach (HR_Master_EarningsDeductions ph in masterheadlist)
                    {
                        HR_Employee_EarningsDeductionsDTO EmployeeDetails = new HR_Employee_EarningsDeductionsDTO();
                        EmployeeDetails.HRMED_Id = ph.HRMED_Id;
                        EmployeeDetails.HRME_Id = dto.HRME_Id;
                        EmployeeDetails.MI_Id = dto.MI_Id;
                        if (ph.HRMED_AmountPercentFlag == "Amount")
                        {
                            EmployeeDetails.HREED_Amount = Convert.ToDecimal(ph.HRMED_AmountPercent);
                            EmployeeDetails.HREED_Percentage = "0";
                        }
                        else
                        {
                            EmployeeDetails.HREED_Percentage = ph.HRMED_AmountPercent;
                            EmployeeDetails.HREED_Amount = 0;
                        }
                        EmployeeDetails.HREED_ActiveFlag = true;
                        //EmployeeDetails. = dto.MI_Id;
                        //EmployeeDetails.MI_Id = dto.MI_Id;
                        HR_Employee_EarningsDeductions phdto = Mapper.Map<HR_Employee_EarningsDeductions>(EmployeeDetails);
                        _HRMSContext.Add(phdto);
                        var flag = _HRMSContext.SaveChanges();
                        if (flag > 0)
                        {
                            // dto.retrunMsg = "Add";
                        }
                        else
                        {
                            // dto.retrunMsg = "false";
                        }

                    }
                }



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }


            return dto;
        }


        public MasterEmployeeDTO getEmployeeSalaryDetails(MasterEmployeeDTO dto)
        {
            List<HR_Master_EarningsDeductions> mstheads = new List<HR_Master_EarningsDeductions>();

            try
            {
                OnLoadSaveEmployeeSalaryHeads(dto);

                HR_Employee_EarningsDeductionsDTO phdto = new HR_Employee_EarningsDeductionsDTO();
                phdto.MI_Id = dto.MI_Id;
                phdto.HRME_Id = dto.HRME_Id;
                //Calculating the standard salarycalculation
                CalculateEmployeeEarnDeductionDetailsByHead(phdto);

                //Employee Earning & deduction details

                List<HR_Employee_EarningsDeductionsDTO> EarningsDeductionsDetails = new List<HR_Employee_EarningsDeductionsDTO>();
                //EarningsDeductionsDetails = _HRMSContext.HR_Employee_EarningsDeductions.AsNoTracking().Where(t => t.HRME_Id.Equals(id)).ToList();


                EarningsDeductionsDetails = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                                             from med in _HRMSContext.HR_Master_EarningsDeductions
                                             where emp.HRMED_Id == med.HRMED_Id && med.HRMED_ActiveFlag == true
                                             && emp.HRME_Id == dto.HRME_Id
                                             select new HR_Employee_EarningsDeductionsDTO
                                             {
                                                 HREED_Id = emp.HREED_Id,
                                                 HRMED_Id = emp.HRMED_Id,
                                                 HRME_Id = emp.HRME_Id,
                                                 MI_Id = emp.MI_Id,
                                                 HREED_Amount = emp.HREED_Amount,
                                                 HREED_Percentage = emp.HREED_Percentage,
                                                 HRMED_EarnDedFlag = med.HRMED_EarnDedFlag,
                                                 HREED_ActiveFlag = emp.HREED_ActiveFlag,
                                                 HREED_MaxApplicableFlg = emp.HREED_MaxApplicableFlg,
                                                 HREED_ApplicableMaxValue = emp.HREED_ApplicableMaxValue,


                                             }
                                ).ToList();


                dto.employeeEarningsDeductionsDetails = EarningsDeductionsDetails.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

        public MasterEmployeeDTO getEmployeeSalaryDetailsByHead(HR_Employee_EarningsDeductionsDTO dto)
        {
            MasterEmployeeDTO empdto = new MasterEmployeeDTO();
            try
            {
                if (dto.HREED_Id > 0)
                {
                    var Documentsresult = _HRMSContext.HR_Employee_EarningsDeductions.Single(t => t.HREED_Id == dto.HREED_Id);
                    //added by 20/05/2017   
                    // DocumentsDTO.HREED_ActiveFlag = true;
                    Mapper.Map(dto, Documentsresult);
                    _HRMSContext.Update(Documentsresult);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        CalculateEmployeeEarnDeductionDetailsByHead(dto);
                    }
                    else
                    {
                        // dto.retrunMsg = "false";
                    }

                }


                List<HR_Employee_EarningsDeductionsDTO> EarningsDeductionsDetails = new List<HR_Employee_EarningsDeductionsDTO>();

                EarningsDeductionsDetails = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                                             from med in _HRMSContext.HR_Master_EarningsDeductions
                                             where emp.HRMED_Id == med.HRMED_Id && med.HRMED_ActiveFlag == true
                                             && emp.HRME_Id == dto.HRME_Id
                                             select new HR_Employee_EarningsDeductionsDTO
                                             {
                                                 HREED_Id = emp.HREED_Id,
                                                 HRMED_Id = emp.HRMED_Id,
                                                 HRME_Id = emp.HRME_Id,
                                                 MI_Id = emp.MI_Id,
                                                 HREED_Amount = emp.HREED_Amount,
                                                 HREED_Percentage = emp.HREED_Percentage,
                                                 HRMED_EarnDedFlag = med.HRMED_EarnDedFlag,
                                                 HREED_ActiveFlag = emp.HREED_ActiveFlag,


                                             }
                                ).ToList();


                empdto.employeeEarningsDeductionsDetails = EarningsDeductionsDetails.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return empdto;
        }

        public HR_Employee_EarningsDeductionsDTO CalculateEmployeeEarnDeductionDetailsByHead(HR_Employee_EarningsDeductionsDTO dto)
        {
            try
            {
                //var empheadDetail = _HRMSContext.HR_Master_EarningsDeductions.SingleOrDefault(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_Id == dto.HRMED_Id);

                //var AmountPercentage = "";
                //if (empheadDetail.HRMED_AmountPercentFlag.Equals("Amount"))
                //{
                //    AmountPercentage = dto.HREED_Amount.ToString();
                //}
                //else
                //{
                //    AmountPercentage = dto.HREED_Percentage;
                //}

                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "EmployeeSalaryDetailDynamicCalculation";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                        SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.HRME_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_ID",
                       SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {

                        // var data = cmd.ExecuteNonQuery();

                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public MasterEmployeeDTO getcaste(MasterEmployeeDTO data)
        {
            var AllCastess = (from a in _HRMSContext.Caste
                              from b in _HRMSContext.CasteCategory
                              where (a.IMCC_Id == b.IMCC_Id && a.MI_Id == data.MI_Id && a.IMCC_Id == data.CasteCategoryId)
                              select a).ToArray();

            data.castedropdownlist = AllCastess.Distinct().ToArray();
            return data;
        }

        public MasterEmployeeDTO getcastecatgory(MasterEmployeeDTO data)
        {
            var Allcastecatgory = (from a in _HRMSContext.ReligionCategory_MappingDMO
                               from b in _HRMSContext.CasteCategory
                               where (a.IVRMMR_Id == data.ReligionId && a.IMCC_Id == b.IMCC_Id && a.IRCC_ActiveFlg == true)select new CasteCategory
                               {
                                   IMCC_Id = a.IMCC_Id,
                                   IMCC_CategoryName = b.IMCC_CategoryName
                               }).ToArray();

            data.casteCategorydropdownlist = Allcastecatgory.Distinct().ToArray();
            return data;
        }

        public async Task<MasterEmployeeDTO> Feeconcession(MasterEmployeeDTO dto)
        {
            try
            {
                dto = await Feeconcessionprocedure(dto, "JSH_Employee_left_fee_Concession");

            }
            catch
            {

            }

            return dto;

        }

        public async Task<MasterEmployeeDTO> Feeconcessionprocedure(MasterEmployeeDTO dto, string ProcedureName)
        {

            try
            {
                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = ProcedureName;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_ID",
                      SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                        SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(dto.HRME_Id)
                    });



                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {



                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public void UpdateLeftEmployee(MasterEmployeeDTO dto)
        {
            if (dto.Employeedto.HRME_LeftFlag == true)
            {
                var feecheck = _HRMSContext.Fee_Master_ConcessionDMO.Where(t => t.MI_Id == dto.MI_Id && t.FMCC_ConcessionFlag == "E").ToList();
                if (feecheck.Count > 0)
                {
                    try
                    {
                        var academic_year = _Context.AcademicYear.Where(e => e.MI_Id == dto.MI_Id && DateTime.Now.Date >= e.ASMAY_From_Date && DateTime.Now.Date <= e.ASMAY_To_Date).Select(t=>t.ASMAY_Id).FirstOrDefault();

                        using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "DELETEEMPCONCESSION";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MI_ID",
                              SqlDbType.BigInt)
                            {
                                Value = Convert.ToInt64(dto.MI_Id)
                            });
                            cmd.Parameters.Add(new SqlParameter("@EMPCDE",
                                SqlDbType.VarChar)
                            {
                                Value = Convert.ToInt64(dto.Employeedto.HRME_Id)
                            });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                                SqlDbType.VarChar)
                            {
                                Value = Convert.ToInt64(academic_year)
                            });
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();
                            var retObject = new List<dynamic>();
                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                        {
                                            dataRow.Add(
                                                dataReader.GetName(iFiled),
                                                dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                            );
                                        }
                                        retObject.Add((ExpandoObject)dataRow);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
            }
        }

        public MasterEmployeeDTO getdepartment(MasterEmployeeDTO data)
        {
            var Alldepartments = (from a in _HRMSContext.HRGroupDeptDessgDMO
                              from b in _HRMSContext.HR_Master_Department
                              where (a.HRMD_Id == b.HRMD_Id && a.MI_Id == data.MI_Id && a.HRMGT_Id == data.HRMGT_Id && a.HRGTDDS_ActiveFlg == true) 
                              select b).OrderBy(t=>t.HRMD_Order).ToArray();
            data.departmentdropdownlist = Alldepartments.Distinct().ToArray();
            return data;
        }

        public MasterEmployeeDTO getdesignation(MasterEmployeeDTO data)
        {
            var Alldesignation = (from a in _HRMSContext.HRGroupDeptDessgDMO
                                  from b in _HRMSContext.HR_Master_Designation
                              where (a.HRMDES_Id == b.HRMDES_Id && a.HRMGT_Id == data.HRMGT_Id && a.MI_Id == data.MI_Id && a.HRMD_Id == data.HRMD_Id && a.HRGTDDS_ActiveFlg == true)
                              select b).OrderBy(t=>t.HRMDES_Order).ToArray();
            data.designationdropdownlist = Alldesignation.Distinct().ToArray();
            return data;
        }
    }
}
