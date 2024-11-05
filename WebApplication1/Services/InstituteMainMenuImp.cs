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
    public class InstituteMainMenuImp : Interfaces.InstituteMainMenuInterface
    {
        private static ConcurrentDictionary<string, InstituteMainMenuDTO> _login =
      new ConcurrentDictionary<string, InstituteMainMenuDTO>();

        private readonly InstituteMainMenuContext _InstituteMainMenuContext;


        public InstituteMainMenuImp(InstituteMainMenuContext InstituteMainMenuContext)
        {
            _InstituteMainMenuContext = InstituteMainMenuContext;
        }

        public InstituteMainMenuDTO GetMasterMainMenuData(InstituteMainMenuDTO InstituteMainMenuDTO)//int IVRMM_Id
        {
            var rolelist = _InstituteMainMenuContext.MasterRoleType.Where(t => t.IVRMRT_Id == InstituteMainMenuDTO.roleId).ToList();



            if (rolelist[0].IVRMRT_Role.Equals("Super Admin"))
            {
                List<Institution> lorgins = new List<Institution>();
                lorgins = _InstituteMainMenuContext.Institute.Where(t=>t.MI_ActiveFlag == 1).ToList();
                InstituteMainMenuDTO.fillinstitution = lorgins.ToArray();
            }
            else if (rolelist[0].IVRMRT_Role.Equals("Multi Admin"))
            {


                var Mo_id_list = _InstituteMainMenuContext.Institute.Where(d => d.MI_Id == InstituteMainMenuDTO.MI_Id && d.MI_ActiveFlag == 1).Select(d => d.MO_Id).ToList();

                var Mi_id_list = _InstituteMainMenuContext.Institute.Where(d => d.MO_Id.Equals(Mo_id_list[0])).Select(d => d.MI_Id).ToList();



                List<Institution> lorgins = new List<Institution>();
                lorgins = _InstituteMainMenuContext.Institute.Where(t => Mo_id_list.Contains(t.MO_Id) && t.MI_ActiveFlag == 1).ToList();
                InstituteMainMenuDTO.fillinstitution = lorgins.ToArray();
            }
            else if (rolelist[0].IVRMRT_Role.Equals("Admin") || rolelist[0].IVRMRT_Role.Equals("COORDINATOR"))
            {

                List<Institution> lorgins = new List<Institution>();
                lorgins = _InstituteMainMenuContext.Institute.Where(t => t.MI_Id == InstituteMainMenuDTO.MI_Id && t.MI_ActiveFlag == 1).ToList();
                InstituteMainMenuDTO.fillinstitution = lorgins.ToArray();
            }


            //Array[] showdata = new Array[50];
            //List<MasterModule> Allname = new List<MasterModule>();
            //Allname = _InstituteMainMenuContext.MasterModule.Where(t => t.Module_ActiveFlag == 0).ToList().ToList();
            //InstituteMainMenuDTO.masterModulesname = Allname.ToArray();



            var MasterMainMenuID = _InstituteMainMenuContext.MasterMainMenuDMO.Where(t => t.IVRMMM_ParentId.Equals(0)).Select(d => d.IVRMMM_Id).ToArray().ToArray();

            var MainMenuINS = _InstituteMainMenuContext.InstituteMainMenuDMO.Where(t => t.IVRMMMI_ParentId.Equals(0)).Select(d => d.IVRMMM_Id).ToArray().ToArray();

            var MainMenuInsIds = _InstituteMainMenuContext.MasterMainMenuDMO.Where(t => t.IVRMMM_ParentId.Equals(0) && !MainMenuINS.Contains(t.IVRMMM_Id)).Select(d => d.IVRMMM_Id).ToArray().ToArray();

            InstituteMainMenuDTO.masterMainMenuName = (from Modulename in _InstituteMainMenuContext.MasterModule
                                                       from MasterMainMenu in _InstituteMainMenuContext.MasterMainMenuDMO
                                                       where (Modulename.IVRMM_Id == MasterMainMenu.IVRMM_Id &&
                                                              MasterMainMenu.IVRMMM_ParentId == 0 && MainMenuInsIds.Contains(MasterMainMenu.IVRMMM_Id))
                                                       select new InstituteMainMenuDTO
                                                       {
                                                           modulename = Modulename.IVRMM_ModuleName,
                                                           IVRMMMI_MenuName = MasterMainMenu.IVRMMM_MenuName,
                                                           IVRMMM_Id = MasterMainMenu.IVRMMM_Id
                                                       }).OrderBy(t => t.IVRMMMI_Id).Distinct().ToArray();




            //InstituteMainMenuDTO.GridDetails = (from Modulename in _InstituteMainMenuContext.MasterModule
            //                                    from MainMenu in _InstituteMainMenuContext.InstituteMainMenuDMO
            //                                    where (Modulename.IVRMM_Id == MainMenu.IVRMM_Id &&
            //                                           MainMenu.IVRMMMI_ParentId == 0 && MasterMainMenuID.Contains(MainMenu.IVRMMM_Id))
            //                                    select new InstituteMainMenuDTO
            //                                    {
            //                                        modulename = Modulename.IVRMM_ModuleName,
            //                                        IVRMMMI_MenuName = MainMenu.IVRMMMI_MenuName,
            //                                        IVRMMMI_Id = MainMenu.IVRMMMI_Id
            //                                    }).OrderBy(t => t.IVRMMMI_Id).Distinct().ToArray();
            InstituteMainMenuDTO.GridDetails = (from Modulename in _InstituteMainMenuContext.MasterModule
                                                from MainMenu in _InstituteMainMenuContext.InstituteMainMenuDMO
                                                from Ins in _InstituteMainMenuContext.Institute
                                                where (Modulename.IVRMM_Id == MainMenu.IVRMM_Id && Ins.MI_Id == MainMenu.MI_Id &&
                                                       MainMenu.IVRMMMI_ParentId == 0 && MasterMainMenuID.Contains(MainMenu.IVRMMM_Id))
                                                select new InstituteMainMenuDTO
                                                {
                                                    modulename = Modulename.IVRMM_ModuleName,
                                                    IVRMMMI_MenuName = MainMenu.IVRMMMI_MenuName,
                                                    IVRMMMI_Id = MainMenu.IVRMMMI_Id,
                                                    MI_Name = Ins.MI_Name,
                                                    IVRMMMI_MenuOrder = MainMenu.IVRMMMI_MenuOrder,
                                                    IVRMMMI_Color= MainMenu.IVRMMMI_Color,
                                                    IVRMMMI_Icon = MainMenu.IVRMMMI_Icon
                                                }).OrderBy(t => t.IVRMMMI_MenuOrder).Distinct().ToArray();



            return InstituteMainMenuDTO;
        }

        public InstituteMainMenuDTO GetSelectedRowDetails(int ID)
        {
            InstituteMainMenuDTO InstituteMainMenuDTO = new InstituteMainMenuDTO();
            List<InstituteMainMenuDMO> lorg = new List<InstituteMainMenuDMO>();
            lorg = _InstituteMainMenuContext.InstituteMainMenuDMO.Where(t => t.IVRMMMI_Id.Equals(ID)).ToList();
            InstituteMainMenuDTO.masterModulesname = lorg.ToArray();
            return InstituteMainMenuDTO;
        }


        public InstituteMainMenuDTO getmoduledetails(InstituteMainMenuDTO data)
        {
            //InstituteMainMenuDTO InstituteMainMenuDTO = new InstituteMainMenuDTO();

            var Mo_id_list = _InstituteMainMenuContext.modulemap.Where(d => d.MI_Id == data.MI_Id && d.IVRMIM_Flag==1).Select(d => d.IVRMM_Id).ToList();

            Array[] showdata = new Array[50];
            List<MasterModule> Allname = new List<MasterModule>();
            Allname = _InstituteMainMenuContext.MasterModule.Where(t => Mo_id_list.Contains(t.IVRMM_Id) && t.Module_ActiveFlag == 1).ToList();
            data.masterModulesname = Allname.ToArray();
            var MasterMainMenuID = _InstituteMainMenuContext.MasterMainMenuDMO.Where(t => t.IVRMMM_ParentId.Equals(0)).Select(d => d.IVRMMM_Id).ToArray().ToArray();

            data.GridDetails = (from Modulename in _InstituteMainMenuContext.MasterModule
                                                from MainMenu in _InstituteMainMenuContext.InstituteMainMenuDMO
                                                from Ins in _InstituteMainMenuContext.Institute
                                                where (Modulename.IVRMM_Id == MainMenu.IVRMM_Id && Ins.MI_Id == MainMenu.MI_Id &&
                                                       MainMenu.IVRMMMI_ParentId == 0 && MasterMainMenuID.Contains(MainMenu.IVRMMM_Id) && Ins.MI_Id==data.MI_Id)
                                                select new InstituteMainMenuDTO
                                                {
                                                    modulename = Modulename.IVRMM_ModuleName,
                                                    IVRMMMI_MenuName = MainMenu.IVRMMMI_MenuName,
                                                    IVRMMMI_Id = MainMenu.IVRMMMI_Id,
                                                    MI_Name = Ins.MI_Name,
                                                    IVRMMMI_MenuOrder = MainMenu.IVRMMMI_MenuOrder,
                                                    IVRMMMI_Color = MainMenu.IVRMMMI_Color,
                                                    IVRMMMI_Icon = MainMenu.IVRMMMI_Icon
                                                }).OrderBy(t => t.IVRMMMI_MenuOrder).Distinct().ToArray();


            return data;
        }

        public InstituteMainMenuDTO getMenudetailsByModuleId(InstituteMainMenuDTO data)
        {

            var MainMenuINS = _InstituteMainMenuContext.InstituteMainMenuDMO.Where(t => t.IVRMMMI_ParentId.Equals(0) && t.MI_Id==data.MI_Id).Select(d => d.IVRMMM_Id).ToArray().ToArray();

            var MainMenuInsIds = _InstituteMainMenuContext.MasterMainMenuDMO.Where(t => t.IVRMMM_ParentId.Equals(0) && !MainMenuINS.Contains(t.IVRMMM_Id)).Select(d => d.IVRMMM_Id).ToArray().ToArray();

            InstituteMainMenuDTO InstituteMainMenuDTO = new InstituteMainMenuDTO();

            if (data.IVRMM_Id == 0)
            {
                InstituteMainMenuDTO.masterMainMenuName = (from Modulename in _InstituteMainMenuContext.MasterModule
                                                           from MasterMainMenu in _InstituteMainMenuContext.MasterMainMenuDMO
                                                           where (Modulename.IVRMM_Id == MasterMainMenu.IVRMM_Id &&
                                                                  MasterMainMenu.IVRMMM_ParentId == 0 && MainMenuInsIds.Contains(MasterMainMenu.IVRMMM_Id))
                                                           select new InstituteMainMenuDTO
                                                           {
                                                               modulename = Modulename.IVRMM_ModuleName,
                                                               IVRMMMI_MenuName = MasterMainMenu.IVRMMM_MenuName,
                                                               IVRMMM_Id = MasterMainMenu.IVRMMM_Id
                                                           }).OrderBy(t => t.IVRMMMI_Id).Distinct().ToArray();
            }
            else
            {

                //from insmod in _InstituteMainMenuContext.Institutemod
                InstituteMainMenuDTO.masterMainMenuName = (from Modulename in _InstituteMainMenuContext.MasterModule
                                                           from MasterMainMenu in _InstituteMainMenuContext.MasterMainMenuDMO

                                                           where (Modulename.IVRMM_Id == MasterMainMenu.IVRMM_Id &&
                                                           Modulename.IVRMM_Id == data.IVRMM_Id &&
                                                             MasterMainMenu.IVRMMM_ParentId == 0 && MainMenuInsIds.Contains(MasterMainMenu.IVRMMM_Id))
                                                           select new InstituteMainMenuDTO
                                                           {
                                                               modulename = Modulename.IVRMM_ModuleName,
                                                               IVRMMMI_MenuName = MasterMainMenu.IVRMMM_MenuName,
                                                               IVRMMM_Id = MasterMainMenu.IVRMMM_Id
                                                           }).OrderBy(t => t.IVRMMMI_Id).Distinct().ToArray();
            }

            return InstituteMainMenuDTO;
        }

        public InstituteMainMenuDTO MasterDeleteMainMenuDTO(int ID)
        {
            InstituteMainMenuDTO InstituteMainMenuDTO = new InstituteMainMenuDTO();
            List<InstituteMainMenuDMO> InstituteMainMenuDMO = new List<InstituteMainMenuDMO>();

            try
            {

                var submenucount = _InstituteMainMenuContext.InstituteMainMenuDMO.Where(t => t.IVRMMMI_ParentId.Equals(ID)).ToList();
                if (submenucount.Count == 0)
                {

                    InstituteMainMenuDMO = _InstituteMainMenuContext.InstituteMainMenuDMO.Where(t => t.IVRMMMI_Id.Equals(ID)).ToList();

                    if (InstituteMainMenuDMO.Any())
                    {
                        _InstituteMainMenuContext.Remove(InstituteMainMenuDMO.ElementAt(0));

                        var flag = _InstituteMainMenuContext.SaveChanges();
                        if (flag == 1)
                        {
                            InstituteMainMenuDTO.returnval = "true";
                        }
                       
                    }
                }
                else
                {
                    InstituteMainMenuDTO.returnval = "false";
                }

                var MainMenuINS = _InstituteMainMenuContext.InstituteMainMenuDMO.Where(t => t.IVRMMMI_ParentId.Equals(0)).Select(d => d.IVRMMMI_Id).ToArray().ToArray();

                //InstituteMainMenuDTO.GridDetails = (from Modulename in _InstituteMainMenuContext.MasterModule
                //                                    from MainMenu in _InstituteMainMenuContext.InstituteMainMenuDMO
                //                                    where (Modulename.IVRMM_Id == MainMenu.IVRMM_Id &&
                //                                           MainMenu.IVRMMMI_ParentId == 0 && MainMenuINS.Contains(MainMenu.IVRMMMI_Id))
                //                                    select new InstituteMainMenuDTO
                //                                    {
                //                                        modulename = Modulename.IVRMM_ModuleName,
                //                                        IVRMMMI_MenuName = MainMenu.IVRMMMI_MenuName,
                //                                        IVRMMMI_Id = MainMenu.IVRMMMI_Id
                //                                    }).OrderBy(t => t.IVRMMMI_Id).Distinct().ToArray();

                InstituteMainMenuDTO.GridDetails = (from Modulename in _InstituteMainMenuContext.MasterModule
                                                    from MainMenu in _InstituteMainMenuContext.InstituteMainMenuDMO
                                                    from Ins in _InstituteMainMenuContext.Institute
                                                    where (Modulename.IVRMM_Id == MainMenu.IVRMM_Id && Ins.MI_Id == MainMenu.MI_Id &&
                                                           MainMenu.IVRMMMI_ParentId == 0 && MainMenuINS.Contains(MainMenu.IVRMMM_Id))
                                                    select new InstituteMainMenuDTO
                                                    {
                                                        modulename = Modulename.IVRMM_ModuleName,
                                                        IVRMMMI_MenuName = MainMenu.IVRMMMI_MenuName,
                                                        IVRMMMI_Id = MainMenu.IVRMMMI_Id,
                                                        MI_Name = Ins.MI_Name,
                                                        IVRMMMI_MenuOrder = MainMenu.IVRMMMI_MenuOrder,
                                                        IVRMMMI_Color = MainMenu.IVRMMMI_Color,
                                                        IVRMMMI_Icon = MainMenu.IVRMMMI_Icon
                                                    }).OrderBy(t => t.IVRMMMI_MenuOrder).Distinct().ToArray();


                InstituteMainMenuDTO.masterMainMenuName = (from Modulename in _InstituteMainMenuContext.MasterModule
                                                           from MainMenu in _InstituteMainMenuContext.InstituteMainMenuDMO
                                                           where (Modulename.IVRMM_Id == MainMenu.IVRMM_Id &&
                                                                  MainMenu.IVRMMMI_ParentId == 0 && !MainMenuINS.Contains(MainMenu.IVRMMMI_Id))
                                                           select new InstituteMainMenuDTO
                                                           {
                                                               IVRMMMI_MenuName = MainMenu.IVRMMMI_MenuName,
                                                               IVRMMMI_Id = MainMenu.IVRMMMI_Id
                                                           }).OrderBy(t => t.IVRMMMI_Id).Distinct().ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return InstituteMainMenuDTO;
        }

        public InstituteMainMenuDTO MasterMainMenuData(InstituteMainMenuDTO mas)
        {

            InstituteMainMenuDMO MM = Mapper.Map<InstituteMainMenuDMO>(mas);

            if (mas.IVRMMMI_Id != 0)
            {
                var result = _InstituteMainMenuContext.InstituteMainMenuDMO.Single(t => t.IVRMMMI_Id == mas.IVRMMMI_Id);

                result.IVRMM_Id = mas.IVRMM_Id;
                result.IVRMMMI_MenuName = mas.IVRMMMI_MenuName;
                //result.IVRMMMI_Color
                //  result.IVRMMMI_Icon
                result.UpdatedDate = DateTime.Now;
                _InstituteMainMenuContext.Update(result);
                var contactExists = _InstituteMainMenuContext.SaveChanges();

                if (contactExists == 1)
                {
                    mas.returnval = "ut";
                }
                else
                {
                    mas.returnval = "uf";
                }

            }
            else
            {
                for (int Q = 0; Q < mas.SelectedMasterMenuDetails.Count; Q++)
                {
                    List<MasterMainMenuDMO> Allname3 = new List<MasterMainMenuDMO>();

                    Allname3 = _InstituteMainMenuContext.MasterMainMenuDMO.Where(t => t.IVRMMM_Id.Equals(mas.SelectedMasterMenuDetails[Q].IVRMMM_Id)).ToList().ToList();

                    MM.IVRMMMI_Id = mas.IVRMMMI_Id;
                    MM.IVRMMMI_MenuName = mas.SelectedMasterMenuDetails[Q].IVRMMMI_MenuName;
                    MM.IVRMMMI_MenuOrder = 0;
                    MM.IVRMMMI_PageNonPageFlag = Allname3[0].IVRMMM_PageNonPageFlag;
                    MM.IVRMMMI_ParentId = Allname3[0].IVRMMM_ParentId;
                    MM.IVRMMM_Id = mas.SelectedMasterMenuDetails[Q].IVRMMM_Id;
                    MM.IVRMM_Id = Allname3[0].IVRMM_Id;
                    MM.MI_Id = mas.MI_Id;
                    //icon & color
                    MM.IVRMMMI_Icon = mas.IVRMMMI_Icon;
                    MM.IVRMMMI_Color = mas.IVRMMMI_Color;
                    MM.CreatedDate = DateTime.Now;
                    MM.UpdatedDate = DateTime.Now;
                    _InstituteMainMenuContext.Add(MM);
                    var contactExists = _InstituteMainMenuContext.SaveChanges();
                    if (contactExists == 1)
                    {
                        mas.returnval = "at";
                    }
                    else
                    {
                        mas.returnval = "af";
                    }
                }

            }

            //mas.GridDetails = (from Modulename in _InstituteMainMenuContext.MasterModule
            //                   from MainMenu in _InstituteMainMenuContext.InstituteMainMenuDMO
            //                   where (Modulename.IVRMM_Id == MainMenu.IVRMM_Id &&
            //                          MainMenu.IVRMMMI_Id == 0)
            //                   select new InstituteMainMenuDTO
            //                   {
            //                       modulename = Modulename.IVRMM_ModuleName,
            //                       IVRMMMI_MenuName = MainMenu.IVRMMMI_MenuName,
            //                       IVRMMMI_Id = MainMenu.IVRMMMI_Id
            //                   }).OrderBy(t => t.IVRMMMI_Id).Distinct().ToArray();

            mas.GridDetails = (from Modulename in _InstituteMainMenuContext.MasterModule
                                                from MainMenu in _InstituteMainMenuContext.InstituteMainMenuDMO
                                                from Ins in _InstituteMainMenuContext.Institute
                                                where (Modulename.IVRMM_Id == MainMenu.IVRMM_Id && Ins.MI_Id == MainMenu.MI_Id &&
                                                       MainMenu.IVRMMMI_ParentId == 0)
                                                select new InstituteMainMenuDTO
                                                {
                                                    modulename = Modulename.IVRMM_ModuleName,
                                                    IVRMMMI_MenuName = MainMenu.IVRMMMI_MenuName,
                                                    IVRMMMI_Id = MainMenu.IVRMMMI_Id,
                                                    MI_Name = Ins.MI_Name,
                                                    IVRMMMI_MenuOrder = MainMenu.IVRMMMI_MenuOrder,
                                                    IVRMMMI_Color = MainMenu.IVRMMMI_Color,
                                                    IVRMMMI_Icon = MainMenu.IVRMMMI_Icon
                                                }).OrderBy(t => t.IVRMMMI_MenuOrder).Distinct().ToArray();


            return mas;
        }


        public InstituteMainMenuDTO changeorderData(InstituteMainMenuDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.menuDTO.Count() > 0)
                {
                    foreach (InstituteMainMenuDTO mob in dto.menuDTO)
                    {
                        if (mob.IVRMMMI_Id > 0)
                        {
                            var result = _InstituteMainMenuContext.InstituteMainMenuDMO.Single(t => t.IVRMMMI_Id.Equals(mob.IVRMMMI_Id));
                            // Mapper.Map(mob, result);
                            result.IVRMMMI_Icon = mob.IVRMMMI_Icon;
                            result.IVRMMMI_Color = mob.IVRMMMI_Color;
                            result.IVRMMMI_MenuName = mob.IVRMMMI_MenuName;
                            result.IVRMMMI_MenuOrder = mob.IVRMMMI_MenuOrder;
                            _InstituteMainMenuContext.Update(result);
                            _InstituteMainMenuContext.SaveChanges();
                        }
                    }

                    dto.retrunMsg = "Order Updated Successfully";

                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }


    }
}
