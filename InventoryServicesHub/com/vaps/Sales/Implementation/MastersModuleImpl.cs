using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Inventory;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Implementation
{
    public class MastersModuleImpl : Interface.MastersModuleInterface
    {
        public InventoryContext _INVContext;
        public DomainModelMsSqlServerContext _Context;

        public MastersModuleImpl(InventoryContext para, DomainModelMsSqlServerContext para2)
        {
            _INVContext = para;
            _Context = para2;
        }


        public MastersModule_DTO getdetails(MastersModule_DTO dTO)
        {
            try
            {
                List<MasterModules> Allname = new List<MasterModules>();
                Allname = _INVContext.MasterModules.ToList();
                dTO.masterModulesname = Allname.ToArray();

                //dTO.deptlist = _Context.HR_Master_DepartmentCodeDMO.OrderBy(t => t.HRMDC_Order).ToArray();
                dTO.deptlist = _Context.HR_Master_Department.Where(t => t.MI_Id == dTO.MI_Id && t.HRMD_ActiveFlag == true).Distinct().OrderBy(t => t.HRMD_DepartmentName).ToArray();

                dTO.projectlist = _INVContext.MastersProject_DMO.Where(t => t.MI_Id == dTO.MI_Id && t.ISMMPR_ActiveFlg == true).Distinct().OrderBy(t => t.ISMMPR_ProjectName).ToArray();

                dTO.modulelist = (from a in _Context.masterModule
                                  where (a.Module_ActiveFlag == 1)
                                  select new MastersModule_DTO
                                  { IVRMM_Id = a.IVRMM_Id, IVRMM_ModuleName = a.IVRMM_ModuleName, }).Distinct().OrderBy(t => t.IVRMM_ModuleName).ToArray();

                dTO.emplist = (from t in _INVContext.MasterEmployee
                               from a in _INVContext.Staff_User_Login
                               where (t.HRME_Id == a.Emp_Code && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false)
                               select new MastersModule_DTO
                               {
                                   empname = t.HRME_EmployeeFirstName + (string.IsNullOrEmpty(t.HRME_EmployeeMiddleName) ? "" : ' ' + t.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(t.HRME_EmployeeLastName) ? "" : ' ' + t.HRME_EmployeeLastName),
                                   HRME_Id = t.HRME_Id,
                               }).Distinct().OrderBy(t => t.empname).ToArray();


                dTO.emplistHead = (from t in _INVContext.MasterEmployee
                                   from a in _INVContext.Staff_User_Login
                                   where (t.HRME_Id == a.Emp_Code && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false)
                                   select new MastersModule_DTO
                                   {
                                       empname = t.HRME_EmployeeFirstName + (string.IsNullOrEmpty(t.HRME_EmployeeMiddleName) ? "" : ' ' + t.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(t.HRME_EmployeeLastName) ? "" : ' ' + t.HRME_EmployeeLastName),
                                       headId = t.HRME_Id,
                                   }).Distinct().OrderBy(t => t.empname).ToArray();


                dTO.alldata = (from a in _INVContext.MastersModule_DMO
                               from b in _INVContext.HR_Master_Department
                                   //from c in _INVContext.MasterEmployee
                               from d in _INVContext.MasterModules
                               from p in _INVContext.MastersProject_DMO
                               where (a.MI_Id == b.MI_Id && a.HRMD_Id == b.HRMD_Id && d.IVRMM_Id == a.IVRMM_Id && b.HRMD_ActiveFlag == true && a.ISMMPR_Id == p.ISMMPR_Id && a.MI_Id == p.MI_Id && b.MI_Id == p.MI_Id && a.MI_Id == dTO.MI_Id)
                               select new MastersModule_DTO
                               {
                                   HRMD_Id = a.HRMD_Id,
                                   HRME_Id = a.ISMMMD_ModuleHeadId,
                                   ISMMMD_Id = a.ISMMMD_Id,
                                   ISMMPR_Id = a.ISMMPR_Id,
                                   IVRMM_Id = a.IVRMM_Id,
                                   ISMMMD_ActiveFlag = a.ISMMMD_ActiveFlag,
                                   IVRMM_ModuleName = d.IVRMM_ModuleName,
                                   ISMMPR_ProjectName = p.ISMMPR_ProjectName,
                                   HRMD_DepartmentName = b.HRMD_DepartmentName
                               }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dTO;
        }

        public MastersModule_DTO saverecord(MastersModule_DTO data)
        {
            try
            {
                if (data.ISMMMD_Id == 0)
                {

                    var duplicate = _INVContext.MastersModule_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRMD_Id == data.HRMD_Id && t.ISMMPR_Id == data.ISMMPR_Id && t.IVRMM_Id == data.IVRMM_Id && t.ISMMMD_ModuleHeadId == data.developerheadlist.FirstOrDefault().headId).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        MastersModule_DMO obj = new MastersModule_DMO();
                        // obj.ISMMMD_Id = data.ISMMMD_Id;
                        obj.MI_Id = data.MI_Id;
                        obj.HRMD_Id = data.HRMD_Id;
                        obj.ISMMPR_Id = data.ISMMPR_Id;
                        obj.IVRMM_Id = data.IVRMM_Id;
                        obj.ISMMMD_ModuleHeadId = data.developerheadlist.FirstOrDefault().headId;
                        obj.ISMMMD_ActiveFlag = true;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
                        obj.ISMMMD_CreatedBy = data.UserId;
                        obj.ISMMMD_UpdatedBy = data.UserId;

                        _INVContext.Add(obj);

                        for (int i = 0; i < data.developerheadlist.Length; i++)
                        {
                            var tempid = data.developerheadlist[i].headId;
                            ISM_Master_Module_DevelopersDMO obj5 = new ISM_Master_Module_DevelopersDMO();

                            obj5.ISMMMDDE_Id = data.ISMMMDDE_Id;
                            obj5.ISMMMD_Id = obj.ISMMMD_Id;
                            obj5.IVRMMMDDE_ModuleIncharge = tempid;
                            obj5.IVRMMMDDE_ModuleHeadFlg = true;
                            obj5.ISMMMDDE_ActiveFlag = true;
                            obj5.ISMMMDDE_CreatedBy = data.UserId;
                            obj5.ISMMMDDE_UpdatedBy = data.UserId;
                            obj5.CreatedDate = DateTime.Now;
                            obj5.UpdatedDate = DateTime.Now;

                            _INVContext.Add(obj5);
                        }

                        for (int i = 0; i < data.developerlist.Length; i++)
                        {
                            var tempid = data.developerlist[i].HRME_Id;
                            ISM_Master_Module_DevelopersDMO obj2 = new ISM_Master_Module_DevelopersDMO();

                            obj2.ISMMMDDE_Id = data.ISMMMDDE_Id;
                            obj2.ISMMMD_Id = obj.ISMMMD_Id;
                            obj2.IVRMMMDDE_ModuleIncharge = tempid;
                            obj2.IVRMMMDDE_ModuleHeadFlg = false;
                            obj2.ISMMMDDE_ActiveFlag = true;
                            obj2.ISMMMDDE_CreatedBy = data.UserId;
                            obj2.ISMMMDDE_UpdatedBy = data.UserId;
                            obj2.CreatedDate = DateTime.Now;
                            obj2.UpdatedDate = DateTime.Now;

                            _INVContext.Add(obj2);
                        }


                        int a = _INVContext.SaveChanges();
                        if (a > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else
                {
                    // for (int i = 0; i < data.developerlist.Length; i++)
                    // {
                    // var temp_empid = data.developerlist[i].HRME_Id;

                    var duplicate = _INVContext.MastersModule_DMO.Where(t => t.MI_Id == data.MI_Id && t.ISMMMD_Id != data.ISMMMD_Id && t.HRMD_Id == data.HRMD_Id && t.ISMMPR_Id == data.ISMMPR_Id && t.IVRMM_Id == data.IVRMM_Id && t.ISMMMD_ModuleHeadId == data.developerheadlist.FirstOrDefault().headId).ToList();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var obj = _INVContext.MastersModule_DMO.Where(t => t.ISMMMD_Id == data.ISMMMD_Id).Single();

                        obj.HRMD_Id = data.HRMD_Id;
                        obj.ISMMPR_Id = data.ISMMPR_Id;
                        obj.IVRMM_Id = data.IVRMM_Id;
                        obj.ISMMMD_ModuleHeadId = data.developerheadlist.FirstOrDefault().headId;
                        obj.UpdatedDate = DateTime.Now;
                        obj.ISMMMD_UpdatedBy = data.UserId;
                        _INVContext.Update(obj);

                        var count = _INVContext.ISM_Master_Module_DevelopersDMO.Where(t => t.ISMMMD_Id == obj.ISMMMD_Id).ToList();
                        if (count.Count > 0)
                        {
                            foreach (var devlis in count)
                            {
                                _INVContext.Remove(devlis);
                            }
                        }

                        for (int i = 0; i < data.developerheadlist.Length; i++)
                        {
                            var tempid = data.developerheadlist[i].headId;
                            ISM_Master_Module_DevelopersDMO obj5 = new ISM_Master_Module_DevelopersDMO();

                            obj5.ISMMMDDE_Id = data.ISMMMDDE_Id;
                            obj5.ISMMMD_Id = obj.ISMMMD_Id;
                            obj5.IVRMMMDDE_ModuleIncharge = tempid;
                            obj5.IVRMMMDDE_ModuleHeadFlg = true;
                            obj5.ISMMMDDE_ActiveFlag = true;
                            obj5.ISMMMDDE_CreatedBy = data.UserId;
                            obj5.ISMMMDDE_UpdatedBy = data.UserId;
                            obj5.CreatedDate = DateTime.Now;
                            obj5.UpdatedDate = DateTime.Now;

                            _INVContext.Add(obj5);
                        }

                        for (int i = 0; i < data.developerlist.Length; i++)
                        {
                            var tempid = data.developerlist[i].HRME_Id;
                            ISM_Master_Module_DevelopersDMO obj2 = new ISM_Master_Module_DevelopersDMO();

                            obj2.ISMMMDDE_Id = data.ISMMMDDE_Id;
                            obj2.ISMMMD_Id = obj.ISMMMD_Id;
                            obj2.IVRMMMDDE_ModuleIncharge = tempid;
                            obj2.IVRMMMDDE_ModuleHeadFlg = false;
                            obj2.ISMMMDDE_ActiveFlag = true;
                            obj2.ISMMMDDE_CreatedBy = data.UserId;
                            obj2.ISMMMDDE_UpdatedBy = data.UserId;
                            obj2.CreatedDate = DateTime.Now;
                            obj2.UpdatedDate = DateTime.Now;

                            _INVContext.Add(obj2);
                        }

                        int a = _INVContext.SaveChanges();
                        if (a > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }

                }

                //}               

                data.alldata = (from a in _INVContext.MastersModule_DMO
                                from b in _INVContext.HR_Master_Department
                                    //from c in _INVContext.MasterEmployee
                                from d in _INVContext.MasterModules
                                from p in _INVContext.MastersProject_DMO
                                where (a.MI_Id == b.MI_Id && a.HRMD_Id == b.HRMD_Id && d.IVRMM_Id == a.IVRMM_Id && b.HRMD_ActiveFlag == true && a.ISMMPR_Id == p.ISMMPR_Id && a.MI_Id == p.MI_Id && b.MI_Id == p.MI_Id && a.MI_Id == data.MI_Id)
                                select new MastersModule_DTO
                                {
                                    HRMD_Id = a.HRMD_Id,
                                    HRME_Id = a.ISMMMD_ModuleHeadId,
                                    ISMMMD_Id = a.ISMMMD_Id,
                                    ISMMPR_Id = a.ISMMPR_Id,
                                    IVRMM_Id = a.IVRMM_Id,
                                    ISMMMD_ActiveFlag = a.ISMMMD_ActiveFlag,
                                    IVRMM_ModuleName = d.IVRMM_ModuleName,
                                    ISMMPR_ProjectName = p.ISMMPR_ProjectName,
                                    HRMD_DepartmentName = b.HRMD_DepartmentName
                                }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public MastersModule_DTO deactiveY(MastersModule_DTO data)
        {
            try
            {
                var result = _INVContext.MastersModule_DMO.Single(t => t.ISMMMD_Id == data.ISMMMD_Id);
                if (result.ISMMMD_ActiveFlag == true)
                {
                    result.ISMMMD_ActiveFlag = false;
                }
                else
                {
                    result.ISMMMD_ActiveFlag = true;
                }

                result.UpdatedDate = DateTime.Now;
                _INVContext.Update(result);
                int a = _INVContext.SaveChanges();
                if (a > 0)
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

        public MastersModule_DTO get_emplist(MastersModule_DTO data)
        {
            try
            {
                var hmdcode = _INVContext.HR_Master_Department.Where(t => t.HRMD_Id == data.HRMD_Id).ToList();


                data.emplist = (from a in _INVContext.HR_Master_Department
                                from b in _INVContext.MasterEmployee
                                from c in _INVContext.HR_Master_DepartmentCodeDMO
                                from d in _INVContext.Staff_User_Login
                                where (a.MI_Id == b.MI_Id && a.HRMD_Id == b.HRMD_Id && a.HRMDC_ID == c.HRMDC_ID && b.HRME_Id == d.Emp_Code && a.HRMD_ActiveFlag == true && b.HRME_ActiveFlag == true && b.HRME_LeftFlag == false) /*&& c.HRMDC_ID == hmdcode.FirstOrDefault().HRMDC_ID*/
                                select new MastersModule_DTO
                                {
                                    empname = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : b.HRME_EmployeeFirstName) + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" || b.HRME_EmployeeMiddleName == "0" ? "" : " " + b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" || b.HRME_EmployeeLastName == "0" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                    HRME_Id = b.HRME_Id,
                                    HRMD_Id = Convert.ToInt64(b.HRMD_Id),
                                    MI_Id = b.MI_Id
                                }).Distinct().ToArray();

                data.emplistHead = (from a in _INVContext.HR_Master_Department
                                    from b in _INVContext.MasterEmployee
                                    from c in _INVContext.HR_Master_DepartmentCodeDMO
                                    from d in _INVContext.Staff_User_Login
                                    where (a.MI_Id == b.MI_Id && a.HRMD_Id == b.HRMD_Id && a.HRMD_ActiveFlag == true && b.HRME_ActiveFlag == true && a.HRMDC_ID == c.HRMDC_ID && b.HRME_Id == d.Emp_Code && b.HRME_LeftFlag == false) /*&& c.HRMDC_ID == hmdcode.FirstOrDefault().HRMDC_ID*/
                                    select new MastersModule_DTO
                                    {
                                        empname = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : b.HRME_EmployeeFirstName) + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" || b.HRME_EmployeeMiddleName == "0" ? "" : " " + b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" || b.HRME_EmployeeLastName == "0" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                        headId = b.HRME_Id,
                                        HRMD_Id = Convert.ToInt64(b.HRMD_Id),
                                        MI_Id = b.MI_Id
                                    }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public MastersModule_DTO editlist(MastersModule_DTO data)
        {
            try
            {
                var edit = (from a in _INVContext.MastersModule_DMO
                            from b in _INVContext.ISM_Master_Module_DevelopersDMO
                            where (a.ISMMMD_Id == b.ISMMMD_Id && a.ISMMMD_Id == data.ISMMMD_Id)
                            select new MastersModule_DTO
                            {
                                ISMMMD_Id = a.ISMMMD_Id,
                                HRMD_Id = a.HRMD_Id,
                                ISMMPR_Id = a.ISMMPR_Id,
                                ISMMMD_ModuleHeadId = a.ISMMMD_ModuleHeadId,
                                ISMMMD_ActiveFlag = a.ISMMMD_ActiveFlag,
                                ISMMMDDE_Id = b.ISMMMDDE_Id,
                                IVRMMMDDE_ModuleIncharge = b.IVRMMMDDE_ModuleIncharge,
                                ISMMMDDE_ActiveFlag = b.ISMMMDDE_ActiveFlag,
                                IVRMM_Id = a.IVRMM_Id,
                                IVRMMMDDE_ModuleHeadFlg = b.IVRMMMDDE_ModuleHeadFlg,
                            }).Distinct().ToList();
                data.editlist = edit.ToArray();

                data.emplist = (from t in _INVContext.MasterEmployee
                                from a in _INVContext.Staff_User_Login
                                where (t.HRME_Id == a.Emp_Code && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false)
                                select new MastersModule_DTO
                                {
                                    empname = t.HRME_EmployeeFirstName + (string.IsNullOrEmpty(t.HRME_EmployeeMiddleName) ? "" : ' ' + t.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(t.HRME_EmployeeLastName) ? "" : ' ' + t.HRME_EmployeeLastName),
                                    HRME_Id = t.HRME_Id,
                                    HRMD_Id = Convert.ToInt64(t.HRMD_Id),
                                    MI_Id = t.MI_Id
                                }).Distinct().OrderBy(t => t.HRME_Id).ToArray();


                data.emplistHead = (from t in _INVContext.MasterEmployee
                                    from a in _INVContext.Staff_User_Login
                                    where (t.HRME_Id == a.Emp_Code && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false)
                                    select new MastersModule_DTO
                                    {
                                        empname = t.HRME_EmployeeFirstName + (string.IsNullOrEmpty(t.HRME_EmployeeMiddleName) ? "" : ' ' + t.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(t.HRME_EmployeeLastName) ? "" : ' ' + t.HRME_EmployeeLastName),
                                        headId = t.HRME_Id,
                                        HRMD_Id = Convert.ToInt64(t.HRMD_Id),
                                        MI_Id = t.MI_Id
                                    }).Distinct().OrderBy(t => t.HRME_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public MastersModule_DTO get_MappedDeveloperlist(MastersModule_DTO data)
        {
            try
            {
                data.developerlistd = (from a in _INVContext.MastersModule_DMO
                                       from b in _INVContext.ISM_Master_Module_DevelopersDMO
                                       from c in _INVContext.MasterModules
                                       where (a.ISMMMD_Id == b.ISMMMD_Id && a.IVRMM_Id == c.IVRMM_Id && a.ISMMMD_Id == data.ISMMMD_Id && c.Module_ActiveFlag == 1)
                                       select new MastersModule_DTO
                                       {
                                           ISMMMD_Id = a.ISMMMD_Id,
                                           HRMD_Id = a.HRMD_Id,
                                           ISMMPR_Id = a.ISMMPR_Id,
                                           ISMMMD_ModuleHeadId = a.ISMMMD_ModuleHeadId,
                                           ISMMMD_ActiveFlag = a.ISMMMD_ActiveFlag,
                                           ISMMMDDE_Id = b.ISMMMDDE_Id,
                                           IVRMMMDDE_ModuleIncharge = b.IVRMMMDDE_ModuleIncharge,
                                           ISMMMDDE_ActiveFlag = b.ISMMMDDE_ActiveFlag,
                                           IVRMM_Id = a.IVRMM_Id,
                                           IVRMMMDDE_ModuleHeadFlg = b.IVRMMMDDE_ModuleHeadFlg,

                                           modulehead1 = _INVContext.MasterEmployee.Where(t => t.HRME_Id == a.ISMMMD_ModuleHeadId).SingleOrDefault().HRME_EmployeeFirstName,
                                           modulehead2 = _INVContext.MasterEmployee.Where(t => t.HRME_Id == a.ISMMMD_ModuleHeadId).SingleOrDefault().HRME_EmployeeMiddleName,
                                           modulehead3 = _INVContext.MasterEmployee.Where(t => t.HRME_Id == a.ISMMMD_ModuleHeadId).SingleOrDefault().HRME_EmployeeLastName,

                                           developerName1 = _INVContext.MasterEmployee.Where(t => t.HRME_Id == b.IVRMMMDDE_ModuleIncharge).SingleOrDefault().HRME_EmployeeFirstName,
                                           developerName2 = _INVContext.MasterEmployee.Where(t => t.HRME_Id == b.IVRMMMDDE_ModuleIncharge).SingleOrDefault().HRME_EmployeeMiddleName,
                                           developerName3 = _INVContext.MasterEmployee.Where(t => t.HRME_Id == b.IVRMMMDDE_ModuleIncharge).SingleOrDefault().HRME_EmployeeLastName,
                                           IVRMM_ModuleName = c.IVRMM_ModuleName,
                                       }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public MastersModule_DTO deactiveDevpMappingdata(MastersModule_DTO data)
        {
            try
            {
                var result = _INVContext.ISM_Master_Module_DevelopersDMO.Single(t => t.ISMMMDDE_Id == data.ISMMMDDE_Id);
                if (result.ISMMMDDE_ActiveFlag == true)
                {
                    result.ISMMMDDE_ActiveFlag = false;
                }
                else
                {
                    result.ISMMMDDE_ActiveFlag = true;
                }

                result.UpdatedDate = DateTime.Now;
                _INVContext.Update(result);
                int a = _INVContext.SaveChanges();
                if (a > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }

                data.developerlistd = (from ab in _INVContext.MastersModule_DMO
                                       from b in _INVContext.ISM_Master_Module_DevelopersDMO
                                       from c in _INVContext.MasterModules
                                       where (ab.ISMMMD_Id == b.ISMMMD_Id && ab.IVRMM_Id == c.IVRMM_Id && ab.MI_Id == data.MI_Id && ab.ISMMMD_Id == data.ISMMMD_Id && c.Module_ActiveFlag == 1)
                                       select new MastersModule_DTO
                                       {
                                           ISMMMD_Id = ab.ISMMMD_Id,
                                           HRMD_Id = ab.HRMD_Id,
                                           ISMMPR_Id = ab.ISMMPR_Id,
                                           ISMMMD_ModuleHeadId = ab.ISMMMD_ModuleHeadId,

                                           ISMMMDDE_Id = b.ISMMMDDE_Id,
                                           IVRMMMDDE_ModuleIncharge = b.IVRMMMDDE_ModuleIncharge,
                                           ISMMMDDE_ActiveFlag = b.ISMMMDDE_ActiveFlag,
                                           IVRMM_Id = ab.IVRMM_Id,
                                           IVRMMMDDE_ModuleHeadFlg = b.IVRMMMDDE_ModuleHeadFlg,

                                           modulehead1 = _INVContext.MasterEmployee.Where(t => t.HRME_Id == ab.ISMMMD_ModuleHeadId).SingleOrDefault().HRME_EmployeeFirstName,
                                           modulehead2 = _INVContext.MasterEmployee.Where(t => t.HRME_Id == ab.ISMMMD_ModuleHeadId).SingleOrDefault().HRME_EmployeeMiddleName,
                                           modulehead3 = _INVContext.MasterEmployee.Where(t => t.HRME_Id == ab.ISMMMD_ModuleHeadId).SingleOrDefault().HRME_EmployeeLastName,

                                           developerName1 = _INVContext.MasterEmployee.Where(t => t.HRME_Id == b.IVRMMMDDE_ModuleIncharge).SingleOrDefault().HRME_EmployeeFirstName,
                                           developerName2 = _INVContext.MasterEmployee.Where(t => t.HRME_Id == b.IVRMMMDDE_ModuleIncharge).SingleOrDefault().HRME_EmployeeFirstName,
                                           developerName3 = _INVContext.MasterEmployee.Where(t => t.HRME_Id == b.IVRMMMDDE_ModuleIncharge).SingleOrDefault().HRME_EmployeeFirstName,
                                           IVRMM_ModuleName = c.IVRMM_ModuleName,
                                       }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }
        //============================task group================
        public ISM_Master_TaskGroup_DTO getdetails_taskgroup(ISM_Master_TaskGroup_DTO dto)
        {
            try
            {
                var hrme = _INVContext.Staff_User_Login.Where(a => a.Id == dto.UserId && a.MI_Id == dto.MI_Id).ToList();
                dto.task_grop_list = (from a in _INVContext.ISM_TaskCreationDMO
                                      from b in _INVContext.ISM_Master_TaskGroupDMO
                                      where a.ISMMTGRP_Id == b.ISMMTGRP_Id
                                      select new ISM_Master_TaskGroup_DTO
                                      {
                                          ISMMTGRP_Id = b.ISMMTGRP_Id,
                                          ISMMTGRP_TaskGroupName = b.ISMMTGRP_TaskGroupName,
                                          ISMMTGRP_ActiveFlag = b.ISMMTGRP_ActiveFlag
                                      }).Distinct().ToArray();

                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_task_grou_dd_proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 300000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                    {
                        // Value = coloumns
                        Value = dto.MI_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
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
                        dto.get_groupname = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    return dto;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;

        }

        public ISM_Master_TaskGroup_DTO getdept(ISM_Master_TaskGroup_DTO dto)
        {
            try
            {

                dto.dept_list = (from a in _INVContext.ISM_Master_TaskGroup_DeptDMO
                                 from b in _INVContext.HR_Master_Department
                                 where a.HRMD_Id == b.HRMD_Id && b.MI_Id == dto.MI_Id && a.ISMMTGRP_Id == dto.ISMMTGRP_Id
                                 select new ISM_Master_TaskGroup_DTO
                                 {
                                     HRMD_Id = a.HRMD_Id,
                                     HRMD_DepartmentName = b.HRMD_DepartmentName

                                 }).Distinct().ToArray();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;

        }

        public ISM_Master_TaskGroup_DTO show_tasklist(ISM_Master_TaskGroup_DTO dto)
        {
            try
            {

                string hrmd_id = "0";
                List<long> hrmd = new List<long>();
                foreach (var item in dto.deptarray)
                {
                    hrmd.Add(item.HRMD_Id);
                }
                for (int a = 0; a < hrmd.Count(); a++)
                {
                    hrmd_id = hrmd_id + ',' + hrmd[a].ToString();
                }
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_task_list_proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 300000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                    {
                        // Value = coloumns
                        Value = dto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id",
                      SqlDbType.VarChar)
                    {
                        Value = hrmd_id
                    });

                    cmd.Parameters.Add(new SqlParameter("@flg",
                      SqlDbType.VarChar)
                    {
                        Value = 0
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
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
                        dto.task_list = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    return dto;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;

        }

        public ISM_Master_TaskGroup_DTO save_taskgrpdata(ISM_Master_TaskGroup_DTO dto)
        {
            try
            {


                foreach (var item in dto.task_listarray)
                {
                    var result = _INVContext.ISM_TaskCreationDMO.Single(a => a.ISMTCR_Id == item.ISMTCR_Id && a.HRMD_Id == item.HRMD_Id);
                    result.ISMMTGRP_Id = dto.ISMMTGRP_Id;
                    result.ISMTCR_TGOrder = item.ISMTCR_TGOrder;
                    _INVContext.Update(result);
                }
                _INVContext.SaveChanges();
                dto.returnval = "Success";

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;

        }

        public ISM_Master_TaskGroup_DTO task_view(ISM_Master_TaskGroup_DTO dto)
        {
            try
            {
                dto.task_view_list = _INVContext.ISM_TaskCreationDMO.Where(a => a.ISMMTGRP_Id == dto.ISMMTGRP_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;

        }

        public ISM_Master_TaskGroup_DTO task_edit(ISM_Master_TaskGroup_DTO dto)
        {
            try
            {
                dto.task_edit = _INVContext.ISM_Master_TaskGroupDMO.Where(a => a.ISMMTGRP_Id == dto.ISMMTGRP_Id).ToArray();

                dto.task_edit_dept = (from a in _INVContext.ISM_Master_TaskGroupDMO
                                      from b in _INVContext.ISM_TaskCreationDMO
                                      from c in _INVContext.HR_Master_Department
                                      where a.ISMMTGRP_Id == b.ISMMTGRP_Id && b.HRMD_Id == c.HRMD_Id && a.ISMMTGRP_Id == dto.ISMMTGRP_Id
                                      select new ISM_Master_TaskGroup_DTO
                                      {
                                          HRMD_Id = c.HRMD_Id
                                      }).ToArray();

                dto.dept_list = (from a in _INVContext.ISM_Master_TaskGroup_DeptDMO
                                 from b in _INVContext.HR_Master_Department
                                 where a.HRMD_Id == b.HRMD_Id && b.MI_Id == dto.MI_Id && a.ISMMTGRP_Id == dto.ISMMTGRP_Id
                                 select new ISM_Master_TaskGroup_DTO
                                 {

                                     HRMD_Id = a.HRMD_Id,
                                     HRMD_DepartmentName = b.HRMD_DepartmentName

                                 }).Distinct().ToArray();
                var dept_list1 = (from a in _INVContext.ISM_Master_TaskGroup_DeptDMO
                                  from b in _INVContext.HR_Master_Department
                                  where a.HRMD_Id == b.HRMD_Id && b.MI_Id == dto.MI_Id && a.ISMMTGRP_Id == dto.ISMMTGRP_Id
                                  select new ISM_Master_TaskGroup_DTO
                                  {
                                      HRMD_Id = a.HRMD_Id,
                                      HRMD_DepartmentName = b.HRMD_DepartmentName

                                  }).Distinct().ToList();


                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_task_edit_list_proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMMTGRP_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.ISMMTGRP_Id)
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
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
                        dto.task_list_temp = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                string hrmd_id = "0";
                List<long> hrmd = new List<long>();
                foreach (var item in dept_list1)
                {
                    hrmd.Add(item.HRMD_Id);
                }
                for (int a = 0; a < hrmd.Count(); a++)
                {
                    hrmd_id = hrmd_id + ',' + hrmd[a].ToString();
                }
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_task_list_proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 300000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                    {
                        // Value = coloumns
                        Value = dto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id",
                      SqlDbType.VarChar)
                    {
                        Value = hrmd_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@flg",
                      SqlDbType.VarChar)
                    {
                        Value = 1
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
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
                        dto.task_list = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    return dto;
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
