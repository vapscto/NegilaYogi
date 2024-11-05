using DataAccessMsSqlServerProvider.com.vapstech.Placement;
using DomainModel.Model.com.vapstech.Placement;
using PreadmissionDTOs.com.vaps.Placement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlacementServiceHub.com.Services
{
    public class PlacementJobScheduleTitleImp : Interfaces.PlacementJobScheduleTitleInterface
    {
        public PlacementContext _context;
        public PlacementJobScheduleTitleImp(PlacementContext _cont)
        {
            _context = _cont;
        }
        public PlacementJobScheduleTitleDTO loaddata(PlacementJobScheduleTitleDTO data)
        {
            try
            {

                data.shiftrole = (from a in _context.applicationRole
                                  where (a.Id == 7) //data.roleid admin : 4 and Student : 7
                                  select new PlacementJobScheduleTitleDTO
                                  {
                                      flag = a.Name
                                  }).ToArray();

                

                data.scompany = (from a in _context.PL_Master_CompanyDMO
                                 from b in _context.PL_CI_Schedule_CompanyDMO
                                 from c in _context.PL_CI_Schedule_Company_JobTitleDMO
                                 where (b.PLMCOMP_Id == a.PLMCOMP_Id && c.PLCISCHCOM_Id == b.PLCISCHCOM_Id)
                                 select new PlacementJobScheduleTitleDTO
                                 {
                                     companyschedulename = (a.PLMCOMP_CompanyName + '-' + b.PLCISCHCOM_JobDetails +'-'+ c.PLCISCHCOMJT_JobTitle),
                                     primarycompanyid = c.PLCISCHCOMJT_Id
                                 }).Distinct().ToArray();




                

                data.scourse = (from a in _context.MasterCourseDMO
                                where (a.MI_Id == data.MI_Id )
                                select new PlacementJobScheduleTitleDTO
                                {
                                    schedulecourse = a.AMCO_CourseName,
                                    idschedulecourse = a.AMCO_Id
                                }).Distinct().ToArray();



               

                data.sbranch = (from a in _context.ClgMasterBranchDMO
                                where (a.MI_Id == data.MI_Id)
                                select new PlacementJobScheduleTitleDTO
                                {
                                    schedulebranch = a.AMB_BranchName,
                                    idschedulebranch = a.AMB_Id
                                }).Distinct().ToArray();


                data.schedulestudentname = (from a in _context.Adm_Master_College_StudentDMO
                                            where (a.MI_Id == data.MI_Id)
                                            select new PlacementJobScheduleTitleDTO
                                            {
                                                studentname = a.AMCST_MiddleName,
                                                idschedulestudentname = a.AMCST_Id
                                            }).Distinct().ToArray();


                data.schedulestudentnames = (from a in _context.Adm_Master_College_StudentDMO
                                            where (a.MI_Id == data.MI_Id)
                                            select new PlacementJobScheduleTitleDTO
                                            {
                                                studentnames = a.AMCST_MiddleName,
                                                idschedulestudentnames = a.AMCST_Id
                                            }).Distinct().ToArray();


                // Student Grid

                data.studentgridtable = (from a in _context.PL_Master_CompanyDMO
                                         from b in _context.PL_CI_Schedule_CompanyDMO
                                         from c in _context.PL_CI_Schedule_Company_JobTitleDMO
                                         from d in _context.PL_CI_Schedule_Company_JobTitle_StudentsDMO
                                         from e in _context.Adm_Master_College_StudentDMO
                                         where (b.PLMCOMP_Id == a.PLMCOMP_Id && c.PLCISCHCOM_Id == b.PLCISCHCOM_Id && d.PLCISCHCOMJT_Id == c.PLCISCHCOMJT_Id && e.AMCST_Id == d.AMCST_Id && a.MI_Id == data.MI_Id && e.MI_Id == data.MI_Id)
                                         select new PlacementJobScheduleTitleDTO
                                         {
                                             csname = a.PLMCOMP_CompanyName,
                                             smname = e.AMCST_MiddleName,
                                             PLCISCHCOMJT_JobTitle = c.PLCISCHCOMJT_JobTitle,
                                             PLCISCHCOMJT_QulaificationCriteria = c.PLCISCHCOMJT_QulaificationCriteria,
                                             PLCISCHCOMJT_OtherDetails = c.PLCISCHCOMJT_OtherDetails,
                                             PLCISCHCOMJTST_Id = d.PLCISCHCOMJTST_Id,
                                             PLCISCHCOMJTST_ActiveFlag = d.PLCISCHCOMJTST_ActiveFlag
                                         }).Distinct().ToArray();


                data.registration = _context.PL_CI_Schedule_Company_JobTitle_StudentsDMO.Select(R => R.PLCISCHCOMJTST_Id).Count();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public PlacementJobScheduleTitleDTO savedetails(PlacementJobScheduleTitleDTO data)
        {
            try
            {
             
                if (data.PLCISCHCOMJTST_Id != 0)
                {

                    var result = _context.PL_CI_Schedule_Company_JobTitle_StudentsDMO.Single(t => t.PLCISCHCOMJTST_Id == data.PLCISCHCOMJTST_Id);

                    
                    result.PLCISCHCOMJT_Id = data.PLCISCHCOMJT_Id;
                    result.AMCST_Id = data.AMCST_Id;
                    result.PLCISCHCOMJTST_ActiveFlag = true;
                    result.PLCISCHCOMJTST_UpdatedDate = DateTime.Now;
                    result.PLCISCHCOMJTST_UpdatedBy = data.UserId;
                   


                    _context.Update(result);

                    var contactExists = _context.SaveChanges();
                    if (contactExists > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }

                else
                {
                    var res = _context.PL_CI_Schedule_Company_JobTitle_StudentsDMO.Where(t => (t.PLCISCHCOMJTST_Id == data.PLCISCHCOMJTST_Id)).ToList();

                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        PL_CI_Schedule_Company_JobTitle_StudentsDMO tax = new PL_CI_Schedule_Company_JobTitle_StudentsDMO();

                        tax.PLCISCHCOMJT_Id = data.PLCISCHCOMJT_Id;
                        tax.AMCST_Id = data.AMCST_Id;
                        tax.PLCISCHCOMJTST_Date = data.fromdate;
                        tax.PLCISCHCOMJTST_ActiveFlag = true;
                        tax.PLCISCHCOMJTST_CreatedDate = DateTime.Now;
                        tax.PLCISCHCOMJTST_CreatedBy = data.UserId;
                        tax.PLCISCHCOMJTST_UpdatedDate = DateTime.Now;
                        tax.PLCISCHCOMJTST_UpdatedBy = data.UserId;


                        _context.Add(tax);

                        var contactExists = _context.SaveChanges();
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public PlacementJobScheduleTitleDTO editdetails(PlacementJobScheduleTitleDTO dto)
        {
            try
            {
               

                dto.editdata = (from a in _context.PL_Master_CompanyDMO
                                         from b in _context.PL_CI_Schedule_CompanyDMO
                                         from c in _context.PL_CI_Schedule_Company_JobTitleDMO
                                         from d in _context.PL_CI_Schedule_Company_JobTitle_StudentsDMO
                                         from e in _context.Adm_Master_College_StudentDMO
                                         where (b.PLMCOMP_Id == a.PLMCOMP_Id && c.PLCISCHCOM_Id == b.PLCISCHCOM_Id && d.PLCISCHCOMJT_Id == c.PLCISCHCOMJT_Id && e.AMCST_Id == d.AMCST_Id && a.MI_Id == dto.MI_Id && e.MI_Id == dto.MI_Id && d.PLCISCHCOMJTST_Id == dto.PLCISCHCOMJTST_Id)
                                         select new PlacementJobScheduleTitleDTO
                                         {
                                             PLMCOMP_CompanyName = a.PLMCOMP_CompanyName,
                                             PLCISCHCOMJT_Id = c.PLCISCHCOMJT_Id,
                                             AMCST_MiddleName = e.AMCST_MiddleName,
                                             AMCST_Id = e.AMCST_Id,
                                             PLCISCHCOMJTST_Date = d.PLCISCHCOMJTST_Date,
                                             PLCISCHCOMJTST_Id = d.PLCISCHCOMJTST_Id
                                         }).Distinct().ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }

            return dto;
        }

        public PlacementJobScheduleTitleDTO deactive(PlacementJobScheduleTitleDTO data)
        {
            try
            {
                var result = _context.PL_CI_Schedule_Company_JobTitle_StudentsDMO.Single(t => t.PLCISCHCOMJTST_Id == data.PLCISCHCOMJTST_Id);

                if (result.PLCISCHCOMJTST_ActiveFlag == true)
                {
                    result.PLCISCHCOMJTST_ActiveFlag = false;
                }
                else if (result.PLCISCHCOMJTST_ActiveFlag == false)
                {
                    result.PLCISCHCOMJTST_ActiveFlag = true;
                }
                result.PLCISCHCOMJTST_UpdatedDate = DateTime.Now;
                _context.Update(result);
                int returnval = _context.SaveChanges();
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

        public PlacementJobScheduleTitleDTO report(PlacementJobScheduleTitleDTO data)
        {
            try
            {
                if (data.grid_arraydatasamb != null && data.grid_arraydatasamb.Length > 0)
                {
                    List<long> amcoidname = new List<long>();
                    foreach (var c in data.grid_arraydatassamco)
                    {
                        amcoidname.Add(c.amcoid);
                    }
                    List<long> ambidname = new List<long>();
                    foreach (var c in data.grid_arraydatasamb)
                    {
                        ambidname.Add(c.ambid);
                    }
                    List<long> sidname = new List<long>();
                    foreach (var c in data.grid_arraydatasss)
                    {
                        sidname.Add(c.sid);
                    }

                    data.admingridtable = (from a in _context.PL_Master_CompanyDMO
                                           from b in _context.PL_CI_Schedule_CompanyDMO
                                           from c in _context.PL_CI_Schedule_Company_JobTitleDMO
                                           from d in _context.PL_CI_Schedule_Company_JobTitle_StudentsDMO
                                           from e in _context.Adm_Master_College_StudentDMO
                                           from f in _context.MasterCourseDMO
                                           from g in _context.ClgMasterBranchDMO
                                           where (b.PLMCOMP_Id == a.PLMCOMP_Id && c.PLCISCHCOM_Id == b.PLCISCHCOM_Id && d.PLCISCHCOMJT_Id == c.PLCISCHCOMJT_Id && e.AMCST_Id == d.AMCST_Id && f.AMCO_Id == e.AMCO_Id && g.AMB_Id == e.AMB_Id && amcoidname.Contains(e.AMCO_Id) && ambidname.Contains(e.AMB_Id) && sidname.Contains(e.AMCST_Id) && a.MI_Id == data.MI_Id && e.MI_Id == data.MI_Id)
                                           select new PlacementJobScheduleTitleDTO
                                           {
                                               PLMCOMP_CompanyName = a.PLMCOMP_CompanyName,
                                               AMCST_MiddleName = e.AMCST_MiddleName,
                                               PLCISCHCOMJT_JobTitle = c.PLCISCHCOMJT_JobTitle,
                                               PLCISCHCOMJT_QulaificationCriteria = c.PLCISCHCOMJT_QulaificationCriteria,
                                               PLCISCHCOMJT_OtherDetails = c.PLCISCHCOMJT_OtherDetails

                                           }).Distinct().ToArray();


                 }

                    

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }




    }
}
