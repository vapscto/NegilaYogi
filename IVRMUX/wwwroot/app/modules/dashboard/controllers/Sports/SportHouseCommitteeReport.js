
(function () {
    'use strict';
    angular
        .module('app')
        .controller('SportHouseCommitteeReportController', SportHouseCommitteeReportController)

    SportHouseCommitteeReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache']
    function SportHouseCommitteeReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache) {


        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.searchValue = '';

        $scope.ddate = {};
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

        //TO  GEt The Values iN Grid

        $scope.BindData = function () {


            apiService.getDATA("SportHouseCommitteeReport/Getdetails").
                then(function (promise) {
                    $scope.asmay_list = promise.asmay_list;


                    //$scope.houseList = promise.houseList;
                    //$scope.yearlt = promise.yearlist;
                    //$scope.houseList = promise.houseList;
                    //$scope.eventList = promise.eventList;
                    //$scope.asmcL_Id = "";
                    //$scope.asmS_Id = "";
                    //$scope.classDropdown = "";
                    //$scope.sectionDropdown = "";


                })
        };


        //$scope.clscatId = 0;
        $scope.columnSort = false;
        $scope.isOptionsRequired = function () {
            return !$scope.stuDropdown.some(function (options) {
                return options.Selected;
            });
        }

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //============================Get House List
        $scope.get_House = function () {
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
            }
            apiService.create("SportHouseCommitteeReport/get_House", data).
                then(function (promise) {

                    $scope.houseList = promise.houseList;

                });
        }


        // TO Show The Data
        $scope.submitted = false;
        $scope.showdetails = function () {
            $scope.selectedhouselist = [];

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                angular.forEach($scope.houseList, function (hous) {
                    if (hous.select == true) {
                        $scope.selectedhouselist.push({ spccmH_Id: hous.spccmH_Id });
                    }
                });

                var data = {

                    "ASMAY_Id": $scope.asmaY_Id,
                    //"SPCCMH_Id": $scope.SPCCMH_Id,
                    selectedhouselist: $scope.selectedhouselist,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("SportHouseCommitteeReport/showdetails", data).
                    then(function (promise) {

                        if (promise.viewlist.length > 0) {
                            $scope.newuser = promise.viewlist;
                            $scope.Cumureport = true;
                            $scope.screport = true;
                            $scope.export = true;

                            angular.forEach($scope.asmay_list, function (fff) {
                                if (fff.asmaY_Id == $scope.asmaY_Id) {
                                    $scope.yearname = fff.asmaY_Year;
                                }
                            })

                        }
                        else {
                            $scope.Cumureport = false;
                            $scope.screport = false;
                            $scope.export = false;
                            swal("No Records Found");
                        }

                    })
            }
        };

        $scope.cancel = function () {
            $scope.SPCCMH_Id = "";
            $scope.Cumureport = false;
            $scope.screport = false;
            $scope.export = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.asmaY_Id = "";
        }

        //for print
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
        }

        // end for print

        $scope.exportToExcel = function (table) {

            //if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
            var exportHref = Excel.tableToExcel(table, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
            // }
            // else {
            //   swal("Please Select Records to be Exported");
            // }
        }


        //////////=========================================================For House
        $scope.searchchkbx = "";
        $scope.all_check = function () {
            var checkStatus = $scope.usercheck;
            angular.forEach($scope.houseList, function (itm) {
                itm.select = checkStatus;
            });
        }

        $scope.togchkbx = function () {
            $scope.usercheck = $scope.houseList.every(function (options) {
                return options.select;
            });
        }

        $scope.isOptionsRequired = function () {
            return !$scope.houseList.some(function (options) {
                return options.select;
            });
        }

        $scope.filterchkbx = function (obj) {
            return (angular.lowercase(obj.spccmH_HouseName)).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }




    }

})();