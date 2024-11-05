(function () {
    'use strict';

    angular
        .module('app')
        .controller('BMIReportController', BMIReportController);


    BMIReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache']
    function BMIReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache) {




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
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;

        //============TO  GEt The Values iN Grid

        $scope.Loaddata = function () {
            var pageid = 2;
            apiService.getURI("BMIReport/getDetails", pageid).
                then(function (promise) {
                    debugger;

                    $scope.yearlt = promise.academicYear;
                    if (promise.houseList.length > 0) {
                        $scope.houseList = promise.houseList;
                    }
                    $scope.ASMAY_Id = "";
                    $scope.ASMCL_Id = "";
                    $scope.ASMS_Id = "";
                    $scope.classDropdown = "";
                    $scope.sectionDropdown = "";
                })
        };


        //=================================Get Class
        $scope.get_class = function () {
            debugger;
            $scope.ASMCL_Id = "";
            $scope.usercheck23 = false;
            $scope.Cumureport = false;
            $scope.export = false;
            $scope.screport = false;
            
            angular.forEach($scope.sectionDropdown, function (tt) {
                tt.select = false;
            });
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            }
            apiService.create("BMIReport/get_class", data)
                .then(function (promise) {
                    $scope.classDropdown = promise.classList;

                })
        }

        //=================================Get Section
        $scope.get_section = function () {
            $scope.usercheck23 = false;
            $scope.Cumureport = false;
            $scope.export = false;
            $scope.screport = false;

            angular.forEach($scope.sectionDropdown, function (tt) {
                tt.select = false;
            });
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            }
            apiService.create("BMIReport/get_section", data)
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
        $scope.selectedStdList = [];
        $scope.showdetails = function () {
            $scope.selectedSectionlist = [];
            debugger;
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                //if ($scope.Type == 'House') {
                //    var data = {
                //        "ASMAY_Id": 0,
                //        "ASMCL_Id": 0,
                //        "ASMS_Id": 0,
                //        "Type": $scope.Type,
                //        "SPCCMH_Id": $scope.SPCCMH_Id,
                //    }

                //}
                //else if ($scope.Type == 'CS') {
                //    var data = {
                //        "ASMAY_Id": $scope.ASMAY_Id,
                //        "ASMCL_Id": $scope.ASMCL_Id,
                //        "ASMS_Id": $scope.ASMS_Id,
                //        "Type": $scope.Type,
                //        "SPCCMH_Id": 0,
                //    }
                //}
                angular.forEach($scope.sectionDropdown, function (section) {
                    if (section.select == true) {
                        $scope.selectedSectionlist.push({ asmS_Id: section.asmS_Id });
                    }
                });
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    //"ASMS_Id": $scope.ASMS_Id,
                    //"Type": $scope.Type,
                    //"SPCCMH_Id": 0,
                    selectedSectionlist: $scope.selectedSectionlist,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("BMIReport/report", data).
                    then(function (promise) {
                        $scope.newuser = promise.viewlist;
                        if ($scope.newuser.length > 0) {
                            $scope.newuser = promise.viewlist;

                            $scope.Cumureport = true;
                            $scope.screport = true;
                            $scope.export = true;
                            angular.forEach($scope.yearlt, function (fff) {
                                if (fff.asmaY_Id == $scope.ASMAY_Id) {
                                    $scope.yearname = fff.asmaY_Year;
                                }
                            })

                        }
                        else {
                            $scope.screport = false;
                            $scope.export = false;
                            $scope.Cumureport = false;
                            swal("No Records Found!");

                        }
                    })
            }
        };

        $scope.cancel = function () {
            //$scope.ASMAY_Id = "";
            //$scope.ASMCL_Id = "";
            //$scope.ASMS_Id = "";
            //$scope.SPCCMD_Id = "";
            //$scope.SPCCMH_Id = "";
            //$scope.Cumureport = false;
            //$scope.screport = false;
            //$scope.export = false;
            //$scope.submitted = false;
            //$scope.myForm.$setPristine();
            //$scope.myForm.$setUntouched();

            $state.reload();
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
                    '<link type="text/css" media="print" href="css/print/Sports/BMIReportPdf.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
            }
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
            debugger;
            if ($scope.Type == 'CS') {
                $scope.Loaddata();
            }
            else if ($scope.Type != 'CS') {
                $scope.asmaY_Id = "";
                $scope.asmcL_Id = "";
                $scope.asmS_Id = "";
                $scope.Loaddata();
            }
        }
        $scope.searchchkbx23 = "";
        $scope.all_check23 = function () {
            var checkStatus = $scope.usercheck23;
            angular.forEach($scope.sectionDropdown, function (itm) {
                itm.select = checkStatus;
            });
        }

        $scope.togchkbx23 = function () {
            $scope.usercheck23 = $scope.sectionDropdown.every(function (options) {
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






//(function () {
//    'use strict';

//    angular
//        .module('app')
//        .controller('BMIReportController', BMIReportController);


//    BMIReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache']
//    function BMIReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache) {




//        $scope.searchValue = "";
//        $scope.DeleteRecord = {};
//        $scope.EditRecord = {};
//        $scope.obj = {};
//        $scope.studentlist = false;
//        $scope.currentPage = 1;
//        $scope.itemsPerPage = 10;
//        $scope.screport = false;


//        $scope.ddate = new Date();
//        $scope.usrname = localStorage.getItem('username');
//        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
//        var paginationformasters;
//        var copty;
//        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
//        if (ivrmcofigsettings.length > 0) {
//            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
//            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
//        }
//        $scope.coptyright = copty;
//        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
//        if (admfigsettings.length > 0) {
//            var logopath = admfigsettings[0].asC_Logo_Path;
//        }
//        $scope.imgname = logopath;

//        //============TO  GEt The Values iN Grid

//        $scope.Loaddata = function () {
//            var pageid = 2;
//            apiService.getURI("BMIReport/getDetails", pageid).
//                then(function (promise) {
//                    debugger;

//                    $scope.yearlt = promise.academicYear;
//                    if (promise.houseList.length > 0) {
//                        $scope.houseList = promise.houseList;
//                    }
//                    $scope.ASMAY_Id = "";
//                    $scope.ASMCL_Id = "";
//                    $scope.ASMS_Id = "";
//                    $scope.classDropdown = "";
//                    $scope.sectionDropdown = "";
//                })
//        };


//        //=================================Get Class
//        $scope.get_class = function () {
//            debugger;
//            $scope.ASMCL_Id = "";
//            var data = {
//                "ASMAY_Id": $scope.ASMAY_Id
//            }
//            apiService.create("BMIReport/get_class", data)
//                .then(function (promise) {
//                    $scope.classDropdown = promise.classList;

//                })
//        }

//        //=================================Get Section
//        $scope.get_section = function () {
//            var data = {
//                "ASMAY_Id": $scope.ASMAY_Id,
//                "ASMCL_Id": $scope.ASMCL_Id
//            }
//            apiService.create("BMIReport/get_section", data)
//                .then(function (promise) {
//                    $scope.sectionDropdown = promise.sectionList;

//                })
//        }

//        //$scope.clscatId = 0;
//        $scope.columnSort = false;
//        $scope.isOptionsRequired = function () {
//            return !$scope.stuDropdown.some(function (options) {
//                return options.Selected;
//            });
//        }

//        $scope.sort = function (keyname) {
//            $scope.sortKey = keyname;   //set the sortKey to the param passed
//            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
//        }

//        $scope.interacted = function (field) {
//            return $scope.submitted;
//        };



//        // TO Show The Data
//        $scope.submitted = false;
//        $scope.selectedStdList = [];
//        $scope.showdetails = function () {

//            debugger;
//            $scope.submitted = true;
//            if ($scope.myForm.$valid) {
//                if ($scope.Type == 'House') {
//                    var data = {
//                        "ASMAY_Id": 0,
//                        "ASMCL_Id": 0,
//                        "ASMS_Id": 0,
//                        "Type": $scope.Type,
//                        "SPCCMH_Id": $scope.SPCCMH_Id,
//                    }

//                }
//                else if ($scope.Type == 'CS') {
//                    var data = {
//                        "ASMAY_Id": $scope.ASMAY_Id,
//                        "ASMCL_Id": $scope.ASMCL_Id,
//                        "ASMS_Id": $scope.ASMS_Id,
//                        "Type": $scope.Type,
//                        "SPCCMH_Id": 0,
//                    }
//                }


//                var config = {
//                    headers: {
//                        'Content-Type': 'application/json;'
//                    }
//                }
//                apiService.create("BMIReport/report", data).
//                    then(function (promise) {
//                        $scope.newuser = promise.viewlist;
//                        if ($scope.newuser.length > 0) {
//                            $scope.newuser = promise.viewlist;

//                            $scope.Cumureport = true;
//                            $scope.screport = true;
//                            $scope.export = true;
//                        }
//                        else {
//                            $scope.screport = false;
//                            $scope.export = false;
//                            $scope.Cumureport = false;
//                            swal("No Records Found!");

//                        }
//                    })
//            }
//        };

//        $scope.cancel = function () {
//            $scope.ASMAY_Id = "";
//            $scope.ASMCL_Id = "";
//            $scope.ASMS_Id = "";
//            $scope.SPCCMD_Id = "";
//            $scope.SPCCMH_Id = "";
//            $scope.Cumureport = false;
//            $scope.screport = false;
//            $scope.export = false;
//            $scope.submitted = false;
//            $scope.myForm.$setPristine();
//            $scope.myForm.$setUntouched();

//            //$state.reload();
//        }

//        //for print
//        $scope.Print = function () {

//            if ($scope.filterValue1 !== null && $scope.filterValue1.length > 0) {
//                var innerContents = document.getElementById("printSectionId").innerHTML;
//                var popupWinindow = window.open('');
//                popupWinindow.document.open();
//                popupWinindow.document.write('<html><head>' +
//                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
//                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
//                    '<link type="text/css" media="print" href="css/print/Sports/BMIReportPdf.css" rel="stylesheet" />' +
//                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
//                );
//                popupWinindow.document.close();
//            }
//        }

//        // end for print

//        $scope.exportToExcel = function (table) {
//            debugger;
//            var exportHref = Excel.tableToExcel(table, 'sheet name');
//            $timeout(function () { location.href = exportHref; }, 100);

//        }



//        //===========================================Radio

//        $scope.changeRadiobtn = function () {
//            $scope.screport = false;
//            $scope.export = false;
//            $scope.Cumureport = false;
//            debugger;
//            if ($scope.Type == 'CS') {
//                $scope.Loaddata();
//            }
//            else if ($scope.Type != 'CS') {
//                $scope.asmaY_Id = "";
//                $scope.asmcL_Id = "";
//                $scope.asmS_Id = "";
//                $scope.Loaddata();
//            }
//        }
        


//    }
//})();





