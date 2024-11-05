using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
using DomainModel.Model.com.vapstech.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    public class INV_ItemConsumptionImpl : Interface.INV_ItemConsumptionInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_ItemConsumptionImpl> _logInv;
        public INV_ItemConsumptionImpl(InventoryContext InvContext, ILogger<INV_ItemConsumptionImpl> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }

        public INV_ItemConsumptionDTO getloaddata(INV_ItemConsumptionDTO data)
        {
            try
            {
                data.get_Store = _INVContext.INV_Master_StoreDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMS_ActiveFlg == true).Distinct().OrderBy(m => m.INVMST_Id).ToArray();
                data.get_item = _INVContext.INV_Master_ItemDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMI_ActiveFlg == true).Distinct().OrderBy(m => m.INVMI_Id).ToArray();
                data.get_Department = _INVContext.HR_Master_Department.Where(m => m.MI_Id == data.MI_Id && m.HRMD_ActiveFlag == true).Distinct().OrderBy(m => m.HRMD_Order).ToArray();
                data.get_Product = _INVContext.INV_Master_ProductDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMP_ActiveFlg == true).Distinct().OrderBy(m => m.INVMP_Id).ToArray();

                data.get_employee = (from a in _INVContext.MasterEmployee
                                     where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true)
                                     select new INV_ItemConsumptionDTO
                                     {
                                         employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                         HRME_EmployeeCode = a.HRME_EmployeeCode,
                                         HRME_Id = a.HRME_Id,
                                         HRME_EmployeeOrder = a.HRME_EmployeeOrder,

                                     }).Distinct().OrderBy(h => h.HRME_EmployeeOrder).ToArray();

                data.get_Student_Cls_Sec=(from a in _INVContext.School_M_Class
                                          from b in _INVContext.School_Adm_Y_StudentDMO
                                          where (a.ASMCL_Id == b.ASMCL_Id && a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true &&  b.AMAY_ActiveFlag == 1 && b.ASMAY_Id == data.ASMAY_Id)
                                          select new INV_ItemConsumptionDTO
                                          {
                                              ASMCL_Id = a.ASMCL_Id,
                                              ASMCL_ClassName=a.ASMCL_ClassName
                                          }).Distinct().OrderBy(m => m.ASMCL_Id).ToArray();



                data.get_itemconsumption = (from a in _INVContext.INV_M_ItemConsumptionDMO
                                            from b in _INVContext.INV_T_ItemConsumptionDMO
                                            from c in _INVContext.INV_Master_StoreDMO
                                            where (a.INVMIC_Id == b.INVMIC_Id && a.INVMST_Id == c.INVMST_Id && a.MI_Id == data.MI_Id)
                                            select new INV_ItemConsumptionDTO
                                            {
                                                INVMIC_Id = a.INVMIC_Id,
                                                INVMST_Id = a.INVMST_Id,
                                                INVMS_StoreName = c.INVMS_StoreName,
                                                INVMIC_StuOtherFlg = a.INVMIC_StuOtherFlg,
                                                INVMIC_ICNo = a.INVMIC_ICNo,
                                                INVMIC_ICDate = a.INVMIC_ICDate,
                                                INVMIC_Remarks = a.INVMIC_Remarks,
                                                INVMIC_ActiveFlg = a.INVMIC_ActiveFlg
                                            }).Distinct().OrderBy(m => m.INVTIC_Id).ToArray();
                //CLG_Adm_Master_SemesterDMO
                data.semesterlist = _INVContext.CLG_Adm_Master_SemesterDMO.Where(R => R.MI_Id == data.MI_Id && R.AMSE_ActiveFlg == true).Distinct().ToArray();
                data.sectionClglist = _INVContext.Adm_College_Master_SectionDMO.Where(R => R.MI_Id == data.MI_Id && R.ACMS_ActiveFlag == true).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("IC load Page:" + ex.Message);
            }
            return data;
        }

        public async Task<INV_ItemConsumptionDTO> getICDetails(INV_ItemConsumptionDTO data)
        {
            try
            {
                //data.get_ICdetails = (from a in _INVContext.INV_M_ItemConsumptionDMO
                //                      from b in _INVContext.INV_T_ItemConsumptionDMO
                //                      from c in _INVContext.INV_Master_ItemDMO
                //                      from d in _INVContext.INV_Master_UOMDMO
                //                      from e in _INVContext.INV_Master_ProductDMO
                //                      where (a.INVMIC_Id == b.INVMIC_Id && b.INVMI_Id == c.INVMI_Id && b.INVMUOM_Id == d.INVMUOM_Id && b.INVMP_Id == e.INVMP_Id && a.MI_Id == data.MI_Id && a.INVMIC_Id == data.INVMIC_Id)
                //                      select new INV_ItemConsumptionDTO
                //                      {
                //                          INVMIC_Id = a.INVMIC_Id,
                //                          INVMIC_StuOtherFlg = a.INVMIC_StuOtherFlg,
                //                          INVTIC_Id = b.INVTIC_Id,
                //                          INVMI_Id = b.INVMI_Id,
                //                          INVMUOM_Id = b.INVMUOM_Id,
                //                          INVMP_Id = b.INVMP_Id,
                //                          INVMI_ItemName = c.INVMI_ItemName,
                //                          INVMUOM_UOMName = d.INVMUOM_UOMName,
                //                          INVMP_ProductName = e.INVMP_ProductName,
                //                          INVTIC_BatchNo = b.INVTIC_BatchNo,
                //                          INVTIC_ICQty = b.INVTIC_ICQty,
                //                          INVTIC_Naration = b.INVTIC_Naration,
                //                          INVTIC_ActiveFlg = b.INVTIC_ActiveFlg

                //                      }).Distinct().OrderBy(m => m.INVMIC_Id).ToArray();
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_ICdetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMIC_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.INVMIC_Id
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
                        data.get_ICdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_ICUserDetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMIC_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.INVMIC_Id
                    });
                    
                    cmd.Parameters.Add(new SqlParameter("@userflag",
                    SqlDbType.VarChar)
                    {
                        Value = data.userflag
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
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
                        data.get_ICuser = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("IC Details Page:" + ex.Message);
            }
            return data;
        }

        public INV_ItemConsumptionDTO savedetails(INV_ItemConsumptionDTO data)
        {
            try
            {
                if (data.INVMIC_Id != 0)
                {
                    var result = _INVContext.INV_M_ItemConsumptionDMO.Single(t => t.MI_Id == data.MI_Id && t.INVMIC_Id == data.INVMIC_Id);
                    result.MI_Id = data.MI_Id;
                    result.INVMIC_StuOtherFlg = data.INVMIC_StuOtherFlg;
                    result.INVMIC_ICNo = data.INVMIC_ICNo;
                    result.INVMIC_ICDate = data.INVMIC_ICDate;
                    result.INVMIC_Remarks = data.INVMIC_Remarks;
                    result.UpdatedDate = DateTime.Now;
                    _INVContext.Update(result);
                    foreach (var ic in data.arrayIC)
                    {
                        var resultSub = _INVContext.INV_T_ItemConsumptionDMO.Single(t => t.INVTIC_Id == ic.INVTIC_Id);
                        resultSub.INVMP_Id = ic.INVMP_Id;
                        resultSub.INVTIC_BatchNo = ic.INVTIC_BatchNo;
                        resultSub.INVTIC_ICQty = ic.INVTIC_ICQty;
                        resultSub.INVTIC_Naration = ic.INVTIC_Naration;
                        resultSub.INVTIC_ActiveFlg = true;
                        resultSub.UpdatedDate = DateTime.Now;
                        _INVContext.Update(resultSub);

                        var context = _INVContext.SaveChanges();
                        if (context > 0)
                        {
                            try
                            {
                                var contactInsert = _INVContext.Database.ExecuteSqlCommand("INV_InsertItemConsumption @p0,@p1,@p2,@p3,@p4", data.MI_Id, result.INVMIC_Id, data.INVMST_Id, resultSub.INVMI_Id, ic.INVTIC_ICPrice);
                                if (contactInsert > 0)
                                {
                                    data.returnduplicatestatus = "Updated";
                                }
                                else
                                {
                                    data.returnduplicatestatus = "notUpdated";
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
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
                    INV_M_ItemConsumptionDMO IMC = new INV_M_ItemConsumptionDMO();
                    IMC.MI_Id = data.MI_Id;
                    IMC.INVMST_Id = data.INVMST_Id;
                    IMC.INVMIC_StuOtherFlg = data.INVMIC_StuOtherFlg;
                    IMC.INVMIC_ICNo = data.INVMIC_ICNo;
                    IMC.INVMIC_ICDate = data.INVMIC_ICDate;
                    IMC.INVMIC_Remarks = data.INVMIC_Remarks;
                    IMC.INVMIC_ActiveFlg = true;
                    IMC.UpdatedDate = DateTime.Now;
                    IMC.CreatedDate = DateTime.Now;
                    _INVContext.Add(IMC);


                    if (data.INVMIC_StuOtherFlg == "Staff")
                    {
                        foreach (var icstaff in data.arrayStaff)
                        {
                            INV_M_IC_StaffDMO ICstaff = new INV_M_IC_StaffDMO();
                            ICstaff.INVMIC_Id = IMC.INVMIC_Id;
                            ICstaff.HRME_Id = icstaff.HRME_Id;
                            ICstaff.INVMICST_ActiveFlg = true;
                            ICstaff.CreatedDate = DateTime.Now;
                            ICstaff.UpdatedDate = DateTime.Now;
                            _INVContext.Add(ICstaff);
                        }
                    }
                    else if (data.INVMIC_StuOtherFlg == "Department")
                    {
                        INV_M_IC_DepartmentDMO ICDept = new INV_M_IC_DepartmentDMO();
                        ICDept.INVMIC_Id = IMC.INVMIC_Id;
                        ICDept.HRMD_Id = data.HRMD_Id;
                        ICDept.INVMICD_ActiveFlg = true;
                        ICDept.CreatedDate = DateTime.Now;
                        ICDept.UpdatedDate = DateTime.Now;
                        _INVContext.Add(ICDept);

                    }
                    else if (data.INVMIC_StuOtherFlg == "Student")
                    {
                        if(data.FlagsC== "C")
                        {
                            foreach (var item in data.arrayStudentname)
                            {
                                NV_M_ItemConsumptionCLGDMO ICAmst = new NV_M_ItemConsumptionCLGDMO();
                                ICAmst.INVMIC_Id = IMC.INVMIC_Id;
                                ICAmst.AMCST_Id = item.AMST_Id;
                                ICAmst.AMSE_Id = item.AMSE_Id;                               
                                ICAmst.INVMICS_ActiveFlg = true;
                                ICAmst.INVMICS_CreatedDate = DateTime.Now;
                                ICAmst.INVMICS_UpdatedDate = DateTime.Now;
                                _INVContext.Add(ICAmst);
                            }
                              

                        }
                        else if (data.FlagsC == "S")
                        {
                            foreach (var item in data.arrayStudentname)
                            {

                                var amst = (from a in _INVContext.Adm_M_Student
                                            from b in _INVContext.School_Adm_Y_StudentDMO
                                            where (a.AMST_Id == b.AMST_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1 && a.MI_Id == data.MI_Id && b.AMST_Id == item.AMST_Id && b.ASMAY_Id == data.ASMAY_Id)
                                            select new INV_ItemConsumptionDTO
                                            {
                                                AMST_Id = a.AMST_Id,
                                                ASMCL_Id = b.ASMCL_Id,
                                                ASMS_Id = b.ASMS_Id
                                            }).Distinct().ToList();
                                var asmcl = amst.FirstOrDefault().ASMCL_Id;
                                var asms = amst.FirstOrDefault().ASMS_Id;

                                INV_M_IC_StudentDMO ICAmst = new INV_M_IC_StudentDMO();
                                ICAmst.INVMIC_Id = IMC.INVMIC_Id;
                                ICAmst.AMST_Id = item.AMST_Id;
                                ICAmst.ASMCL_Id = asmcl;
                                ICAmst.ASMS_Id = asms;
                                ICAmst.INVMICS_ActiveFlg = true;
                                ICAmst.CreatedDate = DateTime.Now;
                                ICAmst.UpdatedDate = DateTime.Now;
                                _INVContext.Add(ICAmst);
                            }
                        }
                       
                    }

                        foreach (var ic in data.arrayIC)
                        {
                            INV_T_ItemConsumptionDMO ITC = new INV_T_ItemConsumptionDMO();
                            ITC.INVMIC_Id = IMC.INVMIC_Id;
                            ITC.INVMI_Id = ic.INVMI_Id;
                            ITC.INVMUOM_Id = ic.INVMUOM_Id;
                            ITC.INVMP_Id = ic.INVMP_Id;
                            ITC.INVTIC_BatchNo = ic.INVTIC_BatchNo;
                            ITC.INVTIC_ICPrice = ic.INVTIC_ICPrice;
                            ITC.INVTIC_ICQty = ic.INVTIC_ICQty;
                            ITC.INVTIC_Naration = ic.INVTIC_Naration;
                            ITC.INVTIC_ActiveFlg = true;
                            ITC.CreatedDate = DateTime.Now;
                            ITC.UpdatedDate = DateTime.Now;
                            _INVContext.Add(ITC);

                            var context = _INVContext.SaveChanges();
                            if (context > 0)
                            {
                                try
                                {
                                    var contactInsert = _INVContext.Database.ExecuteSqlCommand("INV_InsertItemConsumption @p0,@p1,@p2,@p3,@p4", data.MI_Id, IMC.INVMIC_Id, data.INVMST_Id, ITC.INVMI_Id, ic.INVTIC_ICPrice);
                                    if (contactInsert > 0)
                                    {
                                        data.returnduplicatestatus = "Updated";
                                    }
                                    else
                                    {
                                        data.returnduplicatestatus = "notUpdated";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                data.returnval = true;
                            }
                            else
                            {
                                data.returnval = false;
                            }
                        }
                    
                }
            }
            catch (Exception ex)
            {
                data.message = "Error";
                _logInv.LogInformation("IC savedata :" + ex.Message);
            }
            return data;
        }

        public INV_ItemConsumptionDTO deactive(INV_ItemConsumptionDTO data)
        {
            try
            {
                var result = _INVContext.INV_M_ItemConsumptionDMO.Single(t => t.INVMIC_Id == data.INVMIC_Id && t.MI_Id == data.MI_Id);

                if (result.INVMIC_ActiveFlg == true)
                {
                    result.INVMIC_ActiveFlg = false;
                }
                else if (result.INVMIC_ActiveFlg == false)
                {
                    result.INVMIC_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _INVContext.Update(result);

                var resultt = _INVContext.INV_T_ItemConsumptionDMO.Where(t => t.INVMIC_Id == data.INVMIC_Id).ToList();
                foreach (var rt in resultt)
                {
                    if (result.INVMIC_ActiveFlg == true)
                    {
                        rt.INVTIC_ActiveFlg = true;
                        rt.UpdatedDate = DateTime.Now;
                        _INVContext.Update(rt);
                    }
                    if (result.INVMIC_ActiveFlg == false)
                    {
                        rt.INVTIC_ActiveFlg = false;
                        rt.UpdatedDate = DateTime.Now;
                        _INVContext.Update(rt);
                    }
                }
                if (result.INVMIC_ActiveFlg == true)
                {
                    if (data.INVMIC_StuOtherFlg == "Staff")
                    {
                        var resultmflgEmp = _INVContext.INV_M_IC_StaffDMO.Where(t => t.INVMIC_Id == data.INVMIC_Id).ToList();
                        foreach (var ep in resultmflgEmp)
                        {
                            ep.INVMICST_ActiveFlg = true;
                            result.UpdatedDate = DateTime.Now;
                            _INVContext.Update(ep);
                        }
                    }
                    else if (data.INVMIC_StuOtherFlg == "Department")
                    {
                        var resultmflgD = _INVContext.INV_M_IC_DepartmentDMO.Where(t => t.INVMIC_Id == data.INVMIC_Id).ToList();
                        foreach (var dep in resultmflgD)
                        {
                            dep.INVMICD_ActiveFlg = true;
                            result.UpdatedDate = DateTime.Now;
                            _INVContext.Update(dep);
                        }
                    }
                    else if (data.INVMIC_StuOtherFlg == "Student")
                    {
                        var resultmflgST = _INVContext.INV_M_IC_StudentDMO.Where(t => t.INVMIC_Id == data.INVMIC_Id).ToList();
                        foreach (var stu in resultmflgST)
                        {
                            stu.INVMICS_ActiveFlg = true;
                            result.UpdatedDate = DateTime.Now;
                            _INVContext.Update(stu);
                        }
                    }
                }
                else if (result.INVMIC_ActiveFlg == false)
                {
                    if (data.INVMIC_StuOtherFlg == "Staff")
                    {
                        var resultmflgEmp = _INVContext.INV_M_IC_StaffDMO.Where(t => t.INVMIC_Id == data.INVMIC_Id).ToList();
                        foreach (var ep in resultmflgEmp)
                        {
                            ep.INVMICST_ActiveFlg = false;
                            result.UpdatedDate = DateTime.Now;
                            _INVContext.Update(ep);
                        }
                    }
                    else if (data.INVMIC_StuOtherFlg == "Department")
                    {
                        var resultmflgD = _INVContext.INV_M_IC_DepartmentDMO.Where(t => t.INVMIC_Id == data.INVMIC_Id).ToList();
                        foreach (var dep in resultmflgD)
                        {
                            dep.INVMICD_ActiveFlg = false;
                            result.UpdatedDate = DateTime.Now;
                            _INVContext.Update(dep);
                        }
                    }
                    else if (data.INVMIC_StuOtherFlg == "Student")
                    {
                        var resultmflgST = _INVContext.INV_M_IC_StudentDMO.Where(t => t.INVMIC_Id == data.INVMIC_Id).ToList();
                        foreach (var stu in resultmflgST)
                        {
                            stu.INVMICS_ActiveFlg = false;
                            result.UpdatedDate = DateTime.Now;
                            _INVContext.Update(stu);
                        }
                    }
                }
                int returnval = _INVContext.SaveChanges();
                if (returnval > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public INV_ItemConsumptionDTO deactiveSub(INV_ItemConsumptionDTO data)
        {
            try
            {
                var result = _INVContext.INV_T_ItemConsumptionDMO.Single(t => t.INVTIC_Id == data.INVTIC_Id);

                if (result.INVTIC_ActiveFlg == true)
                {
                    result.INVTIC_ActiveFlg = false;
                }
                else if (result.INVTIC_ActiveFlg == false)
                {
                    result.INVTIC_ActiveFlg = true;
                }

                result.UpdatedDate = DateTime.Now;
                _INVContext.Update(result);

                int countactiveT = 0;
                int countactiveF = 0;
                var resultt = _INVContext.INV_T_ItemConsumptionDMO.Where(t => t.INVMIC_Id == data.INVMIC_Id).ToList();
                foreach (var rt in resultt)
                {
                    if (rt.INVTIC_ActiveFlg == false)
                    {
                        countactiveF += 1;
                    }
                    else if (rt.INVTIC_ActiveFlg == true)
                    {
                        countactiveT += 1;
                    }
                }
                var resultmflg = _INVContext.INV_M_ItemConsumptionDMO.Single(t => t.INVMIC_Id == data.INVMIC_Id);
                if (countactiveF > 0 && countactiveT == 0)
                {
                    resultmflg.INVMIC_ActiveFlg = false;
                    if (resultmflg.INVMIC_ActiveFlg == false)
                    {
                        if (data.INVMIC_StuOtherFlg == "Staff")
                        {
                            var resultmflgEmp = _INVContext.INV_M_IC_StaffDMO.Where(t => t.INVMIC_Id == data.INVMIC_Id).ToList();
                            foreach (var ep in resultmflgEmp)
                            {
                                ep.INVMICST_ActiveFlg = false;
                                result.UpdatedDate = DateTime.Now;
                                _INVContext.Update(ep);
                            }
                        }
                        else if (data.INVMIC_StuOtherFlg == "Department")
                        {
                            var resultmflgD = _INVContext.INV_M_IC_DepartmentDMO.Where(t => t.INVMIC_Id == data.INVMIC_Id).ToList();
                            foreach (var dep in resultmflgD)
                            {
                                dep.INVMICD_ActiveFlg = false;
                                result.UpdatedDate = DateTime.Now;
                                _INVContext.Update(dep);
                            }
                        }
                        else if (data.INVMIC_StuOtherFlg == "Student")
                        {
                            var resultmflgST = _INVContext.INV_M_IC_StudentDMO.Where(t => t.INVMIC_Id == data.INVMIC_Id).ToList();
                            foreach (var stu in resultmflgST)
                            {
                                stu.INVMICS_ActiveFlg = false;
                                result.UpdatedDate = DateTime.Now;
                                _INVContext.Update(stu);
                            }
                        }
                    }
                    result.UpdatedDate = DateTime.Now;
                    _INVContext.Update(resultmflg);
                }
                else if (countactiveT > 0 && countactiveF == 0)
                {
                    resultmflg.INVMIC_ActiveFlg = true;
                    if (resultmflg.INVMIC_ActiveFlg == true)
                    {
                        if (data.INVMIC_StuOtherFlg == "Staff")
                        {
                            var resultmflgEmp = _INVContext.INV_M_IC_StaffDMO.Where(t => t.INVMIC_Id == data.INVMIC_Id).ToList();
                            foreach (var ep in resultmflgEmp)
                            {
                                ep.INVMICST_ActiveFlg = true;
                                result.UpdatedDate = DateTime.Now;
                                _INVContext.Update(ep);
                            }
                        }
                        else if (data.INVMIC_StuOtherFlg == "Department")
                        {
                            var resultmflgD = _INVContext.INV_M_IC_DepartmentDMO.Where(t => t.INVMIC_Id == data.INVMIC_Id).ToList();
                            foreach (var dep in resultmflgD)
                            {
                                dep.INVMICD_ActiveFlg = true;
                                result.UpdatedDate = DateTime.Now;
                                _INVContext.Update(dep);
                            }
                        }
                        else if (data.INVMIC_StuOtherFlg == "Student")
                        {
                            var resultmflgST = _INVContext.INV_M_IC_StudentDMO.Where(t => t.INVMIC_Id == data.INVMIC_Id).ToList();
                            foreach (var stu in resultmflgST)
                            {
                                stu.INVMICS_ActiveFlg = true;
                                result.UpdatedDate = DateTime.Now;
                                _INVContext.Update(stu);
                            }
                        }
                    }
                    result.UpdatedDate = DateTime.Now;
                    _INVContext.Update(resultmflg);
                }

                int returnval = _INVContext.SaveChanges();
                if (returnval > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public INV_ItemConsumptionDTO getsection(INV_ItemConsumptionDTO dto)
        {
            try
            {
                dto.get_sectionlist= (from a in _INVContext.School_M_Section
                                      from b in _INVContext.School_Adm_Y_StudentDMO
                                      where (a.ASMS_Id == b.ASMS_Id && a.MI_Id == dto.MI_Id && a.ASMC_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1 && b.ASMAY_Id == dto.ASMAY_Id && b.ASMCL_Id==dto.ASMCL_Id)
                                      select new INV_ItemConsumptionDTO
                                      {
                                          ASMS_Id = a.ASMS_Id,
                                          ASMC_SectionName = a.ASMC_SectionName
                                      }).Distinct().OrderBy(m => m.ASMS_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public INV_ItemConsumptionDTO getstudent(INV_ItemConsumptionDTO dto)
        {
            try
            {
                if(dto.FlagsC=="C")
                {
                    using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "INV_CollegeStudenDetails";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.BigInt)
                        {
                            Value = dto.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                         SqlDbType.BigInt)
                        {
                            Value = dto.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                        SqlDbType.BigInt)
                        {
                            Value = dto.AMSE_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ACMS_Id",
                     SqlDbType.BigInt)
                        {
                            Value = dto.ACMS_Id
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
                            dto.get_Student = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    dto.get_Student = (from a in _INVContext.Adm_M_Student
                                       from b in _INVContext.School_Adm_Y_StudentDMO
                                       where (a.AMST_Id == b.AMST_Id && a.MI_Id == dto.MI_Id && a.AMST_ActiveFlag == 1 && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && b.ASMAY_Id == dto.ASMAY_Id && b.ASMCL_Id == dto.ASMCL_Id && b.ASMS_Id == dto.ASMS_Id)
                                       select new INV_ItemConsumptionDTO
                                       {
                                           AMST_Id = b.AMST_Id,
                                           studentname = ((a.AMST_FirstName == null || a.AMST_FirstName == "" ? "" : " " + a.AMST_FirstName) + (a.AMST_MiddleName == null || a.AMST_MiddleName == "" || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) + (a.AMST_LastName == null || a.AMST_LastName == "" || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim(),
                                           AMST_AdmNo = a.AMST_AdmNo
                                       }).Distinct().OrderBy(m => m.studentname).ToArray();
                }

                

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public INV_ItemConsumptionDTO getobdetails(INV_ItemConsumptionDTO data)
        {
            try
            {

                //data.get_obdetails = (from a in _INVContext.INV_ItemConsumptionDMO
                //                      from b in _INVContext.INV_Master_StoreDMO
                //                      from c in _INVContext.INV_Master_ItemDMO
                //                      from d in _INVContext.INV_Master_UOMDMO
                //                      where (a.INVMST_Id == b.INVMST_Id && a.INVMI_Id == c.INVMI_Id && a.INVMUOM_Id == d.INVMUOM_Id && a.MI_Id == b.MI_Id && c.MI_Id == d.MI_Id && a.MI_Id == data.MI_Id && a.INVOB_Id == data.INVOB_Id)
                //                      select new INV_ItemConsumptionDTO
                //                      {
                //                          INVMS_StoreName = b.INVMS_StoreName,
                //                          INVMI_ItemName = c.INVMI_ItemName,
                //                          INVMUOM_UOMName = d.INVMUOM_UOMName,
                //                          INVMUOM_UOMAliasName = d.INVMUOM_UOMAliasName,
                //                          INVOB_Id = a.INVOB_Id,
                //                          INVMST_Id = a.INVMST_Id,
                //                          INVMI_Id = a.INVMI_Id,
                //                          INVMUOM_Id = a.INVMUOM_Id,
                //                          INVOB_BatchNo = a.INVOB_BatchNo,
                //                          INVOB_PurchaseDate = a.INVOB_PurchaseDate,
                //                          INVOB_PurchaseRate = a.INVOB_PurchaseRate,
                //                          INVOB_SaleRate = a.INVOB_SaleRate,
                //                          INVOB_Qty = a.INVOB_Qty,
                //                          INVOB_Naration = a.INVOB_Naration,
                //                          INVOB_MfgDate = a.INVOB_MfgDate,
                //                          INVOB_ExpDate = a.INVOB_ExpDate,
                //                          INVOB_ActiveFlg = a.INVOB_ActiveFlg,

                //                      }).Distinct().OrderBy(m => m.INVOB_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("IC load Page:" + ex.Message);
            }
            return data;
        }

    }
}
