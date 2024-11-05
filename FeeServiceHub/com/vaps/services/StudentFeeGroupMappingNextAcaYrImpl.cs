using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using FeeServiceHub.com.vaps.interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DomainModel.Model;
using System.Collections.Concurrent;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Data.SqlClient;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using Microsoft.Extensions.Logging;
using DomainModel.Model.com.vapstech.Fee;
using DomainModel.Model.com.vaps.admission;

namespace FeeServiceHub.com.vaps.services
{
    public class StudentFeeGroupMappingNextAcaYrImpl : interfaces.StudentFeeGroupMappingNextAcaYrInterface
    {
        private static ConcurrentDictionary<string, FeeStudentGroupMappingDTO> _login =
           new ConcurrentDictionary<string, FeeStudentGroupMappingDTO>();

        public FeeGroupContext _YearlyFeeGroupMappingContext;
        readonly ILogger<StudentFeeGroupMappingNextAcaYrImpl> _logger;

        public StudentFeeGroupMappingNextAcaYrImpl(FeeGroupContext YearlyFeeGroupMappingContext, ILogger<StudentFeeGroupMappingNextAcaYrImpl> log)
        {
            _YearlyFeeGroupMappingContext = YearlyFeeGroupMappingContext;
            _logger = log;
        }

        public FeeStudentGroupMappingDTO getdata(FeeStudentGroupMappingDTO fee)
        {
            try
            {
                var acayr = _YearlyFeeGroupMappingContext.AcademicYear.Where(r => r.MI_Id == fee.MI_Id && r.Is_Active == true && r.ASMAY_Id == fee.ASMAY_Id).ToList();

                var acaorder = acayr.OrderByDescending(r => r.ASMAY_Order).FirstOrDefault().ASMAY_Order;

                var futureacayrid = _YearlyFeeGroupMappingContext.AcademicYear.Where(r => r.MI_Id == fee.MI_Id && r.Is_Active == true && r.ASMAY_Order == Convert.ToInt32(acaorder) + 1).ToList();

                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.MI_Id == fee.MI_Id && t.Is_Active == true).OrderByDescending(l => l.ASMAY_Order).ToList();
                fee.academicdrp = allyear.Distinct().ToArray();

                List<School_M_Class> classlist = new List<School_M_Class>();
                classlist = _YearlyFeeGroupMappingContext.School_M_Class.Where(t => t.MI_Id == fee.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                fee.fillmasterclass = classlist.ToArray();

                List<School_M_Section> sectionlist = new List<School_M_Section>();
                sectionlist = _YearlyFeeGroupMappingContext.school_M_Section.Where(t => t.MI_Id == fee.MI_Id && t.ASMC_ActiveFlag == 1).ToList();
                fee.fillmastersection = sectionlist.ToArray();

                fee.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                       from b in _YearlyFeeGroupMappingContext.feeGroup
                                       from f in _YearlyFeeGroupMappingContext.FEeGroupLoginPreviledgeDMO
                                       from g in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                       where (a.FMG_Id == b.FMG_Id && a.FMH_Id == f.FMH_Id && a.FMG_Id == f.FMG_ID && a.MI_Id == fee.MI_Id && b.FMG_ActiceFlag == true && a.FYGHM_ActiveFlag == "1" && g.FMH_Id == a.FMH_Id && a.FMG_Id == g.FMG_Id && f.User_Id == fee.user_id && a.ASMAY_Id == futureacayrid[0].ASMAY_Id && a.ASMAY_Id == g.ASMAY_Id)
                                       select new FeeStudentGroupMappingDTO
                                       {
                                           FMG_Id = a.FMG_Id,
                                           FMG_GroupName = b.FMG_GroupName
                                       }).Distinct().ToArray();

                fee.alldata = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                               from c in _YearlyFeeGroupMappingContext.School_M_Class
                               from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                               where (d.ASMAY_Id == fee.ASMAY_Id && a.MI_Id == fee.MI_Id && a.AMST_Id == d.AMST_Id && d.ASMCL_Id == c.ASMCL_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1)
                               select new FeeStudentGroupMappingDTO
                               {
                                   AMST_Id = a.AMST_Id,
                                   AMST_FirstName = a.AMST_FirstName,
                                   AMST_MiddleName = a.AMST_MiddleName,
                                   AMST_LastName = a.AMST_LastName,
                                   AMST_AdmNo = a.AMST_AdmNo,
                                   AMST_RegistrationNo = a.AMST_RegistrationNo,
                                   AMAY_RollNo = d.AMAY_RollNo,
                                   ASMCL_ClassName = c.ASMCL_ClassName

                               }
        ).Distinct().Take(10).ToArray();

               

                var fetchmaxfmsgid = _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO.Where(t => t.MI_Id == fee.MI_Id && t.ASMAY_Id == fee.ASMAY_Id).OrderByDescending(t => t.FMSG_Id).Take(5).Select(t => t.FMSG_Id).ToList();

