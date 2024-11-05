(function () {
    'use strict';
    angular
        .module('app')
        .controller('CategorySeatDistributionController', CategorySeatDistributionController)

    CategorySeatDistributionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$timeout', 'Excel']
    function CategorySeatDistributionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $timeout, Excel) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("CategorySeatDistribution/getdetails", pageid).then(function (promise) {

                $scope.acdlist = promise.acdlist;
                //$scope.courselist = promise.courselist;
                //$scope.semlist = promise.semlist;
                //$scope.branchlist = promise.branchlist;
                $scope.quotalist = promise.quotalist;
            });
        };

        $scope.onselectAcdYear = function (ASMAY_Id) {
            $scope.catreport = false;
            $scope.statestudentlist = [];
            var data = {
                "ASMAY_Id": ASMAY_Id
            };
            apiService.create("StatewiseStudentAdmission/onselectAcdYear", data).then(function (promise) {
                $scope.courselist = promise.courselist;

            });
        };

        $scope.onselectCourse = function () {
            $scope.catreport = false;
            $scope.statestudentlist = [];
            var data = {
                "AMCO_Id": $scope.AMCO_Id,
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("StatewiseStudentAdmission/onselectCourse", data).then(function (promise) {
                $scope.branchlist = promise.branchlist;
            });
        };

        $scope.onselectBranch = function () {
            $scope.catreport = false;
            $scope.statestudentlist = [];
            var data = {

                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id
            };

            apiService.create("StatewiseStudentAdmission/onselectBranch", data).then(function (promise) {
                $scope.semlist = promise.semlist;
            });
        };


        $scope.all_check = function () {

            var toggleStatus = $scope.detail_checked;
            angular.forEach($scope.quotalist, function (itm) {
                itm.ivrm_id = toggleStatus;
            });
        }

        $scope.exportToExcel = function (tableId) {

            if ($scope.column_list !== null && $scope.column_list.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please Select Records to be Exported");
            }

        }

        $scope.onreport = function () {

            if ($scope.myForm.$valid) {

                $scope.albumNameArraycolumn = [];
                angular.forEach($scope.quotalist, function (role) {
                    if (!!role.ivrm_id) $scope.albumNameArraycolumn.push(role);
                })

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "TempararyArrayListcoloumn": $scope.albumNameArraycolumn

                }


                apiService.create("CategorySeatDistribution/onreport", data).then(function (promise) {

                    $scope.main_list = promise.datareport;
                    $scope.column_list = $scope.albumNameArraycolumn;
                    $scope.totalalloted = $scope.getAlloted();
                    $scope.totalallocated = $scope.getAllocated();
                    $scope.totalvacant = $scope.getVacant();
                })


            }
            else {
                $scope.submitted = true;
            }
        }


        $scope.getAlloted = function (int) {
            var totalalloted = 0;
            angular.forEach($scope.main_list, function (e) {
                totalalloted += e.acscD_SeatNos;
            });
            return totalalloted;
        };

        $scope.getAllocated = function (int) {
            var totalallocated = 0;
            angular.forEach($scope.main_list, function (e) {
                totalallocated += e.count;
            });
            return totalallocated;
        };

        $scope.getVacant = function (int) {
            var totalvacant = 0;
            angular.forEach($scope.main_list, function (e) {
                totalvacant += (e.acscD_SeatNos - e.count);
            });
            return totalvacant;
        };

        $scope.printData = function () {

            var innerContents = document.getElementById("table").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }


        $scope.cancel = function () {
            $scope.ASMAY_Id = '';
            $scope.AMCO_Id = '';
            $scope.AMB_Id = '';
            $scope.AMSE_Id = '';
            $scope.ACQ_Id = '';
            $scope.main_list = [];
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";

            angular.forEach($scope.quotalist, function (option9) {
                option9.ivrm_id = false;
            });
        };


    }

})();