
using DataAccessMsSqlServerProvider.com.vapstech.Placement;
using DomainModel.Model.com.vapstech.Placement;
using PlacementServiceHub.com.Interfaces;
using PreadmissionDTOs.com.vaps.Placement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlacementServiceHub.com.Services
{
    public class PL_CI_Schedule_Company_JobTitle_CriteriaImpl : Interfaces.PL_CI_Schedule_Company_JobTitle_CriteriaInterface
    {
        public PlacementContext _PlacementContext;
        public PL_CI_Schedule_Company_JobTitle_CriteriaImpl(PlacementContext PlacementContext)
        {
            _PlacementContext = PlacementContext;
        }
        public PL_CI_Schedule_Company_JobTitle_CriteriaDTO loaddata(int id)
        {
            PL_CI_Schedule_Company_JobTitle_CriteriaDTO dto = new PL_CI_Schedule_Company_JobTitle_CriteriaDTO();
            try
            {


            

                dto.jobtitlelist= _PlacementContext.PL_CI_Schedule_Company_JobTitleDMO.Distinct().ToArray();

                dto.course = (from a in _PlacementContext.mappingDMO
                              from b in _PlacementContext.MasterCourseDMO
                              where (b.AMCO_Id == a.AMCO_Id)
                              select new PL_CI_Schedule_Company_JobTitle_CriteriaDTO
                              {
                                  AMCO_Id = a.AMCO_Id,
                                  AMCO_CourseName=b.AMCO_CourseName



                              }).Distinct().ToArray();


         


                dto.save = (from a in _PlacementContext.PL_CI_Schedule_Company_JobTitle_CriteriaDMO
                            from b in _PlacementContext.PL_CI_Schedule_Company_JobTitleDMO
                            from c in _PlacementContext.mappingDMO
                            from d in _PlacementContext.MasterCourseDMO
                            where (a.PLCISCHCOMJT_Id == b.PLCISCHCOMJT_Id && a.AMCO_Id==c.AMCO_Id && a.AMCO_Id==d.AMCO_Id)
                            select new PL_CI_Schedule_Company_JobTitle_CriteriaDTO
                            {
                                PLCISCHCOMJTCR_Id = a.PLCISCHCOMJTCR_Id,
                                PLCISCHCOMJT_Id = b.PLCISCHCOMJT_Id,
                                PLMCLSMAP_Id = c.PLMCLSMAP_Id,
                                AMCO_Id = a.AMCO_Id,
                                PLCISCHCOMJTCR_CutOfMark =a.PLCISCHCOMJTCR_CutOfMark,
                                PLCISCHCOMJTCR_ActiveFlag = a.PLCISCHCOMJTCR_ActiveFlag,
                                PLCISCHCOMJT_JobTitle = b.PLCISCHCOMJT_JobTitle,
                                AMCO_CourseName = d.AMCO_CourseName
                            }).Distinct().ToArray();        
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return dto;
        }
        public PL_CI_Schedule_Company_JobTitle_CriteriaDTO saveRecord(PL_CI_Schedule_Company_JobTitle_CriteriaDTO data)
        {
            try
            {
                if (data.PLCISCHCOMJTCR_Id > 0)
                {
                    var res = _PlacementContext.PL_CI_Schedule_Company_JobTitle_CriteriaDMO.Where(t => t.AMCO_Id == data.AMCO_Id && t.PLCISCHCOMJT_Id==data.PLCISCHCOMJT_Id && t.PLCISCHCOMJTCR_CutOfMark==data.PLCISCHCOMJTCR_CutOfMark && t.PLCISCHCOMJTCR_OtherDetails==data.PLCISCHCOMJTCR_OtherDetails).ToList();
                    if (res.Count > 0)
                    {
                        data.returnval = "duplicate";
                    }
                    else
                    {
                        var result = _PlacementContext.PL_CI_Schedule_Company_JobTitle_CriteriaDMO.Single(t => t.PLCISCHCOMJTCR_Id == data.PLCISCHCOMJTCR_Id);
                        result.PLCISCHCOMJTCR_Id = data.PLCISCHCOMJTCR_Id;
                        result.PLCISCHCOMJT_Id = data.PLCISCHCOMJT_Id;
                        result.AMCO_Id = data.AMCO_Id;
                        result.PLCISCHCOMJTCR_CutOfMark = data.PLCISCHCOMJTCR_CutOfMark;
                        result.PLCISCHCOMJTCR_OtherDetails = data.PLCISCHCOMJTCR_OtherDetails;                       
                        result.PLCISCHCOMJTCR_ActiveFlag = true;
                        result.PLCISCHCOMJTCR_CreatedDate = DateTime.Now;
                        result.PLCISCHCOMJTCR_UpdatedDate = DateTime.Now;
                        result.PLCISCHCOMJTCR_CreatedBy = data.User_Id;
                        result.PLCISCHCOMJTCR_UpdatedBy = data.User_Id;
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
                    var res = _PlacementContext.PL_CI_Schedule_Company_JobTitle_CriteriaDMO.Where(t => t.AMCO_Id == data.AMCO_Id && t.PLCISCHCOMJT_Id == data.PLCISCHCOMJT_Id && t.PLCISCHCOMJTCR_CutOfMark == data.PLCISCHCOMJTCR_CutOfMark && t.PLCISCHCOMJTCR_OtherDetails==data.PLCISCHCOMJTCR_OtherDetails).ToList();
                    if (res.Count > 0)
                    {
                        data.returnval = "duplicate";
                    }
                    else
                    {
                        PL_CI_Schedule_Company_JobTitle_CriteriaDMO lvl = new PL_CI_Schedule_Company_JobTitle_CriteriaDMO();                    
                        lvl.PLCISCHCOMJT_Id = data.PLCISCHCOMJT_Id;
                        lvl.AMCO_Id = data.AMCO_Id;
                        lvl.PLCISCHCOMJTCR_CutOfMark = data.PLCISCHCOMJTCR_CutOfMark;
                        lvl.PLCISCHCOMJTCR_OtherDetails = data.PLCISCHCOMJTCR_OtherDetails;
                        lvl.PLCISCHCOMJTCR_ActiveFlag = true;
                        lvl.PLCISCHCOMJTCR_CreatedDate = DateTime.Now;
                        lvl.PLCISCHCOMJTCR_UpdatedDate = DateTime.Now;
                        lvl.PLCISCHCOMJTCR_CreatedBy = data.User_Id;
                        lvl.PLCISCHCOMJTCR_UpdatedBy = data.User_Id;
                        _PlacementContext.Add(lvl);
                        var contactExists = _PlacementContext.SaveChanges();                        if (contactExists > 0)                        {
                            data.returnval = "saved";
                        }
                        else
                        {
                            data.returnval = "savingFailed";                        }                    }                }            }            catch (Exception ex)            {                Console.WriteLine(ex.Message);                data.returnval = "admin";
            }            return data;                   }
        public PL_CI_Schedule_Company_JobTitle_CriteriaDTO EditDetails(PL_CI_Schedule_Company_JobTitle_CriteriaDTO dto)        {            try            {
                dto.EditDetails = _PlacementContext.PL_CI_Schedule_Company_JobTitle_CriteriaDMO.Where(d => d.PLCISCHCOMJTCR_Id == dto.PLCISCHCOMJTCR_Id).ToArray();              
            }            catch (Exception ee)            {                Console.WriteLine(ee.Message);            }            return dto;        }

        public PL_CI_Schedule_Company_JobTitle_CriteriaDTO deactivate(PL_CI_Schedule_Company_JobTitle_CriteriaDTO obj)
        {
            try
            {
                var query = _PlacementContext.PL_CI_Schedule_Company_JobTitle_CriteriaDMO.Single(d => d.PLCISCHCOMJTCR_Id == obj.PLCISCHCOMJTCR_Id);
                if (query.PLCISCHCOMJTCR_ActiveFlag == true)
                {
                    query.PLCISCHCOMJTCR_ActiveFlag = false;
                }
                else if (query.PLCISCHCOMJTCR_ActiveFlag == false)
                {
                    query.PLCISCHCOMJTCR_ActiveFlag = true;
                }
                query.PLCISCHCOMJTCR_UpdatedDate = DateTime.Now;
                _PlacementContext.Update(query);
                int s = _PlacementContext.SaveChanges();
                if (s > 0)
                {
                    obj.retval = true;
                }
                else
                {
                    obj.retval = false;
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
       