(function () {
    'use strict';
    angular
        .module('app')
        .controller('Student_TC_apply_ReportController', student_TC_apply_ReportController)
    student_TC_apply_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function student_TC_apply_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {


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
       
        $scope.togchkbx = function () {
            $scope.usercheck = $scope.student_s_list.every(function (options) {
                return options.select;
            });
        }
        //============================check  box
        $scope.searchchkbx = "";
        $scope.all_check = function () {
            var checkStatus = $scope.usercheck;
            angular.forEach($scope.student_s_list, function (itm) {
                itm.select = checkStatus;
            });
        }
        
        //======================
        $scope.onLoadGetData = function () {
           
            apiService.getDATA("OnlineLeaveApp/getdate_sla").then(function (promise) {
                if (promise.class_s_list != null) {
                    $scope.class_s_list = promise.class_s_list;
                    $scope.pdata = false;
                }
                else {
                    swal("No Records Found")
                }
            })
        }

        $scope.getsection = function () {
            var pagid = $scope.asmcL_Id;
            apiService.getURI("OnlineLeaveApp/getsection", pagid).then(function (promise) {
                $scope.section_s_list = promise.section_s_list;
            })
        }

        $scope.getstudent = function () {
            var data = {
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.asmS_Id,

            }

            apiService.create("OnlineLeaveApp/getstudent", data).then(function (promise) {
                $scope.student_s_list = promise.student_s_list;
            })
        }

        $scope.submitted = false;
        $scope.getreport = function () {
            
            $scope.student_id_list = [];

            angular.forEach($scope.student_s_list, function (des) {
                if (des.select == true) {
                    $scope.student_id_list.push({ AMST_Id:des.amsT_Id });
                }
            });

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {

                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id,
                    student_id_list: $scope.student_id_list,
                    

                }

                apiService.create("OnlineLeaveApp/get_TC_Report", data).
                    then(function (promise) {

                        if (promise.student_tc_list.length > 0) {

                            $scope.pdata = true;
                            $scope.Cumureport = true;
                            $scope.screport = true;
                            $scope.export = true;

                            $scope.student_tc_list = promise.student_tc_list;
                           
                            $scope.presentCountgrid = promise.student_tc_list.length;


                        }
                        else {
                            $scope.screport = false;
                            $scope.export = false;
                            $scope.Cumureport = false;
                            swal("No Records Found!");
                            $state.reload();
                        }
                    })
            }
            else {

                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };



        $scope.isOptionsRequired = function () {

            return !$scope.student_s_list.some(function (sec) {
                return sec.select;
            });
        }
       
        $scope.searchValue = '';
        $scope.filterValue1 = function (obj) {

            return (angular.lowercase(obj.employeename3)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }
        $scope.toggleAll = function () {
            $scope.printdatatable = [];
            var checkStatus2 = $scope.checkall;
            angular.forEach($scope.exi_employee_print_list, function (itm) {
                if (itm.checked = checkStatus2) {
                    $scope.printdatatable.push(itm);

                }
            });
        };

        $scope.optionToggled = function () {
            $scope.printdatatable = [];
            angular.forEach($scope.exi_employee_print_list, function (itm) {
                if (itm.checked === true) {
                    $scope.printdatatable.push(itm);
                }

            });
        }




        $scope.Print = function () {

            if ($scope.student_tc_list !== null && $scope.student_tc_list.length > 0) {
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
            else {
                swal("Please Select Records to be Printed");
            }
        }

    };

})();