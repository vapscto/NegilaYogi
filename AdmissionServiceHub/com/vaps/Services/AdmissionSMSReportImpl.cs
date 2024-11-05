using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.admission;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.Birthday;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class AdmissionSMSReportImpl : Interfaces.AdmissionSMSReportInterface
    {
        private static ConcurrentDictionary<string, AdmissionSMSReportDTO> _login =
            new ConcurrentDictionary<string, AdmissionSMSReportDTO>();

        public DomainModelMsSqlServerContext _DomainModelMsSqlServerContext;        

        public AdmissionSMSReportImpl(DomainModelMsSqlServerContext DomainModelMsSqlServerContext)
        {
            _DomainModelMsSqlServerContext = DomainModelMsSqlServerContext;
        }

        public AdmissionSMSReportDTO getdetails(AdmissionSMSReportDTO data)
        {
            try
            {
                data.moduledetails = _DomainModelMsSqlServerContext.masterModule.Where(g => g.Module_ActiveFlag == 1).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public AdmissionSMSReportDTO Getreportdetails(AdmissionSMSReportDTO data)
        {
            try
            {

                if (data.onclickloaddata == "All")
                {
                    if (data.dailybtedates == "daily")
                    {
                        if (data.regorname == "email")
                        {
                            data.messagelist = (from b in _DomainModelMsSqlServerContext.IVRM_Email_sentBox
                                                where b.MI_Id == data.MI_Id && Convert.ToDateTime(b.Datetime).ToString("yyyy-MM-dd") == data.dailydate.ToString("yyyy-MM-dd")
                                                select new AdmissionSMSReportDTO
                                                {
                                                    IVRMESB_ID = b.IVRMESB_ID,
                                                    message = b.Message,
                                                    EmailId = b.Email_Id,
                                                    Datetime = Convert.ToDateTime(b.Datetime),
                                                    Module_Name = b.Module_Name,

                                                }).ToArray();
                        }

                        else if (data.regorname == "sms")
                        {
                            data.messagelist = (from a in _DomainModelMsSqlServerContext.IVRM_sms_sentBoxDMO
                                                where a.MI_Id == data.MI_Id && a.Datetime.ToString("dd/MM/yyyy") == data.dailydate.ToString("dd/MM/yyyy")
                                                select new AdmissionSMSReportDTO
                                                {
                                                    IVRM_SSB_ID = a.IVRM_SSB_ID,
                                                    Module_Name = a.Module_Name,
                                                    mobile = a.Mobile_no,
                                                    message = a.Message,
                                                    Datetime = a.Datetime,

                                                }).ToArray();
                        }
                    }
                    else if (data.dailybtedates == "btwdates")
                    {
                        if (data.regorname == "email")
                        {
                            data.messagelist = (from b in _DomainModelMsSqlServerContext.IVRM_Email_sentBox
                                                where b.MI_Id == data.MI_Id && (b.Datetime >= data.fromdate && b.Datetime <= data.todate)

                                                select new AdmissionSMSReportDTO
                                                {
                                                    IVRMESB_ID = b.IVRMESB_ID,
                                                    message = b.Message,
                                                    EmailId = b.Email_Id,
                                                    Datetime = Convert.ToDateTime(b.Datetime),
                                                    Module_Name = b.Module_Name,
                                                }).ToArray();
                        }

                        else if (data.regorname == "sms")
                        {
                            data.messagelist = (from a in _DomainModelMsSqlServerContext.IVRM_sms_sentBoxDMO
                                                where a.MI_Id == data.MI_Id && (a.Datetime >= data.fromdate && a.Datetime <= data.todate)
                                                select new AdmissionSMSReportDTO
                                                {
                                                    IVRM_SSB_ID = a.IVRM_SSB_ID,
                                                    Module_Name = a.Module_Name,
                                                    mobile = a.Mobile_no,
                                                    message = a.Message,
                                                    Datetime = a.Datetime,
                                                }).ToArray();
                        }

                    }

                }
                else if (data.onclickloaddata == "indi")
                {
                    if (data.dailybtedates == "daily")
                    {
                        if (data.regorname == "email")
                        {

                            data.messagelist = (from b in _DomainModelMsSqlServerContext.IVRM_Email_sentBox
                                                where b.MI_Id == data.MI_Id && Convert.ToDateTime(b.Datetime).ToString("dd/MM/yyyy") == data.dailydate.ToString("dd/MM/yyyy")
                                                 && data.mdata.Contains(b.Module_Name)
                                                select new AdmissionSMSReportDTO
                                                {
                                                    IVRMESB_ID = b.IVRMESB_ID,
                                                    Module_Name = b.Module_Name,
                                                    stud_count = b.IVRMESB_ID,
                                                }).Distinct().GroupBy(id => new { id.Module_Name }).Select(g => new AdmissionSMSReportDTO { Module_Name = g.Key.Module_Name, stud_count = g.Count() }).ToArray();
                        }

                        else if (data.regorname == "sms")
                        {
                            data.messagelist = (from a in _DomainModelMsSqlServerContext.IVRM_sms_sentBoxDMO
                                                where a.MI_Id == data.MI_Id && a.Datetime.ToString("dd/MM/yyyy") == data.dailydate.ToString("dd/MM/yyyy")
                                                 && data.mdata.Contains(a.Module_Name)
                                                select new AdmissionSMSReportDTO
                                                {
                                                    IVRM_SSB_ID = a.IVRM_SSB_ID,
                                                    Module_Name = a.Module_Name,
                                                    stud_count = a.IVRM_SSB_ID,
                                                }).Distinct().GroupBy(id => new { id.Module_Name }).Select(g => new AdmissionSMSReportDTO { Module_Name = g.Key.Module_Name, stud_count = g.Count() }).ToArray();
                        }
                    }
                    else if (data.dailybtedates == "btwdates")
                    {
                        if (data.regorname == "email")
                        {

                            data.messagelist = (from b in _DomainModelMsSqlServerContext.IVRM_Email_sentBox
                                                where (b.MI_Id == data.MI_Id && (b.Datetime >= data.fromdate && b.Datetime <= data.todate) && data.mdata.Contains(b.Module_Name))
                                                select new AdmissionSMSReportDTO
                                                {
                                                    IVRMESB_ID = b.IVRMESB_ID,
                                                    Module_Name = b.Module_Name,
                                                    stud_count = b.IVRMESB_ID,
                                                }).Distinct().GroupBy(id => new { id.Module_Name }).Select(g => new AdmissionSMSReportDTO { Module_Name = g.Key.Module_Name, stud_count = g.Count() }).ToArray();
                        }

                        else if (data.regorname == "sms")
                        {
                            data.messagelist = (from a in _DomainModelMsSqlServerContext.IVRM_sms_sentBoxDMO
                                                where a.MI_Id == data.MI_Id && (a.Datetime >= data.fromdate && a.Datetime <= data.todate)
                                                 && data.mdata.Contains(a.Module_Name)
                                                select new AdmissionSMSReportDTO
                                                {
                                                    IVRM_SSB_ID = a.IVRM_SSB_ID,
                                                    Module_Name = a.Module_Name,

                                                    stud_count = a.IVRM_SSB_ID,

                                                }).Distinct().GroupBy(id => new { id.Module_Name }).Select(g => new AdmissionSMSReportDTO { Module_Name = g.Key.Module_Name, stud_count = g.Count() }).ToArray();
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
            return data;
        }






    }
}
