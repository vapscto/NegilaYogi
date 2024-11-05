(function () {
    'use strict';
    angular.module('app')
        .controller('feedefaultercontrolController', feedefaultercontrolController)
    feedefaultercontrolController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', 'Excel', '$timeout']
    function feedefaultercontrolController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, Excel, $timeout) {

        $scope.userPrivileges = "";
        $scope.filterValue = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId === pageid) {
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
        $scope.loaddata = function () {
            $scope.screport = false;

            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = 10;
            var pageid = 2;
            apiService.getURI("FeeDefaulterReport/feeremarkload", pageid).
                then(function (promise) {
                
                    $scope.fillclass = promise.fillclass; 
                    $scope.adcyear = promise.adcyear; 
                })
        };

        $scope.getsection = function (ASMCL_Id) {
            var data = {
                "ASMCL_Id": ASMCL_Id,
                "ASMAY_Id": $scope.ASMAY_Id
            }
            apiService.create("FeeDefaulterReport/feeremarksection/", data).then(function (promise) {
                if (promise.fillsection !== null && promise.fillsection.length > 0) {
                    $scope.fillsection = promise.fillsection;
                }
                else {
                    swal('No data Found..!');
                }
            });
        }
       

        $scope.submitted = false;
        $scope.showreport = function () {

            //if ($scope.myForm.$valid) {
                if ($scope.fromdate !== null && $scope.fromdate !== undefined) {
                    $scope.fromdate = $filter('date')($scope.fromdate, 'yyyy-MM-dd');
                }
                else {
                    $scope.fromdate = "";
                                    }
                if ($scope.todate !== null && $scope.todate !== undefined) {
                    $scope.todate = $filter('date')($scope.todate, 'yyyy-MM-dd');
                }
                else {
                    $scope.todate = "";
                }
                $scope.sectionarray = [];
                if ($scope.ASMS_Id === "0") {
                    
                    angular.forEach($scope.fillsection, function (qq) {
                        $scope.sectionarray.push({ ASMS_Id: qq.asmS_Id });
                    });
                }
                else {
                    $scope.sectionarray = undefined;
                }
               
                var data = {
                    "fromdate": $scope.fromdate,
                    "todate": $scope.todate,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "sectionarray": $scope.sectionarray
                }
                apiService.create("FeeDefaulterReport/feeremarkreport/", data).
                    then(function (promise) {
                        if (promise.feedefaulterreport !== null && promise.feedefaulterreport.length > 0) {
                            $scope.feedefaulterreport = promise.feedefaulterreport;
                        }
                        else {
                            swal('No Data Found!!!');
                        }

                    });
            //}
            //else {
            //    $scope.submitted = true;
            //}
        };
        $scope.toggleAll = function () {
            $scope.printstudents = [];
            var toggleStatus = $scope.all;
            angular.forEach($scope.feedefaulterreport, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all === true) {
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

            $scope.all = $scope.feedefaulterreport.every(function (itm) { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);


            }
            else {
                $scope.printdatatable.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);

            }

        }
        //export start
        $scope.exportToExcel = function (tableId) {
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'FeeDefaulterRemarks');
                $timeout(function () {
                    var a = document.createElement('a');
                    a.href = exportHref;
                    a.download = "FeeDefaulterRemarks.xls";
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                }, 100);


               // $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please select records to be Exported");
            }

        };

        $scope.printData = function () {
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
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
            else {
                swal("Please Select Records to be Printed");
            }
        }

        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }
})();