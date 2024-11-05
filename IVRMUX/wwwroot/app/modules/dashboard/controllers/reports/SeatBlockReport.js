(function () {
    'use strict';
    angular
.module('app')
.controller('SeatBlockReportController', SeatBlockReportController)

    SeatBlockReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache']
    function SeatBlockReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache) {
       
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
        $scope.coptyright = copty;
        $scope.ddate = {};
        $scope.ddate = new Date();

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
        $scope.printdatatable = [];
        $scope.obj = {};
        $scope.loaddata = function () {
            $scope.paginate1 = "paginate1";
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = paginationformasters;
            $scope.sbreport = false;
            var pageid = 2;
            apiService.getURI("SeatBlockReport/getdetails", pageid).
        then(function (promise) {
            $scope.yearlst = promise.fillyear;
            $scope.classlist = promise.fillclass;



        })
        };
        //export start
        $scope.exportToExcel = function (tableId) {
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please select records to be Exported");
            }

        };

        //search
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return ($filter('date')(obj.blockdate, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0) || ((obj.regno).indexOf($scope.searchValue) >= 0 || (obj.regno).indexOf($scope.searchValue) == null) || (angular.lowercase(obj.name)).indexOf(angular.lowercase($scope.searchValue)) >= 0 || (obj.block).indexOf($scope.searchValue) >= 0 || (obj.class).indexOf($scope.searchValue) >= 0;
        }

        //export end

        // print start
        //$scope.printData = function () {
        //    if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
        //        var divToPrint = document.getElementById("table");
        //        var newWin = window.open();
        //        newWin.document.write(divToPrint.outerHTML);
        //        newWin.print();
        //        newWin.close();
        //    }
        //    else {
        //        swal("Please select records to be printed");
        //    }
        //};
        $scope.ddate = {};
        $scope.ddate = new Date();
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
        //print end
        $scope.toggleAll = function () {
            if ($scope.searchValue == '') {
                var toggleStatus = $scope.all;
                angular.forEach($scope.students, function (itm) {
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

            if ($scope.searchValue != '') {
                var toggleStatus = $scope.all;
                angular.forEach($scope.filterValue1, function (itm) {
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
        };

        $scope.selected = function (SelectedStudentRecord, index) {
            if ($scope.searchValue == '') {
                $scope.all = $scope.students.every(function (itm)
                { return itm.selected; });
                if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                    $scope.printdatatable.push(SelectedStudentRecord);
                }
                else {
                    $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
                }
            }
            if ($scope.searchValue != '') {
                $scope.all = $scope.filterValue1.every(function (itm)
                { return itm.selected; });
                if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                    $scope.printdatatable.push(SelectedStudentRecord);
                }
                else {
                    $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
                }
            }
        };

        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        //$scope.onclickloaddata = function () {
        //    if ($scope.yearwiseorbtwdates === 'yearwise') {
        //        $scope.accyear = false;
        //        $scope.frdatetodate = true;

        //    }
        //    else if ($scope.yearwiseorbtwdates === 'btwdates') {
        //        $scope.accyear = true;
        //        $scope.frdatetodate = false;
        //    }
        //};

        $scope.stdnameorregnowise = function () {
            var data = {

                "stdorregnoflag": $scope.stdnameorregno,

            }
            apiService.create("SeatBlockReport/Getstudlist", data).
        then(function (promise) {
            $scope.studentlst = promise.studentlist;
        })
            
        };
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.setTodate = function (data) {
            console.log(data);
          //  $scope.fromdate = data;
            $scope.obj.todate = data;
            $scope.minDate = new Date(
           $scope.obj.todate.getFullYear(),
           $scope.obj.todate.getMonth(),
           $scope.obj.todate.getDate());

        }
        $scope.checkErr = function (fromdate, todate) {
            $scope.errMessage = '';
            var curDate = new Date();

            if (new Date(fromdate) > new Date(todate)) {
                $scope.errMessage = 'To Date should be greater than from date';
                return false;
            }
            //if (new Date(FromDate) < curDate) {
            //    $scope.errMessage = 'from date should not be before today.';
            //    return false;
            //}
          //  $scope.todate = todate;
        };
        $scope.submitted = false;
        $scope.showreport = function (obj) {
            if ($scope.myForm.$valid) {
                if ($scope.obj.yearwiseorbtwdates == "yearwise") {
                    $scope.obj.fromdate = '';
                    $scope.obj.todate = '';
                    $scope.obj.ASMAY = $scope.obj.ASMAY;
                }
                else if ($scope.obj.yearwiseorbtwdates === "btwdates") {
                    $scope.obj.fromdate = $scope.obj.fromdate;
                    $scope.obj.todate = $scope.obj.todate;
                    $scope.obj.ASMAY = '';
                }

                var data = {

                    "asmayid": $scope.obj.ASMAY,
                    "asmclid": $scope.obj.ASMCL,
                    "fromdate": $scope.obj.fromdate,
                    "todate": $scope.obj.todate,

                }
                apiService.create("SeatBlockReport/Getreportdetails", data).
            then(function (promise) {
                $scope.students = promise.allreports;
                $scope.presentCountgrid = promise.allreports.length;
                if ($scope.students.length >0 && $scope.students != null) {
                    $scope.sbreport = true;
                } else {
                    $scope.sbreport = false;
                    swal("NoRecords Found");
                }
            })
            }
            else {
                $scope.submitted = true;
            }
        };

       // $scope.onclickregnoname = function () {
       //     
       //     var data = {

       //         "regornamedetails": $scope.admname,
       //         "asmayid": $scope.obj.ASMAY,
       //         "asmclid": $scope.obj.ASMCL,


       //     }

       //     apiService.create("SeatBlockReport/getnameregno", data).
       //then(function (promise) {

       //    $scope.studentDropdown = promise.studentlist;
       //    $scope.stu_name_flag = true;
       //    $scope.clear_flag = true;
       //    $scope.report_flag = true;


       //})
       // };




        $scope.Clearid = function () {
            $state.reload();
          
            //  $scope.obj.fromdate = '';
            //  $scope.obj.todate = '';
            ////  $scope.yearlst = "";
            //  $scope.obj.ASMCL = '';
            //  $scope.obj.ASMAY = '';
            //  $scope.sbreport = false;
            //  $scope.submitted = false;
            //  $scope.myForm.$setPristine();
            //  $scope.myForm.$setUntouched();
       
        }
      


    }
}());