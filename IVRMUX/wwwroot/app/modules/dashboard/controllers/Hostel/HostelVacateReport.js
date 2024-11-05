(function () {
    'use strict';
    angular
        .module('app')
        .controller('HostelVacateReportController', HostelVacateReportController);
    HostelVacateReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', 'Excel', '$timeout'];
    function HostelVacateReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, Excel, $timeout) {

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.interacted1 = function (field) {
            return $scope.submitted;
        };

        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        //   $scope.page1 = "page1";
        $scope.search = " ";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        };



        $scope.submitted = false;
        $scope.search = '';
        $scope.type = "student";
        $scope.loaddata = function () {
            var data = {
                "type": "student"
            };
            apiService.create("HostelVacateReport/loaddata", data).then(function (promise) {

                $scope.yeralist = promise.yerlist;
                $scope.hostelist = promise.hostelist;
                $scope.studentlist = promise.studentlist;
                $scope.stafflist = promise.stafflist;
                $scope.guestlist = promise.guestlist;
            });
        };


        $scope.changeradio = function () {
            $scope.radiobutton2 = 'all';
            $scope.hostelflag = false;
            $scope.studentflag = false;
            $scope.staffflag = false;
            $scope.guestflag = false;
        };

        $scope.hostelflag = false;
        $scope.studentflag = false;
        $scope.staffflag = false;
        $scope.guestflag = false;

        $scope.stuchk = 'student';
        $scope.radiobutton2 = 'all';
        $scope.checkradio = function () {
            if ($scope.radiobutton2 === 'all') {
                $scope.hostelflag = false;
                $scope.studentflag = false;
                $scope.staffflag = false;
                $scope.guestflag = false;
            }
            else if ($scope.radiobutton2 === 'Hostel') {
                $scope.hostelflag = true;
                $scope.studentflag = false;
                $scope.staffflag = false;
                $scope.guestflag = false;
            }
            else if ($scope.radiobutton2 === 'individual' && $scope.stuchk === 'student') {
                $scope.studentflag = true;
                $scope.hostelflag = false;
                $scope.staffflag = false;
                $scope.guestflag = false;
            }
            else if ($scope.radiobutton2 === 'individual' && $scope.stuchk === 'staff') {
                $scope.staffflag = true;
                $scope.hostelflag = false;
                $scope.studentflag = false;
                $scope.guestflag = false;
            }
            else if ($scope.radiobutton2 === 'individual' && $scope.stuchk === 'guest') {
                $scope.guestflag = true;
                $scope.hostelflag = false;
                $scope.staffflag = false;
                $scope.studentflag = false;
            }
            else {
                $scope.hostelflag = false;
                $scope.studentflag = false;
                $scope.staffflag = false;
                $scope.guestflag = false;
            }
        };

        //=============get report
        $scope.get_reportlist = function () {
            $scope.gridlistdata = [];
            $scope.studentflag = false;
            $scope.staffflag = false;
            $scope.guestflag = false;
            var fromdate = $scope.fromdate === null ? "" : $filter('date')($scope.fromdate, "yyyy-MM-dd");
            var todate = $scope.enddate === null ? "" : $filter('date')($scope.enddate, "yyyy-MM-dd");

            $scope.SelectedHostellist = [];
            $scope.SelectedStudentlist = [];
            $scope.Selectedstafflist = [];
            $scope.Selectedguestlist = [];

            angular.forEach($scope.hostelist, function (hst) {
                if (hst.select === true) {
                    $scope.SelectedHostellist.push({ HLMH_Id: hst.hlmH_Id });
                }
            });

            angular.forEach($scope.studentlist, function (sect) {
                if (sect.select === true) {
                    $scope.SelectedStudentlist.push({ AMST_Id: sect.amsT_Id });
                }
            });
            angular.forEach($scope.stafflist, function (stf) {
                if (stf.select === true) {
                    $scope.Selectedstafflist.push({ HRME_Id: stf.hrmE_Id });
                }
            });

            angular.forEach($scope.guestlist, function (gut) {
                if (gut.select === true) {
                    $scope.Selectedguestlist.push({ HLHGSTALT_Id: gut.hlhgstalT_Id });
                }
            });

            if ($scope.myForm.$valid) {
                var data = {
                    "type": $scope.stuchk,
                    "type2": $scope.radiobutton2,
                    "Fromdate": fromdate,
                    "ToDate": todate,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    SelectedHostellist: $scope.SelectedHostellist,
                    SelectedStudentlist: $scope.SelectedStudentlist,
                    Selectedstafflist: $scope.Selectedstafflist,
                    Selectedguestlist: $scope.Selectedguestlist
                };
                apiService.create("HostelVacateReport/get_report", data).then(function (promise) {

                    if (promise.gridlistdata.length > 0) {
                        $scope.gridlistdata = promise.gridlistdata;

                        if ($scope.stuchk == 'student') {
                            $scope.studentflag = true;
                        }
                        else if ($scope.stuchk == 'staff') {
                            $scope.staffflag = true;
                        }
                        else if ($scope.stuchk == 'guest') {
                            $scope.guestflag = true;
                        }
                    }
                    else {
                        swal('Record is not Available!!');
                        $state.reload();
                    }
                    $scope.hostelflag = true;
                    $scope.printflag = true;;

                });
            }
            else {
                $scope.submitted = true;
            }
        }
                      
                        //$scope.hostelflag = true;
                        //$scope. = true;
                        //$scope. = true;
                        //$scope.guestflag = true;
                        //$scope.printflag = true;                     
                    
                    //else {
                    //    swal("Record Not Found!");
                    //    $scope.printtable = false;
                    //    $scope.printd = false;
                    //    $scope.printflag = true;
                    //}
        //});
        //    }
        //    //else {
        //    //    $scope.submitted = true;
        //    //}
        //};

        $scope.printdata = function () {    

            var innerContents = '';
            if ($scope.stuchk == 'student') {
                innerContents = document.getElementById("printtable").innerHTML;
            }
            else if ($scope.stuchk == 'staff') {
                innerContents = document.getElementById("printtable2").innerHTML;
            }
            else if ($scope.stuchk == 'guest') {
                innerContents = document.getElementById("printtable3").innerHTML;   
            }

            //if ($scope.filterValue !== null && $scope.filterValue.length > 0) {
            //  var innerContents = document.getElementById("printtable").innerHTML;
           
            //}
            //else if ($scope.filterValue2 !== null && $scope.filterValue2.length > 0) {
            //    var innerContents = document.getElementById("printtable2").innerHTML;
                
            //}
            //else if ($scope.filterValue3 !== null && $scope.filterValue3.length > 0) {
            //    var innerContents = document.getElementById("printtable3").innerHTML;               
            //}
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Library/BookArrivalReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        // $scope.ExportToExcel = function () {
        //    var exportHref = Excel.tableToExcel(printtable, 'sheet name');
        //    $timeout(function () { location.href = exportHref; }, 100);
        //};
        $scope.ExportToExcel = function () {
            var innerContents = '';
            if ($scope.stuchk == 'student') {
                innerContents = document.getElementById("printtable").innerHTML;
                var exportHref = Excel.tableToExcel(printtable, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else if ($scope.stuchk == 'staff') {
                innerContents = document.getElementById("printtable2").innerHTML;
                var exportHref = Excel.tableToExcel(printtable2, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else if ($scope.stuchk == 'guest') {
                innerContents = document.getElementById("printtable3").innerHTML;
                var exportHref = Excel.tableToExcel(printtable3, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
        }


        //hostel dropdown
        $scope.searchchkbx = "";
        $scope.all_check = function () {
            var checkStatus = $scope.usercheck;
            angular.forEach($scope.hostelist, function (itm) {
                itm.select = checkStatus;
            });
        };
        $scope.togchkbx = function () {
            $scope.usercheck = $scope.hostelist.every(function (options) {
                return options.select;
            });
        };
        $scope.isOptionsRequired = function () {
            return !$scope.hostelist.some(function (options) {
                return options.select;
            });
        };
        //student dropdown
        $scope.searchchkbx1 = "";
        $scope.all_check1 = function () {
            var checkStatus = $scope.usercheck1;
            angular.forEach($scope.studentlist, function (itm) {
                itm.select = checkStatus;
            });
        };
        $scope.togchkbx1 = function () {
            $scope.usercheck1 = $scope.studentlist.every(function (options) {
                return options.select;
            });
        };
        $scope.isOptionsRequired1 = function () {
            return !$scope.studentlist.some(function (options) {
                return options.select;
            });
        };
        //staff dropdown
        $scope.searchchkbx2 = "";
        $scope.all_check2 = function () {
            var checkStatus = $scope.usercheck2;
            angular.forEach($scope.stafflist, function (itm) {
                itm.select = checkStatus;
            });
        };
        $scope.togchkbx2 = function () {
            $scope.usercheck2 = $scope.stafflist.every(function (options) {
                return options.select;
            });
        };
        $scope.isOptionsRequired2 = function () {
            return !$scope.stafflist.some(function (options) {
                return options.select;
            });
        };
        //guest dropdown
        $scope.searchchkbx3 = "";
        $scope.all_check3 = function () {
            var checkStatus = $scope.usercheck3;
            angular.forEach($scope.guestlist, function (itm) {
                itm.select = checkStatus;
            });
        };
        $scope.togchkbx3 = function () {
            $scope.usercheck3 = $scope.guestlist.every(function (options) {
                return options.select;
            });
        };
        $scope.isOptionsRequired3 = function () {
            return !$scope.guestlist.some(function (options) {
                return options.select;
            });
        };
       

        $scope.Clearid = function () {
            $state.reload();
        };



        $scope.get_Studentdata = function () {

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };

            apiService.create("HostelVacateReport/get_Studentlist", data).then(function (promise) {
                $scope.studentlist = promise.studentlist;
            });
        };



    }
})();