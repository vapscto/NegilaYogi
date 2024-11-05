using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public class masterTemplateImpl : Interfaces.masterTemplateInterface
    {
        private static ConcurrentDictionary<string, MasterTemplateDTO> _login =
            new ConcurrentDictionary<string, MasterTemplateDTO>();

        public MasterTemplateContext _mastertemplateContext;

        public masterTemplateImpl(MasterTemplateContext mastertemplateContext)
        {
            _mastertemplateContext = mastertemplateContext;
        }
        public MasterTemplateDTO getAllDetails(MasterTemplateDTO templt)
        {
            try
            {
                List<MasterPage> allMasterPage = new List<MasterPage>();
                allMasterPage = _mastertemplateContext.masterpage.ToList();
                templt.pageDrpdwn = allMasterPage.ToArray();

                //List<MasterTemplate> allmasterTemplate = new List<MasterTemplate>();
                //allmasterTemplate = _mastertemplateContext.mastertemplate.ToList();
                //templt.templateList = allmasterTemplate.ToArray();
                templt.templateList = (from a in _mastertemplateContext.mastertemplate
                                       from b in _mastertemplateContext.masterpage
                                       where a.IVRMP_Id == b.IVRMP_Id
                                       select new MasterTemplateDTO
                                       {
                                           IVRMT_Name = a.IVRMT_Name,
                                           IVRMT_Description = a.IVRMT_Description,
                                           IVRMP_Id = a.IVRMP_Id,
                                           IVRMT_Id = a.IVRMT_Id,
                                           IVRMMP_PageName = b.IVRMMP_PageName,
                                           Is_Active = a.Is_Active
                                       }).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return templt;
        }
        public MasterTemplateDTO getSaletypes(int id)
        {

            MasterTemplateDTO data = new MasterTemplateDTO();
          
            try
            {
                using (var cmd = _mastertemplateContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TSAT_Registration_New";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader =  cmd.ExecuteReader())
                        {
                            while ( dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_Saletypes = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public MasterTemplateDTO getdetails(int id)
        {
            MasterTemplateDTO mastr = new MasterTemplateDTO();
            try
            {
                List<MasterTemplate> lorg = new List<MasterTemplate>();
                lorg = _mastertemplateContext.mastertemplate.AsNoTracking().Where(t => t.IVRMT_Id.Equals(id)).ToList();
                mastr.templateList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return mastr;
        }
        public MasterTemplateDTO saveTempldet(MasterTemplateDTO templ)
        {
            try
            {
                MasterTemplate enq = Mapper.Map<MasterTemplate>(templ);
                if (enq.IVRMT_Id > 0)
                {
                    var result = _mastertemplateContext.mastertemplate.Single(t => t.IVRMT_Id == enq.IVRMT_Id);
                  
                    //added by 02/02/2017
                    templ.UpdatedDate = DateTime.Now;
                    Mapper.Map(templ, result);
                    _mastertemplateContext.Update(result);
                    var flag =_mastertemplateContext.SaveChanges();
                    if(flag==1)
                    {
                        templ.returnval = "true";
                    }
                    else
                    {
                        templ.returnval = "false";
                    }
                }
                else
                {
                    var duplicatecount = _mastertemplateContext.mastertemplate.Where(t => t.IVRMT_Name == templ.IVRMT_Name).Count();
                    if (duplicatecount > 0)
                    {
                        templ.returnval = "duplicate";
                    }else
                    {
                        //added by 02/02/2017
                        enq.CreatedDate = DateTime.Now;
                        enq.UpdatedDate = DateTime.Now;
                        enq.Is_Active = true;
                        _mastertemplateContext.Add(enq);
                        var flag = _mastertemplateContext.SaveChanges();
                        if (flag == 1)
                        {
                            templ.returnval = "true";
                        }
                        else
                        {
                            templ.returnval = "false";
                        }
                    }
                   
                }

               

                templ.templateList = (from a in _mastertemplateContext.mastertemplate
                                       from b in _mastertemplateContext.masterpage
                                       where a.IVRMP_Id == b.IVRMP_Id
                                       select new MasterTemplateDTO
                                       {
                                           IVRMT_Name = a.IVRMT_Name,
                                           IVRMT_Description = a.IVRMT_Description,
                                           IVRMP_Id = a.IVRMP_Id,
                                           IVRMT_Id = a.IVRMT_Id,
                                           IVRMMP_PageName = b.IVRMMP_PageName,
                                           Is_Active = a.Is_Active
                                       }).ToArray();


            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return templ;
        }

        public MasterTemplateDTO deleterec(int id)
        {
            
            MasterTemplateDTO org = new MasterTemplateDTO();
            List<MasterTemplate> lorg = new List<MasterTemplate>();

            try
            {
                var result = _mastertemplateContext.mastertemplate.Single(t => t.IVRMT_Id == id);

                if (result.Is_Active == true)
                {
                    result.Is_Active = false;
                }
                else if (result.Is_Active == false)
                {
                    result.Is_Active = true;
                }
                result.UpdatedDate = DateTime.Now;

                _mastertemplateContext.Update(result);
                var flag = _mastertemplateContext.SaveChanges();
                if (flag > 0)
                {
                    if (result.Is_Active == true)
                    {

                        org.returnval = "Activated";
                    }
                    else
                    {
                        org.returnval = "Deactivated";
                    }
                }
                else
                {
                    org.returnval = "fail";
                }

                org.templateList = (from a in _mastertemplateContext.mastertemplate
                                      from b in _mastertemplateContext.masterpage
                                      where a.IVRMP_Id == b.IVRMP_Id
                                      select new MasterTemplateDTO
                                      {
                                          IVRMT_Name = a.IVRMT_Name,
                                          IVRMT_Description = a.IVRMT_Description,
                                          IVRMP_Id = a.IVRMP_Id,
                                          IVRMT_Id = a.IVRMT_Id,
                                          IVRMMP_PageName = b.IVRMMP_PageName,
                                           Is_Active = a.Is_Active
                                            
                                      }).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return org;
        }
        public SatRegistrationDTO satregistration(SatRegistrationDTO data)
        {
            try
            {
                SatRegistrationDMO register = new SatRegistrationDMO();

               
                register.PASRE_FullName = data.PASRE_FullName;
                register.PASRE_EmailId = data.PASRE_EmailId;
                register.PASRE_FatherName = data.PASRE_FatherName;
                register.PASRE_SchoolName = data.PASRE_SchoolName;
                register.PASRE_Gender = data.PASRE_Gender;
                register.PASRE_Address = data.PASRE_Address;
                register.PASRE_MobileNo = data.PASRE_MobileNo;
                register.PASRE_WhatsappNo = data.PASRE_WhatsappNo;
                register.MI_Id = data.MI_Id;
                _mastertemplateContext.Add(register);

                int val = _mastertemplateContext.SaveChanges();

                if (val > 0)
                {
                    data.returnflag = true;
                }
                else
                {
                    data.returnflag = false;
                }

            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

    }
}
