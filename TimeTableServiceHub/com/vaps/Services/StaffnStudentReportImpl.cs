using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTableServiceHub.com.vaps.Interfaces;

namespace TimeTableServiceHub.com.vaps.Services
{
    public class StaffnStudentReportImpl : Interfaces.StaffnStudentReportInterface
    {
        public TTContext _ttcontext;
        public DomainModelMsSqlServerContext _db;

        public StaffnStudentReportImpl(TTContext ttcntx)
        {
            _ttcontext = ttcntx;
        }

        public TT_StaffnStudentReportDTO getdetails(int id)
           {
            TT_StaffnStudentReportDTO TTMB = new TT_StaffnStudentReportDTO();
            try
            {
               

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return TTMB;

        }
        public TT_StaffnStudentReportDTO getreport(TT_StaffnStudentReportDTO org)
        {
            TT_StaffnStudentReportDTO TTMB = new TT_StaffnStudentReportDTO();
            try
            {
                if (org.Status == "A")
                {
                    TTMB.lista = (from a in _ttcontext.TT_Fixing_DayDMO
                                  from b in _ttcontext.TT_Fixing_PeriodDMO
                                  from c in _ttcontext.School_M_Class
                                  from d in _ttcontext.HR_Master_Employee_DMO
                                  from e in _ttcontext.TT_Final_Generation_DetailedDMO
                                  from f in _ttcontext.TT_Master_Staff_AbbreviationDMO
                                  where (e.TTMD_Id == a.TTMD_Id && e.TTMP_Id == b.TTMP_Id && e.ASMCL_Id == c.ASMCL_Id && e.HRME_Id == d.HRME_Id && e.HRME_Id == f.HRME_Id)
                                  select new TT_StaffnStudentReportDTO
                                  {

                                      TTMD_Id = a.TTMD_Id,
                                      HRME_Id = d.HRME_Id,
                                      TTMP_Id = b.TTMP_Id,
                                      staffName = d.HRME_EmployeeFirstName + " " + (d.HRME_EmployeeMiddleName == null || d.HRME_EmployeeMiddleName == " " ||d.HRME_EmployeeMiddleName == "0" ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null || d.HRME_EmployeeLastName == " " || d.HRME_EmployeeLastName == "0" ? " " : d.HRME_EmployeeLastName),
                                      TTMSAB_Abbreviation = f.TTMSAB_Abbreviation,
                                  }).GroupBy(y => y.HRME_Id).ToArray();
                }
                else if(org.Status == "B")
                {

                }
                else if (org.Status == "C")
                {

                }
                else if (org.Status == "D")
                {

                }
                else if (org.Status == "E")
                {

                }
                else if (org.Status == "F")
                {

                }
                else if (org.Status == "G")
                {

                }
                else if (org.Status == "H")
                {

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return TTMB;

        }

    }
}
