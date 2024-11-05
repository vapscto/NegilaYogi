(function () {
    'use strict';
    angular
        .module('app')
        .controller('MasterHolidayController', MasterHolidayController)

    MasterHolidayController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', 'superCache', '$filter']
    function MasterHolidayController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, superCache, $filter) {
        $scope.obj = {};
        //  $scope.group_check = '0';
        $scope.loaddata = function () {

            $scope.Showdepartment = false;
            $scope.all_check();
            $scope.page1 = "page1";
            $scope.page2 = "page2";
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = 10;

            var pageid = 1;
            $scope.printstudents = [];
            apiService.getURI("MasterHoliday/", pageid).
                then(function (promise) {

                    $scope.yearlist = promise.yeardropdown;
                    $scope.holidayType = promise.holidayType;
                    $scope.departmentType = promise.departmentType;
                    console.log($scope.departmentType);
                    $scope.days_list = promise.dayslist;
                    if (promise.report_list != null && promise.report_list != "") {

                        angular.forEach(promise.report_list, function (itm) {
                            itm.fomhwdD_FromDate = $filter('date')(new Date(itm.fomhwdD_FromDate), 'dd-MM-yyyy');
                            itm.fomhwdD_ToDate = $filter('date')(new Date(itm.fomhwdD_ToDate), 'dd-MM-yyyy');
                        })

                        $scope.gridviewDetails = promise.report_list;
                        $scope.rpt = true;
                    }
                    else {
                        $scope.rpt = false;
                    }

                });
            function ShowHide() {
                apiService.getURI("MasterHoliday/", pageid).
                    then(function (promise) {
                        lblShowHide.style.visibility = 'visible';
                    })

            }

        }

        $scope.search = '';
        $scope.filterValue = function (obj) {

            return angular.lowercase(obj.FOMHWDD_Name).indexOf(angular.lowercase($scope.search)) >= 0 ||

                ($filter('date')(obj.FOMHWDD_FromDate, 'dd/MM/yyyy').indexOf($scope.search) >= 0) ||
                ($filter('date')(obj.FOMHWDD_ToDate, 'dd/MM/yyyy').indexOf($scope.search) >= 0);
        }
        $scope.getYear = function () {
            $scope.showgrid = false;


            // For Academic From Date
            $scope.year = new Date($scope.asmaY_Id).getFullYear();
            $scope.month = 0;
            $scope.day = 0;


            $scope.minDatef = new Date(
                $scope.year,
                $scope.month
            );

            $scope.maxDatef = new Date(
                $scope.year,
                $scope.month + 12,
                $scope.day);



            if ($scope.days_list != "" && $scope.days_list != null) {
                if ($scope.days_list.length > 0) {
                    $scope.togchkbx();
                }
            }
        }

        $scope.searchByColumn = function (search, searchColumn) {

            if (search != "" && search != null && search != undefined) {

                var data = {
                    "EnteredData": search,
                    "SearchColumn": searchColumn
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("MasterHoliday/SearchByColumn", data)

                    .then(function (promise) {

                        if (promise.count == 0) {
                            swal("No Records Found");
                            $state.reload();
                        }
                    })
            }
            else {
                swal("Please Enter Value To Be Searched In Search here.....Text Box  And Then Click On Search Icon");
            }
        }


        $scope.Deletedata = function (del_id) {
            var data = {
                "FOMHWDD_Id": del_id.FOMHWDD_Id,

            }
            swal({
                title: "Are you sure?",
                text: "Do you want to Delete This Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: true,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {


                        apiService.create("MasterHoliday/delete/", data).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record Deleted Successfully");
                                    $state.reload();
                                }
                                else {
                                    swal("Failed to Delete, please contact administrator.");
                                }

                            });
                    }
                    else {
                        swal("Cancelled");
                    }
                });
        }




        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        $scope.sort1 = function (keyname) {
            $scope.sortKey1 = keyname;   //set the sortKey to the param passed
            $scope.reverse1 = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.Refrencecheckboxchcked = [];
        $scope.day_datecheckboxchcked = [];
        $scope.submitted = false;
        angular.forEach($scope.days_list, function (itm) {
            if (itm.dayy)
                $scope.Refrencecheckboxchcked.push(itm);
        });


        angular.forEach($scope.datesanddayList, function (itm) {
            if (itm.all == true)
                $scope.day_datecheckboxchcked.push(itm);
        });


        $scope.selectedDaysDate = [];
        $scope.savemasterHolidaydata = function () {

            if ($scope.myForm.$valid) {


                for (var i = 0; i < $scope.yearlist.length; i++) {
                    if ($scope.asmaY_Id == $scope.yearlist[i].hrmlY_Id) {
                        $scope.yearId = $scope.yearlist[i].hrmlY_Id;
                    }
                }
                //for (var i = 0; i < $scope.departmentType.length; i++) {
                //    if ($scope.obj.hrmD_Id == $scope.departmentType[i].hrmD_Id) {
                //        $scope.depId = $scope.departmentType[i].hrmD_Id;
                //    }
                //}

                if ($scope.obj.hrmD_Id != null && $scope.obj.hrmD_Id != undefined && $scope.obj.hrmD_Id > 0) {
                    $scope.depId = $scope.obj.hrmD_Id;
                }
                else {
                    $scope.depId = 0;
                }


                $scope.selectedDaysDate.length = 0;
                if ($scope.dayselection == 'notrequired' && $scope.datesanddayList != "" && $scope.datesanddayList != null) {
                    if ($scope.datesanddayList.length > 0) {
                        for (var i = 0; i < $scope.datesanddayList.length; i++) {
                            if ($scope.datesanddayList[i].all == true) {
                                $scope.selectedDaysDate.push($scope.datesanddayList[i]);
                            }
                        }
                    }

                }
                else if ($scope.dayselection == 'notrequired') {

                }

                else if ($scope.dayselection == 'required' && $scope.datesanddayList != "" && $scope.datesanddayList != null) {
                    if ($scope.datesanddayList.length > 0) {
                        for (var i = 0; i < $scope.datesanddayList.length; i++) {
                            if ($scope.datesanddayList[i].all == true) {
                                $scope.selectedDaysDate.push($scope.datesanddayList[i]);
                            }
                        }
                    }
                }




                $scope.dayflag = false;
                $scope.Start_Date = new Date($scope.Start_Date).toDateString();
               // $scope.End_Date = new Date($scope.End_Date).toDateString();
                if ($scope.group_check == "0") {
                    $scope.dayflag = false;
                    var data = {

                        //  "FOMHWD_CalenderYear": $scope.asmaY_Id,
                        "HRMLY_Id": $scope.yearId,
                        "FO_Start_Date": $scope.Start_Date,
                        "FOMHWDD_ToDate": $scope.Start_Date,
                        //"FOMHWDD_ToDate": $scope.End_Date,
                        "FOMHWD_HolidayWDName": $scope.Holiday_name,
                        "FO_Remark": $scope.Remark,
                        "FOHWDT_Id": $scope.fohwdT_Id,
                        "Day_flag": $scope.dayflag,
                        "HRMD_Id": $scope.depId

                    }
                }
                else if ($scope.group_check == "1") {

                    $scope.dayflag = true;
                    if ($scope.selectedDaysDate.length == 0) {
                        swal("Please Select at least one checkbox");
                        return;
                    }
                    var data = {
                        "HRMLY_Id": $scope.yearId,
                        "FOHWDT_Id": $scope.fohwdT_Id,
                        "Day_flag": $scope.dayflag,
                        daylists1: $scope.day_datecheckboxchcked,
                        "selectedDaysDate": $scope.selectedDaysDate,
                        "HRMD_Id": $scope.depId
                    }
                }

                apiService.create("MasterHoliday/savedata", data).
                    then(function (promise) {

                        if (promise.message != "" && promise.message != null && promise.returnval === true) {
                            swal('Record saved successfully ' + ' ' + promise.message);
                            // $state.reload();
                        }
                        else if (promise.message != "" && promise.message != null) {
                            swal(promise.message);
                            // $state.reload();
                        }
                        else if (promise.returnval === true) {
                            swal('Record saved successfully.');
                            $scope.gridviewDetails = promise.report_list;
                            //$state.reload();
                        }

                        else if (promise.returnval === false) {
                            swal('Failed to save, please contact administrator.');

                        }
                        //$scope.loadData();
                        //$scope.gridOptions1.data = promise.master_eventlist;
                    })

                $state.reload();
                //$scope.loaddata();

            }
            else {
                $scope.submitted = true;

            }


        };

        $scope.clear1 = function () {
            $state.reload();
            $scope.loaddata();
        }

        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.group_check = "0";
        $scope.all_check = function () {


            var toggleStatus = $scope.usercheck;
            angular.forEach($scope.days_list, function (itm) {
                itm.dayy = toggleStatus;
            });

        }
        $scope.showgrid = false;
        $scope.togchkbx = function (index) {
            //$scope.usercheck = $scope.days_list.every(function (options) {
            //    return options.dayy;
            //});

            $scope.Refrencecheckboxchcked.length = 0;
            if ($scope.days_list != "" && $scope.days_list != null) {
                if ($scope.days_list.length > 0) {
                    for (var i = 0; i < $scope.days_list.length; i++) {
                        if ($scope.days_list[i].dayy == true) {
                            $scope.Refrencecheckboxchcked.push($scope.days_list[i]);
                        }
                    }
                }
            }
            //angular.forEach($scope.days_list, function (itm) {
            //    if (itm.dayy)
            //        $scope.Refrencecheckboxchcked.push(itm);
            //});

            var data = {
                "HRMLY_Id": $scope.asmaY_Id,
                "Day_flag": true,
                daylists1: $scope.Refrencecheckboxchcked,
            }
            apiService.create("MasterHoliday/Change", data).
                then(function (promise) {

                    if (promise.datesanddayList != "" && promise.datesanddayList != null) {
                        $scope.datesanddayList = promise.datesanddayList;
                        $scope.showgrid = true;
                    }
                    else {
                        $scope.showgrid = false;
                    }

                    //  $scope.loadData();
                    //  $scope.gridOptions1.data = promise.master_eventlist;
                })



        }


        $scope.toggleAll = function () {
            var toggleStatus = $scope.all_select;
            angular.forEach($scope.datesanddayList, function (itm) {
                itm.all = toggleStatus;
                if ($scope.all_select == true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                    }
                }
                else {
                    $scope.printstudents.splice(itm);
                }
            });
        }



        //$scope.toggleAll = function () {
        //    
        //    var toggleStatus = $scope.all_select;
        //    angular.forEach($scope.datesanddayList, function (itm) {
        //        itm.all = toggleStatus;

        //    });
        //}
        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.all_select = $scope.datesanddayList.every(function (itm) { return itm.selected; });
            if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
                $scope.printstudents.push(SelectedStudentRecord);
            }
            else {
                $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            }

        }

        $scope.isOptionsRequired = function () {
            if ($scope.group_check == "1") {
                return !$scope.days_list.some(function (options) {
                    return options.dayy;
                });
            }
            else if ($scope.group_check == "0") {
                return false;
            }

        }
        $scope.getholidayType = function () {
            $scope.showgrid = false;
            $scope.group_check = 1;
            var holidayType = $scope.fohwdT_Id;
            for (var i = 0; i < $scope.holidayType.length; i++) {
                if (holidayType == $scope.holidayType[i].fohwdT_Id) {
                    if ($scope.holidayType[i].fohtwD_HolidayWDType == 'PUBLIC HOLIDAY' || $scope.holidayType[i].fohtwD_HolidayWDType == 'WEEK END' || $scope.holidayType[i].fohtwD_HolidayWDType == 'PAID HOLIDAY') {
                        $scope.group_check = 0;
                        $scope.dayselection = "notrequired";
                        if ($scope.group_check == "1") {
                            $scope.showdatefield = false;
                        }
                        else if ($scope.group_check == "0") {
                            $scope.showdatefield = true;
                        }



                    }
                    else {
                        $scope.dayselection = "required";
                        $scope.showdatefield = false;
                    }

                    if ($scope.holidayType[i].fohtwD_HolidayWDType == 'WEEK END') {
                        $scope.Showdepartment = true;
                    }
                    else {
                        $scope.Showdepartment = false;
                    }

                }
            }
            if ($scope.asmaY_Id != "" && $scope.asmaY_Id != null && $scope.days_list != "" && $scope.days_list != null) {
                if ($scope.days_list.length > 0) {
                    $scope.togchkbx();
                }
            }
        }
        $scope.load_group_check = function () {
            if ($scope.group_check == "0") {
                if ($scope.days_list != "" && $scope.days_list != null) {
                    if ($scope.days_list.length > 0) {
                        for (var i = 0; i < $scope.days_list.length; i++) {
                            $scope.days_list[i].dayy = false;
                            $scope.showgrid = false;
                        }
                    }
                }
            }
        }


    }
})();


















