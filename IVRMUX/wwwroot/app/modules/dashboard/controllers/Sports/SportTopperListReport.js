
(function () {
    'use strict';
    angular
.module('app')
.controller('SportTopperListReportController', SportTopperListReportController)

    SportTopperListReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache']
    function SportTopperListReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache) {



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

            apiService.getDATA("SportTopperListReport/Getdetails").
                then(function (promise) {
                    debugger;
                    //$scope.eventlist = promise.eventList;
                    $scope.yearlt = promise.yearlist;
                    $scope.sportslist = promise.sportslist;
                    //if (promise.houseList.length > 0) {
                    //    $scope.houseList = promise.houseList;
                    //}
                    //$scope.ASMAY_Id = "";
                    //$scope.ASMCL_Id = "";
                    //$scope.ASMS_Id = "";
                    //$scope.classDropdown = "";
                    //$scope.sectionDropdown = "";
                })
        };


        //=================================Get Class
        $scope.get_class = function () {

            $scope.screport = false;
            $scope.export = false;
            $scope.Cumureport = false;

            $scope.usercheck23 = false;
            $scope.usercheck = false;
            $scope.usercheckevent = false;
            $scope.userchecksports = false;

            angular.forEach($scope.sportslist, function (tt) {
                tt.select = false;
            })
            angular.forEach($scope.sectionDropdown, function (hs) {
                hs.select = false;
            });
            angular.forEach($scope.houseList, function (hs) {
                hs.select = false;
            });
            angular.forEach($scope.eventlist, function (hs) {
                hs.select = false;
            });

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            }
            apiService.create("SportTopperListReport/get_class", data)
                .then(function (promise) {
                    $scope.classDropdown = promise.classList;
                    $scope.eventlist = promise.eventList;
                    $scope.houseList = promise.houseList;
                })
        }

        //=================================Get Section
        $scope.get_section = function () {

            $scope.usercheck23 = false;
            $scope.screport = false;
            $scope.export = false;
            $scope.Cumureport = false;

            angular.forEach($scope.sectionDropdown, function (hs) {
                hs.select = false;
            });

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            }
            apiService.create("SportTopperListReport/get_section", data)
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
            $scope.selectedEventlist = [];
            $scope.selectedSportslist = [];

            debugger;
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                angular.forEach($scope.houseList, function (hous) {
                    if (hous.select == true) {
                        $scope.selectedhouselist.push({ spccmH_Id: hous.spccmH_Id });
                    }
                });

                angular.forEach($scope.eventlist, function (evnt) {
                    if (evnt.select == true) {
                        $scope.selectedEventlist.push({ spccmE_Id: evnt.spccmE_Id });
                    }
                });

                angular.forEach($scope.sportslist, function (sprts) {
                    if (sprts.select == true) {
                        $scope.selectedSportslist.push({ spccmscC_Id: sprts.spccmscC_Id });
                    }
                });

                if ($scope.Type == 'House') {
                    var data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "ASMCL_Id": 0,
                        //"ASMS_Id": 0,
                        "Type": $scope.Type,
                        //"SPCCMH_Id": $scope.SPCCMH_Id,
                       // "SPCCME_Id": $scope.SPCCME_Id,
                       // "SPCCMSCC_Id": $scope.SPCCMSCC_Id,

                        selectedhouselist: $scope.selectedhouselist,
                        selectedeventlist: $scope.selectedEventlist,
                        selectedsportslist: $scope.selectedSportslist,
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
                        //"SPCCMH_Id": $scope.SPCCMH_Id,
                        //"SPCCME_Id": $scope.SPCCME_Id,
                        //"SPCCMSCC_Id": $scope.SPCCMSCC_Id,

                        selectedhouselist: $scope.selectedhouselist,
                        selectedSectionlist: $scope.selectedSectionlist,
                        selectedeventlist: $scope.selectedEventlist,
                        selectedsportslist: $scope.selectedSportslist,
                    }
                }


                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("SportTopperListReport/showdetails", data).
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
            //$scope.SPCCME_Id = "";
            //$scope.SPCCMH_Id = "";
            //$scope.Cumureport = false;
            //$scope.screport = false;
            //$scope.export = false;
            //$scope.submitted = false;
            //$scope.myForm.$setPristine();
            //$scope.myForm.$setUntouched();
            //$scope.SPCCMSCC_Id = "";

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
            debugger;
            if ($scope.Type == 'CS') {
                $scope.usercheck23 = false;
                $scope.usercheck = false;
                $scope.usercheckevent = false;
                $scope.userchecksports = false;
                $scope.ASMAY_Id = "";

                angular.forEach($scope.sportslist, function (tt) {
                    tt.select = false;
                })
                angular.forEach($scope.houseList, function (hs) {
                    hs.select = false;
                });
                angular.forEach($scope.sectionDropdown, function (hs) {
                    hs.select = false;
                });
                angular.forEach($scope.eventlist, function (hs) {
                    hs.select = false;
                });

                $scope.BindData();
            }
            else if ($scope.Type != 'CS') {
                $scope.usercheck23 = false;
                $scope.usercheck = false;
                $scope.usercheckevent = false;
                $scope.userchecksports = false;
                $scope.ASMAY_Id = "";

                angular.forEach($scope.sportslist, function (tt) {
                    tt.select = false;
                })
                angular.forEach($scope.sectionDropdown, function (hs) {
                    hs.select = false;
                });
                angular.forEach($scope.houseList, function (hs) {
                    hs.select = false;
                });
                angular.forEach($scope.eventlist, function (hs) {
                    hs.select = false;
                });

                $scope.BindData();
            }
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

        //=============================================================== For Section

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

        //===============================================================For Event

        $scope.searchchkbxevent = "";
        $scope.all_checkevent = function () {
            var checkStatus = $scope.usercheckevent;
            angular.forEach($scope.eventlist, function (itm) {
                itm.select = checkStatus;
            });
        }

        $scope.togchkbxevent = function () {
            $scope.usercheckevent = $scope.eventlist.every(function (options) {
                return options.select;
            });
        }

        $scope.isOptionsRequiredevent = function () {
            return !$scope.eventlist.some(function (options) {
                return options.select;
            });
        }

        $scope.eventfilterchkbx = function (obj) {
            return (angular.lowercase(obj.spccmE_EventName)).indexOf(angular.lowercase($scope.searchchkbxevent)) >= 0;
        }

        //=============================================================== For Sports

        $scope.searchchkbxsports = "";
        $scope.all_checksports = function () {
            var sprtscheckStatus = $scope.userchecksports;
            angular.forEach($scope.sportslist, function (sprts) {
                sprts.select = sprtscheckStatus;
            });
        }

        $scope.togchkbxsports = function () {
            $scope.userchecksports = $scope.sportslist.every(function (sprts) {
                return sprts.select;
            });
        }

        //$scope.isOptionsRequiredevent = function () {
        //    return !$scope.sportslist.some(function (options) {
        //        return options.select;
        //    });
        //}

        $scope.sportsfilterchkbx = function (sprts) {
            debugger;
            return (angular.lowercase(sprts.spccmscC_SportsCCName)).indexOf(angular.lowercase($scope.searchchkbxsports)) >= 0;
        }







    }

})();