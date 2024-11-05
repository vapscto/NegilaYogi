using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public class PointsReportImpl : Interfaces.PointsReportInterface
    {
        public ScheduleReportContext _SReportContext;
        public DomainModelMsSqlServerContext _SSReportContext;
        public StudentApplicationContext _StudentApplicationContext;

        public PointsReportImpl(StudentApplicationContext StudentApplicationContext, ScheduleReportContext DomainModelContext, DomainModelMsSqlServerContext DomainModelContext1)
        {
            _StudentApplicationContext = StudentApplicationContext;
            _SReportContext = DomainModelContext;
            _SSReportContext = DomainModelContext1;
        }

        public PointsReportDTO getdetails(PointsReportDTO data)
        {
            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _SReportContext.AcademicYear.Where(t => t.MI_Id == data.mid && t.Is_Active == true).OrderByDescending(d=>d.ASMAY_Order).ToList();
                data.yeardropDown = year.ToArray();

                List<School_M_Class> classname = new List<School_M_Class>();
                classname = _SReportContext.admissioncls.Where(t => t.MI_Id == data.mid && t.ASMCL_ActiveFlag == true).OrderBy(d=>d.ASMCL_Order).ToList();
                data.fillclass = classname.ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<PointsReportDTO> Getreportdetails(PointsReportDTO data)
        {
            List<PointsReportDTO> result = new List<PointsReportDTO>();
            try
            {
                data.studentDetails = (from a in _StudentApplicationContext.Enq
                                       from b in _StudentApplicationContext.PointsDMO                                 
                                       where (a.pasr_id == b.PASR_Id  && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id==data.ASMCL_Id)
                                       select new PointsReportDTO
                                       {
                                           PASR_Id = a.pasr_id,
                                           PASR_FirstName = a.PASR_FirstName,
                                           PASR_MiddleName = a.PASR_MiddleName,
                                           PASR_LastName = a.PASR_LastName,
                                           PASR_EmailID=a.PASR_emailId,
                                           PASR_Age = a.PASR_Age,                                                                               
                                           PASAP_AGE = b.PASAP_AGE,
                                           PASAP_INCOME = b.PASAP_INCOME,
                                           PASAP_CASTE = b.PASAP_CASTE,
                                           PASAP_ADRESS = b.PASAP_ADRESS,
                                           PASAP_QA = b.PASAP_QA,
                                           PASAP_TOTAL = b.PASAP_TOTAL,
                                           PASRAPS_ID=a.PASRAPS_ID
                                       }
                   ).OrderByDescending(x => x.PASAP_TOTAL).ToList().ToArray();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return data;

        }
    }
}
