using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using System;
using DomainModel.Model.com.vaps.admission;
using AutoMapper;
using PreadmissionDTOs.com.vaps.admission;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DomainModel.Model.com.vapstech.TT;
using CommonLibrary;
using SendGrid.Helpers.Mail;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using SendGrid;
using System.Net;
using Newtonsoft.Json;
using System.Net.NetworkInformation;
using System.Web;


namespace AdmissionServiceHub.com.vaps.Services
{
    public class SMSMasterApprovalImpl : Interfaces.SMSMasterApprovalInterface
    {
        private readonly AdmissionFormContext _SMSAPLContext;
       
        public DomainModelMsSqlServerContext _db;
        public SMSMasterApprovalImpl(AdmissionFormContext cpContext, DomainModelMsSqlServerContext db)
        {
            _SMSAPLContext = cpContext;
            _db = db;
        }
        public SMSMasterApprovalDTO Getdetails(SMSMasterApprovalDTO data)//int IVRMM_Id
        {

            try
            {
                data.userNamelist = _SMSAPLContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id).OrderBy(t => t.IVRMSTAUL_UserName).ToArray();

                data.smsemailaplist = (from a in _SMSAPLContext.SMSMasterApprovalDMO
                                       from b in _SMSAPLContext.Staff_User_Login
                                       where (b.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && a.IVRMUL_Id == b.Id)
                                       select new SMSMasterApprovalDTO
                                       {
                                           SMA_Id = a.SMA_Id,
                                           IVRMSTAUL_Id = b.Id,
                                           IVRMSTAUL_UserName = b.IVRMSTAUL_UserName,
                                           SMA_Level = a.SMA_Level,
                                           SMA_ActiveFlag = a.SMA_ActiveFlag,
                                           SMA_SMSMailCallFlag = a.SMA_SMSMailCallFlag,
                                           headername=a.SMA_HeaderName
                                       }

                   ).ToArray();

                var templatelist = _SMSAPLContext.SMSEmailSetting.Where(t => t.MI_Id == data.MI_Id).Distinct().ToList();

                data.headernamelist = templatelist.Distinct().ToArray();


            }
            catch (Exception ex)
            {

                throw ex;
            }




            return data;

        }




        public SMSMasterApprovalDTO deactivate(SMSMasterApprovalDTO data)
        {


            try
            {


                if (data.SMA_Id > 0)
                {
                    var result = _SMSAPLContext.SMSMasterApprovalDMO.Single(t => t.SMA_Id == data.SMA_Id);
                    if (result.SMA_ActiveFlag == true)
                    {

                        result.SMA_ActiveFlag = false;

                        _SMSAPLContext.Update(result);


                    }
                    else
                    {
                        result.SMA_ActiveFlag = true;

                        _SMSAPLContext.Update(result);
                    }


                    var flag = _SMSAPLContext.SaveChanges();
                    if (flag == 1)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }







            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;

        }
        public SMSMasterApprovalDTO editdata(SMSMasterApprovalDTO data)//int IVRMM_Id
        {


            try
            {
                if (data.SMA_Id > 0)
                {
                    data.editdata = _SMSAPLContext.SMSMasterApprovalDMO.Where(t => t.MI_Id == data.MI_Id && t.SMA_Id == data.SMA_Id).ToArray();
                }





            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;

        }


        public SMSMasterApprovalDTO GetAttendence(SMSMasterApprovalDTO data)//int IVRMM_Id
        {


            try
            {






            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;

        }



        public SMSMasterApprovalDTO savedetails(SMSMasterApprovalDTO data)
        {
            try
            {
                data.dup = "";

                if (data.SMA_Id > 0)
                {
                    var res = _SMSAPLContext.SMSMasterApprovalDMO.Where(t => t.MI_Id == data.MI_Id && t.SMA_Level == data.SMA_Level && t.SMA_SMSMailCallFlag == data.SMA_SMSMailCallFlag && t.SMA_Id != data.SMA_Id && t.IVRMUL_Id == data.IVRMSTAUL_Id && t.SMA_HeaderName==data.headername && t.SMA_ActiveFlag==true).ToList();
                    if (res.Count() > 0)
                    {
                        data.dup = "Duplicate";
                    }
                    else
                    {
                        var levelcheck= _SMSAPLContext.SMSMasterApprovalDMO.Where(t => t.MI_Id == data.MI_Id && t.SMA_Level == data.SMA_Level && t.SMA_SMSMailCallFlag == data.SMA_SMSMailCallFlag && t.SMA_Id != data.SMA_Id && t.SMA_HeaderName == data.headername &&  t.SMA_ActiveFlag == true).ToList();

                        if (levelcheck.Count>0)
                        {
                            data.dup = "level";
                        }
                    
                    else
                    {
                            var result = _SMSAPLContext.SMSMasterApprovalDMO.Single(t => t.MI_Id == data.MI_Id && t.SMA_Id == data.SMA_Id);
                            result.IVRMUL_Id = data.IVRMSTAUL_Id;
                            result.SMA_SMSMailCallFlag = data.SMA_SMSMailCallFlag;
                            result.SMA_Level = data.SMA_Level;
                            result.SMA_HeaderName = data.headername;
                            _SMSAPLContext.Update(result);
                            var contactExists = _SMSAPLContext.SaveChanges();
                            if (contactExists == 1)
                            {
                                data.returnval = true;
                            }
                            else
                            {
                                data.returnval = false;
                            }
                        }
                    }
                }
                else
                {
                    var result = _SMSAPLContext.SMSMasterApprovalDMO.Where(t => t.MI_Id == data.MI_Id && t.IVRMUL_Id == data.IVRMSTAUL_Id && t.SMA_SMSMailCallFlag == data.SMA_SMSMailCallFlag && t.SMA_HeaderName == data.headername && t.SMA_Level == data.SMA_Level).ToList();

                    if (result.Count > 0)
                    {
                        data.dup = "Duplicate";
                    }
                    else
                    {
                        SMSMasterApprovalDMO obj = new SMSMasterApprovalDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.SMA_Level = data.SMA_Level;
                        obj.IVRMUL_Id = data.IVRMSTAUL_Id;
                        obj.SMA_SMSMailCallFlag = data.SMA_SMSMailCallFlag;
                        obj.SMA_HeaderName = data.headername;
                        obj.SMA_ActiveFlag = true;

                        _SMSAPLContext.Add(obj);
                        var contactExists = _SMSAPLContext.SaveChanges();
                        if (contactExists == 1)
                        {

                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }

                }








            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }

    }

 
}
