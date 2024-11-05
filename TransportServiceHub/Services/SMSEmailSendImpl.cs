using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.admission;
using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.Birthday;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using CommonLibrary;
using System.Threading.Tasks;

namespace TransportServiceHub.Services
{
    public class SMSEmailSendImpl : Interfaces.SMSEmailSendInterface
    {
        private static ConcurrentDictionary<string, SMSEmailSendDTO> _login =
      new ConcurrentDictionary<string, SMSEmailSendDTO>();

        public TransportContext _context;
        ILogger<SMSEmailSendImpl> _areaimpl;
        public DomainModelMsSqlServerContext _db;
        public SMSEmailSendImpl(ILogger<SMSEmailSendImpl> areaimpl, TransportContext context, DomainModelMsSqlServerContext db)
        {

            _areaimpl = areaimpl;
            _context = context;
            _db = db;

        }
        public SMSEmailSendDTO getdata(int id)
        {
            SMSEmailSendDTO data = new SMSEmailSendDTO();
            data.MI_Id = id;
            try
            {
                List<AcademicYear> allyear = new List<AcademicYear>();

                allyear = _context.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == id).OrderByDescending(t => t.ASMAY_Order).ToList();

                data.YearList = allyear.Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

                List<School_M_Class> allclass = new List<School_M_Class>();
                allclass = _context.School_M_Class.Where(y => y.ASMCL_ActiveFlag == true && y.MI_Id == id).ToList();
                data.classlist = allclass.Distinct().OrderBy(a => a.ASMCL_Order).ToArray();

                List<School_M_Section> allsec = new List<School_M_Section>();
                allsec = _context.School_M_Section.Where(s => s.MI_Id == id && s.ASMC_ActiveFlag==1).ToList();
                data.sectionlist = allsec.Distinct().OrderBy(a => a.ASMC_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Transport Error Driver Char savedata" + ex.Message);
            }

            return data;
        }
        public SMSEmailSendDTO Getreportdetails(SMSEmailSendDTO data)
        {
            try
            {
                var checkdefalutsms = _db.AdmissionStandardDMO.Where(a => a.MI_Id == data.MI_Id).ToList();

                List<long?> classid = new List<long?>();
                List<long> sectionid = new List<long>();

                if (data.ASMCL_Id > 0)
                {
                    var classidlist = (from a in _db.AcademicYear
                                       from b in _db.Masterclasscategory
                                       from c in _db.School_M_Class
                                       where (a.ASMAY_Id == b.ASMAY_Id && b.ASMCL_Id == c.ASMCL_Id && a.Is_Active == true && b.Is_Active == true
                                       && c.ASMCL_ActiveFlag == true && b.ASMCL_Id == data.ASMCL_Id && b.ASMAY_Id == data.ASMAY_Id)
                                       select c).Distinct().ToList();
                    foreach (var x in classidlist)
                    {
                        classid.Add(x.ASMCL_Id);
                    }
                }

                else
                {
                    var classidlist = (from a in _db.AcademicYear
                                       from b in _db.Masterclasscategory
                                       from c in _db.School_M_Class
                                       where (a.ASMAY_Id == b.ASMAY_Id && b.ASMCL_Id == c.ASMCL_Id && a.Is_Active == true && b.Is_Active == true
                                       && c.ASMCL_ActiveFlag == true && b.ASMAY_Id == data.ASMAY_Id)
                                       select c).Distinct().ToList();
                    foreach (var x in classidlist)
                    {
                        classid.Add(x.ASMCL_Id);
                    }
                }

                if (data.ASMS_Id != 0)
                {
                    var sectionidlist = (from a in _db.AcademicYear
                                         from b in _db.Masterclasscategory
                                         from c in _db.School_M_Class
                                         from d in _db.AdmSchoolMasterClassCatSec
                                         from e in _db.School_M_Section
                                         where (a.ASMAY_Id == b.ASMAY_Id && b.ASMCL_Id == c.ASMCL_Id && a.Is_Active == true && b.Is_Active == true
                                         && d.ASMCC_Id == b.ASMCC_Id && d.ASMS_Id == e.ASMS_Id && e.ASMC_ActiveFlag == 1 && c.ASMCL_ActiveFlag == true
                                         && classid.Contains(b.ASMCL_Id) && b.ASMAY_Id == data.ASMAY_Id && d.ASMS_Id == data.ASMS_Id)
                                         select e).Distinct().ToList();
                    foreach (var y in sectionidlist)
                    {
                        sectionid.Add(y.ASMS_Id);
                    }
                }
                else
                {
                    var sectionidlist = (from a in _db.AcademicYear
                                         from b in _db.Masterclasscategory
                                         from c in _db.School_M_Class
                                         from d in _db.AdmSchoolMasterClassCatSec
                                         from e in _db.School_M_Section
                                         where (a.ASMAY_Id == b.ASMAY_Id && b.ASMCL_Id == c.ASMCL_Id && a.Is_Active == true && b.Is_Active == true
                                         && d.ASMCC_Id == b.ASMCC_Id && d.ASMS_Id == e.ASMS_Id && e.ASMC_ActiveFlag == 1 && c.ASMCL_ActiveFlag == true
                                         && classid.Contains(b.ASMCL_Id) && b.ASMAY_Id == data.ASMAY_Id)
                                         select e).Distinct().ToList();
                    foreach (var y in sectionidlist)
                    {
                        sectionid.Add(y.ASMS_Id);
                    }
                }


                if (checkdefalutsms.FirstOrDefault().ASC_DefaultSMS_Flag == "F")
                {
                    if (data.filterdata == "stureg")
                    {
                        data.messagelist = (from b in _context.Adm_M_Student
                                            from c in _context.School_M_Class
                                            from d in _context.School_M_Section
                                            from e in _context.School_Adm_Y_StudentDMO
                                            from f in _context.ApplUser
                                            from a in _context.StudentAppUserLoginDMO
                                            where (a.AMST_ID == b.AMST_Id && e.AMST_Id == b.AMST_Id && f.Id == a.STD_APP_ID && c.ASMCL_Id == e.ASMCL_Id
                                            && d.ASMS_Id == e.ASMS_Id && b.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id && classid.Contains(e.ASMCL_Id)
                                            && sectionid.Contains(e.ASMS_Id) && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1 && e.AMAY_ActiveFlag == 1)
                                            select new SMSEmailSendDTO
                                            {
                                                ASMCL_ClassName = c.ASMCL_ClassName,
                                                ASMC_SectionName = d.ASMC_SectionName,
                                                username = f.UserName,
                                                password = "Password@123",
                                                studentname = ((b.AMST_FirstName == null ? "" : b.AMST_FirstName) +
                                                (b.AMST_MiddleName == null ? "" : " " + b.AMST_MiddleName) +
                                                (b.AMST_LastName == null ? "" : " " + b.AMST_LastName)).Trim(),
                                                AMST_AdmNo = b.AMST_AdmNo == null ? "" : b.AMST_AdmNo,
                                                AMST_MobileNo = Convert.ToInt64(b.AMST_FatherMobleNo),
                                                AMST_emailId = b.AMST_FatheremailId,
                                                AMST_Id = b.AMST_Id
                                            }).Distinct().ToArray();

                    }

                    else if (data.filterdata == "stutra")
                    {
                        data.messagelist = (from b in _context.Adm_M_Student
                                            from c in _context.School_M_Class
                                            from d in _context.School_M_Section
                                            from e in _context.School_Adm_Y_StudentDMO
                                            from f in _context.ApplUser
                                            from a in _context.StudentAppUserLoginDMO
                                            from g in _context.TR_Student_RouteDMO
                                            where (g.AMST_Id == b.AMST_Id && b.MI_Id == g.MI_Id && a.AMST_ID == b.AMST_Id && e.AMST_Id == b.AMST_Id
                                            && f.Id == a.STD_APP_ID && c.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == e.ASMS_Id && b.MI_Id == data.MI_Id
                                            && e.ASMAY_Id == data.ASMAY_Id && classid.Contains(e.ASMCL_Id) && sectionid.Contains(e.ASMS_Id) && b.AMST_SOL == "S"
                                            && b.AMST_ActiveFlag == 1 && e.AMAY_ActiveFlag == 1 && g.ASMAY_Id == data.ASMAY_Id && g.TRSR_ActiveFlg == true)
                                            select new SMSEmailSendDTO
                                            {
                                                ASMCL_ClassName = c.ASMCL_ClassName,
                                                ASMC_SectionName = d.ASMC_SectionName,
                                                username = f.UserName,
                                                password = "Password@123",
                                                studentname = ((b.AMST_FirstName == null ? "" : b.AMST_FirstName) +
                                                (b.AMST_MiddleName == null ? "" : " " + b.AMST_MiddleName) +
                                                (b.AMST_LastName == null ? "" : " " + b.AMST_LastName)).Trim(),
                                                AMST_AdmNo = b.AMST_AdmNo == null ? "" : b.AMST_AdmNo,
                                                AMST_MobileNo = Convert.ToInt64(b.AMST_FatherMobleNo),
                                                AMST_emailId = b.AMST_FatheremailId,
                                                AMST_Id = b.AMST_Id
                                            }).Distinct().ToArray();

                    }

                    else if (data.filterdata == "stunew")
                    {
                        data.messagelist = (from b in _context.Adm_M_Student
                                            from c in _context.School_M_Class
                                            from f in _context.ApplUser
                                            from a in _context.StudentAppUserLoginDMO

                                            where (a.AMST_ID == b.AMST_Id
                                            && f.Id == a.STD_APP_ID && b.MI_Id == data.MI_Id && c.ASMCL_Id == b.ASMCL_Id
                                            && classid.Contains(b.ASMCL_Id) && b.AMST_SOL == "S"
                                            && b.AMST_ActiveFlag == 1 && b.ASMAY_Id == data.ASMAY_Id)
                                            select new SMSEmailSendDTO
                                            {
                                                ASMCL_ClassName = c.ASMCL_ClassName,
                                                username = f.UserName,
                                                password = "Password@123",
                                                studentname = ((b.AMST_FirstName == null ? "" : b.AMST_FirstName) +
                                                (b.AMST_MiddleName == null ? "" : " " + b.AMST_MiddleName) +
                                                (b.AMST_LastName == null ? "" : " " + b.AMST_LastName)).Trim(),
                                                AMST_AdmNo = b.AMST_AdmNo == null ? "" : b.AMST_AdmNo,
                                                AMST_MobileNo = Convert.ToInt64(b.AMST_FatherMobleNo),
                                                AMST_emailId = b.AMST_FatheremailId,
                                                AMST_Id = b.AMST_Id
                                            }).Distinct().ToArray();
                    }

                }

                else if (checkdefalutsms.FirstOrDefault().ASC_DefaultSMS_Flag == "M")
                {
                    if (data.filterdata == "stureg")
                    {
                        data.messagelist = (from b in _context.Adm_M_Student
                                            from c in _context.School_M_Class
                                            from d in _context.School_M_Section
                                            from e in _context.School_Adm_Y_StudentDMO
                                            from f in _context.ApplUser
                                            from a in _context.StudentAppUserLoginDMO
                                            where (a.AMST_ID == b.AMST_Id && e.AMST_Id == b.AMST_Id && f.Id == a.STD_APP_ID && c.ASMCL_Id == e.ASMCL_Id
                                            && d.ASMS_Id == e.ASMS_Id && b.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id && classid.Contains(e.ASMCL_Id)
                                            && sectionid.Contains(e.ASMS_Id) && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1 && e.AMAY_ActiveFlag == 1)
                                            select new SMSEmailSendDTO
                                            {
                                                ASMCL_ClassName = c.ASMCL_ClassName,
                                                ASMC_SectionName = d.ASMC_SectionName,
                                                username = f.UserName,
                                                password = "Password@123",
                                                studentname = ((b.AMST_FirstName == null ? "" : b.AMST_FirstName) +
                                                (b.AMST_MiddleName == null ? "" : " " + b.AMST_MiddleName) +
                                                (b.AMST_LastName == null ? "" : " " + b.AMST_LastName)).Trim(),
                                                AMST_AdmNo = b.AMST_AdmNo == null ? "" : b.AMST_AdmNo,
                                                AMST_MobileNo = Convert.ToInt64(b.AMST_MotherMobileNo),
                                                AMST_emailId = b.AMST_MotherEmailId,
                                                AMST_Id = b.AMST_Id
                                            }).Distinct().ToArray();

                    }

                    else if (data.filterdata == "stutra")
                    {
                        data.messagelist = (from b in _context.Adm_M_Student
                                            from c in _context.School_M_Class
                                            from d in _context.School_M_Section
                                            from e in _context.School_Adm_Y_StudentDMO
                                            from f in _context.ApplUser
                                            from a in _context.StudentAppUserLoginDMO
                                            from g in _context.TR_Student_RouteDMO
                                            where (g.AMST_Id == b.AMST_Id && b.MI_Id == g.MI_Id && a.AMST_ID == b.AMST_Id && e.AMST_Id == b.AMST_Id
                                            && f.Id == a.STD_APP_ID && c.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == e.ASMS_Id && b.MI_Id == data.MI_Id
                                            && e.ASMAY_Id == data.ASMAY_Id && classid.Contains(e.ASMCL_Id) && sectionid.Contains(e.ASMS_Id) && b.AMST_SOL == "S"
                                            && b.AMST_ActiveFlag == 1 && e.AMAY_ActiveFlag == 1 && g.ASMAY_Id == data.ASMAY_Id && g.TRSR_ActiveFlg == true)
                                            select new SMSEmailSendDTO
                                            {
                                                ASMCL_ClassName = c.ASMCL_ClassName,
                                                ASMC_SectionName = d.ASMC_SectionName,
                                                username = f.UserName,
                                                password = "Password@123",
                                                studentname = ((b.AMST_FirstName == null ? "" : b.AMST_FirstName) +
                                                (b.AMST_MiddleName == null ? "" : " " + b.AMST_MiddleName) +
                                                (b.AMST_LastName == null ? "" : " " + b.AMST_LastName)).Trim(),
                                                AMST_AdmNo = b.AMST_AdmNo == null ? "" : b.AMST_AdmNo,
                                                AMST_MobileNo = Convert.ToInt64(b.AMST_MotherMobileNo),
                                                AMST_emailId = b.AMST_MotherEmailId,
                                                AMST_Id = b.AMST_Id
                                            }).Distinct().ToArray();
                    }

                    else if (data.filterdata == "stunew")
                    {
                        data.messagelist = (from b in _context.Adm_M_Student
                                            from c in _context.School_M_Class
                                            from f in _context.ApplUser
                                            from a in _context.StudentAppUserLoginDMO

                                            where (a.AMST_ID == b.AMST_Id && b.ASMCL_Id == c.ASMCL_Id
                                            && f.Id == a.STD_APP_ID && b.MI_Id == data.MI_Id
                                            && classid.Contains(b.ASMCL_Id) && b.AMST_SOL == "S"
                                            && b.AMST_ActiveFlag == 1 && b.ASMAY_Id == data.ASMAY_Id)
                                            select new SMSEmailSendDTO
                                            {
                                                ASMCL_ClassName = c.ASMCL_ClassName,
                                                username = f.UserName,
                                                password = "Password@123",
                                                studentname = ((b.AMST_FirstName == null ? "" : b.AMST_FirstName) +
                                                (b.AMST_MiddleName == null ? "" : " " + b.AMST_MiddleName) +
                                                (b.AMST_LastName == null ? "" : " " + b.AMST_LastName)).Trim(),
                                                AMST_AdmNo = b.AMST_AdmNo == null ? "" : b.AMST_AdmNo,
                                                AMST_MobileNo = Convert.ToInt64(b.AMST_MotherMobileNo),
                                                AMST_emailId = b.AMST_FatheremailId,
                                                AMST_Id = b.AMST_Id
                                            }).Distinct().ToArray();

                    }
                }

                else
                {
                    if (data.filterdata == "stureg")
                    {
                        data.messagelist = (from b in _context.Adm_M_Student
                                            from c in _context.School_M_Class
                                            from d in _context.School_M_Section
                                            from e in _context.School_Adm_Y_StudentDMO
                                            from f in _context.ApplUser
                                            from a in _context.StudentAppUserLoginDMO
                                            where (a.AMST_ID == b.AMST_Id && e.AMST_Id == b.AMST_Id && f.Id == a.STD_APP_ID && c.ASMCL_Id == e.ASMCL_Id
                                            && d.ASMS_Id == e.ASMS_Id && b.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id && classid.Contains(e.ASMCL_Id)
                                            && sectionid.Contains(e.ASMS_Id) && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1 && e.AMAY_ActiveFlag == 1)
                                            select new SMSEmailSendDTO
                                            {
                                                ASMCL_ClassName = c.ASMCL_ClassName,
                                                ASMC_SectionName = d.ASMC_SectionName,
                                                username = f.UserName,
                                                password = "Password@123",
                                                studentname = ((b.AMST_FirstName == null ? "" : b.AMST_FirstName) +
                                                (b.AMST_MiddleName == null ? "" : " " + b.AMST_MiddleName) +
                                                (b.AMST_LastName == null ? "" : " " + b.AMST_LastName)).Trim(),
                                                AMST_AdmNo = b.AMST_AdmNo == null ? "" : b.AMST_AdmNo,
                                                AMST_MobileNo = b.AMST_MobileNo,
                                                AMST_emailId = b.AMST_emailId,
                                                AMST_Id = b.AMST_Id
                                            }).Distinct().ToArray();

                    }

                    else if (data.filterdata == "stutra")
                    {
                        data.messagelist = (from b in _context.Adm_M_Student
                                            from c in _context.School_M_Class
                                            from d in _context.School_M_Section
                                            from e in _context.School_Adm_Y_StudentDMO
                                            from f in _context.ApplUser
                                            from a in _context.StudentAppUserLoginDMO
                                            from g in _context.TR_Student_RouteDMO
                                            where (g.AMST_Id == b.AMST_Id && b.MI_Id == g.MI_Id && a.AMST_ID == b.AMST_Id && e.AMST_Id == b.AMST_Id
                                            && f.Id == a.STD_APP_ID && c.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == e.ASMS_Id && b.MI_Id == data.MI_Id
                                            && e.ASMAY_Id == data.ASMAY_Id && classid.Contains(e.ASMCL_Id) && sectionid.Contains(e.ASMS_Id) && b.AMST_SOL == "S"
                                            && b.AMST_ActiveFlag == 1 && e.AMAY_ActiveFlag == 1 && g.ASMAY_Id == data.ASMAY_Id && g.TRSR_ActiveFlg == true)
                                            select new SMSEmailSendDTO
                                            {
                                                ASMCL_ClassName = c.ASMCL_ClassName,
                                                ASMC_SectionName = d.ASMC_SectionName,
                                                username = f.UserName,
                                                password = "Password@123",
                                                studentname = ((b.AMST_FirstName == null ? "" : b.AMST_FirstName) +
                                                (b.AMST_MiddleName == null ? "" : " " + b.AMST_MiddleName) +
                                                (b.AMST_LastName == null ? "" : " " + b.AMST_LastName)).Trim(),
                                                AMST_AdmNo = b.AMST_AdmNo == null ? "" : b.AMST_AdmNo,
                                                AMST_MobileNo = b.AMST_MobileNo,
                                                AMST_emailId = b.AMST_emailId,
                                                AMST_Id = b.AMST_Id
                                            }).Distinct().ToArray();
                    }

                    else if (data.filterdata == "stunew")
                    {
                        data.messagelist = (from b in _context.Adm_M_Student
                                            from c in _context.School_M_Class
                                            from f in _context.ApplUser
                                            from a in _context.StudentAppUserLoginDMO

                                            where (a.AMST_ID == b.AMST_Id && b.ASMCL_Id==c.ASMCL_Id
                                            && f.Id == a.STD_APP_ID && b.MI_Id == data.MI_Id
                                            && classid.Contains(b.ASMCL_Id) && b.AMST_SOL == "S"
                                            && b.AMST_ActiveFlag == 1 && b.ASMAY_Id == data.ASMAY_Id)
                                            select new SMSEmailSendDTO
                                            {
                                                ASMCL_ClassName = c.ASMCL_ClassName,
                                                username = f.UserName,
                                                password = "Password@123",
                                                studentname = ((b.AMST_FirstName == null ? "" : b.AMST_FirstName) +
                                                (b.AMST_MiddleName == null ? "" : " " + b.AMST_MiddleName) +
                                                (b.AMST_LastName == null ? "" : " " + b.AMST_LastName)).Trim(),
                                                AMST_AdmNo = b.AMST_AdmNo == null ? "" : b.AMST_AdmNo,
                                                AMST_MobileNo = b.AMST_MobileNo,
                                                AMST_emailId = b.AMST_FatheremailId,
                                                AMST_Id = b.AMST_Id
                                            }).Distinct().ToArray();

                    }
                }

                if (data.filterdata == "Alumni")
                {
                    if (data.ASMCL_Id == 0)
                    {
                        var getclassids = _db.School_M_Class.Where(a => a.MI_Id == data.MI_Id).ToList();

                        foreach (var c in getclassids)
                        {
                            classid.Add(c.ASMCL_Id);
                        }
                    }
                    else
                    {
                        classid.Add(data.ASMCL_Id);
                    }


                    data.messagelist = (from a in _db.Alumni_M_StudentDMO
                                        from b in _db.School_M_Class
                                        from c in _db.ApplicationUser
                                        from d in _db.AlumniUserRegistrationDMO
                                        from e in _db.Alumni_User_LoginDMO
                                        where (a.ALMST_Id == d.ALMST_Id && e.ALSREG_Id == d.ALSREG_Id && a.ASMCL_Id_Left == b.ASMCL_Id && c.Id == e.IVRMUL_Id
                                        && a.MI_Id == data.MI_Id && a.ASMAY_Id_Left == data.ASMAY_Id && classid.Contains(a.ASMCL_Id_Left)
                                        )
                                        select new SMSEmailSendDTO
                                        {
                                            ASMCL_ClassName = b.ASMCL_ClassName,
                                            username = c.UserName,
                                            password = "Password@123",
                                            studentname = ((a.ALMST_FirstName == null ? "" : a.ALMST_FirstName) +
                                            (a.ALMST_MiddleName == null ? "" : " " + a.ALMST_MiddleName) +
                                            (a.ALMST_LastName == null ? "" : " " + a.ALMST_LastName)).Trim(),
                                            AMST_AdmNo = a.ALMST_AdmNo == null ? "" : a.ALMST_AdmNo,
                                            AMST_MobileNo = Convert.ToInt64(a.ALMST_MobileNo),
                                            AMST_emailId = a.ALMST_emailId,
                                            AMST_Id = a.ALMST_Id
                                        }).Distinct().ToArray();

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Transport Error Driver Char savedata" + ex.Message);
            }
            return data;
        }
        public async Task<SMSEmailSendDTO> smssend(SMSEmailSendDTO data)
        {
            SMS sms = new SMS(_db);

            for (var i = 0; i < data.data_array.Length; i++)
            {
                if (data.data_array[i].AMST_MobileNo != 0)
                {
                    if (data.filterdata != "Alumni")
                    {
                        data.success = await sms.sendSms(data.MI_Id, data.data_array[i].AMST_MobileNo, "ADMINISTRATOR", data.data_array[i].AMST_Id);
                    }
                    else
                    {
                        data.success = await sms.sendSms(data.MI_Id, data.data_array[i].AMST_MobileNo, "ALUMNIUSER", data.data_array[i].AMST_Id);
                    }

                }
            }
            return data;
        }


        public async Task<SMSEmailSendDTO> sendWhatsAppCall(SMSEmailSendDTO data)
        {
            Email whatsapp = new Email(_db);
            //SMS sms = new SMS(_db);
            string s = "";
            for (var i = 0; i < data.data_array.Length; i++)
            {
                if (data.data_array[i].AMST_MobileNo != 0)
                {
                    if (data.filterdata != "Alumni")
                    {
                        s  = await whatsapp.Whatsapp(data.MI_Id, data.data_array[i].AMST_MobileNo.ToString(), "administrator", data.data_array[i].AMST_Id);
                    }
                    else
                    {
                         s = await whatsapp.Whatsapp(data.MI_Id, data.data_array[i].AMST_MobileNo.ToString(), "ALUMNIUSER", data.data_array[i].AMST_Id);


                    }

                }
            }
            if(s =="success")
            {
                data.success = "success";
            }
            return data;
        }





        public SMSEmailSendDTO emailsend(SMSEmailSendDTO data)
        {

            Email Email = new Email(_db);
            for (var i = 0; i < data.data_array.Length; i++)
            {
                if (data.data_array[i].AMST_emailId != "null" && data.data_array[i].AMST_emailId != null && data.data_array[i].AMST_emailId != "")
                {
                    if (data.filterdata != "Alumni")
                    {
                        data.success = Email.sendmail(data.MI_Id, data.data_array[i].AMST_emailId, "ADMINISTRATOR", data.data_array[i].AMST_Id);
                    }
                    else
                    {
                        data.success = Email.sendmail(data.MI_Id, data.data_array[i].AMST_emailId, "ALUMNIUSER", data.data_array[i].AMST_Id);
                    }
                }
            }
            return data;
        }
    }
}