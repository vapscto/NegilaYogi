using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Admission;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Impl
{
    public class ClgSeatDistributionImpl : Interface.ClgSeatDistributionInterface
    {
        public ClgAdmissionContext _ClgAdmissionContext;
        ILogger<ClgSeatDistributionImpl> _logSeat;
        public ClgSeatDistributionImpl(ClgAdmissionContext ClgAdmissionContext, ILogger<ClgSeatDistributionImpl> log)
        {
            _ClgAdmissionContext = ClgAdmissionContext;
            _logSeat = log;
        }
        public ClgSeatDistributionDTO getalldetails(ClgSeatDistributionDTO data)
        {
            try
            {
                data.getYear = _ClgAdmissionContext.AcademicYear.Where(a => a.MI_Id.Equals(data.MI_Id)).ToArray();

                data.getSeatsdetails = (from a in _ClgAdmissionContext.Clg_Adm_College_Seat_DistributionDMO
                                        from b in _ClgAdmissionContext.ClgMasterBranchDMO
                                        from c in _ClgAdmissionContext.MasterCourseDMO
                                        from d in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                                        from e in _ClgAdmissionContext.Clg_Adm_College_QuotaDMO
                                        from f in _ClgAdmissionContext.Clg_Adm_College_Quota_CategoryDMO

                                        where (a.AMCO_Id == c.AMCO_Id && a.AMB_Id == b.AMB_Id && a.AMSE_Id == d.AMSE_Id && a.ACQ_Id == e.ACQ_Id && a.ACQC_Id == f.ACQC_Id && a.MI_Id == b.MI_Id && c.MI_Id == d.MI_Id && e.MI_Id == f.MI_Id && a.MI_Id == data.MI_Id)
                                        select new ClgSeatDistributionDTO
                                        {
                                            ACSCD_Id = a.ACSCD_Id,
                                            AMCO_CourseName = c.AMCO_CourseName,
                                            AMB_BranchName = b.AMB_BranchName,
                                            AMSE_SEMName = d.AMSE_SEMName,
                                            ACQ_QuotaName = e.ACQ_QuotaName,
                                            ACQC_CategoryName = f.ACQC_CategoryName,
                                            ACSCD_SeatPer = a.ACSCD_SeatPer,
                                            ACSCD_SeatNos = a.ACSCD_SeatNos
                                        }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                _logSeat.LogInformation("Seat distribution getalldetails :" + ex.Message);
            }
            return data;
        }

        public ClgSeatDistributionDTO getCoursedata(ClgSeatDistributionDTO data)
        {
            try
            {
                data.getCourse = (from a in _ClgAdmissionContext.MasterCourseDMO
                                  from b in _ClgAdmissionContext.CLG_Adm_College_AY_CourseDMO
                                  from c in _ClgAdmissionContext.AcademicYear
                                  where (a.AMCO_Id == b.AMCO_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id)
                                  select new ClgSeatDistributionDTO
                                  {
                                      AMCO_Id = a.AMCO_Id,
                                      AMCO_CourseName = a.AMCO_CourseName,
                                      AMCO_CourseCode = a.AMCO_CourseCode
                                  }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _logSeat.LogInformation("Seat Distribution getCoursedata :" + ex.Message);
            }
            return data;
        }

        public ClgSeatDistributionDTO getBranchdata(ClgSeatDistributionDTO data)
        {
            try
            {
                data.getBranch = (from a in _ClgAdmissionContext.ClgMasterBranchDMO
                                  from b in _ClgAdmissionContext.CLG_Adm_College_AY_CourseDMO
                                  from c in _ClgAdmissionContext.CLG_Adm_College_AY_Course_BranchDMO
                                  from d in _ClgAdmissionContext.AcademicYear
                                  where (a.AMB_Id == c.AMB_Id && b.ACAYC_Id == c.ACAYC_Id && b.ASMAY_Id == d.ASMAY_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && b.AMCO_Id == data.AMCO_Id)
                                  select new ClgSeatDistributionDTO
                                  {
                                      AMB_Id = a.AMB_Id,
                                      AMB_BranchName = a.AMB_BranchName,
                                      AMB_BranchCode = a.AMB_BranchCode,
                                      AMB_BranchType = a.AMB_BranchType,
                                      AMB_AidedUnAided = a.AMB_AidedUnAided,
                                  }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _logSeat.LogInformation("Seat distribution  getBranchdata :" + ex.Message);
            }
            return data;
        }
        public ClgSeatDistributionDTO getSemesterdata(ClgSeatDistributionDTO data)
        {
            try
            {
                data.getBranchDetails = (from a in _ClgAdmissionContext.ClgMasterBranchDMO
                                         from b in _ClgAdmissionContext.CLG_Adm_College_AY_CourseDMO
                                         from c in _ClgAdmissionContext.CLG_Adm_College_AY_Course_BranchDMO
                                         from d in _ClgAdmissionContext.AcademicYear
                                         where (a.AMB_Id == c.AMB_Id && b.ACAYC_Id == c.ACAYC_Id && b.ASMAY_Id == d.ASMAY_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && b.AMCO_Id == data.AMCO_Id && c.AMB_Id==data.AMB_Id)
                                         select new ClgSeatDistributionDTO
                                         {
                                             AMB_Id = a.AMB_Id,
                                             AMB_BranchName = a.AMB_BranchName,
                                             AMB_BranchCode = a.AMB_BranchCode,
                                             AMB_BranchType = a.AMB_BranchType,
                                             AMB_StudentCapacity = a.AMB_StudentCapacity,
                                             AMB_AidedUnAided = a.AMB_AidedUnAided
                                         }).Distinct().ToArray();

                //var get_Seat_ID = _ClgAdmissionContext.Clg_Adm_College_Seat_DistributionDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMB_Id == data.AMB_Id && a.AMCO_Id == data.AMCO_Id).ToList();

                //List<long> AMSE_Ids = new List<long>();
                //if (get_Seat_ID.Count > 0)
                //{
                //    foreach (var sem_ID in get_Seat_ID)
                //    {
                //        AMSE_Ids.Add(sem_ID.AMSE_Id);
                //    }
                //    data.getSemester = (from a in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                //                        from b in _ClgAdmissionContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                //                        from c in _ClgAdmissionContext.Clg_Adm_College_Seat_DistributionDMO

                //                        where (b.AMSE_Id == a.AMSE_Id && a.MI_Id == data.MI_Id && !AMSE_Ids.Contains(a.AMSE_Id))
                //                        select new ClgSeatDistributionDTO
                //                        {
                //                            AMSE_Id = a.AMSE_Id,
                //                            AMSE_SEMName = a.AMSE_SEMName,
                //                            AMSE_SEMCode = a.AMSE_SEMCode,
                //                            AMSE_EvenOdd = a.AMSE_EvenOdd
                //                        }).Distinct().ToArray();
                //}
                //else
                //{
                data.getSemester = (from a in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                                    from b in _ClgAdmissionContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                    from c in _ClgAdmissionContext.CLG_Adm_College_AY_CourseDMO
                                    from d in _ClgAdmissionContext.CLG_Adm_College_AY_Course_BranchDMO
                                    where (b.AMSE_Id == a.AMSE_Id && a.MI_Id == data.MI_Id && c.ACAYC_Id==d.ACAYC_Id && b.ACAYCB_Id==d.ACAYCB_Id && c.AMCO_Id==data.AMCO_Id && d.AMB_Id==data.AMB_Id && c.ASMAY_Id==data.ASMAY_Id)
                                    select new ClgSeatDistributionDTO
                                    {
                                        AMSE_Id = a.AMSE_Id,
                                        AMSE_SEMName = a.AMSE_SEMName,
                                        AMSE_SEMCode = a.AMSE_SEMCode,
                                        AMSE_EvenOdd = a.AMSE_EvenOdd
                                    }).Distinct().ToArray();
                //}

            }
            catch (Exception ex)
            {
                _logSeat.LogInformation("Seat Distribution getSemesterdata :" + ex.Message);
            }
            return data;
        }

        public ClgSeatDistributionDTO get_Category(ClgSeatDistributionDTO data)
        {
            try
            {

                data.getSeattotal = (from a in _ClgAdmissionContext.Clg_Adm_College_Seat_DistributionDMO
                                     from b in _ClgAdmissionContext.ClgMasterBranchDMO
                                     from c in _ClgAdmissionContext.MasterCourseDMO
                                     from d in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                                     from e in _ClgAdmissionContext.Clg_Adm_College_QuotaDMO
                                     from f in _ClgAdmissionContext.Clg_Adm_College_Quota_CategoryDMO

                                     where (a.AMCO_Id == c.AMCO_Id && a.AMB_Id == b.AMB_Id && a.AMSE_Id == d.AMSE_Id && a.ACQ_Id == e.ACQ_Id && a.ACQC_Id == f.ACQC_Id && a.MI_Id == b.MI_Id && c.MI_Id == d.MI_Id && e.MI_Id == f.MI_Id && a.MI_Id == data.MI_Id)
                                     select new ClgSeatDistributionDTO
                                     {
                                         ACSCD_SeatNos = a.ACSCD_SeatNos
                                     }).Distinct().ToArray();

                data.getSeatCategory = (from a in _ClgAdmissionContext.Clg_Adm_College_QuotaDMO
                                        from b in _ClgAdmissionContext.Clg_Adm_College_Quota_CategoryDMO
                                        from c in _ClgAdmissionContext.Clg_Adm_College_Quota_Category_MappingDMO
                                        where (a.ACQ_Id == c.ACQ_Id && b.ACQC_Id == c.ACQC_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id)
                                        select new ClgSeatDistributionDTO
                                        {
                                            ACQ_Id = a.ACQ_Id,
                                            ACQC_Id = b.ACQC_Id,
                                            ACQ_QuotaName = a.ACQ_QuotaName,
                                            ACQC_CategoryName = b.ACQC_CategoryName

                                        }).Distinct().ToArray();


                var check_count = _ClgAdmissionContext.Clg_Adm_College_Seat_DistributionDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCO_Id == data.AMCO_Id
                  && a.AMB_Id == data.AMB_Id && a.ASMAY_Id == data.ASMAY_Id).ToList();
                if (check_count.Count > 0)
                {
                    data.getSeatsdetails1 = (from a in _ClgAdmissionContext.Clg_Adm_College_Seat_DistributionDMO
                                             from b in _ClgAdmissionContext.ClgMasterBranchDMO
                                             from c in _ClgAdmissionContext.MasterCourseDMO
                                             from d in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                                             from e in _ClgAdmissionContext.Clg_Adm_College_QuotaDMO
                                             from f in _ClgAdmissionContext.Clg_Adm_College_Quota_CategoryDMO

                                             where (a.AMCO_Id == c.AMCO_Id && a.AMB_Id == b.AMB_Id && a.AMSE_Id == d.AMSE_Id && a.ACQ_Id == e.ACQ_Id && a.ACQC_Id == f.ACQC_Id && a.MI_Id == b.MI_Id && c.MI_Id == d.MI_Id && e.MI_Id == f.MI_Id && a.MI_Id == data.MI_Id)
                                             select new ClgSeatDistributionDTO
                                             {
                                                 ACQ_Id = a.ACQ_Id,
                                                 ACQC_Id = a.ACQC_Id,
                                                 ACSCD_Id = a.ACSCD_Id,
                                                 AMCO_CourseName = c.AMCO_CourseName,
                                                 AMB_BranchName = b.AMB_BranchName,
                                                 AMSE_SEMName = d.AMSE_SEMName,
                                                 ACQ_QuotaName = e.ACQ_QuotaName,
                                                 ACQC_CategoryName = f.ACQC_CategoryName,
                                                 ACSCD_SeatPer = a.ACSCD_SeatPer,
                                                 ACSCD_SeatNos = a.ACSCD_SeatNos,
                                                 ACSCD_Remarks = a.ACSCD_Remarks
                                             }).Distinct().ToArray();

                }




            }
            catch (Exception ex)
            {
                _logSeat.LogInformation("Seat Distribution  Get category :" + ex.Message);
            }
            return data;
        }

        public ClgSeatDistributionDTO get_Seattotal(ClgSeatDistributionDTO data)
        {
            try
            {
                data.getSeattotal = (from a in _ClgAdmissionContext.Clg_Adm_College_Seat_DistributionDMO
                                     from b in _ClgAdmissionContext.ClgMasterBranchDMO
                                     from c in _ClgAdmissionContext.MasterCourseDMO
                                     from d in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                                     from e in _ClgAdmissionContext.Clg_Adm_College_QuotaDMO
                                     from f in _ClgAdmissionContext.Clg_Adm_College_Quota_CategoryDMO

                                     where (a.AMCO_Id == c.AMCO_Id && a.AMB_Id == b.AMB_Id && a.AMSE_Id == d.AMSE_Id && a.ACQ_Id == e.ACQ_Id && a.ACQC_Id == f.ACQC_Id && a.MI_Id == b.MI_Id && c.MI_Id == d.MI_Id && e.MI_Id == f.MI_Id && a.MI_Id == data.MI_Id)
                                     select new ClgSeatDistributionDTO
                                     {
                                         ACSCD_SeatNos = a.ACSCD_SeatNos
                                     }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _logSeat.LogInformation("Seat Distribution  Get category :" + ex.Message);
            }
            return data;
        }
        public ClgSeatDistributionDTO savedata(ClgSeatDistributionDTO data)
        {
            try
            {
                foreach (var sem in data.AMSE_Sem)
                {
                    foreach (var x in data.seat_data)
                    {
                        //var res = _ClgAdmissionContext.Clg_Adm_College_Seat_DistributionDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMB_Id == data.AMB_Id && t.AMSE_Id == sem && t.ACQ_Id == x.ACQ_Id && t.ACQC_Id == x.ACQC_Id 
                        //&& t.ACSCD_SeatPer == x.ACSCD_SeatPer && t.ACSCD_SeatNos == x.ACSCD_SeatNos && t.ACSCD_Remarks == x.ACSCD_Remarks).ToList();

                        var res = _ClgAdmissionContext.Clg_Adm_College_Seat_DistributionDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMB_Id == data.AMB_Id && t.AMSE_Id == sem && t.ACQ_Id == x.ACQ_Id && t.ACQC_Id == x.ACQC_Id).ToList();


                        if (res.Count > 0)
                        {
                            var result = _ClgAdmissionContext.Clg_Adm_College_Seat_DistributionDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMB_Id == data.AMB_Id && t.AMSE_Id == sem && t.ACQ_Id == x.ACQ_Id && t.ACQC_Id == x.ACQC_Id && t.ACSCD_Id == res.FirstOrDefault().ACSCD_Id);

                            result.ACSCD_SeatPer = x.ACSCD_SeatPer;
                            result.ACSCD_SeatNos = x.ACSCD_SeatNos;
                            result.ACSCD_Remarks = x.ACSCD_Remarks;
                            result.UpdatedDate = DateTime.Now;
                            _ClgAdmissionContext.Update(result);
                            //data.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {
                            Clg_Adm_College_Seat_DistributionDMO seatObj = new Clg_Adm_College_Seat_DistributionDMO();

                            seatObj.MI_Id = data.MI_Id;
                            seatObj.ASMAY_Id = data.ASMAY_Id;
                            seatObj.AMCO_Id = data.AMCO_Id;
                            seatObj.AMB_Id = data.AMB_Id;
                            seatObj.AMSE_Id = sem;
                            seatObj.ACQ_Id = x.ACQ_Id;
                            seatObj.ACQC_Id = x.ACQC_Id;
                            seatObj.ACSCD_SeatPer = x.ACSCD_SeatPer;
                            seatObj.ACSCD_SeatNos = x.ACSCD_SeatNos;
                            seatObj.ACSCD_Remarks = x.ACSCD_Remarks;
                            seatObj.ACSCD_ActiveFlg = true;
                            seatObj.UpdatedDate = DateTime.Now;
                            seatObj.CreatedDate = DateTime.Now;
                            seatObj.UpdatedDate = DateTime.Now;
                            _ClgAdmissionContext.Add(seatObj);
                        }
                    }
                }
                var contactExists = _ClgAdmissionContext.SaveChanges();
                if (contactExists > 0)
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
                _logSeat.LogInformation("Seat Distribution savedata :" + ex.Message);
            }
            return data;
        }


        //master competitive exam 
        public Master_Competitive_AdmExamsClgDTO getexamdetails(Master_Competitive_AdmExamsClgDTO obj)
        {

            try
            {
                obj.examdetailsarray = (from a in _ClgAdmissionContext.Master_Competitive_AdmExamsClgDMO
                                        where (a.AMCEXM_ActiveFlg == true && a.MI_Id == obj.MI_Id)
                                        select new Master_Competitive_AdmExamsClgDTO
                                        {
                                            AMCEXM_Id = a.AMCEXM_Id,
                                            AMCEXM_CompetitiveExams = a.AMCEXM_CompetitiveExams,
                                            AMCEXM_CompulsoryFlg = a.AMCEXM_CompulsoryFlg
                                        }).Distinct().ToArray();
                obj.subdetailsarray = (from a in _ClgAdmissionContext.Master_CompetitiveExamsSubjectsAdmClgDMO
                                       from b in _ClgAdmissionContext.Master_Competitive_AdmExamsClgDMO
                                       where (a.AMCEXM_Id == b.AMCEXM_Id && b.AMCEXM_ActiveFlg == true && b.MI_Id == obj.MI_Id && a.AMCEXMSUB_ActiveFlg == true)
                                       select new Master_Competitive_AdmExamsClgDTO
                                       {
                                           AMCEXMSUB_Id = a.AMCEXMSUB_Id,
                                           AMCEXMSUB_SubjectName = a.AMCEXMSUB_SubjectName,
                                           AMCEXM_CompetitiveExams = b.AMCEXM_CompetitiveExams,
                                           AMCEXMSUB_MaxMarks = a.AMCEXMSUB_MaxMarks
                                       }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return obj;
        }

        public Master_Competitive_AdmExamsClgDTO saveExamDetails(Master_Competitive_AdmExamsClgDTO page)
        {
            bool returnresult = false;
            page.returnMsg = "";
            try
            {
                Master_Competitive_AdmExamsClgDMO maspge = Mapper.Map<Master_Competitive_AdmExamsClgDMO>(page);

                if (page.AMCEXM_Id > 0)
                {
                    var resultCount = _ClgAdmissionContext.Master_Competitive_AdmExamsClgDMO.Where(t => t.AMCEXM_CompetitiveExams == maspge.AMCEXM_CompetitiveExams && t.AMCEXM_Id != page.AMCEXM_Id && t.MI_Id == page.MI_Id).Count();

                    if (resultCount == 0)
                    {
                        var result = _ClgAdmissionContext.Master_Competitive_AdmExamsClgDMO.Single(t => t.AMCEXM_Id == maspge.AMCEXM_Id);

                        result.AMCEXM_CompetitiveExams = maspge.AMCEXM_CompetitiveExams;
                        result.AMCEXM_CompulsoryFlg = maspge.AMCEXM_CompulsoryFlg;
                        result.AMCEXM_ActiveFlg = true;


                        //added by 02/02/2017

                        result.AMCEXM_UpdatedDate = DateTime.Now;
                        _ClgAdmissionContext.Update(result);
                        var contactExists = _ClgAdmissionContext.SaveChanges();

                        if (contactExists == 1)
                        {
                            returnresult = true;
                            page.returnval = returnresult;
                            page.returnMsg = "update";
                        }
                        else
                        {
                            returnresult = false;
                            page.returnval = returnresult;
                        }
                    }
                    else
                    {
                        page.returnMsg = "duplicate";
                        return page;
                    }
                }
                else
                {
                    var resultCount = _ClgAdmissionContext.Master_Competitive_AdmExamsClgDMO.Where(t => t.AMCEXM_CompetitiveExams == maspge.AMCEXM_CompetitiveExams && t.AMCEXM_ActiveFlg==true && t.MI_Id== maspge.MI_Id).Count();

                    if (resultCount > 0)
                    {
                        page.returnMsg = "duplicate";
                        return page;
                    }
                    //added by 02/02/2017
                    maspge.AMCEXM_CreatedDate = DateTime.Now;
                    maspge.AMCEXM_UpdatedDate = DateTime.Now;
                    maspge.AMCEXM_ActiveFlg = true;
                    _ClgAdmissionContext.Add(maspge);
                    var contactExists = _ClgAdmissionContext.SaveChanges();

                    if (contactExists == 1)
                    {
                        returnresult = true;
                        page.returnval = returnresult;
                        page.returnMsg = "add";
                    }
                    else
                    {
                        returnresult = false;
                        page.returnval = returnresult;
                    }
                }

                List<Master_Competitive_AdmExamsClgDMO> allpages = new List<Master_Competitive_AdmExamsClgDMO>();
                allpages = _ClgAdmissionContext.Master_Competitive_AdmExamsClgDMO.ToList();
                page.pagesdata = allpages.OrderByDescending(c => c.AMCEXM_CreatedDate).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return page;
        }


        public Master_Competitive_AdmExamsClgDTO saveExamMapDetails(Master_Competitive_AdmExamsClgDTO page)
        {
            bool returnresult = false;
            page.returnMsg = "";
            try
            {
                Master_CompetitiveExamsSubjectsAdmClgDMO maspge = Mapper.Map<Master_CompetitiveExamsSubjectsAdmClgDMO>(page);

                if (page.AMCEXMSUB_Id > 0)
                {
                    var resultCount = _ClgAdmissionContext.Master_CompetitiveExamsSubjectsAdmClgDMO.Where(t => t.AMCEXMSUB_SubjectName == maspge.AMCEXMSUB_SubjectName && t.AMCEXM_Id != page.AMCEXM_Id && t.AMCEXMSUB_ActiveFlg == true ).Count();

                    if (resultCount == 0)
                    {

                        var result = _ClgAdmissionContext.Master_CompetitiveExamsSubjectsAdmClgDMO.Single(t => t.AMCEXMSUB_Id == maspge.AMCEXMSUB_Id);

                        result.AMCEXMSUB_SubjectName = maspge.AMCEXMSUB_SubjectName;
                        result.AMCEXM_Id = maspge.AMCEXM_Id;
                        result.AMCEXMSUB_MaxMarks = maspge.AMCEXMSUB_MaxMarks;
                        result.AMCEXMSUB_ActiveFlg = true;
                        result.AMCEXMSUB_UpdatedDate = DateTime.Now;
                        _ClgAdmissionContext.Update(result);
                        var contactExists = _ClgAdmissionContext.SaveChanges();

                        if (contactExists == 1)
                        {
                            returnresult = true;
                            page.returnval = returnresult;
                            page.returnMsg = "update";
                        }
                        else
                        {
                            returnresult = false;
                            page.returnval = returnresult;
                        }
                    }
                    else
                    {
                        page.returnMsg = "duplicate";
                        return page;
                    }
                }
                else
                {
                    var resultCount = _ClgAdmissionContext.Master_CompetitiveExamsSubjectsAdmClgDMO.Where(t => t.AMCEXMSUB_SubjectName == maspge.AMCEXMSUB_SubjectName && t.AMCEXM_Id == maspge.AMCEXM_Id && t.AMCEXMSUB_ActiveFlg == true).Count();

                    if (resultCount > 0)
                    {
                        page.returnMsg = "duplicate";
                        return page;
                    }
                    //added by 02/02/2017
                    maspge.AMCEXMSUB_CreatedDate = DateTime.Now;
                    maspge.AMCEXMSUB_UpdatedDate = DateTime.Now;
                    maspge.AMCEXMSUB_ActiveFlg = true;
                    _ClgAdmissionContext.Add(maspge);
                    var contactExists = _ClgAdmissionContext.SaveChanges();

                    if (contactExists == 1)
                    {
                        returnresult = true;
                        page.returnval = returnresult;
                        page.returnMsg = "add";
                    }
                    else
                    {
                        returnresult = false;
                        page.returnval = returnresult;
                    }
                }

                List<Master_CompetitiveExamsSubjectsAdmClgDMO> allpages = new List<Master_CompetitiveExamsSubjectsAdmClgDMO>();
                allpages = _ClgAdmissionContext.Master_CompetitiveExamsSubjectsAdmClgDMO.ToList();
                page.pagesdatatwo = allpages.OrderByDescending(c => c.AMCEXMSUB_CreatedDate).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return page;
        }

        public Master_Competitive_AdmExamsClgDTO getexamedit(int id)
        {
            Master_Competitive_AdmExamsClgDTO page = new Master_Competitive_AdmExamsClgDTO();
            try
            {
                List<Master_Competitive_AdmExamsClgDMO> lorg = new List<Master_Competitive_AdmExamsClgDMO>();
                lorg = _ClgAdmissionContext.Master_Competitive_AdmExamsClgDMO.AsNoTracking().Where(t => t.AMCEXM_Id.Equals(id)).ToList();
                page.pagesdata = lorg.OrderByDescending(c => c.AMCEXM_CreatedDate).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }


        public Master_Competitive_AdmExamsClgDTO deleterecord(int id)
        {

            List<Master_Competitive_AdmExamsClgDMO> mastersect = new List<Master_Competitive_AdmExamsClgDMO>(); // Mapper.Map<Organisation>(org);
            Master_Competitive_AdmExamsClgDMO maspge = new Master_Competitive_AdmExamsClgDMO();

            Master_Competitive_AdmExamsClgDTO mas = new Master_Competitive_AdmExamsClgDTO();

            try
            {
                var result = _ClgAdmissionContext.Master_Competitive_AdmExamsClgDMO.Single(t => t.AMCEXM_Id == id);

                if (result.AMCEXM_ActiveFlg == true)
                {
                    result.AMCEXM_ActiveFlg = false;
                    result.AMCEXM_UpdatedDate = DateTime.Now;
                    result.AMCEXM_CreatedDate = result.AMCEXM_CreatedDate;
                    _ClgAdmissionContext.Update(result);
                    _ClgAdmissionContext.SaveChanges();
                    mas.returnval = true;
                }
                else
                {
                    result.AMCEXM_UpdatedDate = DateTime.Now;
                    result.AMCEXM_CreatedDate = result.AMCEXM_CreatedDate;
                    result.AMCEXM_ActiveFlg = true;
                    _ClgAdmissionContext.Update(result);
                    _ClgAdmissionContext.SaveChanges();
                    mas.returnval = false;
                }
                List<Master_Competitive_AdmExamsClgDMO> allmasterexam = new List<Master_Competitive_AdmExamsClgDMO>();
                allmasterexam = _ClgAdmissionContext.Master_Competitive_AdmExamsClgDMO.Where(d => d.MI_Id == mas.MI_Id).ToList();
                mas.MasterExamData = allmasterexam.OrderByDescending(a => a.AMCEXM_CreatedDate).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                //master.returnval = ee.Message;
            }

            return mas;
        }


        public Master_Competitive_AdmExamsClgDTO getsubedit(int id)
        {
            Master_Competitive_AdmExamsClgDTO page = new Master_Competitive_AdmExamsClgDTO();
            try
            {
                List<Master_CompetitiveExamsSubjectsAdmClgDMO> lorgsub = new List<Master_CompetitiveExamsSubjectsAdmClgDMO>();
                lorgsub = _ClgAdmissionContext.Master_CompetitiveExamsSubjectsAdmClgDMO.AsNoTracking().Where(t => t.AMCEXMSUB_Id.Equals(id)).ToList();
                page.pagesdata = lorgsub.OrderByDescending(c => c.AMCEXMSUB_CreatedDate).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }


        public Master_Competitive_AdmExamsClgDTO deleterecordsub(int id)
        {

            List<Master_CompetitiveExamsSubjectsAdmClgDMO> mastersect = new List<Master_CompetitiveExamsSubjectsAdmClgDMO>(); // Mapper.Map<Organisation>(org);
            Master_CompetitiveExamsSubjectsAdmClgDMO maspge = new Master_CompetitiveExamsSubjectsAdmClgDMO();

            Master_Competitive_AdmExamsClgDTO mas = new Master_Competitive_AdmExamsClgDTO();

            try
            {
                var result = _ClgAdmissionContext.Master_CompetitiveExamsSubjectsAdmClgDMO.Single(t => t.AMCEXMSUB_Id == id);

                if (result.AMCEXMSUB_ActiveFlg == true)
                {
                    result.AMCEXMSUB_ActiveFlg = false;
                    result.AMCEXMSUB_UpdatedDate = DateTime.Now;
                    result.AMCEXMSUB_CreatedDate = result.AMCEXMSUB_CreatedDate;
                    _ClgAdmissionContext.Update(result);
                    _ClgAdmissionContext.SaveChanges();
                    mas.returnval = true;
                }
                else
                {
                    result.AMCEXMSUB_UpdatedDate = DateTime.Now;
                    result.AMCEXMSUB_CreatedDate = result.AMCEXMSUB_CreatedDate;
                    result.AMCEXMSUB_ActiveFlg = true;
                    _ClgAdmissionContext.Update(result);
                    _ClgAdmissionContext.SaveChanges();
                    mas.returnval = false;
                }
                //List<Master_Competitive_ExamsClgDMO> allmasterexam = new List<Master_Competitive_ExamsClgDMO>();
                //allmasterexam = _precontext.Master_Competitive_ExamsClgDMO.Where(d => d.MI_Id == mas.MI_Id).ToList();
                //mas.MasterExamData = allmasterexam.OrderByDescending(a => a.PAMCEXM_CreatedDate).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                //master.returnval = ee.Message;
            }

            return mas;
        }

    }
}
