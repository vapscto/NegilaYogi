
//dashboard.controller("AttendanceStaffWiseController", ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache','Excel','$timeout',
//function ($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, Excel, $timeout) {

    (function () {
        'use strict';
        angular
            .module('app')
            .controller('AttendanceStaffWiseController', AttendanceStaffWiseController)
        AttendanceStaffWiseController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', 'Excel', '$timeout']
        function AttendanceStaffWiseController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, Excel, $timeout) {
    $scope.currentPage = 1;
    $scope.itemsPerPage = 10;
    $scope.loadradioval = true;  
            var id = 1;
            $scope.tadprint = false;
            var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
            if (admfigsettings.length > 0) {
                var logopath = admfigsettings[0].asC_Logo_Path;
            }
            $scope.imgname = logopath;
            $scope.ddate = new Date();
            $scope.usrname = localStorage.getItem('username');
    $scope.departmentdropdown = [];
    $scope.Designation_types = [];
    $scope.deptcheck = false;
    $scope.desgcheck = false;
    $scope.employeelst = [];
    $scope.gridtab = false;
    $scope.maxDatemf = new Date();
           
    apiService.getURI("AttendanceStaffWise/Getdepartment", id).
then(function (promise) {
    
    $scope.groupTypedropdown = promise.groupTypedropdown;
})

            $scope.all_checkgrptype = function () {
                debugger;
                var toggleStatus = $scope.groupTypeselectedAll;
                angular.forEach($scope.groupTypedropdown, function (itm) {
                    itm.selected = toggleStatus;
                });
                $scope.get_department();
            }

            $scope.get_department = function () {
                $scope.Designation_types = [];
                $scope.desgcheck = '';
                $scope.deptcheck = '';

                $scope.groupTypeselectedAll = $scope.groupTypedropdown.every(function (options) {
                    return options.selected;
                });

                $scope.get_departmentnew();

            }


            $scope.get_departmentnew = function () {
                // $scope.groupTypeselectedAll = "";
                var groupTypeselected = [];
                angular.forEach($scope.groupTypedropdown, function (itm) {
                    if (itm.selected) {
                        groupTypeselected.push(itm.hrmgT_Id);//
                    }

                });

                if (groupTypeselected != undefined) {
                    var data = {
                        //"multipledep": groupidss,
                        hrmgT_IdList: groupTypeselected,
                    }
                    apiService.create("AttendanceStaffWise/get_department", data).
                        then(function (promise) {

                            $scope.departmentdropdown = promise.departmentdropdown;
                        })
                }
                else {
                    $scope.Designation_types = "";
                    $scope.employeedropdown = "";
                    $scope.departmentdropdown = "";
                }
            }




    $scope.all_checkdep = function () {
        
        var toggleStatus = $scope.deptcheck;
        angular.forEach($scope.departmentdropdown, function (itm) {
            itm.selected = toggleStatus;
        });
        $scope.get_designation();
    }

    $scope.get_designation = function () {
        $scope.deptcheck = $scope.departmentdropdown.every(function (options) {
            return options.selected;
        });
        
        $scope.get_designationnew();
    }
    $scope.get_designationnew = function () {
        $scope.desgcheck = "";
        var groupidss;
        for (var i = 0; i < $scope.departmentdropdown.length; i++) {
            if ($scope.departmentdropdown[i].selected == true) {

                if (groupidss == undefined)
                    groupidss = $scope.departmentdropdown[i].hrmD_Id;
                else
                    groupidss = groupidss + "," + $scope.departmentdropdown[i].hrmD_Id;
            }
        }

        if (groupidss != undefined) {
            var data = {
                "multipledep": groupidss,
            }
            apiService.create("AttendanceStaffWise/get_designation", data).
            then(function (promise) {
                
                $scope.Designation_types = promise.designationdropdown;
            })
        }
        else {
            $scope.Designation_types = "";
            $scope.Employeelst = "";
        }
    }

    $scope.all_checkdesg = function () {
        
        var toggleStatus = $scope.desgcheck;
        angular.forEach($scope.Designation_types, function (itm) {
            itm.selected = toggleStatus;
        });
        $scope.get_employee();
    }

    //fill desg end
    //fill employee start
    $scope.get_employee = function () {
        $scope.desgcheck = $scope.Designation_types.every(function (options) {

            return options.selected;
        });
        
        $scope.get_employeenew();
    }
    $scope.get_employeenew = function () {
        $scope.stf = false;
        var deptIds;
        for (var i = 0; i < $scope.departmentdropdown.length; i++) {
            if ($scope.departmentdropdown[i].selected == true) {

                if (deptIds == undefined)
                    deptIds = $scope.departmentdropdown[i].hrmD_Id;
                else
                    deptIds = deptIds + "," + $scope.departmentdropdown[i].hrmD_Id;
            }
        }
        var groupidss;
        for (var i = 0; i < $scope.Designation_types.length; i++) {
            if ($scope.Designation_types[i].selected == true) {

                if (groupidss == undefined)
                    groupidss = $scope.Designation_types[i].hrmdeS_Id;
                else
                    groupidss = groupidss + "," + $scope.Designation_types[i].hrmdeS_Id;
            }
        }
        if (groupidss != undefined) {
            var data = {
                "multipledes": groupidss,
                "multipledep": deptIds
            }
            apiService.create("AttendanceStaffWise/get_employee", data).
            then(function (promise) {
                
                $scope.employeelst = promise.stafflist;
                if ($scope.employeelst.length > 0) {
                    $scope.stf = true;
                }
            })
        }
        else {
            $scope.employeelst = "";
        }
    }


    $scope.GetReport = function ()
    {
        if ($scope.myForm2.$valid) {
        
        $scope.gridtab = false;
       // alert($scope.hrmE_Id)
    var data = {
        "Fromdate": new Date($scope.fromdate).toDateString(),
        "Todate": new Date($scope.todate).toDateString(),
        "HRME_Id": $scope.hrmE_Id,
    } 
    var config = {
        headers: {
            'Content-Type': 'application/json;'
        }
    }
    
    apiService.create("AttendanceStaffWise/Getdetails", data).then(function (promise) {
        
        $scope.reportlist = promise.staffName;
        if ($scope.reportlist != null) {
            $scope.gridtab = true;
        }
        else {
            swal("No record found")
        }
    })
        }
        else {
            $scope.submitted = true;
        }
}


    $scope.printData = function () {
        var innerContents = document.getElementById("printrcp").innerHTML;
        var popupWinindow = window.open('');
        popupWinindow.document.open();
        popupWinindow.document.write('<html><head>' +
             '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
              '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ProgressReport/BGIProgressReportPdf.css" />' +
          '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
        popupWinindow.document.close();

    }

    $scope.exptoex = function (tableId) {
        //alert(tableId);
        if ($scope.reportlist !== null && $scope.reportlist.length > 0) {
            var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        }
        else {
            swal("Please Select Records to be Exported");
        }
    }

    $scope.order = function (keyname) {
        $scope.sortKey = keyname;
        $scope.reverse = !$scope.reverse;
    };

    $scope.submitted = false;    
    $scope.printstudents = [];
    $scope.toggleAll1 = function () {
        var toggleStatus = $scope.Stflist;
        angular.forEach($scope.employeelst, function (itm) {
            itm.selected = toggleStatus;
            if ($scope.Stflist == true) {
                if ($scope.printstudents.indexOf(itm) === -1) {
                    $scope.printstudents.push(itm);
                }
            }
            else {
                $scope.printstudents.splice(itm);
            }
        });
    }

    $scope.optionToggled = function (SelectedStudentRecord, index) {
        $scope.all = $scope.employeelst.every(function (itm)
        { return itm.selected; });
        if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
            $scope.printstudents.push(SelectedStudentRecord);
        }
        else {
            $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
        }
    }
    
   
    $scope.isOptionsRequired1 = function () {
        return !$scope.departmentdropdown.some(function (options) {
            return options.selected;
        });
    }
    $scope.isOptionsRequired2 = function () {
        return !$scope.Designation_types.some(function (options1) {
            return options1.selected;
        });
    }
    $scope.interacted2 = function (field) {
        return $scope.submitted;
    };
 
   


    $scope.cancel2 = function () {

        $state.reload();
        //$scope.stfmsg = "";
        //$scope.Stflist = false;
        //angular.forEach($scope.departmentdropdown, function (user) {
        //    user.selected = false;
        //})
        //$scope.employeelst = [];
        //$scope.desgcheck = false;
        //$scope.stf = false;
        //$scope.Designation_types = [];
        //$scope.gridtab = false;
        //$scope.deptcheck = false;
        //$scope.fromdate = false;
        //$scope.todate = false;
        //$scope.reportlist = false;
    }
        }
    })();