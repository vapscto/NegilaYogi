using DataAccessMsSqlServerProvider.com.vapstech.Placement;
using DomainModel.Model.com.vapstech.Placement;
using PreadmissionDTOs.com.vaps.Placement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlacementServiceHub.com.Services
{
    public class semmarkIMPL : Interfaces.semmarkInterface
    {
        public PlacementContext _PlacementContext;
        public semmarkIMPL(PlacementContext PlacementContext)
        {
            _PlacementContext = PlacementContext;
        }
        //load data
        public semmarkDTO loaddata(int id)
        {

            semmarkDTO dto = new semmarkDTO();
            try
            {
                dto.getsavedata = _PlacementContext.CLG_Adm_Master_SemesterDMO.Distinct().ToArray();
               //dto.jobtitlelist = _PlacementContext.PL_CI_Schedule_Company_JobTitleDMO.Distinct().ToArray();

                dto.jobtitlelist = (from a in _PlacementContext.PL_CI_Schedule_Company_JobTitleDMO
                                    from b in _PlacementContext.PL_CI_Schedule_Company_JobTitle_CourseBranchDMO
                                    from c in _PlacementContext.ClgMasterBranchDMO
                                    from d in _PlacementContext.MasterCourseDMO

                                    where (a.PLCISCHCOMJT_Id == b.PLCISCHCOMJT_Id && b.AMB_Id == c.AMB_Id && b.AMCO_Id == d.AMCO_Id)
                                    select new semmarkDTO
                                    {
                                       
                                        PLCISCHCOMJT_JobTitle = a.PLCISCHCOMJT_JobTitle + '-'+ c.AMB_BranchName + '-' + d.AMCO_CourseName,


            }).Distinct().ToArray();


                dto.save = (from a in _PlacementContext.semmarkDMO
                            from b in _PlacementContext.PL_CI_Schedule_Company_JobTitleDMO
                            from c in _PlacementContext.CLG_Adm_Master_SemesterDMO

                            where (a.PLCISCHCOMJT_Id == b.PLCISCHCOMJT_Id && a.AMSE_Id == c.AMSE_Id )
                            select new semmarkDTO
                            {
                                PLCISCHCOMJT_Id = b.PLCISCHCOMJT_Id,
                                AMSE_Id = c.AMSE_Id,
                                PLCISCHCOMJTSEM_CutOfMarks = a.PLCISCHCOMJTSEM_CutOfMarks,
                                PLCISCHCOMJTSEM_OtherDetails = a.PLCISCHCOMJTSEM_OtherDetails,
                                PLCISCHCOMJTSEM_ActiveFlag = a.PLCISCHCOMJTSEM_ActiveFlag,
                                PLCISCHCOMJT_JobTitle = b.PLCISCHCOMJT_JobTitle,
                                AMSE_SEMName = c.AMSE_SEMName,
                                PLCISCHCOMJTSEM_Id = a.PLCISCHCOMJTSEM_Id


                            }).Distinct().ToArray();

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
               
            }
            return dto;

        }

        //savedata
        public semmarkDTO savedata(semmarkDTO data)
        {
            try
            {
                if (data.PLCISCHCOMJTSEM_Id != 0)
                {
                    var res = _PlacementContext.semmarkDMO.Where(t => t.PLCISCHCOMJTSEM_CutOfMarks == data.PLCISCHCOMJTSEM_CutOfMarks && t.PLCISCHCOMJTSEM_OtherDetails == data.PLCISCHCOMJTSEM_OtherDetails).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _PlacementContext.semmarkDMO.Single(t => t.PLCISCHCOMJTSEM_Id == data.PLCISCHCOMJTSEM_Id);
                        result.PLCISCHCOMJT_Id = data.PLCISCHCOMJT_Id;
                        result.AMSE_Id = data.AMSE_Id;
                        result.PLCISCHCOMJTSEM_CutOfMarks = data.PLCISCHCOMJTSEM_CutOfMarks;
                        result.PLCISCHCOMJTSEM_OtherDetails = data.PLCISCHCOMJTSEM_OtherDetails;
                        result.PLCISCHCOMJTSEM_CreatedDate = DateTime.Now;
                        result.PLCISCHCOMJTSEM_UpdatedDate = DateTime.Now;
                        result.PLCISCHCOMJTSEM_CreatedBy = data.PLCISCHCOMJTSEM_CreatedBy;
                        result.PLCISCHCOMJTSEM_UpdatedBy = data.PLCISCHCOMJTSEM_UpdatedBy;
                        result.PLCISCHCOMJTSEM_ActiveFlag = true;
                        _PlacementContext.Update(result);

                        var contactExists = _PlacementContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = "update";
                        }
                        else
                        {
                            data.returnval = "Notupdate";
                        }
                    }
                }
                else
                {
                    var res = _PlacementContext.semmarkDMO.Where(t => t.PLCISCHCOMJTSEM_CutOfMarks == data.PLCISCHCOMJTSEM_CutOfMarks && t.PLCISCHCOMJTSEM_OtherDetails == data.PLCISCHCOMJTSEM_OtherDetails).ToList();

                    if (res.Count > 0)
                    {
                        data.returnval = "Duplicate";
                    }
                    else
                    {
                        semmarkDMO an = new semmarkDMO();

                        an.PLCISCHCOMJT_Id = data.PLCISCHCOMJT_Id;
                        an.AMSE_Id = data.AMSE_Id;
                        an.PLCISCHCOMJTSEM_CutOfMarks = data.PLCISCHCOMJTSEM_CutOfMarks;
                        an.PLCISCHCOMJTSEM_OtherDetails = data.PLCISCHCOMJTSEM_OtherDetails;
                        an.PLCISCHCOMJTSEM_CreatedDate = DateTime.Now;
                        an.PLCISCHCOMJTSEM_UpdatedDate = DateTime.Now;
                        an.PLCISCHCOMJTSEM_CreatedBy = data.User_Id;
                        an.PLCISCHCOMJTSEM_UpdatedBy = data.User_Id;
                        an.PLCISCHCOMJTSEM_ActiveFlag = true;
                        _PlacementContext.Add(an);

                        var contactExists = _PlacementContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = "saved";
                        }
                        else
                        {
                            data.returnval = "notsaved";
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }

        //edit
        public semmarkDTO edit(semmarkDTO dto)        {            try            {
                dto.editdata = _PlacementContext.semmarkDMO.Where(t => t.PLCISCHCOMJTSEM_Id == dto.PLCISCHCOMJTSEM_Id).ToArray();
                         }            catch (Exception ee)            {                Console.WriteLine(ee.Message);            }            return dto;        }

        //deactive
        public semmarkDTO deactive(semmarkDTO data)
        {
            try
            {
                var result = _PlacementContext.semmarkDMO.Single(t => t.PLCISCHCOMJTSEM_Id == data.PLCISCHCOMJTSEM_Id);

                if (result.PLCISCHCOMJTSEM_ActiveFlag == true)
                {
                    result.PLCISCHCOMJTSEM_ActiveFlag = false;
                }
                else if (result.PLCISCHCOMJTSEM_ActiveFlag == false)
                {
                    result.PLCISCHCOMJTSEM_ActiveFlag = true;
                }

                _PlacementContext.Update(result);
                int returnval = _PlacementContext.SaveChanges();
                if (returnval > 0)
                {
                    data.returnval = "true";
                }
                else
                {
                    data.returnval = "false";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }

    }
}
