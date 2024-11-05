using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using WebApplication1.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DomainModel.Model;
using AutoMapper;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace WebApplication1.Services
{
    public class InstitutionTemplateImpl :Interfaces.InstitutionTemplateInterface
    {
        private static ConcurrentDictionary<string, StudentApplicationDTO> _login =
             new ConcurrentDictionary<string, StudentApplicationDTO>();

        public DomainModelMsSqlServerContext _db;

        public InstitutionTemplateImpl(DomainModelMsSqlServerContext dmoc)
        {
            _db = dmoc;
        }

        public async Task<CommonDTO> getBasicData(int id)
        {
            CommonDTO cdto = new CommonDTO();
            try
            {
                var aa1 = await _db.Institute.Where(i=>i.MI_ActiveFlag == 1).ToListAsync();
                cdto.InstituteList = aa1.ToArray();

                var aa3 = await _db.mstTemplate.Where(i => i.Is_Active == true).ToListAsync();
                cdto.Templates = aa3.ToArray();

                cdto.InstituteTemplates = (from a in _db.InstitutionTemplate
                                           from b in _db.Institute
                                           from c in _db.mstTemplate
                                           where (a.IVRMIT_MI_Id == b.MI_Id && a.IVRMT_Id == c.IVRMT_Id)
                                           select new InstitutionTemplateDTO {
                                               IVRMIT_Id = a.IVRMIT_Id,
                                               Template_name = c.IVRMT_Name,
                                               IVRMT_Id = a.IVRMT_Id,
                                               Institute_Name = b.MI_Name,
                                               IVRMIT_MI_Id = a.IVRMIT_MI_Id,
                                               IVRMIT_Category_Id = a.IVRMIT_Category_Id,
                                               IVRMIT_ActiveFlag = a.IVRMIT_ActiveFlag,
                                               IVRMIT_DeleteFlag = a.IVRMIT_DeleteFlag
                                           }).ToArray();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return cdto;
        }

        public InstitutionTemplateDTO getEditData(int Id)
        {
            InstitutionTemplateDTO idto = new InstitutionTemplateDTO();
            try
            {
                var aa1 = _db.InstitutionTemplate.Where(d => d.IVRMIT_Id == Id);
                idto.InstEditlist = aa1.ToArray();
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
            }
            return idto;
        }

        public void deleteRec(int id)
        {
            try
            {
                var stureg = _db.InstitutionTemplate.Single(s => s.IVRMIT_Id == id);
                if (id != 0)
                {
                    if (stureg.IVRMIT_DeleteFlag == false)
                    {
                        stureg.IVRMIT_DeleteFlag = true;
                    }
                    else
                    {
                        stureg.IVRMIT_DeleteFlag = false;
                    }
                    //added by 02/02/2017
                    stureg.UpdatedDate = DateTime.Now;
                    _db.Update(stureg);
                    _db.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public void DeactiveActive(int id)
        {
            try
            {
                var stureg = _db.InstitutionTemplate.Single(s => s.IVRMIT_Id == id);
                if (id != 0)
                {
                    if (stureg.IVRMIT_ActiveFlag == false)
                    {
                        stureg.IVRMIT_ActiveFlag = true;
                    }
                    else
                    {
                        stureg.IVRMIT_ActiveFlag = false;
                    }
                    //added by 02/02/2017
                    stureg.UpdatedDate = DateTime.Now;
                    _db.Update(stureg);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public InstitutionTemplateDTO SaveInstituteTemp(InstitutionTemplateDTO InstTemp)
        {
            InstTemp.returnMsg = "";
            try
            {
                InstituteTemplate instTemp = Mapper.Map<InstituteTemplate>(InstTemp);
                if (instTemp.IVRMIT_Id == 0)
                {
                    var inst = _db.InstitutionTemplate.Where(s => s.IVRMT_Id == InstTemp.IVRMT_Id && s.IVRMIT_MI_Id == InstTemp.IVRMIT_MI_Id).ToList();
                    if(inst.Count() <= 0)
                    {
                        instTemp.IVRMIT_ActiveFlag = true;
                        instTemp.IVRMIT_DeleteFlag = false;
                        //added by 02/02/2017
                        instTemp.CreatedDate = DateTime.Now;
                        instTemp.UpdatedDate = DateTime.Now;
                        _db.Add(instTemp);
                        _db.SaveChanges();

                        InstTemp.returnMsg = "add";
                    }
                    else
                    {
                        InstTemp.returnMsg = "duplicate";
                        return InstTemp;
                    }
                }
                else
                {
                    var stureg = _db.InstitutionTemplate.Single(s => s.IVRMIT_Id == instTemp.IVRMIT_Id);
                    //added by 02/02/2017
                   // instTemp.CreatedDate = DateTime.Now;
                    instTemp.UpdatedDate = DateTime.Now;
                    Mapper.Map(InstTemp, stureg);
                    _db.SaveChanges();
                    InstTemp.returnMsg = "update";
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                InstTemp.returnMsg = "error";
                return InstTemp;
            }
            return InstTemp;
        }
    }
}
