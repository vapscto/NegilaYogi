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
using DomainModel.Model.com.vapstech.MobileApp;

namespace WebApplication1.Services
{
    public class MasterPageImpl : Interfaces.MasterPageInterface
    {
        private static ConcurrentDictionary<string, MasterPageDTO> _login =
           new ConcurrentDictionary<string, MasterPageDTO>();

        public MasterPageContext _MasterPageContext;
        public MasterPageImpl(MasterPageContext MasterPageContext)
        {
            _MasterPageContext = MasterPageContext;
        }
        public MasterPageDTO saveorgdet(MasterPageDTO page)
        {
            try
            {
                MasterPage maspge = Mapper.Map<MasterPage>(page);

                if(page.IVRMP_Id>0)
                {
                    var duplicateresult = _MasterPageContext.masterpage.Where(t => t.IVRMMP_PageName == page.IVRMMP_PageName && t.IVRMP_Id!= page.IVRMP_Id);

                    if (duplicateresult.Count() > 0)
                    {
                        page.returnval = "Duplicate Records";
                    }
                    else
                    {

                        var result = _MasterPageContext.masterpage.Single(t => t.IVRMP_Id == maspge.IVRMP_Id);
                        // var result = _OrganisationContext.Organisation.AsNoTracking().Single(t => t.MO_Id == enq.MO_Id);

                        result.IVRMMP_PageName = maspge.IVRMMP_PageName;
                        result.IVRMP_PageDesc = maspge.IVRMP_PageDesc;
                        result.IVRMP_PageURL = maspge.IVRMP_PageDesc;
                        result.userid = maspge.userid;
                        result.IVRMP_MandatoryFlag = maspge.IVRMP_MandatoryFlag;
                        result.IVRMP_PageDisplayName = maspge.IVRMP_PageDisplayName;
                        //added by 02/02/2017

                        result.UpdatedDate = DateTime.Now;
                        _MasterPageContext.Update(result);
                        var contactExists = _MasterPageContext.SaveChanges();

                        if (contactExists == 1)
                        {

                            page.returnval = "Records Updated Successfully";
                        }
                        else
                        {

                            page.returnval = "Records Not Updated";
                        }
                    }
                }
                else
                {
                    var duplicateresult = _MasterPageContext.masterpage.Where(t => t.IVRMMP_PageName == page.IVRMMP_PageName);

                    if(duplicateresult.Count()>0)
                    {
                        page.returnval = "Duplicate Records";
                    }
                    else
                    {
                        //added by 02/02/2017
                        maspge.CreatedDate = DateTime.Now;
                        maspge.UpdatedDate = DateTime.Now;
                        _MasterPageContext.Add(maspge);
                        var contactExists = _MasterPageContext.SaveChanges();

                        if (contactExists == 1)
                        {

                            page.returnval = "Record Saved Successfully";
                        }
                        else
                        {

                            page.returnval = "Record Not Saved";
                        }
                    }
                }

                List<MasterPage> allpages = new List<MasterPage>();
                allpages = _MasterPageContext.masterpage.OrderByDescending(t=>t.CreatedDate).ToList();
                page.pagesdata = allpages.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return page;
        }

        public MasterPageDTO mobilesaveorgdet(MasterPageDTO page)
        {
            try            {                 //IVRM_MobileApp_Page maspge = Mapper.Map<IVRM_MobileApp_Page>(page);                if (page.IVRMMAP_Id > 0)                {                    var duplicateresult = _MasterPageContext.IVRM_MobileApp_Page.Where(t => t.IVRMMAP_AppPageName == page.IVRMMAP_AppPageName && t.IVRMMAP_Id != page.IVRMMAP_Id);                    if (duplicateresult.Count() > 0)                    {                        page.returnval = "Duplicate Records";                    }                    else                    {                        var result = _MasterPageContext.IVRM_MobileApp_Page.Single(t => t.IVRMMAP_Id == page.IVRMMAP_Id);                        // var result = _OrganisationContext.Organisation.AsNoTracking().Single(t => t.MO_Id == enq.MO_Id);                        result.IVRMMAP_AppPageName = page.IVRMMAP_AppPageName;                        result.IVRMMAP_AppPageDesc = page.IVRMMAP_AppPageDesc;                        result.IVRMMAP_Displayname = page.IVRMMAP_Displayname;                        result.IVRMMAP_AppPageURL = page.IVRMMAP_AppPageURL;                        result.IVRMMAP_ActiveFlg = true;                                               //added by 02/02/2017                        result.UpdatedDate = DateTime.Now;                        _MasterPageContext.Update(result);                        var contactExists = _MasterPageContext.SaveChanges();                        if (contactExists == 1)                        {                            page.returnval = "Records Updated Successfully";                        }                        else                        {                            page.returnval = "Records Not Updated";                        }                    }                }                else                {                    var duplicateresult = _MasterPageContext.IVRM_MobileApp_Page.Where(t => t.IVRMMAP_AppPageName == page.IVRMMAP_AppPageName);                    if (duplicateresult.Count() > 0)                    {                        page.returnval = "Duplicate Records";                    }                    else                    {                        IVRM_MobileApp_Page maspge = new IVRM_MobileApp_Page();                        //added by 02/02/2017                        maspge.IVRMMAP_AppPageName = page.IVRMMAP_AppPageName;                        maspge.IVRMMAP_Displayname = page.IVRMMAP_Displayname;                        maspge.IVRMMAP_AppPageDesc = page.IVRMMAP_AppPageDesc;                        maspge.IVRMMAP_AppPageURL = page.IVRMMAP_AppPageURL;                        maspge.IVRMMAP_Id = page.IVRMMAP_Id;                        maspge.CreatedDate = DateTime.Now;                        maspge.UpdatedDate = DateTime.Now;                        maspge.IVRMMAP_ActiveFlg = true;                        _MasterPageContext.Add(maspge);                        var contactExists = _MasterPageContext.SaveChanges();                        if (contactExists == 1)                        {                            page.returnval = "Record Saved Successfully";                        }                        else                        {                            page.returnval = "Record Not Saved";                        }                    }                }                List<IVRM_MobileApp_Page> allpages = new List<IVRM_MobileApp_Page>();                allpages = _MasterPageContext.IVRM_MobileApp_Page.OrderByDescending(t => t.CreatedDate).ToList();                page.pagesdata = allpages.ToArray();            }            catch (Exception ee)            {                Console.WriteLine(ee.Message);            }

            return page;
        }

        public MasterPageDTO deleterec(int id)
        {
            MasterPageDTO page = new MasterPageDTO();
            List<MasterPage> lorg = new List<MasterPage>();
            lorg = _MasterPageContext.masterpage.Where(t => t.IVRMP_Id.Equals(id)).ToList();

            try
            {
                if (lorg.Any())
                {
                    _MasterPageContext.Remove(lorg.ElementAt(0));

                    var contactExists = _MasterPageContext.SaveChanges();
                    if (contactExists == 1)
                    {
                       
                        page.returnval = "Record Deleted Successfully";
                    }
                    else
                    {
                       
                        page.returnval = "Record Not Deleted";
                    }
                }

                List<MasterPage> allpages = new List<MasterPage>();
                allpages = _MasterPageContext.masterpage.OrderByDescending(t => t.CreatedDate).ToList();
                page.pagesdata = allpages.ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return page;
        }

        public MasterPageDTO mobiledeleterec(MasterPageDTO id)
        {
            MasterPageDTO page = new MasterPageDTO();
            List<IVRM_MobileApp_Page> lorg = new List<IVRM_MobileApp_Page>();
            lorg = _MasterPageContext.IVRM_MobileApp_Page.Where(t => t.IVRMMAP_Id.Equals(id.IVRMMAP_Id)).ToList();
            var lorg1 = _MasterPageContext.IVRM_User_MobileApp_Login_Privileges.Where(t => t.IVRMMAP_Id.Equals(id.IVRMMAP_Id)).ToList();
            var lorg2 = _MasterPageContext.IVRM_Role_MobileApp_Privileges.Where(t => t.IVRMMAP_Id.Equals(id.IVRMMAP_Id)).ToList();
            if (lorg1.Count > 0)
            {
                page.returnval = "User MobileApp mapped";
            }
            else if (lorg2.Count > 0)
            {
                page.returnval = "Role MobileApp mapped";
            }
            else
            {
                try
                {
                    if (lorg.Any())
                    {
                        _MasterPageContext.Remove(lorg.ElementAt(0));

                        var contactExists = _MasterPageContext.SaveChanges();
                        if (contactExists == 1)
                        {

                            page.returnval = "Record Deleted Successfully";
                        }
                        else
                        {

                            page.returnval = "Record Not Deleted";
                        }
                    }

                }

                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }
            }
            List<IVRM_MobileApp_Page> allpages = new List<IVRM_MobileApp_Page>();
            allpages = _MasterPageContext.IVRM_MobileApp_Page.OrderByDescending(t => t.CreatedDate).ToList();
            page.pagesdata = allpages.ToArray();

            return page;
        }

        public MasterPageDTO getdetails(int id)
        {
            MasterPageDTO org = new MasterPageDTO();
            try
            {
                List<MasterPage> lorg = new List<MasterPage>();
                lorg = _MasterPageContext.masterpage.OrderBy(t=>t.CreatedDate).ToList();
                org.pagesdata = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return org;
        }

        public MasterPageDTO getalldetailsmobile (int id)
        {
            MasterPageDTO org = new MasterPageDTO();
            try
            {
                List<IVRM_MobileApp_Page> lorg = new List<IVRM_MobileApp_Page>();
                lorg = _MasterPageContext.IVRM_MobileApp_Page.OrderBy(t => t.CreatedDate).ToList();
                org.pagesdata = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return org;
        }

        public MasterPageDTO getpageedit(int id)
        {
            MasterPageDTO page = new MasterPageDTO();
            try
            {
                List<MasterPage> lorg = new List<MasterPage>();
                lorg = _MasterPageContext.masterpage.AsNoTracking().Where(t => t.IVRMP_Id.Equals(id)).ToList();
                page.pagesdata = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public MasterPageDTO mobilegetdetails (int id)
        {
            MasterPageDTO page = new MasterPageDTO();
            try
            {
                List<IVRM_MobileApp_Page> lorg = new List<IVRM_MobileApp_Page>();
                lorg = _MasterPageContext.IVRM_MobileApp_Page.AsNoTracking().Where(t => t.IVRMMAP_Id.Equals(id)).ToList();
                page.pagesdata = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public MasterPageDTO getsearchdata(int id, MasterPageDTO org)
        {
            //string filetype = "All";
            MasterPageDTO pagedata = new MasterPageDTO();
            try
            {
                List<MasterPage> lorg = new List<MasterPage>();
                if (org.IVRMP_PageDesc == "Name")
                {
                    lorg = _MasterPageContext.masterpage.Where(t => t.IVRMMP_PageName.Contains(org.IVRMMP_PageName)).ToList();

                }
                if (org.IVRMP_PageDesc == "All")
                {
                    lorg = _MasterPageContext.masterpage.ToList();
                }
                org.pagesdata = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return org;
        }
    }
}
