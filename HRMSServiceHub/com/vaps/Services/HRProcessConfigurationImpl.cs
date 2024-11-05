using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
namespace HRMSServicesHub.com.vaps.Services
{


    public class HRProcessConfigurationImpl : Interfaces.HRProcessConfigurationInterface
    {

        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public HRProcessConfigurationImpl(HRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
        {
            _HRMSContext = HRMSContext;
            _Context = Context;
        }

        public HR_ProcessDTO getBasicData(HR_ProcessDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                dto = GetAllDropdownAndDatatableDetails(dto);
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }



            return dto;
        }

        public HR_ProcessDTO GetAllDropdownAndDatatableDetails(HR_ProcessDTO dto)
        {

            List<HR_PROCESSDMO> datalist = new List<HR_PROCESSDMO>();

            List<HR_Master_GroupType> GroupTypelist = new List<HR_Master_GroupType>();

            List<HR_Master_Department> Departmentlist = new List<HR_Master_Department>();
            List<HR_Master_Designation> Designationlist = new List<HR_Master_Designation>();

            List<HR_Master_Grade> Gradelist = new List<HR_Master_Grade>();

            List<Staff_User_Login> Approved = new List<Staff_User_Login>();

            List<HR_PROCESS_PRIVILEGEDTO> previlege = new List<HR_PROCESS_PRIVILEGEDTO>();
            try
            {



                GroupTypelist = _HRMSContext.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                dto.groupTypedropdownlist = GroupTypelist.ToArray();


                Departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                dto.departmentdropdownlist = Departmentlist.ToArray();

                //Designationlist
                Designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                dto.designationdropdownlist = Designationlist.ToArray();

                //Gradelist
                Gradelist = _HRMSContext.HR_Master_Grade.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMG_ActiveFlag == true).OrderBy(t => t.HRMG_Order).ToList();
                dto.gradedropdownlist = Gradelist.ToArray();


                dto.approveid = _Context.ApplicationUser.Where(t => t.RoleTypeFlag == "Staff").ToArray();

                dto.privalue = _HRMSContext.HR_PROCESS_PRIVILEGEDTO.ToArray();


                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Process_athorisation";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id ", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
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
                        dto.gridlist = retObject.Distinct().ToArray();
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
            }
            return dto;
        }



