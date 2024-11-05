using System;
using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using AutoMapper;
namespace WebApplication1.Services
{
    public class InstituteSubMenuImp : Interfaces.InstituteSubMenuInterface
    {
        private static ConcurrentDictionary<string, InstituteMainMenuDTO> _login =
      new ConcurrentDictionary<string, InstituteMainMenuDTO>();

        private readonly InstituteMainMenuContext _InstituteMainMenuContext;


        public InstituteSubMenuImp(InstituteMainMenuContext InstituteMainMenuContext)
        {
            _InstituteMainMenuContext = InstituteMainMenuContext;
        }

        public InstituteMainMenuDTO GetMasterSubMenuData(InstituteMainMenuDTO InstituteMainMenuDTO)//int IVRMM_Id
        {

            var rolelist = _InstituteMainMenuContext.MasterRoleType.Where(t => t.IVRMRT_Id == InstituteMainMenuDTO.roleId).ToList();

            if (rolelist[0].IVRMRT_Role == "Super Admin")
            {
                List<Institution> lorgins = new List<Institution>();
                lorgins = _InstituteMainMenuContext.Institute.Where(t => t.MI_ActiveFlag == 1).ToList();
                InstituteMainMenuDTO.fillinstitution = lorgins.ToArray();
            }
            else if (rolelist[0].IVRMRT_Role.Equals("Multi Admin"))
            {


                var Mo_id_list = _InstituteMainMenuContext.Institute.Where(d => d.MI_Id == InstituteMainMenuDTO.MI_Id && d.MI_ActiveFlag == 1).Select(d => d.MO_Id).ToList();





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

            Array[] showdata = new Array[50];
            List<MasterModule> Allname = new List<MasterModule>();
            Allname = _InstituteMainMenuContext.MasterModule.Where(t => t.Module_ActiveFlag == 1).ToList().ToList();
            InstituteMainMenuDTO.masterModulesname = Allname.ToArray();


            var MainMenuINS = _InstituteMainMenuContext.InstituteMainMenuDMO.Where(t => t.IVRMMMI_ParentId == 0).Select(d => d.IVRMMM_Id).ToArray().ToArray();

            InstituteMainMenuDTO.masterMainMenuName = (from Modulename in _InstituteMainMenuContext.MasterModule
                                                       from MasterMainMenu in _InstituteMainMenuContext.MasterMainMenuDMO
                                                       where (Modulename.IVRMM_Id == MasterMainMenu.IVRMM_Id &&
                                                              MasterMainMenu.IVRMMM_ParentId == 0 && MainMenuINS.Contains(MasterMainMenu.IVRMMM_Id))
                                                       select new InstituteMainMenuDTO
                                                       {
                                                           modulename = Modulename.IVRMM_ModuleName,
                                                           IVRMMMI_MenuName = MasterMainMenu.IVRMMM_MenuName,
                                                           IVRMMM_Id = MasterMainMenu.IVRMMM_Id
                                                       }).OrderBy(t => t.IVRMMMI_Id).Distinct().ToArray();

            var MasterMainMenuID = _InstituteMainMenuContext.MasterMainMenuDMO.Where(t => t.IVRMMM_ParentId != 0).Select(d => d.IVRMMM_Id).ToArray().ToArray();

            var SUbMenuINS = _InstituteMainMenuContext.InstituteMainMenuDMO.Where(t => t.IVRMMMI_ParentId != 0).Select(d => d.IVRMMM_Id).ToArray().ToArray();

            var SubMenuInsIds = _InstituteMainMenuContext.MasterMainMenuDMO.Where(t => t.IVRMMM_ParentId != 0 && !SUbMenuINS.Contains(t.IVRMMM_Id)).Select(d => d.IVRMMM_Id).ToArray().ToArray();

            InstituteMainMenuDTO.masterSubMenuName = (from Modulename in _InstituteMainMenuContext.MasterModule
                                                      from MasterMainMenu in _InstituteMainMenuContext.MasterMainMenuDMO
                                                      where (Modulename.IVRMM_Id == MasterMainMenu.IVRMM_Id &&
                                                             MasterMainMenu.IVRMMM_ParentId != 0 && SubMenuInsIds.Contains(MasterMainMenu.IVRMMM_Id))
                                                      select new InstituteMainMenuDTO
                                                      {
                                                          modulename = Modulename.IVRMM_ModuleName,
                                                          IVRMMMI_MenuName = MasterMainMenu.IVRMMM_MenuName,
                                                          IVRMMM_Id = MasterMainMenu.IVRMMM_Id,
                                                          IVRMMMI_MenuOrder = MasterMainMenu.IVRMMM_MenuOrder
                                                      }).OrderBy(t => t.IVRMMMI_Id).Distinct().ToArray();




            InstituteMainMenuDTO.GridDetails = (from Modulename in _InstituteMainMenuContext.MasterModule
                                                from MainMenu in _InstituteMainMenuContext.InstituteMainMenuDMO
                                                from Ins in _InstituteMainMenuContext.Institute
                                                where (Modulename.IVRMM_Id == MainMenu.IVRMM_Id && Ins.MI_Id == MainMenu.MI_Id &&
                                                       MainMenu.IVRMMMI_ParentId != 0 && MasterMainMenuID.Contains(MainMenu.IVRMMM_Id))
                                                select new InstituteMainMenuDTO
                                                {
                                                    modulename = Modulename.IVRMM_ModuleName,
                                                    IVRMMMI_MenuName = MainMenu.IVRMMMI_MenuName,
                                                    IVRMMMI_Id = MainMenu.IVRMMMI_Id,
                                                    MI_Name = Ins.MI_Name,
                                                    IVRMMMI_MenuOrder = MainMenu.IVRMMMI_MenuOrder
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

            var Mo_id_list = _InstituteMainMenuContext.modulemap.Where(d => d.MI_Id == data.MI_Id).Select(d => d.IVRMM_Id).ToList();

            Array[] showdata = new Array[50];
            List<MasterModule> Allname = new List<MasterModule>();
            Allname = _InstituteMainMenuContext.MasterModule.Where(t => Mo_id_list.Contains(t.IVRMM_Id)).ToList();
            data.masterModulesname = Allname.ToArray();

            var MasterMainMenuID = _InstituteMainMenuContext.MasterMainMenuDMO.Where(t => t.IVRMMM_ParentId != 0).Select(d => d.IVRMMM_Id).ToArray().ToArray();

            data.GridDetails = (from Modulename in _InstituteMainMenuContext.MasterModule
                                                from MainMenu in _InstituteMainMenuContext.InstituteMainMenuDMO
                                                from Ins in _InstituteMainMenuContext.Institute
                                                where (Modulename.IVRMM_Id == MainMenu.IVRMM_Id && Ins.MI_Id == MainMenu.MI_Id &&
                                                       MainMenu.IVRMMMI_ParentId != 0 && MasterMainMenuID.Contains(MainMenu.IVRMMM_Id)
                                                       && MainMenu.MI_Id ==data.MI_Id)
                                                select new InstituteMainMenuDTO
                                                {
                                                    modulename = Modulename.IVRMM_ModuleName,
                                                    IVRMMMI_MenuName = MainMenu.IVRMMMI_MenuName,
                                                    IVRMMMI_Id = MainMenu.IVRMMMI_Id,
                                                    MI_Name = Ins.MI_Name,
                                                    IVRMMMI_MenuOrder = MainMenu.IVRMMMI_MenuOrder
                                                }).OrderBy(t => t.IVRMMMI_MenuOrder).Distinct().ToArray();



            return data;
        }

        public InstituteMainMenuDTO getMenudetailsByModuleId(InstituteMainMenuDTO ID)
        {

            var MainMenuINS = _InstituteMainMenuContext.InstituteMainMenuDMO.Where(t => t.IVRMMMI_ParentId.Equals(0)).Select(d => d.IVRMMM_Id).ToArray().ToArray();
            var MasterMainMenuID = _InstituteMainMenuContext.MasterMainMenuDMO.Where(t => t.IVRMMM_ParentId != 0).Select(d => d.IVRMMM_Id).ToArray().ToArray();
            InstituteMainMenuDTO InstituteMainMenuDTO = new InstituteMainMenuDTO();

            if (ID.IVRMM_Id == 0)
            {
                InstituteMainMenuDTO.masterMainMenuName = (from Modulename in _InstituteMainMenuContext.MasterModule
                                                           from MasterMainMenu in _InstituteMainMenuContext.MasterMainMenuDMO
                                                           from insmenu in _InstituteMainMenuContext.InstituteMainMenuDMO
                                                           where (Modulename.IVRMM_Id == MasterMainMenu.IVRMM_Id && Modulename.IVRMM_Id== insmenu.IVRMM_Id && MasterMainMenu.IVRMMM_Id==insmenu.IVRMMM_Id &&
                                                                  MasterMainMenu.IVRMMM_ParentId == 0 && MainMenuINS.Contains(MasterMainMenu.IVRMMM_Id) && insmenu.MI_Id == ID.MI_Id)
                                                           select new InstituteMainMenuDTO
                                                           {
                                                               modulename = Modulename.IVRMM_ModuleName,
                                                               IVRMMMI_MenuName = insmenu.IVRMMMI_MenuName,
                                                               IVRMMM_Id = MasterMainMenu.IVRMMM_Id
                                                           }).OrderBy(t => t.IVRMMMI_Id).Distinct().ToArray();
            }
            else
            {

                InstituteMainMenuDTO.masterMainMenuName = (from Modulename in _InstituteMainMenuContext.MasterModule
                                                           from MasterMainMenu in _InstituteMainMenuContext.MasterMainMenuDMO
                                                           from insmenu in _InstituteMainMenuContext.InstituteMainMenuDMO
                                                           where (Modulename.IVRMM_Id == MasterMainMenu.IVRMM_Id &&
                                                           Modulename.IVRMM_Id == ID.IVRMM_Id && Modulename.IVRMM_Id == insmenu.IVRMM_Id && MasterMainMenu.IVRMMM_Id == insmenu.IVRMMM_Id &&
                                                                  MasterMainMenu.IVRMMM_ParentId == 0 && MainMenuINS.Contains(MasterMainMenu.IVRMMM_Id) && insmenu.MI_Id== ID.MI_Id)
                                                           select new InstituteMainMenuDTO
                                                           {
                                                               modulename = Modulename.IVRMM_ModuleName,
                                                               IVRMMMI_MenuName = insmenu.IVRMMMI_MenuName,
                                                               IVRMMM_Id = MasterMainMenu.IVRMMM_Id
                                                           }).OrderBy(t => t.IVRMMMI_Id).Distinct().ToArray();
            }

          

            InstituteMainMenuDTO.GridDetails = (from Modulename in _InstituteMainMenuContext.MasterModule
                                from MainMenu in _InstituteMainMenuContext.InstituteMainMenuDMO
                                from Ins in _InstituteMainMenuContext.Institute
                                where (Modulename.IVRMM_Id == MainMenu.IVRMM_Id && Ins.MI_Id == MainMenu.MI_Id &&
                                       MainMenu.IVRMMMI_ParentId != 0 && MasterMainMenuID.Contains(MainMenu.IVRMMM_Id)
                                       && MainMenu.MI_Id == ID.MI_Id && MainMenu.IVRMM_Id== ID.IVRMM_Id)
                                select new InstituteMainMenuDTO
                                {
                                    modulename = Modulename.IVRMM_ModuleName,
                                    IVRMMMI_MenuName = MainMenu.IVRMMMI_MenuName,
                                    IVRMMMI_Id = MainMenu.IVRMMMI_Id,
                                    MI_Name = Ins.MI_Name,
                                    IVRMMMI_MenuOrder = MainMenu.IVRMMMI_MenuOrder
                                }).OrderBy(t => t.IVRMMMI_MenuOrder).Distinct().ToArray();


            return InstituteMainMenuDTO;
        }

        //public InstituteMainMenuDTO getSubMenudetailsByMainMenuId(InstituteMainMenuDTO data)
        //{
        //    var SUbMenuINS = _InstituteMainMenuContext.InstituteMainMenuDMO.Where(t => t.IVRMMMI_ParentId != 0 && t.MI_Id == data.MI_Id).Select(d => d.IVRMMM_Id).ToArray().ToArray();

        //    var SubMenuInsIds = _InstituteMainMenuContext.MasterMainMenuDMO.Where(t => t.IVRMMM_ParentId != 0 && t.IVRMMM_ParentId == data.IVRMMM_Id).Select(d => d.IVRMMM_Id).ToArray().ToArray();

        //    InstituteMainMenuDTO InstituteMainMenuDTO = new InstituteMainMenuDTO();

        //    if (data.IVRMMM_Id == 0)
        //    {
        //        InstituteMainMenuDTO.masterSubMenuName = (from Modulename in _InstituteMainMenuContext.MasterModule
        //                                                  from MasterMainMenu in _InstituteMainMenuContext.MasterMainMenuDMO
        //                                                  where (Modulename.IVRMM_Id == MasterMainMenu.IVRMM_Id &&
        //                                                         MasterMainMenu.IVRMMM_ParentId != 0 && SubMenuInsIds.Contains(MasterMainMenu.IVRMMM_Id))
        //                                                  select new InstituteMainMenuDTO
        //                                                  {
        //                                                      modulename = Modulename.IVRMM_ModuleName,
        //                                                      IVRMMMI_MenuName = MasterMainMenu.IVRMMM_MenuName,
        //                                                      IVRMMM_Id = MasterMainMenu.IVRMMM_Id
        //                                                  }).OrderBy(t => t.IVRMMMI_Id).Distinct().ToArray();
        //    }
        //    else
        //    {

        //        //InstituteMainMenuDTO.masterSubMenuName = (from Modulename in _InstituteMainMenuContext.MasterModule
        //        //                                           from MasterMainMenu in _InstituteMainMenuContext.MasterMainMenuDMO
        //        //                                           where (Modulename.IVRMM_Id == MasterMainMenu.IVRMM_Id &&
        //        //                                           MasterMainMenu.IVRMMM_ParentId == ID &&
        //        //                                                  MasterMainMenu.IVRMMM_ParentId != 0 ) /*&& SubMenuInsIds.Contains(MasterMainMenu.IVRMMM_Id)*/
        //        //                                          select new InstituteMainMenuDTO
        //        //                                           {
        //        //                                               modulename = Modulename.IVRMM_ModuleName,
        //        //                                               IVRMMMI_MenuName = MasterMainMenu.IVRMMM_MenuName,
        //        //                                               IVRMMM_Id = MasterMainMenu.IVRMMM_Id
        //        //                                           }).OrderBy(t => t.IVRMMMI_Id).Distinct().ToArray();

        //        InstituteMainMenuDTO.masterSubMenuName = (from MasterMainMenu in _InstituteMainMenuContext.MasterMainMenuDMO
        //                                                  from insMasterMainMenu in _InstituteMainMenuContext.InstituteMainMenuDMO
        //                                                  where (MasterMainMenu.IVRMMM_Id == insMasterMainMenu.IVRMMM_Id && MasterMainMenu.IVRMMM_ParentId == data.IVRMMM_Id && MasterMainMenu.IVRMMM_ParentId != 0 &&
        //                                                  !SubMenuInsIds.Contains(insMasterMainMenu.IVRMMMI_Id))
        //                                                  select new InstituteMainMenuDTO
        //                                                  {
        //                                                      //  modulename = Modulename.IVRMM_ModuleName,
        //                                                      IVRMMMI_MenuName = MasterMainMenu.IVRMMM_MenuName,
        //                                                      IVRMMM_Id = MasterMainMenu.IVRMMM_Id
        //                                                  }).OrderBy(t => t.IVRMMMI_Id).Distinct().ToArray();

        //    }

        //    return InstituteMainMenuDTO;
        //}

        public InstituteMainMenuDTO getSubMenudetailsByMainMenuId(InstituteMainMenuDTO data)
        {
            

            var SUbMenuINS = _InstituteMainMenuContext.InstituteMainMenuDMO.Where(t => t.MI_Id==data.MI_Id).Select(d => d.IVRMMM_Id).ToArray().ToArray();

            var SubMenuInsIds = _InstituteMainMenuContext.MasterMainMenuDMO.Where(t => t.IVRMMM_ParentId == data.IVRMMM_Id && !SUbMenuINS.Contains(t.IVRMMM_Id)).Select(d => d.IVRMMM_Id).ToArray().ToArray();

           // var Exactmenu = new long[SubMenuInsIds.Length];
            List<long> Exactmenu = new List<long>();
            for (int Q = 0; Q < SubMenuInsIds.Length; Q++)
            {
                for (int p = 0; p < SUbMenuINS.Length; p++)
                {
                    if(SubMenuInsIds[Q]!= SUbMenuINS[p])
                    {
                        Exactmenu.Add(SubMenuInsIds[Q]);
                    }
                }
            }


                // var SubMenuInsIds = _InstituteMainMenuContext.MasterMainMenuDMO.Where(t => t.IVRMMM_ParentId == data.IVRMMM_Id).Select(d => d.IVRMMM_Id).ToArray().ToArray();

                InstituteMainMenuDTO InstituteMainMenuDTO = new InstituteMainMenuDTO();

            if (data.IVRMMM_Id == 0)
            {
                InstituteMainMenuDTO.masterSubMenuName = (from Modulename in _InstituteMainMenuContext.MasterModule
                                                          from MasterMainMenu in _InstituteMainMenuContext.MasterMainMenuDMO
                                                         
                                                          where (Modulename.IVRMM_Id == MasterMainMenu.IVRMM_Id && 
                                                                 MasterMainMenu.IVRMMM_ParentId != 0 && SubMenuInsIds.Contains(MasterMainMenu.IVRMMM_Id))
                                                          select new InstituteMainMenuDTO
                                                          {
                                                              modulename = Modulename.IVRMM_ModuleName,
                                                              IVRMMMI_MenuName = MasterMainMenu.IVRMMM_MenuName,
                                                              IVRMMM_Id = MasterMainMenu.IVRMMM_Id,
                                                              IVRMMMI_MenuOrder = MasterMainMenu.IVRMMM_MenuOrder
                                                          }).OrderBy(t => t.IVRMMMI_Id).Distinct().ToArray();
            }
            else
            {

                InstituteMainMenuDTO.masterSubMenuName = (from Modulename in _InstituteMainMenuContext.MasterModule
                                                          from MasterMainMenu in _InstituteMainMenuContext.MasterMainMenuDMO
                                                         
                                                          where (Modulename.IVRMM_Id == MasterMainMenu.IVRMM_Id && 
                                                                 MasterMainMenu.IVRMMM_ParentId != 0) && Exactmenu.Contains(MasterMainMenu.IVRMMM_Id) 
                                                          select new InstituteMainMenuDTO
                                                          {
                                                              modulename = Modulename.IVRMM_ModuleName,
                                                              IVRMMMI_MenuName = MasterMainMenu.IVRMMM_MenuName,
                                                              IVRMMM_Id = MasterMainMenu.IVRMMM_Id,
                                                              IVRMMMI_MenuOrder = MasterMainMenu.IVRMMM_MenuOrder
                                                          }).OrderBy(t => t.IVRMMMI_Id).Distinct().ToArray();

                //InstituteMainMenuDTO.masterSubMenuName = (from MasterMainMenu in _InstituteMainMenuContext.MasterMainMenuDMO
                //                                          from insMasterMainMenu in _InstituteMainMenuContext.InstituteMainMenuDMO
                //                                          where (MasterMainMenu.IVRMMM_Id == insMasterMainMenu.IVRMMM_Id && MasterMainMenu.IVRMMM_ParentId == data.IVRMMM_Id && MasterMainMenu.IVRMMM_ParentId != 0 &&
                //                                          !SubMenuInsIds.Contains(insMasterMainMenu.IVRMMMI_Id))
                //                                          select new InstituteMainMenuDTO
                //                                          {
                //                                              //  modulename = Modulename.IVRMM_ModuleName,
                //                                              IVRMMMI_MenuName = MasterMainMenu.IVRMMM_MenuName,
                //                                              IVRMMM_Id = MasterMainMenu.IVRMMM_Id
                //                                          }).OrderBy(t => t.IVRMMMI_Id).Distinct().ToArray();

            }

            return InstituteMainMenuDTO;
        }

        public InstituteMainMenuDTO MasterDeleteMainMenuDTO(int ID)
        {
            InstituteMainMenuDTO InstituteMainMenuDTO = new InstituteMainMenuDTO();
            List<InstituteMainMenuDMO> InstituteMainMenuDMO = new List<InstituteMainMenuDMO>();

            try
            {
                var submenudelete = _InstituteMainMenuContext.submenupageinst.Where(t => t.IVRMMMI_Id.Equals(ID)).ToList();

                if (submenudelete.Count == 0)
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


                var MasterMainMenuID = _InstituteMainMenuContext.MasterMainMenuDMO.Where(t => t.IVRMMM_ParentId.Equals(0)).Select(d => d.IVRMMM_Id).ToArray().ToArray();

                var SUbMenuINS = _InstituteMainMenuContext.InstituteMainMenuDMO.Where(t => t.IVRMMMI_ParentId != 0).Select(d => d.IVRMMM_Id).ToArray().ToArray();

                var SubMenuInsIds = _InstituteMainMenuContext.MasterMainMenuDMO.Where(t => t.IVRMMM_ParentId != 0 && !SUbMenuINS.Contains(t.IVRMMM_Id)).Select(d => d.IVRMMM_Id).ToArray().ToArray();

                InstituteMainMenuDTO.masterSubMenuName = (from Modulename in _InstituteMainMenuContext.MasterModule
                                                          from MasterMainMenu in _InstituteMainMenuContext.MasterMainMenuDMO
                                                          where (Modulename.IVRMM_Id == MasterMainMenu.IVRMM_Id &&
                                                                 MasterMainMenu.IVRMMM_ParentId != 0 && SubMenuInsIds.Contains(MasterMainMenu.IVRMMM_Id))
                                                          select new InstituteMainMenuDTO
                                                          {
                                                              modulename = Modulename.IVRMM_ModuleName,
                                                              IVRMMMI_MenuName = MasterMainMenu.IVRMMM_MenuName,
                                                              IVRMMM_Id = MasterMainMenu.IVRMMM_Id,
                                                              IVRMMMI_MenuOrder = MasterMainMenu.IVRMMM_MenuOrder
                                                          }).OrderBy(t => t.IVRMMMI_Id).Distinct().ToArray();




                InstituteMainMenuDTO.GridDetails = (from Modulename in _InstituteMainMenuContext.MasterModule
                                                    from MainMenu in _InstituteMainMenuContext.InstituteMainMenuDMO
                                                    from Ins in _InstituteMainMenuContext.Institute
                                                    where (Modulename.IVRMM_Id == MainMenu.IVRMM_Id && Ins.MI_Id == MainMenu.MI_Id &&
                                                           MainMenu.IVRMMMI_ParentId != 0 && MasterMainMenuID.Contains(MainMenu.IVRMMM_Id))
                                                    select new InstituteMainMenuDTO
                                                    {
                                                        modulename = Modulename.IVRMM_ModuleName,
                                                        IVRMMMI_MenuName = MainMenu.IVRMMMI_MenuName,
                                                        IVRMMMI_Id = MainMenu.IVRMMMI_Id,
                                                        MI_Name = Ins.MI_Name,
                                                        IVRMMMI_MenuOrder = MainMenu.IVRMMMI_MenuOrder
                                                    }).OrderBy(t => t.IVRMMMI_MenuOrder).Distinct().ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return InstituteMainMenuDTO;
        }



        public InstituteMainMenuDTO MasterMainMenuData(InstituteMainMenuDTO mas)
        {

            using (var transaction = _InstituteMainMenuContext.Database.BeginTransaction())
            {

                if (mas.IVRMMMI_Id != 0)
                {
                    var result = _InstituteMainMenuContext.InstituteMainMenuDMO.Single(t => t.IVRMMMI_Id == mas.IVRMMMI_Id);

                    result.IVRMM_Id = mas.IVRMM_Id;
                    result.IVRMMMI_MenuName = mas.IVRMMMI_MenuName;
                    result.UpdatedDate = DateTime.Now;
                    _InstituteMainMenuContext.Update(result);
                    var contactExists = _InstituteMainMenuContext.SaveChanges();

                    if (contactExists == 1)
                    {
                        mas.returnval = "utrue";
                    }
                    else
                    {
                        mas.returnval = "ufalse";
                    }

                }
                else
                {
                    for (int Q = 0; Q < mas.SelectedMasterMenuDetails.Count; Q++)
                    {
                        List<MasterMainMenuDMO> Allname3 = new List<MasterMainMenuDMO>();
                        // List<InstituteMainMenuDMO> Allname4 = new List<InstituteMainMenuDMO>();

                        InstituteMainMenuDMO MM = Mapper.Map<InstituteMainMenuDMO>(mas);

                        Allname3 = _InstituteMainMenuContext.MasterMainMenuDMO.Where(t => t.IVRMMM_Id.Equals(mas.SelectedMasterMenuDetails[Q].IVRMMM_Id)).ToList();

                        //radha
                        var Allname4 = _InstituteMainMenuContext.MasterMainMenuDMO.Where(t => t.IVRMMM_Id.Equals(mas.SelectedMasterMenuDetails[Q].IVRMMM_Id)).Select(t => t.IVRMMM_ParentId).ToArray().ToArray();

                        var allname5 = _InstituteMainMenuContext.InstituteMainMenuDMO.Where(t => t.IVRMMM_Id.Equals(Allname4[0]) && t.MI_Id==mas.MI_Id).ToArray();
                        // var allname5 = _InstituteMainMenuContext.InstituteMainMenuDMO.Where(!Allname4.Contains(t=>t.IVRMMM_Id)).toArray();

                        var duplicationcheck = _InstituteMainMenuContext.InstituteMainMenuDMO.Where(t => t.IVRMMMI_ParentId != 0 && t.IVRMM_Id == Allname3[0].IVRMM_Id && t.IVRMMM_Id == mas.SelectedMasterMenuDetails[Q].IVRMMM_Id  && t.MI_Id==mas.MI_Id).ToArray();

                        if (duplicationcheck.Length >= 1)
                        {
                            mas.returnval = "Duplicate";
                            return mas;
                        }
                        else
                        {
                            //List<InstituteMainMenuDMO> Order = new List<InstituteMainMenuDMO>();
                            //Order = _InstituteMainMenuContext.InstituteMainMenuDMO.Where(t => t.IVRMMMI_ParentId != 0 && t.IVRMMMI_MenuOrder == mas.IVRMMMI_MenuOrder && t.IVRMM_Id == Allname3[0].IVRMM_Id && t.MI_Id == mas.MI_Id).ToList();
                            //if (Order.Count > 0)
                            //{
                            //    mas.returnval = "DuplicateOrder";
                            //    return mas;
                            //}
                            MM.IVRMMMI_Id = mas.IVRMMMI_Id;
                            MM.IVRMMMI_MenuName = mas.SelectedMasterMenuDetails[Q].IVRMMMI_MenuName;
                            // MM.IVRMMMI_MenuOrder = Allname3[0].IVRMMM_MenuOrder;
                            MM.IVRMMMI_MenuOrder =mas.IVRMMMI_MenuOrder;
                            MM.IVRMMMI_PageNonPageFlag = Allname3[0].IVRMMM_PageNonPageFlag;
                            MM.IVRMMMI_ParentId = allname5[0].IVRMMMI_Id;
                            MM.IVRMMM_Id = mas.SelectedMasterMenuDetails[Q].IVRMMM_Id;
                            MM.IVRMM_Id = Allname3[0].IVRMM_Id;
                            MM.MI_Id = mas.MI_Id;


                            MM.CreatedDate = DateTime.Now;
                            MM.UpdatedDate = DateTime.Now;
                            _InstituteMainMenuContext.Add(MM);
                            var contactExists = _InstituteMainMenuContext.SaveChanges();

                            if (contactExists == 1)
                            {
                                mas.returnval = "atrue";

                                if (MM.IVRMMMI_Id > 0)
                                {
                                    var result = _InstituteMainMenuContext.InstituteMainMenuDMO.Single(t => t.IVRMMMI_Id.Equals(MM.IVRMMMI_Id));
                                    // Mapper.Map(mob, result);
                                    result.IVRMMMI_MenuOrder = Convert.ToInt32(MM.IVRMMMI_Id);
                                    _InstituteMainMenuContext.Update(result);
                                    _InstituteMainMenuContext.SaveChanges();
                                }

                            }
                            else
                            {
                                mas.returnval = "afalse";
                            }

                        }
                    }

                    transaction.Commit();
                }

            }

            var MasterMainMenuID = _InstituteMainMenuContext.MasterMainMenuDMO.Where(t => t.IVRMMM_ParentId.Equals(0)).Select(d => d.IVRMMM_Id).ToArray().ToArray();

            var SUbMenuINS = _InstituteMainMenuContext.InstituteMainMenuDMO.Where(t => t.IVRMMMI_ParentId != 0).Select(d => d.IVRMMM_Id).ToArray().ToArray();

            var SubMenuInsIds = _InstituteMainMenuContext.MasterMainMenuDMO.Where(t => t.IVRMMM_ParentId != 0 && !SUbMenuINS.Contains(t.IVRMMM_Id)).Select(d => d.IVRMMM_Id).ToArray().ToArray();

            mas.masterSubMenuName = (from Modulename in _InstituteMainMenuContext.MasterModule
                                     from MasterMainMenu in _InstituteMainMenuContext.MasterMainMenuDMO
                                     where (Modulename.IVRMM_Id == MasterMainMenu.IVRMM_Id &&
                                            MasterMainMenu.IVRMMM_ParentId != 0 && SubMenuInsIds.Contains(MasterMainMenu.IVRMMM_Id))
                                     select new InstituteMainMenuDTO
                                     {
                                         modulename = Modulename.IVRMM_ModuleName,
                                         IVRMMMI_MenuName = MasterMainMenu.IVRMMM_MenuName,
                                         IVRMMM_Id = MasterMainMenu.IVRMMM_Id,
                                         IVRMMMI_MenuOrder = MasterMainMenu.IVRMMM_MenuOrder
                                     }).OrderBy(t => t.IVRMMMI_Id).Distinct().ToArray();




            mas.GridDetails = (from Modulename in _InstituteMainMenuContext.MasterModule
                               from MainMenu in _InstituteMainMenuContext.InstituteMainMenuDMO
                               from Ins in _InstituteMainMenuContext.Institute
                               where (Modulename.IVRMM_Id == MainMenu.IVRMM_Id && Ins.MI_Id == MainMenu.MI_Id &&
                                      MainMenu.IVRMMMI_ParentId != 0 && MasterMainMenuID.Contains(MainMenu.IVRMMM_Id))
                               select new InstituteMainMenuDTO
                               {
                                   modulename = Modulename.IVRMM_ModuleName,
                                   IVRMMMI_MenuName = MainMenu.IVRMMMI_MenuName,
                                   IVRMMMI_Id = MainMenu.IVRMMMI_Id,
                                   MI_Name = Ins.MI_Name,
                                   IVRMMMI_MenuOrder = MainMenu.IVRMMMI_MenuOrder
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
