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
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using TimeTableServiceHub.com.vaps.Interfaces;

namespace TimeTableServiceHub.com.vaps.Services
{
    public class TTStaffWiseTTImpl : Interfaces.StaffWiseTTInterface
    {

        public TTContext _ttcontext;
        public DomainModelMsSqlServerContext _db;

        public TTStaffWiseTTImpl(TTContext ttcntx)
        {
            _ttcontext = ttcntx;
        }

        public TTStaffWiseTTDTO getreport(TTStaffWiseTTDTO _category)
        {
            TTStaffWiseTTDTO objpge = Mapper.Map<TTStaffWiseTTDTO>(_category);
            List<TTStaffWiseTTDTO> list = new List<TTStaffWiseTTDTO>();
            List<TTStaffWiseTTDTO> result = new List<TTStaffWiseTTDTO>();
            try
            {
                objpge.periodslst = _ttcontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(objpge.MI_Id)).ToList().ToArray();

                objpge.gridweeks = _ttcontext.TT_Master_DayDMO.Where(t => t.MI_Id.Equals(objpge.MI_Id) && t.TTMD_ActiveFlag == true).ToArray();

                for (int i = 0; i < objpge.staffarray.Count(); i++)
                {
                    var temp_stfid = objpge.staffarray[i].HRME_Id;
                    objpge.Time_Table = (from a in _ttcontext.TT_Master_DayDMO
                                         from b in _ttcontext.TT_Master_PeriodDMO
                                         from c in _ttcontext.School_M_Class
                                         from d in _ttcontext.School_M_Section
                                         from e in _ttcontext.IVRM_School_Master_SubjectsDMO
                                         from f in _ttcontext.HR_Master_Employee_DMO
                                         from g in _ttcontext.TT_Final_GenerationDMO
                                         from h in _ttcontext.TT_Final_Generation_DetailedDMO
                                         from ii in _ttcontext.TTMasterCategoryDMO
                                         where (g.MI_Id == objpge.MI_Id && a.MI_Id == g.MI_Id && b.MI_Id == g.MI_Id && c.MI_Id == g.MI_Id && d.MI_Id == g.MI_Id && e.MI_Id == g.MI_Id && f.MI_Id == g.MI_Id && ii.MI_Id == g.MI_Id && ii.TTMC_Id == g.TTMC_Id && h.TTFG_Id == g.TTFG_Id && c.ASMCL_Id == h.ASMCL_Id && d.ASMS_Id == h.ASMS_Id && e.ISMS_Id == h.ISMS_Id && f.HRME_Id == h.HRME_Id && a.TTMD_Id == h.TTMD_Id && b.TTMP_Id == h.TTMP_Id && h.HRME_Id == temp_stfid && g.ASMAY_Id == objpge.ASMAY_Id)
                                         select new TTStaffWiseTTDTO
                                         {
                                             TTFGD_Id = h.TTFGD_Id,
                                             TTFG_Id = g.TTFG_Id,
                                             ASMCL_Id = h.ASMCL_Id,
                                             ASMS_Id = h.ASMS_Id,
                                             HRME_Id = h.HRME_Id,
                                             ISMS_Id = h.ISMS_Id,
                                             TTMD_Id = h.TTMD_Id,
                                             TTMP_Id = h.TTMP_Id,
                                             TTMC_Id = g.TTMC_Id,
                                             TTMD_DayName = a.TTMD_DayName,
                                             TTMP_PeriodName = b.TTMP_PeriodName,
                                             ASMCL_ClassName = c.ASMCL_ClassName,
                                             ASMC_SectionName = d.ASMC_SectionName,
                                             staffName = f.HRME_EmployeeFirstName +" " + (f.HRME_EmployeeMiddleName == null || f.HRME_EmployeeMiddleName == " " || f.HRME_EmployeeMiddleName == "0" ? " " : f.HRME_EmployeeMiddleName) + " " + (f.HRME_EmployeeLastName == null || f.HRME_EmployeeLastName == " " || f.HRME_EmployeeLastName == "0" ? " " : f.HRME_EmployeeLastName),
                                             ISMS_SubjectName = e.ISMS_SubjectName,
                                             TTMC_CategoryName = ii.TTMC_CategoryName,

                                         }
                                  ).Distinct().OrderBy(t=>t.staffName).ToArray();



                    foreach (TTStaffWiseTTDTO dto in objpge.Time_Table)
                    {
                        list.Add(dto);
                    }
                }
                objpge.TT = list.ToArray();


                using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_getStaffwithteachclssub";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.Int) { Value = _category.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.Int) { Value = _category.ASMAY_Id });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new TTStaffWiseTTDTO
                                {
                                    Names = dataReader["Names"].ToString(),                                 
                                    HRME_Id =Convert.ToInt64(dataReader["HRME_Id"].ToString()),
                                });
                                objpge.staffnameswithclas = result.ToArray();
                            }
                            if (dataReader.NextResult())
                            {
                                while (dataReader.Read())
                                {
                                    result.Add(new TTStaffWiseTTDTO
                                    {
                                        Names = dataReader["subjectname"].ToString(),
                                        HRME_Id = Convert.ToInt64(dataReader["HRME_Id"].ToString()),
                                    });
                                    objpge.nameswithclas = result.ToArray();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return _category;
        }
        public TTStaffWiseTTDTO getdetails(int id)
        {
            TTStaffWiseTTDTO data = new TTStaffWiseTTDTO();
            try
            {
                data.acayear = _ttcontext.AcademicYear.AsNoTracking().Where(t => t.MI_Id == id && t.Is_Active == true).ToList().Distinct().OrderByDescending(rr=>rr.ASMAY_Order).ToArray();
                data.categorylist = _ttcontext.TTMasterCategoryDMO.AsNoTracking().Where(t => t.MI_Id == id && t.TTMC_ActiveFlag == true).ToList().Distinct().ToArray();
                data.stafflist = (from b in _ttcontext.HR_Master_Employee_DMO
                                  from a in _ttcontext.TT_Master_Staff_AbbreviationDMO
                                  from c in _ttcontext.TT_Final_Generation_DetailedDMO
                                  from d in _ttcontext.TT_Final_GenerationDMO
                                  where (b.MI_Id.Equals(id) && b.HRME_ActiveFlag.Equals(true) && a.HRME_Id == b.HRME_Id && a.TTMSAB_ActiveFlag == true && c.HRME_Id == a.HRME_Id && c.TTFG_Id == d.TTFG_Id && d.TTFG_ActiveFlag == true)
                                  select new TTStaffWiseTTDTO
                                  {
                                      HRME_Id = b.HRME_Id,
                                      staffName = b.HRME_EmployeeFirstName+" " + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == " " || b.HRME_EmployeeMiddleName == "0" ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == " " || b.HRME_EmployeeLastName == "0" ? " " : b.HRME_EmployeeLastName),
                                  }).Distinct().OrderBy(i=>i.staffName).ToArray();
                data.classlist = _ttcontext.School_M_Class.Where(t => t.MI_Id == id && t.ASMCL_ActiveFlag == true).ToList().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }



    }
}
