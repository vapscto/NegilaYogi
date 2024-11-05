
using DataAccessMsSqlServerProvider.com.vapstech.Placement;
using DomainModel.Model.com.vapstech.Placement;
using PreadmissionDTOs.com.vaps.Placement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlacementServiceHub.com.Services
{
    public class PL_CI_Schedule_Company_JobTitleImpl : Interfaces.PL_CI_Schedule_Company_JobTitleInterface
    {
        public PlacementContext _PlacementContext;
        public PL_CI_Schedule_Company_JobTitleImpl(PlacementContext PlacementContext)
        {
            _PlacementContext = PlacementContext;
        }
        public PL_CI_Schedule_Company_JobTitleDTO loaddata(int id)
        {
            PL_CI_Schedule_Company_JobTitleDTO dto = new PL_CI_Schedule_Company_JobTitleDTO();
            try
            {
                dto.pages = (from b in _PlacementContext.PL_CI_Schedule_CompanyDMO
                             from c in _PlacementContext.PL_Master_CompanyDMO
                             where (b.PLMCOMP_Id == c.PLMCOMP_Id)
                             select new PL_CI_Schedule_Company_JobTitleDTO
                             {
                                 PLMCOMP_Id = c.PLMCOMP_Id,
                                 PLMCOMP_CompanyName = c.PLMCOMP_CompanyName,
                                 PLCISCHCOM_Id = b.PLCISCHCOM_Id,
                             }).Distinct().ToArray();

               // dto.course = _PlacementContext.MasterCourseDMO.Distinct().ToArray();

                dto.save = (from a in _PlacementContext.PL_CI_Schedule_Company_JobTitleDMO
                            from b in _PlacementContext.PL_CI_Schedule_CompanyDMO
                            from c in _PlacementContext.PL_Master_CompanyDMO
                            where (a.PLCISCHCOM_Id == b.PLCISCHCOM_Id && b.PLMCOMP_Id == c.PLMCOMP_Id)
                            select new PL_CI_Schedule_Company_JobTitleDTO
                            {
                                PLMCOMP_Id = c.PLMCOMP_Id,
                                PLCISCHCOM_Id = a.PLCISCHCOM_Id,
                                PLCISCHCOMJT_JobTitle=a.PLCISCHCOMJT_JobTitle,
                                PLCISCHCOMJT_QulaificationCriteria=a.PLCISCHCOMJT_QulaificationCriteria,
                                PLCISCHCOMJT_NoOfInterviewRounds = a.PLCISCHCOMJT_NoOfInterviewRounds,
                                PLCISCHCOMJT_ActiveFlag=a.PLCISCHCOMJT_ActiveFlag,
                                PLMCOMP_CompanyName=c.PLMCOMP_CompanyName,
                                PLCISCHCOMJT_Id = a.PLCISCHCOMJT_Id
                            }).Distinct().ToArray();             
            }            
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
      
        public PL_CI_Schedule_Company_JobTitleDTO saveRecord(PL_CI_Schedule_Company_JobTitleDTO data)
        {
            try
            {
                if (data.PLCISCHCOMJT_Id > 0)
                {
                    var checkduplicate = _PlacementContext.PL_CI_Schedule_Company_JobTitleDMO.Where(t =>t.PLCISCHCOM_Id == data.PLCISCHCOM_Id && t.PLCISCHCOMJT_JobTitle == data.PLCISCHCOMJT_JobTitle && t.PLCISCHCOMJT_NoOfInterviewRounds == data.PLCISCHCOMJT_NoOfInterviewRounds && t.PLCISCHCOMJT_Id == data.PLCISCHCOMJT_Id && t.PLCISCHCOMJT_QulaificationCriteria==data.PLCISCHCOMJT_QulaificationCriteria && t.PLCISCHCOMJT_OtherDetails==data.PLCISCHCOMJT_OtherDetails).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        data.returnval = "duplicate";
                    }
                    else
                    {
                        var result = _PlacementContext.PL_CI_Schedule_Company_JobTitleDMO.Single(t => t.PLCISCHCOMJT_Id == data.PLCISCHCOMJT_Id);
                        result.PLCISCHCOM_Id = data.PLCISCHCOM_Id;                       
                        result.PLCISCHCOMJT_JobTitle = data.PLCISCHCOMJT_JobTitle;
                        result.PLCISCHCOMJT_QulaificationCriteria = data.PLCISCHCOMJT_QulaificationCriteria;
                        result.PLCISCHCOMJT_NoOfInterviewRounds = data.PLCISCHCOMJT_NoOfInterviewRounds;
                        result.PLCISCHCOMJT_OtherDetails = data.PLCISCHCOMJT_OtherDetails;
                        result.PLCISCHCOMJT_ActiveFlag = true;
                        result.PLCISCHCOMJT_CreatedDate = DateTime.Now;
                        result.PLCISCHCOMJT_UpdatedDate = DateTime.Now;
                        result.PLCISCHCOMJT_CreatedBy = data.User_Id;
                        result.PLCISCHCOMJT_UpdatedBy = data.User_Id;
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
                    var checkduplicate = _PlacementContext.PL_CI_Schedule_Company_JobTitleDMO.Where(t => t.PLCISCHCOM_Id == data.PLCISCHCOM_Id && t.PLCISCHCOMJT_JobTitle == data.PLCISCHCOMJT_JobTitle && t.PLCISCHCOMJT_NoOfInterviewRounds == data.PLCISCHCOMJT_NoOfInterviewRounds && t.PLCISCHCOMJT_QulaificationCriteria==data.PLCISCHCOMJT_QulaificationCriteria && t.PLCISCHCOMJT_OtherDetails==data.PLCISCHCOMJT_OtherDetails).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        data.returnval = "duplicate";
                    }
                    else
                    {
                        PL_CI_Schedule_Company_JobTitleDMO lvl = new PL_CI_Schedule_Company_JobTitleDMO();
                        lvl.PLCISCHCOM_Id = data.PLCISCHCOM_Id;
                        lvl.PLCISCHCOMJT_JobTitle = data.PLCISCHCOMJT_JobTitle;
                        lvl.PLCISCHCOMJT_QulaificationCriteria = data.PLCISCHCOMJT_QulaificationCriteria;
                        lvl.PLCISCHCOMJT_NoOfInterviewRounds = data.PLCISCHCOMJT_NoOfInterviewRounds;
                        lvl.PLCISCHCOMJT_OtherDetails = data.PLCISCHCOMJT_OtherDetails;
                        lvl.PLCISCHCOMJT_ActiveFlag = true;
                        lvl.PLCISCHCOMJT_CreatedDate = DateTime.Now;
                        lvl.PLCISCHCOMJT_UpdatedDate = DateTime.Now;
                        lvl.PLCISCHCOMJT_CreatedBy = data.User_Id;
                        lvl.PLCISCHCOMJT_UpdatedBy = data.User_Id;
                        _PlacementContext.Add(lvl);
                        var contactExists = _PlacementContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = "saved";
                        }
                        else
                        {
                            data.returnval = "savingFailed";
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
        public PL_CI_Schedule_Company_JobTitleDTO EditDetails(PL_CI_Schedule_Company_JobTitleDTO dto)
        {
            try
            {
                dto.EditDetails = _PlacementContext.PL_CI_Schedule_Company_JobTitleDMO.Where(d => d.PLCISCHCOMJT_Id == dto.PLCISCHCOMJT_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public PL_CI_Schedule_Company_JobTitleDTO deactivate(PL_CI_Schedule_Company_JobTitleDTO obj)
        {
            try
            {
                var query = _PlacementContext.PL_CI_Schedule_Company_JobTitleDMO.Single(d => d.PLCISCHCOMJT_Id == obj.PLCISCHCOMJT_Id);
                if (query.PLCISCHCOMJT_ActiveFlag == true)
                {
                    query.PLCISCHCOMJT_ActiveFlag = false;
                }
                else if (query.PLCISCHCOMJT_ActiveFlag == false)
                {
                    query.PLCISCHCOMJT_ActiveFlag = true;
                }
                query.PLCISCHCOMJT_UpdatedDate = DateTime.Now;
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