                fee.alldatathird = (from a in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                                    from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                    from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                    from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                    where (a.FMG_Id==b.FMG_Id && a.AMST_Id==c.AMST_Id && a.AMST_Id==d.AMST_Id && b.FMG_Id==d.FMG_Id && d.MI_Id==fee.MI_Id && d.ASMAY_Id== futureacayrid.FirstOrDefault().ASMAY_Id)
                                    select new FeeStudentGroupMappingDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        AMST_FirstName = c.AMST_FirstName,
                                        AMST_MiddleName = c.AMST_MiddleName,
                                        AMST_LastName = c.AMST_LastName,
                                        AMST_AdmNo = c.AMST_AdmNo,
                                        AMST_RegistrationNo = c.AMST_RegistrationNo,
                                        FMG_GroupName = b.FMG_GroupName,
                                        AMST_Mobile = c.AMST_MobileNo,
                                        FMSG_Id = a.FMSG_Id,
                                        FMG_Id = b.FMG_Id
                                    }
       ).Distinct().OrderBy(t => t.FMSG_Id).ToArray();

                fee.configsetting = _YearlyFeeGroupMappingContext.feemastersettings.Where(s => s.MI_Id == fee.MI_Id && s.userid == fee.user_id).ToList().Distinct().ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return fee;
        }

        public FeeStudentGroupMappingDTO getgroupmappedheads(FeeStudentGroupMappingDTO data)
        {
            try
            {
                if(data.ASMS_Id==0)
                {
                    data.alldata = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                    from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                    where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1)
                                    select new FeeStudentGroupMappingDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        AMST_FirstName = a.AMST_FirstName,
                                        AMST_MiddleName = a.AMST_MiddleName,
                                        AMST_LastName = a.AMST_LastName,
                                        AMST_AdmNo = a.AMST_AdmNo,
                                        AMST_RegistrationNo = a.AMST_RegistrationNo,
                                        AMAY_RollNo = b.AMAY_RollNo
                                    }
   ).OrderBy(t => t.AMST_FirstName).Take(5).ToArray();
                }
                else if (data.ASMS_Id != 0)
                {
                    data.alldata = (from a in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                    from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                    where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id==data.ASMS_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1)
                                    select new FeeStudentGroupMappingDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        AMST_FirstName = a.AMST_FirstName,
                                        AMST_MiddleName = a.AMST_MiddleName,
                                        AMST_LastName = a.AMST_LastName,
                                        AMST_AdmNo = a.AMST_AdmNo,
                                        AMST_RegistrationNo = a.AMST_RegistrationNo,
                                        AMAY_RollNo = b.AMAY_RollNo
                                    }
  ).OrderBy(t => t.AMST_FirstName).Take(5).ToArray();
                }
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FeeStudentGroupMappingDTO savedetails(FeeStudentGroupMappingDTO data)
        {
            try
            {
                if(data.studentdata.Count()>0)
                {
                    for(int i=0;i< data.studentdata.Count();i++)
                    {
                        var acayr = _YearlyFeeGroupMappingContext.AcademicYear.Where(r => r.MI_Id == data.MI_Id && r.Is_Active == true && r.ASMAY_Id == data.ASMAY_Id).ToList();

                        var acaorder = acayr.OrderByDescending(r => r.ASMAY_Order).FirstOrDefault().ASMAY_Order;

                        var futureacayrid = _YearlyFeeGroupMappingContext.AcademicYear.Where(r => r.MI_Id == data.MI_Id && r.Is_Active == true && r.ASMAY_Order == Convert.ToInt32(acaorder) + 1).ToList();


                        var currentclass = _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO.Where(r => r.AMST_Id == data.studentdata[i].AMST_Id && r.ASMAY_Id == data.ASMAY_Id).ToList();

                        var fetchcurrentclass = _YearlyFeeGroupMappingContext.School_M_Class.Where(r => r.ASMCL_Id == currentclass[0].ASMCL_Id && r.MI_Id == data.MI_Id).ToList();

                        var fetchcurrentclassorder = fetchcurrentclass.OrderByDescending(r => r.ASMCL_Order).FirstOrDefault().ASMCL_Order;

                        var futureclass = _YearlyFeeGroupMappingContext.School_M_Class.Where(r => r.MI_Id == data.MI_Id && r.ASMCL_Order == fetchcurrentclassorder + 1).ToList();

                        var confirmstatusss = _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Auto_Fee_Group_mapping_Next_Academic_Year @p0,@p1,@p2,@p3,@p4", data.MI_Id, data.ASMAY_Id, data.user_id, data.studentdata[i].AMST_Id, data.FMG_Id, futureacayrid[0].ASMAY_Id, futureclass[0].ASMCL_Id);

                        if(confirmstatusss>0)
                        {
                            data.returnval = "true";
                        }
                        else
                        {
                            data.returnval = "false";
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                _logger.LogError(ee.Message);
            }
            return data;
        }

        public FeeStudentGroupMappingDTO searching(FeeStudentGroupMappingDTO data)
        {
            try
            {
                var acayr = _YearlyFeeGroupMappingContext.AcademicYear.Where(r => r.MI_Id == data.MI_Id && r.Is_Active == true && r.ASMAY_Id == data.ASMAY_Id).ToList();

                var acaorder = acayr.OrderByDescending(r => r.ASMAY_Order).FirstOrDefault().ASMAY_Order;

                var futureacayrid = _YearlyFeeGroupMappingContext.AcademicYear.Where(r => r.MI_Id == data.MI_Id && r.Is_Active == true && r.ASMAY_Order == Convert.ToInt32(acaorder) + 1).ToList();

                switch (data.searchType)
                {
                    case "1":
                        data.alldatathird = (from a in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                                             from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                             from e in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                             where (a.AMST_Id==e.AMST_Id &&a.FMG_Id==b.FMG_Id && a.MI_Id==data.MI_Id && a.ASMAY_Id== futureacayrid[0].ASMAY_Id &&        e.AMST_AdmNo.Contains(data.searchtext))
                                             select new FeeStudentGroupMappingDTO
                                             {
                                                 AMST_Id = a.AMST_Id,
                                                 AMST_FirstName = e.AMST_FirstName,
                                                 AMST_MiddleName = e.AMST_MiddleName,
                                                 AMST_LastName = e.AMST_LastName,
                                                 AMST_AdmNo = e.AMST_AdmNo,
                                                 AMST_RegistrationNo = e.AMST_RegistrationNo,
                                                 FMG_GroupName = b.FMG_GroupName,
                                                 AMST_Mobile = e.AMST_MobileNo,
                                                 FMSG_Id = a.FMSG_Id,
                                                 FMG_Id = b.FMG_Id
                                             }
      ).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();
                        break;
                    case "2":
                        data.alldatathird = (from a in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                                             from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                             from e in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                             where (a.AMST_Id == e.AMST_Id && a.FMG_Id == b.FMG_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == futureacayrid[0].ASMAY_Id && e.AMST_FirstName.Contains(data.searchtext))
                                             select new FeeStudentGroupMappingDTO
                                             {
                                                 AMST_Id = a.AMST_Id,
                                                 AMST_FirstName = e.AMST_FirstName,
                                                 AMST_MiddleName = e.AMST_MiddleName,
                                                 AMST_LastName = e.AMST_LastName,
                                                 AMST_AdmNo = e.AMST_AdmNo,
                                                 AMST_RegistrationNo = e.AMST_RegistrationNo,
                                                 FMG_GroupName = b.FMG_GroupName,
                                                 AMST_Mobile = e.AMST_MobileNo,
                                                 FMSG_Id = a.FMSG_Id,
                                                 FMG_Id = b.FMG_Id
                                             }
      ).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();
                        break;
                    case "3":
                        data.alldatathird = (from a in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                                             from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                             from e in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                             where (a.AMST_Id == e.AMST_Id && a.FMG_Id == b.FMG_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == futureacayrid[0].ASMAY_Id && b.FMG_GroupName.Contains(data.searchtext))
                                             select new FeeStudentGroupMappingDTO
                                             {
                                                 AMST_Id = a.AMST_Id,
                                                 AMST_FirstName = e.AMST_FirstName,
                                                 AMST_MiddleName = e.AMST_MiddleName,
                                                 AMST_LastName = e.AMST_LastName,
                                                 AMST_AdmNo = e.AMST_AdmNo,
                                                 AMST_RegistrationNo = e.AMST_RegistrationNo,
                                                 FMG_GroupName = b.FMG_GroupName,
                                                 AMST_Mobile = e.AMST_MobileNo,
                                                 FMSG_Id = a.FMSG_Id,
                                                 FMG_Id = b.FMG_Id
                                             }
       ).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();
                        break;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }

        public FeeStudentGroupMappingDTO Deletedetails(FeeStudentGroupMappingDTO data)
        {
            try
            {
                var acayr = _YearlyFeeGroupMappingContext.AcademicYear.Where(r => r.MI_Id == data.MI_Id && r.Is_Active == true && r.ASMAY_Id == data.ASMAY_Id).ToList();

                var acaorder = acayr.OrderByDescending(r => r.ASMAY_Order).FirstOrDefault().ASMAY_Order;

                var futureacayrid = _YearlyFeeGroupMappingContext.AcademicYear.Where(r => r.MI_Id == data.MI_Id && r.Is_Active == true && r.ASMAY_Order == Convert.ToInt32(acaorder) + 1).ToList();

                var outputval = _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("DeleteFutureStudentFeeGroupMapping @p0,@p1,@p2,@p3,@p4,@p5", data.MI_Id, data.AMST_Id, data.ASMAY_Id, data.FMSG_Id, futureacayrid[0].ASMAY_Id,data.FMG_Id);

                if (outputval >= 1)
                {
                    data.returnval = "true";
                }
                else
                {
                    data.returnval = "false";
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}



