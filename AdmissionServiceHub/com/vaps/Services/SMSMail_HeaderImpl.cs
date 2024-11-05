using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.admission;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class SMSMail_HeaderImpl:Interfaces.SMSMail_HeaderInterface
    {
        public DomainModelMsSqlServerContext _Context;
        public SMSMail_HeaderImpl(DomainModelMsSqlServerContext context)
        {
            _Context = context;
        }
        public SMSMail_HeaderDTO getalldetails(SMSMail_HeaderDTO data)
        {
            try
            {
                data.alldata = _Context.SMSMail_HeaderDMO.Distinct().ToArray();

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public SMSMail_HeaderDTO getdata(SMSMail_HeaderDTO data)
        {
            try
            {
                if(data.ISMH_Id==0)
                {
                    var duplicate = _Context.SMSMail_HeaderDMO.Where(t => t.ISMH_HeaderName.Trim().ToLower() == data.ISMH_HeaderName.Trim().ToLower()).Distinct().ToList();
                    if(duplicate.Count()>0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        SMSMail_HeaderDMO a = new SMSMail_HeaderDMO();
                        a.ISMH_HeaderName = data.ISMH_HeaderName;
                        _Context.Add(a);
                    }
                    var w = _Context.SaveChanges();
                    if(w > 0)
                    {
                        data.msg = "Saved";
                    }
                    else
                    {
                        data.msg = "Failed";
                    }
                }
                else if(data.ISMH_Id > 0)
                {
                    var duplicate = _Context.SMSMail_HeaderDMO.Where(t => t.ISMH_Id != data.ISMH_Id && t.ISMH_HeaderName.Trim().ToLower() == data.ISMH_HeaderName.Trim().ToLower()).Distinct().ToArray();
                    if(duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var j = _Context.SMSMail_HeaderDMO.Single(t => t.ISMH_Id == data.ISMH_Id);
                        j.ISMH_HeaderName = data.ISMH_HeaderName;
                        _Context.Update(j);
                    }
                    var s = _Context.SaveChanges();
                    if (s > 0)
                    {
                        data.msg = "updated";
                    }
                    else
                    {
                        data.msg = "failed";
                    }
                        
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public SMSMail_HeaderDTO edittab1(SMSMail_HeaderDTO data)

        {
            try
            {
                data.Editlist = _Context.SMSMail_HeaderDMO.Where(t => t.ISMH_Id == data.ISMH_Id).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public SMSMail_HeaderDTO delete(SMSMail_HeaderDTO data)

        {
            try
            {
               var list = _Context.SMSMail_HeaderDMO.Where(t => t.ISMH_Id == data.ISMH_Id).Distinct().ToList();
                if (list.Count>0)
                {
                    foreach (var item in list)
                    {
                        _Context.Remove(item);
                    }

                    var s = _Context.SaveChanges();
                    if (s > 0)
                    {
                        data.msg = "success";
                    }
                    else
                    {
                        data.msg = "failed";
                    }
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
