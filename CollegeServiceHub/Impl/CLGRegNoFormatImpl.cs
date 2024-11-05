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
    public class CLGRegNoFormatImpl : Interface.CLGRegNoFormatInterface
    {
        private static ConcurrentDictionary<string, CLGAdm_College_RegNo_FormatDTO> MsCadm =
           new ConcurrentDictionary<string, CLGAdm_College_RegNo_FormatDTO>();

        public ClgAdmissionContext _context;
        public CLGRegNoFormatImpl(ClgAdmissionContext mscadm)
        {

            _context= mscadm;
        }

        public CLGAdm_College_RegNo_FormatDTO Savedetails(CLGAdm_College_RegNo_FormatDTO data)
        {
          
            try
            {
                if (data.ACRF_Id > 0)
                {
                   
                        var resultobj = _context.CLGAdm_College_RegNo_FormatDMO.Single(t => t.ACRF_Id.Equals(data.ACRF_Id) && t.MI_Id.Equals(data.MI_Id));

                        resultobj.ACRF_CollegeCodeFlg = data.ACRF_CollegeCodeFlg;
                        resultobj.ACRF_CCOrderFlg = data.ACRF_CCOrderFlg;
                        resultobj.ACRF_AYCodeFlg = data.ACRF_AYCodeFlg;
                        resultobj.ACRF_AYCodeOrderFlg = data.ACRF_AYCodeOrderFlg;
                        resultobj.ACRF_BranchCodeFlg = data.ACRF_BranchCodeFlg;
                        resultobj.ACRF_BranchCodeOrderFlg = data.ACRF_BranchCodeOrderFlg;
                        resultobj.ACRF_NumericWidth = data.ACRF_NumericWidth;
                        resultobj.ACRF_SLNo = data.ACRF_SLNo;
                        resultobj.ACRF_StartingNo = data.ACRF_StartingNo;
                        resultobj.ACRF_PrefilZeroFlg = data.ACRF_PrefilZeroFlg;
                      
                        resultobj.UpdatedDate = DateTime.Now;
                        //obj.AMCO_ActiveFlag = 1;
                        // obj.CreatedDate = DateTime.Now;
                        resultobj.UpdatedDate = DateTime.Now;

                        _context.Update(resultobj);
                        int contactExists = _context.SaveChanges();
                        if (contactExists == 1)
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
                    var result = _context.CLGAdm_College_RegNo_FormatDMO.Where(t => t.MI_Id == data.MI_Id).ToList();
                    if (result.Count() > 0)
                    {
                        data.duplicateval = true;
                    }
                    else
                    {
                        CLGAdm_College_RegNo_FormatDMO obj = new CLGAdm_College_RegNo_FormatDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.ACRF_CollegeCodeFlg = data.ACRF_CollegeCodeFlg;
                        obj.ACRF_CCOrderFlg = data.ACRF_CCOrderFlg;
                        obj.ACRF_AYCodeFlg = data.ACRF_AYCodeFlg;
                        obj.ACRF_AYCodeOrderFlg = data.ACRF_AYCodeOrderFlg;
                        obj.ACRF_BranchCodeFlg = data.ACRF_BranchCodeFlg;
                        obj.ACRF_BranchCodeOrderFlg = data.ACRF_BranchCodeOrderFlg;
                        obj.ACRF_NumericWidth = data.ACRF_NumericWidth;
                        obj.ACRF_SLNo = data.ACRF_SLNo;
                        obj.ACRF_StartingNo = data.ACRF_StartingNo;
                        obj.ACRF_PrefilZeroFlg = data.ACRF_PrefilZeroFlg;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;

                        _context.Add(obj);
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
                }
            }
            catch(Exception msg)
            {
                Console.WriteLine(msg.Message);
            }
            return data;
        }

        public CLGAdm_College_RegNo_FormatDTO getalldetails(int id)
        {

            CLGAdm_College_RegNo_FormatDTO data = new CLGAdm_College_RegNo_FormatDTO();
         
            data.datalist = _context.CLGAdm_College_RegNo_FormatDMO.Where(t => t.MI_Id == Convert.ToInt64(id)).Distinct().ToArray();

            return data;
        }

        public CLGAdm_College_RegNo_FormatDTO Deletedetails(CLGAdm_College_RegNo_FormatDTO id)
        {
            try
           {
            //    var result = mastercourse.MasterCourseDMO.Single(t => t.AMCO_Id == id.AMCO_Id);
            //    //   result.MI_Id = id.MI_Id;

            //    if(result.AMCO_ActiveFlag==true)
            //    {
            //        result.AMCO_ActiveFlag = false;
            //    }
            //    else if (result.AMCO_ActiveFlag==false)
            //    {
            //        result.AMCO_ActiveFlag = true;
            //    }               
            //    result.UpdatedDate = DateTime.Now;
            //    mastercourse.Update(result);
            //    int returnval=mastercourse.SaveChanges();
            //    if(returnval>0)
            //    {
            //        id.returnval = true;
            //    }
            //    else
            //    {
            //        id.returnval = false;
            //    }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return id;
        }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           