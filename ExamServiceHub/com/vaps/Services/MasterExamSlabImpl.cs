
using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.COE;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model;
using DomainModel.Model.com.vaps.Exam;
using DomainModel.Model.com.vapstech.COE;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Services
{
    public class MasterExamSlabImpl : Interfaces.MasterExamSlabInterface
    {
        private static ConcurrentDictionary<string, MasterExamSlabDTO> _login = new ConcurrentDictionary<string, MasterExamSlabDTO>();

        public ExamContext _examcontext;
        ILogger<MasterExamSlabImpl> _acdimpl;
        public MasterExamSlabImpl(ExamContext ttcategory)
        {
            _examcontext = ttcategory;
        }
        public MasterExamSlabDTO savedetail(MasterExamSlabDTO data)
        {


            try
            {
                if (data.SlabLists != null && data.SlabLists.Length > 0)
                {
                    var SlbCount = _examcontext.Exm_Master_slabDMO.Where(t => t.MI_Id == data.MI_Id).ToList();
                    if(SlbCount!=null &&  SlbCount.Count > 0)
                    {
                        foreach (var f in SlbCount)
                        {
                            f.EMPTSL_ActiveFlg = false;
                            f.EMPTSL_UpdatedDate = DateTime.Now;
                            f.EMPTSL_UpdatedBy = data.UserId;
                            _examcontext.Update(f);
                            //_examcontext.Remove(f);
                        }
                    }
                    foreach (var d in data.SlabLists)
                    {

                        Exm_Master_slabDMO obj = new Exm_Master_slabDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.EMPTSL_PercentFrom = d.EMPTSL_PercentFrom;
                        obj.EMPTSL_PercentTo = d.EMPTSL_PercentTo;
                        obj.EMPTSL_Points = d.EMPTSL_Points;
                        obj.EMPTSL_ActiveFlg = true;
                        obj.EMPTSL_CreatedBy = data.UserId;
                        obj.EMPTSL_UpdatedBy = data.UserId;
                        obj.EMPTSL_CreatedDate = DateTime.Now;
                        obj.EMPTSL_UpdatedDate = DateTime.Now;
                        _examcontext.Add(obj);

                    }
                    int i = _examcontext.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = "Save";
                    }
                    else
                    {
                        data.returnval = "notsave";
                    }
                }
            }
            catch (Exception ee)
            {
                _acdimpl.LogError(ee.Message);
                _acdimpl.LogDebug(ee.Message);
                Console.WriteLine(ee.Message);
                data.returnval = "admin";
            }

            return data;
        }



        public MasterExamSlabDTO getdetails(int id)
        {
            MasterExamSlabDTO TTMC = new MasterExamSlabDTO();
            try
            {
                TTMC.GetDetails = _examcontext.Exm_Master_slabDMO.Where(t => t.MI_Id == id &&
                t.EMPTSL_ActiveFlg == true).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return TTMC;
        }

    }
}