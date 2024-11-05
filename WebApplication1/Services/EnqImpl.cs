using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using WebApplication1.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DomainModel.Model;
using AutoMapper;
using System.Collections.Concurrent;
using CommonLibrary;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using System.Data.SqlClient;
using System.Dynamic;
using System.Data;
using DomainModel.Model.com.vapstech.College.Fees;

namespace WebApplication1.Services
{
    public class EnqImpl : Interfaces.EnquiryInterface
    {

        private static ConcurrentDictionary<string, EnqDTO> _login =
               new ConcurrentDictionary<string, EnqDTO>();

        public Enquirycontext _Enquirycontext;
        public DomainModelMsSqlServerContext _context;
        //ILogger<EnqImpl> _logger;
        readonly ILogger<EnqImpl> _logger;

        public EnqImpl(Enquirycontext Enquirycontext, DomainModelMsSqlServerContext context, ILogger<EnqImpl> log)
        {
            _Enquirycontext = Enquirycontext;
            _context = context;
            _logger = log;
        }
        public Enq countrydrp(Enq en)
        {
            _logger.LogInformation("Hello, world!");
            _logger.LogDebug("Get start details");

            try
            {

                List<Country> allCountry = new List<Country>();
                allCountry = _Enquirycontext.country.ToList();
                en.countryDrpDown = allCountry.ToArray();

                List<State> allstate = new List<State>();
                allstate = _Enquirycontext.state.ToList();
                en.stateDrpDown = allstate.ToArray();

                List<Enquiry> alldetails = new List<Enquiry>();
                alldetails = _Enquirycontext.Enquir.Where(t => t.Id.Equals(en.Id) && t.MI_Id.Equals(en.MI_Id)).OrderBy(t => t.CreatedDate).ToList();
                en.enqdata = alldetails.ToArray();

                //Added By Sripad Joshi
                //For loading year,class,reference and source Dropdown.

                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _Enquirycontext.academicyear.Where(t => t.MI_Id.Equals(en.MI_Id) && t.ASMAY_Id.Equals(en.ASMAY_Id)).ToList();
                en.yearDrpDwn = allyear.ToArray();

                List<School_M_Class> allclass = new List<School_M_Class>();
                allclass = _context.School_M_Class.Where(t => t.MI_Id == en.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                en.classDrpDwn = allclass.ToArray();

                List<MasterCategory> allcategory = new List<MasterCategory>();
                allcategory = _context.mastercategory.Where(t => t.MI_Id == en.MI_Id && t.AMC_ActiveFlag == 1).ToList();
                en.categoryDrpDwn = allcategory.ToArray();

                //if (en.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                //{
                //    en.GeneratedNumber = GenerateEnquiryNumber(en.MI_Id, en.ASMAY_Id, en);
                //}
                //else if (en.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Manual")
                //{
                //    en.GeneratedNumber = "";
                //}
            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
                _logger.LogError(e.Message);
            }

            return en;
        }


        public static string GetCurrentFinancialYear()
        {
            int CurrentYear = DateTime.Today.Year;
            int PreviousYear = DateTime.Today.Year - 1;
            int NextYear = DateTime.Today.Year + 1;
            string PreYear = PreviousYear.ToString();
            string NexYear = NextYear.ToString();
            string CurYear = CurrentYear.ToString();
            string FinYear = null;

            if (DateTime.Today.Month > 3)
                FinYear = CurYear;
            else
                FinYear = PreYear;
            return FinYear.Trim();
        }

        //public Enq createdata(Enq en)
        //{
        //    en.PASE_Date = DateTime.UtcNow;
        //    _Enquirycontext.Add(en);
        //    _Enquirycontext.SaveChanges();
        //    return en;


        //}

        public Enq clearEnqdata(int Id)
        {
            bool returnval = true;
            Id = 9;
            Enq org = new Enq();
            List<Enquiry> enquirys = new List<Enquiry>();

            var enqui = _Enquirycontext.Enquir.Where(t => t.PASE_Id.Equals(Id));
            //   enquirys = _Enquirycontext.Enquir.Where(t => t.PASE_Id.Equals(Id));

            if (enqui.Any())
            {
                _Enquirycontext.Remove(enquirys.ElementAt(0));

                _Enquirycontext.SaveChanges();
            }

            if (enqui.Count() > 0)
            {
                returnval = true;
            }
            else
            {
                returnval = false;
            }

            return org;
        }

        public StateDTO enqdrpcountrydata(int id)
        {


            Array[] drpall = new Array[3];
            //CountryDTO[] coun=new CountryDTO[] 
            //    {
            //        new CountryDTO {IVRMMC_Id = 101 ,IVRMMC_CountryName ="India" ,MO_Name="hutching"},
            //        new CountryDTO {IVRMMC_Id = 101 ,IVRMMC_CountryName ="India" ,MO_Name="khosys" },
            //          new CountryDTO {IVRMMC_Id = 101 ,IVRMMC_CountryName ="India" ,MO_Name="St.Philomina"},


            //    } ;

            StateDTO enq = new StateDTO();
            List<State> allstate = new List<State>();
            //     allstate = _Enquirycontext.country.Where(t => t.IVRMMC_Id.Equals(id)).FirstO;
            // allstate.Add(new (101, "India"));

            allstate = _Enquirycontext.state.ToList();
            allstate = _Enquirycontext.state.Where(t => t.IVRMMC_Id.Equals(id)).ToList();
            enq.stateDrpDown = allstate.ToArray();




            return enq;

            //ViewBag.Country = "UK";
        }

        public CityDTO getcity(int id)
        {
            Array[] drpall = new Array[3];
            CityDTO enq = new CityDTO();
            List<City> allcity = new List<City>();
            allcity = _Enquirycontext.city.ToList();
            //allcity = _OrganisationContext.city.Where(t => t.IVRMMS_Id.Equals(stateid) && t.IVRMMC_Id.Equals(stateid)).ToList();
            allcity = _Enquirycontext.city.Where(t => t.IVRMMS_Id.Equals(id)).ToList();
            enq.cityDrpDown = allcity.ToArray();
            return enq;
        }

        public async Task<Enq> saveEnqdata(Enq enqu)
        {
            enqu.returnMsg = "";
            Enquiry enq = Mapper.Map<Enquiry>(enqu);
            try
            {
                if (enq.PASE_Id > 0)
                {

                    var result1 = _Enquirycontext.Enquir.Where(t => (t.PASE_FirstName == enqu.PASE_FirstName && t.PASE_MiddleName == enqu.PASE_MiddleName &&
                    t.PASE_LastName == enq.PASE_LastName && t.PASE_Id != enq.PASE_Id));
                    if (result1.Count() >= 1)
                    {
                        enqu.returnduplicatestatus = "Duplicate";

                    }
                    else
                    {

                        var result = _Enquirycontext.Enquir.Single(t => t.PASE_Id == enq.PASE_Id);
                        result.AMC_Id = enq.AMC_Id;
                        result.ASMAY_Id = enq.ASMAY_Id;
                        result.IVRMMC_Id = enq.IVRMMC_Id;
                        result.ASMCL_Id = enq.ASMCL_Id;
                        // result.MI_Id = enq.MI_Id;
                        result.PASE_Address1 = enq.PASE_Address1;
                        result.PASE_Address2 = enq.PASE_Address2;
                        result.PASE_Address3 = enq.PASE_Address3;
                        result.PASE_Pincode = enq.PASE_Pincode;
                        result.PASE_MobileNo = enq.PASE_MobileNo;
                        result.PASE_Phone = enq.PASE_Phone;
                        result.PASE_emailid = enq.PASE_emailid;
                        result.PASE_City = enq.PASE_City;
                        result.PASE_State = enq.PASE_State;
                        result.PASE_EnquiryNo = enq.PASE_EnquiryNo;
                        result.PASE_FirstName = enq.PASE_FirstName;
                        result.PASE_MiddleName = enq.PASE_MiddleName;
                        result.PASE_LastName = enq.PASE_LastName;
                        result.PASE_EnquiryDetails = enq.PASE_EnquiryDetails;

                        TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                        DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);


                        result.UpdatedDate = indianTime;

                        // result.PASE_Date = enq.PASE_Date;
                        // result.PASE_ActiveFlag = enq.PASE_ActiveFlag;
                        //added by 02/02/2017

                        result.UpdatedDate = DateTime.Now;
                        _Enquirycontext.Update(result);

                        var flag = _Enquirycontext.SaveChanges();
                        if (flag == 1)
                        {
                            enqu.returnval = true;
                            enqu.returnMsg = "u";
                            SMS sms = new SMS(_context);

                            string s = await sms.sendSms(enq.MI_Id, result.PASE_MobileNo, "ENQUIRY", enq.PASE_Id);

                            Email Email = new Email(_context);

                            string m = Email.sendmail(enq.MI_Id, result.PASE_emailid, "ENQUIRY", enq.PASE_Id);

                        }
                        else
                        {
                            enqu.returnMsg = "u";
                            enqu.returnval = false;
                        }
                    }
                }
                else
                {
                    //var Emailcount = _Enquirycontext.Enquir.Where(t => t.MI_Id == enq.MI_Id && t.PASE_emailid == enq.PASE_emailid).Count();
                    //if (Emailcount > 0)
                    //{
                    //    enqu.returnMsg = "Email Id is already exist.!";
                    //    enqu.returnval = false;
                    //    return enqu;
                    //}
                    //var MobileNocount = _Enquirycontext.Enquir.Where(t => t.MI_Id == enq.MI_Id && t.PASE_MobileNo == enq.PASE_MobileNo).Count();
                    //if (MobileNocount > 0)
                    //{
                    //    enqu.returnMsg = "Mobile Number is already exist.!";
                    //    enqu.returnval = false;
                    //    return enqu;
                    //}

                    //Get Autogenerated Enquiry number


                    if (enqu.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                    {
                        GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
                        enqu.transnumbconfigurationsettingsss.MI_Id = enq.MI_Id;
                        enqu.transnumbconfigurationsettingsss.ASMAY_Id = enq.ASMAY_Id;
                        enq.PASE_EnquiryNo = a.GenerateNumber(enqu.transnumbconfigurationsettingsss);

                        // enq.PASE_EnquiryNo = GenerateEnquiryNumber(enq.MI_Id, enq.ASMAY_Id, enqu).ToString();
                    }
                    else
                    {
                        enq.PASE_EnquiryNo = enqu.PASE_EnquiryNo;

                        var count = _Enquirycontext.Enquir.Where(t => t.MI_Id == enq.MI_Id && t.PASE_EnquiryNo == enq.PASE_EnquiryNo).Count();
                        if (count > 0)
                        {
                            enqu.returnMsg = "Enquiry No is already exist.!";
                            enqu.returnval = false;
                            return enqu;
                        }
                    }
                    //  enq.PASE_EnquiryNo = enqu.PASE_EnquiryNo;

                    /////////////////

                    enq.PASE_ActiveFlag = 1;
                    TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                    enq.PASE_Date = indianTime;
                    //added by 02/02/2017


                    enq.CreatedDate = indianTime;
                    enq.UpdatedDate = indianTime;



                    _Enquirycontext.Add(enq);

                    var flag = _Enquirycontext.SaveChanges();

                    if (flag == 1)
                    {
                        enqu.returnMsg = "a";
                        enqu.returnval = true;

                        SMS sms = new SMS(_context);

                        string s = await sms.sendSms(enq.MI_Id, enq.PASE_MobileNo, "ENQUIRY", enq.PASE_Id);

                        Email Email = new Email(_context);

                        string m = Email.sendmail(enq.MI_Id, enq.PASE_emailid, "ENQUIRY", enq.PASE_Id);

                    }
                    else
                    {
                        enqu.returnMsg = "a";
                        enqu.returnval = false;
                    }
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return enqu;
        }

        public Enq EditDetails(int id)
        {
            Enq enquirydetails = new Enq();
            try
            {

                List<Enquiry> enq = new List<Enquiry>();
                enq = _Enquirycontext.Enquir.AsNoTracking().Where(t => t.PASE_Id.Equals(id)).ToList();
                enquirydetails.enqdata = enq.ToArray();

                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _Enquirycontext.academicyear.Where(t => t.ASMAY_Id.Equals(enq[0].ASMAY_Id)).ToList();
                enquirydetails.yearDrpDwn = allyear.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enquirydetails;
        }

        public Enq DeleteEnqDetails(Enq enquirydetails)
        {

            //Enq enquirydetails = new Enq();
            List<Enquiry> enquiry = new List<Enquiry>(); // Mapper.Map<Organisation>(org);
            //_OrganisationContext.Remove(enq);
            //_OrganisationContext.SaveChanges();

            try
            {
                enquiry = _Enquirycontext.Enquir.Where(t => t.PASE_Id.Equals(enquirydetails.PASE_Id)).ToList();

                if (enquiry.Any())
                {
                    _Enquirycontext.Remove(enquiry.ElementAt(0));

                    var flag = _Enquirycontext.SaveChanges();
                    if (flag == 1)
                    {
                        enquirydetails.returnval = true;
                    }
                    else
                    {
                        enquirydetails.returnval = false;
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enquirydetails;
        }

        //public Enq GetAllDetais(Enq en)
        //{
        //    List<Enquiry> allEnquiryDetails = new List<Enquiry>();
        //    Enquiry enq = Mapper.Map<Enquiry>(en);

        //    var result = _Enquirycontext.Enquir.Single(t => t.PASE_Id==enq.PASE_Id);


        //    result.PASE_Phone= enq.PASE_Phone;
        //    result.PASE_emailid = enq.PASE_emailid;
        //    result.PASE_FirstName = enq.PASE_FirstName;



        //    allEnquiryDetails = _Enquirycontext.Enquir.ToList();
        //    en.enqdata = allEnquiryDetails.ToArray();

        //    return en;
        //}


        //Dashboard Mapping
        public dasAzure_StorageDTO storageDetails(dasAzure_StorageDTO id)
        {
            dasAzure_StorageDTO enq = new dasAzure_StorageDTO();

            enq.rowdata = _context.IVRM_Storage_path_Details.ToArray();
            enq.roledata = _context.MasterRoleType.ToArray();

            enq.userdata = _context.MasterRoleType.Where(u => u.flag == "U").ToArray();
            enq.institutionlist = _context.Institute.ToArray();

            //List<Dashboard_page_mapping> allpages = new List<Dashboard_page_mapping>();
            //allpages = _context.Dashboard_page_mapping.ToList();
            //enq.mappingdata = allpages.OrderByDescending(c => c.IVRM_CreatedDate).ToArray();
            enq.preadmissionmapping = (from a in _context.Preadmission_Dashboard_PageDMO
                                       from b in _context.Institute
                                       where a.PAPG_MIID == b.MI_Id
                                       select new dasMappingDTO
                                       {
                                           PAPG_ID = a.PAPG_ID,
                                           PAPG_PAGENAME = a.PAPG_PAGENAME,
                                           PAPG_MIID = a.PAPG_MIID,
                                           MI_Name = b.MI_Name
                                       }).ToArray();

            enq.mappingdata = (from a in _context.Dashboard_page_mapping
                                from b in _context.Institute
                                where a.MI_ID == b.MI_Id
                                select new dasMappingDTO
                                {
                                    IVRMP_Dasboard_PageName = a.IVRMP_Dasboard_PageName,
                                    IVRMRT_Role = a.IVRMRT_Role,
                                    returnMsg = b.MI_Name,
                                    IVRM_DBID = a.IVRM_DBID
                                }).ToArray();
            //enq.rowdata = row.ToArray();

            //List<UserRoleWithInstituteDMO> allinstitution = new List<UserRoleWithInstituteDMO>();
            //allinstitution = _context.UserRoleWithInstituteDMO.ToList();
            //enq.gridalldata = allpages.OrderByDescending(c => c.CreatedDate).ToArray();
            return enq;
        }

        public dasAzure_StorageDTO editstorage(int id)
        {
            dasAzure_StorageDTO page = new dasAzure_StorageDTO();
            try
            {
                List<IVRM_Storage_path_Details> lorg = new List<IVRM_Storage_path_Details>();
                lorg = _context.IVRM_Storage_path_Details.AsNoTracking().Where(t => t.IVRM_SD.Equals(id)).ToList();
                page.roweditdata = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public dasAzure_StorageDTO saveStoragedetails(dasAzure_StorageDTO page)
        {
            bool returnresult = false;
            page.returnMsg = "";
            try
            {
                IVRM_Storage_path_Details maspge = Mapper.Map<IVRM_Storage_path_Details>(page);


                // var resultCount = _context.IVRM_Storage_path_Details.Count();

                if (page.IVRM_SD > 0)
                {
                    var result = _context.IVRM_Storage_path_Details.Single(t => t.IVRM_SD == maspge.IVRM_SD);

                    result.IVRM_SD_Access_Name = maspge.IVRM_SD_Access_Name;
                    result.IVRM_SD_Access_Key = maspge.IVRM_SD_Access_Key;
                    result.IVRM_VMS_Subscription_URL = maspge.IVRM_VMS_Subscription_URL;

                    _context.Update(result);
                    var contactExists = _context.SaveChanges();

                    if (contactExists == 1)
                    {
                        returnresult = true;

                        page.returnMsg = "update";
                    }
                    else
                    {
                        returnresult = false;

                    }
                }
                else
                {
                    var resultCount = _context.IVRM_Storage_path_Details.Count();
                    if (resultCount > 0)
                    {
                        page.returnMsg = "noupdate";
                    }
                    else if (resultCount == 0)
                    {
                        _context.Add(maspge);
                        var contactExists = _context.SaveChanges();

                        if (contactExists == 1)
                        {
                            returnresult = true;
                            page.returnMsg = "add";
                        }
                    }




                }
                List<IVRM_Storage_path_Details> allpages = new List<IVRM_Storage_path_Details>();
                page.rowdata = _context.IVRM_Storage_path_Details.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return page;
        }

        public dasMappingDTO saveMappingdetail(dasMappingDTO page)
        {
            bool returnresult = false;
            page.returnMsg = "";
            try
            {
                Dashboard_page_mapping maspge = Mapper.Map<Dashboard_page_mapping>(page);

                if (page.IVRM_DBID > 0)
                {
                    var resultCount = _context.Dashboard_page_mapping.Where(t => t.IVRMRT_Role == maspge.IVRMRT_Role && t.IVRMP_Dasboard_PageName == page.IVRMP_Dasboard_PageName && t.MI_ID == page.MI_Id).Count();

                    if (resultCount == 0)
                    {
                        var result = _context.Dashboard_page_mapping.Single(t => t.IVRM_DBID == maspge.IVRM_DBID);

                        result.IVRMP_Dasboard_PageName = maspge.IVRMP_Dasboard_PageName;
                        result.IVRMRT_Role = maspge.IVRMRT_Role;
                        result.IVRM_UpdatedDate = DateTime.Now;
                        _context.Update(result);
                        var contactExists = _context.SaveChanges();

                        if (contactExists == 1)
                        {
                            returnresult = true;
                            page.returnval = returnresult;
                            page.returnMsg = "update";
                        }
                        else
                        {
                            returnresult = false;
                            page.returnval = returnresult;
                        }
                    }
                    else
                    {
                        page.returnMsg = "duplicate";
                        return page;
                    }
                }
                else
                {
                    var resultCount = _context.Dashboard_page_mapping.Where(t => t.IVRMRT_Role == maspge.IVRMRT_Role && t.IVRMP_Dasboard_PageName == maspge.IVRMP_Dasboard_PageName && t.MI_ID == maspge.MI_ID).Count();

                    if (resultCount > 0)
                    {
                        page.returnMsg = "duplicate";
                        return page;
                    }
                    //added by 02/02/2017
                    maspge.IVRM_CreatedDate = DateTime.Now;
                    maspge.IVRM_UpdatedDate = DateTime.Now;

                    _context.Add(maspge);
                    var contactExists = _context.SaveChanges();

                    if (contactExists == 1)
                    {
                        returnresult = true;
                        page.returnval = returnresult;
                        page.returnMsg = "add";
                    }
                    else
                    {
                        returnresult = false;
                        page.returnval = returnresult;
                    }
                }

                //List<Dashboard_page_mapping> allpages = new List<Dashboard_page_mapping>();
                //allpages = _context.Dashboard_page_mapping.ToList();
                //page.mappingdata = allpages.OrderByDescending(c => c.IVRM_CreatedDate).ToArray();



                page.mappingdata = (from a in _context.Dashboard_page_mapping
                                    from b in _context.Institute
                                           where a.MI_ID == b.MI_Id 
                                           select new dasMappingDTO
                                           {
                                               IVRMP_Dasboard_PageName=a.IVRMP_Dasboard_PageName,
                                               IVRMRT_Role=a.IVRMRT_Role,
                                               returnMsg = b.MI_Name,
                                               IVRM_DBID = a.IVRM_DBID
                                           }).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return page;
        }


        public dasMappingDTO getmappingedit(int id)
        {
            dasMappingDTO page = new dasMappingDTO();
            try
            {
                List<Dashboard_page_mapping> lorg = new List<Dashboard_page_mapping>();
                lorg = _context.Dashboard_page_mapping.AsNoTracking().Where(t => t.IVRM_DBID.Equals(id)).ToList();
                page.mappingeditdata = lorg.OrderByDescending(c => c.IVRM_CreatedDate).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public dasMappingDTO deletemappingrecord(int id)
        {

            List<Dashboard_page_mapping> mastersect = new List<Dashboard_page_mapping>(); // Mapper.Map<Organisation>(org);
            Dashboard_page_mapping maspge = new Dashboard_page_mapping();

            dasMappingDTO mas = new dasMappingDTO();

            try
            {
                var result = _context.Dashboard_page_mapping.Single(t => t.IVRM_DBID == id);

                _context.Remove(result);
                _context.SaveChanges();
                mas.returnval = true;

                //if (result.AMCEXM_ActiveFlg == true)
                //{
                //    result.AMCEXM_ActiveFlg = false;
                //    result.AMCEXM_UpdatedDate = DateTime.Now;
                //    result.AMCEXM_CreatedDate = result.AMCEXM_CreatedDate;
                //    _ClgAdmissionContext.Update(result);
                //    _ClgAdmissionContext.SaveChanges();
                //    mas.returnval = true;
                //}
                //else
                //{
                //    result.AMCEXM_UpdatedDate = DateTime.Now;
                //    result.AMCEXM_CreatedDate = result.AMCEXM_CreatedDate;
                //    result.AMCEXM_ActiveFlg = true;
                //    _ClgAdmissionContext.Update(result);
                //    _ClgAdmissionContext.SaveChanges();
                //    mas.returnval = false;
                //}


                //List<Dashboard_page_mapping> allmasterexam = new List<Dashboard_page_mapping>();
                //allmasterexam = _context.Dashboard_page_mapping.Where(d => d.MI_ID == mas.MI_Id).ToList();
                //mas.mappingdata = allmasterexam.OrderByDescending(a => a.IVRM_CreatedDate).ToArray();


                mas.mappingdata = (from a in _context.Dashboard_page_mapping
                                   from b in _context.Institute
                                   where a.MI_ID == b.MI_Id
                                   select new dasMappingDTO
                                   {
                                       IVRMP_Dasboard_PageName = a.IVRMP_Dasboard_PageName,
                                       IVRMRT_Role = a.IVRMRT_Role,
                                       returnMsg = b.MI_Name,
                                       IVRM_DBID = a.IVRM_DBID
                                   }).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                //master.returnval = ee.Message;
            }

            return mas;
        }

        //RoleWise Institution Mapping
        public IVRM_User_Login_InstitutionwiseDTO getuserdata(IVRM_User_Login_InstitutionwiseDTO id)
        {

            try
            {
                try
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "UserRoleData";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@IVRMRT_Id", SqlDbType.VarChar) { Value = id.IVRMRT_Id });
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
                            id.roleuserdata = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                //  return data;

                return id;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return id;
        }


        public IVRM_User_Login_InstitutionwiseDTO getinstitution(IVRM_User_Login_InstitutionwiseDTO id)
        {

            try
            {
                try
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "UserRoleInstitution";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.VarChar) { Value = id.Id });

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
                            id.institutiondata = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }


                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "UserRoleMappedInstitution";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.BigInt) { Value = id.Id });
                        cmd.Parameters.Add(new SqlParameter("@IVRMRT_Id", SqlDbType.BigInt) { Value = id.IVRMRT_Id });
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
                            id.institutionMappedData = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                //  return data;

                return id;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return id;
        }


        public IVRM_User_Login_InstitutionwiseDTO getcartdata(IVRM_User_Login_InstitutionwiseDTO id)
        {

            try
            {
                var inst_ids = "0";
                if (id.institutionarray.Length > 0)
                {
                    foreach (var ue in id.institutionarray)
                    {
                        inst_ids = inst_ids + "," + ue.MI_Id;

                    }

                }
                try
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "UserRoleInstitutionCart";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar) { Value = id.Id });
                        cmd.Parameters.Add(new SqlParameter("@Role_Id", SqlDbType.VarChar) { Value = id.IVRMRT_Id });
                        cmd.Parameters.Add(new SqlParameter("@MI_Ids", SqlDbType.VarChar) { Value = inst_ids });

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
                            id.cartdata = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                //  return data;

                return id;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return id;
        }


        public IVRM_User_Login_InstitutionwiseDTO savethirdDetail(IVRM_User_Login_InstitutionwiseDTO page)
        {
            bool returnresult = false;
            page.returnMsg = "";
            try
            {
                if (page.Selected_List.Count() > 0)

                {

                    foreach (IVRM_User_Login_InstitutionwiseDTO dto in page.Selected_List)
                    {
                        var duplicatecnt = _context.UserRoleWithInstituteDMO.Where(t => t.Id == page.Id && t.MI_Id == dto.MI_Id && t.Activeflag == 1).Count();
                        if (duplicatecnt > 0)
                        {
                            page.returnMsg = "duplicate";
                            return page;
                        }
                        else
                        {
                            
                            var resultCount = _context.UserRoleWithInstituteDMO.Where(t => t.Id == page.Id && t.MI_Id == dto.MI_Id && t.Activeflag == 0).ToList();

                            if (resultCount.Count > 0)
                            {

                                var result = _context.UserRoleWithInstituteDMO.Single(t => t.Id == page.Id && t.MI_Id == dto.MI_Id && t.Activeflag == 0);

                                result.MI_Id = dto.MI_Id;
                                result.Id = dto.Id;
                                result.Activeflag = 1;
                                result.CreatedDate = DateTime.Now;
                                result.UpdatedDate = DateTime.Now;
                                _context.Update(result);
                            }
                            else
                            {
                                UserRoleWithInstituteDMO maspge = new UserRoleWithInstituteDMO();
                                maspge.MI_Id = dto.MI_Id;
                                maspge.Id = dto.Id;
                                maspge.Activeflag = 1;
                                maspge.CreatedDate = DateTime.Now;
                                maspge.UpdatedDate = DateTime.Now;

                                _context.Add(maspge);
                            }


                            //}

                        }

                        var contactExists = _context.SaveChanges();

                        if (contactExists > 0)
                        {
                            returnresult = true;
                            page.returnval = returnresult;
                            page.returnMsg = "add";
                        }
                        else
                        {
                            returnresult = false;
                            page.returnval = returnresult;
                        }
                    }
                   
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "UserRoleMappedInstitution_data";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.BigInt) { Value = page.Id });
                    cmd.Parameters.Add(new SqlParameter("@IVRMRT_Id", SqlDbType.BigInt) { Value = page.IVRMRT_Id });
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
                        page.institutionMappedData = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "UserRoleInstitution";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.VarChar) { Value = page.Id });

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
                        page.institutiondata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                List<UserRoleWithInstituteDMO> allpages = new List<UserRoleWithInstituteDMO>();
                allpages = _context.UserRoleWithInstituteDMO.ToList();
                page.gridalldata = allpages.OrderByDescending(c => c.CreatedDate).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return page;
        }


        //public IVRM_User_Login_InstitutionwiseDTO loadthirddata(IVRM_User_Login_InstitutionwiseDTO id)
        //{
        //    IVRM_User_Login_InstitutionwiseDTO enq = new IVRM_User_Login_InstitutionwiseDTO();

        //    enq.userdata = _context.MasterRoleType.Where(u => u.flag == "U").ToArray();

        //    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
        //    {
        //        cmd.CommandText = "UserRoleInstitutionData";
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar) { Value = id.Id });
        //        cmd.Parameters.Add(new SqlParameter("@Role_Id", SqlDbType.VarChar) { Value = id.IVRMRT_Id });


        //        if (cmd.Connection.State != ConnectionState.Open)
        //            cmd.Connection.Open();

        //        var retObject = new List<dynamic>();
        //        try
        //        {
        //            using (var dataReader = cmd.ExecuteReader())
        //            {
        //                while (dataReader.Read())
        //                {
        //                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                    {
        //                        dataRow.Add(
        //                            dataReader.GetName(iFiled),
        //                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
        //                        );
        //                    }

        //                    retObject.Add((ExpandoObject)dataRow);
        //                }
        //            }
        //            id.gridalldata = retObject.ToArray();
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.Write(ex.Message);
        //        }
        //    }


        //    //List<UserRoleWithInstituteDMO> allinstitution = new List<UserRoleWithInstituteDMO>();
        //    //allinstitution = _context.UserRoleWithInstituteDMO.ToList();
        //    //enq.gridalldata = allpages.OrderByDescending(c => c.CreatedDate).ToArray();
        //    return enq;
        //}

        public IVRM_User_Login_InstitutionwiseDTO deletegriddata(IVRM_User_Login_InstitutionwiseDTO page)
        {



            // IVRM_User_Login_InstitutionwiseDTO mas = new IVRM_User_Login_InstitutionwiseDTO();

            try
            {
                var resultCount = _context.UserRoleWithInstituteDMO.Where(t => t.Id == page.Id && t.Activeflag==1).Count();

                if (resultCount >= 2)
                {
                    var result = _context.UserRoleWithInstituteDMO.Where(t => t.IVRMULI_Id == page.IVRMULI_Id).FirstOrDefault();
                    if (result.Activeflag == 1)
                    {
                        result.Activeflag = 0;
                    }
                    else
                    {
                        result.Activeflag = 1;
                    }
                    result.UpdatedDate = DateTime.Now;

                    _context.Update(result);
                    int i = _context.SaveChanges();
                    if (i > 0)
                    {
                        page.returnMsg = "Update";
                    }
                    else
                    {
                        page.returnMsg = "Failed";

                    }


                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "UserRoleMappedInstitution_data";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.BigInt) { Value = page.Id });
                        cmd.Parameters.Add(new SqlParameter("@IVRMRT_Id", SqlDbType.BigInt) { Value = page.IVRMRT_Id });
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
                            page.institutionMappedData = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }


                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "UserRoleInstitution";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.VarChar) { Value = page.Id });

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
                            page.institutiondata = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }

                }
                else
                {
                    page.returnMsg = "minimum";
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return page;
        }


        //preadmission master save
        public dasMappingDTO savepreadmissionDetail(dasMappingDTO page)
        {
            bool returnresult = false;
            page.returnMsg = "";
            try
            {
                //Preadmission_Dashboard_PageDMO maspge = Mapper.Map<Preadmission_Dashboard_PageDMO>(page);

                if (page.PAPG_ID > 0)
                {
                    var resultCount = _context.Preadmission_Dashboard_PageDMO.Where(t => t.PAPG_ID == page.PAPG_ID && t.PAPG_PAGENAME == page.PAPG_PAGENAME).Count();

                    if (resultCount == 0)
                    {
                        var result = _context.Preadmission_Dashboard_PageDMO.Single(t => t.PAPG_MIID == page.PAPG_ID);

                        result.PAPG_PAGENAME = page.PAPG_PAGENAME;
                        result.PAPG_MIID = page.PAPG_MIID;
                        result.PAPG_UpdatedDate = DateTime.Now;
                        result.PAPG_UpdatedBy = page.Userid;
                        _context.Update(result);
                        var contactExists = _context.SaveChanges();

                        if (contactExists == 1)
                        {
                            returnresult = true;
                            page.returnval = returnresult;
                            page.returnMsg = "update";
                        }
                        else
                        {
                            returnresult = false;
                            page.returnval = returnresult;
                        }
                    }
                    else
                    {
                        page.returnMsg = "duplicate";
                        return page;
                    }
                }
                else
                {
                    Preadmission_Dashboard_PageDMO maspge = new Preadmission_Dashboard_PageDMO();
                    var resultCount = _context.Preadmission_Dashboard_PageDMO.Where(t => t.PAPG_ID == page.PAPG_ID && t.PAPG_PAGENAME == page.PAPG_PAGENAME).Count();

                    if (resultCount > 0)
                    {
                        page.returnMsg = "duplicate";
                        return page;
                    }
                    //added by 02/02/2017
                    maspge.PAPG_PAGENAME = page.PAPG_PAGENAME;
                    maspge.PAPG_MIID = page.PAPG_MIID;
                    maspge.PAPG_CreatedDate = DateTime.Now;
                    maspge.PAPG_UpdatedDate = DateTime.Now;
                    maspge.PAPG_CreatedBy = page.Userid;
                    maspge.PAPG_UpdatedBy = page.Userid;


                    _context.Add(maspge);
                    var contactExists = _context.SaveChanges();

                    if (contactExists == 1)
                    {
                        returnresult = true;
                        page.returnval = returnresult;
                        page.returnMsg = "add";
                    }
                    else
                    {
                        returnresult = false;
                        page.returnval = returnresult;
                    }
                }
                page.preadmissionmapping = (from a in _context.Preadmission_Dashboard_PageDMO
                                            from b in _context.Institute
                                            where a.PAPG_MIID == b.MI_Id
                                            select new dasMappingDTO
                                            {
                                                PAPG_PAGENAME = a.PAPG_PAGENAME,
                                                PAPG_MIID = a.PAPG_MIID,
                                                MI_Name = b.MI_Name
                                            }).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return page;
        }
        public dasMappingDTO getpremappingedit(dasMappingDTO data)
        {

            try
            {

                data.preadmissionmapping = (from a in _context.Preadmission_Dashboard_PageDMO
                                            from b in _context.Institute
                                            where (a.PAPG_MIID == b.MI_Id && a.PAPG_ID == data.PAPG_ID)
                                            select new dasMappingDTO
                                            {
                                                PAPG_PAGENAME = a.PAPG_PAGENAME,
                                                PAPG_MIID = a.PAPG_MIID,
                                                MI_Name = b.MI_Name
                                            }).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }


        public dasMappingDTO deletepremappingrecord(dasMappingDTO data)
        {
            List<Preadmission_Dashboard_PageDMO> mastersect = new List<Preadmission_Dashboard_PageDMO>();
            Preadmission_Dashboard_PageDMO maspge = new Preadmission_Dashboard_PageDMO();

            dasMappingDTO mas = new dasMappingDTO();
            try
            {
                var result = _context.Preadmission_Dashboard_PageDMO.Single(t => t.PAPG_ID == data.PAPG_ID);

                _context.Remove(result);
                _context.SaveChanges();
                mas.returnval = true;

                mas.preadmissionmapping = (from a in _context.Preadmission_Dashboard_PageDMO
                                           from b in _context.Institute
                                           where (a.PAPG_MIID == b.MI_Id && a.PAPG_ID == data.PAPG_ID)
                                           select new dasMappingDTO
                                           {
                                               PAPG_PAGENAME = a.PAPG_PAGENAME,
                                               PAPG_MIID = a.PAPG_MIID,
                                               MI_Name = b.MI_Name
                                           }).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

    }
}
