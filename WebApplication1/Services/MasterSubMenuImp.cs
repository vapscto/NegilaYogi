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
    public class MasterSubMenuImp : Interfaces.MasterSubMenuInterface
    {
        private static ConcurrentDictionary<string, MasterMainMenuDTO> _login =
      new ConcurrentDictionary<string, MasterMainMenuDTO>();

        private readonly MasterMainMenuContext _MatserMainMenuContext;


        public MasterSubMenuImp(MasterMainMenuContext MatserMainMenuContext)
        {
            _MatserMainMenuContext = MatserMainMenuContext;
        }

        public MasterMainMenuDTO GetMasterSubMenuData(MasterMainMenuDTO MasterMainMenuDTO)//int IVRMM_Id
        {
            Array[] showdata11 = new Array[50];
            List<MasterModule> Allname11 = new List<MasterModule>();
            Allname11 = _MatserMainMenuContext.MasterModule.Where(t => t.Module_ActiveFlag == 1).ToList().ToList();
            MasterMainMenuDTO.masterModulesname = Allname11.ToArray();

            Array[] showdata = new Array[50];
            List<MasterMainMenuDMO> Allname = new List<MasterMainMenuDMO>();
            Allname = _MatserMainMenuContext.MasterMainMenuDMO.Where(t=> t.IVRMMM_ParentId == 0).ToList().ToList();
            MasterMainMenuDTO.masterMainMenuName = Allname.ToArray();

            MasterMainMenuDTO.GridDetails = (from Modulename in _MatserMainMenuContext.MasterModule
                                from MainMenu in _MatserMainMenuContext.MasterMainMenuDMO
                              from SubMenu in _MatserMainMenuContext.MasterMainMenuDMO
                              where (Modulename.IVRMM_Id == SubMenu.IVRMM_Id 
                              && SubMenu.IVRMMM_ParentId != 0 
                              && MainMenu.IVRMMM_ParentId == 0
                              && SubMenu.IVRMMM_ParentId == MainMenu.IVRMMM_Id)
                                             select new MasterMainMenuDTO
                                             {
                                                 modulename = Modulename.IVRMM_ModuleName,
                                                 IVRMMM_MenuName = MainMenu.IVRMMM_MenuName,
                                                 SubMenuName = SubMenu.IVRMMM_MenuName,
                                                 IVRMMM_MenuOrder = SubMenu.IVRMMM_MenuOrder,
                                                 IVRMMM_Id = SubMenu.IVRMMM_Id
                                             }).OrderBy(t => t.modulename).Distinct().ToArray();


            return MasterMainMenuDTO;
        }

        public MasterMainMenuDTO GetSelectedRowDetails(int ID)
        {
            MasterMainMenuDTO MasterMainMenuDTO = new MasterMainMenuDTO();
            List<MasterMainMenuDMO> lorg = new List<MasterMainMenuDMO>();
            MasterMainMenuDTO.masterModulesname = (from MainMenu in _MatserMainMenuContext.MasterMainMenuDMO
                                                   from SubMenu in _MatserMainMenuContext.MasterMainMenuDMO
                                                   where (MainMenu.IVRMMM_ParentId == 0 &&
                                                          SubMenu.IVRMMM_ParentId != 0 &&
                                                          SubMenu.IVRMMM_ParentId == MainMenu.IVRMMM_Id &&
                                                          SubMenu.IVRMMM_Id == ID)
                                                   select new MasterMainMenuDTO
                                                   {
                                                       IVRMM_Id = SubMenu.IVRMM_Id,
                                                       IVRMMM_Id_select = SubMenu.IVRMMM_ParentId,
                                                       SubMenuName = SubMenu.IVRMMM_MenuName,
                                                       IVRMMM_MenuOrder = SubMenu.IVRMMM_MenuOrder,
                                                       IVRMMM_Id = SubMenu.IVRMMM_Id
                                                   }).OrderBy(t => t.IVRMMM_Id).Distinct().ToArray();
            return MasterMainMenuDTO;
        }        

        public MasterMainMenuDTO MasterDeleteSubMenuDTO(int ID)
        {
            MasterMainMenuDTO MasterMainMenuDTO = new MasterMainMenuDTO();
            List<MasterMainMenuDMO> MasterMainMenuDMO = new List<MasterMainMenuDMO>();

            try
            {
                var submenudelete = _MatserMainMenuContext.IVRM_Master_Menu_Page_MappingDMO.Where(t => t.IVRMMM_Id.Equals(ID)).ToList();

                var submenudeleteins = _MatserMainMenuContext.InstituteMainMenuDMO.Where(t => t.IVRMMMI_Id.Equals(ID)).ToList();

                if (submenudelete.Count==0 && submenudeleteins.Count == 0)
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

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return MasterMainMenuDTO;
        }

        public MasterMainMenuDTO MasterSubMenuData(MasterMainMenuDTO mas)
        {
            MasterMainMenuDMO MM = Mapper.Map<MasterMainMenuDMO>(mas);

            try
            {
                if (mas.IVRMMM_Id != 0)
                {

                    Array[] showdata1 = new Array[1];
                    List<MasterMainMenuDMO> Allname1 = new List<MasterMainMenuDMO>();
                    //  Allname1 = _MatserMainMenuContext.MasterMainMenuDMO.Where(t => t.IVRMM_Id.Equals(mas.IVRMM_Id) && t.IVRMMM_ParentId.Equals(mas.IVRMMM_ParentId) && t.IVRMMM_MenuName.Equals(mas.IVRMMM_MenuName) && t.IVRMMM_Id!= mas.IVRMMM_Id).ToList();
                    Allname1 = _MatserMainMenuContext.MasterMainMenuDMO.Where(t => t.IVRMM_Id.Equals(mas.IVRMM_Id) && t.IVRMMM_ParentId.Equals(mas.IVRMMM_ParentId) && t.IVRMMM_MenuName.Equals(mas.IVRMMM_MenuName) && t.IVRMMM_Id != mas.IVRMMM_Id).ToList();
                    mas.studentDetails = Allname1.ToArray();
                    if (Allname1.Count > 0)
                    {
                        mas.returnval = "Duplicate";
                    }
                    else
                    {
                        List<MasterMainMenuDMO> Order = new List<MasterMainMenuDMO>();
                        Order = _MatserMainMenuContext.MasterMainMenuDMO.Where(t => t.IVRMM_Id.Equals(mas.IVRMM_Id) && t.IVRMMM_ParentId.Equals(mas.IVRMMM_ParentId) && t.IVRMMM_MenuOrder == mas.IVRMMM_MenuOrder).ToList();
                        if (Order.Count > 0)
                        {
                            mas.returnval = "DuplicateOrder";
                            return mas;
                        }
                        var result = _MatserMainMenuContext.MasterMainMenuDMO.Single(t => t.IVRMMM_Id == mas.IVRMMM_Id);

                        result.IVRMM_Id = mas.IVRMM_Id;
                        result.IVRMMM_MenuName = mas.IVRMMM_MenuName;
                        result.IVRMMM_MenuOrder = mas.IVRMMM_MenuOrder;
                        result.IVRMMM_PageNonPageFlag = mas.IVRMMM_PageNonPageFlag;
                        result.IVRMMM_ParentId = mas.IVRMMM_ParentId;

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
                            mas.returnval = "false";
                        }
                    }
                }
                else
                {
                    Array[] showdata1 = new Array[1];
                    List<MasterMainMenuDMO> Allname1 = new List<MasterMainMenuDMO>();
                    Allname1 = _MatserMainMenuContext.MasterMainMenuDMO.Where(t => t.IVRMM_Id.Equals(mas.IVRMM_Id) && t.IVRMMM_ParentId.Equals(mas.IVRMMM_ParentId) && t.IVRMMM_MenuName.Equals(mas.IVRMMM_MenuName)).ToList();
                    mas.studentDetails = Allname1.ToArray();
                    if (Allname1.Count > 0)
                    {
                        mas.returnval = "Duplicate";
                    }
                    else
                    {
                        List<MasterMainMenuDMO> Order = new List<MasterMainMenuDMO>();
                        Order = _MatserMainMenuContext.MasterMainMenuDMO.Where(t => t.IVRMM_Id.Equals(mas.IVRMM_Id) && t.IVRMMM_ParentId.Equals(mas.IVRMMM_ParentId) &&  t.IVRMMM_MenuOrder == mas.IVRMMM_MenuOrder).ToList();
                        if (Order.Count > 0)
                        {
                            mas.returnval = "DuplicateOrder";
                            return mas;
                        }

                        //added by 02/02/2017
                        MM.CreatedDate = DateTime.Now;
                        MM.UpdatedDate = DateTime.Now;

                        _MatserMainMenuContext.Add(MM);
                        var contactExists = _MatserMainMenuContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            mas.returnval = "Add";
                        }
                        else
                        {
                            mas.returnval = "false";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                mas.returnval = "false";
                return mas;
            }
            

          
            return mas;
        }
    }
}
