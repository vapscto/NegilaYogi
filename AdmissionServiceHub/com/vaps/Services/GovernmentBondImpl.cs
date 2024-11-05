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
    public class GovernmentBondImpl : Interfaces.GovernmentBondInterface
    {
        private static ConcurrentDictionary<string, GovernmentBondDTO> _login =
        new ConcurrentDictionary<string, GovernmentBondDTO>();

        public GovernmentBondContext _GovernmentBondContext;
        public AdmissionFormContext _admission;


        public GovernmentBondImpl(GovernmentBondContext GovernmentBondContext,AdmissionFormContext admission)
        {
            _GovernmentBondContext = GovernmentBondContext;
            _admission = admission;
        }

        public GovernmentBondDTO GetGovernmentBondData(GovernmentBondDTO GovernmentBondDTO)//int IVRMM_Id
        {
            List<GovernmentBondDMO> Allname = new List<GovernmentBondDMO>();
            Allname = _GovernmentBondContext.GovernmentBondDMO.Where(d=>d.MI_Id== GovernmentBondDTO.MI_Id).OrderByDescending(a=>a.CreatedDate).ToList();
            GovernmentBondDTO.GovernmentBondname = Allname.ToArray();
            if(GovernmentBondDTO.GovernmentBondname.Length > 0)
            {
                GovernmentBondDTO.count = GovernmentBondDTO.GovernmentBondname.Length;
            }
            else
            {
                GovernmentBondDTO.count = 0;
            }
            return GovernmentBondDTO;
        }

        public GovernmentBondDTO GetSelectedRowDetails(int ID)
        {
            GovernmentBondDTO GovernmentBondDTO = new GovernmentBondDTO();
            List<GovernmentBondDMO> lorg = new List<GovernmentBondDMO>();
            lorg = _GovernmentBondContext.GovernmentBondDMO.Where(t => t.IMGB_Id==ID).ToList();
            GovernmentBondDTO.GovernmentBondname = lorg.ToArray();
            return GovernmentBondDTO;
        }

        public GovernmentBondDTO MasterDeleteModulesData(GovernmentBondDTO GovernmentBondDTO1)
        {
           // GovernmentBondDTO GovernmentBondDTO = new GovernmentBondDTO();
            List<GovernmentBondDMO> masters = new List<GovernmentBondDMO>();
           
            try
            {
                var ismapped = _admission.MasterStudentBondDMO.Where(d => d.MI_Id == GovernmentBondDTO1.MI_Id && d.IMGB_Id == GovernmentBondDTO1.IMGB_Id).ToList();
                if(ismapped.Count > 0)
                {
                    GovernmentBondDTO1.returnMsg = "Sorry...You can not delete this record.Because this record is mapped with student";
                }
                else
                {
                    masters = _GovernmentBondContext.GovernmentBondDMO.Where(t => t.IMGB_Id == GovernmentBondDTO1.IMGB_Id).ToList();
                    if (masters.Any())
                    {
                        _GovernmentBondContext.Remove(masters.ElementAt(0));
                        var flag = _GovernmentBondContext.SaveChanges();
                        if (flag > 0)
                        {
                            GovernmentBondDTO1.returnMsg = "deleted";
                        }
                        else
                        {
                            GovernmentBondDTO1.returnMsg = "deletefailed";
                        }
                    }
                }
                 
            }
            catch(Exception e)
            {
                GovernmentBondDTO1.returnMsg = "Sorry...You can not delete this record.Because this record is mapped with student";
                Console.WriteLine(e.Message);
               
            }
            return GovernmentBondDTO1;
        }

        public GovernmentBondDTO GovernmentBondData(GovernmentBondDTO mas)
        {
            mas.returnMsg = "";
            try
            {
                GovernmentBondDMO MM = Mapper.Map<GovernmentBondDMO>(mas);
                if (mas.IMGB_Id != 0)
                {
                    var resultCount = _GovernmentBondContext.GovernmentBondDMO.Where(t => t.IMGB_Name == mas.IMGB_Name && t.IMGB_Id != mas.IMGB_Id).Count();
                    if (resultCount > 0)
                    {
                        mas.returnMsg = "duplicate";
                        return mas;
                    }
                    else
                    {
                       
                        var result = _GovernmentBondContext.GovernmentBondDMO.Single(t => t.IMGB_Id == mas.IMGB_Id);
                        //result.IVRMM_Id = mas.IVRMM_Id;

                        result.IMGB_Name = mas.IMGB_Name;
                        result.IMGB_Description = mas.IMGB_Description;
                        result.UpdatedDate = DateTime.Now;
                        _GovernmentBondContext.Update(result);
                        var flag= _GovernmentBondContext.SaveChanges();
                        if(flag > 0)
                        {
                            mas.returnMsg = "update";
                        }
                        else
                        {
                            mas.returnMsg = "updatefailed";
                        }
                    }
                }
                else
                {
                    var resultCount = _GovernmentBondContext.GovernmentBondDMO.Where(t => t.IMGB_Name == mas.IMGB_Name).Count();
                    if (resultCount > 0)
                    {
                        mas.returnMsg = "duplicate";
                        return mas;
                    }
                    else
                    {
                        GovernmentBondDMO MM3 = new GovernmentBondDMO();
                        MM3.IMGB_Name = mas.IMGB_Name;
                        MM3.MI_Id = mas.MI_Id;
                        MM3.IMGB_Description = mas.IMGB_Description;
                        MM3.CreatedDate = DateTime.Now;
                        MM3.UpdatedDate = DateTime.Now;
                        _GovernmentBondContext.Add(MM3);
                        var flag = _GovernmentBondContext.SaveChanges();
                        if(flag > 0)
                        {
                            mas.returnMsg = "add";
                        }
                        else
                        {
                            mas.returnMsg = "addfailed";
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
              //  return mas;
            }

            return mas;
        }

    }
}
