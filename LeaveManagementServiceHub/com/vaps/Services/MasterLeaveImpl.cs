using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.LeaveManagement;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.LeaveManagement;
using LeaveManagementServiceHub.com.vaps.Interfaces;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace LeaveManagementServiceHub.com.vaps.Services
{
    public class MasterLeaveImpl : MasterLeaveInterface
    {
        private static ConcurrentDictionary<string, MasterLeaveDTO> _login =
         new ConcurrentDictionary<string, MasterLeaveDTO>();


        public LMContext _lmcontext;
        public MasterLeaveImpl(LMContext ttcategory)
        {
            _lmcontext = ttcategory;
        }

        public MasterLeaveDTO GetLeave(MasterLeaveDTO data)
        {
            try
            {
                List<HR_Master_Leave_DMO> m_events = new List<HR_Master_Leave_DMO>();
                m_events = _lmcontext.HR_Master_Leave_DMO.Where(e => e.MI_Id == data.MI_Id).ToList();
                data.master_eventlist = m_events.ToArray();
                var leaveorderlist = _lmcontext.HR_Master_Leave_DMO.Where(t => t.MI_Id.Equals(data.MI_Id)).ToList();
                data.leaveorderlist = leaveorderlist.ToArray();
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return data;
        }


        public MasterLeaveDTO saveData(MasterLeaveDTO data)
        {
            HR_Master_Leave_DMO objpge = Mapper.Map<HR_Master_Leave_DMO>(data);
            try
            {
                if (objpge.HRML_Id > 0)
                {
                    //var resultCount = _lmcontext.HR_Master_Leave_DMO.Where(t => t.HRML_LeaveName == objpge.HRML_LeaveName && t.MI_Id == objpge.MI_Id && t.HRML_Id != objpge.HRML_Id).Count();
                    var resultCount = _lmcontext.HR_Master_Leave_DMO.Where(t => t.HRML_LeaveName == objpge.HRML_LeaveName && t.HRML_LeaveDetails == objpge.HRML_LeaveDetails && t.HRML_LeaveType == objpge.HRML_LeaveType && t.HRML_LeaveCreditFlg == objpge.HRML_LeaveCreditFlg && t.MI_Id == objpge.MI_Id && t.HRML_Id != objpge.HRML_Id && t.HRML_LateDeductFlag == objpge.HRML_LateDeductFlag).Count();
                    if (resultCount == 0)
                    {
                        var result = _lmcontext.HR_Master_Leave_DMO.Single(t => t.HRML_Id == objpge.HRML_Id && t.MI_Id == objpge.MI_Id);

                        result.HRML_LeaveName = objpge.HRML_LeaveName;
                        result.HRML_LeaveCode = objpge.HRML_LeaveCode;
                        result.HRML_LeaveDetails = objpge.HRML_LeaveDetails;                      
                        result.HRML_LeaveType = objpge.HRML_LeaveType;
                        result.HRML_LeaveCreditFlg = objpge.HRML_LeaveCreditFlg;
                        result.HRML_LateDeductFlag = objpge.HRML_LateDeductFlag;
                        result.UpdatedDate = DateTime.Now;
                        _lmcontext.Update(result);
                        var contactExists = _lmcontext.SaveChanges();
                        if (contactExists == 1)
                        {
                            data.returnval_Update = true;
                        }
                        else
                        {
                            data.returnval_Update = false;
                        }
                    }
                    else
                    {
                        data.returnduplicatestatus = "Duplicate";
                        return data;
                    }
                }
                else
                {
                    //var result = _lmcontext.HR_Master_Leave_DMO.Where(t => t.HRML_LeaveName == objpge.HRML_LeaveName && t.MI_Id == objpge.MI_Id).Count();
                    var result = _lmcontext.HR_Master_Leave_DMO.Where(t => t.HRML_LeaveName == objpge.HRML_LeaveName && t.HRML_LeaveDetails == objpge.HRML_LeaveDetails && t.HRML_LeaveType == objpge.HRML_LeaveType && t.HRML_LeaveCreditFlg == objpge.HRML_LeaveCreditFlg && t.MI_Id == objpge.MI_Id && t.HRML_Id != objpge.HRML_Id && t.HRML_LateDeductFlag == objpge.HRML_LateDeductFlag).Count();
                    if (result > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else if (result == 0)
                    {
                        objpge.CreatedDate = DateTime.Now;
                        objpge.UpdatedDate = DateTime.Now;

                        _lmcontext.Add(objpge);
                        var contactExists = _lmcontext.SaveChanges();
                        if (contactExists == 1)
                        {
                            data.returnval_add = true;
                        }
                        else
                        {
                            data.returnval_add = false;
                        }
                    }
                }

                List<HR_Master_Leave_DMO> m_events = new List<HR_Master_Leave_DMO>();
                m_events = _lmcontext.HR_Master_Leave_DMO.Where(e => e.MI_Id == objpge.MI_Id).ToList();
                data.master_eventlist = m_events.ToArray();
                var leaveorderlist = _lmcontext.HR_Master_Leave_DMO.Where(t => t.MI_Id.Equals(data.MI_Id)).ToList();
                data.leaveorderlist = leaveorderlist.ToArray();
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return data;
        }


        public MasterLeaveDTO Edit(int id)
        {
            MasterLeaveDTO page = new MasterLeaveDTO();
            try
            {
                List<HR_Master_Leave_DMO> events_m = new List<HR_Master_Leave_DMO>();
                events_m = _lmcontext.HR_Master_Leave_DMO.Where(e => e.HRML_Id == id).ToList();
                page.edit_m_event = events_m.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public MasterLeaveDTO validateordernumber(MasterLeaveDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.MasterLeaveDTOO.Count() > 0)
                {
                    foreach (MasterLeaveDTO mob in dto.MasterLeaveDTOO)
                    {
                        if (mob.HRML_Id > 0)
                        {
                            var result = _lmcontext.HR_Master_Leave_DMO.Single(t => t.HRML_Id.Equals(mob.HRML_Id));
                            Mapper.Map(mob, result);
                            _lmcontext.Update(result);
                            _lmcontext.SaveChanges();
                        }
                    }
                    dto.retrunMsg = "Order Updated sucessfully";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured"; ;
            }
            return dto;
        }

        //public MasterLeaveDTO deletepages(MasterLeaveDTO data)
        //{
        //    bool returnresult = false;
        //    bool dupl = false;
        //    MasterLeaveDTO page = new MasterLeaveDTO();
        //    List<HR_Master_Leave_DMO> lorgrecords = new List<HR_Master_Leave_DMO>();
        //    lorgrecords = _lmcontext.HR_Master_Leave_DMO.Where(t => t.HRML_Id.Equals(data)).ToList();
        //    if (lorgrecords.Count == 0)
        //    {

        //        List<HR_Master_Leave_DMO> lorg = new List<HR_Master_Leave_DMO>();
        //        lorg = _lmcontext.HR_Master_Leave_DMO.Where(t => t.HRML_Id.Equals(data)).ToList();

        //        try
        //        {
        //            if (lorg.Any())
        //            {
        //                _lmcontext.Remove(lorg.ElementAt(0));

        //                var contactExists = _lmcontext.SaveChanges();
        //                if (contactExists == 1)
        //                {
        //                    returnresult = true;
        //                    page.returnval = returnresult;
        //                }
        //                else
        //                {
        //                    returnresult = false;
        //                    page.returnval = returnresult;
        //                }
        //            }

        //            List<HR_Master_Leave_DMO> allpages = new List<HR_Master_Leave_DMO>();
        //            allpages = _lmcontext.HR_Master_Leave_DMO.ToList();
        //            page.leaveData = allpages.ToArray();
        //        }
        //        catch (Exception ee)
        //        {
        //           // _logger.LogError(ee.Message);
        //            Console.WriteLine(ee.Message);
        //        }
        //    }
        //    else
        //    {
        //        dupl = false;
        //        page.dupr = dupl;
        //    }
        //    return page;
        //}
        public MasterLeaveDTO deletepages(MasterLeaveDTO data)
        {
           // MasterLeaveDTO page = new MasterLeaveDTO();
            try
            {
                List<HR_Master_Leave_DMO> lorg = new List<HR_Master_Leave_DMO>();
                lorg = _lmcontext.HR_Master_Leave_DMO.Where(t => t.HRML_Id.Equals(data.HRML_Id)).ToList();
                if (lorg.Any())
                {
                    _lmcontext.Remove(lorg.ElementAt(0));
                    var contactExists = _lmcontext.SaveChanges();
                    if (contactExists == 1)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
                List<HR_Master_Leave_DMO> allpages = new List<HR_Master_Leave_DMO>();
                allpages = _lmcontext.HR_Master_Leave_DMO.ToList();
                data.leaveData = allpages.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public MasterLeaveDTO getpageedit(int id)
        {
            MasterLeaveDTO page = new MasterLeaveDTO();
            try
            {

                List<HR_Master_Leave_DMO> events_m = new List<HR_Master_Leave_DMO>();
                events_m = _lmcontext.HR_Master_Leave_DMO.Where(e => e.HRML_Id == id).ToList();
                page.edit_m_event = events_m.ToArray();
                //List<HR_Master_Leave_DMO> lorg = new List<HR_Master_Leave_DMO>();
                //lorg = _lmcontext.HR_Master_Leave_DMO.AsNoTracking().Where(t => t.TTMSAB_Id.Equals(id)).ToList();
                //page.sujectslistedit = lorg.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public MasterLeaveDTO searchByColumn(MasterLeaveDTO data)
        {
            return data;
        }

        public MasterLeaveDTO deactivate(MasterLeaveDTO data)
        {
            return data;
        }
      
    }
}
