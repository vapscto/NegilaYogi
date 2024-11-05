
(function () {
    'use strict';
    angular
        .module('app')
        .controller('SportPointsAndRecordReportController', SportPointsAndRecordReportController)

    SportPointsAndRecordReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache']
    function SportPointsAndRecordReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache) {


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

            apiService.getDATA("SportPointsAndRecordReport/Getdetails").
                then(function (promise) {
                    debugger;

                    $scope.yearlt = promise.yearlist;
                    $scope.sportslist = promise.sportslist;
                    $scope.categoryList = promise.categoryList;

                    $scope.ranklist = [
                        { spccestR_Rank: '1', name: '1' },
                        { spccestR_Rank: '2', name: '2' },
                        { spccestR_Rank: '3', name: '3' },
                        { spccestR_Rank: '4', name: '4' },
                        { spccestR_Rank: '5', name: '5' },
                        { spccestR_Rank: '6', name: '6' },
                        { spccestR_Rank: '7', name: '7' },
                        { spccestR_Rank: '8', name: '8' },
                        { spccestR_Rank: '9', name: '9' },
                        { spccestR_Rank: '10', name: '10' },
                    ];



                })
        };


        //=================================Get Class
        $scope.get_class = function () {
            debugger;
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
            apiService.create("SportPointsAndRecordReport/get_class", data)
                .then(function (promise) {
                    $scope.classDropdown = promise.classList;
                    $scope.houseList = promise.houseList;
                    $scope.eventlist = promise.eventList;
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
            apiService.create("SportPointsAndRecordReport/get_section", data)
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
            $scope.placelist = [];
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
                angular.forEach($scope.ranklist, function (sprts) {
                    if (sprts.selectedplace == true) {
                        $scope.placelist.push({ spccestR_Rank: sprts.spccestR_Rank });
                    }
                });

                if ($scope.Type == 'House') {
                    var data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        //"ASMCL_Id": 0,
                        "Type": $scope.Type,
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
                        "Type": $scope.Type,
                        selectedhouselist: $scope.selectedhouselist,
                        selectedSectionlist: $scope.selectedSectionlist,
                        selectedeventlist: $scope.selectedEventlist,
                        selectedsportslist: $scope.selectedSportslist,

                    }
                }
                else if ($scope.Type == 'CC') {
                    var data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "Type": $scope.Type,
                        "SPCCMCC_Id": $scope.spccmcC_Id,
                        selectedhouselist: $scope.selectedhouselist,
                        selectedeventlist: $scope.selectedEventlist,
                        selectedsportslist: $scope.selectedSportslist,
                        placListData: $scope.placelist,

                    }

                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("SportPointsAndRecordReport/showdetails", data).
                    then(function (promise) {

                        if ($scope.Type == 'CC') {
                            $scope.viewlistCCwise = promise.viewlistCCwise;
                            if ($scope.viewlistCCwise.length > 0) {
                                angular.forEach($scope.yearlt, function (fff) {
                                    if (fff.asmaY_Id == $scope.ASMAY_Id) {
                                        $scope.yearname = fff.asmaY_Year;
                                    }
                                })
                                angular.forEach($scope.categoryList, function (cat) {
                                    if (cat.spccmcC_Id == $scope.spccmcC_Id) {
                                        $scope.categoryname = cat.spccmcC_CompitionCategory;

                                    }
                                })
                                var eventt_list = [];
                                angular.forEach($scope.viewlistCCwise, function (main) {
                                    if (eventt_list.length == 0) {
                                        eventt_list.push({ SPCCME_Id: main.SPCCME_Id, SPCCME_EventName: main.SPCCME_EventName });
                                    }
                                    else if (eventt_list.length > 0) {
                                        var sssubj_cnt = 0;
                                        angular.forEach(eventt_list, function (ssub) {
                                            if (ssub.SPCCME_Id == main.SPCCME_Id) {
                                                sssubj_cnt += 1;
                                            }
                                        })
                                        if (sssubj_cnt == 0) {
                                            eventt_list.push({ SPCCME_Id: main.SPCCME_Id, SPCCME_EventName: main.SPCCME_EventName });
                                        }
                                    }
                                })

                                console.log("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
                                console.log(eventt_list);
                                $scope.mainlist = [];
                                var count = 0;
                                angular.forEach(eventt_list, function (evnt) {
                                    $scope.listofdata = [];
                                    count += 1,
                                        angular.forEach($scope.viewlistCCwise, function (std) {
                                            if (evnt.SPCCME_Id == std.SPCCME_Id) {
                                                $scope.listofdata.push({
                                                    AMST_Id: std.AMST_Id, AMST_Name: std.AMST_Name, ASMCL_ClassName: std.ASMCL_ClassName, ASMC_SectionName: std.ASMC_SectionName, AMST_AdmNo: std.AMST_AdmNo, SPCCESTR_Rank: std.SPCCESTR_Rank, SPCCESTR_Points: std.SPCCESTR_Points, SPCCME_EventName: std.SPCCME_EventName, SPCCMEV_EventVenue: std.SPCCMEV_EventVenue, SPCCMH_HouseName: std.SPCCMH_HouseName, ASMAY_Year: std.ASMAY_Year, SPCCESTR_Remarks: std.SPCCESTR_Remarks, SPCCE_StartDate: std.SPCCE_StartDate
                                                    //SPCCMSCC_SportsCCName: std.SPCCMSCC_SportsCCName,
                                                })
                                            }
                                        })
                                    $scope.mainlist.push({
                                        SPCCME_Id: evnt.SPCCME_Id, SPCCME_EventName: evnt.SPCCME_EventName, studentlist: $scope.listofdata, rowlength: count, lentrow: $scope.listofdata.length
                                    })
                                })

                                console.log("#########################");
                                console.log($scope.mainlist);
                            }
                            else {
                                swal("No Records Found!");
                            }
                           
                        }

                        else {
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
                        }
                        
                       
                    })

            }
            else {
                $scope.submitted = true;
            } 
        };

        $scope.cancel = function () {

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
            var exportHref = Excel.tableToExcel(table, 'sheet spccestR_Rank');
            $timeout(function () { location.href = exportHref; }, 100);
        }

        $scope.Print23 = function () {
            
            var innerContents = document.getElementById("printCCwise").innerHTML;
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

        $scope.exportToExcel23 = function (table) {
            debugger;
            var exportHref = Excel.tableToExcel(table, 'sheet printCCwise');
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
            return (angular.lowercase(sprts.spccmscC_SportsCCName)).indexOf(angular.lowercase($scope.searchchkbxsports)) >= 0;
        }


        ////======================place
        $scope.all_checkPlace = function () {
            var checkStatusplace = $scope.usercheckPlace;
            angular.forEach($scope.ranklist, function (itm) {
                itm.selectedplace = checkStatusplace;
            });
        }

        $scope.togchkbx = function () {
            $scope.usercheckPlace = $scope.ranklist.every(function (options) {
                return options.selectedplace;
            });
        }




    }

})();