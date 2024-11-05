using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
using DomainModel.Model.com.vapstech.Inventory;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Implementation
{
    public class ISM_ClientProject_MappingIMPL : Interface.ISM_ClientProject_MappingInterface
    {
        public InventoryContext _INVContext;
        public ISM_ClientProject_MappingIMPL(InventoryContext hh)
        {
            _INVContext = hh;
        }
        //====================================loaddata ===========================================
        public ISM_ClientProject_MappingDTO loaddata(ISM_ClientProject_MappingDTO data)
        {
            try
            {
                data.client_list = _INVContext.clientTable.Where(t => t.MI_Id == data.MI_Id && t.ISMMCLT_ActiveFlag == true).Distinct().ToArray();

                data.project_list = _INVContext.MastersProject_DMO.Where(t => t.MI_Id == data.MI_Id && t.ISMMPR_ActiveFlg == true).Distinct().ToArray();

                data.get_clentlist = (from a in _INVContext.clientTable
                                      from b in _INVContext.MastersProject_DMO
                                      from c in _INVContext.MastersModule_DMO
                                      from d in _INVContext.ISM_Master_Client_ProjectDMO
                                      from e in _INVContext.MasterModules
                                      where (a.ISMMCLT_Id == d.ISMMCLT_Id && b.ISMMPR_Id == d.ISMMPR_Id && c.ISMMMD_Id == d.ISMMMD_Id && c.IVRMM_Id == e.IVRMM_Id && a.MI_Id == data.MI_Id)
                                      select new ISM_ClientProject_MappingDTO
                                      {
                                          ISMMCLTPR_Id = d.ISMMCLTPR_Id,
                                          ISMMCLT_ClientName = a.ISMMCLT_ClientName,
                                          ISMMPR_ProjectName = b.ISMMPR_ProjectName,
                                          IVRMM_ModuleName = e.IVRMM_ModuleName,
                                          ISMMCLT_Id = d.ISMMCLT_Id,
                                          ISMMPR_Id = d.ISMMPR_Id,
                                          ISMMMD_Id = d.ISMMMD_Id,
                                          ISMMCLTPR_ActiveFlag = d.ISMMCLTPR_ActiveFlag,
                                      }).Distinct().OrderByDescending(t => t.ISMMCLTPR_Id).ToArray();

                //data.get_department = (from a in _INVContext.HR_Master_Department
                //                       from b in _INVContext.MasterEmployee
                //                       where (a.HRMD_Id == b.HRMD_Id && a.MI_Id == b.MI_Id && a.HRMD_ActiveFlag == true && b.HRME_ActiveFlag == true && a.MI_Id == data.MI_Id)
                //                       select new ISM_TaskCreationDTO
                //                       {
                //                           HRMD_Id = a.HRMD_Id,
                //                           HRMD_DepartmentName = a.HRMD_DepartmentName
                //                       }).Distinct().ToArray();

                data.get_department = (from a in _INVContext.HR_Master_Department
                                       where (a.HRMD_ActiveFlag == true && a.MI_Id == data.MI_Id)
                                       select new ISM_ClientProject_MappingDTO
                                       {
                                           HRMD_Id = a.HRMD_Id,
                                           HRMD_DepartmentName = a.HRMD_DepartmentName
                                       }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //==============================================project list======================================//
        public ISM_ClientProject_MappingDTO getproject(ISM_ClientProject_MappingDTO data)
        {
            try
            {
                data.project_list = (from a in _INVContext.clientTable
                                     from b in _INVContext.MastersProject_DMO
                                     from c in _INVContext.MastersModule_DMO
                                     where (a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.ISMMCLT_ActiveFlag == true && b.ISMMPR_ActiveFlg == true && b.ISMMPR_Id == c.ISMMPR_Id && c.HRMD_Id == data.HRMD_Id)
                                     select new ISM_ClientProject_MappingDTO
                                     {
                                         ISMMPR_ProjectName = b.ISMMPR_ProjectName,
                                         ISMMPR_Id = b.ISMMPR_Id
                                     }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        //=================================module==================================================
        public ISM_ClientProject_MappingDTO getmodule(ISM_ClientProject_MappingDTO data)
        {
            try
            {
                data.module_list = (from a in _INVContext.MasterModules
                                    from b in _INVContext.MastersModule_DMO
                                    from c in _INVContext.MastersProject_DMO
                                    where (a.IVRMM_Id == b.IVRMM_Id && b.ISMMPR_Id == c.ISMMPR_Id && b.MI_Id == c.MI_Id
                                    && a.Module_ActiveFlag == 1 && b.ISMMMD_ActiveFlag == true && c.ISMMPR_ActiveFlg == true && b.ISMMPR_Id == data.ISMMPR_Id && b.HRMD_Id == data.HRMD_Id)
                                    select new ISM_ClientProject_MappingDTO
                                    {
                                        IVRMM_Id = b.IVRMM_Id,
                                        ISMMMD_Id = b.ISMMMD_Id,
                                        IVRMM_ModuleName = a.IVRMM_ModuleName,
                                    }).Distinct().OrderBy(b => b.IVRMM_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //==========================================save data=======================================
        public ISM_ClientProject_MappingDTO savedata(ISM_ClientProject_MappingDTO data)
        {
            try
            {
                if (data.ISMMCLTPR_Id == 0)
                {
                    for (int i = 0; i < data.moduleidss.Length; i++)
                    {

                        var tempdata = data.moduleidss[i].ISMMMD_Id;
                        {
                            //var duplicate = _INVContext.ISM_Master_Client_ProjectDMO.Where(t => t.ISMMCLTPR_Id != 0 && t.ISMMCLT_Id == data.ISMMCLT_Id && t.ISMMPR_Id == data.ISMMPR_Id && t.ISMMCLTPR_MOURefNo==data.ISMMCLTPR_MOURefNo && t.ISMMCLTPR_ProposalRefNo==data.ISMMCLTPR_ProposalRefNo && t.ISMMMD_Id==tempdata && t.ISMMCLTPR_NodalOfficerName==data.ISMMCLTPR_NodalOfficerName).ToArray();
                            var duplicate = _INVContext.ISM_Master_Client_ProjectDMO.Where(t => t.ISMMCLTPR_Id != 0 && t.ISMMCLT_Id == data.ISMMCLT_Id && t.ISMMPR_Id == data.ISMMPR_Id && t.ISMMMD_Id == tempdata).ToArray();

                            if (duplicate.Count() > 0)
                            {
                                data.duplicate = true;
                            }
                            else
                            {
                                ISM_Master_Client_ProjectDMO rrr = new ISM_Master_Client_ProjectDMO();
                                rrr.ISMMCLT_Id = data.ISMMCLT_Id;
                                rrr.ISMMPR_Id = data.ISMMPR_Id;
                                rrr.ISMMCLTPR_Id = data.ISMMCLTPR_Id;
                                rrr.ISMMMD_Id = tempdata;
                                rrr.ISMMCLTPR_ProposalRefNo = data.ISMMCLTPR_ProposalRefNo;
                                rrr.ISMMCLTPR_ProposalSentDate = data.ISMMCLTPR_ProposalSentDate;
                                rrr.ISMMCLTPR_DealClosureDate = data.ISMMCLTPR_DealClosureDate;
                                rrr.ISMMCLTPR_MOURefNo = data.ISMMCLTPR_MOURefNo;
                                rrr.ISMMCLTPR_MOUDate = data.ISMMCLTPR_MOUDate;
                                rrr.ISMMCLTPR_MOURepresentedBy = data.ISMMCLTPR_MOURepresentedBy;
                                rrr.ISMMCLTPR_MOUStartDate = data.ISMMCLTPR_MOUStartDate;
                                rrr.ISMMCLTPR_MOUEndDate = data.ISMMCLTPR_MOUEndDate;
                                rrr.ISMMCLTPR_NodalOfficerName = data.ISMMCLTPR_NodalOfficerName;
                                rrr.ISMMCLTPR_NodalOfficerContactNo = data.ISMMCLTPR_NodalOfficerContactNo;
                                rrr.ISMMCLTPR_NodalOfficerEmailId = data.ISMMCLTPR_NodalOfficerEmailId;
                                rrr.ISMMCLTPR_ProjectDuration = data.ISMMCLTPR_ProjectDuration;
                                rrr.ISMMCLTPR_TotalStudent = data.ISMMCLTPR_TotalStudent;
                                rrr.ISMMCLTPR_CostPerStudent = data.ISMMCLTPR_CostPerStudent;
                                rrr.ISMMCLTPR_EnhancementPerYr = data.ISMMCLTPR_EnhancementPerYr;
                                rrr.CreatedDate = DateTime.Now;
                                rrr.UpdatedDate = DateTime.Now;
                                rrr.ISMMCLTPR_ActiveFlag = true;
                                rrr.ISMMCLTPR_CreatedBy = data.UserId;
                                rrr.ISMMCLTPR_UpdatedBy = data.UserId;
                                //rrr.ISMMCLTPR_WorkOrder = data.ISMMCLTPR_WorkOrder;
                                _INVContext.Add(rrr);
                                int y = _INVContext.SaveChanges();
                                if (y > 0)
                                {
                                    data.msg = "saved";
                                }
                                else
                                {
                                    data.msg = "Failed";
                                }
                            }
                        }
                    }
                }
                else if (data.ISMMCLTPR_Id > 0)
                {
                    for (int i = 0; i < data.moduleidss.Length; i++)
                    {
                        var tempdata = data.moduleidss[i].ISMMMD_Id;
                        {
                            var duplicate = _INVContext.ISM_Master_Client_ProjectDMO.Where(t => t.ISMMCLT_Id == data.ISMMCLT_Id && t.ISMMPR_Id == data.ISMMPR_Id && t.ISMMMD_Id == tempdata && t.ISMMCLTPR_Id != data.ISMMCLTPR_Id).ToArray();
                            if (duplicate.Count() > 0)
                            {
                                data.duplicate = true;
                            }
                            else
                            {
                                var yy = _INVContext.ISM_Master_Client_ProjectDMO.Where(t => t.ISMMCLTPR_Id == data.ISMMCLTPR_Id).SingleOrDefault();
                                yy.ISMMCLTPR_CreatedBy = data.UserId;
                                yy.ISMMCLTPR_UpdatedBy = data.UserId;
                                yy.ISMMCLT_Id = data.ISMMCLT_Id;
                                yy.ISMMPR_Id = data.ISMMPR_Id;
                                yy.ISMMMD_Id = tempdata;
                                yy.CreatedDate = DateTime.Now;
                                yy.UpdatedDate = DateTime.Now;
                                yy.ISMMCLTPR_Id = data.ISMMCLTPR_Id;
                                yy.ISMMCLTPR_ProposalRefNo = data.ISMMCLTPR_ProposalRefNo;
                                yy.ISMMCLTPR_ProposalSentDate = data.ISMMCLTPR_ProposalSentDate;
                                yy.ISMMCLTPR_DealClosureDate = data.ISMMCLTPR_DealClosureDate;
                                yy.ISMMCLTPR_MOURefNo = data.ISMMCLTPR_MOURefNo;
                                yy.ISMMCLTPR_MOUDate = data.ISMMCLTPR_MOUDate;
                                yy.ISMMCLTPR_MOURepresentedBy = data.ISMMCLTPR_MOURepresentedBy;
                                yy.ISMMCLTPR_MOUStartDate = data.ISMMCLTPR_MOUStartDate;
                                yy.ISMMCLTPR_MOUEndDate = data.ISMMCLTPR_MOUEndDate;
                                yy.ISMMCLTPR_NodalOfficerName = data.ISMMCLTPR_NodalOfficerName;
                                yy.ISMMCLTPR_NodalOfficerContactNo = data.ISMMCLTPR_NodalOfficerContactNo;
                                yy.ISMMCLTPR_NodalOfficerEmailId = data.ISMMCLTPR_NodalOfficerEmailId;
                                yy.ISMMCLTPR_ProjectDuration = data.ISMMCLTPR_ProjectDuration;
                                yy.ISMMCLTPR_TotalStudent = data.ISMMCLTPR_TotalStudent;
                                yy.ISMMCLTPR_CostPerStudent = data.ISMMCLTPR_CostPerStudent;
                                yy.ISMMCLTPR_EnhancementPerYr = data.ISMMCLTPR_EnhancementPerYr;
                                //yy.ISMMCLTPR_WorkOrder = data.ISMMCLTPR_WorkOrder;
                                _INVContext.Update(yy);
                                int r = _INVContext.SaveChanges();
                                if (r > 0)
                                {
                                    data.msg = "updated";
                                }
                                else
                                {
                                    data.msg = "failed";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        //==========================================edit data======================================
        public ISM_ClientProject_MappingDTO Editdata(ISM_ClientProject_MappingDTO data)
        {
            try
            {
                var editlist = (from a in _INVContext.MasterModules
                                from b in _INVContext.MastersModule_DMO
                                from c in _INVContext.ISM_Master_Client_ProjectDMO
                                where (c.ISMMCLTPR_Id == data.ISMMCLTPR_Id && a.IVRMM_Id == b.IVRMM_Id && b.ISMMMD_Id == c.ISMMMD_Id)
                                select new ISM_ClientProject_MappingDTO
                                {
                                    ISMMCLTPR_Id = c.ISMMCLTPR_Id,
                                    ISMMCLT_Id = c.ISMMCLT_Id,
                                    ISMMPR_Id = c.ISMMPR_Id,
                                    IVRMM_Id = b.IVRMM_Id,
                                    ISMMMD_Id = b.ISMMMD_Id,
                                    ISMMCLTPR_ProposalRefNo = c.ISMMCLTPR_ProposalRefNo,
                                    ISMMCLTPR_ProposalSentDate = c.ISMMCLTPR_ProposalSentDate,
                                    ISMMCLTPR_DealClosureDate = c.ISMMCLTPR_DealClosureDate,
                                    ISMMCLTPR_MOURefNo = c.ISMMCLTPR_MOURefNo,
                                    ISMMCLTPR_MOUDate = c.ISMMCLTPR_MOUDate,
                                    ISMMCLTPR_MOURepresentedBy = c.ISMMCLTPR_MOURepresentedBy,
                                    ISMMCLTPR_MOUStartDate = c.ISMMCLTPR_MOUStartDate,
                                    ISMMCLTPR_MOUEndDate = c.ISMMCLTPR_MOUEndDate,
                                    ISMMCLTPR_NodalOfficerName = c.ISMMCLTPR_NodalOfficerName,
                                    ISMMCLTPR_NodalOfficerContactNo = c.ISMMCLTPR_NodalOfficerContactNo,
                                    ISMMCLTPR_NodalOfficerEmailId = c.ISMMCLTPR_NodalOfficerEmailId,
                                    ISMMCLTPR_ProjectDuration = c.ISMMCLTPR_ProjectDuration,
                                    ISMMCLTPR_TotalStudent = c.ISMMCLTPR_TotalStudent,
                                    ISMMCLTPR_CostPerStudent = c.ISMMCLTPR_CostPerStudent,
                                    ISMMCLTPR_EnhancementPerYr = c.ISMMCLTPR_EnhancementPerYr,
                                    HRMD_Id = b.HRMD_Id,
                                    //ISMMCLTPR_WorkOrder = c.ISMMCLTPR_WorkOrder

                                }).Distinct().ToList();

                data.editlist = editlist.ToArray();
                if (editlist[0].ISMMMD_Id != 0)
                {
                    var ee = (from a in _INVContext.ISM_Master_Client_ProjectDMO
                              from b in _INVContext.MasterModules
                              where (a.ISMMCLTPR_Id == editlist[0].ISMMCLTPR_Id && b.IVRMM_Id == editlist[0].IVRMM_Id && a.ISMMMD_Id == editlist[0].ISMMMD_Id)
                              select new ISM_ClientProject_MappingDTO
                              {
                                  ISMMPR_Id = a.ISMMPR_Id,
                                  ISMMCLT_Id = a.ISMMCLT_Id,
                                  IVRMM_Id = b.IVRMM_Id,
                                  ISMMMD_Id = a.ISMMMD_Id

                              }).Distinct().ToList();
                    data.ISMMCLT_Id = ee[0].ISMMCLT_Id;
                    data.ISMMPR_Id = ee[0].ISMMPR_Id;
                    data.ISMMMD_Id = ee[0].ISMMMD_Id;
                    data.IVRMM_Id = ee[0].IVRMM_Id;
                    data.HRMD_Id = editlist[0].HRMD_Id;
                    data.client_list = (from a in _INVContext.clientTable
                                        where (a.MI_Id == data.MI_Id && a.ISMMCLT_ActiveFlag == true)
                                        select new ISM_ClientProject_MappingDTO
                                        {
                                            ISMMCLT_Id = a.ISMMCLT_Id,
                                            ISMMCLT_ClientName = a.ISMMCLT_ClientName,
                                        }
                                       ).Distinct().ToArray();

                    data.project_list = (from a in _INVContext.clientTable
                                         from b in _INVContext.MastersProject_DMO
                                         from c in _INVContext.MastersModule_DMO
                                         where (a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.ISMMCLT_ActiveFlag == true && b.ISMMPR_ActiveFlg == true && b.ISMMPR_Id == c.ISMMPR_Id && c.HRMD_Id == data.HRMD_Id)
                                         select new ISM_ClientProject_MappingDTO
                                         {
                                             ISMMPR_ProjectName = b.ISMMPR_ProjectName,
                                             ISMMPR_Id = b.ISMMPR_Id
                                         }).Distinct().ToArray();

                    data.module_list = (from a in _INVContext.MasterModules
                                        from b in _INVContext.MastersModule_DMO
                                        from c in _INVContext.MastersProject_DMO
                                        where (a.IVRMM_Id == b.IVRMM_Id && b.ISMMPR_Id == c.ISMMPR_Id && b.MI_Id == c.MI_Id
                                        && a.Module_ActiveFlag == 1 && b.ISMMMD_ActiveFlag == true && c.ISMMPR_ActiveFlg == true && b.ISMMPR_Id == data.ISMMPR_Id && b.HRMD_Id == data.HRMD_Id)
                                        select new ISM_ClientProject_MappingDTO
                                        {
                                            IVRMM_Id = b.IVRMM_Id,
                                            ISMMMD_Id = b.ISMMMD_Id,
                                            IVRMM_ModuleName = a.IVRMM_ModuleName,
                                        }).Distinct().OrderBy(b => b.IVRMM_Id).ToArray();
                    data.ISMMCLT_Id = ee[0].ISMMCLT_Id;
                    data.ISMMPR_Id = ee[0].ISMMPR_Id;
                    data.ISMMMD_Id = ee[0].ISMMMD_Id;
                    data.IVRMM_Id = ee[0].IVRMM_Id;
                    data.HRMD_Id = ee[0].HRMD_Id;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        //==========================================Deactive data=====================================
        public ISM_ClientProject_MappingDTO clientDecative(ISM_ClientProject_MappingDTO data)
        {
            try
            {
                var u = _INVContext.ISM_Master_Client_ProjectDMO.Where(t => t.ISMMCLTPR_Id == data.ISMMCLTPR_Id).SingleOrDefault();
                if (u.ISMMCLTPR_ActiveFlag == true)
                {
                    u.ISMMCLTPR_ActiveFlag = false;
                }
                else if (u.ISMMCLTPR_ActiveFlag == false)
                {
                    u.ISMMCLTPR_ActiveFlag = true;
                }

                _INVContext.Update(u);
                int o = _INVContext.SaveChanges();
                if (o > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
    }
}
