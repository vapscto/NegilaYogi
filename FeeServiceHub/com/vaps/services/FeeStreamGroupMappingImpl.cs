using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using FeeServiceHub.com.vaps.interfaces;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.IO;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using PreadmissionDTOs.com.vaps.admission;
using DomainModel.Model.com.vaps.admission;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using CommonLibrary;
using DomainModel.Model.com.vapstech.Fee;
using Microsoft.Extensions.Logging;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeStreamGroupMappingImpl : interfaces.FeeStreamGroupMappingInterface
    {
        public FeeGroupContext _FeeGroupContext;
        public DomainModelMsSqlServerContext _db;
        public ILogger<FeeStreamGroupMappingDTO> _log;
        private object _logger;

        public FeeStreamGroupMappingImpl(FeeGroupContext frgContext, DomainModelMsSqlServerContext db)
        {
            _FeeGroupContext = frgContext;
            _db = db;
        }


        public FeeStreamGroupMappingDTO getData(FeeStreamGroupMappingDTO data)
        {

            try
            {
                data.yearlist = _FeeGroupContext.AcademicYear.Where(y => y.MI_Id == data.MI_Id && y.Is_Active == true).OrderByDescending(y=>y.ASMAY_Order).ToArray();
                data.classdetails = _FeeGroupContext.School_M_Class.Where(c => c.MI_Id == data.MI_Id && c.ASMCL_ActiveFlag == true).ToArray();
                data.groupdetails = _FeeGroupContext.feeGroup.Where(g => g.MI_Id == data.MI_Id && g.FMG_ActiceFlag == true).ToArray();
                data.stream = (from a in _FeeGroupContext.Pre_Adm_Syllabus.Where(c => c.MI_ID == data.MI_Id)  select a).ToArray();

                data.getdeatils = (from a in _FeeGroupContext.Fee_Master_Stream_Group_MappingDMO
                                   from b in _FeeGroupContext.AcademicYear
                                   from c in _FeeGroupContext.School_M_Class
                                   from d in _FeeGroupContext.feeGroup
                                   from e in _FeeGroupContext.Pre_Adm_Syllabus
                                   where (a.MI_Id == data.MI_Id && b.MI_Id == a.MI_Id && c.MI_Id == a.MI_Id && d.MI_Id == a.MI_Id && e.MI_ID == a.MI_Id && b.ASMAY_Id == a.ASMAY_Id && c.ASMCL_Id == a.ASMCL_ID && d.FMG_Id == a.FMG_Id && e.PASL_ID == a.PASL_ID)
                                   select new FeeStreamGroupMappingDTO
                                   {
                                       FMSGM_Id = a.FMSGM_Id,
                                       FMG_Id = a.FMG_Id,
                                       ASMAY_Id = a.ASMAY_Id,
                                       PASL_ID = a.PASL_ID,
                                       ASMCL_Id = a.ASMCL_ID,
                                       ASMAY_Year = b.ASMAY_Year,
                                       ASMCL_ClassName = c.ASMCL_ClassName,
                                       FMG_GroupName = d.FMG_GroupName,
                                       PASL_Name = e.PASL_Name,
                                       FMSGM_Active = a.FMSGM_Active

                                   }).Distinct().ToArray();




            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


        public FeeStreamGroupMappingDTO saveData(FeeStreamGroupMappingDTO data)
        {
            try
            {
                if (data.FMSGM_Id > 0)
                {
                    Fee_Master_Stream_Group_MappingDMO feegrpmap = new Fee_Master_Stream_Group_MappingDMO();
                    feegrpmap.MI_Id = data.MI_Id;
                    for (int i = 0; i < data.TempararyArrayList.Length; i++)
                    {
                        for (int j = 0; j < data.TempararyArrayList1.Length; j++)
                        {
                            feegrpmap.FMG_Id = data.TempararyArrayList[0].FMG_Id;
                            //    feegrpmap.FMSGM_Id = data.FMSGM_Id;
                            feegrpmap.FMSGM_Active = 1;
                            feegrpmap.PASL_ID = data.TempararyArrayList1[0].PASL_ID;
                            feegrpmap.ASMAY_Id = data.ASMAY_Id;
                            feegrpmap.ASMCL_ID = data.ASMCL_Id;
                            feegrpmap.FMSG_CreatedDate = DateTime.Now;
                            feegrpmap.FMSG_UpdatedDate = DateTime.Now;
                            _FeeGroupContext.Add(feegrpmap);
                            int n = _FeeGroupContext.SaveChanges();
                            if (n > 0)
                            {
                                data.message = "Update";
                                data.retrunval = true;
                            }
                            else
                            {
                                data.message = "Update";
                                data.retrunval = false;
                            }

                        }

                    }
                }
                else
                {
                    int j = 0;
                    while (j < data.TempararyArrayList.Count())
                    {
                        Fee_Master_Stream_Group_MappingDMO feegrpmap = new Fee_Master_Stream_Group_MappingDMO();

                        feegrpmap.MI_Id = data.MI_Id;
                        feegrpmap.FMG_Id = data.TempararyArrayList[j].FMG_Id;
                        // feegrpmap.FMSGM_Id = data.FMSGM_Id;
                        feegrpmap.FMSGM_Active = 1;
                        feegrpmap.PASL_ID = data.TempararyArrayList1[0].PASL_ID;
                        feegrpmap.ASMAY_Id = data.ASMAY_Id;
                        feegrpmap.ASMCL_ID = data.ASMCL_Id;
                        feegrpmap.FMSG_CreatedDate = DateTime.Now;
                        feegrpmap.FMSG_UpdatedDate = DateTime.Now;
                        _FeeGroupContext.Add(feegrpmap);
                        j++;
                    }
                  
                    int n = _FeeGroupContext.SaveChanges();
                    if (n > 0)
                    {
                        data.message = "Saved";
                        data.retrunval = true;
                    }
                    else
                    {
                        data.message = "Add";
                        data.retrunval = false;
                    }
                }

            }

            catch (Exception ex)
            {
                _log.LogInformation("error in fee stream group mapping  savedata" + ex.Message);
            }
            return data;
        }


        public FeeStreamGroupMappingDTO deactivate(FeeStreamGroupMappingDTO id)
        {
            try
            {
               // Fee_Master_Stream_Group_MappingDMO feepge = Mapper.Map<Fee_Master_Stream_Group_MappingDMO>(id);
               // Fee_Master_Stream_Group_MappingDMO feepge = new Fee_Master_Stream_Group_MappingDMO();
                if (id.FMSGM_Id > 0)
                {
                    var result = _FeeGroupContext.Fee_Master_Stream_Group_MappingDMO.Single(t => t.FMSGM_Id == id.FMSGM_Id);
                    result.FMSG_UpdatedDate = DateTime.Now;
                    if (result.FMSGM_Active == 1)
                    {
                        result.FMSGM_Active = 0;
                    }
                    else
                    {
                        result.FMSGM_Active = 1;
                    }
                    _FeeGroupContext.Update(result);
                    var flag = _FeeGroupContext.SaveChanges();
                    if (flag == 1)
                    {
                        id.returnval = true;
                    }
                    else
                    {
                        id.returnval = false;
                    }
                }
            }
            catch (Exception e)
            {
              //  _logger.LogError(e.Message);
                Console.WriteLine(e.InnerException);
            }
            return id;
        }



        public FeeStreamGroupMappingDTO Editdetails(int id)
        {
            FeeStreamGroupMappingDTO FMG = new FeeStreamGroupMappingDTO();
            try
            {
                List<Fee_Master_Stream_Group_MappingDMO> masterfeegroup = new List<Fee_Master_Stream_Group_MappingDMO>();
                masterfeegroup = _FeeGroupContext.Fee_Master_Stream_Group_MappingDMO.AsNoTracking().Where(t => t.FMSGM_Id.Equals(id)).ToList();

                FMG.GroupData = masterfeegroup.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return FMG;
        }


    }
}




