(function () {
    'use strict';
    angular.module('app').controller('CLGStudentRouteMappingbuspassController', CLGStudentRouteMappingbuspassController)

    CLGStudentRouteMappingbuspassController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter']
    function CLGStudentRouteMappingbuspassController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        if (paginationformasters == 0 || paginationformasters == undefined || paginationformasters == null) {
            paginationformasters = 20;
        }
        $scope.obj = {};

        $scope.editbtn = false;

        //  paginationformasters = 10;
        $scope.sort1 = function (keyname) {
            $scope.sortKey1 = keyname;   //set the sortKey to the param passed
            $scope.reverse1 = !$scope.reverse1; //if true make it false and vice versa
        }
        var temp_grp_list = [];
        //  $scope.TRSRCO_Date = new Date();
        $scope.radioval = '';

        $scope.get_course = function () {
            $scope.AMB_Id = '';
            $scope.AMCO_Id = '';
            $scope.AMSE_Id = '';
            $scope.ACMS_Id = '';
            $scope.semisterlist = [];
            $scope.branchlist = [];
            if ($scope.ASMAY_Id == "" || $scope.ASMAY_Id == undefined) {
                swal("Please Select The Academic Year !");
            }
            else {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                }
                apiService.create("CLGTRNCommon/get_course", data).then(function (promise) {

                    $scope.courselist = promise.courselist;

                    if (promise.courselist == "" || promise.courselist == null) {
                        swal("No Course/Branch Are Mapped To Selected Academic Year");
                    }
                });
            }
        };

        $scope.getbranch_catg = function () {
            $scope.AMB_Id = '';
            $scope.AMSE_Id = '';
            $scope.ACMS_Id = '';
            $scope.semisterlist = [];
            $scope.branchlist = [];
            $scope.semisterlist = [];
            var data = {
                "AMCO_Id": $scope.AMCO_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
            }
            apiService.create("CLGTRNCommon/getbranch", data).then(function (promise) {
                $scope.branchlist = promise.branchlist;
                if (promise.branchlist == "" || promise.branchlist == null) {
                    swal("No Branch Are Mapped To Selected Course");
                }
            });
        };

        $scope.get_semister = function () {
            $scope.ACMS_Id = '';
            var data = {
                "AMCO_Id": $scope.AMCO_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMB_Id": $scope.AMB_Id,
            }
            apiService.create("CLGTRNCommon/get_semister", data).then(function (promise) {
                $scope.semisterlist = promise.semisterlist;
                if (promise.semisterlist == "" || promise.semisterlist == null) {
                    swal("No Semester Are Mapped To Selected Course/Branch");
                }
            });
        };

        $scope.get_section = function () {
            var data = {
                "AMCO_Id": $scope.AMCO_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id,
            }
            apiService.create("CLGTRNCommon/get_section", data).then(function (promise) {
                $scope.section_list = promise.sectionlist;
                if (promise.section_list == "" || promise.section_list == null) {
                    swal("No Sections Are Mapped To Selected Course/Branch");
                }
            });
        };

        $scope.picloclist = [];

        $scope.on_pic_route_change = function () {
            $scope.picloclist = [];
            var data = {
                "TRMR_Id": $scope.trmR_Id_pic,
                //  "Adm_no_name": $scope.radio_button,
            }
            apiService.create("CLGTRNCommon/get_location", data).then(function (promise) {
                $scope.picloclist = promise.locationlist;
            });
        };

        $scope.droploclist = [];
        $scope.on_drp_route_change = function () {
            $scope.droploclist = [];
            var data = {
                "TRMR_Id": $scope.trmR_Id_drp,
                //  "Adm_no_name": $scope.radio_button,
            }
            apiService.create("CLGTRNCommon/get_location", data).then(function (promise) {
                $scope.droploclist = promise.locationlist;
            });
        };

        $scope.changeradio = function () {
            $scope.ASMAY_Id = '';
            $scope.clsdrp = '';
            $scope.sectiondrp = '';
            $scope.students = [];
        }

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("CLGStudentRouteMapping/getdata", pageid).then(function (promise) {

                $scope.yearlist = promise.yealist;
                $scope.sectionlist = promise.sectionlist;

                $scope.route_list = promise.routelist;
                $scope.location_list = promise.locationlist;
                $scope.schedule_list = promise.schedulelist;
                //  $scope.group_list = promise.grouplist;
                temp_grp_list = promise.grouplist;
                $scope.sesslist_list_p = promise.picsesslist;
                $scope.sesslist_list_d = promise.drpsesslist;

                $scope.drpsesslist = promise.drpsesslist;
                $scope.picsesslist = promise.picsesslist;

                $scope.pick_route_new = promise.routelist;
                $scope.drop_route_new = promise.routelist;
                // $scope.grid_list = promise.reportdatelist;

                $scope.students_list = promise.reportdatelist;
                $scope.monthdropdown = promise.monthdropdown;
                $scope.buspasslist = promise.buspasslist;
                $scope.buspasslists = promise.buspasslist.length;
                
            });
        };

        //var maxDate = Math.max.apply(Math, $scope.students_list);
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.radioval = 'class_wise';
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //$scope.clear_fee_challan = function () {
        //    $state.reload();
        //    $scope.loaddata();
        //}

        $scope.get_cls_secs = function () {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                //  "Adm_no_name": $scope.radio_button,
            };

            apiService.create("StudentRouteMapping/get_cls_secs", data).then(function (promise) {
                $scope.class_list = promise.classlist;
                $scope.section_list = promise.sectionlist;
                $scope.clsdrp = "";
                $scope.sectiondrp = "";
                var iddata = $scope.ASMAY_Id;
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
                    // $scope.TRSRCO_Date = new Date();


                }
                $scope.Selected_stu_grps_list = [];
                $scope.students = [];
            });
        };

        $scope.Clearid = function () {
            $state.reload();
            $scope.loaddata();
        };

        $scope.toggleAll1 = function () {
            var toggleStatus = $scope.all1;
            angular.forEach($scope.students, function (itm) {
                itm.checkedvalue1 = toggleStatus;
            });
        };

        $scope.optionToggled1 = function (user) {
            $scope.all1 = $scope.students.every(function (itm) { return itm.checkedvalue1; })
        };

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.students, function (itm) {
                itm.checkedvalue = toggleStatus;
            });
        };

        $scope.optionToggled = function (user) {
            $scope.all = $scope.students.every(function (itm) { return itm.checkedvalue; })
        }

        $scope.students1 = [];
        $scope.savedata = function (pagesrecord) {
            debugger;
            var somedata = [];
            $scope.students1 = [];
            if ($scope.myForm.$valid) {
                var count_g = 0;
                //angular.forEach(pagesrecord, function (re) {
                //    if (re.checkedvalue //&& !re.grp_flag) {
                //        count_g += 1;
                //    }
                //});

                if (count_g == 0) {
                    angular.forEach(pagesrecord, function (student) {
                        if (student.checkedvalue == true) {
                            $scope.students1.push(student);
                        }
                    });
                    console.log($scope.students1);

                    var sch_cnt = 0;
                    if ($scope.students1.length === 0) {
                        swal('Please Select altleast one Record')
                    }
                    else if (sch_cnt > 0) {
                        swal('Pickup Schedule and Drop Schedule Should not be same')
                    }
                    else {
                        angular.forEach($scope.students1, function (student) {
                            angular.forEach($scope.Selected_stu_grps_list, function (student1) {
                                if (student.amcsT_Id == student1.amcsT_Id) {
                                    if (student1.TRRSCO_ApplicationNo == "" || student1.TRRSCO_ApplicationNo == undefined || student1.TRRSCO_ApplicationNo == null) {
                                        student1.TRRSCO_ApplicationNo = 0;
                                    }
                                    somedata.push(student1);
                                }
                            });
                        });

                        $scope.TRSRCO_Date = new Date($scope.TRSRCO_Date).toDateString();
                        angular.forEach($scope.students1, function (ore) {

                            if (ore.TRSR_PickupSchedule == "") {
                                ore.TRSR_PickupSchedule = 0;
                            }
                            if (ore.TRRSCO_PickUpLocation == "") {
                                ore.TRRSCO_PickUpLocation = 0;
                            }
                            if (ore.TRSR_DropSchedule == "") {
                                ore.TRSR_DropSchedule = 0;
                            }
                            if (ore.TRRSCO_DropLocation == "") {
                                ore.TRRSCO_DropLocation = 0;
                            }

                            if (ore.PickUp_Session == "" || ore.PickUp_Session == undefined || ore.PickUp_Session == null) {
                                ore.PickUp_Session = 0;
                            }
                            if (ore.Drop_Session == "" || ore.Drop_Session == undefined || ore.Drop_Session == null) {
                                ore.Drop_Session = 0;
                            }
                            if (ore.trsrcO_PickUpRoute == "" || ore.trsrcO_PickUpRoute == undefined || ore.trsrcO_PickUpRoute == null) {
                                ore.trsrcO_PickUpRoute = 0;
                            }
                            if (ore.TRSRCO_DropRoute == "" || ore.TRSRCO_DropRoute == undefined || ore.TRSRCO_DropRoute == null) {
                                ore.TRSRCO_DropRoute = 0;
                            }

                            if (ore.TRRSCO_ApplicationNo == "" || ore.TRRSCO_ApplicationNo == undefined || ore.TRRSCO_ApplicationNo == null) {
                                ore.TRRSCO_ApplicationNo = 0;
                            }
                        })

                        var fromdate1 = $scope.TRSRCO_Date == null ? "" : $filter('date')($scope.TRSRCO_Date, "yyyy-MM-dd");
                        var data = {
                            savetmpdata: $scope.students1,
                            "TRSRCO_Date": fromdate1,
                            some_data: somedata,
                            "ASMAY_Id": Number($scope.ASMAY_Id),
                            "studenttype": $scope.radioval,
                        };

                        apiService.create("CLGStudentRouteMapping/savedata", data).then(function (promise) {
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
                        });
                    }
                }
                else {
                    swal("Map Selected Students With Groups");
                }
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.savedata_route = function (pagesrecord) {
            debugger;
            $scope.students1 = [];
            var somedata = [];
            if ($scope.myForm.$valid) {
                var count_g = 0;
                if ($scope.trmR_Id_pic == undefined || $scope.trmR_Id_pic == '' || $scope.trmR_Id_pic == null) {
                    $scope.trmR_Id_pic = 0;
                }

                if ($scope.trmR_Id_drp == undefined || $scope.trmR_Id_drp == '' || $scope.trmR_Id_drp == null) {
                    $scope.trmR_Id_drp = 0;
                }

                //angular.forEach(pagesrecord, function (re) {
                //    if (re.checkedvalue1 && !re.grp_flag) {
                //        count_g += 1;
                //    }
                //});

                if (count_g == 0) {

                    angular.forEach(pagesrecord, function (student) {
                        if (student.checkedvalue1 == true) {
                            $scope.students1.push(student);
                        }
                    });

                    console.log($scope.students1)
                    var sch_cnt = 0;
                    angular.forEach($scope.students1, function (v) {
                        if (v.TRSR_PickupSchedule == v.TRSR_DropSchedule) {
                            sch_cnt += 1;
                        }
                    });

                    if ($scope.students1.length === 0) {
                        swal('Please Select altleast one Record');
                    }

                    else if ($scope.trmR_Id_pic == 0 && $scope.trmR_Id_drp == 0) {
                        swal('Select PickUp OR Drop Route')
                        $scope.trmR_Id_pic = '';
                        $scope.trmR_Id_drp = '';
                    }
                    else {
                        var somedata = [];
                        angular.forEach($scope.students1, function (student) {
                            angular.forEach($scope.Selected_stu_grps_list, function (student1) {
                                if (student.amcsT_Id == student1.amcsT_Id) {

                                    if (student1.TRRSCO_ApplicationNo == "" || student1.TRRSCO_ApplicationNo == undefined || student1.TRRSCO_ApplicationNo == null) {
                                        student1.TRRSCO_ApplicationNo = 0;
                                    }
                                    somedata.push(student1);
                                }
                            });
                        });

                        $scope.TRSRCO_Date = new Date($scope.TRSRCO_Date).toDateString();
                        angular.forEach($scope.students1, function (ore) {

                            if (ore.TRSR_PickupSchedule == "") {
                                ore.TRSR_PickupSchedule = 0;
                            }
                            if (ore.TRRSCO_PickUpLocation == "") {
                                ore.TRRSCO_PickUpLocation = 0;
                            }
                            if (ore.TRSR_DropSchedule == "") {
                                ore.TRSR_DropSchedule = 0;
                            }
                            if (ore.TRRSCO_DropLocation == "") {
                                ore.TRRSCO_DropLocation = 0;
                            }
                            if (ore.TRMR_Id == "") {
                                ore.TRMR_Id = 0;
                            }
                        })
                        if ($scope.TRRSCO_DropLocation == '') {
                            $scope.TRRSCO_DropLocation = 0;
                        }
                        if ($scope.TRRSCO_PickUpLocation == '') {
                            $scope.TRRSCO_PickUpLocation = 0;
                        }
                        if ($scope.PickUp_Session == '') {
                            $scope.PickUp_Session = 0;
                        }
                        if ($scope.Drop_Session == '') {
                            $scope.Drop_Session = 0;
                        }

                        var fromdate1 = $scope.TRSRCO_Date == null ? "" : $filter('date')($scope.TRSRCO_Date, "yyyy-MM-dd");
                        var data = {

                            savetmpdata: $scope.students1,
                            "TRSRCO_Date": fromdate1,
                            some_data: somedata,
                            "ASMAY_Id": Number($scope.ASMAY_Id),
                            "TRRSCO_DropLocation": $scope.TRRSCO_DropLocation,
                            "TRRSCO_PickUpLocation": $scope.TRRSCO_PickUpLocation,
                            "PickUp_Session": $scope.PickUp_Session,
                            "Drop_Session": $scope.Drop_Session,
                            "TRSRCO_PickUpRoute": $scope.trmR_Id_pic,
                            "TRSRCO_DropRoute": $scope.trmR_Id_drp,
                            "studenttype": $scope.radioval,
                        }

                        apiService.create("CLGStudentRouteMapping/savedata", data).then(function (promise) {

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
                        });
                    }
                }
                else {
                    swal("Map Selected Students With Groups");
                }
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.clear_fee_balance = function () {
            $state.reload();
        };

        $scope.student_falg = false;

        //$scope.Balance_report = true;
        $scope.print_flag = true;
        $scope.submitted = false;

        $scope.ShowReportdatabuspass = function () {
            $scope.students = [];
            $scope.alrdy_stu_list = [];
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "ACMS_Id": $scope.ACMS_Id,
                    "IVRM_Month_Id": $scope.IVRM_Month_Id
                }
                apiService.create("CLGStudentRouteMapping/getreporteditbuspass/", data).then(function (promise) {
                    
                    if (promise.admsudentslist != null && promise.admsudentslist != "") {
                        if (promise.admsudentslist[0].TRMR_RouteName != null && promise.admsudentslist[0].TRMR_RouteName == "Generated") {
                            swal("Bus Pass already generated");
                        }
                        else if (promise.admsudentslist[0].TRMR_RouteName != null && promise.admsudentslist[0].TRMR_RouteName == "NotGenerated") {
                            swal("There is No Student Found");
                        }
                        
                        else {
                            $scope.Balance_report = true;
                            $scope.fee_clear = false;
                            $scope.print_flag = false;
                            $scope.students = promise.admsudentslist;

                            debugger;
                            angular.forEach($scope.students, function (stu) {
                                stu.sesslist_list_d = $scope.sesslist_list_d;
                                stu.sesslist_list_p = $scope.sesslist_list_p;
                                stu.PickUp_Session = '';
                                stu.Drop_Session = '';
                                stu.trsrcO_PickUpRoute = '';
                                //stu.Drop_Session = '';

                            });
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
            else {
                $scope.submitted = true;
            }
        };


        $scope.savedata_buspass = function (pagesrecord) {
            debugger;
            $scope.students1 = [];
            if ($scope.myForm.$valid) {

                angular.forEach(pagesrecord, function (student) {
                    if (student.checkedvalue == true) {
                        student.checkedvalue = 1;
                        $scope.students1.push(
                            {
                                TRSRCO_PickUpRoute: student.trsrcO_PickUpRoutebus,
                                TRRSCO_PickUpLocation: student.trrscO_PickUpLocation,
                                AMCST_Id: student.amcsT_Id,
                                ACSRM_Mapping_Flag: student.checkedvalue 
                            }
                        );
                    }
                });
                console.log($scope.students1)
                if ($scope.students1.length === 0) {
                    swal('Please Select altleast one Record');
                }
                else {
                    var data = {
                        //"AMCST_Id": $scope.AMCST_Id,

                       "ASMAY_Id": $scope.ASMAY_Id,
                        "AMCO_Id": $scope.AMCO_Id,
                        "AMB_Id": $scope.AMB_Id,
                       "AMSE_Id": $scope.AMSE_Id,
                        "ACMS_Id": $scope.ACMS_Id,
                        "ACSRM_Monthid": $scope.IVRM_Month_Id,
                         savetmpdata: $scope.students1,
                      
                    }

                    apiService.create("CLGStudentRouteMapping/savedatabuspass", data).then(function (promise) {

                        if (promise.returnval == true) {
                            //  swal('Record ' + promise.returntxt + ' Successfully', 'success');
                            //$scope.Clearid();
                            swal('Bus Pass generated Successfully ');
                        }
                        else {
                            // swal('Record Not ' + promise.returntxt + ' Successfully', 'Failed');
                            swal('Bus Pass already generated ');

                        }
                        $state.reload();
                        // $scope.loaddata();
                    });
                }


            }
            else {
                $scope.submitted = true;
            }
        };








        $scope.students = [];
        $scope.filterdata = "NameRegNo";

        $scope.get_sections = function () {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.clsdrp
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("StudentRouteMapping/get_sections", data).then(function (promise) {
                $scope.section_list = promise.sectionlist;
                $scope.sectiondrp = "";
                $scope.Selected_stu_grps_list = [];
                $scope.students = [];
            });
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
        };

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

                apiService.create("StudentRouteMapping/searching", data).then(function (promise) {

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
                });
            }
            else {
                swal("Data Is Needed For Search ");
            }
        };

        $scope.clearsearch = function () {
            $scope.search123 = "";
            $scope.search_flag = false;
            $scope.searchtxt = "";
            $scope.searchnumbr = "";
            $scope.searchdat = "";
        };
        //search end

        $scope.clear_dues_stu = function (user) {
            angular.forEach($scope.students, function (se) {
                if (se.amcsT_Id == user.amcsT_Id) {
                    if (Number(se.fmoB_Institution_Due) > 0)
                        se.fmoB_Student_Due = 0;
                    //else if(Number(se.fmoB_Institution_Due)==0)
                    //    se.fmoB_Student_Due = 1;
                }
            })
        }
        $scope.clear_dues_inst = function (user) {
            angular.forEach($scope.students, function (se) {
                if (se.amcsT_Id == user.amcsT_Id) {
                    if (Number(se.fmoB_Institution_Due) > 0)
                        se.fmoB_Institution_Due = 0;
                    //else if (Number(se.fmoB_Institution_Due) == 0)
                    //    se.fmoB_Institution_Due = 1;

                }
            })
        }

        $scope.disableappno = false;
        $scope.TRRSCO_ApplicationNo1 = "";

        $scope.select_grps = function (sel_subexms, user) {
            debugger;
            $scope.group_list = temp_grp_list;
            $scope.Student_Name = user.amcsT_FirstName;
            $scope.amcsT_Id = user.amcsT_Id;
            $scope.astA_Id = user.ASTA_Id;
            //  $scope.TRRSCO_ApplicationNo = $scope.Selected_stu_grps_list[a].TRRSCO_ApplicationNo;
            $scope.trrscO_PickUpMobileNo = user.amsT_MobileNo;
            $scope.trrscO_DropMobileNo = user.amsT_MobileNo;
            var count = 0;
            for (var a = 0; a < $scope.Selected_stu_grps_list.length; a++) {
                if ($scope.Selected_stu_grps_list[a].amcsT_Id == user.amcsT_Id) {
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
                    $scope.TRRSCO_ApplicationNo = $scope.Selected_stu_grps_list[a].TRRSCO_ApplicationNo;
                    $scope.TRRSCO_ApplicationNo1 = $scope.Selected_stu_grps_list[a].TRRSCO_ApplicationNo;
                    $scope.TRRSCO_PickUpMobileNo = $scope.Selected_stu_grps_list[a].TRRSCO_PickUpMobileNo;
                    $scope.TRRSCO_DropMobileNo = $scope.Selected_stu_grps_list[a].TRRSCO_DropMobileNo;

                }
                //if ($scope.TRRSCO_ApplicationNo1 != "") {
                //    $scope.disableappno = true;
                //} else {
                //    $scope.disableappno = false;
                //}
                if ($scope.astA_Id > 0) {
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
                                if ($scope.Selected_stu_grps_list[i].acmsT_Id == user.amcsT_Id) {
                                    $scope.Selected_stu_grps_list.splice(i, 1);
                                }
                            }
                            swal('Deleted Successfully');
                        }
                        else {
                            swal("Now You Can Change Selection");
                            angular.forEach($scope.students, function (itm) {
                                if (itm.amcsT_Id == user.amcsT_Id) {

                                    itm.grp_flag = true;
                                    $scope.select_grps(itm.grp_flag, user);
                                    $('#popup5').modal('show');
                                }
                            })
                        }
                    });
            }
        };

        $scope.exam_check = false;
        $scope.clearpopupgrid5 = function () {

            if ($scope.exam_check == true) {
                angular.forEach($scope.students, function (itm) {
                    if (itm.amcsT_Id == $scope.amcsT_Id) {
                        itm.grp_flag = false;
                    }
                });
            }

            $scope.TRRSCO_ApplicationNo1 = "";
            $('#popup5').modal('hide');
        }
        $scope.clear2 = function () {

            $scope.group_list = temp_grp_list;
            angular.forEach($scope.group_list, function (itm1) {
                itm1.checkedvalue_g = false;
            })
            $scope.TRRSCO_ApplicationNo = "";
            //$scope.TRRSCO_PickUpMobileNo = "";
            //$scope.TRRSCO_DropMobileNo = "";

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
            return $scope.submitted2;
        };
        $scope.Selected_stu_grps_list = [];

        $scope.saveddata2 = function (grps) {

            $scope.submitted2 = true;
            if ($scope.myForm2.$valid) {

                var Selected_grps_list = [];
                angular.forEach(grps, function (itm) {
                    if (itm.checkedvalue_g) {
                        Selected_grps_list.push({ TRML_Id: itm.fmG_Id, TRMA_AreaName: itm.fmG_GroupName });
                    }
                });

                for (var i = 0; i < $scope.Selected_stu_grps_list.length; i++) {
                    var already_count = 0;
                    if ($scope.amcsT_Id == $scope.Selected_stu_grps_list[i].amcsT_Id) {
                        already_count += 1;
                        // $scope.Selected_stu_grps_list.splice(i, 1);
                        if (already_count > 0) {
                            $scope.Selected_stu_grps_list.splice(i, 1);
                        }
                    }
                }

                $scope.Selected_stu_grps_list.push({
                    amcsT_Id: $scope.amcsT_Id, grp_list: Selected_grps_list,
                    TRRSCO_ApplicationNo: $scope.TRRSCO_ApplicationNo,
                    TRRSCO_PickUpMobileNo: $scope.TRRSCO_PickUpMobileNo,
                    TRRSCO_DropMobileNo: $scope.TRRSCO_DropMobileNo
                });
                //}

                angular.forEach($scope.students, function (stu) {
                    if (stu.amcsT_Id === parseInt($scope.amcsT_Id)) {
                        stu.TRRSCO_ApplicationNo = $scope.TRRSCO_ApplicationNo;
                        stu.TRRSCO_PickUpMobileNo = $scope.TRRSCO_PickUpMobileNo;
                        stu.TRRSCO_DropMobileNo = $scope.TRRSCO_DropMobileNo;
                    }
                });

                $('#popup5').modal('hide');
                $scope.group_list = [];
            }
            else {
                $scope.submitted2 = true;
            }
        };
        $scope.students = [];
        $scope.clear1 = function () {
            $scope.ASMAY_Id = "";
            $scope.TRSRCO_Date = "";
            // $scope.TRSRCO_Date = new Date();
            $scope.clsdrp = "";
            $scope.sectiondrp = "";
            $scope.Selected_stu_grps_list = [];
            $scope.students = [];
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        }

        $scope.editvalue = function (userdata) {
            debugger;
            $scope.radioval = 'class_wise';
            $scope.students = [];
            $scope.alrdy_stu_list = [];
            $scope.grpeditlist = [];
            var data = {
                "ASMAY_Id": userdata.asmaY_Id,
                "AMCST_Id": userdata.amcsT_Id,
                "TRSRCO_Id": userdata.trsrcO_Id,
            }
            apiService.create("CLGStudentRouteMapping/getreporteditbuspass/", data).then(function (promise) {
                if (promise.admsudentslist != null && promise.admsudentslist != "") {
                    $scope.editbtn = true;
                    $scope.group_list = temp_grp_list;
                    $scope.Balance_report = true;
                    $scope.fee_clear = false;
                    $scope.print_flag = false;
                    $scope.students = promise.admsudentslist;
                    $scope.grpeditlist = promise.grpeditlist;

                    angular.forEach($scope.students, function (stu) {
                        stu.sesslist_list_d = $scope.sesslist_list_d;
                        stu.sesslist_list_p = $scope.sesslist_list_p;
                        stu.checkedvalue = false;
                    });
                    $scope.ASMAY_Id = $scope.students[0].asmaY_Id
                    $scope.AMCO_Id = $scope.students[0].amcO_Id

                    if ($scope.ASMAY_Id != null && $scope.ASMAY_Id != '' && $scope.ASMAY_Id != undefined) {
                        $scope.get_course();
                        $scope.AMCO_Id = $scope.students[0].amcO_Id
                    }

                    $scope.AMCO_Id = $scope.students[0].amcO_Id

                    if ($scope.ASMAY_Id != null && $scope.ASMAY_Id != '' && $scope.ASMAY_Id != undefined && $scope.AMCO_Id != null && $scope.AMCO_Id != '' && $scope.AMCO_Id != undefined) {
                        $scope.getbranch_catg();
                        $scope.AMB_Id = $scope.students[0].amB_Id
                    }

                    if ($scope.ASMAY_Id != null && $scope.ASMAY_Id != '' && $scope.ASMAY_Id != undefined && $scope.AMCO_Id != null && $scope.AMCO_Id != '' && $scope.AMCO_Id != undefined && $scope.AMB_Id != null && $scope.AMB_Id != '' && $scope.AMB_Id != undefined) {
                        $scope.get_semister();
                        $scope.AMSE_Id = $scope.students[0].amsE_Id
                    }

                    $scope.ACMS_Id = $scope.students[0].acmS_Id

                    $scope.alrdy_stu_list = promise.alrdy_stu_list;
                    if (promise.alrdy_stu_list != null && promise.alrdy_stu_list != "" && promise.alrdy_stu_list.length > 0) {
                        $scope.TRSRCO_Date = new Date(promise.alrdy_stu_list[0].trsrcO_Date);

                        //$scope.TRSRCO_Date = promise.alrdy_stu_list[0].trsrcO_Date;
                        $scope.Selected_stu_grps_list = [];
                        angular.forEach($scope.students, function (stu1) {
                            var Selected_grps_list = [];
                            angular.forEach(promise.alrdy_stu_list, function (stu2) {
                                if (stu2.amcsT_Id == stu1.amcsT_Id /*&& stu2.asmaY_Id == $scope.asmaY_Id*/) {
                                    stu1.checkedvalue = true;
                                    $scope.optionToggled(stu1);
                                    stu1.trsrcO_PickUpRoute = stu2.trsrcO_PickUpRoute;
                                    // stu1.TRSR_PickupSchedule = stu2.trsR_PickupSchedule;
                                    stu1.TRRSCO_PickUpLocation = stu2.trrscO_PickUpLocation;
                                    stu1.TRSRCO_DropRoute = stu2.trsrcO_DropRoute;
                                    //stu1.TRSR_DropSchedule = stu2.trsR_DropSchedule;
                                    stu1.TRRSCO_DropLocation = stu2.trrscO_DropLocation;
                                    stu1.Drop_Session = stu2.trsrcO_DropSession;
                                    stu1.PickUp_Session = stu2.trsrcO_PickupSession;
                                    stu1.ASTA_Id = stu2.astA_Id;

                                    if (stu1.trsrcO_PickUpRoute == 0) {
                                        stu1.trsrcO_PickUpRoute = '';
                                    }
                                    if (stu1.TRRSCO_PickUpLocation == 0) {
                                        stu1.TRRSCO_PickUpLocation = '';
                                    }
                                    if (stu1.PickUp_Session == 0) {
                                        stu1.PickUp_Session = '';
                                    }
                                    if (stu1.TRSRCO_DropRoute == 0) {
                                        stu1.TRSRCO_DropRoute = '';
                                    }
                                    if (stu1.TRRSCO_DropLocation == 0) {
                                        stu1.TRRSCO_DropLocation = '';
                                    }
                                    if (stu1.Drop_Session == 0) {
                                        stu1.Drop_Session = '';
                                    }

                                    //var Selected_grps_list = [];
                                    angular.forEach($scope.grpeditlist, function (jj) {
                                        angular.forEach($scope.group_list, function (itm) {
                                            if (itm.fmG_Id == jj.fmG_Id) {
                                                Selected_grps_list.push({ TRML_Id: itm.fmG_Id, TRMA_AreaName: itm.fmG_GroupName });
                                            }
                                        });
                                    });

                                    for (var i = 0; i < $scope.Selected_stu_grps_list.length; i++) {
                                        var already_count = 0;
                                        if (stu2.amcsT_Id == $scope.Selected_stu_grps_list[i].amcsT_Id) {
                                            already_count += 1;
                                            // $scope.Selected_stu_grps_list.splice(i, 1);
                                            if (already_count > 0) {
                                                $scope.Selected_stu_grps_list.splice(i, 1);
                                            }
                                        }
                                    }

                                    if (Selected_grps_list !== null && Selected_grps_list.length > 0) {
                                        stu1.grp_flag = true;
                                    }

                                    $scope.Selected_stu_grps_list.push({
                                        amcsT_Id: stu2.amcsT_Id, grp_list: Selected_grps_list,
                                        TRRSCO_ApplicationNo: stu2.trrscO_ApplicationNo,
                                        TRRSCO_PickUpMobileNo: stu2.trrscO_PickUpMobileNo,
                                        TRRSCO_DropMobileNo: stu2.trrscO_DropMobileNo
                                    });

                                    if (stu1.trsrcO_PickUpRoute != undefined && stu1.trsrcO_PickUpRoute != ''
                                        && stu1.trsrcO_PickUpRoute != null && stu1.trsrcO_PickUpRoute != 0) {
                                        $scope.get_loca_sches_p(stu1);
                                    }
                                    if (stu1.TRSRCO_DropRoute != undefined && stu1.TRSRCO_DropRoute != '' && stu1.TRSRCO_DropRoute != null
                                        && stu1.TRSRCO_DropRoute != 0) {
                                        $scope.get_loca_sches_d(stu1);
                                    }
                                }
                            });
                        });
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

        $scope.deactive = function (employee) {

            var mgs = "";
            var confirmmgs = "";

            var data = {
                "TRSRCO_Id": employee.trsrcO_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.trrscO_ActiveFlg === true) {
                //  mgs = "Deactive";
                mgs = "Deactivate";
                confirmmgs = "De-activated";
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

                        apiService.create("CLGStudentRouteMapping/deactivate", data).
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

        $scope.viewrecordspopup = function (user) {
            $scope.reportdatelist1 = [];
            var data = {
                "TRSRCO_Id": user.trsrcO_Id
            }
            apiService.create("CLGStudentRouteMapping/viewrecordspopup", data).
                then(function (promise) {
                    $scope.reportdatelist1 = promise.reportdatelist1;
                    $scope.pickUp_ScheduleName = $scope.reportdatelist1[0].pickup_SessionName;
                    $scope.pickUp_LocationName = $scope.reportdatelist1[0].PickUp_LocationName;
                    $scope.trrscO_PickUpMobileNo = $scope.reportdatelist1[0].TRRSCO_PickUpMobileNo;
                    $scope.drop_ScheduleName = $scope.reportdatelist1[0].drop_SessionName;
                    $scope.drop_LocationName = $scope.reportdatelist1[0].Drop_LocationName;
                    $scope.trrscO_DropMobileNo = $scope.reportdatelist1[0].TRRSCO_DropMobileNo;
                    $scope.amsT_FirstName = $scope.reportdatelist1[0].amcsT_FirstName;

                });
            //$scope.viewobj = user;
        }

        $scope.searchColumn1 = '';
        $scope.searchfield = '';
        $scope.search123 = '';

        $scope.searchByclear = function () {
            $state.reload();
        }

        $scope.searchByColumn = function () {
            if ($scope.searchColumn1 == '1') {
                $scope.searchfield = $scope.search1;
            }
            else {
                $scope.searchfield = $scope.search123;
            }
            if ($scope.searchfield != "" && $scope.searchfield != null && $scope.searchfield != undefined) {
                var data = {
                    "EnteredData": $scope.searchfield,
                    "SearchColumn": $scope.searchColumn1,
                }
                apiService.create("CLGStudentRouteMapping/SearchByColumn", data).then(function (promise) {
                    if (promise.reportdatelist.length == 0) {
                        swal("No Records Found");
                        $state.reload();
                    }
                    else {
                        var date_td = new Date();
                        angular.forEach(promise.reportdatelist, function (we) {
                            if (new Date(we.trsrcO_Date) > date_td) {
                                we.opt_flag = true;
                            }
                            we.trsrcO_Date = $filter('date')(new Date(we.trsrcO_Date), 'dd-MM-yyyy');
                        })
                        $scope.students_list = promise.reportdatelist;
                        // $scope.students_list = promise.reportdatelist;
                        $scope.search = "";
                    }
                });
            }
            else {
                swal("Please Enter Value To Be Searched In Search here.....Text Box  And Then Click On Search Icon");
            }
        }

        $scope.get_loca_sches_p = function (stu) {
            debugger;
            var data = {
                "TRMR_Id": stu.trsrcO_PickUpRoute,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("CLGTRNCommon/get_location", data).
                then(function (promise) {

                    stu.location_list_p = promise.locationlist;
                    // stu.schedule_list_p = promise.schedulelist;
                    if ($scope.alrdy_stu_list != null && $scope.alrdy_stu_list != "" && $scope.alrdy_stu_list.length > 0) {
                        if (stu.TRSR_PickupSchedule == 0) {
                            stu.TRSR_PickupSchedule = "";
                        }
                        if (stu.TRRSCO_PickUpLocation == 0) {
                            stu.TRRSCO_PickUpLocation = "";
                        }
                        //if (stu.TRSR_DropSchedule == 0) {
                        //    stu.TRSR_DropSchedule = "";
                        //}
                        //if (stu.TRRSCO_DropLocation == 0) {
                        //    stu.TRRSCO_DropLocation = "";
                        //}
                    }
                    else {
                        stu.TRSR_PickupSchedule = "";
                        stu.TRRSCO_PickUpLocation = "";
                    }
                    if (promise.locationlist == null || promise.locationlist.length == 0) {

                        stu.TRMR_Id = "";
                    }
                    if ((promise.locationlist == null || promise.locationlist.length == 0) && stu.TRMR_Id != "") {
                        //swal("Selected Route Not Mapped With Location/Schedules");
                        stu.TRMR_Id = "";
                    }
                })
        };
        $scope.get_loca_sches_d = function (stu) {

            var data = {
                "TRMR_Id": stu.TRSRCO_DropRoute,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            //apiService.create("StudentRouteMapping/get_loca_sches", stu).
            apiService.create("CLGTRNCommon/get_location", data).
                then(function (promise) {

                    stu.location_list_d = promise.locationlist;
                    //  stu.schedule_list_d = promise.schedulelist;
                    if ($scope.alrdy_stu_list != null && $scope.alrdy_stu_list != "" && $scope.alrdy_stu_list.length > 0) {
                        //if (stu.TRSR_PickupSchedule == 0) {
                        //    stu.TRSR_PickupSchedule = "";
                        //}
                        //if (stu.TRRSCO_PickUpLocation == 0) {
                        //    stu.TRRSCO_PickUpLocation = "";
                        //}
                        if (stu.TRSR_DropSchedule == 0) {
                            stu.TRSR_DropSchedule = "";
                        }
                        if (stu.TRRSCO_DropLocation == 0) {
                            stu.TRRSCO_DropLocation = "";
                        }
                    }
                    else {
                        stu.TRSR_DropSchedule = "";
                        stu.TRRSCO_DropLocation = "";
                    }

                    if (promise.locationlist == null || promise.locationlist.length == 0) {
                        stu.TRSRCO_DropRoute = "";
                    }

                    if ((promise.locationlist == null || promise.locationlist.length == 0) && stu.TRSRCO_DropRoute != "") {
                        //swal("Selected Route Not Mapped With Location/Schedules");
                        stu.TRSRCO_DropRoute = "";
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
            if ($scope.TRRSCO_ApplicationNo != "") {
                var count = 0;
                angular.forEach($scope.Selected_stu_grps_list, function (stu) {
                    if (stu.TRRSCO_ApplicationNo == $scope.TRRSCO_ApplicationNo) {
                        count += 1;
                    }
                })
                if (count == 0) {
                    var data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "AMCST_Id": $scope.amcsT_Id,
                        "TRRSCO_ApplicationNo": $scope.TRRSCO_ApplicationNo,
                    }
                    apiService.create("CLGStudentRouteMapping/checkduplicateno", data).then(function (promise) {
                        if (promise != null) {
                            if (promise.duplicateno == "Duplicate") {
                                swal("Application Number Already Exists");
                                $scope.TRRSCO_ApplicationNo = "";
                            }
                            //else {
                            //    $scope.TRRSCO_ApplicationNo = $scope.TRRSCO_ApplicationNo;
                            //}
                        }
                    })
                }
                else if (count > 0) {
                    swal("Application Number Already Exists");
                    $scope.TRRSCO_ApplicationNo = "";
                }


            }
        }
        $scope.searchValue = '';
        $scope.searchValues = '';

        $scope.viewbuspasspopup = function (user) {
            $scope.studentdata = [];
            $scope.studentdata.push(user);
            $scope.StudentName = $scope.studentdata[0].StudentName;
            $scope.RegNo = $scope.studentdata[0].RegNo;
            $scope.Department = $scope.studentdata[0].Department;
            $scope.Month = $scope.studentdata[0].Month;
            $scope.Location = $scope.studentdata[0].Location;
            $scope.Photo = $scope.studentdata[0].Photo;
            $scope.MI_Logo = $scope.studentdata[0].MI_Logo;
            $scope.Route_Nmae = $scope.studentdata[0].Route_Nmae;
            $scope.acd = $scope.studentdata[0].acd;
            
        }  
    }
})();

