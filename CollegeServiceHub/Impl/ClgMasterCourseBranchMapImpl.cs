using AutoMapper;
using CollegeServiceHub.Interface;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Admission;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Impl
{
    public class ClgMasterCourseBranchMapImpl : ClgMasterCourseBranchMapInterface
    {
        private static ConcurrentDictionary<string, ClgMasterCourseBranchMapDTO> MsCadm = new ConcurrentDictionary<string, ClgMasterCourseBranchMapDTO>();

        public ClgAdmissionContext mastercourse;
        public ClgMasterCourseBranchMapImpl(ClgAdmissionContext mscadm)
        {

            mastercourse = mscadm;
        }

        public ClgMasterCourseBranchMapDTO Savedetails(ClgMasterCourseBranchMapDTO id)
        {
            try
            {
                ClgMasterCourseBranchMapDTO cat = new ClgMasterCourseBranchMapDTO();
                if (id.AMCOBM_Id == 0)
                {
                    var check_branch = mastercourse.Adm_Course_Branch_MappingDMO.Where(a => a.MI_Id == id.MI_Id && a.AMB_Id == id.AMB_Id && a.AMCOBM_ActiveFlg == true).ToList();
                    if (check_branch.Count > 0)
                    {
                        id.message = "Exists";
                    }
                    else
                    {
                        //Adm_Course_Branch_MappingDMO obje_p = Mapper.Map<Adm_Course_Branch_MappingDMO>(id);

                        Adm_Course_Branch_MappingDMO obje_p = new Adm_Course_Branch_MappingDMO();
                        obje_p.MI_Id = id.MI_Id;
                        obje_p.AMCO_Id = id.AMCO_Id;
                        obje_p.AMB_Id = id.AMB_Id;
                        obje_p.AMCOBM_ActiveFlg = true;
                        obje_p.CreatedDate = DateTime.Now;
                        obje_p.UpdatedDate = DateTime.Now;

                        obje_p.AMCOBM_Code = id.AMCOBM_Code;
                        obje_p.AMCOBM_CBCSFlg = id.AMCOBM_CBCSFlg;
                        obje_p.AMCOBM_ElectiveFlg = id.AMCOBM_ElectiveFlg;
                        obje_p.AMCOBM_CBCSIntroYear = id.AMCOBM_CBCSIntroYear;
                        obje_p.AMCOBM_FileName = id.AMCOBM_FileName;
                        obje_p.AMCOBM_FilePath = id.AMCOBM_FilePath;
                        obje_p.AMCOBM_ElectiveIntroYear = id.AMCOBM_ElectiveIntroYear;

                        mastercourse.Add(obje_p);


                        for (int kk = 0; kk < id.semester_list.Count(); kk++)
                        {
                            AdmCourseBranchSemesterMappingDMO branchsem = new AdmCourseBranchSemesterMappingDMO();
                            branchsem.AMCOBM_Id = obje_p.AMCOBM_Id;
                            branchsem.MI_Id = id.MI_Id;
                            branchsem.AMSE_Id = id.semester_list[kk].AMSE_Id;
                            branchsem.AMCOBMS_ActiveFlg = true;
                            branchsem.CreatedDate = DateTime.Now;
                            branchsem.UpdatedDate = DateTime.Now;
                            mastercourse.Add(branchsem);
                        }

                        int returnval = mastercourse.SaveChanges();
                        if (returnval > 0)
                        {
                            id.returnval = true;
                            id.message = "Add";
                        }
                        else
                        {
                            id.returnval = false;
                            id.message = "Add";
                        }
                    }
                }
                else
                {
                    var check_branch = mastercourse.Adm_Course_Branch_MappingDMO.Where(a => a.MI_Id == id.MI_Id && a.AMB_Id == id.AMB_Id && a.AMCOBM_ActiveFlg == true && a.AMCOBM_Id != id.AMCOBM_Id).ToList();
                    if (check_branch.Count > 0)
                    {
                        id.message = "Exists";
                    }
                    else
                    {
                        var result = mastercourse.Adm_Course_Branch_MappingDMO.Single(a => a.MI_Id == id.MI_Id && a.AMCOBM_Id == id.AMCOBM_Id);
                        result.AMCO_Id = id.AMCO_Id;
                        result.AMB_Id = id.AMB_Id;

                        result.AMCOBM_Code = id.AMCOBM_Code;
                        result.AMCOBM_CBCSFlg = id.AMCOBM_CBCSFlg;
                        result.AMCOBM_ElectiveFlg = id.AMCOBM_ElectiveFlg;
                        result.AMCOBM_CBCSIntroYear = id.AMCOBM_CBCSIntroYear;
                        result.AMCOBM_FileName = id.AMCOBM_FileName;
                        result.AMCOBM_FilePath = id.AMCOBM_FilePath;
                        result.AMCOBM_ElectiveIntroYear = id.AMCOBM_ElectiveIntroYear;

                        result.UpdatedDate = DateTime.Now;
                        mastercourse.Update(result);

                        for (int kk = 0; kk < id.semester_list.Count(); kk++)
                        {
                            var checkseme = mastercourse.AdmCourseBranchSemesterMappingDMO.Where(a => a.AMCOBM_Id == id.AMCOBM_Id && a.AMSE_Id == id.semester_list[kk].AMSE_Id).ToList();
                            if (checkseme.Count > 0)
                            {
                                var resultd = mastercourse.AdmCourseBranchSemesterMappingDMO.Single(a => a.AMCOBM_Id == id.AMCOBM_Id && a.AMSE_Id == id.semester_list[kk].AMSE_Id);
                                resultd.AMSE_Id = id.semester_list[kk].AMSE_Id;
                                resultd.UpdatedDate = DateTime.Now;
                                mastercourse.Update(resultd);
                            }
                            else
                            {
                                AdmCourseBranchSemesterMappingDMO branchsem = new AdmCourseBranchSemesterMappingDMO();
                                branchsem.AMCOBM_Id = id.AMCOBM_Id;
                                branchsem.AMSE_Id = id.semester_list[kk].AMSE_Id;
                                branchsem.AMCOBMS_ActiveFlg = true;
                                branchsem.MI_Id = id.MI_Id;
                                branchsem.CreatedDate = DateTime.Now;
                                branchsem.UpdatedDate = DateTime.Now;
                                mastercourse.Add(branchsem);
                            }
                        }

                        int returnval = mastercourse.SaveChanges();
                        if (returnval > 0)
                        {
                            id.returnval = true;
                            id.message = "Update";
                        }
                        else
                        {
                            id.returnval = false;
                            id.message = "Update";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                id.message = "Error";
            }


            return id;


        }

        public ClgMasterCourseBranchMapDTO getalldetails(ClgMasterCourseBranchMapDTO data)
        {
            try
            {
                data.MasterCourseList = mastercourse.MasterCourseDMO.Where(t => t.MI_Id == Convert.ToInt64(data.MI_Id) && t.AMCO_ActiveFlag == true).Distinct().ToArray();
                data.masterbranchList = mastercourse.ClgMasterBranchDMO.Where(t => t.MI_Id == Convert.ToInt64(data.MI_Id) && t.AMB_ActiveFlag == true).Distinct().ToArray();
                data.mastersemesterlist = mastercourse.CLG_Adm_Master_SemesterDMO.Where(t => t.MI_Id == Convert.ToInt64(data.MI_Id) && t.AMSE_ActiveFlg == true).Distinct().ToArray();

                var Heads = (from a in mastercourse.MasterCourseDMO
                             from b in mastercourse.ClgMasterBranchDMO
                             from c in mastercourse.Adm_Course_Branch_MappingDMO
                             where (a.AMCO_Id == c.AMCO_Id && b.AMB_Id == c.AMB_Id && c.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && c.MI_Id == data.MI_Id)
                             select new ClgMasterCourseBranchMapDTO
                             {
                                 AMCO_Id = a.AMCO_Id,
                                 AMCO_CourseName = a.AMCO_CourseName,
                                 AMB_Id = b.AMB_Id,
                                 AMB_BranchName = b.AMB_BranchName,
                                 AMCOBM_Id = c.AMCOBM_Id,
                                 AMCOBM_ActiveFlg = c.AMCOBM_ActiveFlg,
                                 AMCOBM_Code=c.AMCOBM_Code,
                             }).Distinct().ToList();

                data.grid = Heads.ToArray();


                data.cbcsyearlist = (from a in mastercourse.AcademicYear
                                     where (a.MI_Id == data.MI_Id && a.Is_Active == true)
                                     select new ClgMasterCourseBranchMapDTO
                                     {
                                         cbcsyearid = a.ASMAY_Id,
                                         cbcsyearname = a.ASMAY_Year,
                                     }).Distinct().ToArray();

                data.electiveyearlist = (from a in mastercourse.AcademicYear
                                         where (a.MI_Id == data.MI_Id && a.Is_Active == true)
                                         select new ClgMasterCourseBranchMapDTO
                                         {
                                             electiveyearid = a.ASMAY_Id,
                                             electiveyearname = a.ASMAY_Year,
                                         }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            //data.grid= mastercourse.Adm_Course_Branch_MappingDMO.Where(t => t.MI_Id == Convert.ToInt64(data.MI_Id)).Distinct().ToArray();

            return data;
        }

        public ClgMasterCourseBranchMapDTO Deletedetails(ClgMasterCourseBranchMapDTO id)
        {
            try
            {
                var result = mastercourse.Adm_Course_Branch_MappingDMO.Single(t => t.AMCOBM_Id == id.AMCOBM_Id);

                if (result.AMCOBM_ActiveFlg == true)
                {
                    result.AMCOBM_ActiveFlg = false;
                }
                else if (result.AMCOBM_ActiveFlg == false)
                {
                    result.AMCOBM_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                mastercourse.Update(result);
                int returnval = mastercourse.SaveChanges();
                if (returnval > 0)
                {
                    id.returnval = true;
                }
                else
                {
                    id.returnval = false;
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return id;
        }

        public ClgMasterCourseBranchMapDTO showmodaldetails(ClgMasterCourseBranchMapDTO data)
        {
            try
            {
                data.semesterbrancklist = (from a in mastercourse.Adm_Course_Branch_MappingDMO
                                           from b in mastercourse.AdmCourseBranchSemesterMappingDMO
                                           from c in mastercourse.CLG_Adm_Master_SemesterDMO
                                           from d in mastercourse.ClgMasterBranchDMO
                                           from e in mastercourse.MasterCourseDMO
                                           where (a.AMCOBM_Id == b.AMCOBM_Id && a.AMB_Id == d.AMB_Id && a.AMCO_Id == e.AMCO_Id && b.AMSE_Id == c.AMSE_Id && a.MI_Id == data.MI_Id
                                           && b.AMCOBM_Id == data.AMCOBM_Id)
                                           select new ClgMasterCourseBranchMapDTO
                                           {
                                               AMCOBMS_Id = b.AMCOBMS_Id,
                                               AMSE_Id = b.AMSE_Id,
                                               AMSE_SEMName = c.AMSE_SEMName,
                                               AMCOBMS_ActiveFlg = b.AMCOBMS_ActiveFlg,
                                               AMB_BranchName = d.AMB_BranchName,
                                               AMCO_CourseName = e.AMCO_CourseName
                                           }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClgMasterCourseBranchMapDTO deactivesem(ClgMasterCourseBranchMapDTO data)
        {
            try
            {
                var result = mastercourse.AdmCourseBranchSemesterMappingDMO.Single(a => a.MI_Id == data.MI_Id && a.AMCOBMS_Id == data.AMCOBMS_Id);
                if (result.AMCOBMS_ActiveFlg == true)
                {
                    result.AMCOBMS_ActiveFlg = false;
                }
                else
                {
                    result.AMCOBMS_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                mastercourse.Update(result);
                var i = mastercourse.SaveChanges();
                if (i > 0)
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
                data.returnval = false;
            }
            return data;
        }
        public ClgMasterCourseBranchMapDTO edit(ClgMasterCourseBranchMapDTO data)
        {
            try
            {
                data.editdata = (from a in mastercourse.Adm_Course_Branch_MappingDMO
                                 from b in mastercourse.AdmCourseBranchSemesterMappingDMO
                                 from c in mastercourse.CLG_Adm_Master_SemesterDMO
                                 from d in mastercourse.ClgMasterBranchDMO
                                 from e in mastercourse.MasterCourseDMO
                                 where (a.AMCOBM_Id == b.AMCOBM_Id && a.AMB_Id == d.AMB_Id && a.AMCO_Id == e.AMCO_Id && b.AMSE_Id == c.AMSE_Id && a.MI_Id == data.MI_Id
                                 && b.AMCOBM_Id == data.AMCOBM_Id && a.AMCOBM_Id == data.AMCOBM_Id)
                                 select new ClgMasterCourseBranchMapDTO
                                 {
                                     AMCOBMS_Id = b.AMCOBMS_Id,
                                     AMSE_Id = b.AMSE_Id,
                                     AMSE_SEMName = c.AMSE_SEMName,
                                     AMCOBMS_ActiveFlg = b.AMCOBMS_ActiveFlg,
                                     AMB_BranchName = d.AMB_BranchName,
                                     AMCO_CourseName = e.AMCO_CourseName,
                                     AMCO_Id = e.AMCO_Id,
                                     AMB_Id = a.AMB_Id,

                                     AMCOBM_Code = a.AMCOBM_Code,
                                     AMCOBM_CBCSFlg = a.AMCOBM_CBCSFlg,
                                     AMCOBM_ElectiveFlg = a.AMCOBM_ElectiveFlg,
                                     AMCOBM_CBCSIntroYear = a.AMCOBM_CBCSIntroYear,
                                     AMCOBM_FileName = a.AMCOBM_FileName,
                                     AMCOBM_FilePath = a.AMCOBM_FilePath,
                                     AMCOBM_ElectiveIntroYear = a.AMCOBM_ElectiveIntroYear,

                                 }).ToArray();

                data.cbcsyearlist = (from a in mastercourse.AcademicYear
                                     where (a.MI_Id == data.MI_Id && a.Is_Active == true)
                                     select new ClgMasterCourseBranchMapDTO
                                     {
                                         cbcsyearid = a.ASMAY_Id,
                                         cbcsyearname = a.ASMAY_Year,
                                     }).Distinct().ToArray();

                data.electiveyearlist = (from a in mastercourse.AcademicYear
                                         where (a.MI_Id == data.MI_Id && a.Is_Active == true)
                                         select new ClgMasterCourseBranchMapDTO
                                         {
                                             electiveyearid = a.ASMAY_Id,
                                             electiveyearname = a.ASMAY_Year,
                                         }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}