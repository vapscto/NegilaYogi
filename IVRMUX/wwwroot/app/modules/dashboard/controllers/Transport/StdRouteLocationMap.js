

(function () {
    'use strict';
    angular
.module('app')
        .controller('StdRouteLocationMapController', StdRouteLocationMapController)

    StdRouteLocationMapController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter']
    function StdRouteLocationMapController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {
        var paginationformasters;
        //var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        //if (ivrmcofigsettings.length > 0) {
        //    paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        //}
        $scope.obj = {};
        $scope.searchValue = '';
        $scope.TRSLM_Id =0;
        //  paginationformasters = 10;
        $scope.sort1 = function (keyname) {
            
            $scope.sortKey1 = keyname;   //set the sortKey to the param passed
            $scope.reverse1 = !$scope.reverse1; //if true make it false and vice versa
        }
        var temp_grp_list = [];
        $scope.TRSR_Date = new Date();
        $scope.loaddata = function () {
            
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("StdRouteLocationMap/getalldetails123", pageid).
        then(function (promise) {
            
            $scope.yearlist = promise.acayear;
            $scope.section_list = promise.sectionlist;
            $scope.class_list = promise.classlist;
            $scope.route_list = promise.busroutelist;
            $scope.location_list = promise.locationlist;
            $scope.schedule_list = promise.schedulelist;
            $scope.group_list = promise.grouplist;
            temp_grp_list = promise.grouplist;
            // $scope.grid_list = promise.reportdatelist;
            var date_td = new Date();
            angular.forEach(promise.reportdatelist, function (we) {
                if (new Date(we.trsR_Date) > date_td) {
                    we.opt_flag = true;
                }
                we.trsR_Date = $filter('date')(new Date(we.trsR_Date), 'dd-MM-yyyy');
            })
            $scope.students_list = promise.reportdatelist;



            //var maxDate = Math.max.apply(Math, $scope.students_list);
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

        })
        }


        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        //$scope.clear_fee_challan = function () {
        //    $state.reload();
        //    $scope.loaddata();
        //}

        $scope.get_cls_secs = function () {
            
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                //  "Adm_no_name": $scope.radio_button,
            }
            apiService.create("StdRouteLocationMap/get_cls_secs", data).then(function (promise) {
                
                $scope.class_list = promise.classlist;
                $scope.section_list = promise.sectionlist;

                $scope.clsdrp = "";
                $scope.sectiondrp = "";

                var iddata = $scope.asmaY_Id;
                
                for (var k = 0; k < $scope.yearlist.length; k++) {

                    if ($scope.yearlist[k].asmaY_Id == iddata) {

                        var data = $scope.yearlist[k].asmaY_Year;

                    }

                }
                $scope.Today_td = new Date();
                if (data != null) {
                    
                    console.log(data);
                    var name, name1;
                    for (var i = 0; i < data.length; i++) {
                        if (i < 4) {
                            if (i == 0) {
                                name = data[i];
                            } else {
                                name += data[i];
                            }
                        }
                        if (i > 4) {
                            if (i == 5) {
                                name1 = data[5];
                            } else {
                                name1 += data[i];
                            }
                        }
                    }
                    $scope.fromDate = name;
                    $scope.toDate = name1;
                    $scope.frommon = "";
                    $scope.tomon = "";
                    $scope.fromDay = "";
                    $scope.toDay = "";
                    // For Academic From Date
                    $scope.minDatemf = new Date(
                          $scope.fromDate,
                           $scope.frommon,
                            $scope.fromDay + 1);

                    $scope.maxDatemf = new Date(
                          $scope.toDate,
                           $scope.tomon,
                            $scope.toDay + 365);
                    $scope.TRSR_Date = new Date();


                }
                $scope.Selected_stu_grps_list = [];
                $scope.students = [];
            });
        }



        $scope.editvalue = function (userdata) {
            debugger;
            $scope.radioval = 'class_wise';
            $scope.students = [];
            $scope.alrdy_stu_list = [];
            $scope.grpeditlist = [];
            var data = {
                "ASMAY_Id": userdata.asmaY_Id,
                "AMST_Id": userdata.amsT_Id,
                "TRSLM_Id": userdata.trslM_Id,
            }
            apiService.create("StdRouteLocationMap/getreportedit/", data).
                then(function (promise) {
                    //if (promise.admsudentslist != null && promise.admsudentslist != "") {

                    //    $scope.Balance_report = true;
                    //    $scope.fee_clear = false;
                    //    $scope.print_flag = false;
                    //    $scope.students = promise.admsudentslist;
                    //    $scope.grpeditlist = promise.grpeditlist;
                    //    //For Filter Location MB
                    //    angular.forEach($scope.students, function (stu) {
                    //        // stu.schedule_list_p = $scope.schedule_list;
                    //        // stu.schedule_list_d = $scope.schedule_list;
                    //        stu.location_list_p = $scope.location_list;
                    //        stu.location_list_d = $scope.location_list;
                    //        stu.sesslist_list_d = $scope.sesslist_list_d;
                    //        stu.sesslist_list_p = $scope.sesslist_list_p;
                    //    })
                    //    //MB


                    //    $scope.asmaY_Id = $scope.students[0].asmaY_Id
                    //    $scope.clsdrp = $scope.students[0].asmcL_Id
                    //    $scope.sectiondrp = $scope.students[0].asmS_Id

                    //    $scope.alrdy_stu_list = promise.alrdy_stu_list;
                    //    if (promise.alrdy_stu_list != null && promise.alrdy_stu_list != "" && promise.alrdy_stu_list.length > 0) {

                    //        $scope.Selected_stu_grps_list = [];
                    //        angular.forEach($scope.students, function (stu1) {
                    //            var Selected_grps_list = [];
                    //            angular.forEach(promise.alrdy_stu_list, function (stu2) {
                    //                if (stu2.amsT_Id == stu1.amsT_Id /*&& stu2.asmaY_Id == $scope.asmaY_Id*/) {
                    //                    stu1.checkedvalue = true;
                    //                    $scope.optionToggled(stu1);
                    //                    stu1.grp_flag = true;
                    //                    stu1.TRMR_Id = stu2.trmR_Id;
                    //                    // stu1.TRSR_PickupSchedule = stu2.trsR_PickupSchedule;
                    //                    stu1.TRSR_PickUpLocation = stu2.trsR_PickUpLocation;
                    //                    stu1.TRMR_Drop_Route = stu2.trmR_Drop_Route;
                    //                    //stu1.TRSR_DropSchedule = stu2.trsR_DropSchedule;
                    //                    stu1.TRSR_DropLocation = stu2.trsR_DropLocation;
                    //                    stu1.Drop_Session = stu2.trsR_DropSession;
                    //                    stu1.PickUp_Session = stu2.trsR_PickupSession;
                    //                    stu1.ASTA_Id = stu2.astA_Id;
                    //                    //var Selected_grps_list = [];

                    //                    angular.forEach($scope.grpeditlist, function (jj) {
                    //                        angular.forEach($scope.group_list, function (itm) {
                    //                            if (itm.fmG_Id == jj.fmG_Id) {

                    //                                Selected_grps_list.push({ TRML_Id: itm.fmG_Id, TRMA_AreaName: itm.fmG_GroupName });
                    //                            }
                    //                        })
                    //                    })
                    //                    for (var i = 0; i < $scope.Selected_stu_grps_list.length; i++) {
                    //                        var already_count = 0;
                    //                        if (stu2.amsT_Id == $scope.Selected_stu_grps_list[i].amsT_Id) {
                    //                            already_count += 1;
                    //                            // $scope.Selected_stu_grps_list.splice(i, 1);
                    //                            if (already_count > 0) {
                    //                                $scope.Selected_stu_grps_list.splice(i, 1);
                    //                            }
                    //                        }
                    //                    }

                    //                    $scope.Selected_stu_grps_list.push({ amsT_Id: stu2.amsT_Id, grp_list: Selected_grps_list, TRSR_ApplicationNo: stu2.trsR_ApplicationNo, TRSR_PickUpMobileNo: stu2.trsR_PickUpMobileNo, TRSR_DropMobileNo: stu2.trsR_DropMobileNo });
                    //                    if (stu1.TRMR_Id != undefined && stu1.TRMR_Id != '' && stu1.TRMR_Id != null) {
                    //                        $scope.get_loca_sches_p(stu1);

                    //                    }
                    //                    if (stu1.TRMR_Drop_Route != undefined && stu1.TRMR_Drop_Route != '' && stu1.TRMR_Drop_Route != null) {
                    //                        $scope.get_loca_sches_d(stu1);

                    //                    }

                    //                }
                    //            })
                    //        })
                    //    }


                    //}
                    if (promise.admsudentslist != null && promise.admsudentslist != "") {

                        $scope.Balance_report = true;
                        $scope.fee_clear = false;
                        $scope.print_flag = false;
                        $scope.students = promise.admsudentslist;
                        //For Filter Location MB
                        angular.forEach($scope.students, function (stu) {
                            //stu.schedule_list_p = $scope.schedule_list;
                            //stu.schedule_list_d = $scope.schedule_list;
                            stu.location_list_p = $scope.location_list;
                            //stu.location_list_d = $scope.location_list;
                        })
                        //MB
                             $scope.asmaY_Id = $scope.students[0].asmaY_Id
                        $scope.clsdrp = $scope.students[0].asmcL_Id
                        $scope.sectiondrp = $scope.students[0].asmS_Id
                        $scope.TRSLM_Id = $scope.students[0].trslM_Id

                        debugger;
                        $scope.alrdy_stu_list = promise.alrdy_stu_list;
                        $scope.TRSLM_Id = $scope.alrdy_stu_list[0].trslM_Id
                        if (promise.alrdy_stu_list != null && promise.alrdy_stu_list != "" && promise.alrdy_stu_list.length > 0) {

                            $scope.Selected_stu_grps_list = [];
                            angular.forEach($scope.students, function (stu1) {
                                var Selected_grps_list = [];
                                angular.forEach(promise.alrdy_stu_list, function (stu2) {
                                    if (stu2.amsT_Id == stu1.amsT_Id ) {
                                        stu1.checkedvalue = true;
                                        $scope.optionToggled(stu1);
                                        stu1.grp_flag = true;
                                        stu1.TRMR_Id = stu2.trmR_Id;
                                       
                                        stu1.TRMR_Drop_Route = stu2.trmR_Id;
                                        $scope.on_pic_route_change(stu1.TRMR_Id);

                                        stu1.TRSR_PickUpLocation = stu2.trmL_Id;

                                        //var Selected_grps_list = [];
                                        //angular.forEach($scope.group_list, function (itm) {
                                        //    if (itm.fmG_Id == stu2.fmG_Id) {

                                        //        Selected_grps_list.push({ TRML_Id: itm.fmG_Id, TRMA_AreaName: itm.fmG_GroupName });
                                        //    }
                                        //})

                                        //for (var i = 0; i < $scope.Selected_stu_grps_list.length; i++) {
                                        //    var already_count = 0;
                                        //    if (stu2.amsT_Id == $scope.Selected_stu_grps_list[i].amsT_Id) {
                                        //        already_count += 1;
                                        //        // $scope.Selected_stu_grps_list.splice(i, 1);
                                        //        if (already_count > 0) {
                                        //            $scope.Selected_stu_grps_list.splice(i, 1);
                                        //        }
                                        //    }
                                        //}

                                        //$scope.Selected_stu_grps_list.push({ amsT_Id: stu2.amsT_Id, grp_list: Selected_grps_list, TRSR_ApplicationNo: stu2.trsR_ApplicationNo, TRSR_PickUpMobileNo: stu2.trsR_PickUpMobileNo, TRSR_DropMobileNo: stu2.trsR_DropMobileNo });
                                        //$scope.get_loca_sches_p(stu1);
                                        //  $scope.get_loca_sches_d(stu1);
                                        $scope.get_loca_sches_p12(stu1)
                                    }
                                })
                            })
                        }






                    }
                    else {
                        swal("No Record Found");
                        $scope.Balance_report = false;
                        $scope.print_flag = true;
                        $scope.Clearid();
                    }
                })
        }
        $scope.Clearid = function () {
            $state.reload();
            $scope.loaddata();
        }

        $scope.get_data = function () {

            var data = {
                
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.clsdrp,
                "ASMS_Id": $scope.sectiondrp,
            }
            apiService.create("StdRouteLocationMap/get_data", data).then(function (promise) {

                if (promise.admsudentslist != null && promise.admsudentslist != "") {

                    $scope.Balance_report = true;
                    $scope.fee_clear = false;
                    $scope.print_flag = false;
                    $scope.students = promise.admsudentslist;
                    //For Filter Location MB
                    angular.forEach($scope.students, function (stu) {
                        //stu.schedule_list_p = $scope.schedule_list;
                        //stu.schedule_list_d = $scope.schedule_list;
                        stu.location_list_p = $scope.location_list;
                        //stu.location_list_d = $scope.location_list;
                    })
                    //MB
                    $scope.asmaY_Id = $scope.students[0].asmaY_Id
                    $scope.clsdrp = $scope.students[0].asmcL_Id
                    $scope.sectiondrp = $scope.students[0].asmS_Id
                    $scope.TRSLM_Id = $scope.students[0].trslM_Id

                   
                    $scope.alrdy_stu_list = promise.alrdy_stu_list;
                    $scope.TRSLM_Id = $scope.alrdy_stu_list[0].trslM_Id
                    if (promise.alrdy_stu_list != null && promise.alrdy_stu_list != "" && promise.alrdy_stu_list.length > 0) {

                        $scope.Selected_stu_grps_list = [];
                        angular.forEach($scope.students, function (stu1) {
                            var Selected_grps_list = [];
                            angular.forEach(promise.alrdy_stu_list, function (stu2) {
                                if (stu2.amsT_Id == stu1.amsT_Id) {
                                    stu1.checkedvalue = true;
                                    $scope.optionToggled(stu1);
                                    stu1.grp_flag = true;
                                    stu1.TRMR_Id = stu2.trmR_Id;

                                    stu1.TRMR_Drop_Route = stu2.trmR_Id;
                                    $scope.on_pic_route_change(stu1.TRMR_Id);

                                    stu1.TRSR_PickUpLocation = stu2.trmL_Id;

                                    //var Selected_grps_list = [];
                                    //angular.forEach($scope.group_list, function (itm) {
                                    //    if (itm.fmG_Id == stu2.fmG_Id) {

                                    //        Selected_grps_list.push({ TRML_Id: itm.fmG_Id, TRMA_AreaName: itm.fmG_GroupName });
                                    //    }
                                    //})

                                    //for (var i = 0; i < $scope.Selected_stu_grps_list.length; i++) {
                                    //    var already_count = 0;
                                    //    if (stu2.amsT_Id == $scope.Selected_stu_grps_list[i].amsT_Id) {
                                    //        already_count += 1;
                                    //        // $scope.Selected_stu_grps_list.splice(i, 1);
                                    //        if (already_count > 0) {
                                    //            $scope.Selected_stu_grps_list.splice(i, 1);
                                    //        }
                                    //    }
                                    //}

                                    //$scope.Selected_stu_grps_list.push({ amsT_Id: stu2.amsT_Id, grp_list: Selected_grps_list, TRSR_ApplicationNo: stu2.trsR_ApplicationNo, TRSR_PickUpMobileNo: stu2.trsR_PickUpMobileNo, TRSR_DropMobileNo: stu2.trsR_DropMobileNo });
                                    //$scope.get_loca_sches_p(stu1);
                                    //  $scope.get_loca_sches_d(stu1);
                                    $scope.get_loca_sches_p12(stu1)
                                }
                            })
                        })
                    }






                }
                else {
                    swal("No Record Found");
                    $scope.Balance_report = false;
                    $scope.print_flag = true;
                    $scope.Clearid();
                }
        

            });
        }



        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.students, function (itm) {
                itm.checkedvalue = toggleStatus;
            });
        }

        $scope.optionToggled = function (user) {
            $scope.all = $scope.students.every(function (itm) { return itm.checkedvalue; })

            //if (user.checkedvalue == false) {
            //    user.TRMR_Id = '';
            //    user.TRSR_PickUpLocation = '';
            //    user.installmentlist = [];
            //}

        }

        $scope.students1 = [];
        $scope.savedata = function (pagesrecord) {
            debugger;
            if ($scope.myForm.$valid) {
                $scope.students1 = [];
                var count_g = 0;
                angular.forEach(pagesrecord, function (re) {
                    if (re.checkedvalue && !re.grp_flag) {
                        count_g += 1;
                    }
                })
                //if (count_g == 0) {
                    if ($scope.all == true) {
                        angular.forEach(pagesrecord, function (student) {
                            $scope.students1.push(student);
                        });
                    } else {
                        angular.forEach(pagesrecord, function (student) {
                            if (student.checkedvalue == true) {
                                $scope.students1.push(student);
                            }
                        });
                    }


                    if ($scope.students1.length === 0) {
                        swal('Please Select altleast one Record')
                    }
                    else {
                        var somedata = [];
                        angular.forEach($scope.students1, function (std) {
                            angular.forEach(std.installmentlist, function (ab) {
                                if (ab.checkedgrplst1 == true) {
                                    somedata.push({ amsT_Id: std.amsT_Id, ftI_Id: ab.FTI_Id });
                                    
                                }
                            })

                        });
                        $scope.TRSR_Date = new Date($scope.TRSR_Date).toDateString();
                        //angular.forEach($scope.students1, function (ore) {

                        //    if (ore.TRSR_PickupSchedule == "") {
                        //        ore.TRSR_PickupSchedule = 0;
                        //    }
                        //    if (ore.TRSR_PickUpLocation == "") {
                        //        ore.TRSR_PickUpLocation = 0;
                        //    }
                        //    if (ore.TRSR_DropSchedule == "") {
                        //        ore.TRSR_DropSchedule = 0;
                        //    }
                        //    if (ore.TRSR_DropLocation == "") {
                        //        ore.TRSR_DropLocation = 0;
                        //    }
                        //})

                        var data = {

                            stddatalist: $scope.students1,
                         "TRSR_Date": $scope.TRSR_Date,
                            Inst_data: somedata,
                            "ASMAY_Id": Number($scope.asmaY_Id),
                            "TRSLM_Id": $scope.TRSLM_Id,

                        }
                        
                        
                        var config = {
                            headers: {
                                'Content-Type': 'application/json;'
                            }
                        }

                        apiService.create("StdRouteLocationMap/savedata", data).

                    then(function (promise) {

                        if (promise.returnval == true) {

                            //  swal('Record ' + promise.returntxt + ' Successfully', 'success');
                            //$scope.Clearid();
                            swal('Data Successfully Saved');
                        }
                        else {
                            // swal('Record Not ' + promise.returntxt + ' Successfully', 'Failed');
                            swal('Data Not Saved');

                        }
                        $state.reload();
                        // $scope.loaddata();
                    })
                    }
                //}
                //else {
                //    swal("Map Selected Students With Groups");
                //}



            }
            else {
                $scope.submitted = true;
            }

        }
        $scope.clear_fee_balance = function () {
           

            $state.reload();

        }

        $scope.student_falg = false;

        $scope.on_pic_route_change = function (rr) {
       
            $scope.picloclist = [];
            $scope.picsesslist = [];
            var data = {
                "TRMR_Id": rr,
                //  "Adm_no_name": $scope.radio_button,
            }
            apiService.create("StdRouteLocationMap/on_pic_route_change", data).then(function (promise) {

                $scope.location_list = promise.picloclist;
              
                angular.forEach($scope.students, function (stu) {
                    if (stu.checkedvalue == true) {
                        if (stu.TRMR_Id == rr) {
                            stu.location_list_p = $scope.location_list;
                        }
                    }
                    
                  
                   
                })
               

            });
        }


        //$scope.Balance_report = true;
        $scope.print_flag = true;
        $scope.submitted = false;
        $scope.ShowReportdata = function () {
            $scope.TRSLM_Id = 0;
            if ($scope.myForm.$valid) {

                var data = {

                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.clsdrp,
                    "ASMS_Id": $scope.sectiondrp,
                    //"studenttype": $scope.status,

                }
                apiService.create("StdRouteLocationMap/getreport/", data).
            then(function (promise) {
                if (promise.admsudentslist != null && promise.admsudentslist != "") {

                    $scope.Balance_report = true;
                    $scope.fee_clear = false;
                    $scope.print_flag = false;
                    $scope.students = promise.admsudentslist;
             
                    angular.forEach($scope.students, function (stu) {
     
                   stu.location_list_p = $scope.location_list;
                    
                    })
        

              
                    $scope.alrdy_stu_list = promise.alrdy_stu_list;
                    if (promise.alrdy_stu_list != null && promise.alrdy_stu_list != "" && promise.alrdy_stu_list.length > 0) {

                        $scope.Selected_stu_grps_list = [];
                        angular.forEach($scope.students, function (stu1) {
                            var Selected_grps_list = [];
                            angular.forEach(promise.alrdy_stu_list, function (stu2) {
                                if (stu2.amsT_Id == stu1.amsT_Id && stu2.asmaY_Id == $scope.asmaY_Id) {
                                    stu1.checkedvalue = true;
                                    $scope.optionToggled(stu1);
                                    stu1.grp_flag = true;
                                    stu1.TRMR_Id = stu2.trmR_Id;
                                    stu1.TRSR_PickUpLocation = stu2.trmL_Id;
                                    stu1.TRMR_Drop_Route = stu2.trmR_Id;
                                   
                                  
                                    $scope.get_loca_sches_p1(stu1)
                                }
                            })
                        })
                    }




                  

                }
                else {
                    swal("No Record Found");
                    $scope.Balance_report = false;
                    $scope.print_flag = true;
                    $scope.Clearid();
                }
            })
            }
            else {
                $scope.submitted = true;
            }
        }
        $scope.students = [];
        $scope.filterdata = "NameRegNo";
        $scope.get_sections = function () {
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.clsdrp
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("StdRouteLocationMap/get_sections", data).
           then(function (promise) {
               $scope.section_list = promise.sectionlist;
               $scope.sectiondrp = "";
               $scope.Selected_stu_grps_list = [];
               $scope.students = [];

           })
        };


        //search start

        $scope.search_flag = false;
        $scope.search123 = "";
        var search_s = "";
        var list_s = [];
        $scope.onselectsearch = function () {
            search_s = $scope.search123;
            list_s = $scope.receiptgrid;
            if (search_s == "" || search_s == undefined) {
                swal("Select Any Field For Search");
                $scope.search_flag = false;
            }
            else {
                $scope.search_flag = true;

                if ($scope.search123 == "3" || $scope.search123 == "4") {
                    $scope.txt = false;
                    $scope.numbr = true;
                    // $scope.dat = false;

                }
                    //else if ($scope.search123 == "4") {

                    //    $scope.txt = false;
                    //    $scope.numbr = false;
                    //    $scope.dat = true;

                    //}
                else {
                    $scope.txt = true;
                    $scope.numbr = false;
                    // $scope.dat = false;

                }
                $scope.searchtxt = "";
                //   $scope.searchdat = "";
                $scope.searchnumbr = "";

            }
        }
        $scope.ShowSearch_Report = function () {
            
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            if ($scope.searchtxt != "" || $scope.searchnumbr != "") { // || $scope.searchdat != ""
                if ($scope.search123 == "3" || $scope.search123 == "4") {
                    var data = {
                        "searchType": $scope.search123,
                        "searchnumber": $scope.searchnumbr
                    }
                }
                    //else if ($scope.search123 == "4") {
                    //    

                    //    var date = new Date($scope.searchdat).toDateString("dd/MM/yyyy");
                    //    var data = {
                    //        "searchType": $scope.search123,
                    //        "searchdate": date
                    //    }
                    //}
                else {

                    var data = {
                        "searchType": $scope.search123,
                        "searchtext": $scope.searchtxt
                    }
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("StdRouteLocationMap/searching", data).
            then(function (promise) {
                
                $scope.students_list = promise.tempararyArrayhEADListnew;
                $scope.totcountfirst = promise.tempararyArrayhEADListnew.length;
                //for (var i = 0; i < promise.tempararyArrayhEADListnew.length; i++) {
                //    var name1 = promise.tempararyArrayhEADListnew[i].amsT_FirstName;
                //    if (promise.tempararyArrayhEADListnew[i].amsT_MiddleName !== null) {
                //        name1 += " " + promise.tempararyArrayhEADListnew[i].amsT_MiddleName;
                //    }
                //    if (promise.tempararyArrayhEADListnew[i].amsT_LastName != null) {
                //        name1 += " " + promise.tempararyArrayhEADListnew[i].amsT_LastName;
                //    }
                //    $scope.vals1.push(name1);
                //}
                //angular.forEach($scope.vals1, function (v, k) {
                //    $scope.angularData1.nameList1.push({
                //        'fullname1': v
                //    });
                //});

                //var s = 0;
                //angular.forEach($scope.students_list, function (obj) {
                //    //Using bracket notation
                //    obj["fullname1"] = $scope.angularData1.nameList1[s].fullname1;
                //    s++;
                //});
                if (promise.tempararyArrayhEADListnew == null || promise.tempararyArrayhEADListnew == "") {
                    swal("Record Does Not Exist For Searched Data !!!!!!")
                }
            })
            }
            else {
                swal("Data Is Needed For Search ");
            }
        }
        $scope.clearsearch = function () {
            $scope.search123 = "";
            $scope.search_flag = false;
            $scope.searchtxt = "";
            $scope.searchnumbr = "";
            $scope.searchdat = "";
        }
        //search end

        $scope.clear_dues_stu = function (user) {
            angular.forEach($scope.students, function (se) {
                if (se.amsT_Id == user.amsT_Id) {
                    if (Number(se.fmoB_Institution_Due) > 0)
                        se.fmoB_Student_Due = 0;
                    //else if(Number(se.fmoB_Institution_Due)==0)
                    //    se.fmoB_Student_Due = 1;
                }
            })
        }
        $scope.clear_dues_inst = function (user) {
            angular.forEach($scope.students, function (se) {
                if (se.amsT_Id == user.amsT_Id) {
                    if (Number(se.fmoB_Institution_Due) > 0)
                        se.fmoB_Institution_Due = 0;
                    //else if (Number(se.fmoB_Institution_Due) == 0)
                    //    se.fmoB_Institution_Due = 1;

                }
            })
        }

        $scope.disableappno = false;
        $scope.TRSR_ApplicationNo1 = "";

        $scope.select_grps = function (sel_subexms, user) {

            debugger;
            $scope.group_list = temp_grp_list;
            $scope.Student_Name = user.amsT_FirstName;
            $scope.amsT_Id = user.amsT_Id;
            $scope.astA_Id = user.ASTA_Id;
            //  $scope.TRSR_ApplicationNo = $scope.Selected_stu_grps_list[a].TRSR_ApplicationNo;
            $scope.TRSR_PickUpMobileNo = user.amsT_MobileNo;
            $scope.TRSR_DropMobileNo = user.amsT_MobileNo;
            var count = 0;
            for (var a = 0; a < $scope.Selected_stu_grps_list.length; a++) {
                if ($scope.Selected_stu_grps_list[a].amsT_Id == user.amsT_Id) {
                    count += 1;
                    for (var b = 0; b < $scope.group_list.length; b++) {
                        var subexam_count = 0;
                        for (var c = 0; c < $scope.Selected_stu_grps_list[a].grp_list.length; c++) {
                            if ($scope.group_list[b].fmG_Id == $scope.Selected_stu_grps_list[a].grp_list[c].TRML_Id) {
                                var itm = $scope.group_list[b];
                                var itm1 = $scope.Selected_stu_grps_list[a].grp_list[c];
                                subexam_count += 1;
                                itm.checkedvalue_g = true;
                            }

                        }
                        if (subexam_count == 0) {
                            var itm = $scope.group_list[b];
                            itm.checkedvalue_g = false;
                        }
                    }
                    $scope.TRSR_ApplicationNo = $scope.Selected_stu_grps_list[a].TRSR_ApplicationNo;
                    $scope.TRSR_ApplicationNo1 = $scope.Selected_stu_grps_list[a].TRSR_ApplicationNo;
                    $scope.TRSR_PickUpMobileNo = $scope.Selected_stu_grps_list[a].TRSR_PickUpMobileNo;
                    $scope.TRSR_DropMobileNo = $scope.Selected_stu_grps_list[a].TRSR_DropMobileNo;

                }
                //if ($scope.TRSR_ApplicationNo1 != "") {
                //    $scope.disableappno = true;
                //} else {
                //    $scope.disableappno = false;
                //}
                if ($scope.astA_Id >0) {
                    $scope.disableappno = true;
                } else {
                    $scope.disableappno = false;
                }

            }
            if (sel_subexms == true) {
                //$scope.exam_check = true;

                if (count == 0) {
                    $scope.exam_check = true;
                    $scope.clear2();

                }
                $('#popup5').modal('show');
            } else if (sel_subexms == false) {
                $scope.exam_check = false;

                swal({
                    title: "Are you sure",
                    text: "Do you want to Delete Groups Mapped to " + $scope.Student_Name + " ???",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete Selection!",
                    // cancelButtonColor: "#24d46a",
                    cancelButtonText: "No, Change Selection!",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
               function (isConfirm) {
                   if (isConfirm) {
                       for (var i = 0; i < $scope.Selected_stu_grps_list.length; i++) {
                           if ($scope.Selected_stu_grps_list[i].amsT_Id == user.amsT_Id) {
                               $scope.Selected_stu_grps_list.splice(i, 1);
                           }
                       }
                       swal('Deleted Successfully');
                   }
                   else {
                       swal("Now You Can Change Selection");
                       angular.forEach($scope.students, function (itm) {
                           if (itm.amsT_Id == user.amsT_Id) {
                               
                               itm.grp_flag = true;
                               $scope.select_grps(itm.grp_flag, user);
                               $('#popup5').modal('show');
                           }
                       })
                   }
               });


            }

        }
        $scope.exam_check = false;
        $scope.clearpopupgrid5 = function () {
            
            if ($scope.exam_check == true) {
                angular.forEach($scope.students, function (itm) {
                    if (itm.amsT_Id == $scope.amsT_Id) {
                        
                        itm.grp_flag = false;
                    }
                })
            }
            $scope.TRSR_ApplicationNo1 = "";
            $('#popup5').modal('hide');
        }
        $scope.clear2 = function () {
            
            $scope.group_list = temp_grp_list;
            angular.forEach($scope.group_list, function (itm1) {
                itm1.checkedvalue_g = false;
            })
            $scope.TRSR_ApplicationNo = "";
            //$scope.TRSR_PickUpMobileNo = "";
            //$scope.TRSR_DropMobileNo = "";

            $scope.submitted2 = false;
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();

        };
        $scope.isOptionsRequired_grp = function () {
            return !$scope.group_list.some(function (options1) {
                return options1.checkedvalue_g;
            });
        }
        $scope.submitted2 = false;
        $scope.interacted2 = function (field) {
            return $scope.submitted2 || field.$dirty;
        };
        $scope.Selected_stu_grps_list = [];
    
        $scope.students = [];
        $scope.clear1 = function () {
            $scope.asmaY_Id = "";
            $scope.TRSR_Date = "";
            $scope.TRSR_Date = new Date();
            $scope.clsdrp = "";
            $scope.sectiondrp = "";
            $scope.Selected_stu_grps_list = [];
            $scope.students = [];
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        }
        $scope.deactive = function (employee) {
            
            var mgs = "";
            var confirmmgs = "";

            var data = {
                "TRSLM_Id": employee.trslM_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.trslM_ActiveFlag === true) {
                //  mgs = "Deactive";
                mgs = "Delete";
                confirmmgs = "Deleted";
            }
            else {
                //mgs = "Active";
                mgs = "Activate";
                confirmmgs = "Activated";



            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
        function (isConfirm) {
            if (isConfirm) {

                apiService.create("StdRouteLocationMap/deactivate", data).
                then(function (promise) {
                    if (promise.returnval == true) {
                        swal("Record " + confirmmgs + " " + "successfully");
                    }
                    else {
                        swal("Record " + mgs + " Failed");
                    }
                    $state.reload();
                    //$scope.clear();
                    //$scope.BindData();
                })

            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
        });
        }

    



        $scope.searchByColumn = function (search, searchColumn) {
            if (search != "" && search != null && search != undefined) {
                var data = {
                    "EnteredData": search,
                    "SearchColumn": searchColumn,
                }
                apiService.create("StdRouteLocationMap/SearchByColumn", data)
          .then(function (promise) {
              if (promise.reportdatelist.length == 0) {
                  swal("No Records Found");
                  $state.reload();
              }
              else {
                  var date_td = new Date();
                  angular.forEach(promise.reportdatelist, function (we) {
                      if (new Date(we.trsR_Date) > date_td) {
                          we.opt_flag = true;
                      }
                      we.trsR_Date = $filter('date')(new Date(we.trsR_Date), 'dd-MM-yyyy');
                  })
                  $scope.students_list = promise.reportdatelist;
                  // $scope.students_list = promise.reportdatelist;
                  $scope.search = "";
              }
          })
            }
            else {
                swal("Please Enter Value To Be Searched In Search here.....Text Box  And Then Click On Search Icon");
            }
        }
        $scope.savedinstallment = [];
        $scope.paidinstallment = [];
        $scope.get_loca_sches_p12 = function (stu) {
           
            stu.installmentlist = [];
            var data = {
                "TRML_Id": stu.TRSR_PickUpLocation,
                "AMST_Id": stu.amsT_Id,
                "ASMAY_Id": stu.asmaY_Id,
                "ASMCL_Id": stu.asmcL_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("StdRouteLocationMap/get_loca_sches1", data).
                then(function (promise) {
                    
                    $scope.check_fee = promise.check_fee;
                    if (promise.check_fee == null || $scope.check_fee.length == 0) {
                        swal("Selected Location Not Mapped With Fee Group");
                        stu.TRSR_PickUpLocation = "";
                    }
                    else {
                        stu.installmentlist = promise.installmentlist;
                        $scope.savedinstallment = promise.installmentlist_saved;
                        $scope.paidinstallment = promise.installmentlist_Paid;
                        angular.forEach(stu.installmentlist, function (nn) {

                            angular.forEach($scope.savedinstallment, function(vv) {
                                debugger;
                                if (nn.FTI_Id == vv.FTI_ID) {
                                    nn.checkedgrplst1 = true;
                                   
                                }
                               
                            })

                            angular.forEach($scope.paidinstallment, function (gg) {
                                debugger;
                                if (nn.FTI_Id == gg.FTI_ID) {
                                    nn.dis = true;
                                }

                            })

                            

                            //nn.checkedgrplst1 = true;

                        })

                    }      
                })
        };

        $scope.get_loca_sches_p1 = function (stu) {
            debugger;
            stu.installmentlist = [];
            var data = {
                "TRML_Id": stu.TRSR_PickUpLocation,
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.clsdrp,
                "AMST_Id": stu.amsT_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("StdRouteLocationMap/get_loca_sches1", data).
                then(function (promise) {

                    $scope.check_fee = promise.check_fee;
                    if (promise.check_fee == null || $scope.check_fee.length == 0) {
                        swal("Selected Location Not Mapped With Fee Group");
                        stu.TRSR_PickUpLocation = "";
                    }
                    else {
                        stu.installmentlist = promise.installmentlist;

                        //angular.forEach(stu.installmentlist, function (nn) {
                        //    nn.checkedgrplst1 = true;
                        //})

                        $scope.savedinstallment = promise.installmentlist_saved;
                        $scope.paidinstallment = promise.installmentlist_Paid;
                        angular.forEach(stu.installmentlist, function (nn) {
                            angular.forEach($scope.savedinstallment, function (vv) {
                                debugger;
                                if (nn.FTI_Id == vv.FTI_ID) {
                                    nn.checkedgrplst1 = true;

                                }

                            })

                            angular.forEach($scope.paidinstallment, function (gg) {
                                debugger;
                                if (nn.FTI_Id == gg.FTI_ID) {
                                    nn.dis = true;
                                }

                            })


                        })
                    }
                })
        };

        $scope.get_loca_sches_p = function (stu) {
            var data = {
                "TRMR_Id": stu.TRMR_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            //apiService.create("StdRouteLocationMap/get_loca_sches", stu).
            apiService.create("StdRouteLocationMap/get_loca_sches", data).
           then(function (promise) {

               stu.location_list_p = promise.locationlist;
               stu.schedule_list_p = promise.schedulelist;
               if ($scope.alrdy_stu_list != null && $scope.alrdy_stu_list != "" && $scope.alrdy_stu_list.length > 0) {
                   if (stu.TRSR_PickupSchedule == 0) {
                       stu.TRSR_PickupSchedule = "";
                   }
                   if (stu.TRSR_PickUpLocation == 0) {
                       stu.TRSR_PickUpLocation = "";
                   }
               
               }
               else {
                   stu.TRSR_PickupSchedule = "";
                   stu.TRSR_PickUpLocation = "";
               }
               if (promise.locationlist == null || promise.schedulelist == null || promise.locationlist.length == 0 || promise.schedulelist.length == 0) {
                   swal("Selected Route Not Mapped With Location/Schedules");
                   stu.TRMR_Id = "";
               }
           })
        };
        $scope.get_loca_sches_d = function (stu) {

            var data = {
                "TRMR_Id": stu.TRMR_Drop_Route,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            //apiService.create("StdRouteLocationMap/get_loca_sches", stu).
            apiService.create("StdRouteLocationMap/get_loca_sches", data).
           then(function (promise) {

               stu.location_list_d = promise.locationlist;
               stu.schedule_list_d = promise.schedulelist;
               if ($scope.alrdy_stu_list != null && $scope.alrdy_stu_list != "" && $scope.alrdy_stu_list.length > 0) {
                   //if (stu.TRSR_PickupSchedule == 0) {
                   //    stu.TRSR_PickupSchedule = "";
                   //}
                   //if (stu.TRSR_PickUpLocation == 0) {
                   //    stu.TRSR_PickUpLocation = "";
                   //}
                   if (stu.TRSR_DropSchedule == 0) {
                       stu.TRSR_DropSchedule = "";
                   }
                   if (stu.TRSR_DropLocation == 0) {
                       stu.TRSR_DropLocation = "";
                   }
               }
               else {
                   stu.TRSR_DropSchedule = "";
                   stu.TRSR_DropLocation = "";
               }
               if (promise.locationlist == null || promise.schedulelist == null || promise.locationlist.length == 0 || promise.schedulelist.length == 0) {
                   swal("Selected Route Not Mapped With Location/Schedules");
                   stu.TRMR_Drop_Route = "";
               }
           })
        };
        $scope.select_one = function (grps_lst, grp) {
            angular.forEach(grps_lst, function (ft) {
                if (ft.fmG_Id != grp.fmG_Id) {
                    ft.checkedvalue_g = false;
                }
            })
        }
        $scope.checkduplicateno = function () {
            if ($scope.TRSR_ApplicationNo != "")
            {
                var count = 0;
                angular.forEach($scope.Selected_stu_grps_list, function (stu) {
                    if(stu.TRSR_ApplicationNo==$scope.TRSR_ApplicationNo)
                    {
                        count += 1;                        
                    }
                })
                if (count == 0)
                {
                    var data = {
                        "ASMAY_Id":$scope.asmaY_Id,
                        "AMST_Id":$scope.amsT_Id,
                        "TRSR_ApplicationNo": $scope.TRSR_ApplicationNo,
                    }
                    apiService.create("StdRouteLocationMap/checkduplicateno", data).then(function (promise) {
                        if (promise != null) {
                            if (promise.duplicateno == "Duplicate") {
                                swal("Application Number Already Exists");
                                $scope.TRSR_ApplicationNo = "";
                            }
                            //else {
                            //    $scope.TRSR_ApplicationNo = $scope.TRSR_ApplicationNo;
                            //}
                        }
                    })
                }
                else if(count>0)
                {
                    swal("Application Number Already Exists");
                    $scope.TRSR_ApplicationNo = "";
                }

          
            }
        }

    }
})();

