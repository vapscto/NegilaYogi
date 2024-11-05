(function () {
    'use strict';
    angular.module('app')
        .controller('Settlement_ReportController', Settlement_ReportController)
    Settlement_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', 'Excel', '$timeout']
    function Settlement_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, Excel, $timeout) {

        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }


        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.sortKey = "regno";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.presentCountgrid = 0;

        $scope.tadprint = false;
        $scope.nameschedule = true;
        $scope.printdatatable = [];
        $scope.fromdate =new Date();
        $scope.todate =new Date();
        //$scope.loaddata = function () {
        //    $scope.screport = false;

        //    $scope.currentPage1 = 1;
        //    $scope.itemsPerPage1 = 10;
        //    var pageid = 2;
        //    apiService.getURI("Settlement_Report/getreport", pageid).
        //        then(function (promise) {
        //            $scope.yearlst = promise.fillyear;
        //            $scope.classlist = promise.fillclass;
        //        })
        //};


        $scope.submitted = false;
        $scope.showreport = function () {

            if ($scope.myForm.$valid) {

                $scope.fromdate = $filter('date')($scope.fromdate, 'yyyy-MM-dd'); 
                $scope.todate = $filter('date')($scope.todate, 'yyyy-MM-dd'); 
                //new Date($scope.fromdate).toDateString();
                //new Date($scope.todate).toDateString();
                var data = {
                    "fromdate": $scope.fromdate,
                    "todate": $scope.todate,
                }
                apiService.create("FeeStatusReport/GetSettlementReport/", data).
                    then(function (promise) {
                        if (promise.feestatusreportlist.length > 0 && promise.feestatusreportlist !== null) {
                            $scope.feestatusreportlist = promise.feestatusreportlist;

                            $scope.taskid5 = [];
                            angular.forEach($scope.feestatusreportlist, function (dev) {
                                if ($scope.taskid5.length === 0) {
                                    $scope.taskid5.push({ MI_Id: dev.MI_Id, MI_Name: dev.MI_Name});
                                } else if ($scope.taskid5.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.taskid5, function (emp) {
                                        if (emp.MI_Id === dev.MI_Id) {
                                            intcount += 1;
                                        }
                                    });

                                    if (intcount === 0) {
                                        $scope.taskid5.push({ MI_Id: dev.MI_Id, MI_Name: dev.MI_Name });
                                    }
                                }
                            });

                            console.log($scope.taskid5);
                            angular.forEach($scope.taskid5, function (ddd) {
                                $scope.templist = [];
                                angular.forEach($scope.feestatusreportlist, function (dd) {
                                    if (dd.MI_Id === ddd.MI_Id) {
                                        $scope.templist.push(dd);
                                    }
                                });
                                ddd.feestatusreportlist = $scope.templist;
                            });


                        }
                        else {
                            swal('No Data Found!!!');
                        }

                    })
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.toggleAll = function () {
            $scope.printstudents = [];
            var toggleStatus = $scope.all;
            angular.forEach($scope.deletemsglist, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    if ($scope.printdatatable.indexOf(itm) === -1) {
                        $scope.printdatatable.push(itm);

                    }
                }
                else {
                    $scope.printdatatable.splice(itm);

                }
            });
        }
        $scope.optionToggled = function (SelectedStudentRecord, index) {

            $scope.all = $scope.deletemsglist.every(function (itm) { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);


            }
            else {
                $scope.printdatatable.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);

            }

        }
        //export start
        $scope.exportToExcel = function (tableId) {
          
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            

        };

        $scope.printData = function () {
           
                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
           
        }

        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }
})();