        public HR_ProcessDTO SaveUpdate(HR_ProcessDTO dto)
        {
            try
            {

                if (dto.HRPAON_Id > 0)
                {


                    foreach (var c in dto.approvaluser_array)
                    {
                        var recorddetails = _HRMSContext.HR_Process_Auth_OrderNoDMO.Where(a => a.HRPAON_Id == dto.HRPAON_Id).ToList();

                        var result = (from a in _HRMSContext.HR_PROCESSDMO
                                      from b in _HRMSContext.HR_Process_Auth_OrderNoDMO
                                      where (a.HRPA_Id == b.HRPA_Id && a.MI_Id == dto.MI_Id && a.HRMG_Id == dto.HRMG_Id && a.HRMGT_Id == dto.HRMGT_Id && a.HRLP_EmailTo == dto.HRLP_EmailTo && a.HRLP_EmailCC == dto.HRLP_EmailCC
                                                   && a.HRMD_Id == dto.HRMD_Id && a.HRMDES_Id == dto.HRMDES_Id && a.HRPA_Id == recorddetails[0].HRPA_Id && a.HRPA_TypeFlag == c.hR_PR_NAME && b.IVRMUL_Id == c.Approval_HRME_Id
                                                   && b.HRPAON_SanctionLevelNo == c.ApprovalLevelNo && b.HRPAON_FinalFlg == c.ApprovalFinalFlag && b.IVRMUL_Id == dto.Id)
                                      select b).Count();

                        if (result > 0)
                        {
                            dto.retrunMsg = "duplicate";
                        }
                        else
                        {


                            if (recorddetails.Count > 0)
                            {
                                var update = _HRMSContext.HR_PROCESSDMO.Single(a => a.HRPA_Id == recorddetails[0].HRPA_Id);
                                update.HRMDES_Id = dto.HRMDES_Id;
                                update.HRMD_Id = dto.HRMD_Id;
                                update.HRMGT_Id = dto.HRMGT_Id;
                                update.HRMG_Id = dto.HRMG_Id;
                                update.MI_Id = dto.MI_Id;
                                update.HRLP_EmailTo = dto.HRLP_EmailTo;
                                update.HRLP_EmailCC = dto.HRLP_EmailCC;
                                update.HRPA_TypeFlag = c.hR_PR_NAME;
                                update.UpdatedDate = DateTime.Now;
                                update.HRPA_UpdatedBy = dto.UserId;
                                _HRMSContext.Update(update);
                                _HRMSContext.SaveChanges();

                            }

                            if (recorddetails.Count > 0)
                            {
                                var update = _HRMSContext.HR_Process_Auth_OrderNoDMO.Single(a => a.HRPAON_Id == dto.HRPAON_Id);
                                update.UpdatedDate = DateTime.Now;
                                update.HRPAON_SanctionLevelNo = c.ApprovalLevelNo;
                                update.HRPAON_FinalFlg = c.ApprovalFinalFlag;
                                update.IVRMUL_Id = c.Approval_HRME_Id;
                                update.HRPAON_UpdatedBy = dto.UserId;
                                _HRMSContext.Update(update);
                                var i = _HRMSContext.SaveChanges();
                                if (i > 0)
                                {
                                    dto.retrunMsg = "Updated";
                                }
                                else
                                {
                                    dto.retrunMsg = "NotUpdated";
                                }

                            }


                        }
                    }

                }
                else
                {
                    long HRPA_Id = 0;

                    var result = _HRMSContext.HR_PROCESSDMO.Where(a => a.MI_Id == dto.MI_Id && a.HRMG_Id == dto.HRMG_Id && a.HRMGT_Id == dto.HRMGT_Id && a.HRLP_EmailTo == dto.HRLP_EmailTo && a.HRLP_EmailCC == dto.HRLP_EmailCC
                                  && a.HRMD_Id == dto.HRMD_Id && a.HRMDES_Id == dto.HRMDES_Id && a.HRPA_Id == dto.HRPA_Id).ToList();

                    if (result.Count > 0)
                    {
                        HRPA_Id = result[0].HRPA_Id;

                        var result_update = _HRMSContext.HR_PROCESSDMO.Single(a => a.MI_Id == dto.MI_Id && a.HRMG_Id == dto.HRMG_Id && a.HRLP_EmailTo == dto.HRLP_EmailTo && a.HRLP_EmailCC == dto.HRLP_EmailCC
                        && a.HRMGT_Id == dto.HRMGT_Id && a.HRMD_Id == dto.HRMD_Id && a.HRMDES_Id == dto.HRMDES_Id && a.HRPA_Id == dto.HRPA_Id);

                        //result_update.HRLA_UpdatedBy = data.UserId;
                        result_update.UpdatedDate = DateTime.Now;
                        _HRMSContext.Update(result_update);
                    }
                    else
                    {
                        foreach (var c in dto.approvaluser_array)
                        {
                            HRPA_Id = 0;
                            var typeflage = _HRMSContext.HR_PROCESSDMO.Where(a => a.MI_Id == dto.MI_Id && a.HRMG_Id == dto.HRMG_Id
                         && a.HRMGT_Id == dto.HRMGT_Id && a.HRMD_Id == dto.HRMD_Id && a.HRMDES_Id == dto.HRMDES_Id && a.HRPA_TypeFlag == c.hR_PR_NAME).ToList();

                            if (typeflage.Count() == 0)
                            {

                                HR_PROCESSDMO objpge = new HR_PROCESSDMO
                                {
                                    HRPA_Id = dto.HRPA_Id,
                                    HRMDES_Id = dto.HRMDES_Id,
                                    HRMD_Id = dto.HRMD_Id,
                                    HRMGT_Id = dto.HRMGT_Id,
                                    HRMG_Id = dto.HRMG_Id,
                                    MI_Id = dto.MI_Id,
                                    HRLP_EmailTo = dto.HRLP_EmailTo,
                                    HRLP_EmailCC = dto.HRLP_EmailCC,
                                    HRPA_TypeFlag = c.hR_PR_NAME,
                                    CreatedDate = DateTime.Now,
                                    UpdatedDate = DateTime.Now,
                                    HRPA_CreatedBy = dto.UserId,
                                    HRPA_UpdatedBy = dto.UserId

                                };

                                _HRMSContext.Add(objpge);
                                _HRMSContext.SaveChanges();
                                HRPA_Id = objpge.HRPA_Id;

                            }
                            var Approved_HRME_Id = c.Approval_HRME_Id;

                            if (HRPA_Id > 0)
                            {
                                var check_emp_user_mapping = _HRMSContext.HR_Process_Auth_OrderNoDMO.Where(a => a.HRPA_Id == HRPA_Id).ToList();

                                if (check_emp_user_mapping.Count > 0)
                                {
                                    var HRPAON_Id = check_emp_user_mapping.FirstOrDefault().HRPAON_Id;
                                    var result_emp_user_mapping = _HRMSContext.HR_Process_Auth_OrderNoDMO.Single(a => a.HRPA_Id == HRPA_Id
                                    && a.IVRMUL_Id == Approved_HRME_Id && a.HRPAON_Id == HRPAON_Id);

                                    result_emp_user_mapping.UpdatedDate = DateTime.Now;
                                    result_emp_user_mapping.HRPAON_SanctionLevelNo = c.ApprovalLevelNo;
                                    result_emp_user_mapping.HRPAON_FinalFlg = c.ApprovalFinalFlag;
                                    result_emp_user_mapping.HRPAON_UpdatedBy = dto.UserId;
                                    _HRMSContext.Update(result_emp_user_mapping);
                                }
                                else
                                {
                                    HR_Process_Auth_OrderNoDMO hR_Process_Auth_OrderNo1 = new HR_Process_Auth_OrderNoDMO
                                    {
                                        IVRMUL_Id = Approved_HRME_Id,
                                        HRPA_Id = HRPA_Id,
                                        HRPAON_SanctionLevelNo = c.ApprovalLevelNo,
                                        HRPAON_FinalFlg = c.ApprovalFinalFlag,
                                        CreatedDate = DateTime.Now,
                                        UpdatedDate = DateTime.Now,


                                    };
                                    _HRMSContext.Add(hR_Process_Auth_OrderNo1);
                                }
                            }
                            else
                            {
                                HR_Process_Auth_OrderNoDMO hR_Process_Auth_OrderNo_DMO = new HR_Process_Auth_OrderNoDMO
                                {
                                    IVRMUL_Id = Approved_HRME_Id,
                                    HRPA_Id = typeflage[0].HRPA_Id,
                                    HRPAON_SanctionLevelNo = c.ApprovalLevelNo,
                                    HRPAON_FinalFlg = c.ApprovalFinalFlag,
                                    CreatedDate = DateTime.Now,
                                    UpdatedDate = DateTime.Now,
                                };
                                _HRMSContext.Add(hR_Process_Auth_OrderNo_DMO);
                            }
                        }

                    }


                    var i = _HRMSContext.SaveChanges();
                    if (i > 0)
                    {
                        dto.retrunMsg = "Add";
                    }
                    else
                    {
                        dto.retrunMsg = "NotAdded";
                    }
                }

            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public void SaveUpdateProcess_Auth_OrderNo(HR_Process_Auth_OrderNo dto)
        {
            try
            {

                HR_Process_Auth_OrderNoDMO obje_child = Mapper.Map<HR_Process_Auth_OrderNoDMO>(dto);
                if (dto.HRPAON_Id > 0)
                {
                    var childresult = _HRMSContext.HR_Process_Auth_OrderNoDMO.Single(t => t.HRPAON_Id == dto.HRPAON_Id);

                    dto.UpdatedDate = DateTime.Now;
                    childresult.HRPAON_UpdatedBy = dto.LogInUserId;
                    Mapper.Map(dto, childresult);
                    _HRMSContext.Update(childresult);
                    var existcount = _HRMSContext.SaveChanges();
                }
                else
                {
                    obje_child.CreatedDate = DateTime.Now;
                    obje_child.UpdatedDate = DateTime.Now;
                    obje_child.HRPAON_UpdatedBy = dto.LogInUserId;
                    obje_child.HRPAON_CreatedBy = dto.LogInUserId;

                    _HRMSContext.Add(obje_child);
                    var existcount = _HRMSContext.SaveChanges();
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        public HR_ProcessDTO editData(int id)
        {
            HR_ProcessDTO page = new HR_ProcessDTO();
            try
            {

                page.griddisplay = (from a in _HRMSContext.HR_PROCESSDMO
                                    from b in _HRMSContext.HR_Process_Auth_OrderNoDMO
                                    from c in _HRMSContext.Staff_User_Login
                                    where (a.HRPA_Id == b.HRPA_Id && c.Id == b.IVRMUL_Id && b.HRPAON_Id == id)
                                    select new HR_ProcessDTO
                                    {
                                        HRPA_Id = a.HRPA_Id,
                                        HRMG_Id = a.HRMG_Id,
                                        IVRMUL_Id =  Convert.ToInt32(b.IVRMUL_Id),
                                        HRPAON_SanctionLevelNo = b.HRPAON_SanctionLevelNo,
                                        HRMGT_Id = a.HRMGT_Id,
                                        HRMD_Id = a.HRMD_Id,
                                        HRMDES_Id = a.HRMDES_Id,
                                        HRLP_EmailTo = a.HRLP_EmailTo,
                                        HRLP_EmailCC = a.HRLP_EmailCC,
                                        HRPA_TypeFlag = a.HRPA_TypeFlag,
                                        HRPAON_FinalFlg = b.HRPAON_FinalFlg,
                                        IVRMUL_UserName = c.IVRMSTAUL_UserName,
                                        IVRMSTAUL_Id = Convert.ToInt32(c.Id)
                                    }).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        //public HR_ProcessDTO editData(int id)
        //{
        //    HR_ProcessDTO dto = new HR_ProcessDTO();

        //    dto.retrunMsg = "";
        //    try
        //    {
        //        var empHeads = (from a in .HR_PROCESSDMO
        //                        from b in _HRMSContext.HR_Process_Auth_OrderNoDMO
        //                        from c in _HRMSContext.Staff_User_Login
        //                        where (a.HRPA_Id == b.HRPA_Id && c.Id == b.IVRMUL_Id && b.HRPAON_Id == id)
        //                        select new HR_ProcessDTO
        //                        {
        //                            HRPA_Id = a.HRPA_Id,
        //                            HRMGT_Id = a.HRMGT_Id,
        //                            HRMD_Id = a.HRMD_Id,
        //                            HRMDES_Id = a.HRMDES_Id,
        //                            HRMG_Id = a.HRMG_Id,
        //                            HRLP_EmailTo = a.HRLP_EmailTo,
        //                            HRLP_EmailCC = a.HRLP_EmailCC,
        //                            HRPA_TypeFlag = a.HRPA_TypeFlag,
        //                            HRPAON_Id = b.HRPAON_Id,
        //                            IVRMSTAUL_Id = c.Id,
        //                            HRPAON_SanctionLevelNo = b.HRPAON_SanctionLevelNo,
        //                            HRPAON_FinalFlg = b.HRPAON_FinalFlg,
        //                            // HRLP_EmailTo = a.HRLP_EmailTo,
        //                            IVRMUL_UserName = c.IVRMSTAUL_UserName
        //                        }).Distinct().ToList();



        //        dto.griddisplay = empHeads.ToArray();
        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //        dto.retrunMsg = "Error occured";
        //    }

        //    return dto;
        //}


        public HR_ProcessDTO deactivate(int id)
        {

            HR_ProcessDTO dto = new HR_ProcessDTO();
            return dto;
        }

        //public HR_ProcessDTO SaveUpdate(HR_ProcessDTO dto)
        //{
        //    //dto.retrunMsg = "";
        //    //int lorg = 0, countchild = 0;
        //    try
        //    {

        //        for (int s = 0; s < dto.tempactivites.Length; s++)
        //        {
        //            dto.HRPA_TypeFlag = dto.tempactivites[s].columnName;
        //            if (dto.HRPA_Id == 0)
        //            {
        //                HR_PROCESSDMO obje_p = Mapper.Map<HR_PROCESSDMO>(dto);
        //                //obje_p.HRPA_TypeFlag = dto.HRPA_TypeFlag;
        //                obje_p.CreatedDate = DateTime.Now;
        //                obje_p.UpdatedDate = DateTime.Now;
        //                obje_p.HRPA_UpdatedBy = dto.LogInUserId;
        //                obje_p.HRPA_CreatedBy = dto.LogInUserId;
        //                _HRMSContext.Add(obje_p);
        //                var existcount = _HRMSContext.SaveChanges();
        //                if (existcount >= 1)
        //                {

        //                    dto.HRPA_Id = obje_p.HRPA_Id;
        //                    if (dto.HRPA_Id > 0 && dto.IVRMUL_Id > 0)
        //                    {
        //                        HR_Process_Auth_OrderNo dtochild = new HR_Process_Auth_OrderNo();
        //                        dtochild.HRPA_Id = dto.HRPA_Id;
        //                        dtochild.HRPAON_Id = dto.HRPAON_Id;
        //                        dtochild.HRPAON_FinalFlg = dto.HRPAON_FinalFlg;
        //                        dtochild.HRPAON_SanctionLevelNo = dto.HRPAON_SanctionLevelNo;
        //                        dtochild.IVRMUL_Id = dto.IVRMUL_Id;
        //                        SaveUpdateProcess_Auth_OrderNo(dtochild);
        //                        dto.retrunMsg = "save";
        //                        dto.returnval = true;
        //                    }
        //                }
        //                else
        //                {
        //                    dto.retrunMsg = "Fail";
        //                    dto.returnval = false;
        //                }

        //            }
        //            else
        //            {
        //                var result_obje_p = _HRMSContext.HR_PROCESSDMO.Single(t => t.HRPA_Id == dto.HRPA_Id);
        //                dto.UpdatedDate = DateTime.Now;
        //                result_obje_p.HRPA_UpdatedBy = dto.LogInUserId;
        //                Mapper.Map(dto, result_obje_p);
        //                _HRMSContext.Update(result_obje_p);
        //                var existcount = _HRMSContext.SaveChanges();
        //                if (existcount >= 1)
        //                {
        //                    if (dto.HRPA_Id > 0 && dto.IVRMUL_Id > 0)
        //                    {
        //                        HR_Process_Auth_OrderNo dtochild = new HR_Process_Auth_OrderNo();
        //                        dtochild.HRPA_Id = dto.HRPA_Id;
        //                        dtochild.HRPAON_Id = dto.HRPAON_Id;
        //                        dtochild.HRPAON_FinalFlg = dto.HRPAON_FinalFlg;
        //                        dtochild.HRPAON_SanctionLevelNo = dto.HRPAON_SanctionLevelNo;
        //                        dtochild.IVRMUL_Id = dto.IVRMUL_Id;

        //                        SaveUpdateProcess_Auth_OrderNo(dtochild);
        //                        dto.retrunMsg = "save";
        //                        dto.returnval = true;
        //                    }
        //                }
        //                else
        //                {
        //                    dto.retrunMsg = "Fail";
        //                    dto.returnval = false;
        //                }

        //            }

        //        }

        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //        dto.retrunMsg = "Error occured";
        //    }

        //    return dto;
        //}

        public HR_ProcessDTO deleteauth(HR_ProcessDTO data)
        {
            HR_ProcessDTO page = new HR_ProcessDTO();
            try
            {
                List<HR_Process_Auth_OrderNoDMO> lorg = new List<HR_Process_Auth_OrderNoDMO>();
                lorg = _HRMSContext.HR_Process_Auth_OrderNoDMO.Where(t => t.HRPA_Id.Equals(data.HRPA_Id) && t.HRPAON_Id.Equals(data.HRPAON_Id)).ToList();
                if (lorg.Any())
                {
                    _HRMSContext.Remove(lorg.ElementAt(0));
                    var contactExists = _HRMSContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        page.returnval = true;

                        var checkresult = _HRMSContext.HR_Process_Auth_OrderNoDMO.Where(a => a.HRPA_Id == data.HRPA_Id).Count();
                        if (checkresult == 0)
                        {
                            try
                            {
                                Array details = _HRMSContext.HR_Process_Auth_OrderNoDMO.Where(a => a.HRPA_Id == data.HRPA_Id).ToArray();

                                if (details.Length > 0)
                                {
                                    foreach (var d in details)
                                    {
                                        _HRMSContext.Remove(d);
                                    }

                                    var contactExists1 = _HRMSContext.SaveChanges();
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                    else
                    {
                        page.returnval = false;
                    }
                }
                List<HR_PROCESSDMO> allpages = new List<HR_PROCESSDMO>();
                allpages = _HRMSContext.HR_PROCESSDMO.ToList();
                page.authData = allpages.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

    }


}



