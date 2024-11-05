using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.Portals.Principal;
using AutoMapper;
using DomainModel.Model.com.vapstech.Portals.Employee;

namespace PortalHub.com.vaps.Principal.Services
{
    public class NoticeImpl : Interfaces.NoticeInterface
    {
        public PortalContext _PrincipalDashboardContext;
        
        //public DomainModelMsSqlServerContext _db;
        public NoticeImpl(PortalContext cpContext)
        {
            _PrincipalDashboardContext = cpContext;
        }
        public Notice_DTO savedetail(Notice_DTO data)
        {
            try
            {
                if (data.images_list.Length > 0)
                {
                    foreach (var act3 in data.images_list)
                    {
                        IVRM_NoticeBoardDMO objpge = Mapper.Map<IVRM_NoticeBoardDMO>(data);
                        objpge.INTB_Attachment = act3.imagePath;
                        objpge.INTB_ActiveFlag = true;
                        objpge.UpdatedDate = DateTime.Now;
                        objpge.CreatedDate = DateTime.Now;
                        _PrincipalDashboardContext.Add(objpge);
                    }
                }
                else
                {
                    IVRM_NoticeBoardDMO objpge = Mapper.Map<IVRM_NoticeBoardDMO>(data);
                    objpge.INTB_ActiveFlag = true;
                    objpge.UpdatedDate = DateTime.Now;
                    objpge.CreatedDate = DateTime.Now;
                    _PrincipalDashboardContext.Add(objpge);
                }
               
                var contactExists = _PrincipalDashboardContext.SaveChanges();
                if (contactExists == 1)
                {
                    data.returnval = true;
                    data.returnsavestatus = "saved";
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch(Exception ex)
            {

            }
            return data;
        }

        public Notice_DTO Getdetails(Notice_DTO data)
        {
            try
            {
                var query = _PrincipalDashboardContext.IVRM_NoticeBoardDMO.Where(s => s.MI_Id == data.MI_Id).ToArray();
                if (query.Length > 0)
                {
                    data.notice_data = query;
                    data.returnval = true;
                    //data.returnsavestatus = "saved";
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch(Exception e)
            {

            }
            return data;
        }

        public Notice_DTO deactivate(Notice_DTO data)
        {
            try
            {
                var query = _PrincipalDashboardContext.IVRM_NoticeBoardDMO.Single(s => s.MI_Id == data.MI_Id && s.INTB_Id==data.INTB_Id);
               
                    if (query.INTB_ActiveFlag == true)
                    {
                        query.INTB_ActiveFlag = false;
                    }
                    else
                    {
                         query.INTB_ActiveFlag = true;
                    }
                    query.UpdatedDate = DateTime.Now;
                    _PrincipalDashboardContext.Update(query);
                    var contactExists = _PrincipalDashboardContext.SaveChanges();
                    if (contactExists == 1)
                    {
                        data.returnval = true;
                        //data.returnsavestatus = "saved";
                    }
                    else
                    {
                        data.returnval = false;
                    }
               
                   
            }
            catch (Exception e)
            {

            }
            return data;
        }
    }
}
