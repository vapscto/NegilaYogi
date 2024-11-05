using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using System;
using DomainModel.Model.com.vaps.admission;
using AutoMapper;
using PreadmissionDTOs.com.vaps.admission;



namespace AdmissionServiceHub.com.vaps.Services
{
    public class MasterSmsEmailParameterImpl : Interfaces.MasterSmsEmailParameterInterface
    {
        private static ConcurrentDictionary<string, MasterSmsEmailParameterDTO> _login =
        new ConcurrentDictionary<string, MasterSmsEmailParameterDTO>();

        private readonly castecategoryContext _Context;


        public MasterSmsEmailParameterImpl(castecategoryContext castecategoryContext)
        {
            _Context = castecategoryContext;
        }

        public MasterSmsEmailParameterDTO GetcastecategoryData(MasterSmsEmailParameterDTO data)
        {
            try
            {
                data.parameterlist = _Context.SMS_MAIL_PARAMETER_DMO.Distinct().OrderByDescending(e=>e.ISMP_ID).ToArray();


            }
            catch (Exception s)
            {

                throw s;
            }

            
            return data;
        }

        public MasterSmsEmailParameterDTO edit(MasterSmsEmailParameterDTO data)
        {
            data.editlist = _Context.SMS_MAIL_PARAMETER_DMO.Where(e => e.ISMP_ID == data.ISMP_ID).ToArray();


            return data;
        }

        public MasterSmsEmailParameterDTO deletedata(int ID)
        {
        MasterSmsEmailParameterDTO data = new MasterSmsEmailParameterDTO();
            var deletelist = _Context.SMS_MAIL_PARAMETER_DMO.Where(e => e.ISMP_ID == ID).ToList();
            if (deletelist.Count>0)
            {
                foreach (var item in deletelist)
                {
                    _Context.Remove(item);
                }

                int i = _Context.SaveChanges();
                if (i>0)
                {
                    data.returnVal = true;
                }
                else
                {
                    data.returnVal = false;
                }
            }
            return data;
        }

        public MasterSmsEmailParameterDTO Savedata(MasterSmsEmailParameterDTO mas)
        {


            if (mas.ISMP_ID != 0)
            {
                var checkDuplicates1 = _Context.SMS_MAIL_PARAMETER_DMO.Where(d => d.ISMP_NAME.Trim().ToLower() == mas.ISMP_NAME.Trim().ToLower() && d.ISMP_ID != mas.ISMP_ID).ToList();
                if (checkDuplicates1.Count == 0)
                {
                    var result = _Context.SMS_MAIL_PARAMETER_DMO.Single(t => t.ISMP_ID == mas.ISMP_ID);


                    result.ISMP_NAME = mas.ISMP_NAME.Trim();
                    result.ISMP_ParameterDesc = mas.ISMP_ParameterDesc;
                    result.ISMP_Query = mas.ISMP_Query;
                    result.UpdatedDate = DateTime.Now;
                    _Context.Update(result);
                    int n = _Context.SaveChanges();
                    if (n > 0)
                    {
                        mas.messageupdate = "Update";
                        mas.returnVal = true;
                    }
                    else
                    {
                        mas.messageupdate = "Update";
                        mas.returnVal = false;
                    }
                }
                
                else
                {
                    mas.message = "data Already Exists";
                }
                
            }
            else
            {
                SMS_MAIL_PARAMETER_DMO MM3 = new SMS_MAIL_PARAMETER_DMO();
                var checkDuplicates1 = _Context.SMS_MAIL_PARAMETER_DMO.Where(d => d.ISMP_NAME.Trim().ToLower() == mas.ISMP_NAME.Trim().ToLower()).ToList();
                if (checkDuplicates1.Count > 0)
                {
                    mas.message = "data Already Exists";
                }
                else
                {
                    MM3.ISMP_NAME = mas.ISMP_NAME.Trim();
                    MM3.ISMP_ParameterDesc = mas.ISMP_ParameterDesc;
                    MM3.ISMP_Query = mas.ISMP_Query;
                    MM3.CreatedDate = DateTime.Now;
                    MM3.UpdatedDate = DateTime.Now;
                    _Context.Add(MM3);
                    int n = _Context.SaveChanges();
                    if (n > 0)
                    {
                        mas.messageupdate = "Save";
                        mas.returnVal = true;
                    }
                    else
                    {
                        mas.messageupdate = "Save";
                        mas.returnVal = false;
                    }
                }
            }
           
            return mas;
        }



