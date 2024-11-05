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
using CommonLibrary;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class EMAILSMSTemplateSettingImpl : Interfaces.EMAILSMSTemplateSettingInterface
    {
        private static ConcurrentDictionary<string, EMAILSMSTemplateSettingDTO> _login =
        new ConcurrentDictionary<string, EMAILSMSTemplateSettingDTO>();

        private readonly MasterActivityContext _MasterActivityContext;
        private readonly DomainModelMsSqlServerContext _context;

        public EMAILSMSTemplateSettingImpl(MasterActivityContext MasterActivityContext, DomainModelMsSqlServerContext context)
        {
            _MasterActivityContext = MasterActivityContext;
            _context = context;
        }

        public EMAILSMSTemplateSettingDTO Getdetails(EMAILSMSTemplateSettingDTO data)
        {
            try
            {
                data.modulellist = _MasterActivityContext.MasterModule.Where(a => a.Module_ActiveFlag == 1).OrderBy(r => r.IVRMM_ModuleName).ToArray
               ();

                string accountname = "";
                string accesskey = "";
                ReadTemplateFromAzure obj = new ReadTemplateFromAzure();

                var datatstu = _context.IVRM_Storage_path_Details.ToList();
                if (datatstu.Count() > 0)
                {
                    accountname = datatstu.FirstOrDefault().IVRM_SD_Access_Name;
                    accesskey = datatstu.FirstOrDefault().IVRM_SD_Access_Key;
                }


                string html = obj.getHtmlContentFromAzure(accountname, accesskey, "busform/" + data.MI_Id, "Busform.html", 0);
                data.htmldata = html;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }

        public EMAILSMSTemplateSettingDTO GetSelectedRowDetails(int ID)
        {
            EMAILSMSTemplateSettingDTO EMAILSMSTemplateSettingDTO = new EMAILSMSTemplateSettingDTO();
            //List<MasterActivityDMO> lorg = new List<MasterActivityDMO>();
            //lorg = _MasterActivityContext.MasterActivityDMO.Where(t => t.AMA_Id.Equals(ID)).ToList();
            //EMAILSMSTemplateSettingDTO.MasterActivityname = lorg.ToArray();
            return EMAILSMSTemplateSettingDTO;
        }

        public EMAILSMSTemplateSettingDTO MasterDeleteModulesData(int ID)
        {
            EMAILSMSTemplateSettingDTO EMAILSMSTemplateSettingDTO = new EMAILSMSTemplateSettingDTO();
            List<MasterActivityDMO> masters = new List<MasterActivityDMO>();
            //try
            //{
            //    var check_activty_used = _MasterActivityContext.StudentActitvityDMO.Where(t => t.AMA_Id == ID).ToList();
            //    if (check_activty_used.Count == 0)
            //    {
            //        masters = _MasterActivityContext.MasterActivityDMO.Where(t => t.AMA_Id.Equals(ID)).ToList();
            //        if (masters.Any())
            //        {
            //            _MasterActivityContext.Remove(masters.ElementAt(0));
            //            _MasterActivityContext.SaveChanges();
            //        }
            //        else
            //        {

            //        }
            //    }
            //    else
            //    {
            //        EMAILSMSTemplateSettingDTO.message = "Delete";
            //    }
            //}catch(Exception ex)
            //{
            //    EMAILSMSTemplateSettingDTO.message = "Delete";
            //    Console.WriteLine(ex.Message);
            //}
            return EMAILSMSTemplateSettingDTO;
        }


        public EMAILSMSTemplateSettingDTO MasterActivityData(EMAILSMSTemplateSettingDTO mas)
        {

            MasterActivityDMO MM = Mapper.Map<MasterActivityDMO>(mas);
            if (mas.AMA_Id != 0)
            {
                var checkDuplicates1 = _MasterActivityContext.MasterActivityDMO.Where(t => t.AMA_Activity.Equals(mas.AMA_Activity) && t.AMA_Id != mas.AMA_Id).ToList();

                if (checkDuplicates1.Count > 0)
                {
                    mas.message = "Record Already Exist";
                }
                else
                {

                    var result = _MasterActivityContext.MasterActivityDMO.Single(t => t.AMA_Id == mas.AMA_Id);
                    result.AMA_Activity = mas.AMA_Activity;
                    result.AMA_Activity_Desc = mas.AMA_Activity_Desc;
                    result.UpdatedDate = DateTime.Now;
                    result.CreatedDate = result.CreatedDate;
                    result.MI_Id = mas.MI_Id;
                    _MasterActivityContext.Update(result);
                    int n = _MasterActivityContext.SaveChanges();
                    if (n > 0)
                    {
                        mas.returnval = true;
                        mas.messageupdate = "Update";
                    }
                    else
                    {
                        mas.returnval = false;
                        mas.messageupdate = "Update";
                    }
                }
            }
            else
            {
                MasterActivityDMO MM3 = new MasterActivityDMO();
                MM.CreatedDate = DateTime.Now;
                MM.UpdatedDate = DateTime.Now;
                Array[] showdata1 = new Array[1];

                var checkDuplicates = _MasterActivityContext.MasterActivityDMO.Where(t => t.AMA_Activity.Equals(mas.AMA_Activity)).ToList();

                if (checkDuplicates.Count > 0)
                {
                    mas.message = "Record Already Exist";
                }
                else
                {
                    MM3.AMA_Activity = mas.AMA_Activity;
                    MM3.AMA_Activity_Desc = mas.AMA_Activity_Desc;
                    MM3.MI_Id = mas.MI_Id;
                    MM3.CreatedDate = DateTime.Now;
                    MM3.UpdatedDate = DateTime.Now;
                    _MasterActivityContext.Add(MM3);
                    int n = _MasterActivityContext.SaveChanges();
                    if (n > 0)
                    {
                        mas.returnval = true;
                        mas.messageupdate = "Save";
                    }
                    else
                    {
                        mas.returnval = false;
                        mas.messageupdate = "Save";
                    }
                }


            }
            return mas;
        }
    }
}
