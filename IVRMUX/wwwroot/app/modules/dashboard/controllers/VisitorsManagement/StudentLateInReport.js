(function () {
    'use strict';

    angular
        .module('app')
        .controller('StudentLateInReportController', StudentLateInReportController);

    StudentLateInReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter'];

    function StudentLateInReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {
        $scope.submitted = false;
        $scope.table_flag = false;
        $scope.searchValue = "";



        $scope.ddate = new Date();
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.coptyright = copty;
        $scope.sortKey = "regno";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.presentCountgrid = 0;

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;


        $scope.Onload = function () {
            apiService.getURI("StudentLateInReport/loaddata/", 5).then(function (promise) {
                $scope.academicYear = promise.academicYear;
                $scope.classList = promise.classList;
                $scope.sectionList = promise.sectionList;

                $scope.month_list = promise.month_list;
            });
        }

        $scope.cancel = function () {

            $state.reload();
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;


        $scope.getReport = function () {
            debugger;
            $scope.Type = 'CS';
            $scope.selectedClasslist = [];
            $scope.selectedSectionlist = [];
            if ($scope.myForm.$valid) {

                var fromdate1 = $scope.AttendanceFromDate == null ? "" : $filter('date')($scope.AttendanceFromDate, "yyyy-MM-dd");
                var todate1 = $scope.AttendanceToDate == null ? "" : $filter('date')($scope.AttendanceToDate, "yyyy-MM-dd");

                if ($scope.Type == 'CS') {

                    angular.forEach($scope.classList, function (cls) {
                        if (cls.select == true) {
                            $scope.selectedClasslist.push({ asmcL_Id: cls.asmcL_Id });
                        }
                    });

                    angular.forEach($scope.sectionList, function (sect) {
                        if (sect.select == true) {
                            $scope.selectedSectionlist.push({ asmS_Id: sect.asmS_Id });
                        }
                    });

                    var data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "Type": $scope.Type,
                        selectedClasslist: $scope.selectedClasslist,
                        selectedSectionlist: $scope.selectedSectionlist,
                        "AttendanceFromDate": fromdate1,
                        "AttendanceToDate": todate1,
                        "all1": $scope.all1,
                        "month_id": $scope.month,
                    }
                }
                else if ($scope.Type == 'AY') {

                    var data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "Type": $scope.Type,
                        "AttendanceFromDate": fromdate1,
                        "AttendanceToDate": todate1,
                    }
                }


                apiService.create("StudentLateInReport/getReport/", data).
                    then(function (promise) {

                        if (promise.reportlist.length > 0) {
                            $scope.reportlist = promise.reportlist;
                            $scope.presentCountgrid = $scope.reportlist.length;

                            $scope.Cumureport = true;
                            $scope.screport = true;
                            $scope.export = true;
                            debugger;
                            angular.forEach($scope.academicYear, function (aa) {
                                if (aa.asmaY_Id == $scope.ASMAY_Id) {
                                    $scope.yearname = aa.asmaY_Year;
                                }
                            })

                        }
                        else {
                            swal("No Records Found");
                            $scope.yearEndReport = "";
                            $scope.screport = false;
                            $scope.export = false;
                            $scope.Cumureport = false;
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        }
        $scope.printdatatable = [];
        $scope.toggleAll = function () {
            var toggleStatus = $scope.checkall;
            angular.forEach($scope.yearEndReport, function (itm) {
                itm.checked = toggleStatus;
                if ($scope.checkall == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
        }
        $scope.optionToggled = function (SelectedStudentRecord, index) {

            $scope.checkall = $scope.yearEndReport.every(function (itm) { return itm.checked; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }


        }
        $scope.Print = function () {

            if ($scope.filterValue1 !== null && $scope.filterValue1.length > 0) {
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

        $scope.changeRadiobtn = function () {

            $scope.screport = false;
            $scope.export = false;
            $scope.Cumureport = false;

            if ($scope.Type == 'AY') {

                $scope.usercheck23 = false;
                $scope.usercheck = false;
                $scope.ASMAY_Id = "";


                angular.forEach($scope.classList, function (hs) {
                    hs.select = false;
                });
                angular.forEach($scope.sectionList, function (hs) {
                    hs.select = false;
                });


                $scope.Onload();
            }
            else if ($scope.Type == 'CS') {

                $scope.usercheck23 = false;
                $scope.usercheck = false;
                $scope.ASMAY_Id = "";

                angular.forEach($scope.classList, function (hs) {
                    hs.select = false;
                });
                angular.forEach($scope.sectionList, function (hs) {
                    hs.select = false;
                });

                $scope.Onload();
            }
        }

        $scope.ExportToExcel = function (tableId) {
            if ($scope.filterValue1 !== null && $scope.filterValue1.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please Select Records to be Exported");
            }
        }




        //////////=========================================================For House
        $scope.searchchkbx = "";
        $scope.all_check = function () {
            var checkStatus = $scope.usercheck;
            angular.forEach($scope.classList, function (itm) {
                itm.select = checkStatus;
            });
        }

        $scope.togchkbx = function () {
            $scope.usercheck = $scope.classList.every(function (options) {
                return options.select;
            });
        }

        $scope.isOptionsRequired = function () {
            return !$scope.classList.some(function (options) {
                return options.select;
            });
        }

        $scope.filterchkbx = function (obj) {
            return (angular.lowercase(obj.asmcL_ClassName)).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }

        $scope.filterchkbx23 = function (data) {
            return (angular.lowercase(data.asmC_SectionName)).indexOf(angular.lowercase($scope.searchchkbx23)) >= 0;
        }
        //=============================================================== For Section

        $scope.searchchkbx23 = "";
        $scope.all_check23 = function () {
            var checkStatus = $scope.usercheck23;
            angular.forEach($scope.sectionList, function (itm) {
                itm.select = checkStatus;
            });
        }

        $scope.togchkbx23 = function () {
            $scope.usercheck23 = $scope.sectionList.every(function (options) {
                return options.select;
            });
        }

        $scope.isOptionsRequired23 = function () {
            return !$scope.sectionList.some(function (options) {
                return options.select;
            });
        }

        //$scope.filterchkbx23 = function (obj) {
        //    return (angular.lowercase(obj.sectionName)).indexOf(angular.lowercase($scope.searchchkbx23)) >= 0;
        //}




    }
})();
