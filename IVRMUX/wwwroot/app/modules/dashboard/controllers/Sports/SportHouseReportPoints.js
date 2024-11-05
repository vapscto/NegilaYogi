
(function () {
    'use strict';
    angular
        .module('app')
        .controller('SportHouseReportPointsController', SportHouseReportController)

    SportHouseReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache']
    function SportHouseReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache) {

        $scope.searchValue = "";
        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.screport = false;


        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null &&  ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;

        //============TO  GEt The Values iN Grid

        $scope.BindData = function () {

            apiService.getDATA("SportHouseReport/Getdetails").
                then(function (promise) {
                    debugger;

                    $scope.yearlt = promise.yearlist;
                    $scope.exam_list = promise.exam_list;

                })
        };


        //=================================Get Class
        $scope.get_class = function () {
            debugger;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
            }
            apiService.create("SportHouseReport/get_class", data)
                .then(function (promise) {
                    $scope.classDropdown = promise.classList;
                    $scope.houseList = promise.houseList;
                    $scope.sectionDropdown = promise.sectionList;

                })
        }

        //=================================Get Section
        $scope.get_section = function () {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            }
            apiService.create("SportHouseReport/get_section", data)
                .then(function (promise) {
                    $scope.sectionDropdown = promise.sectionList;

                })
        }

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



        // TO Show The Data
        $scope.submitted = false;

        $scope.showdetails = function () {
            $scope.yearname = "";
            $scope.TotalValues = "";
            $scope.selectedhouselist = [];
            $scope.selectedSectionlist = [];
            $scope.Classelectedlist = [];
            $scope.Housewise = [];
            $scope.newuser = [];
            $scope.submitted = true;
            $scope.exam_listtemp = [];
            $scope.HouseTotal = [];
            if ($scope.myForm.$valid) {
                //yearlt
                angular.forEach($scope.yearlt, function (year) {
                    if (year.asmaY_Id == $scope.ASMAY_Id) {
                        $scope.yearname = year.asmaY_Year;
                    }
                });
                angular.forEach($scope.houseList, function (hous) {
                    if (hous.select == true) {
                        $scope.selectedhouselist.push({
                            spccmH_Id: hous.spccmH_Id,
                            SPCCMH_HouseName: hous.spccmH_HouseName
                        });
                        $scope.Housewise.push({                            SPCCMH_HouseName: hous.spccmH_HouseName,                        });                        $scope.Housewise.push({                            SPCCMH_HouseName: hous.spccmH_HouseName + "_Points",                            SPH_HouseName: hous.spccmH_HouseName                        });
                    }
                });
                //Copleston House_Points
                angular.forEach($scope.sectionDropdown, function (section) {
                    if (section.select == true) {
                        $scope.selectedSectionlist.push({ asmS_Id: section.asmS_Id });
                    }
                });
                angular.forEach($scope.classDropdown, function (section) {
                    if (section.select == true) {
                        $scope.Classelectedlist.push({ asmcL_Id: section.asmcL_Id });
                    }
                });
                //Housewise
                if ($scope.exam_list != null && $scope.exam_list.length > 0 && $scope.EME_Id > 0) {
                    angular.forEach($scope.exam_list, function (exam) {
                        if (exam.emE_Id == $scope.EME_Id) {
                            $scope.exam_listtemp.push({ EME_Id: exam.emE_Id });
                        }
                    });
                }
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "Classelectedlist": $scope.Classelectedlist,            
                    selectedhouselist: $scope.selectedhouselist,
                    selectedSectionlist: $scope.selectedSectionlist,
                  //  ExamListHouses: $scope.exam_listtemp,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("SportHouseReport/showdetailsNew", data).
                    then(function (promise) {
                        if (promise.viewlist != null && promise.viewlist.length > 0) {
                            $scope.newuser = promise.viewlist;
                            $scope.HouseTotal = promise.houseTotal;
                            $scope.OverallCount = promise.overallCount;
                            
                        }
                        else {
                            swal("Record Not Found !");
                        }
                       
                    })
            }
        };

        $scope.cancel = function () {           
            $state.reload();
        }

        //for print
        $scope.Print = function () {

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

        // end for print

        $scope.exportToExcel = function (table) {
            debugger;
            var exportHref = Excel.tableToExcel(table, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);

        }



        //===========================================Radio

        $scope.changeRadiobtn = function () {
            $scope.screport = false;
            $scope.export = false;
            $scope.Cumureport = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            debugger;
            if ($scope.Type == 'CS') {
                //$scope.ASMAY_Id = "";
                //$scope.ASMCL_Id = "";
                //$scope.ASMS_Id = "";
                //$scope.SPCCMH_Id = "";                
                $scope.BindData();
            }
            else if ($scope.Type == 'House') {
                //$scope.ASMAY_Id = "";
                //$scope.ASMCL_Id = "";
                //$scope.ASMS_Id = "";
                //$scope.SPCCMH_Id = ""; 
                $scope.BindData();
            }
        }


        //////////=========================================================
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

        $scope.searchchkbx23 = "";
        $scope.all_check23 = function () {
            var checkStatus = $scope.usercheck23;
            angular.forEach($scope.sectionDropdown, function (itm) {
                itm.select = checkStatus;
            });
        }
        $scope.all_check234 = function () {
            var checkStatus = $scope.usercheck234;
            angular.forEach($scope.classDropdown, function (itm) {
                itm.select = checkStatus;
            });
        }
        $scope.togchkbx23 = function () {
            $scope.usercheck23 = $scope.sectionDropdown.every(function (options) {
                return options.select;
            });
        }
        $scope.togchkbx234 = function () {
            $scope.usercheck234 = $scope.classDropdown.every(function (options) {
                return options.select;
            });
        }
        $scope.isOptionsRequired234 = function () {
            return !$scope.classDropdown.some(function (options) {
                return options.select;
            });
        }
        $scope.isOptionsRequired23 = function () {
            return !$scope.sectionDropdown.some(function (options) {
                return options.select;
            });
        }

        $scope.filterchkbx23 = function (obj) {
            return (angular.lowercase(obj.asmC_SectionName)).indexOf(angular.lowercase($scope.searchchkbx23)) >= 0;
        }


    }

})();