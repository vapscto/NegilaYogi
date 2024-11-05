using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model.com.vapstech.Exam;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Services
{
    public class ExamTTsessionmasterImpl : Interfaces.ExamTTsessionmasterInterface
    {

        public ExamTimeTableContext _exmttcontext;
        ILogger<ExamTTsessionmasterImpl> _log;

        public ExamTTsessionmasterImpl(ExamTimeTableContext exmttcontext, ILogger<ExamTTsessionmasterImpl> log)
        {
            _exmttcontext = exmttcontext;
            _log = log;
        }
        public ExamTTsessionmasterDTO Getdetails(ExamTTsessionmasterDTO data)
        {
            try
            {
                data.getdetails = _exmttcontext.Exm_TT_M_SessionDMO.Where(a => a.MI_Id == data.MI_id).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Error In Exam Timetable Getdetails :" + ex.Message);
            }
            return data;
        }
        public ExamTTsessionmasterDTO savedetails(ExamTTsessionmasterDTO data)
        {
            try
            {
                if (data.ETTS_Id > 0)
                {

                    var checkduplicate = _exmttcontext.Exm_TT_M_SessionDMO.Where(a => a.MI_Id == data.MI_id && a.ETTS_SessionName.Equals(data.ETTS_SessionName) && a.ETTS_Id != data.ETTS_Id).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        var result = _exmttcontext.Exm_TT_M_SessionDMO.Single(a => a.MI_Id == data.MI_id && a.ETTS_Id.Equals(data.ETTS_Id));
                        result.ETTS_SessionName = data.ETTS_SessionName;
                        result.ETTS_StartTime = data.ETTS_StartTime;
                        result.ETTS_EndTime = data.ETTS_EndTime;
                        _exmttcontext.Update(result);
                        int k = _exmttcontext.SaveChanges();
                        if (k > 0)
                        {
                            data.returnval = true;
                            data.message = "Update";
                        }
                        else
                        {
                            data.returnval = false;
                            data.message = "Update";
                        }
                    }

                }
                else
                {
                    var checkduplicate = _exmttcontext.Exm_TT_M_SessionDMO.Where(a => a.MI_Id == data.MI_id && a.ETTS_SessionName.Equals(data.ETTS_SessionName)).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        Exm_TT_M_SessionDMO exmtt = new Exm_TT_M_SessionDMO();
                        exmtt.MI_Id = data.MI_id;
                        exmtt.ETTS_SessionName = data.ETTS_SessionName;
                        exmtt.ETTS_StartTime = data.ETTS_StartTime;
                        exmtt.ETTS_EndTime = data.ETTS_EndTime;
                        exmtt.ETTS_Abreviation = "";
                        exmtt.ETTS_Activeflag = true;
                        exmtt.CreatedDate = DateTime.Now;
                        exmtt.UpdatedDate = DateTime.Now;
                        _exmttcontext.Add(exmtt);
                        int k = _exmttcontext.SaveChanges();
                        if (k > 0)
                        {
                            data.returnval = true;
                            data.message = "Add";
                        }
                        else
                        {
                            data.returnval = false;
                            data.message = "Add";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("Error In Exam Timetable savedetails :" + ex.Message);
            }
            return data;
        }

        public ExamTTsessionmasterDTO editdetails(ExamTTsessionmasterDTO data)
        {
            try
            {
                data.editlist = _exmttcontext.Exm_TT_M_SessionDMO.Where(a => a.MI_Id == data.MI_id && a.ETTS_Id == data.ETTS_Id).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Error In Exam Timetable editdetails :" + ex.Message);
            }
            return data;
        }
        public ExamTTsessionmasterDTO deactivate(ExamTTsessionmasterDTO data)
        {
            try
            {
                var result = _exmttcontext.Exm_TT_M_SessionDMO.Single(a => a.MI_Id == data.MI_id && a.ETTS_Id == data.ETTS_Id);
                if (result.ETTS_Activeflag == true)
                {
                    result.ETTS_Activeflag = false;
                }
                else
                {
                    result.ETTS_Activeflag = true;
                }
                result.UpdatedDate = DateTime.Now;
                _exmttcontext.Update(result);
                int k = _exmttcontext.SaveChanges();
                if (k > 0)
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
                _log.LogInformation("Error In Exam Timetable deactivate :" + ex.Message);
            }
            return data;
        }


    }
}
