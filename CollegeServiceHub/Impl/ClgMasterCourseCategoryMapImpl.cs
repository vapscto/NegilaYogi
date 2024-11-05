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
    public class ClgMasterCourseCategoryMapImpl : ClgMasterCourseCategoryMapInterface
    {
        private static ConcurrentDictionary<string, ClgMasterCourseCategoryMapDTO> MsCadm = new ConcurrentDictionary<string, ClgMasterCourseCategoryMapDTO>();

        public ClgAdmissionContext mastercourse;
        public ClgMasterCourseCategoryMapImpl(ClgAdmissionContext mscadm)
        {

            mastercourse = mscadm;
        }

        public ClgMasterCourseCategoryMapDTO Savedetails(ClgMasterCourseCategoryMapDTO id)
        {
            if (id.AMCOCM_Id == 0)
            {
                try
                {
                    var check_categorymapped = mastercourse.ClgMasterCourseCategorycategoryMap.Where(a => a.MI_Id == id.MI_Id && a.AMCO_Id == id.AMCO_Id && a.AMCOCM_ActiveFlg == true).ToList();
                    if (check_categorymapped.Count > 0)
                    {
                        id.message = "Exists";
                    }
                    else
                    {
                        ClgMasterCourseCategoryMapDMO obje_p = Mapper.Map<ClgMasterCourseCategoryMapDMO>(id);
                        obje_p.AMCOCM_ActiveFlg = true;
                        obje_p.CreatedDate = DateTime.Now;
                        obje_p.UpdatedDate = DateTime.Now;
                        mastercourse.Add(obje_p);
                        var existcount = mastercourse.SaveChanges();
                        if (existcount > 0)
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
                catch (Exception ex)
                {
                    id.message = "Contact";
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                try
                {
                    var check_categorymapped = mastercourse.ClgMasterCourseCategorycategoryMap.Where(a => a.MI_Id == id.MI_Id && a.AMCO_Id == id.AMCO_Id && a.AMCOCM_ActiveFlg == true && a.AMCOCM_Id!=id.AMCOCM_Id).ToList();

                    if (check_categorymapped.Count > 0)
                    {
                        id.message = "Exists";
                    }
                    else
                    {
                        var result = mastercourse.ClgMasterCourseCategorycategoryMap.Single(a => a.MI_Id == id.MI_Id && a.AMCOCM_Id == id.AMCOCM_Id);
                        result.AMCO_Id = id.AMCO_Id;
                        result.AMCOC_Id = id.AMCOC_Id;
                        result.UpdatedDate = DateTime.Now;
                        mastercourse.Update(result);
                        var idd = mastercourse.SaveChanges();
                        if (idd > 0)
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
                catch(Exception ex)
                {
                    id.message = "Contact";
                    Console.WriteLine(ex.Message);
                }
            }
            return id;

        }

        public ClgMasterCourseCategoryMapDTO getalldetails(ClgMasterCourseCategoryMapDTO data)
        {
            try
            {
                // ClgMasterCourseCategoryMapDTO data = new ClgMasterCourseCategoryMapDTO();
                data.MasterCourseList = mastercourse.MasterCourseDMO.Where(t => t.MI_Id == Convert.ToInt64(data.MI_Id)).Distinct().ToArray();

                data.mastercategory = mastercourse.mastercategory.Where(t => t.MI_Id == Convert.ToInt64(data.MI_Id)).Distinct().ToArray();

                var Heads = (from a in mastercourse.MasterCourseDMO
                             from b in mastercourse.mastercategory
                             from c in mastercourse.ClgMasterCourseCategorycategoryMap
                             where (a.AMCO_Id == c.AMCO_Id && b.AMCOC_Id == c.AMCOC_Id && c.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id==data.MI_Id
                              && c.MI_Id == data.MI_Id)
                             select new ClgMasterCourseCategoryMapDTO
                             {
                                 AMCO_Id = a.AMCO_Id,
                                 AMCO_CourseName = a.AMCO_CourseName,
                                 AMCOC_Id = b.AMCOC_Id,
                                 AMCOC_Name = b.AMCOC_Name,
                                 AMCOCM_Id = c.AMCOCM_Id,
                                AMCOCM_ActiveFlg=c.AMCOCM_ActiveFlg


                             }).Distinct().ToList();



                data.grid = Heads.ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }



            return data;
        }

        public ClgMasterCourseCategoryMapDTO deactive(ClgMasterCourseCategoryMapDTO id)
        {
            try
            {
                var result = mastercourse.ClgMasterCourseCategorycategoryMap.Single(t => t.AMCOCM_Id == id.AMCOCM_Id);

                if (result.AMCOCM_ActiveFlg == true)
                {
                    result.AMCOCM_ActiveFlg = false;
                }
                else if (result.AMCOCM_ActiveFlg == false)
                {
                    result.AMCOCM_ActiveFlg = true;
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
    }
}