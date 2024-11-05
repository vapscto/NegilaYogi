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
    public class MasterMainMenuImp : Interfaces.MasterMainMenuInterface
    {
        private static ConcurrentDictionary<string, MasterMainMenuDTO> _login =
      new ConcurrentDictionary<string, MasterMainMenuDTO>();

        private readonly MasterMainMenuContext _MatserMainMenuContext;


        public MasterMainMenuImp(MasterMainMenuContext MatserMainMenuContext)
        {
            _MatserMainMenuContext = MatserMainMenuContext;
        }

        public MasterMainMenuDTO GetMasterMainMenuData(MasterMainMenuDTO MasterMainMenuDTO)//int IVRMM_Id
        {

            Array[] showdata = new Array[50];
            List<MasterModule> Allname = new List<MasterModule>();
            Allname = _MatserMainMenuContext.MasterModule.Where(t=> t.Module_ActiveFlag== 1).ToList().ToList();
            MasterMainMenuDTO.masterModulesname = Allname.ToArray();    

            MasterMainMenuDTO.GridDetails = (from Modulename in _MatserMainMenuContext.MasterModule
                               from MainMenu in _MatserMainMenuContext.MasterMainMenuDMO
                               where (Modulename.IVRMM_Id == MainMenu.IVRMM_Id &&
                                      MainMenu.IVRMMM_ParentId == 0)
                               select new MasterMainMenuDTO
                               {
                                   modulename = Modulename.IVRMM_ModuleName,
                                   IVRMMM_MenuName = MainMenu.IVRMMM_MenuName,
                                   IVRMMM_Id = MainMenu.IVRMMM_Id
                               }).OrderBy(t => t.IVRMMM_Id).Distinct().ToArray();


           

            return MasterMainMenuDTO;
        }

        public MasterMainMenuDTO GetSelectedRowDetails(int ID)
        {
            MasterMainMenuDTO MasterMainMenuDTO = new MasterMainMenuDTO();
            List<MasterMainMenuDMO> lorg = new List<MasterMainMenuDMO>();
            lorg = _MatserMainMenuContext.MasterMainMenuDMO.Where(t => t.IVRMMM_Id.Equals(ID)).ToList();
            MasterMainMenuDTO.masterModulesname = lorg.ToArray();
            return MasterMainMenuDTO;
        }        

        public MasterMainMenuDTO MasterDeleteMainMenuDTO(int ID)
        {
            MasterMainMenuDTO MasterMainMenuDTO = new MasterMainMenuDTO();
            List<MasterMainMenuDMO> MasterMainMenuDMO = new List<MasterMainMenuDMO>();

            try
            {
                var  submenucount   = _MatserMainMenuContext.MasterMainMenuDMO.Where(t => t.IVRMMM_ParentId.Equals(ID)).ToList();

                var submenucountinst  = _MatserMainMenuContext.InstituteMainMenuDMO.Where(t => t.IVRMMM_Id.Equals(ID)).ToList();

                if (submenucount.Count == 0 && submenucountinst.Count==0)
                {

                    MasterMainMenuDMO = _MatserMainMenuContext.MasterMainMenuDMO.Where(t => t.IVRMMM_Id.Equals(ID)).ToList();

                    if (MasterMainMenuDMO.Any())
                    {
                        _MatserMainMenuContext.Remove(MasterMainMenuDMO.ElementAt(0));

                        var flag = _MatserMainMenuContext.SaveChanges();
                        if (flag == 1)
                        {
                            MasterMainMenuDTO.returnval = "true";
                          
                        }
                      
                    }
                }
                else
                {
                    MasterMainMenuDTO.returnval = "false";
                }


                MasterMainMenuDTO.GridDetails = (from Modulename in _MatserMainMenuContext.MasterModule
                                                 from MainMenu in _MatserMainMenuContext.MasterMainMenuDMO
                                                 where (Modulename.IVRMM_Id == MainMenu.IVRMM_Id &&
                                                        MainMenu.IVRMMM_ParentId == 0)
                                                 select new MasterMainMenuDTO
                                                 {
                                                     modulename = Modulename.IVRMM_ModuleName,
                                                     IVRMMM_MenuName = MainMenu.IVRMMM_MenuName,
                                                     IVRMMM_Id = MainMenu.IVRMMM_Id
                                                 }).OrderBy(t => t.IVRMMM_Id).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return MasterMainMenuDTO;
        }

        public MasterMainMenuDTO MasterMainMenuData(MasterMainMenuDTO mas)
        {

            MasterMainMenuDMO MM = Mapper.Map<MasterMainMenuDMO>(mas);

            if (mas.IVRMMM_Id != 0)
            {
                var result1 = _MatserMainMenuContext.MasterMainMenuDMO.Where(t => t.IVRMMM_MenuName == mas.IVRMMM_MenuName && t.IVRMM_Id == mas.IVRMM_Id && t.IVRMMM_Id!= mas.IVRMMM_Id) ;

                if (result1.Count() > 0)
                {
                    mas.returnval = "Duplicate";
                }
                else
                {

                    var result = _MatserMainMenuContext.MasterMainMenuDMO.Single(t => t.IVRMMM_Id == mas.IVRMMM_Id);

                    result.IVRMM_Id = mas.IVRMM_Id;
                    result.IVRMMM_MenuName = mas.IVRMMM_MenuName;
                    //added by 02/02/2017
                    result.UpdatedDate = DateTime.Now;
                    _MatserMainMenuContext.Update(result);
                    var contactExists = _MatserMainMenuContext.SaveChanges();

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
                var result = _MatserMainMenuContext.MasterMainMenuDMO.Where(t => t.IVRMMM_MenuName == mas.IVRMMM_MenuName && t.IVRMM_Id== mas.IVRMM_Id);

                if (result.Count() > 0)
                {
                    mas.returnval = "Duplicate";
                }
                else
                {
                    //added by 02/02/2017
                    MM.CreatedDate = DateTime.Now;
                    MM.UpdatedDate = DateTime.Now;
                    _MatserMainMenuContext.Add(MM);
                    var contactExists = _MatserMainMenuContext.SaveChanges();
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

            mas.GridDetails = (from Modulename in _MatserMainMenuContext.MasterModule
                                             from MainMenu in _MatserMainMenuContext.MasterMainMenuDMO
                                             where (Modulename.IVRMM_Id == MainMenu.IVRMM_Id &&
                                                    MainMenu.IVRMMM_ParentId == 0)
                                             select new MasterMainMenuDTO
                                             {
                                                 modulename = Modulename.IVRMM_ModuleName,
                                                 IVRMMM_MenuName = MainMenu.IVRMMM_MenuName,
                                                 IVRMMM_Id = MainMenu.IVRMMM_Id
                                             }).OrderBy(t => t.IVRMMM_Id).Distinct().ToArray();

            return mas;
        }
    }
}
