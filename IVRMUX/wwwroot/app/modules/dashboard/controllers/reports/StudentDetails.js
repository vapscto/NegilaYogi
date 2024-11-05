
(function () {
    'use strict';
    angular
.module('app')
.controller('StudentDetailsController', StudentDetailsController)

    StudentDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', 'Excel', '$timeout']
    function StudentDetailsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, Excel, $timeout) {

        $scope.notrequired = true;

        $scope.obj = {};
        $scope.tadprint = false;
        $scope.nameschedule = true;
        $scope.printdatatable = [];

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.coptyright = copty;
        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.sortKey = "User_Name";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.presentCountgrid = 0;

        $scope.tadprint = false;
        $scope.printdatatable = [];
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.userPrivileges = "";
        $scope.tcreport = false;
        $scope.pagination = true;

        //Date:23-12-2016 for displaying privileges.
        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }
       
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.coptyright = copty;
        $scope.sortKey = "regno";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.presentCountgrid = 0;

        $scope.usrname = localStorage.getItem('username');

      

        $scope.imgname = logopath;

        $scope.loaddata = function () {
            $scope.screport = false;
            $scope.paginate1 = "paginate1";
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = paginationformasters;
            var pageid = 2;
            apiService.getURI("StudentDetails/getdetails", pageid).
            then(function (promise) {
                $scope.yearlst = promise.yeardropDown;
                $scope.classlist = promise.fillclass;
                $scope.IsHiddendown = false;
            })
        };

        $scope.exportToExcel = function () {


            if ($scope.details == "T") {
                $scope.routename = 'STUDENT TRANSPORT REPORT';
            }
            else if ($scope.details == "S") {
                $scope.routename = 'STUDENT SIBLING REPORT';
            }
            else if ($scope.details == "H") {
                $scope.routename = 'STUDENT HOSTEL REPORT';
            }
            else if ($scope.details == "O") {
                $scope.routename = 'STUDENT OVERAGE REPORT';
            }
            else if ($scope.details == "U") {
                $scope.routename = 'STUDENT UNDERAGE REPORT';
            }           

            var excelname = $scope.routename.toLowerCase();
            excelname = excelname.toUpperCase() + '.xls';
            var printSectionId = '#table22';
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(printSectionId, $scope.routename);
                $timeout(function () {
                    var a = document.createElement('a');
                    a.href = exportHref;
                    a.download = excelname;
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                }, 100);
                //$state.reload();
            }
            else {
                swal("Please Select Records to be Exported");
            }

        }

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

     
        $scope.sortBy = function (keyname) {
            
            $scope.propertyName = keyname;
            $scope.reverse = !$scope.reverse;
        };


        //$scope.sortBy = function (propertyName) {
        //    $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
        //    $scope.propertyName = propertyName;
        //};


        $scope.cancel = function () {
            $scope.loaddata();
        }

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.cancel = function () {
            $state.reload();
        };

        $scope.toggleAll = function () {
            if ($scope.searchValue == '') {
                var toggleStatus = $scope.all;
                angular.forEach($scope.reportdetails, function (itm) {
                    itm.selected = toggleStatus;
                    if ($scope.all == true) {
                        $scope.printdatatable.push(itm);
                    }
                    else {
                        $scope.printdatatable.splice(itm);
                    }
                });
            }

            if ($scope.searchValue != '') {
                toggleStatus = $scope.all;
                angular.forEach($scope.filterValue1, function (itm) {
                    itm.selected = toggleStatus;
                    if ($scope.all == true) {
                        $scope.printdatatable.push(itm);
                    }
                    else {
                        $scope.printdatatable.splice(itm);
                    }
                });
            }

        };
        $scope.selected = function (SelectedStudentRecord, index) {
            if ($scope.searchValue == '') {
                $scope.all = $scope.reportdetails.every(function (itm) { return itm.selected; });
                if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                    $scope.printdatatable.push(SelectedStudentRecord);
                }
                else {
                    $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
                }
            }
            if ($scope.searchValue != '') {
                $scope.all = $scope.filterValue1.every(function (itm) { return itm.selected; });
                if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                    $scope.printdatatable.push(SelectedStudentRecord);
                }
                else {
                    $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
                }
            }
        };

        $scope.submitted = false;
        $scope.showreport = function () {
        
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY,
                    "ASMCL_Id": $scope.ASMCL,
                    "type": $scope.details      
                }
                apiService.create("StudentDetails/Getreportdetails", data).
                    then(function (promise) {
                        if (promise.studentDetails != null && promise.studentDetails.length > 0) {
                            $scope.reportdetails = promise.studentDetails;
                            $scope.siblinglist = promise.siblinglist;
                            $scope.presentCountgrid = $scope.reportdetails.length;
                            angular.forEach($scope.reportdetails, function (xy) {
                                xy.sib = [];
                            })
                            if ( $scope.reportdetails.length > 0 && promise.siblinglist.length > 0) {
                                for (var i = 0; i < promise.siblinglist.length; i++) {
                                    for (var j = 0; j <  $scope.reportdetails.length; j++) {
                                        if (promise.siblinglist[i].pasR_Id ==  $scope.reportdetails[j].PASR_Id) {
                                             $scope.reportdetails[j].sib.push({ siblingname: promise.siblinglist[i].siblingname, siblingclass: promise.siblinglist[i].siblingclass, siblingadmno: promise.siblinglist[i].siblingadmno });
                                        }
                                    }
                                }
                            }
                            $scope.tcreport = true;         
                        } 
                    else {
                        swal("No Records Found");
                        $scope.tcreport = false;                     
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };     
        $scope.Clearid = function () {
            $scope.ASMAY = "";
            $scope.ASMCL = "";
            $scope.IsHiddendown = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        };
    }
})();