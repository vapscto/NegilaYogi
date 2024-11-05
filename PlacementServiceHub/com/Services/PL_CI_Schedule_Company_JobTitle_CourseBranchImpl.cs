using DataAccessMsSqlServerProvider.com.vapstech.Placement;
using DomainModel.Model.com.vapstech.Placement;
using PreadmissionDTOs.com.vaps.Placement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlacementServiceHub.com.Services
{
    public class PL_CI_Schedule_Company_JobTitle_CourseBranchImpl : Interfaces.PL_CI_Schedule_Company_JobTitle_CourseBranchInterface
    {
        public PlacementContext _PlacementContext;
        public PL_CI_Schedule_Company_JobTitle_CourseBranchImpl(PlacementContext PlacementContext)
        {
            _PlacementContext = PlacementContext;
        }
        public PL_CI_Schedule_Company_JobTitle_CourseBranchDTO loaddata(int id)
        {
            PL_CI_Schedule_Company_JobTitle_CourseBranchDTO dto = new PL_CI_Schedule_Company_JobTitle_CourseBranchDTO();
            try
            {
                dto.pages = _PlacementContext.PL_CI_Schedule_Company_JobTitleDMO.Distinct().ToArray();
                dto.course = _PlacementContext.MasterCourseDMO.Distinct().ToArray();
                dto.branch = _PlacementContext.ClgMasterBranchDMO.Distinct().ToArray();
                dto.save = (from a in _PlacementContext.PL_CI_Schedule_Company_JobTitle_CourseBranchDMO
                            from b in _PlacementContext.MasterCourseDMO
                            from c in _PlacementContext.ClgMasterBranchDMO
                            from d in _PlacementContext.PL_CI_Schedule_Company_JobTitleDMO
                            where (a.AMCO_Id == b.AMCO_Id && a.AMB_Id == c.AMB_Id && a.PLCISCHCOMJT_Id==d.PLCISCHCOMJT_Id)
                            select new PL_CI_Schedule_Company_JobTitle_CourseBranchDTO
                            {
                                PLCISCHCOMJTCB_Id = a.PLCISCHCOMJTCB_Id,
                                PLCISCHCOMJT_Id = d.PLCISCHCOMJT_Id,
                                AMCO_Id = b.AMCO_Id,
                                AMB_Id = c.AMB_Id,
                                AMB_BranchName=c.AMB_BranchName,
                                AMCO_CourseName=b.AMCO_CourseName,
                                PLCISCHCOMJT_JobTitle=d.PLCISCHCOMJT_JobTitle,
                                PLCISCHCOMJTCB_ApplicableSEM =a.PLCISCHCOMJTCB_ApplicableSEM,
                                PLCISCHCOMJTCB_ActiveFlag = a.PLCISCHCOMJTCB_ActiveFlag,                            
                            }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
       
    


        public PL_CI_Schedule_Company_JobTitle_CourseBranchDTO saveRecord(PL_CI_Schedule_Company_JobTitle_CourseBranchDTO data)
        {
            try
            {
                if (data.PLCISCHCOMJTCB_Id > 0)
                {
                    var res = _PlacementContext.PL_CI_Schedule_Company_JobTitle_CourseBranchDMO.Where(d => d.PLCISCHCOMJT_Id == data.PLCISCHCOMJT_Id && d.AMB_Id == data.AMB_Id && d.AMCO_Id == data.AMCO_Id && d.PLCISCHCOMJTCB_ApplicableSEM == data.PLCISCHCOMJTCB_ApplicableSEM).ToList();
                    if (res.Count > 0)
                    {
                        data.returnval = "duplicate";
                    }
                    else
                    {
                        var result = _PlacementContext.PL_CI_Schedule_Company_JobTitle_CourseBranchDMO.Single(d => d.PLCISCHCOMJTCB_Id == data.PLCISCHCOMJTCB_Id);
                        result.PLCISCHCOMJTCB_Id = data.PLCISCHCOMJTCB_Id;
                        result.PLCISCHCOMJT_Id = data.PLCISCHCOMJT_Id;
                        result.AMCO_Id = data.AMCO_Id;
                        result.AMB_Id = data.AMB_Id;
                        result.PLCISCHCOMJTCB_ApplicableSEM = data.PLCISCHCOMJTCB_ApplicableSEM;
                        result.PLCISCHCOMJTCB_ActiveFlag = true;
                        result.PLCISCHCOMJTCB_CreatedDate = DateTime.Now;
                        result.PLCISCHCOMJTCB_UpdatedDate = DateTime.Now;
                        result.PLCISCHCOMJTCB_CreatedBy = data.User_Id;
                        result.PLCISCHCOMJTCB_UpdatedBy = data.User_Id;
                        _PlacementContext.Update(result);
                        var contactExists = _PlacementContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = "updated";
                        }
                        else
                        {
                            data.returnval = "updateFailed";
                        }
                    }
                }
                else
                {
                    var res = _PlacementContext.PL_CI_Schedule_Company_JobTitle_CourseBranchDMO.Where(d => d.PLCISCHCOMJT_Id == data.PLCISCHCOMJT_Id && d.AMB_Id == data.AMB_Id && d.AMCO_Id == data.AMCO_Id && d.PLCISCHCOMJTCB_ApplicableSEM == data.PLCISCHCOMJTCB_ApplicableSEM).ToList();
                    if (res.Count > 0)
                    {
                        data.returnval = "duplicate";
                    }
                    else
                    {
                        PL_CI_Schedule_Company_JobTitle_CourseBranchDMO lvl = new PL_CI_Schedule_Company_JobTitle_CourseBranchDMO();
                        lvl.PLCISCHCOMJTCB_Id = data.PLCISCHCOMJTCB_Id;
                        lvl.PLCISCHCOMJT_Id = data.PLCISCHCOMJT_Id;
                        lvl.AMCO_Id = data.AMCO_Id;
                        lvl.AMB_Id = data.AMB_Id;
                        lvl.PLCISCHCOMJTCB_ApplicableSEM = data.PLCISCHCOMJTCB_ApplicableSEM;
                        lvl.PLCISCHCOMJTCB_ActiveFlag = true;
                        lvl.PLCISCHCOMJTCB_CreatedDate = DateTime.Now;
                        lvl.PLCISCHCOMJTCB_UpdatedDate = DateTime.Now;
                        lvl.PLCISCHCOMJTCB_CreatedBy = data.User_Id;
                        lvl.PLCISCHCOMJTCB_UpdatedBy = data.User_Id;
                        _PlacementContext.Add(lvl);
                        var contactExists = _PlacementContext.SaveChanges();                        if (contactExists > 0)                        {
                            data.returnval = "saved";
                        }
                        else
                        {
                            data.returnval = "savingFailed";                        }                    }                }            }            catch (Exception ex)            {                Console.WriteLine(ex.Message);                data.returnval = "admin";
            }            return data;
        }

        public PL_CI_Schedule_Company_JobTitle_CourseBranchDTO EditDetails(PL_CI_Schedule_Company_JobTitle_CourseBranchDTO dto)        {            try            {
                dto.EditDetails = _PlacementContext.PL_CI_Schedule_Company_JobTitle_CourseBranchDMO.Where(d => d.PLCISCHCOMJTCB_Id == dto.PLCISCHCOMJTCB_Id).ToArray();
            }            catch (Exception ee)            {                Console.WriteLine(ee.Message);            }            return dto;        }

        public PL_CI_Schedule_Company_JobTitle_CourseBranchDTO deactivate(PL_CI_Schedule_Company_JobTitle_CourseBranchDTO obj)
        {
            try
            {
                var query = _PlacementContext.PL_CI_Schedule_Company_JobTitle_CourseBranchDMO.Single(d => d.PLCISCHCOMJTCB_Id == obj.PLCISCHCOMJTCB_Id);
                if (query.PLCISCHCOMJTCB_ActiveFlag == true)
                {
                    query.PLCISCHCOMJTCB_ActiveFlag = false;
                }
                else if (query.PLCISCHCOMJTCB_ActiveFlag == false)
                {
                    query.PLCISCHCOMJTCB_ActiveFlag = true;
                }

                query.PLCISCHCOMJTCB_UpdatedDate = DateTime.Now;
                _PlacementContext.Update(query);
                int s = _PlacementContext.SaveChanges();
                if (s > 0)
                {
                    obj.retval = "true";
                }
                else
                {
                    obj.retval = "false";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }
    }
}
