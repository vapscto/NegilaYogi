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
    public class StudentFeeGroupMappingGroupDeletionImpl : interfaces.StudentFeeGroupMappingGroupDeletionInterface
    {
        private static ConcurrentDictionary<string, FeeStudentGroupMappingDTO> _login =
          new ConcurrentDictionary<string, FeeStudentGroupMappingDTO>();

        public FeeGroupContext _YearlyFeeGroupMappingContext;
        readonly ILogger<StudentFeeGroupMappingGroupDeletionImpl> _logger;

        public StudentFeeGroupMappingGroupDeletionImpl(FeeGroupContext YearlyFeeGroupMappingContext, ILogger<StudentFeeGroupMappingGroupDeletionImpl> log)
        {
            _YearlyFeeGroupMappingContext = YearlyFeeGroupMappingContext;
            _logger = log;
        }
        public FeeStudentGroupMappingDTO deleterecord(FeeStudentGroupMappingDTO data)
        {
            using (var transaction = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
            {
                var outputval = 0;
                try
                {
                    for (int i = 0; i < data.studentdata.Length; i++)
                    {
                        // outputval = _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("DeleteStudentFeeGroupMapping @p0,@p1,@p2,@p3,@p4", data.MI_Id, data.AMST_Id, data.ASMAY_Id, data.FMG_Id, data.FMSG_Id);
                        outputval = _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("DeleteStudentFeeGroupMapping @p0,@p1,@p2,@p3,@p4", data.MI_Id, data.studentdata[i].AMST_Id, data.ASMAY_Id, data.studentdata[i].FMG_Id, data.studentdata[i].FMSG_Id);
                    }
                    if (outputval >= 1)
                    {
                        transaction.Commit();
                        data.returnval = "true";
                    }
                    else
                    {
                        data.returnval = "false";
                    }

                    data.alldatathird = (from a in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                                         from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                         from c in _YearlyFeeGroupMappingContext.School_M_Class
                                         from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                         from e in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                         from f in _YearlyFeeGroupMappingContext.AcademicYear
                                         where (f.ASMAY_Id == d.ASMAY_Id && a.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && a.MI_Id == b.MI_Id && a.AMST_Id == d.AMST_Id && a.FMG_Id == b.FMG_Id && d.ASMCL_Id == c.ASMCL_Id && e.AMST_Id == d.AMST_Id && e.AMST_SOL == "S")
                                         select new FeeStudentGroupMappingDTO
                                         {
                                             AMST_Id = a.AMST_Id,
                                             AMST_FirstName = e.AMST_FirstName,
                                             AMST_MiddleName = e.AMST_MiddleName,
                                             AMST_LastName = e.AMST_LastName,
                                             AMST_AdmNo = e.AMST_AdmNo,
                                             AMST_RegistrationNo = e.AMST_RegistrationNo,
                                             AMAY_RollNo = d.AMAY_RollNo,
                                             FMG_GroupName = b.FMG_GroupName,
                                             ASMCL_ClassName = c.ASMCL_ClassName,
                                             FMSG_Id = a.FMSG_Id,
                                             FMG_Id = b.FMG_Id
                                         }
          ).ToArray();
                }

                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                    _logger.LogError(ee.Message);
                }

                return data;
            }
        }
        public FeeStudentGroupMappingDTO getdata(FeeStudentGroupMappingDTO fee)
        {

            try
            {
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
                                        where (a.FMG_Id == b.FMG_Id && a.FMH_Id == f.FMH_Id && a.FMG_Id == f.FMG_ID && a.MI_Id == fee.MI_Id && b.FMG_ActiceFlag == true
                                        && a.FYGHM_ActiveFlag == "1" && g.FMH_Id == a.FMH_Id && a.FMG_Id == g.FMG_Id 
                                        && a.ASMAY_Id == fee.ASMAY_Id && a.ASMAY_Id == g.ASMAY_Id)
                                        select new FeeStudentGroupMappingDTO
                                        {
                                            FMG_Id = a.FMG_Id,
                                            FMG_GroupName = b.FMG_GroupName
                                        }).Distinct().ToArray();

                fee.configsetting = _YearlyFeeGroupMappingContext.feemastersettings.Where(s => s.MI_Id == fee.MI_Id && s.userid == fee.user_id).ToList().Distinct().ToArray();



            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return fee;

        }
        public FeeStudentGroupMappingDTO Getreport(FeeStudentGroupMappingDTO data)
        {

            try
            {

                if (data.flag == 1)
                {
                    data.alldatathird = (from a in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                                         from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                         from c in _YearlyFeeGroupMappingContext.School_M_Class
                                         from g in _YearlyFeeGroupMappingContext.school_M_Section
                                         from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                         from e in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                         from f in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                         where (d.ASMS_Id == g.ASMS_Id && a.ASMAY_Id == d.ASMAY_Id && a.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && a.MI_Id == b.MI_Id && a.AMST_Id == d.AMST_Id && a.FMG_Id == b.FMG_Id && d.ASMCL_Id == c.ASMCL_Id && e.AMST_Id == d.AMST_Id && e.AMST_SOL == "S" && d.AMST_Id == f.AMST_Id && e.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1
                                         && a.FMG_Id == data.FMG_Id && data.ASMS_Ids.Contains(g.ASMS_Id) && data.ASMCL_Ids.Contains(c.ASMCL_Id))
                                         select new FeeStudentGroupMappingDTO
                                         {
                                             AMST_Id = a.AMST_Id,
                                             AMST_FirstName = e.AMST_FirstName,
                                             AMST_MiddleName = e.AMST_MiddleName,
                                             AMST_LastName = e.AMST_LastName,
                                             AMST_AdmNo = e.AMST_AdmNo,
                                             AMST_RegistrationNo = e.AMST_RegistrationNo,
                                             AMAY_RollNo = d.AMAY_RollNo,
                                             FMG_GroupName = b.FMG_GroupName,
                                             ASMCL_ClassName = c.ASMCL_ClassName,
                                             ASMC_SectionName = g.ASMC_SectionName,
                                             AMST_Mobile = e.AMST_MobileNo,
                                             FMSG_Id = a.FMSG_Id,
                                             FMG_Id = b.FMG_Id
                                         }
          ).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();
                }
                if (data.flag==2)
                {
                    data.alldatathird = (from a in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                                         from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                         from c in _YearlyFeeGroupMappingContext.School_M_Class
                                         from g in _YearlyFeeGroupMappingContext.school_M_Section
                                         from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                         from e in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                         from f in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                         where (d.ASMS_Id == g.ASMS_Id && a.ASMAY_Id == d.ASMAY_Id && a.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && a.MI_Id == b.MI_Id && a.AMST_Id == d.AMST_Id && a.FMG_Id == b.FMG_Id && d.ASMCL_Id == c.ASMCL_Id && e.AMST_Id == d.AMST_Id && e.AMST_SOL == "S" && d.AMST_Id == f.AMST_Id && e.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1
                                         && a.FMG_Id == data.FMG_Id && data.ASMS_Ids.Contains(g.ASMS_Id) && c.ASMCL_Id == data.ASMCL_Id)
                                         select new FeeStudentGroupMappingDTO
                                         {
                                             AMST_Id = a.AMST_Id,
                                             AMST_FirstName = e.AMST_FirstName,
                                             AMST_MiddleName = e.AMST_MiddleName,
                                             AMST_LastName = e.AMST_LastName,
                                             AMST_AdmNo = e.AMST_AdmNo,
                                             AMST_RegistrationNo = e.AMST_RegistrationNo,
                                             AMAY_RollNo = d.AMAY_RollNo,
                                             FMG_GroupName = b.FMG_GroupName,
                                             ASMCL_ClassName = c.ASMCL_ClassName,
                                             ASMC_SectionName = g.ASMC_SectionName,
                                             AMST_Mobile = e.AMST_MobileNo,
                                             FMSG_Id = a.FMSG_Id,
                                             FMG_Id = b.FMG_Id
                                         }
          ).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();
                }
         
                if(data.flag==3)
                {
                    data.alldatathird = (from a in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                                         from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                         from c in _YearlyFeeGroupMappingContext.School_M_Class
                                         from g in _YearlyFeeGroupMappingContext.school_M_Section
                                         from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                         from e in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                         from f in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                         where (d.ASMS_Id == g.ASMS_Id && a.ASMAY_Id == d.ASMAY_Id && a.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && a.MI_Id == b.MI_Id && a.AMST_Id == d.AMST_Id && a.FMG_Id == b.FMG_Id && d.ASMCL_Id == c.ASMCL_Id && e.AMST_Id == d.AMST_Id && e.AMST_SOL == "S" && d.AMST_Id == f.AMST_Id && e.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1
                                         && a.FMG_Id == data.FMG_Id && g.ASMS_Id == data.ASMS_Id && data.ASMCL_Ids.Contains(c.ASMCL_Id))
                                         select new FeeStudentGroupMappingDTO
                                         {
                                             AMST_Id = a.AMST_Id,
                                             AMST_FirstName = e.AMST_FirstName,
                                             AMST_MiddleName = e.AMST_MiddleName,
                                             AMST_LastName = e.AMST_LastName,
                                             AMST_AdmNo = e.AMST_AdmNo,
                                             AMST_RegistrationNo = e.AMST_RegistrationNo,
                                             AMAY_RollNo = d.AMAY_RollNo,
                                             FMG_GroupName = b.FMG_GroupName,
                                             ASMCL_ClassName = c.ASMCL_ClassName,
                                             ASMC_SectionName = g.ASMC_SectionName,
                                             AMST_Mobile = e.AMST_MobileNo,
                                             FMSG_Id = a.FMSG_Id,
                                             FMG_Id = b.FMG_Id
                                         }
          ).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();
                }
                if (data.flag == 4)
                {
                    data.alldatathird = (from a in _YearlyFeeGroupMappingContext.FeeStudentGroupMappingDMO
                                         from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                         from c in _YearlyFeeGroupMappingContext.School_M_Class
                                         from g in _YearlyFeeGroupMappingContext.school_M_Section
                                         from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                         from e in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                         from f in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                         where (d.ASMS_Id == g.ASMS_Id && a.ASMAY_Id == d.ASMAY_Id && a.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && a.MI_Id == b.MI_Id && a.AMST_Id == d.AMST_Id && a.FMG_Id == b.FMG_Id && d.ASMCL_Id == c.ASMCL_Id && e.AMST_Id == d.AMST_Id && e.AMST_SOL == "S" && d.AMST_Id == f.AMST_Id && e.AMST_ActiveFlag == 1 && d.AMAY_ActiveFlag == 1
                                         && a.FMG_Id == data.FMG_Id && g.ASMS_Id == data.ASMS_Id && c.ASMCL_Id == data.ASMCL_Id)
                                         select new FeeStudentGroupMappingDTO
                                         {
                                             AMST_Id = a.AMST_Id,
                                             AMST_FirstName = e.AMST_FirstName,
                                             AMST_MiddleName = e.AMST_MiddleName,
                                             AMST_LastName = e.AMST_LastName,
                                             AMST_AdmNo = e.AMST_AdmNo,
                                             AMST_RegistrationNo = e.AMST_RegistrationNo,
                                             AMAY_RollNo = d.AMAY_RollNo,
                                             FMG_GroupName = b.FMG_GroupName,
                                             ASMCL_ClassName = c.ASMCL_ClassName,
                                             ASMC_SectionName = g.ASMC_SectionName,
                                             AMST_Mobile = e.AMST_MobileNo,
                                             FMSG_Id = a.FMSG_Id,
                                             FMG_Id = b.FMG_Id
                                         }
          ).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();
                }

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                _logger.LogError(ee.Message);
            }

            return data;

        }
        public FeeStudentGroupMappingDTO onclickClass(FeeStudentGroupMappingDTO data)
        {


            try
            {
                data.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO
                                       from b in _YearlyFeeGroupMappingContext.feeGroup
                                       from f in _YearlyFeeGroupMappingContext.FEeGroupLoginPreviledgeDMO
                                       from g in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                       where (a.FMG_Id == b.FMG_Id && a.FMH_Id == f.FMH_Id && a.FMG_Id == f.FMG_ID && a.MI_Id == data.MI_Id && b.FMG_ActiceFlag == true 
                                       && a.FYGHM_ActiveFlag == "1" && g.FMH_Id == a.FMH_Id && a.FMG_Id == g.FMG_Id 
                                       && a.ASMAY_Id == data.ASMAY_Id && a.ASMAY_Id == g.ASMAY_Id)
                                       select new FeeStudentGroupMappingDTO
                                       {
                                           FMG_Id = a.FMG_Id,
                                           FMG_GroupName = b.FMG_GroupName
                                       }).Distinct().ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                _logger.LogError(ee.Message);
            }

            return data;

        }



    }
}
