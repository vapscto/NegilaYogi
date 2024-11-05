using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.VMS.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Recruitment.com.vaps.Services
{
    public class QuestionPaperTypeImpl : Interfaces.QuestionPaperTypeInterface
    {
        public VMSContext _VMSContext;
        public DomainModelMsSqlServerContext _Context;
        public QuestionPaperTypeImpl(VMSContext VMSContext, DomainModelMsSqlServerContext OrganisationContext)
        {
            _VMSContext = VMSContext;
            _Context = OrganisationContext;
        }

        public QuestionPaperTypeDTO getalldetails(QuestionPaperTypeDTO data)
        {
            try
            {
                data.positionlist = _VMSContext.HR_Master_PositionDMO.Where(a => a.MI_Id == data.MI_Id && a.HRMP_ActiveFlg == true).Distinct().OrderBy(e => e.HRMP_Position).ToArray();

                data.qnstypelist = (from a in _VMSContext.HR_Master_PositionDMO
                                    from b in _VMSContext.OT_QuestionPaperTypeDMO
                                    where a.HRMP_Id == b.HRMP_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id
                                    select new QuestionPaperTypeDTO
                                    {
                                        HRMP_Position = a.HRMP_Position,
                                        HRMP_Id = a.HRMP_Id,
                                        OTQPTYP_Id = b.OTQPTYP_Id,
                                        OTQPTYP_QuestionPaperName = b.OTQPTYP_QuestionPaperName,
                                        OTQPTYP_QuestionPaperDesc = b.OTQPTYP_QuestionPaperDesc,
                                        OTQPTYP_ActiveFlg = b.OTQPTYP_ActiveFlg,
                                    }).Distinct().OrderByDescending(e => e.OTQPTYP_Id).ToArray();
                    
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
              
            }
            return data;
        }

        public QuestionPaperTypeDTO savedetails(QuestionPaperTypeDTO data)
        {
            data.retrunMsg = "";
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
            try
            {

                if (data.OTQPTYP_Id>0)
                {
                    var duplicatecountresult = _VMSContext.OT_QuestionPaperTypeDMO.Where(e => e.OTQPTYP_QuestionPaperName.ToLower().Trim() == data.OTQPTYP_QuestionPaperName.ToLower().Trim() && e.OTQPTYP_Id != data.OTQPTYP_Id && e.HRMP_Id==data.HRMP_Id).ToList();
                    if (duplicatecountresult.Count>0)
                    {
                        data.retrunMsg = "Duplicate";
                        return data;
                    }
                    else
                    {
                        var res = _VMSContext.OT_QuestionPaperTypeDMO.Single(e => e.OTQPTYP_Id == data.OTQPTYP_Id);
                        res.HRMP_Id = data.HRMP_Id;
                        res.OTQPTYP_QuestionPaperName = data.OTQPTYP_QuestionPaperName;
                        res.OTQPTYP_QuestionPaperDesc = data.OTQPTYP_QuestionPaperDesc;
                        res.OTQPTYP_UpdatedDate = indianTime;
                        res.OTQPTYP_UpdatedBy = data.User_Id;
                        _VMSContext.Update(res);
                        int cx = _VMSContext.SaveChanges();

                        if (cx >0)
                        {
                            data.retrval = true;
                        }
                        else
                        {
                            data.retrval = false;
                        }

                    }
                }
                else
                {
                    var duplicatecountresult = _VMSContext.OT_QuestionPaperTypeDMO.Where(e => e.OTQPTYP_QuestionPaperName.ToLower().Trim() == data.OTQPTYP_QuestionPaperName.ToLower().Trim()  && e.HRMP_Id == data.HRMP_Id).ToList();
                    if (duplicatecountresult.Count > 0)
                    {
                        data.retrunMsg = "Duplicate";
                        return data;
                    }
                    else
                    {
                        OT_QuestionPaperTypeDMO obj = new OT_QuestionPaperTypeDMO();
                        obj.HRMP_Id = data.HRMP_Id;
                        obj.MI_Id = data.MI_Id;
                        obj.OTQPTYP_QuestionPaperName = data.OTQPTYP_QuestionPaperName;
                        obj.OTQPTYP_QuestionPaperDesc = data.OTQPTYP_QuestionPaperDesc;
                        obj.OTQPTYP_CreatedDate = indianTime;
                        obj.OTQPTYP_UpdatedDate = indianTime;
                        obj.OTQPTYP_UpdatedBy = data.User_Id;
                        obj.OTQPTYP_CreatedBy = data.User_Id;
                        obj.OTQPTYP_ActiveFlg = true;
                        _VMSContext.Add(obj);
                        int cx = _VMSContext.SaveChanges();

                        if (cx > 0)
                        {
                            data.retrval = true;
                        }
                        else
                        {
                            data.retrval = false;
                        }

                    }
                }


               
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
               data.retrunMsg = "Error";
            }
            return data;
        }

        public QuestionPaperTypeDTO editData(int id)
        {
            QuestionPaperTypeDTO data = new QuestionPaperTypeDTO();
            try
            {
                data.editlist= _VMSContext.OT_QuestionPaperTypeDMO.Where(e => e.OTQPTYP_Id == id).Distinct().ToArray();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }

        public QuestionPaperTypeDTO deactivate(QuestionPaperTypeDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                if (data.OTQPTYP_Id > 0)
                {
                    var result = _VMSContext.OT_QuestionPaperTypeDMO.Single(t => t.OTQPTYP_Id == data.OTQPTYP_Id);

                    if (result.OTQPTYP_ActiveFlg == true)
                    {
                        result.OTQPTYP_ActiveFlg = false;
                    }
                    else 
                    {
                        result.OTQPTYP_ActiveFlg = true;
                    }
                    result.OTQPTYP_UpdatedDate = indianTime;
                    result.OTQPTYP_UpdatedBy = data.User_Id;

                    _VMSContext.Update(result);
                    var flag = _VMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        data.retrval = true;
                    }
                    else
                    {
                        data.retrval = false;
                    }

                   
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                data.retrunMsg = "Error occured";
            }

            return data;
        }

      

    }
}
