using AutoMapper;

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
    public class MasterCourseImpl : Interface.MasterCourseInterface
    {
        private static ConcurrentDictionary<string, MasterCourseDTO> MsCadm =
           new ConcurrentDictionary<string, MasterCourseDTO>();

        public ClgAdmissionContext mastercourse;
        public MasterCourseImpl(ClgAdmissionContext mscadm)
        {

            mastercourse = mscadm;
        }

        public MasterCourseDTO Savedetails(MasterCourseDTO id)
        {

            try
            {
                if (id.AMCO_Id > 0)
                {
                    var ChkDuplicate = mastercourse.MasterCourseDMO.Where(t => (t.AMCO_CourseName.Equals(id.AMCO_CourseName) || t.AMCO_CourseCode.Equals(id.AMCO_CourseCode)) && t.MI_Id.Equals(id.MI_Id) && t.AMCO_Id != id.AMCO_Id).ToList();
                    if (ChkDuplicate.Count() > 0)
                    {
                        id.duplicateval = true;
                    }
                    else
                    {
                        var resultobj = mastercourse.MasterCourseDMO.Single(t => t.AMCO_Id.Equals(id.AMCO_Id) && t.MI_Id.Equals(id.MI_Id));
                        resultobj.MI_Id = id.MI_Id;
                        resultobj.AMCO_CourseName = id.AMCO_CourseName;
                        resultobj.AMCO_CourseCode = id.AMCO_CourseCode;
                        resultobj.AMCO_CourseInfo = id.AMCO_CourseInfo;
                        resultobj.AMCO_CourseFlag = id.AMCO_CourseFlag;
                        resultobj.AMCO_NoOfYears = id.AMCO_NoOfYears;
                        resultobj.AMCO_NoOfSemesters = id.AMCO_NoOfSemesters;
                        resultobj.AMCO_MinAttPer = id.AMCO_MinAttPer;
                        resultobj.AMCO_FeeAplFlg = id.AMCO_FeeAplFlg;
                        resultobj.AMCO_Order = id.AMCO_Order;
                        resultobj.AMCO_RegFeeFlg = id.AMCO_RegFeeFlg;
                        resultobj.UpdatedDate = DateTime.Now;

                        mastercourse.Update(resultobj);
                        int contactExists = mastercourse.SaveChanges();
                        if (contactExists == 1)
                        {
                            id.returnval = true;
                        }
                        else
                        {
                            id.returnval = false;
                        }

                    }
                }
                else
                {
                    var result = mastercourse.MasterCourseDMO.Where(t => (t.AMCO_CourseName.Equals(id.AMCO_CourseName) || t.AMCO_CourseCode.Equals(id.AMCO_CourseCode)) && t.MI_Id.Equals(id.MI_Id)).ToList();
                    if (result.Count() > 0)
                    {
                        id.duplicateval = true;
                    }
                    else
                    {
                        MasterCourseDMO obj = new MasterCourseDMO();
                        // obj.AMCO_Id = id.AMCO_Id;
                        obj.MI_Id = id.MI_Id;
                        obj.AMCO_CourseName = id.AMCO_CourseName;
                        obj.AMCO_CourseCode = id.AMCO_CourseCode;
                        obj.AMCO_CourseInfo = id.AMCO_CourseInfo;
                        obj.AMCO_CourseFlag = id.AMCO_CourseFlag;
                        obj.AMCO_NoOfYears = id.AMCO_NoOfYears;
                        obj.AMCO_NoOfSemesters = id.AMCO_NoOfSemesters;
                        obj.AMCO_MinAttPer = id.AMCO_MinAttPer;
                        obj.AMCO_FeeAplFlg = id.AMCO_FeeAplFlg;
                        obj.AMCO_Order = id.AMCO_Order;
                        obj.AMCO_RegFeeFlg = id.AMCO_RegFeeFlg;
                        obj.AMCO_ActiveFlag = id.AMCO_ActiveFlag;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
                        obj.AMCO_ActiveFlag = true;

                        mastercourse.Add(obj);
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
                }
            }
            catch (Exception msg)
            {
                Console.WriteLine(msg.Message);
            }
            return id;
        }

        public MasterCourseDTO getalldetails(int id)
        {
            MasterCourseDTO data = new MasterCourseDTO();
            data.MasterCourseList = mastercourse.MasterCourseDMO.Where(t => t.MI_Id == Convert.ToInt64(id)).Distinct().ToArray();
            data.MasterCourseList1 = mastercourse.MasterCourseDMO.Where(t => t.MI_Id == Convert.ToInt64(id)).Distinct().OrderBy(a=>a.AMCO_Order).ToArray();

            return data;
        }

        public MasterCourseDTO Deletedetails(MasterCourseDTO id)
        {
            try
            {
                var result = mastercourse.MasterCourseDMO.Single(t => t.AMCO_Id == id.AMCO_Id);

                if (result.AMCO_ActiveFlag == true)
                {
                    result.AMCO_ActiveFlag = false;
                }
                else if (result.AMCO_ActiveFlag == false)
                {
                    result.AMCO_ActiveFlag = true;
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

        public MasterCourseDTO getOrder(MasterCourseDTO data)
        {
            try
            {
                int id = 0;
                for (int i = 0; i < data.coursedto.Count(); i++)
                {
                    id = id + 1;
                    var reult = mastercourse.MasterCourseDMO.Single(t => t.MI_Id == data.MI_Id && t.AMCO_Id == data.coursedto[i].AMCO_Id);

                    if (i == 0)
                    {
                        reult.AMCO_Order = id;
                    }
                    else
                    {
                        reult.AMCO_Order = id;
                    }
                    mastercourse.Update(reult);
                    var flag = mastercourse.SaveChanges();
                    if (flag > 0)
                    {
                        data.retrunMsg = "Course Order Updated";
                    }
                    else
                    {
                        data.retrunMsg = " Failed To Update Course Order";
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.retrunMsg = " Failed To Update Course Order";
            }
            return data;
        }
        public MasterCourseDTO EditData(MasterCourseDTO data)
        {
            try
            {
                data.editdetails = mastercourse.MasterCourseDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCO_Id == data.AMCO_Id).ToArray();

                var check_coursemapped = (from a in mastercourse.CLG_Adm_College_AY_CourseDMO
                                          from b in mastercourse.MasterCourseDMO
                                          where (a.AMCO_Id == b.AMCO_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                          select a).ToList();
                if (check_coursemapped.Count() > 0)
                {
                    data.msg = "Mapped";
                }
                else
                {
                    data.msg = "No";
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.retrunMsg = " Failed To EditData";
            }
            return data;
        }

    }
}