        ///HTML TEMPLATE
        ///
        public MasterSmsEmailParameterDTO htmlGetcastecategoryData(MasterSmsEmailParameterDTO data)
        {
            try
            {

                data.parameterlist = _Context.IVRM_Master_HTMLTemplatesDMO.Where(a=>a.MI_Id==data.MI_Id).Distinct().OrderBy(e => e.ISMHTML_HTMLName).ToArray();

                data.parms = _Context.SMS_MAIL_PARAMETER_DMO.Distinct().OrderByDescending(e => e.ISMP_ID).OrderBy(e=>e.ISMP_NAME).Distinct().ToArray();
            }
            catch (Exception s)
            {

                throw s;
            }


            return data;
        }

        public MasterSmsEmailParameterDTO htmledit(MasterSmsEmailParameterDTO data)
        {
            data.editlist = _Context.IVRM_Master_HTMLTemplatesDMO.Where(e => e.ISMHTML_Id == data.ISMHTML_Id).ToArray();


            return data;
        }

        public MasterSmsEmailParameterDTO htmldeletedata(MasterSmsEmailParameterDTO data)
        {
          
            var deletelist = _Context.IVRM_Master_HTMLTemplatesDMO.Single(e => e.ISMHTML_Id == data.ISMHTML_Id);
            if (deletelist.ISMHTML_ActiveFlg==true)
            {
                deletelist.ISMHTML_ActiveFlg = false;

            }
            else
            {
                deletelist.ISMHTML_ActiveFlg = true;
            }

            int i = _Context.SaveChanges();
            if (i > 0)
            {
                data.returnVal = true;
            }
            else
            {
                data.returnVal = false;
            }
            return data;
        }

        public MasterSmsEmailParameterDTO htmlSavedata(MasterSmsEmailParameterDTO mas)
        {


            if (mas.ISMHTML_Id != 0)
            {
                var checkDuplicates1 = _Context.IVRM_Master_HTMLTemplatesDMO.Where(d => d.ISMHTML_HTMLName.Trim().ToLower() == mas.ISMHTML_HTMLName.Trim().ToLower() && d.ISMHTML_Id != mas.ISMHTML_Id && d.MI_Id==mas.MI_Id).ToList();
                if (checkDuplicates1.Count == 0)
                {
                    var result = _Context.IVRM_Master_HTMLTemplatesDMO.Single(t => t.ISMHTML_Id == mas.ISMHTML_Id);


                    result.ISMHTML_HTMLTemplate = mas.ISMHTML_HTMLTemplate.Trim();
                    result.ISMHTML_HTMLName = mas.ISMHTML_HTMLName.Trim();
                    result.ISMHTML_UpdatedBy = mas.User_id;
                    result.ISMHTML_UpdatedDate = DateTime.Now;
                    _Context.Update(result);
                    int n = _Context.SaveChanges();
                    if (n > 0)
                    {
                        mas.messageupdate = "Update";
                        mas.returnVal = true;
                    }
                    else
                    {
                        mas.messageupdate = "Update";
                        mas.returnVal = false;
                    }
                }

                else
                {
                    mas.message = "data Already Exists";
                }

            }
            else
            {
                IVRM_Master_HTMLTemplatesDMO MM3 = new IVRM_Master_HTMLTemplatesDMO();
                var checkDuplicates1 = _Context.IVRM_Master_HTMLTemplatesDMO.Where(d => d.ISMHTML_HTMLName.Trim().ToLower() == mas.ISMHTML_HTMLName.Trim().ToLower() && d.MI_Id == mas.MI_Id).ToList();
                if (checkDuplicates1.Count > 0)
                {
                    mas.message = "data Already Exists";
                }
                else
                {
                    MM3.ISMHTML_HTMLTemplate = mas.ISMHTML_HTMLTemplate.Trim();
                    MM3.ISMHTML_HTMLName = mas.ISMHTML_HTMLName.Trim();
                    MM3.MI_Id = mas.MI_Id;
                    MM3.ISMHTML_UpdatedBy = mas.User_id;
                    MM3.ISMHTML_CreatedBy = mas.User_id;
                    MM3.ISMHTML_UpdatedDate = DateTime.Now;
                    MM3.ISMHTML_CreatedDate = DateTime.Now;
                    MM3.ISMHTML_ActiveFlg = true;
                    _Context.Add(MM3);
                    int n = _Context.SaveChanges();
                    if (n > 0)
                    {
                        mas.messageupdate = "Save";
                        mas.returnVal = true;
                    }
                    else
                    {
                        mas.messageupdate = "Save";
                        mas.returnVal = false;
                    }
                }
            }

            return mas;
        }

    }
}
