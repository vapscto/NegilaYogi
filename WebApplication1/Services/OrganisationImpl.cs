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
using System.Collections.Concurrent;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using CommonLibrary;

namespace WebApplication1.Services
{
    public class OrganisationImpl : Interfaces.Organisationinterface
    {
        private static ConcurrentDictionary<string, OrganisationDTO> _login =
            new ConcurrentDictionary<string, OrganisationDTO>();

        public OrganisationContext _OrganisationContext;
        public OrganisationImpl(OrganisationContext OrganisationContext)
        {
            _OrganisationContext = OrganisationContext;
        }

        public OrganisationDTO saveorgdet(OrganisationDTO org)
        {
            try
            {
                Organisation enq = Mapper.Map<Organisation>(org);
                //OrganisationPhone phone = Mapper.Map<OrganisationPhone>(org);
                //OrganisationEmail email = Mapper.Map<OrganisationEmail>(org);
                //OrganisationMobile mobile = Mapper.Map<OrganisationMobile>(org);

                if (enq.MO_Id > 0)
                {

                    var result1= _OrganisationContext.Organisation.Where(t => t.MO_Name == enq.MO_Name && t.MO_Id!=org.MO_Id).Count();
                    var result2  = _OrganisationContext.Organisation.Where(t => t.MT_Domain_name == enq.MT_Domain_name );
                    if (result1 >= 1)
                    {
                        org.returnval = "Trust Name Already Exists";
                        org.returnduplicatestatus = "Duplicate";
                    }
                    else if
                        (result1 >= 1)
                    {
                        org.returnval = "Domain Name Already Exists";
                        org.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _OrganisationContext.Organisation.Single(t => t.MO_Id == enq.MO_Id);
                        // var result = _OrganisationContext.Organisation.AsNoTracking().Single(t => t.MO_Id == enq.MO_Id);

                        result.IVRMMCT_Name = enq.IVRMMCT_Name; // commented on 10-11-2016
                        result.IVRMMS_Id = enq.IVRMMS_Id;
                        result.IVRMMC_Id = enq.IVRMMC_Id;
                        result.MO_Name = enq.MO_Name;
                        result.MO_Address1 = enq.MO_Address1;
                        result.MO_Address2 = enq.MO_Address2;
                        result.MO_Address3 = enq.MO_Address3;
                        result.MO_Landmark = enq.MO_Landmark;
                        result.MO_Pincode = enq.MO_Pincode;
                        result.MO_FaxNo = enq.MO_FaxNo;
                        result.MO_Website = enq.MO_Website;
                        result.MO_OrganisationType = enq.MO_OrganisationType;
                        result.MT_Currency = enq.MT_Currency;
                        result.MT_Domain_name = enq.MT_Domain_name;


                        //added by 02/02/2017

                        result.UpdatedDate = DateTime.Now;
                        _OrganisationContext.Update(result);
                        var contactExists = _OrganisationContext.SaveChanges();

                        if (contactExists == 1)
                        {

                            org.returnval = "Trust Updated Successfully";
                        }
                        else
                        {
                            org.returnval = "Trust Not Updated";
                        }
                    }
                }
                else
                {
                    var result = _OrganisationContext.Organisation.Where(t => t.MO_Name == enq.MO_Name);
                    var result2 = _OrganisationContext.Organisation.Where(t => t.MT_Domain_name == enq.MT_Domain_name);
                    if (result.Count()>=1)
                    {
                        org.returnval = "Trust Name Already Exists";
                        org.returnduplicatestatus = "Duplicate";
                    }
                    else if
                        (result2.Count() >= 1)
                    {
                        org.returnval = "Domain Name Alreday Exists";
                        org.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {

                        //added by 02/02/2017
                        enq.CreatedDate = DateTime.Now;
                        enq.UpdatedDate = DateTime.Now;
                        _OrganisationContext.Add(enq);
                        var contactExists = _OrganisationContext.SaveChanges();

                        if (contactExists == 1)
                        {
                            org.returnval = "Trust Saved Successfully";
                        }
                        else
                        {
                            org.returnval = "Trust Not Saved";
                        }
                    }
                }

                //add/uppdate phone details
                if (org.phones.Count() > 0 && org.returnduplicatestatus!="Duplicate")
                {

                    //rakesh

                    List<long> temparr = new List<long>();
                    List<long> temparr1 = new List<long>();
                    foreach (Organisation_Phone_NoDTO ph in org.phones)
                    {
                        temparr.Add(ph.MOPN_Id);
                    }

                    Array Phone_Noresultremove = _OrganisationContext.organisationPhone.Where(t => !temparr.Contains(t.MOPN_Id) && t.MO_Id == org.MO_Id).ToArray();
                    foreach (OrganisationPhone ph1 in Phone_Noresultremove)
                    {

                        _OrganisationContext.Remove(ph1);

                    }

                    //rakesh

                    foreach (Organisation_Phone_NoDTO ph in org.phones)
                    {
                        ph.MO_Id = enq.MO_Id;
                        OrganisationPhone phone = Mapper.Map<OrganisationPhone>(ph);
                        if (phone.MOPN_Id > 0)
                        {
                            var Phone_Noresult = _OrganisationContext.organisationPhone.Single(t => t.MOPN_Id == ph.MOPN_Id);
                            Phone_Noresult.MOPN_Id = ph.MOPN_Id;
                            Phone_Noresult.MOPN_PhoneNo = ph.MOPN_PhoneNo;
                            Phone_Noresult.MOP_Flag = ph.MOP_Flag;
                            Phone_Noresult.MO_Id = ph.MO_Id;

                            //added by 02/02/2017
                       
                            Phone_Noresult.UpdatedDate = DateTime.Now;
                            _OrganisationContext.Update(Phone_Noresult);
                        }
                        else
                        {

                            //added by 02/02/2017
                            phone.CreatedDate = DateTime.Now;
                            phone.UpdatedDate = DateTime.Now;
                            _OrganisationContext.Add(phone);
                        }

                        _OrganisationContext.SaveChanges();
                    }
                }

                //add/update Mobile details
                if (org.mobiles.Count() > 0 && org.returnduplicatestatus != "Duplicate")
                {

                    //rakesh

                    List<long> temparr = new List<long>();
                    List<long> temparr1 = new List<long>();
                    foreach (Organisation_MobileDTO ph in org.mobiles)
                    {
                        temparr.Add(ph.MOMN_Id);
                    }

                    Array Phone_Noresultremove = _OrganisationContext.organisationMobile.Where(t => !temparr.Contains(t.MOMN_Id) && t.MO_Id == org.MO_Id).ToArray();
                    foreach (OrganisationMobile ph1 in Phone_Noresultremove)
                    {

                        _OrganisationContext.Remove(ph1);

                    }

                    //rakesh

                    foreach (Organisation_MobileDTO ph in org.mobiles)
                    {
                        ph.MO_Id = enq.MO_Id;
                        OrganisationMobile mobile = Mapper.Map<OrganisationMobile>(ph);

                        if (mobile.MOMN_Id > 0)
                        {
                            var MobileNoresult = _OrganisationContext.organisationMobile.Single(t => t.MOMN_Id == ph.MOMN_Id);
                            MobileNoresult.MOMN_Id = ph.MOMN_Id;
                            MobileNoresult.MOMN_MobileNo = ph.MOMN_MobileNo;
                            MobileNoresult.MOM_Flag = ph.MOM_Flag;
                            MobileNoresult.MO_Id = ph.MO_Id;
                            //added by 02/02/2017
                         
                            MobileNoresult.UpdatedDate = DateTime.Now;
                            _OrganisationContext.Update(MobileNoresult);
                        }
                        else
                        {   //added by 02/02/2017
                            mobile.CreatedDate = DateTime.Now;
                            mobile.UpdatedDate = DateTime.Now;
                            _OrganisationContext.Add(mobile);
                        }
                        _OrganisationContext.SaveChanges();

                    }
                }

                //add/update Email details
                if (org.emails.Count() > 0 && org.returnduplicatestatus != "Duplicate")
                {
                    //rakesh
                    List<long> temparr = new List<long>();
                    List<long> temparr1 = new List<long>();
                    foreach (Organisation_EmailIdDTO ph in org.emails)
                    {
                        temparr.Add(ph.MOE_Id);


                    }

                    Array Phone_Noresultremove = _OrganisationContext.organisationEmail.Where(t => !temparr.Contains(t.MOE_Id) && t.MO_Id == org.MO_Id).ToArray();
                    foreach (OrganisationEmail ph1 in Phone_Noresultremove)
                    {

                        _OrganisationContext.Remove(ph1);

                    }
                    //rakesh

                    foreach (Organisation_EmailIdDTO ph in org.emails)
                    {
                        ph.MO_Id = enq.MO_Id;
                        OrganisationEmail email = Mapper.Map<OrganisationEmail>(ph);

                        if (email.MOE_Id > 0)
                        {
                            var emailresult = _OrganisationContext.organisationEmail.Single(t => t.MOE_Id == ph.MOE_Id);
                            emailresult.MOE_Id = ph.MOE_Id;
                            emailresult.MOE_EmailId = ph.MOE_EmailId;
                            emailresult.MOE_Flag = ph.MOE_Flag;
                            emailresult.MO_Id = ph.MO_Id;
                            //added by 02/02/2017
                  
                            emailresult.UpdatedDate = DateTime.Now;
                            _OrganisationContext.Update(emailresult);
                        }
                        else
                        {
                            //added by 02/02/2017
                            email.CreatedDate = DateTime.Now;
                            email.UpdatedDate = DateTime.Now;
                            _OrganisationContext.Add(email);
                        }

                        _OrganisationContext.SaveChanges();

                    }
                }

                List<Organisation> allorganisation = new List<Organisation>();
                allorganisation = _OrganisationContext.Organisation.ToList();
                org.organisationname = allorganisation.ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                org.returnval = "Data Not Saved/Updated.Kindly Contact Administrator";
            }

            return org;
        }

        //public EnqDTO countrydrp(EnqDTO stu)
         public async Task<OrganisationDTO> countrydrp(OrganisationDTO stu)
        {
            try
            {
                List<Country> allCountry = new List<Country>();
                allCountry = await _OrganisationContext.country.ToListAsync();
                stu.countryDrpDown = allCountry.ToArray();

                //List<City> allcity = new List<City>();
                //allcity = await _OrganisationContext.city.ToListAsync();
                //stu.cityDrpDown = allcity.ToArray();

                List<State> allstate = new List<State>();
                allstate =await _OrganisationContext.State.ToListAsync();
                stu.stateDrpDown = allstate.ToArray();

                List<Organisation> allorganisation = new List<Organisation>();

                var rolelist = _OrganisationContext.MasterRoleType.Where(t => t.IVRMRT_Id == stu.RoleId).ToList();

                if (rolelist[0].IVRMRT_Role == "Super Admin")
                {
                    allorganisation = await _OrganisationContext.Organisation.OrderBy(t => t.CreatedDate).ToListAsync();
                    stu.organisationname = allorganisation.ToArray();
                }
                else if (rolelist[0].IVRMRT_Role.Equals("Multi Admin"))
                {
                    allorganisation = await _OrganisationContext.Organisation.Where(t => t.MO_Id == stu.MO_Id).OrderBy(t => t.CreatedDate).ToListAsync();
                    stu.organisationname = allorganisation.ToArray();

                }
                else if (rolelist[0].IVRMRT_Role.Equals("Admin"))
                {
                    allorganisation = await _OrganisationContext.Organisation.Where(t => t.MO_Id == stu.MO_Id).OrderBy(t => t.CreatedDate).ToListAsync();
                    stu.organisationname = allorganisation.ToArray();

                }

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return stu;
        }

       // public async Task<StateDTO> enqdrpcountrydata(int id)
        public StateDTO enqdrpcountrydata(int id)
        {
            StateDTO enq = new StateDTO();
            try
            {
                Array[] drpall = new Array[3];
                List<State> allstate = new List<State>();
                allstate = _OrganisationContext.State.ToList();
                allstate = _OrganisationContext.State.Where(t => t.IVRMMC_Id.Equals(id)).ToList();

                //Array[] drpcurre = new Array[3];
                List<Country> getcurr = new List<Country>();
                string curr = "";
                getcurr = _OrganisationContext.country.Where(t => t.IVRMMC_Id.Equals(id)).ToList();

                foreach (var value in getcurr)
                {
                    curr = value.IVRMMC_Currency;
                }

                enq.defaultcurrency = curr;


                //var statelist = _OrganisationContext.city
                //            .Join(_OrganisationContext.State,
                //                  c => c.IVRMMS_Id,
                //                  o => o.IVRMMS_Id,
                //                  (c, o) => new
                //                  {
                //                      IVRMMS_Id = c.IVRMMS_Id,
                //                      IVRMMS_Name = o.IVRMMS_Name,
                //                      IVRMMCT_Id = c.IVRMMCT_Id
                //                  }).Where(c => c.IVRMMCT_Id.Equals(id)).Distinct();

                //enq.stateDrpDown = statelist.ToArray();

                enq.stateDrpDown = allstate.ToArray();

                //List<Country> country = new List<Country>();
                //country = _OrganisationContext.country.AsNoTracking().Where(t => t.IVRMMC_Id.Equals(id)).ToList();
                //enq.ccodelength = country.ToArray();

                var result = _OrganisationContext.country.Single(t => t.IVRMMC_Id == id);
                // var result = _OrganisationContext.Organisation.AsNoTracking().Single(t => t.MO_Id == enq.MO_Id);

                enq.IVRMMC_CountryPhCode = result.IVRMMC_CountryPhCode;
                enq.ccodelength = result.IVRMMC_MobileNoLength;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return enq;
        }

        public CountryDTO getcity(int id)
        {
            CountryDTO enq = new CountryDTO();
            try
            {
                Array[] drpall = new Array[3];
                List<Country> getcurr = new List<Country>();
                string curr = "";
                getcurr = _OrganisationContext.country.Where(t => t.IVRMMC_Id.Equals(id)).ToList();

                foreach (var value in getcurr)
                {
                    curr = value.IVRMMC_Currency;
                }

                enq.defaultcurrency = curr;
               
                // enq.defaultcurrency = getcurr.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return enq;
        }

        public OrganisationDTO deleterec(int id)
        {
            bool returnresult = false;
            OrganisationDTO org = new OrganisationDTO();
            List<Organisation> lorg = new List<Organisation>(); // Mapper.Map<Organisation>(org);

            List<OrganisationPhone> Phone_No = new List<OrganisationPhone>();
            List<OrganisationMobile> MobileNo = new List<OrganisationMobile>();
            List<OrganisationEmail> EmailId = new List<OrganisationEmail>();

            try
            {

                //Phone_No = _OrganisationContext.organisationPhone.Where(t => t.MO_Id.Equals(id)).ToList();
                //MobileNo = _OrganisationContext.organisationMobile.Where(t => t.MO_Id.Equals(id)).ToList();
                //EmailId = _OrganisationContext.organisationEmail.Where(t => t.MO_Id.Equals(id)).ToList();

                //if (Phone_No.Count() > 0)
                //{
                //    for (int i = 0; i < Phone_No.Count(); i++)
                //    {
                //        _OrganisationContext.Remove(Phone_No.ElementAt(i));

                //        _OrganisationContext.SaveChanges();
                //    }

                //}

                //if (MobileNo.Count() > 0)
                //{
                //    for (int i = 0; i < MobileNo.Count(); i++)
                //    {
                //        _OrganisationContext.Remove(MobileNo.ElementAt(i));

                //        _OrganisationContext.SaveChanges();
                //    }
                //}

                //if (EmailId.Count() > 0)
                //{
                //    for (int i = 0; i < EmailId.Count(); i++)
                //    {
                //        _OrganisationContext.Remove(EmailId.ElementAt(i));

                //        _OrganisationContext.SaveChanges();
                //    }
                //}

                //lorg = _OrganisationContext.Organisation.Where(t => t.MO_Id.Equals(id)).ToList();
                var result = _OrganisationContext.Organisation.Single(t => t.MO_Id == id);

                if(result.MO_ActiveFlag == 1)
                {
                    result.MO_ActiveFlag = 0;
                }
                else if (result.MO_ActiveFlag == 0)
                {
                    result.MO_ActiveFlag = 1;
                }


                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                result.UpdatedDate = indianTime;
                _OrganisationContext.Update(result);
                   // _OrganisationContext.Remove(lorg.ElementAt(0));
                    var contactExists = _OrganisationContext.SaveChanges();

                    if (contactExists == 1)
                    {
                        if (result.MO_ActiveFlag == 0)
                        {
                            org.returnval = "Trust Deactivated Successfully";
                        }
                        else if (result.MO_ActiveFlag == 1)
                        {
                            org.returnval = "Trust Activated Successfully";
                        }
                    }
                    else
                    {
                        org.returnval = "Trust Not Activated/Deactivated ";
                    }
               
                List<Organisation> allorganisation = new List<Organisation>();
                allorganisation = _OrganisationContext.Organisation.ToList();
                org.organisationname = allorganisation.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                org.returnval = "Trust Not Activated/Deactivated.Kindly Contact Administrator";
            }

            return org;
        }

        public OrganisationDTO getdetails(int id)
        {
            OrganisationDTO org = new OrganisationDTO();
            try
            {
                List<Organisation> lorg = new List<Organisation>();
                lorg = _OrganisationContext.Organisation.AsNoTracking().Where(t => t.MO_Id.Equals(id)).ToList();
                org.organisationname = lorg.ToArray();

                List<OrganisationMobile> mob = new List<OrganisationMobile>();
                mob = _OrganisationContext.organisationMobile.AsNoTracking().Where(t => t.MO_Id.Equals(id)).ToList();
                org.MobilearrayList = mob.ToArray();

                List<OrganisationPhone> phn = new List<OrganisationPhone>();
                phn = _OrganisationContext.organisationPhone.AsNoTracking().Where(t => t.MO_Id.Equals(id)).ToList();
                org.PhonearrayList = phn.ToArray();

                List<OrganisationEmail> email = new List<OrganisationEmail>();
                email = _OrganisationContext.organisationEmail.AsNoTracking().Where(t => t.MO_Id.Equals(id)).ToList();
                org.EmailarrayList = email.ToArray();

             
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return org;
        }

        public OrganisationDTO getcurrency(int id)
        {
            OrganisationDTO enq = new OrganisationDTO();
            try
            {
                Array[] drpall = new Array[3];
                List<Country> getcurr = new List<Country>();
                string curr = "";
                getcurr = _OrganisationContext.country.Where(t => t.IVRMMC_Id.Equals(id)).ToList();

                foreach (var value in getcurr)
                {
                    curr = value.IVRMMC_Currency;
                }

                enq.defaultcurrency = curr;
               // enq.defaultcurrency = getcurr.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return enq;
        }

        public OrganisationDTO getfilterdet(int filtype, OrganisationDTO data)
        {
            string filetype = "All";
            OrganisationDTO org = new OrganisationDTO();
            try
            {
                List<Organisation> lorg = new List<Organisation>();
                if(data.MO_Address2 == "Trust Type")
                {
                    lorg = _OrganisationContext.Organisation.Where(t => t.MO_OrganisationType.Contains(data.MO_Address1)).ToList();
                    //lorg = _OrganisationContext.Organisation.OrderBy(x => x.MO_OrganisationType).ToList();
                }
                if (data.MO_Address2 == "Trust Name")
                {
                    lorg = _OrganisationContext.Organisation.Where(t => t.MO_Name.Contains(data.MO_Address1)).ToList();
                    //lorg = _OrganisationContext.Organisation.OrderBy(x => x.MO_Name).ToList();
                }
                if (data.MO_Address2 == "All")
                {
                    lorg = _OrganisationContext.Organisation.ToList();
                    //lorg = _OrganisationContext.Organisation.OrderBy(x => x.MO_Name).ToList();
                }

                org.organisationname = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return org;
        }

        public async Task<OrganisationDTO> getorgSearchedDetails(SortingPagingInfoDTO data)
        {
            OrganisationDTO trustDTO = new OrganisationDTO();
            try
            {
                await(trustPaginationdetails(data, trustDTO));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return trustDTO;
        }

        public async Task<OrganisationDTO> trustPaginationdetails(SortingPagingInfoDTO spidto, OrganisationDTO stu)
        {
            try
            {
                int avtiveflag = 0;
                var allorganisation = from a in _OrganisationContext.Organisation select a ;

                if (allorganisation.Count() > 0)
                {
                    if (!string.IsNullOrEmpty(spidto.searchString))
                    {
                       spidto.CurrentPageIndex = 1;

                        if(spidto.searchString == "Enable")
                        {
                            avtiveflag = 1;
                        }
                        else if (spidto.searchString == "Disable")
                        {
                            avtiveflag = 0;
                        }

                        switch (spidto.searchType)
                        {
                            case "mO_Name":
                                allorganisation = allorganisation.Where(d => d.MO_Name.Contains(spidto.searchString)).OrderByDescending(d => d.CreatedDate);
                                break;
                            case "mO_OrganisationType":
                                allorganisation = allorganisation.Where(d => d.MO_OrganisationType.Contains(spidto.searchString)).OrderByDescending(d => d.CreatedDate);
                                break;
                            case "ActiveInactive":
                                allorganisation = allorganisation.Where(d => d.MO_ActiveFlag == avtiveflag).OrderByDescending(d => d.CreatedDate);
                                break;
                            default:
                                allorganisation = allorganisation.Where(s => s.MO_Name.Contains(spidto.searchString) || s.MO_OrganisationType.Contains(spidto.searchString)).OrderByDescending(d => d.CreatedDate);
                                break;
                        }

                        //switch (spidto.searchType)
                        //{
                        //    case "mO_Name":
                        //        allorganisation = allorganisation.Where(d => d.MO_Name.Contains(spidto.searchString));
                        //        break;
                        //    case "mO_OrganisationType":
                        //        allorganisation = allorganisation.Where(d => d.MO_OrganisationType.Contains(spidto.searchString));
                        //        break;
                        //    case "ActiveInactive":
                        //        allorganisation = allorganisation.Where(d => d.MO_ActiveFlag == avtiveflag);
                        //        break;
                        //    default:
                        //        allorganisation = allorganisation.Where(s => s.MO_Name.Contains(spidto.searchString) || s.MO_OrganisationType.Contains(spidto.searchString));
                        //        break;
                        //}

                        //if(allorganisation.Count()> spidto.PageSize)
                        //{
                        //    spidto.CurrentPageIndex = 1;
                        //}
                    }
                    switch (spidto.sortOrder)
                    {
                        case "mO_Name":
                            allorganisation = allorganisation.OrderByDescending(s => s.MO_Name);
                            break;
                        case "mO_OrganisationType":
                            allorganisation = allorganisation.OrderBy(s => s.MO_OrganisationType);
                            break;
                        default:
                            allorganisation = allorganisation.OrderByDescending(s => s.CreatedDate);
                            break;
                    }

                    //switch (spidto.sortOrder)
                    //{
                    //    case "mO_Name":
                    //        allorganisation = allorganisation.OrderByDescending(s => s.MO_Name);
                    //        break;
                    //    case "mO_OrganisationType":
                    //        allorganisation = allorganisation.OrderBy(s => s.MO_OrganisationType);
                    //        break;
                    //    default:
                    //        allorganisation = allorganisation.OrderBy(s => s.MO_Name);
                    //        break;
                    //}

                    PaginatedList<Organisation> List = await PaginatedList<Organisation>.CreateAsync(allorganisation.AsNoTracking(), spidto.CurrentPageIndex, spidto.PageSize);
                    stu.organisationname = List.ToArray();

                    stu.trustPagination = spidto;
                    // stu.instutePagination.PageSize = pageSize;
                    stu.trustPagination.PageCount = List.TotalPages;
                    stu.trustPagination.CurrentPageIndex = List.PageIndex;
                    stu.trustPagination.TotalItems = List.TotalRecords;
                }
            }
            catch (Exception e)
            {
                string ex = e.Message;
            }
            return stu;
        }

    }
}
