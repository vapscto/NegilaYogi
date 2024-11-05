
using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.HRMS;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.HRMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.HRMS.Services
{
    public class HrmsConsolidatedReportImpl : Interface.HrmsConsolidatedReportInterface
    {
        public NaacHRMSContext _context;
        public HRMSContext _HRMSContext;

        public HrmsConsolidatedReportImpl(NaacHRMSContext context, HRMSContext HRMSContext)
        {
            _context = context;
            _HRMSContext = HRMSContext;
        }
        public HRMS_NAAC_DTO getdetails(HRMS_NAAC_DTO data)
        {
            try
            {
                data = GetAllDropdownAndDatatableDetails(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public HRMS_NAAC_DTO GetAllDropdownAndDatatableDetails(HRMS_NAAC_DTO dto)
        {
            List<HR_Master_GroupType> GroupTypelist = new List<HR_Master_GroupType>();
            List<HR_Master_Department> Departmentlist = new List<HR_Master_Department>();
            List<HR_Master_Designation> Designationlist = new List<HR_Master_Designation>();
            try
            {
                //GroupTypelist
                GroupTypelist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                dto.groupTypedropdown = GroupTypelist.ToArray();

                //Departmentlist
                Departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                dto.departmentdropdown = Departmentlist.ToArray();

                //Designationlist
                Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                dto.designationdropdown = Designationlist.ToArray();

                dto.leaveyear = _HRMSContext.HR_MasterLeaveYear.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMLY_ActiveFlag == true).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

        public HRMS_NAAC_DTO get_depts(HRMS_NAAC_DTO data)
        {
            try
            {
                data.departmentdropdown = (from a in _HRMSContext.MasterEmployee
                                           from b in _HRMSContext.HR_Master_Department
                                           where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag && a.HRMD_Id == b.HRMD_Id && data.hrmgT_IdList.Contains(a.HRMGT_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMD_ActiveFlag == true)
                                           select b).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public HRMS_NAAC_DTO get_desig(HRMS_NAAC_DTO data)
        {
            try
            {
                data.designationdropdown = (from a in _HRMSContext.MasterEmployee
                                            from b in _HRMSContext.HR_Master_Designation
                                            where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag && a.HRMDES_Id == b.HRMDES_Id && data.hrmgT_IdList.Contains(a.HRMGT_Id) && data.hrmD_IdList.Contains(a.HRMD_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
                                            select b).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public HRMS_NAAC_DTO get_Employe_ob(HRMS_NAAC_DTO data)
        {
            List<long> selected_typ = new List<long>();

            foreach (var itm in data.emptypes)
            {
                selected_typ.Add(itm.HRMGT_Id);
            }
            List<long> selected_dep = new List<long>();

            foreach (var itm in data.empdept)
            {
                selected_dep.Add(itm.HRMD_Id);
            }
            List<long> selected_des = new List<long>();

            foreach (var itm in data.empdesg)
            {
                selected_des.Add(itm.HRMDES_Id);
            }

            data.get_emp = (from a in _HRMSContext.MasterEmployee
                            from b in _HRMSContext.HR_Master_Designation
                            from c in _HRMSContext.HR_Master_Department
                            from d in _HRMSContext.HR_Master_GroupType
                            where (a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id == c.HRMD_Id && c.HRMD_ActiveFlag == true
                            && b.HRMDES_ActiveFlag == true && a.HRME_ActiveFlag == true && a.HRMGT_Id == d.HRMGT_Id && d.HRMGT_ActiveFlag == true
                             && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && a.MI_Id == d.MI_Id && selected_typ.Contains(d.HRMGT_Id) && selected_dep.Contains(c.HRMD_Id) && selected_des.Contains(b.HRMDES_Id))
                            select new HRMS_NAAC_DTO
                            {
                                HRME_Id = a.HRME_Id,
                                HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim()
                            }
                     ).Distinct().ToArray();

            return data;
        }

        public async Task<HRMS_NAAC_DTO> getEmployeReportAsync(HRMS_NAAC_DTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NaacHRMSConsolidatedReport";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMLY_Id", SqlDbType.BigInt)
                    {
                        Value = dto.HRMLY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@dataoption", SqlDbType.VarChar)
                    {
                        Value = dto.dataoption
                    });
                    cmd.Parameters.Add(new SqlParameter("@EmployeeId", SqlDbType.VarChar)
                    {
                        Value = dto.selectedEmployee
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.employeereport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error";
            }
            return dto;
        }

        public HRMS_NAAC_DTO get_EmployeALLDATA(HRMS_NAAC_DTO dto)
        {
            try
            {
                if (dto.HRME_Id > 0)
                {
                    dto.NAACPersonalDeatilsDTO = (from a in _HRMSContext.MasterEmployee
                                                  from b in _HRMSContext.Master_Employee_Qulaification
                                                  from c in _HRMSContext.HR_Master_Department
                                                  where (a.MI_Id == dto.MI_Id && a.HRME_ActiveFlag == true && a.HRME_Id == dto.HRME_Id && a.HRME_Id == b.HRME_Id && a.HRMD_Id == c.HRMD_Id)
                                                  select new NAACPersonalDeatilsDTO
                                                  {
                                                      HRME_Id = a.HRME_Id,
                                                      HRMD_Id = a.HRMD_Id,
                                                      HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                                      HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                                      HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                                                      HRMD_DepartmentName = c.HRMD_DepartmentName,
                                                      HRME_DOJ = a.HRME_DOJ,
                                                      HRME_QualificationName = b.HRME_QualificationName
                                                  }).ToArray();

                    dto.internatiolnalcount = _context.HR_Employee_JournalDMO.Where(a => a.MI_Id == dto.MI_Id && a.HRME_Id == dto.HRME_Id && a.HREJORNL_ActiveFlg == true && a.HREJORNL_NatOrIntNatFlg == "International").ToArray().Count();
                    dto.nationalcount = _context.HR_Employee_JournalDMO.Where(a => a.MI_Id == dto.MI_Id && a.HRME_Id == dto.HRME_Id && a.HREJORNL_ActiveFlg == true && a.HREJORNL_NatOrIntNatFlg == "National").ToArray().Count();
                    dto.nonrefjoucount = _context.HR_Employee_JournalDMO.Where(a => a.MI_Id == dto.MI_Id && a.HRME_Id == dto.HRME_Id && a.HREJORNL_ActiveFlg == true && a.HREJORNL_RefOrNonRefFlg == "Non-Refereed").ToArray().Count();
                    //dto.patentcount = _context.HR_Employee_JournalDMO.Where(a => a.MI_Id == dto.MI_Id && a.HRME_Id == dto.HRME_Id && a.HREJORNL_ActiveFlg == true && a.HREJORNL_RefOrNonRefFlg == "Non-Refereed").ToArray().Count();
                    dto.bookcount = _context.HR_Employee_BookDMO.Where(a => a.MI_Id == dto.MI_Id && a.HRME_Id == dto.HRME_Id && a.HREBK_ActiveFlg == true).ToArray().Count();
                    dto.bookchaptercount = _context.HR_Employee_BookChapterDMO.Where(a => a.MI_Id == dto.MI_Id && a.HRME_Id == dto.HRME_Id && a.HREBKCP_ActiveFlg == true).ToArray().Count();
                    //dto.citationcount = _context.HR_Employee_BookDMO.Where(a => a.MI_Id == dto.MI_Id && a.HRME_Id == dto.HRME_Id && a.HREBK_ActiveFlg == true).ToArray().Count();


                    dto.orientlist = (from a in _context.HR_Employee_OrientationCourseDMO
                                      where (a.MI_Id == dto.MI_Id && a.HREORCO_ActiveFlg == true && a.HRME_Id == dto.HRME_Id)
                                      select a).ToArray();

                    dto.StudentActivitylist = (from a in _context.HR_Employee_StudentActivitiesDMO
                                               where (a.MI_Id == dto.MI_Id && a.HRESACT_ActiveFlg == true && a.HRME_Id == dto.HRME_Id)
                                               select a).ToArray();

                    dto.ProfessionalActivitylist = (from a in _context.HR_Employee_DevActivitiesDMO
                                                    where (a.MI_Id == dto.MI_Id && a.HREDACT_ActiveFlg == true && a.HRME_Id == dto.HRME_Id)
                                                    select a).ToArray();

                    dto.ResearchProjectlist = (from a in _context.HR_Employee_ResearchProjectsDMO
                                               where (a.MI_Id == dto.MI_Id && a.HREREPR_ActiveFlg == true && a.HRME_Id == dto.HRME_Id)
                                               select a).ToArray();

                    dto.ResearchGuidelist = (from a in _context.HR_Employee_ResearchGuidanceDMO
                                             where (a.MI_Id == dto.MI_Id && a.HREREGU_ActiveFlg == true && a.HRME_Id == dto.HRME_Id)
                                             select a).ToArray();

                    dto.BOSBOElist = (from a in _context.HR_Employee_BOSBOEDMO
                                      where (a.MI_Id == dto.MI_Id && a.HREBOS_ActiveFlg == true && a.HRME_Id == dto.HRME_Id)
                                      select a).ToArray();

                    dto.RefJournallist = (from a in _context.HR_Employee_JournalDMO
                                          where (a.MI_Id == dto.MI_Id && a.HREJORNL_ActiveFlg == true && a.HRME_Id == dto.HRME_Id && a.HREJORNL_RefOrNonRefFlg == "Refereed")
                                          select a).ToArray();

                    dto.NonRefJournallist = (from a in _context.HR_Employee_JournalDMO
                                             where (a.MI_Id == dto.MI_Id && a.HREJORNL_ActiveFlg == true && a.HRME_Id == dto.HRME_Id && a.HREJORNL_RefOrNonRefFlg == "Non-Refereed")
                                             select a).ToArray();

                    dto.Conferencelist = (from a in _context.HR_Employee_ConferenceDMO
                                          where (a.MI_Id == dto.MI_Id && a.HRECONF_ActiveFlg == true && a.HRME_Id == dto.HRME_Id)
                                          select a).ToArray();

                    dto.Booklist = (from a in _context.HR_Employee_BookDMO
                                    where (a.MI_Id == dto.MI_Id && a.HREBK_ActiveFlg == true && a.HRME_Id == dto.HRME_Id)
                                    select a).ToArray();

                    dto.BookChapterlist = (from a in _context.HR_Employee_BookChapterDMO
                                           where (a.MI_Id == dto.MI_Id && a.HREBKCP_ActiveFlg == true && a.HRME_Id == dto.HRME_Id)
                                           select a).ToArray();

                    dto.Commetteelist = (from a in _context.HR_Employee_CommitteeDMO
                                         where (a.MI_Id == dto.MI_Id && a.HRECOM_ActiveFlg == true && a.HRME_Id == dto.HRME_Id)
                                         select a).ToArray();

                    dto.OtherDetailSlist = (from a in _context.HR_Employee_OtherDetailsDMO
                                            where (a.MI_Id == dto.MI_Id && a.HREOTHDET_ActiveFlg == true && a.HRME_Id == dto.HRME_Id)
                                            select a).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
    }
}
