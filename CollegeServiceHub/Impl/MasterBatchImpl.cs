using CollegeServiceHub.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.College.Admission;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Admission;

namespace CollegeServiceHub.Impl
{
    public class MasterBatchImpl : Interface.MasterBatchInterface
    {
        public ClgAdmissionContext _ClgAdmissionContext;
        public MasterBatchImpl(ClgAdmissionContext ClgAdmissionContext)
        {
            _ClgAdmissionContext = ClgAdmissionContext;

        }

        public AdmCollegeMasterBatchDTO activedeactivebatch(AdmCollegeMasterBatchDTO data)
        {
            try
            {
                var check_batch_used = _ClgAdmissionContext.Adm_Master_College_StudentDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMB_Id == data.ACMB_Id).ToList();
                if (check_batch_used.Count > 0)
                {
                    data.message = "Record Mapped We Can Not Deactive";
                }
                else
                {
                    var query = _ClgAdmissionContext.AdmCollegeMasterBatchDMO.Single(t => t.MI_Id == data.MI_Id && t.ACMB_Id == data.ACMB_Id);
                    if (query.ACMSN_ActiveFlag == true)
                    {
                        query.ACMSN_ActiveFlag = false;
                        _ClgAdmissionContext.Update(query);
                        var flag = _ClgAdmissionContext.SaveChanges();
                        if (flag > 0)
                        {
                            var msg = "Batch De-Activated Successfully";
                            data.message = msg;
                        }
                        else
                        {
                            var msg = "Failed To De-Activated Batch";
                            data.message = msg;
                        }
                    }
                    else
                    {
                        query.ACMSN_ActiveFlag = true;
                        _ClgAdmissionContext.Update(query);
                        var flag = _ClgAdmissionContext.SaveChanges();
                        if (flag > 0)
                        {
                            var msg = "Batch Activated Successfully";
                            data.message = msg;
                        }
                        else
                        {
                            var msg = "Failed To Activated Batch";                           
                            data.message = msg;
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

        public AdmCollegeMasterBatchDTO editbatch(AdmCollegeMasterBatchDTO data)
        {
            try
            {
                var query = _ClgAdmissionContext.AdmCollegeMasterBatchDMO.Where(f => f.ACMB_Id == data.ACMB_Id && f.MI_Id == data.MI_Id && f.ACMSN_ActiveFlag == true).ToArray();
                if (query.Length > 0)
                {
                    data.batchlist = query;
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
            }
            return data;
        }

        public AdmCollegeMasterBatchDTO getdata(AdmCollegeMasterBatchDTO data)
        {
            try
            {
                var query02 = _ClgAdmissionContext.AdmCollegeMasterBatchDMO.Where(w => w.MI_Id == data.MI_Id).ToArray();
                if (query02.Length > 0)
                {
                    data.batchlist = query02;
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
            }
            return data;
        }

        public AdmCollegeMasterBatchDTO savebatch(AdmCollegeMasterBatchDTO data)
        {
            try
            {
                if (data.ACMB_Id > 0)
                {
                    var check_exists = _ClgAdmissionContext.AdmCollegeMasterBatchDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMSN_SessionName == data.ACMSN_SessionName 
                    && a.ACMB_Id != data.ACMB_Id).ToList();

                    var check_exists1 = _ClgAdmissionContext.AdmCollegeMasterBatchDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMNS_Order == data.ACMNS_Order
                   && a.ACMB_Id != data.ACMB_Id).ToList();

                    if (check_exists.Count > 0 || check_exists1.Count>0) 
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        var query = _ClgAdmissionContext.AdmCollegeMasterBatchDMO.Single(d => d.ACMB_Id == data.ACMB_Id);
                        query.ACMNS_Order = data.ACMNS_Order;
                        query.ACMSN_SessionName = data.ACMSN_SessionName;
                        query.UpdatedDate = DateTime.Now;
                        _ClgAdmissionContext.Update(query);
                        var flag = _ClgAdmissionContext.SaveChanges();
                        if (flag > 0)
                        {
                            data.returnval = true;
                            data.message = "Update";
                        }
                        else
                        {
                            data.returnval = true;
                            data.message = "Update";
                        }
                    }
                }
                else
                {
                    var query01 = _ClgAdmissionContext.AdmCollegeMasterBatchDMO.Where(q => q.ACMSN_SessionName == data.ACMSN_SessionName && q.MI_Id == data.MI_Id).ToArray();

                    var query012 = _ClgAdmissionContext.AdmCollegeMasterBatchDMO.Where(q => q.ACMNS_Order == data.ACMNS_Order && q.MI_Id == data.MI_Id).ToArray();

                    if (query01.Length > 0 || query012.Length > 0)
                    {
                        data.returnval = false;
                        data.message = "Duplicate";
                    }
                    else
                    {
                        AdmCollegeMasterBatchDMO obj = AutoMapper.Mapper.Map<AdmCollegeMasterBatchDMO>(data);
                        obj.ACMSN_ActiveFlag = true;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
                        _ClgAdmissionContext.Add(obj);
                       var i= _ClgAdmissionContext.SaveChanges();
                        if (i > 0)
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
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
