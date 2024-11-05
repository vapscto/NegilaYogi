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
using System.Collections.Concurrent;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace WebApplication1.Services
{
    public class MasterModulesImp : Interfaces.MasterModulesInterface
    {
        private static ConcurrentDictionary<string, MasterModulesDTO> _login =
      new ConcurrentDictionary<string, MasterModulesDTO>();

        private readonly MasterModulesContext _MasterModulesContext;


        public MasterModulesImp(MasterModulesContext MasterModulesContext)
        {
            _MasterModulesContext = MasterModulesContext;
        }

        public MasterModulesDTO GetMasterModulesData(MasterModulesDTO MasterModulesDTO)//int IVRMM_Id
        {

            Array[] showdata = new Array[50];
            List<MasterModules> Allname = new List<MasterModules>();
            Allname = _MasterModulesContext.MasterModules.ToList();
            MasterModulesDTO.masterModulesname = Allname.ToArray();

            return MasterModulesDTO;
        }

        public MasterModulesDTO GetSelectedRowDetails(int ID)
        {
            MasterModulesDTO MasterModulesDTO = new MasterModulesDTO();
            List<MasterModules> lorg = new List<MasterModules>();
            lorg = _MasterModulesContext.MasterModules.Where(t => t.IVRMM_Id.Equals(ID)).ToList();
            MasterModulesDTO.masterModulesname = lorg.ToArray();
            return MasterModulesDTO;
        }        

        public MasterModulesDTO MasterDeleteModulesData(int ID)
        {
            MasterModulesDTO MasterModulesDTO = new MasterModulesDTO();
           // List<MasterModules> masters = new List<MasterModules>();
           // var masters =  _MasterModulesContext.MasterModules.Where(t => t.IVRMM_Id.Equals(ID)).ToList();

            //lorg = _OrganisationContext.Organisation.Where(t => t.MO_Id.Equals(id)).ToList();
            var result = _MasterModulesContext.MasterModules.Single(t => t.IVRMM_Id.Equals(ID));

            if (result.Module_ActiveFlag == 1)
            {
                result.Module_ActiveFlag = 0;
            }
            else if (result.Module_ActiveFlag == 0)
            {
                result.Module_ActiveFlag = 1;
            }
            //added by 02/02/2017
           
            result.UpdatedDate = DateTime.Now;
            _MasterModulesContext.Update(result);
            var contactExists = _MasterModulesContext.SaveChanges();


            if (contactExists == 1)
            {
                MasterModulesDTO.returnval = "true";
            }
            else
            {
                MasterModulesDTO.returnval = "false";
            }

            //  _MasterModulesContext.Remove(masters.ElementAt(0));
            //_MasterModulesContext.SaveChanges(); 

            Array[] showdata = new Array[50];
            List<MasterModules> Allname = new List<MasterModules>();
            Allname = _MasterModulesContext.MasterModules.ToList();
            MasterModulesDTO.masterModulesname = Allname.ToArray();

            return MasterModulesDTO;
        }

        public MasterModulesDTO MasterModulesData(MasterModulesDTO mas)
        {

            MasterModules MM = Mapper.Map<MasterModules>(mas);

            if (mas.IVRMM_Id != 0)
            {

                var result1 = _MasterModulesContext.MasterModules.Where(t => t.IVRMM_ModuleName == mas.IVRMM_ModuleName && t.IVRMM_Id!= mas.IVRMM_Id);
                if (result1.Count() >= 1)
                {
                    mas.returnduplicatestatus = "Duplicate";
                    
                }
                else
                {
                    var result = _MasterModulesContext.MasterModules.Single(t => t.IVRMM_Id == mas.IVRMM_Id);

                    //result.IVRMM_Id = mas.IVRMM_Id;
                    result.IVRMM_ModuleName = mas.IVRMM_ModuleName;
                    result.IVRMM_ModuleDesc = mas.IVRMM_ModuleDesc;
                    result.userid = mas.userid;
                    //added by 02/02/2017

                    result.UpdatedDate = DateTime.Now;
                    _MasterModulesContext.Update(result);
                    var contactExists = _MasterModulesContext.SaveChanges();

                    if (contactExists == 1)
                    {
                        mas.returnval = "Update";
                    }
                    else
                    {
                        mas.returnval = "NotUpdate";
                    }
                }

            }
            else
            {

                var result = _MasterModulesContext.MasterModules.Where(t => t.IVRMM_ModuleName== mas.IVRMM_ModuleName);
                if (result.Count() >= 1)
                {
                    mas.returnduplicatestatus = "Duplicate";
                }
                else
                {

                    //added by 02/02/2017
                    MM.CreatedDate = DateTime.Now;
                    MM.UpdatedDate = DateTime.Now;
                    _MasterModulesContext.Add(MM);
                    var contactExists = _MasterModulesContext.SaveChanges();
                    if (contactExists == 1)
                    {
                        mas.returnval = "Save";
                    }
                    else
                    {
                        mas.returnval = "NotSave";
                    }
                }
                
            }

            Array[] showdata = new Array[50];
            List<MasterModules> Allname = new List<MasterModules>();
            Allname = _MasterModulesContext.MasterModules.ToList();
            mas.masterModulesname = Allname.ToArray();

            return mas;
        }
    }
}
