(function () {
    'use strict';

    angular
        .module('app')
        .controller('StudentAgeCalcController', StudentAgeCalcController);

    StudentAgeCalcController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout'];

    function StudentAgeCalcController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {


     

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

        $scope.BindData = function () {

            apiService.getDATA("StudentAgeCalcReport/Getdetails").
                then(function (promise) {
                    debugger;

                    $scope.yearlt = promise.yearlist;
                    $scope.categoryList = promise.categoryList;

                })
        };


        //=================================Get Class
        $scope.get_class = function () {
            debugger;
            $scope.usercheck23 = false;
            $scope.usercheck = false;
            $scope.Cumureport = false;
            $scope.export = false;
            $scope.screport = false;
            //$scope.ASMAY_Id = "";
            angular.forEach($scope.houseList, function (hs) {
                hs.select = false;
            });
            angular.forEach($scope.sectionDropdown, function (hs) {
                hs.select = false;
            });
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
            }
            apiService.create("StudentAgeCalcReport/get_class", data)
                .then(function (promise) {
                    $scope.classList = promise.classList;
                    $scope.houseList = promise.houseList;

                })
        }

        //=================================Get Section
        $scope.get_section = function () {

            $scope.usercheck23 = false;
         
            $scope.Cumureport = false;
            $scope.export = false;
            $scope.screport = false;
            
            angular.forEach($scope.sectionDropdown, function (hs) {
                hs.select = false;
            });
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            }
            apiService.create("StudentAgeCalcReport/get_section", data)
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
            $scope.selectedhouselist = [];
            $scope.selectedSectionlist = [];
            debugger;
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                
               
                    angular.forEach($scope.houseList, function (hous) {
                        if (hous.select == true) {
                            $scope.selectedhouselist.push({ spccmH_Id:hous.spccmH_Id });
                        }
                    });

                if ($scope.Type == 'House') {
                    var data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "ASMCL_Id": 0,
                        //"ASMS_Id": 0,
                        "Type": $scope.Type,
                        //"SPCCMH_Id": $scope.SPCCMH_Id,
                       // "SPCCMCC_Id": $scope.spccmcC_Id,
                        selectedhouselist: $scope.selectedhouselist,
                    }

                }
                else if ($scope.Type == 'CS') {

                    angular.forEach($scope.sectionDropdown, function (section) {
                        if (section.select == true) {
                            $scope.selectedSectionlist.push({ asmS_Id: section.asmS_Id });
                        }
                    });
                    var data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "ASMCL_Id": $scope.ASMCL_Id,
                       //"ASMS_Id": $scope.ASMS_Id,
                        "Type": $scope.Type,
                       // "SPCCMCC_Id": $scope.spccmcC_Id,
                        //"SPCCMH_Id": $scope.SPCCMH_Id,
                        selectedhouselist: $scope.selectedhouselist,
                        selectedSectionlist: $scope.selectedSectionlist,
                    }
                }


                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("StudentAgeCalcReport/showdetails", data).
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
                            swal("Records are not Availabe!");

                        }
                    })
            }
        };

        $scope.cancel = function () {
            //$scope.ASMAY_Id = "";
            //$scope.ASMCL_Id = "";
            //$scope.ASMS_Id = "";
            //$scope.SPCCME_Id = "";
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
                    '<link type="text/css" media="print" href="css/print/Sports/HouseReportPdf.css" rel="stylesheet" />' +
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
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            debugger;
            if ($scope.Type == 'CS') {
                $scope.usercheck23 = false;
                $scope.usercheck = false;             
                
                $scope.ASMAY_Id = "";              
                angular.forEach($scope.houseList, function (hs) {
                    hs.select = false;
                });
                angular.forEach($scope.sectionDropdown, function (hs) {
                    hs.select = false;
                });
                $scope.BindData();
            }
            else if ($scope.Type == 'House') {
                $scope.usercheck23 = false;
                $scope.usercheck = false;

                $scope.ASMAY_Id = "";
                angular.forEach($scope.houseList, function (hs) {
                    hs.select = false;
                });
                angular.forEach($scope.sectionDropdown, function (hs) {
                    hs.select = false;
                });
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


///////////////////////------------------------------------------------Old Code...............


        //$scope.searchValue = "";
        //$scope.currentPage = 1;
        //$scope.itemsPerPage = 10;
        //$scope.SPCCESTR_Id = 0;


        //$scope.ddate = new Date();
        //var paginationformasters;
        //var copty;
        //var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        //if (ivrmcofigsettings.length > 0) {
        //    paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        //    copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        //}
        //$scope.coptyright = copty;
        //$scope.sortKey = "regno";   //set the sortKey to the param passed
        //$scope.reverse = true; //if true make it false and vice versa

        //$scope.presentCountgrid = 0;

        //$scope.usrname = localStorage.getItem('username');
        //var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        //if (admfigsettings.length > 0) {
        //    var logopath = admfigsettings[0].asC_Logo_Path;
        //}

        //$scope.imgname = logopath;


        //$scope.sort = function (key) {
        //    $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
        //    $scope.sortKey = key;
        //}
        //$scope.loadgrid = function () {
        //    apiService.getURI("StudentAgeCalc/loadgrid/", 1).then(function (promise) {
        //        $scope.academicYear = promise.academicYear;
        //        $scope.classList = promise.classList;
        //        $scope.sectionList = promise.sectionList;
        //        $scope.cancel();
        //    });
        //}
     
        //$scope.submitted = false;
        //$scope.report = function () {
        //    if ($scope.myForm.$valid) {

        //        var obj = {
        //            //"SPCCAC_Id": $scope.SPCCAC_Id,
        //            "ASMAY_Id": $scope.ASMAY_Id,
        //            "ASMCL_Id": $scope.asmcL_Id,
        //            "ASMS_Id": $scope.asmS_Id
                   
        //        }
        //        apiService.create("StudentAgeCalc/report", obj).
        //            then(function (promise) {
        //                if (promise.datareport.length > 0 && promise.datareport != null) {
        //                    $scope.datareport = promise.datareport;
        //                    $scope.Cumureport = true;
        //                    $scope.screport = true;
        //                }
        //                else {
        //                    swal("Age is not Calculated");
        //                    $scope.cancel();
        //                }
        //            });
        //    }
        //    else {
        //        $scope.submitted = true;
        //    }
        //}

        //$scope.Print = function () {
        //    var innerContents = '';
        //    innerContents = document.getElementById("printSectionId").innerHTML;
        //    var popupWinindow = window.open('');
        //    popupWinindow.document.open();
        //    popupWinindow.document.write('<html><head>' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
        //        '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
        //        '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
        //    );
        //    popupWinindow.document.close();
        //}

        //$scope.cancel = function () {

        //    $scope.SPCCAC_Id = 0;
        //    $scope.ASMAY_Id = "";
        //    $scope.asmcL_Id = "";
        //    $scope.asmS_Id = "";
        //    $scope.Cumureport = false;
        //    $scope.screport = false;
        //    $scope.submitted = false;
        //    $scope.myForm.$setPristine();
        //    $scope.myForm.$setUntouched();
        //}
        //$scope.interacted = function (field) {
        //    return $scope.submitted;
        //};

        //$scope.addColumn4 = function () {
        //    $scope.selected = $scope.studentList.every(function (itm) { return itm.selected; });
        //};



   


    }
})();