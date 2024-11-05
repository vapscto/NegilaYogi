(function () {
    'use strict';

    angular
        .module('app')
        .controller('SRKVSSportsReportController', SRKVSSportsReportController);

    SRKVSSportsReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout'];

    function SRKVSSportsReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {
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
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        //$scope.imgname = logopath;
        $scope.searchchkbx234 = "";
        $scope.searchchkbx23 = "";
        $scope.imgname = "";
        $scope.fromdate = new Date();
        $scope.employeeid = [];
        $scope.Meet_Year = "";
        $scope.Meet_Year = "65";

        //============TO  GEt The Values iN Grid
        $scope.BindData = function () {
            apiService.getDATA("SRKVSReport/Getdetails").
                then(function (promise) {
                    $scope.yearlt = promise.yearlist;
                    $scope.categoryList = promise.categoryList;
                    $scope.CompetetionLevel = promise.competetionLevel;
                    $scope.MasterEvent = promise.masterEvent;
                    $scope.spcgrplist_new = promise.getMasterEvent;
                    $scope.ASMAY_Id = promise.asmaY_Id;
                })
        };
        $scope.all_check23 = function () {
            var checkStatus = $scope.obj.usercheck23;
            angular.forEach($scope.CompetetionLevel, function (itm) {
                itm.select = checkStatus;
            });
            $scope.employeeid = [];
        }

        //=================================Get Class

        //all_check2347
        $scope.all_check2347 = function () {
            var checkStatus = $scope.obj.usercheck2347;
            angular.forEach($scope.spcgrplist_newsub, function (itm) {
                itm.select = checkStatus;
            });
            $scope.employeeid = [];
        }
        $scope.isOptionsRequired234 = function () {
            return !$scope.categoryList.some(function (options) {
                return options.select;
            });
        }
        $scope.isOptionsRequired2347 = function () {
            return !$scope.spcgrplist_newsub.some(function (options) {
                return options.select;
            });
        }
        $scope.isOptionsRequired23 = function () {
            return !$scope.CompetetionLevel.some(function (options) {
                return options.select;
            });
        }
        //$scope.clscatId = 0;
        $scope.columnSort = false;


        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.get_class = function () {
            $scope.employeeid = [];
            $scope.GetReport = [];
            $scope.yearname = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,

            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("SRKVSReport/showdetails", data).
                then(function (promise) {

                    if ($scope.GetReport != null && $scope.GetReport.length > 0) {
                        $scope.GetReport = promise.getReport;
                        $scope.SPCCME_EventName = promise.name;
                    }
                    else {
                        swal("Record Not Found !");
                    }
                })
        }

        $scope.RepeatDta = function () {
            $scope.employeeid = [];
            $scope.categoryList = [];
            $scope.CompetetionLevel = [];
            $scope.MasterEvent = [];
            $scope.spcgrplist_new = [];
            $scope.sportsCCList = [];
            $scope.spcgrplist_newsub = [];

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,

            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("SRKVSReport/get_class", data).
                then(function (promise) {
                    $scope.categoryList = promise.categoryList;
                    $scope.CompetetionLevel = promise.competetionLevel;
                    $scope.MasterEvent = promise.masterEvent;
                    $scope.spcgrplist_new = promise.getMasterEvent;
                    $scope.sportsCCList = promise.sportsCCList;
                })
        }
        // TO Show The Data
        $scope.submitted = false;
        $scope.showdetails = function () {
            $scope.selectedhouselist = [];
            $scope.RomeLetters = [];
            $scope.selectedSectionlist = [];
            $scope.Categorylisttemp = [];
            $scope.CompetetionLeveltemp = [];
            $scope.GetReport = [];
            $scope.yearname = "";
            $scope.Sportleveltemp = [];
            $scope.GetReport = [];
            $scope.SubEventLists = [];
            $scope.employeeid = [];
            $scope.gettsreport = [];
            $scope.spccmcL_CompitionLevel = "";
            $scope.spccmcC_CompitionCategory = "";
            $scope.SPCCMSCCG_SportsCCGroupName = "";
            $scope.MainEvent = "";
            if ($scope.MasterEvent != null && $scope.MasterEvent.length > 0) {
                $scope.Sportleveltemp.push({
                    SPCCME_Id: $scope.SPCCME_Id
                })
            }
            if ($scope.spcgrplist_new != null && $scope.spcgrplist_new.length > 0) {
                angular.forEach($scope.spcgrplist_new, function (itm) {
                    if (itm.spccmsccG_Id == $scope.obj.spccmsccG_Id) {
                        $scope.MainEvent = itm.spccmsccG_SportsCCGroupName;
                    }
                });
            }
            if ($scope.categoryList != null && $scope.categoryList.length > 0) {
                if ($scope.Type == "Outdoor" || $scope.Type == "Team" || $scope.Type == "Externals" || $scope.Type == 'Finish' || $scope.Type == 'Selection') {
                    $scope.Categorylisttemp.push({
                        SPCCMCC_Id: $scope.obj.spccmcC_Id
                    })

                    //spccmcL_CompitionLevel
                }
                else {
                    angular.forEach($scope.categoryList, function (itm) {
                        if (itm.select === true) {
                            $scope.Categorylisttemp.push({
                                SPCCMCC_Id: itm.spccmcC_Id
                            })
                        }
                    });
                }

            }
            if ($scope.CompetetionLevel != null && $scope.CompetetionLevel.length > 0) {
                if ($scope.Type == 'Consolidated' || $scope.Type == 'Finish' || $scope.Type == 'Indoor' || $scope.Type == 'Outdoor' || $scope.Type == "Team" || $scope.Type == "Externals" || $scope.Type == 'Selection') {
                    $scope.CompetetionLeveltemp.push({
                        SPCCMCL_Id: $scope.obj.spccmcL_Id
                    })
                    angular.forEach($scope.CompetetionLevel, function (itm) {
                        if (itm.spccmcL_Id == $scope.obj.spccmcL_Id) {
                            $scope.spccmcL_CompitionLevel = itm.spccmcL_CompitionLevel;
                        }
                    });
                }
                else {
                    angular.forEach($scope.CompetetionLevel, function (itm) {
                        if (itm.select === true) {
                            $scope.CompetetionLeveltemp.push({
                                SPCCMCL_Id: itm.spccmcL_Id
                            })
                        }
                    });
                }

            }
            if ($scope.yearlt != null && $scope.yearlt.length > 0) {
                angular.forEach($scope.yearlt, function (itm) {
                    if (itm.asmaY_Id == $scope.ASMAY_Id) {
                        $scope.yearname = itm.asmaY_Year;
                    }

                });
            }
            //SubEventLists
            if ($scope.spcgrplist_newsub != null && $scope.spcgrplist_newsub.length > 0) {
                if ($scope.obj.spccmsccG_Idsub > 0) {
                    $scope.SubEventLists.push({
                        SPCCMSCCG_Id: $scope.obj.spccmsccG_Idsub
                    })
                }
                else {
                    angular.forEach($scope.spcgrplist_newsub, function (itm) {

                        if (itm.select === true) {
                            $scope.SubEventLists.push({
                                SPCCMSCCG_Id: itm.spccmsccG_Id
                            })
                        }
                    });
                }
            }


            $scope.submitted = true;
            $scope.SPCCSTMR_StudentName = "";
            $scope.SPCCEST_Record = "";
            $scope.SPCCMUOM_UOMName = "";
            $scope.SPCCSTMR_AcademicYear = "";
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "Type": $scope.Type,
                    "Categorylists": $scope.Categorylisttemp,
                    "CompetetionLevels": $scope.CompetetionLeveltemp,
                    "Sportleveltemps": $scope.Sportleveltemp,
                    "SPCCME_Id": Number($scope.SPCCME_Id),
                    // "SPCCMSCCG_Id": $scope.spccmsccG_Idsub,
                    "SubEventLists": $scope.SubEventLists

                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("SRKVSReport/showdetails", data).
                    then(function (promise) {
                        $scope.imgname = promise.logo;
                        if (promise.getReport != null && promise.getReport.length > 0) {
                            if ($scope.Type == "Consolidated" || $scope.Type == "Indoor" || $scope.Type == 'Outdoor' || $scope.Type == "Team" || $scope.Type == "Externals") {
                                $scope.GetReport = promise.getReport;
                                $scope.SPCCME_EventName = promise.name;
                                $scope.SPCCMCC_CompitionCategory = $scope.GetReport[0].SPCCMCC_CompitionCategory;
                                $scope.SPCCMSCCG_SportsCCGroupName = $scope.GetReport[0].SPCCMSCCG_SportsCCGroupName;
                                $scope.SPCCEST_Remarks = $scope.GetReport[0].SPCCEST_Remarks;
                                $scope.SPCCMUOM_UOMName = "";
                              
                               


                                $scope.employeeid = [];
                                if ($scope.Type == "Consolidated") {
                                    for (var i = 0; i < $scope.GetReport.length; i++) {
                                        $scope.RomeLetter = "";
                                        var num = $scope.GetReport[i].SPCCESTR_Rank;
                                        $scope.RomoenConvert(num);
                                        $scope.GetReport[i].SPCCESTR_Rank = $scope.RomeLetter;
                                    }

                                }
                                angular.forEach($scope.GetReport, function (dev) {
                                    if ($scope.employeeid.length === 0) {
                                        $scope.employeeid.push({
                                            AMST_Id: dev.AMST_Id, SPCCMCL_Id: dev.SPCCMCL_Id, SPCCESTR_Rank: dev.SPCCESTR_Rank, AMST_AdmNo: dev.AMST_AdmNo, AMST_FirstName: dev.AMST_FirstName
                                            , ASMCL_ClassName: dev.ASMCL_ClassName, ASMC_SectionName: dev.ASMC_SectionName, SPCCMSCCG_Id: dev.SPCCMSCCG_Id, SPCCMSCCG_SportsCCGroupName: dev.SPCCMSCCG_SportsCCGroupName, SPCCEST_Remarks: dev.SPCCEST_Remarks, SPCCMUOM_UOMName: dev.SPCCMUOM_UOMName
                                        });
                                    } else if ($scope.employeeid.length > 0) {
                                        var intcount = 0;
                                        angular.forEach($scope.employeeid, function (emp) {
                                            if (emp.AMST_Id === dev.AMST_Id && emp.SPCCMSCCG_Id == dev.SPCCMSCCG_Id) {
                                                intcount += 1;
                                            }
                                        });
                                        if (intcount === 0) {
                                            $scope.employeeid.push({
                                                AMST_Id: dev.AMST_Id, SPCCMCL_Id: dev.SPCCMCL_Id, SPCCESTR_Rank: dev.SPCCESTR_Rank, AMST_AdmNo: dev.AMST_AdmNo, AMST_FirstName: dev.AMST_FirstName
                                                , ASMCL_ClassName: dev.ASMCL_ClassName, ASMC_SectionName: dev.ASMC_SectionName, SPCCMSCCG_Id: dev.SPCCMSCCG_Id, SPCCMSCCG_SportsCCGroupName: dev.SPCCMSCCG_SportsCCGroupName, SPCCEST_Remarks: dev.SPCCEST_Remarks, SPCCMUOM_UOMName: dev.SPCCMUOM_UOMName
                                            });
                                        }
                                    }

                                });
                                angular.forEach($scope.employeeid, function (ddd) {
                                    $scope.templist = [];
                                    var sum_deviation = 0;
                                    angular.forEach($scope.GetReport, function (dd) {
                                        if (dd.AMST_Id === ddd.AMST_Id && dd.SPCCMSCCG_Id == ddd.SPCCMSCCG_Id) {
                                            $scope.templist.push(dd);
                                        }
                                    });
                                    ddd.RecordDetails = $scope.templist;
                                });


                                $scope.employeeid1 = [];

                                angular.forEach($scope.employeeid, function (dev) {

                                    if ($scope.employeeid1.length === 0) {
                                        $scope.employeeid1.push({
                                            RecordDetails: dev.RecordDetails,
                                            SPCCMSCCG_Id: dev.SPCCMSCCG_Id,
                                            SPCCMSCCG_SportsCCGroupName: dev.SPCCMSCCG_SportsCCGroupName,
                                            SPCCEST_Remarks: dev.SPCCEST_Remarks,
                                            SPCCMCL_Id: dev.SPCCMCL_Id
                                        });
                                    } else if ($scope.employeeid1.length > 0) {
                                        var intcount = 0;
                                        angular.forEach($scope.employeeid1, function (emp) {
                                            if (emp.SPCCMSCCG_Id == dev.SPCCMSCCG_Id) {
                                                intcount += 1;
                                            }
                                        });
                                        if (intcount === 0) {
                                            $scope.employeeid1.push({
                                                RecordDetails: dev.RecordDetails,
                                                SPCCMSCCG_Id: dev.SPCCMSCCG_Id,
                                                SPCCMSCCG_SportsCCGroupName: dev.SPCCMSCCG_SportsCCGroupName,
                                                SPCCEST_Remarks: dev.SPCCEST_Remarks,
                                                SPCCMCL_Id: dev.SPCCMCL_Id
                                            });
                                        }
                                    }

                                });


                                angular.forEach($scope.employeeid1, function (ddd) {
                                    $scope.templist1 = [];
                                    angular.forEach($scope.employeeid, function (dd) {
                                        if (dd.SPCCMSCCG_Id == ddd.SPCCMSCCG_Id) {
                                            $scope.templist1.push(dd);
                                        }
                                    });
                                    ddd.RecordDetails1 = $scope.templist1;
                                });

                                $scope.gettsreport = promise.gettsreport;
                                console.log($scope.employeeid);


                            }
                            else if ($scope.Type == "Finish") {
                                $scope.GetReport = promise.getReport;
                                $scope.getReportfinish = promise.getReportfinish;
                                $scope.SPCCME_EventName = promise.name;
                                $scope.SPCCMSCC_NoOfAttempts = "";
                                $scope.SPCCMUOM_UOMName = "";
                                $scope.SPCCMCC_CompitionCategory = $scope.GetReport[0].SPCCMCC_CompitionCategory;
                                $scope.SPCCMSCCG_SportsCCGroupName = $scope.GetReport[0].SPCCMSCCG_SportsCCGroupName;
                                $scope.SPCCMUOM_UOMName = $scope.GetReport[0].SPCCMUOM_UOMName;
                                $scope.employeeid = [];
                                angular.forEach($scope.GetReport, function (dev) {
                                    if ($scope.employeeid.length === 0) {
                                        $scope.employeeid.push({
                                            SPCCMCC_Id: dev.SPCCMCC_Id,
                                            SPCCMCL_Id: dev.SPCCMCL_Id,
                                            SPCCMSCCG_Id: dev.SPCCMSCCG_Id,
                                            SPCCMCC_CompitionCategory: dev.SPCCMCC_CompitionCategory,
                                            SPCCMSCC_SportsCCName: dev.SPCCMSCC_SportsCCName,
                                            AMST_FirstName: dev.AMST_FirstName,
                                            SPCCEST_Record: dev.SPCCEST_Record,
                                            SPCCMSCCG_SportsCCGroupName: dev.SPCCMSCCG_SportsCCGroupName,
                                            SPCCESTR_Rank: dev.SPCCESTR_Rank,
                                            SPCCMUOM_UOMName: dev.SPCCMUOM_UOMName,
                                            AMST_Id: dev.AMST_Id,
                                            SPCCESTR_Value: dev.SPCCESTR_Value,
                                            SPCCEST_Remarks: dev.SPCCEST_Remarks,
                                        });
                                    }

                                    else if ($scope.employeeid.length > 0) {
                                        var intcount = 0;
                                        angular.forEach($scope.employeeid, function (emp) {
                                            if (emp.SPCCMCL_Id === dev.SPCCMCL_Id && emp.SPCCMCC_Id == dev.SPCCMCC_Id && emp.SPCCMSCCG_Id == dev.SPCCMSCCG_Id
                                            ) {
                                                intcount += 1;
                                            }
                                        });
                                        if (intcount === 0) {
                                            $scope.employeeid.push({
                                                SPCCMCC_Id: dev.SPCCMCC_Id,
                                                SPCCMCL_Id: dev.SPCCMCL_Id,
                                                SPCCMSCCG_Id: dev.SPCCMSCCG_Id,
                                                SPCCMCC_CompitionCategory: dev.SPCCMCC_CompitionCategory,
                                                SPCCMSCC_SportsCCName: dev.SPCCMSCC_SportsCCName,
                                                AMST_FirstName: dev.AMST_FirstName,
                                                SPCCEST_Record: dev.SPCCEST_Record,
                                                SPCCMSCCG_SportsCCGroupName: dev.SPCCMSCCG_SportsCCGroupName,
                                                SPCCESTR_Rank: dev.SPCCESTR_Rank,
                                                SPCCMUOM_UOMName: dev.SPCCMUOM_UOMName,
                                                AMST_Id: dev.AMST_Id,
                                                SPCCEST_Remarks: dev.SPCCEST_Remarks,
                                                SPCCESTR_Value: dev.SPCCESTR_Value,
                                            });
                                        }
                                    }
                                });
                                console.log($scope.employeeid);
                                angular.forEach($scope.employeeid, function (ddd) {
                                    $scope.templist = [];
                                    angular.forEach($scope.GetReport, function (dd) {
                                        if (dd.SPCCMCC_Id === ddd.SPCCMCC_Id && dd.SPCCMCL_Id === ddd.SPCCMCL_Id
                                            && dd.SPCCMSCCG_Id == ddd.SPCCMSCCG_Id) {

                                            $scope.templist.push(dd);
                                        }
                                    });
                                    ddd.RecordDetails = $scope.templist;

                                });




                                $scope.gettsreport = promise.gettsreport;
                                console.log($scope.employeeid);
                                $scope.SPCCMSCC_NoOfAttempts = $scope.GetReport[0].SPCCMSCC_NoOfAttempts;
                                if ($scope.SPCCMSCC_NoOfAttempts > 0) {
                                    for (var i = 0; i < $scope.SPCCMSCC_NoOfAttempts; i++) {
                                        $scope.RomeLetter = "";
                                        var num = i + 1;
                                        $scope.RomoenConvert(num);
                                        $scope.RomeLetters.push({
                                            RomeLetter: $scope.RomeLetter,
                                            Value: num
                                        })
                                    }

                                }
                            }
                            else if ($scope.Type == "Selection") {
                                $scope.GetReport = promise.getReport;
                                $scope.SPCCME_EventName = promise.name;
                                $scope.SPCCMCC_CompitionCategory = $scope.GetReport[0].SPCCMCC_CompitionCategory;
                                $scope.SPCCMSCCG_SportsCCGroupName = $scope.GetReport[0].SPCCMSCCG_SportsCCGroupName;
                                $scope.employeeid = [];

                                angular.forEach($scope.GetReport, function (dev) {
                                    if ($scope.employeeid.length === 0) {
                                        $scope.employeeid.push({
                                            SPCCMCC_Id: dev.SPCCMCC_Id,
                                            SPCCMCL_Id: dev.SPCCMCL_Id,
                                            SPCCMSCCG_Id: dev.SPCCMSCCG_Id,
                                            SPCCMCC_CompitionCategory: dev.SPCCMCC_CompitionCategory,
                                            SPCCMSCC_SportsCCName: dev.SPCCMSCC_SportsCCName,
                                            AMST_FirstName: dev.AMST_FirstName,
                                            SPCCEST_Record: dev.SPCCEST_Record,
                                            SPCCMSCCG_SportsCCGroupName: dev.SPCCMSCCG_SportsCCGroupName,
                                            SPCCESTR_Rank: dev.SPCCESTR_Rank,
                                            SPCCMUOM_UOMName: dev.SPCCMUOM_UOMName,
                                            SPCCEST_Id: dev.SPCCEST_Id,

                                        });
                                    }
                                    else if ($scope.employeeid.length > 0) {
                                        var intcount = 0;
                                        angular.forEach($scope.employeeid, function (emp) {
                                            if (emp.SPCCMCL_Id === dev.SPCCMCL_Id && emp.SPCCMCC_Id == dev.SPCCMCC_Id && emp.SPCCMSCCG_Id == dev.SPCCMSCCG_Id && dev.SPCCEST_Id == emp.SPCCEST_Id) {
                                                intcount += 1;
                                            }
                                        });
                                        if (intcount === 0) {
                                            $scope.employeeid.push({
                                                SPCCMCC_Id: dev.SPCCMCC_Id,
                                                SPCCMCL_Id: dev.SPCCMCL_Id,
                                                SPCCMSCCG_Id: dev.SPCCMSCCG_Id,
                                                SPCCMCC_CompitionCategory: dev.SPCCMCC_CompitionCategory,
                                                SPCCMSCC_SportsCCName: dev.SPCCMSCC_SportsCCName,
                                                AMST_FirstName: dev.AMST_FirstName,
                                                SPCCEST_Record: dev.SPCCEST_Record,
                                                SPCCMSCCG_SportsCCGroupName: dev.SPCCMSCCG_SportsCCGroupName,
                                                SPCCESTR_Rank: dev.SPCCESTR_Rank,
                                                SPCCMUOM_UOMName: dev.SPCCMUOM_UOMName,
                                                SPCCEST_Id: dev.SPCCEST_Id,
                                                AMST_Id: dev.AMST_Id,
                                            });
                                        }
                                    }
                                });
                                console.log($scope.employeeid);
                                angular.forEach($scope.employeeid, function (ddd) {
                                    $scope.templist = [];

                                    angular.forEach($scope.GetReport, function (dd) {
                                        if (dd.SPCCMCC_Id === ddd.SPCCMCC_Id && dd.SPCCMCL_Id === ddd.SPCCMCL_Id && dd.SPCCMSCCG_Id == ddd.SPCCMSCCG_Id && dd.SPCCEST_Id == ddd.SPCCEST_Id) {

                                            $scope.templist.push(dd);
                                        }
                                    });
                                    ddd.RecordDetails = $scope.templist;

                                });
                                //
                                $scope.gettsreport = promise.gettsreport;
                                console.log($scope.employeeid);
                            }
                            else {
                                $scope.employeeid = [];
                                $scope.GetReporttemp = [];
                                $scope.GetReporttemp = promise.getReport;
                                $scope.SPCCMUOM_UOMName = promise.getReport[0].SPCCMUOM_UOMName;
                                // $scope.GetReport = promise.getReport;
                                if ($scope.Type == "Record") {
                                    angular.forEach(promise.getReport, function (dd) {
                                        if (dd.AMST_FirstName != null && dd.AMST_FirstName != "") {
                                            $scope.GetReport.push({
                                                AMST_FirstName: dd.AMST_FirstName,
                                                ASMAY_Year: dd.ASMAY_Year,
                                                SPCCME_EventName: dd.SPCCME_EventName,
                                                SPCCMCC_CompitionCategory: dd.SPCCMCC_CompitionCategory,
                                                SPCCMCL_CompitionLevel: dd.SPCCMCL_CompitionLevel,
                                                SPCCMSCC_SportsCCName: dd.SPCCMSCC_SportsCCName,
                                                SPCCMUOM_UOMName: dd.SPCCMUOM_UOMName,
                                                SPCCMSCCG_SportsCCGroupName: dd.SPCCMSCCG_SportsCCGroupName,
                                                AMAY_RollNo: dd.AMAY_RollNo,
                                                SPCCESTR_Value: dd.SPCCEST_Record,
                                                SPCCSTMR_Id: dd.SPCCSTMR_Id,
                                                SPCCMCC_Id: dd.SPCCMCC_Id,
                                                SPCCMSCCG_Id: dd.SPCCMSCCG_Id,
                                                SPCCMUOM_UOMName: dd.SPCCMUOM_UOMName,
                                            })
                                        }
                                        else {
                                            $scope.GetReport.push({
                                                AMST_FirstName: dd.SPCCSTMR_StudentName,
                                                ASMAY_Year: dd.SPCCSTMR_AcademicYear,
                                                SPCCME_EventName: dd.SPCCME_EventName,
                                                SPCCMCC_CompitionCategory: dd.SPCCMCC_CompitionCategory,
                                                SPCCMCL_CompitionLevel: dd.SPCCMCL_CompitionLevel,
                                                SPCCMSCC_SportsCCName: dd.SPCCMSCC_SportsCCName,
                                                SPCCMUOM_UOMName: dd.SPCCMUOM_UOMName,
                                                SPCCMSCCG_SportsCCGroupName: dd.SPCCMSCCG_SportsCCGroupName,
                                                AMAY_RollNo: dd.AMAY_RollNo,
                                                SPCCESTR_Value: dd.SPCCEST_Record,
                                                SPCCSTMR_Id: dd.SPCCSTMR_Id,
                                                SPCCMCC_Id: dd.SPCCMCC_Id,
                                                SPCCMSCCG_Id: dd.SPCCMSCCG_Id,
                                                SPCCMUOM_UOMName: dd.SPCCMUOM_UOMName,
                                            })
                                        }
                                    });
                                    //SPCCMUOM_UOMName 
                                }
                                else {
                                    $scope.GetReport = promise.getReport;
                                }
                                if ($scope.GetReport[0].SPCCMSCC_NoOfAttempts > 0) {
                                    $scope.SPCCMSCC_NoOfAttempts = $scope.GetReport[0].SPCCMSCC_NoOfAttempts;
                                    for (var i = 0; i < $scope.SPCCMSCC_NoOfAttempts; i++) {
                                        $scope.RomeLetter = "";
                                        var num = i + 1;
                                        $scope.RomoenConvert(num);
                                        $scope.RomeLetters.push({
                                            RomeLetter: $scope.RomeLetter,
                                            Value: num
                                        })
                                    }

                                }


                                $scope.SPCCME_EventName = promise.name;

                                // $scope.SPCCSTMR_StudentName = promise.gettsreport[0].SPCCSTMR_StudentName;
                                // $scope.SPCCEST_Record = promise.gettsreport[0].SPCCESTR_Value;
                                // $scope.SPCCSTMR_AcademicYear = promise.gettsreport[0].SPCCSTMR_AcademicYear;

                                angular.forEach($scope.GetReport, function (dev) {
                                    if ($scope.employeeid.length === 0 && dev.Record != "Broken") {
                                        $scope.employeeid.push({
                                            SPCCMCC_Id: dev.SPCCMCC_Id,
                                            SPCCMCC_CompitionCategory: dev.SPCCMCC_CompitionCategory,
                                            SPCCMSCC_SportsCCName: dev.SPCCMSCC_SportsCCName,
                                            AMST_FirstName: dev.AMST_FirstName,
                                            SPCCEST_Record: dev.SPCCEST_Record,
                                            SPCCMSCCG_SportsCCGroupName: dev.SPCCMSCCG_SportsCCGroupName,
                                            SPCCMUOM_UOMName: dev.SPCCMUOM_UOMName,
                                            ASMCL_ClassName: dev.ASMCL_ClassName,
                                            ASMC_SectionName: dev.ASMC_SectionName,

                                        });
                                    } else if ($scope.employeeid.length > 0 && dev.Record != "Broken") {
                                        var intcount = 0;
                                        angular.forEach($scope.employeeid, function (emp) {
                                            if (emp.SPCCMCC_Id === dev.SPCCMCC_Id) {
                                                intcount += 1;
                                            }
                                        });
                                        if (intcount === 0) {
                                            $scope.employeeid.push({
                                                SPCCMCC_Id: dev.SPCCMCC_Id,
                                                SPCCMCC_CompitionCategory: dev.SPCCMCC_CompitionCategory,
                                                SPCCMSCC_SportsCCName: dev.SPCCMSCC_SportsCCName,
                                                AMST_FirstName: dev.AMST_FirstName,
                                                SPCCEST_Record: dev.SPCCEST_Record,
                                                SPCCMSCCG_SportsCCGroupName: dev.SPCCMSCCG_SportsCCGroupName,
                                                SPCCMUOM_UOMName: dev.SPCCMUOM_UOMName,
                                                ASMCL_ClassName: dev.ASMCL_ClassName,
                                                ASMC_SectionName: dev.ASMC_SectionName,
                                            });
                                        }
                                    }
                                });


                                console.log($scope.employeeid);

                                angular.forEach($scope.employeeid, function (ddd) {
                                    $scope.templist = [];

                                    angular.forEach($scope.GetReport, function (dd) {
                                        if (dd.SPCCMCC_Id === ddd.SPCCMCC_Id) {

                                            $scope.templist.push(dd);
                                        }
                                    });
                                    ddd.RecordDetails = $scope.templist;

                                });

                                //added By sanjeev
                                if (promise.gettsreport[0].SPCCMCC_CompitionCategory != null && promise.gettsreport[0].SPCCMCC_CompitionCategory != "") {
                                    $scope.SPCCMCC_CompitionCategory = promise.gettsreport[0].SPCCMCC_CompitionCategory;
                                }
                                if (promise.gettsreport[0].SPCCSTMR_StudentName != null && promise.gettsreport[0].SPCCSTMR_StudentName != "") {
                                    $scope.SPCCSTMR_StudentName = promise.gettsreport[0].SPCCSTMR_StudentName;
                                }
                                if (promise.gettsreport[0].SPCCESTR_Value != null && promise.gettsreport[0].SPCCESTR_Value != "") {
                                    $scope.SPCCEST_Record = promise.gettsreport[0].SPCCESTR_Value;
                                }
                                if (promise.gettsreport[0].SPCCSTMR_AcademicYear != null && promise.gettsreport[0].SPCCSTMR_AcademicYear != "") {
                                    $scope.SPCCSTMR_AcademicYear = promise.gettsreport[0].SPCCSTMR_AcademicYear;
                                }
                                console.log($scope.employeeid);
                            }


                        }
                        else {
                            swal("Record Not Found !");
                        }
                    })
            }
        };
        $scope.printToCart = function () {
            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/EMPPFSchemePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.cancel = function () {

            $state.reload();
        }



        $scope.exportToExcel = function (table) {
            debugger;
            var exportHref = Excel.tableToExcel(table, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);

        }



        //===========================================Radio


        //////////=========================================================
        $scope.searchchkbx = "";
        $scope.all_check = function () {
            var checkStatus = $scope.usercheck;
            angular.forEach($scope.houseList, function (itm) {
                itm.select = checkStatus;
            });
            $scope.employeeid = [];
        }



        $scope.isOptionsRequired = function () {
            return !$scope.houseList.some(function (options) {
                return options.select;
            });
        }





        $scope.searchchkbx234 = "";
        $scope.all_check234 = function () {
            var checkStatus = $scope.obj.usercheck234;
            angular.forEach($scope.categoryList, function (itm) {
                itm.select = checkStatus;
            });
            $scope.employeeid = [];
        }


        $scope.isOptionsRequired23 = function () {
            return !$scope.CompetetionLevel.some(function (options) {
                return options.select;
            });
        }





        $scope.get_SportsName = function () {
            $scope.spcgrplist_newsub = [];
            $scope.sportsCCList = [];
            $scope.spccmsccG_Idsub = "";
            $scope.obj.spccmscC_Id = "";
            $scope.employeeid = [];
            var obj = {
                "SPCCMSCCG_Id": $scope.obj.spccmsccG_Id,
                //"ASMAY_Id": $scope.asmaY_Id,
            }
            apiService.create("EventsStudentRecord/get_SportsName", obj).
                then(function (promise) {

                    $scope.sportsCCList = promise.sportsCCList;
                });
        }
        $scope.get_uom_Name = function () {
            $scope.spcgrplist_newsub = [];
            $scope.employeeid = [];
            $scope.obj.spccmsccG_Idsub = "";
            var obj = {
                "SPCCMSCC_Id": $scope.obj.spccmscC_Id,
                "SPCCMSCCG_Id": $scope.obj.spccmsccG_Id,
            }
            apiService.create("EventsStudentRecord/get_uom_Name", obj).
                then(function (promise) {

                    $scope.uomList = promise.uomList;
                    $scope.spcgrplist_newsub = promise.getsubsubevent;
                });
        }

        $scope.RomoenConvert = function (num) {
            $scope.RomeLetter = "";
            var roman = {
                I: 1,
                II: 2,
                III: 3,
                IV: 4,
                V: 5,
                VI: 6,
                VII: 7,
                VIII: 8,
                IX: 9,
                X: 10,
                XI: 11,
                XII: 12,
                XIII: 13,
                XIV: 14,
                XV: 15,
                XVI: 16,
                XVII: 17,
                XVIII: 18,
                XIX: 19,
                XX: 20,
                XXI: 21,
                XXII: 22,
                XXIII: 23,
                XXIV: 24,
                XXV: 25,
                XXVI: 26,
                XXVII: 27,
                XXVIII: 28,
                XXIX: 29,
                XXX: 30,
            };
            var str = '';

            for (var i of Object.keys(roman)) {
                var q = Math.floor(num / roman[i]);
                num -= q * roman[i];
                str += i.repeat(q);
            }
            $scope.RomeLetter = str;
        }
    }
})();