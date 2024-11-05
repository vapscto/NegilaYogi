
(function () {
    'use strict';
    angular
        .module('app')
        .controller('CirculationReportController', BookCirculationReportController)

    BookCirculationReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$filter', 'Excel', '$timeout']
    function BookCirculationReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $filter, Excel, $timeout) {

        $scope.submitted = false;

        $scope.boxfalg = false;
        // $scope.statusdata = false;
        //  $scope.minDate = new Date();
        $scope.printflag = false;
        $scope.obj = {};
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

        $scope.sublistone = [];

        //---------------Load --data
        $scope.loaddata = function () {
            
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            
            //var pageid = 2;
            apiService.getURI("BookCirculationReport/getdetails", 2).then(function (promise) {
                $scope.booktitle = promise.booktitle;
                $scope.classList = promise.classList;
                $scope.sectionList = promise.sectionList;
                $scope.usercheck = true;
                $scope.usercheck23 = true;

                $scope.booktype = promise.booktype;

                $scope.booklist = promise.booklist;

                $scope.parentsubjectlist = promise.parentsubjectlist;

               // $scope.subsubjectlist = promise.subsubjectlist;

                $scope.lib_list = promise.lib_list;

                $scope.LMAL_Id = promise.lmaL_Id;
                angular.forEach($scope.classList, function (itm) {
                    itm.select = true;
                });
                angular.forEach($scope.sectionList, function (itm) {
                    itm.select = true;
                });
                $scope.temptable = promise.subsubjectlist;

                if (promise.subsubjectlist != null && promise.subsubjectlist.length > 0) {
                    angular.forEach(promise.subsubjectlist, function (itm1) {
                        if (itm1.lmS_Level == '1') {
                            $scope.sublistone.push({
                                LMS_SubjectName: itm1.lmS_SubjectName,
                                LMS_SubjectNo: itm1.lmS_SubjectNo,
                                LMS_Id: itm1.lmS_ParentId,
                            })
                        }
                    })
                    $scope.sublisbackone = $scope.sublistone;
                }
            })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }
        //--------------End-Load --data


        $scope.Type = "all";
        $scope.AGType = "Student";
        $scope.TrnType = "all";

        //---------------Get-Report
        $scope.allgrid = false;
        $scope.selectedClasslist = [];
        $scope.selectedSectionlist = [];
        $scope.get_report = function () {
            $scope.selectedClasslist = [];
            $scope.selectedSectionlist = [];
            $scope.stthomosSubject = [];
            if ($scope.sublist != null && $scope.sublist.length > 0) {
                angular.forEach($scope.sublist, function (dd) {
                    if (dd.lmS_Ids == true) {
                        $scope.stthomosSubject.push({
                           
                            LMS_Id: dd.lmS_Id,

                        })
                    }
                });
            }
            else {
                $scope.stthomosSubject = $scope.temptable;
            }
            $scope.allgrid = false;

            if (($scope.IssueFromDate == undefined && $scope.IssueFromDate == null) || ($scope.IssueToDate == undefined && $scope.IssueToDate == null)) {
                //$scope.IssueFromDate = "";
                //$scope.IssueToDate = "";
            }
            if (($scope.DueFromdate == undefined && $scope.DueFromdate == null) || ($scope.DueTodate == undefined && $scope.DueTodate == null)) {
                //$scope.DueFromdate = "";
                //$scope.DueTodate = "";
            }
            var fromdate1 = $scope.IssueFromDate == null ? "" : $filter('date')($scope.IssueFromDate, "yyyy-MM-dd");
            var todate1 = $scope.IssueToDate == null ? "" : $filter('date')($scope.IssueToDate, "yyyy-MM-dd");
            var fromdate2 = $scope.DueFromdate == null ? "" : $filter('date')($scope.DueFromdate, "yyyy-MM-dd");
            var todate2 = $scope.DueTodate == null ? "" : $filter('date')($scope.DueTodate, "yyyy-MM-dd");

            if ($scope.myForm.$valid) {
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
                    "Type": $scope.Type,
                    "AGType": $scope.AGType,
                    "TrnType": $scope.TrnType,
                    "IssueFromDate": fromdate1,
                    "IssueToDate": todate1,
                    "DueFromdate": fromdate2,
                    "DueTodate": todate2,
                    "LMAL_Id": $scope.LMAL_Id,
                    selectedClasslist: $scope.selectedClasslist,
                    selectedSectionlist: $scope.selectedSectionlist,
                    "BookSummaryCircular": $scope.stthomosSubject,
                    "BookSummary": true,

                }
                apiService.create("BookCirculationReport/get_report", data)
                    .then(function (promise) {
                        if (promise.reportlist.length > 0) {
                            $scope.reportlist = promise.reportlist;
                            if ($scope.AGType == 'all') {
                                $scope.allgrid = true;
                            }
                            else if ($scope.AGType == 'Student') {
                                $scope.allgrid = true;
                            }
                            else if ($scope.AGType == 'Staff') {
                                $scope.allgrid = true;
                            }
                            else if ($scope.AGType == 'Guest') {
                                $scope.allgrid = true;
                            }
                            else if ($scope.AGType == 'Department') {
                                $scope.allgrid = true;
                            }
                        }
                        else {
                            swal('Record is not Available!!');
                            $state.reload();
                        }
                        //$scope.reportlist = promise.reportlist;

                        $scope.boxfalg = true;

                        $scope.printflag = true;
                    })
            }
            else {
                $scope.submitted = true;
            }

        }
        //-------------End--Get-Report


        //$scope.radioall = function (alldata) {
        //    
        //    $scope.book_Trans_Id = alldata[0].book_Trans_Id;
        //    if ($scope.book_Trans_Id != "" && $scope.book_Trans_Id != null && $scope.book_Trans_Id != undefined) {
        //        var data = {
        //            "Book_Trans_Id": $scope.book_Trans_Id
        //        }

        //        apiService.create("BookCirculationReport/radioall", data)
        //            .then(function (promise) {

        //                $scope.alldata = promise.alldata;
        //            })
        //    }
        //    else {
        //        swal('Id is not Available');
        //    }
        //}


        //$scope.getstuddetails = function () {
        //    

        //    if ($scope.LMB_BookType != undefined && $scope.LMB_BookType != "") {
        //        var data = {
        //            "LMB_BookType": $scope.LMB_BookType
        //        }

        //        apiService.create("BookCirculationReport/getstuddetails", data)
        //            .then(function (promise) {
        //                $scope.alldata = promise.alldata;


        //              //  $scope.statusdata = true;
        //            })
        //    }
        //    else {
        //        swal('Id is not Available');
        //    }
        //};



        //===========print===========//
        $scope.printData = function () {
            var innerContents = '';
            if ($scope.AGType == 'Student') {
                innerContents = document.getElementById("printtable1").innerHTML;
            }
            else if ($scope.AGType == 'Staff') {
                innerContents = document.getElementById("printtable2").innerHTML;
            }
            else if ($scope.AGType == 'Guest') {
                innerContents = document.getElementById("printtable3").innerHTML;
            }
            else if ($scope.AGType == 'Department') {
                innerContents = document.getElementById("printtable4").innerHTML;
            }
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Library/BookCirculationReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        //==============End==============//


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //--------------Clear-field.....
        $scope.Clearid = function () {
            $state.reload();
        }

        $scope.ExportToExcel = function () {
            var innerContents = '';
            if ($scope.AGType == 'Student') {
                innerContents = document.getElementById("printtable1").innerHTML;
                var exportHref = Excel.tableToExcel(printtable1, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else if ($scope.AGType == 'Staff') {
                innerContents = document.getElementById("printtable2").innerHTML;
                var exportHref = Excel.tableToExcel(printtable2, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else if ($scope.AGType == 'Guest') {
                innerContents = document.getElementById("printtable3").innerHTML;
                var exportHref = Excel.tableToExcel(printtable3, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else if ($scope.AGType == 'Department') {
                innerContents = document.getElementById("printtable4").innerHTML;
                var exportHref = Excel.tableToExcel(printtable4, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }

        }




        //////////=========================================================For House
        $scope.searchchkbx = "";
        $scope.all_check = function () {
            var checkStatus = $scope.usercheck;
            angular.forEach($scope.classList, function (itm) {
                itm.select = checkStatus;
            });
            if ($scope.usercheck == false) {
                $scope.usercheck23 = "";
                angular.forEach($scope.sectionList, function (obj) {
                    obj.select = false;
                });
            }
            else if ($scope.usercheck == true) {
                angular.forEach($scope.sectionList, function (obj) {
                    obj.select = true;
                });
                $scope.togchkbx23();
            }
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

        //$scope.filterchkbx = function (obj) {
        //    return (angular.lowercase(obj.className)).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        //}

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

        $scope.filterchkbx23 = function (obj) {
            return (angular.lowercase(obj.sectionName)).indexOf(angular.lowercase($scope.searchchkbx23)) >= 0;
        }

        //$scope.maxDatemf = new Date();
        $scope.gettodate = function () {

            $scope.minDatemf = new Date($scope.IssueFromDate);
            //$scope.maxDatemf = new Date();
        };


        $scope.gettodate23 = function () {

            $scope.minDatemf = new Date($scope.DueFromdate);
            //$scope.maxDatemf = new Date();
        };
        //summary
        //Summary
        $scope.OnClickAll = function () {
            $scope.getdata = [];
            angular.forEach($scope.sublist, function (dd) {
                dd.lmS_Ids = $scope.obj.all;
            });
        };
        $scope.isOptionsRequired1 = function () {
            return !$scope.sublist.some(function (options) {
                return options.lmS_Ids;
            });
        };
        //isOptionsRequired2
        $scope.isOptionsRequired2 = function () {
            return !$scope.sublistwo.some(function (options) {
                return options.lmS_Idss;
            });
        };
        $scope.obj.alls = false;
        $scope.obj.all = false;
        $scope.OnClickAlltwo = function () {
            $scope.getdata = [];
            $scope.obj.all = false;
            angular.forEach($scope.sublistwo, function (dd) {
                dd.lmS_Idss = $scope.obj.alls;
            });
            $scope.OnselctSummarytwo();
        };
        $scope.OnselctSummary = function () {
            $scope.getdata = [];
            $scope.obj.alls = false;
            $scope.obj.all = false;
            if ($scope.lmS_Id > 0 && $scope.lmS_Id != 10000123) {
                $scope.sublistwo = [];
                angular.forEach($scope.temptable, function (dd) {
                    if (dd.lmS_ParentId == $scope.lmS_Id && dd.lmS_Level == '2') {
                        $scope.sublistwo.push({
                            LMS_SubjectName: dd.lmS_SubjectName,
                            LMS_SubjectNo: dd.lmS_SubjectNo,
                            LMS_Id: dd.lmS_Id,
                        })
                    }
                });
            }

        }
        //
        $scope.OnselctSummarytwo = function () {
            $scope.getdata = [];
            $scope.tempthirdTable = [];
            $scope.sublist = [];
            if ($scope.sublistwo != null && $scope.sublistwo.length > 0) {
                angular.forEach($scope.sublistwo, function (dd) {
                    if (dd.lmS_Idss == true) {
                        $scope.tempthirdTable.push({
                            LMS_Id: dd.LMS_Id
                        })
                    }
                });
            }
            if ($scope.tempthirdTable != null && $scope.tempthirdTable.length > 0) {
                angular.forEach($scope.temptable, function (itm1) {
                    if (itm1.lmS_Level == '3') {
                        angular.forEach($scope.tempthirdTable, function (dd) {
                            if (itm1.lmS_ParentId == dd.LMS_Id) {
                                $scope.sublist.push({
                                    lmS_SubjectName: itm1.lmS_SubjectName,
                                    lmS_Id: itm1.lmS_Id,
                                    lmS_SubjectNo: itm1.lmS_SubjectNo,
                                })
                            }
                        });
                    }

                });
            }
        }
    }
})();

