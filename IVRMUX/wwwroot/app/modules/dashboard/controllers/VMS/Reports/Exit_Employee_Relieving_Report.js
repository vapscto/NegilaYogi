(function () {
    'use strict';
    angular
        .module('app')
        .controller('Exit_Employee_RelievingController', exit_Employee_RelievingController)
    exit_Employee_RelievingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function exit_Employee_RelievingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {


        $scope.cancel = function () {
            $state.reload();
        }


        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');


        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }

        $scope.itemsPerPage = paginationformasters;
        if ($scope.itemsPerPage == undefined || $scope.itemsPerPage == null) {
            $scope.itemsPerPage = 5;
        }
        $scope.currentPage = 1;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        $scope.reporsmart = false;

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.routetypelist = [];
        $scope.vh = false;
        $scope.FMG_Id = 0;
        //===============================search check box
        $scope.filterchkbx = function (obj) {
            return (angular.lowercase(obj.hrmD_DepartmentName)).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }
        $scope.filterchkbx23 = function (obj) {
            return (angular.lowercase(obj.hrmdeS_DesignationName)).indexOf(angular.lowercase($scope.searchchkbx23)) >= 0;
        }
        //============================
        $scope.togchkbx23 = function () {
            $scope.usercheck23 = $scope.designation_list_R.every(function (options) {
                return options.select1;
            });
        }
        $scope.togchkbx = function () {
            $scope.usercheck = $scope.department_list_R.every(function (options) {
                return options.select;
            });
        }
        //============================check  box
        $scope.searchchkbx = "";
        $scope.all_check = function () {
            var checkStatus = $scope.usercheck;
            angular.forEach($scope.department_list_R, function (itm) {
                itm.select = checkStatus;
            });
        }
        $scope.searchchkbx23 = "";
        $scope.all_check23 = function () {
            var checkStatus = $scope.usercheck23;
            angular.forEach($scope.designation_list_R, function (itm) {
                itm.select1 = checkStatus;
            });
        }
        //======================
        $scope.onLoadGetData = function () {
            apiService.getDATA("Exit_Employee/get_all_relieving_data_R").then(function (promise) {
                if (promise.department_list_R != null) {
                    $scope.department_list_R = promise.department_list_R;
                    $scope.designation_list_R = promise.designation_list_R;

                }
                else {
                    swal("No Records Found")
                }
            })
        }
        $scope.searchValue = '';
        $scope.filterValue1 = function (obj) {

            return (angular.lowercase(obj.employeename3)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }

       // $scope.submitted = false;
        $scope.getreport = function () {
            $scope.selectedept_list = [];
            $scope.selectedesig_list = [];



            angular.forEach($scope.department_list_R, function (dept) {
                if (dept.select == true) {
                    $scope.selectedept_list.push({ HRMD_Id: dept.hrmD_Id });
                }
            });
            angular.forEach($scope.designation_list_R, function (des) {
                if (des.select1 == true) {
                    $scope.selectedesig_list.push({ HRMDES_Id: des.hrmdeS_Id });
                }
            });

            Date.prototype.AddDays = function (noOfDays) {
                this.setTime(this.getTime() + (noOfDays * (1000 * 60 * 60 * 24)));
                return this;
            }
            var dateNew = new Date();
            var noOfDays = 2;

            $scope.newdate = dateNew.AddDays(noOfDays);
           // $scope.StartDate1 = $filter('date')($scope.newdate, "yyyy-MM-dd");


            if ($scope.Fromdate == null || $scope.Fromdate == '') {
                $scope.StartDate1 = $filter('date')($scope.newdate, "yyyy-MM-dd");
            }
            else {
                $scope.StartDate1 = $filter('date')($scope.Fromdate, "yyyy-MM-dd");
            }
            if ($scope.Todate == null || $scope.Todate == '') {
                $scope.EndDate1 = $filter('date')($scope.newdate, "yyyy-MM-dd");
            }
            else {
                $scope.EndDate1 = $filter('date')($scope.Todate, "yyyy-MM-dd");
            }
           
            $scope.grid_flag = false;
           
           
            
            
           $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                  "FROMDATE": $scope.StartDate1,
                    "TODATE": $scope.EndDate1,
                     selectedept_list: $scope.selectedept_list,
                    selectedesig_list: $scope.selectedesig_list,

                }

                apiService.create("Exit_Employee/showdetails_relieving_R", data).
                    then(function (promise) {

                        if (promise.exi_employee_print_list.length > 0) {


                            $scope.Cumureport = true;
                            $scope.screport = true;
                            $scope.export = true;

                            $scope.exi_employee_print_list = promise.exi_employee_print_list;
                            $scope.presentCountgrid = promise.exi_employee_print_list.length;
                        }
                        else {
                            $scope.screport = false;
                            $scope.export = false;
                            $scope.Cumureport = false;
                            swal("No Records Found!");

                        }
                    })
            }
            else {

                $scope.submitted = true;
            }
        };

       


        $scope.Print_preview = function (ww) {
                       var hrmeid = ww.hrme_id;
            apiService.getURI("Exit_Employee/print_exit_employee", hrmeid).then(function (promise) {
               
                $scope.employeename_p = promise.exit_employee_print_report[0].employeename_p;
                $scope.HRME_EmployeeCode_p = promise.exit_employee_print_report[0].HRME_EmployeeCode_p;
                $scope.HRMDES_DesignationName_p = promise.exit_employee_print_report[0].HRMDES_DesignationName_p;
                $scope.HRME_DOJ_p = promise.exit_employee_print_report[0].HRME_DOJ_p;
                $scope.year_p = promise.exit_employee_print_report[0].year_p;
                $scope.month_p = promise.exit_employee_print_report[0].month_p;
                $scope.company_Name_p = promise.exit_employee_print_report[0].company_Name_p;
                $scope.ISMRESG_AccRejDate_p = promise.exit_employee_print_report[0].ISMRESG_AccRejDate_p;
                $scope.ISMRESG_TentativeLeavingDate_p = promise.exit_employee_print_report[0].ISMRESG_TentativeLeavingDate_p;
                $scope.Gdate_p = promise.exit_employee_print_report[0].Gdate_p;
                $scope.exit_print_list2 = promise.exit_print_list2
            
               
            })
        }
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.print = function () {

            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/Sports/HouseReportPdf.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
            
        }
        $scope.isOptionsRequired = function () {

            return !$scope.department_list_R.some(function (sec) {
                return sec.select;
            });
        }
        $scope.isOptionsRequired1 = function () {

            return !$scope.designation_list_R.some(function (sec) {
                return sec.select1;
            });
        }
    };

})();