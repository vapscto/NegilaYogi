using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Sports;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Sports;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Services
{
    public class HouseInchargeImpl : Interfaces.HouseInchargeInterface
    {
        DomainModelMsSqlServerContext _db;
        SportsContext _sportcontext;

        public HouseInchargeImpl(SportsContext spc, DomainModelMsSqlServerContext contxt)
        {
            _sportcontext = spc;
            _db = contxt;
        }

        public SPCC_Master_House_Staff_DTO Getdetails(SPCC_Master_House_Staff_DTO data)
        {
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _sportcontext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToList();
                data.yearlist = list.ToArray();


                data.filldepartment = _sportcontext.HR_Master_Department.Where(a => a.MI_Id == data.MI_Id && a.HRMD_ActiveFlag == true).Distinct().OrderBy(e => e.HRMD_Order).ToArray();

                #region Staff Filteration
                //var stfdata = _sportcontext.SPCC_Master_House_Staff_DMO.Where(t => t.MI_Id == data.MI_Id).ToList();
                //if (stfdata.Count > 0)
                //{
                //    var hrmeid = stfdata.Select(f => f.HRME_Id);
                //    data.emplist = (from a in _sportcontext.MasterEmployee
                //                    from b in _sportcontext.SPCC_Master_House_Staff_DMO
                //                    where (a.MI_Id==b.MI_Id && !hrmeid.Contains(a.HRME_Id) && a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                //                    select new SPCC_Master_House_Staff_DTO
                //                    {
                //                        empname = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                //                        HRME_Id = a.HRME_Id,
                //                        HRME_EmployeeOrder = a.HRME_EmployeeOrder,
                //                    }).Distinct().OrderBy(a => a.HRME_EmployeeOrder).ToArray();
                //}
                //else
                //{
                //    data.emplist = (from a in _sportcontext.MasterEmployee
                //                    where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                //                    select new SPCC_Master_House_Staff_DTO
                //                    {
                //                        empname = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                //                        HRME_Id = a.HRME_Id,
                //                        HRME_EmployeeOrder = a.HRME_EmployeeOrder,
                //                    }).Distinct().OrderBy(a => a.HRME_EmployeeOrder).ToArray();
                //}
                #endregion  Staff Filteration


                data.alldata = (from a in _sportcontext.SPCC_Master_House_Staff_DMO
                                from b in _sportcontext.SportStudentHouseDivisionDMO
                                from c in _sportcontext.SportMasterHouseDMO
                                from d in _sportcontext.AcademicYear
                                from e in _sportcontext.MasterEmployee
                                    //from m in _sportcontext.Emp_MobileNo
                                from dp in _sportcontext.HR_Master_Department
                                from des in _sportcontext.HR_Master_Designation

                                where (a.MI_Id == b.MI_Id && a.SPCCMH_Id == b.SPCCMH_Id && b.SPCCMH_Id == c.SPCCMH_Id && a.HRME_Id == e.HRME_Id && b.ASMAY_Id == a.ASMAY_Id && a.ASMAY_Id == d.ASMAY_Id && e.HRMD_Id == dp.HRMD_Id && e.HRMDES_Id == des.HRMDES_Id /*&& e.HRME_Id == m.HRME_Id && m.HRMEMNO_DeFaultFlag == "default"*/)
                                select new SPCC_Master_House_Staff_DTO
                                {
                                    SPCCMHS_Id = a.SPCCMHS_Id,
                                    HRME_Id = a.HRME_Id,
                                    SPCCMH_Id = a.SPCCMH_Id,
                                    SPCCMHS_Description = a.SPCCMHS_Description,
                                    SPCCMHS_ActiveFlag = a.SPCCMHS_ActiveFlag,
                                    ASMAY_Id = a.ASMAY_Id,
                                    ASMAY_Year = d.ASMAY_Year,
                                    SPCCMH_HouseName = c.SPCCMH_HouseName,
                                    empname = ((e.HRME_EmployeeFirstName == null ? " " : e.HRME_EmployeeFirstName) + " " + (e.HRME_EmployeeMiddleName == null ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null ? " " : e.HRME_EmployeeLastName)).Trim(),
                                    empcode = e.HRME_EmployeeCode,
                                    mobileNo = (e.HRME_MobileNo).ToString(),
                                    // mobileNo = (m.HRMEMNO_MobileNo).ToString(),
                                    HRMD_DepartmentName = dp.HRMD_DepartmentName,
                                    HRMDES_DesignationName = des.HRMDES_DesignationName,

                                }).Distinct().OrderBy(t => t.HRME_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public SPCC_Master_House_Staff_DTO saverecord(SPCC_Master_House_Staff_DTO data)
        {
            try
            {
                if (data.SPCCMHS_Id > 0)
                {
                    if (data.emplstdata.Length > 0)
                    {
                        var ids = data.emplstdata.Select(a => a.HRME_Id).ToList();
                        foreach (var hrmeids in ids)
                        {
                            var Duplicate = _sportcontext.SPCC_Master_House_Staff_DMO.Where(t => t.MI_Id == data.MI_Id && t.SPCCMHS_Id != data.SPCCMHS_Id && t.SPCCMH_Id == data.SPCCMH_Id && t.HRME_Id == hrmeids && t.ASMAY_Id == data.ASMAY_Id).ToList();


                            if (Duplicate.Count() > 0)
                            {
                                data.dulicate = true;
                            }
                            else
                            {
                                var update = _sportcontext.SPCC_Master_House_Staff_DMO.Where(t => t.SPCCMHS_Id == data.SPCCMHS_Id && t.MI_Id == data.MI_Id).SingleOrDefault();

                                update.SPCCMH_Id = data.SPCCMH_Id;
                                update.HRME_Id = hrmeids;
                                update.SPCCMHS_Description = data.SPCCMHS_Description;

                                update.UpdatedDate = DateTime.Now;

                                _sportcontext.Update(update);
                                var contactExists = _sportcontext.SaveChanges();
                                if (contactExists > 0)
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
                }
                else
                {
                    if (data.emplstdata.Length > 0)
                    {
                        var ids = data.emplstdata.Select(a => a.HRME_Id).ToList();
                        foreach (var hrmeids in ids)
                        {
                            var Duplicate = _sportcontext.SPCC_Master_House_Staff_DMO.Where(t => t.MI_Id == data.MI_Id && t.SPCCMH_Id == data.SPCCMH_Id && t.HRME_Id == hrmeids && t.ASMAY_Id == data.ASMAY_Id).ToList();

                            if (Duplicate.Count() > 0)
                            {
                                data.dulicate = true;
                            }
                            else
                            {
                                SPCC_Master_House_Staff_DMO obj = new SPCC_Master_House_Staff_DMO();

                                obj.SPCCMHS_Id = data.SPCCMHS_Id;
                                obj.MI_Id = data.MI_Id;
                                obj.ASMAY_Id = data.ASMAY_Id;
                                obj.SPCCMH_Id = data.SPCCMH_Id;
                                obj.HRME_Id = hrmeids;
                                obj.SPCCMHS_Description = data.SPCCMHS_Description;
                                obj.SPCCMHS_ActiveFlag = true;
                                obj.CreatedDate = DateTime.Now;
                                obj.UpdatedDate = DateTime.Now;

                                _sportcontext.Add(obj);
                                var contactExists = _sportcontext.SaveChanges();
                                if (contactExists > 0)
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



                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public SPCC_Master_House_Staff_DTO get_House(SPCC_Master_House_Staff_DTO data)
        {
            try
            {
                data.houseList = (from t in _sportcontext.SportMasterHouseDMO
                                  from b in _sportcontext.SportStudentHouseDivisionDMO
                                  where (t.MI_Id == b.MI_Id && t.SPCCMH_Id == b.SPCCMH_Id && t.MI_Id == data.MI_Id && t.SPCCMH_ActiveFlag == true && b.SPCCMH_ActiveFlag == true)
                                  select t
                                 ).Distinct().OrderBy(t => t.SPCCMH_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public SPCC_Master_House_Staff_DTO deactive(SPCC_Master_House_Staff_DTO data)
        {
            try
            {
                var result = _sportcontext.SPCC_Master_House_Staff_DMO.Single(t => t.SPCCMHS_Id == data.SPCCMHS_Id && t.MI_Id == data.MI_Id);

                if (result.SPCCMHS_ActiveFlag == true)
                {
                    result.SPCCMHS_ActiveFlag = false;
                }
                else if (result.SPCCMHS_ActiveFlag == false)
                {
                    result.SPCCMHS_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;
                _sportcontext.Update(result);
                int rowAffected = _sportcontext.SaveChanges();
                if (rowAffected > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public SPCC_Master_House_Staff_DTO editrecord(SPCC_Master_House_Staff_DTO id)
        {
            try
            {



                var datlist = (from a in _sportcontext.SPCC_Master_House_Staff_DMO
                               from b in _sportcontext.SportMasterHouseDMO
                               from c in _sportcontext.SportStudentHouseDivisionDMO
                               from d in _sportcontext.AcademicYear
                               from f in _sportcontext.MasterEmployee

                               where (a.SPCCMH_Id == b.SPCCMH_Id && b.SPCCMH_Id == c.SPCCMH_Id && a.ASMAY_Id == c.ASMAY_Id && c.ASMAY_Id == d.ASMAY_Id
                               && a.HRME_Id == f.HRME_Id && a.MI_Id == id.MI_Id && a.ASMAY_Id == id.ASMAY_Id && a.SPCCMHS_Id == id.SPCCMHS_Id)
                               select new SPCC_Master_House_Staff_DTO
                               {

                                   SPCCMHS_Id = a.SPCCMHS_Id,
                                   SPCCMH_Id = a.SPCCMH_Id,
                                   HRME_Id = a.HRME_Id,
                                   SPCCMHS_Description = a.SPCCMHS_Description,
                                   SPCCMHS_ActiveFlag = a.SPCCMHS_ActiveFlag,
                                   ASMAY_Id = a.ASMAY_Id,
                                   HRMD_Id = f.HRMD_Id,
                                   HRMDES_Id = f.HRMDES_Id,

                               }).Distinct().ToList();
                id.editdata = datlist.ToArray();

                var hrmdid = datlist.Select(a => a.HRMD_Id);
                id.filldesignation = (from a in _sportcontext.MasterEmployee
                                      from b in _sportcontext.HR_Master_Designation
                                      where (a.MI_Id == id.MI_Id && a.HRME_ActiveFlag && a.HRMDES_Id == b.HRMDES_Id && hrmdid.Contains(a.HRMD_Id) && b.MI_Id.Equals(id.MI_Id) && b.HRMDES_ActiveFlag == true)
                                      select b).Distinct().ToArray();


                id.emplist = (from a in _sportcontext.MasterEmployee
                              where (a.MI_Id == id.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && hrmdid.Contains(a.HRMD_Id))
                              select new SPCC_Master_House_Staff_DTO
                              {
                                  empname = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                  HRME_Id = a.HRME_Id,
                                  HRME_EmployeeOrder = a.HRME_EmployeeOrder,
                              }).Distinct().OrderBy(a => a.HRME_EmployeeOrder).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return id;
        }

        public SPCC_Master_House_Staff_DTO get_staff1(SPCC_Master_House_Staff_DTO data)
        {
            try
            {


                var stfdata = _sportcontext.SPCC_Master_House_Staff_DMO.Where(t => t.MI_Id == data.MI_Id).ToList();
                if (stfdata.Count > 0)
                {
                    var hrmeid = stfdata.Select(f => f.HRME_Id);
                    data.emplist = (from a in _sportcontext.MasterEmployee
                                    from b in _sportcontext.SPCC_Master_House_Staff_DMO
                                    where (a.MI_Id == b.MI_Id && !hrmeid.Contains(a.HRME_Id) && a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.HRMDES_Id == data.HRMDES_Id && a.HRMD_Id == data.HRMD_Id)
                                    select new SPCC_Master_House_Staff_DTO
                                    {
                                        empname = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                        HRME_Id = a.HRME_Id,
                                        HRME_EmployeeOrder = a.HRME_EmployeeOrder,
                                    }).Distinct().OrderBy(a => a.HRME_EmployeeOrder).ToArray();
                }
                else
                {
                    data.emplist = (from a in _sportcontext.MasterEmployee
                                    where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.HRMDES_Id == data.HRMDES_Id && a.HRMD_Id == data.HRMD_Id)
                                    select new SPCC_Master_House_Staff_DTO
                                    {
                                        empname = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                        HRME_Id = a.HRME_Id,
                                        HRME_EmployeeOrder = a.HRME_EmployeeOrder,
                                    }).Distinct().OrderBy(a => a.HRME_EmployeeOrder).ToArray();
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;

        }

        public SPCC_Master_House_Staff_DTO getdepchange(SPCC_Master_House_Staff_DTO data)
        {
            try
            {


                data.filldesignation = (from a in _sportcontext.MasterEmployee
                                        from b in _sportcontext.HR_Master_Designation
                                        where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag && a.HRMDES_Id == b.HRMDES_Id && data.HRMD_Id == a.HRMD_Id && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
                                        select b).Distinct().ToArray();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;

        }


    }
}
