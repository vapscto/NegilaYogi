using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.admission;
using DomainModel.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class ClasssectionorderImpl :Interfaces.ClasssectionorderInterface
    {
        public ClasssectionorderContext _db;
        private readonly ILogger _log;
        public ClasssectionorderImpl(ClasssectionorderContext db, ILogger<AdmissionImportImpl> acdimpl , ILogger<StudentAdmissionImp> loggerFactor)
        {                  
            _db = db;
            _log = loggerFactor;
          
        }
        public ClasssectionorderDTO getdetails(int id)
        {
            ClasssectionorderDTO data = new ClasssectionorderDTO();
            //data.classdetails = _db.accclass.Where(t => t.MI_Id == id && t.ASMCL_ActiveFlag == true).ToArray();
           // data.sectiondetails = _db.accsection.Where(t => t.MI_Id == id && t.ASMC_ActiveFlag == 1).ToArray();

            data.classdetails = (from a in _db.accclass
                                 where (a.MI_Id == id && a.ASMCL_ActiveFlag == true)
                                 select a).OrderBy(a => a.ASMCL_Order).ToArray();

            data.sectiondetails = (from a in _db.accsection
                                   where (a.MI_Id == id && a.ASMC_ActiveFlag == 1)
                                 select a).OrderBy(a => a.ASMC_Order).ToArray();
            return data;
        }

        public ClasssectionorderDTO save(ClasssectionorderDTO data)
        {
            try
            {
                _log.LogInformation("class order");
                int id = 0;
                if (data.flagclass == "class")
                {
                    for (int i = 0; i < data.classorder.Count; i++)
                    {
                        _log.LogInformation("enter in class order");
                        var reult = _db.accclass.Single(t => t.MI_Id == data.miid && t.ASMCL_Id == data.classorder[i].ASMCL_Id);
                        id = id + 1;
                        _log.LogInformation("class_id_order:'" + id + "'");
                        if (i == 0)
                        {
                            _log.LogInformation("enter i if condiition class");
                            reult.ASMCL_Order = id;
                        }
                        else
                        {
                            _log.LogInformation("enter i else condiition class");
                            reult.ASMCL_Order = id;
                        }
                        _db.Update(reult);
                        var flag = _db.SaveChanges();
                        if (flag > 0)
                        {
                            _log.LogInformation("data saved successful class ");
                            data.retruval = true;
                        }
                        else
                        {
                            _log.LogInformation("data not saved successful class");
                            data.retruval = false;
                        }
                    }
                }
                //changing the class order 
               
                _log.LogInformation("out of class order");

                //changing the section order 

                _log.LogInformation("section order");

                int ids = 0;
                if (data.flagsec == "section")
                {
                    for (int k = 0; k < data.secorder.Count; k++)
                    {
                        _log.LogInformation("enter in section order");
                        var reult1 = _db.accsection.Single(t => t.MI_Id == data.miid && t.ASMS_Id == data.secorder[k].ASMS_Id);
                        ids = ids + 1;
                        _log.LogInformation("section_id_order:'" + ids + "'");
                        if (k == 0)
                        {
                            _log.LogInformation("enter k if condiition section");
                            reult1.ASMC_Order = ids;
                        }
                        else
                        {
                            _log.LogInformation("enter i else condiition section");
                            reult1.ASMC_Order = ids;
                        }
                        //Mapper.Map(data, reult1);
                        _db.Update(reult1);
                        var flag = _db.SaveChanges();
                        if (flag > 0)
                        {
                            _log.LogInformation("data saved successful section ");
                            data.retruval = true;
                        }
                        else
                        {
                            _log.LogInformation("data not saved successful section");
                            data.retruval = false;
                        }
                    }
                }
              
                _log.LogInformation("out of section order");
            }
            catch (Exception ex)
            {
                _log.LogError("Error class_section_order:'" + ex.Message + "'");
            }
            return data;
        }

    }
}
