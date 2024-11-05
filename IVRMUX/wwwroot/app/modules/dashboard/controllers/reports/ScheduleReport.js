(function () {
    'use strict';
    angular.module('app')
    .controller('ScheduleReportController', ScheduleReportController)
    ScheduleReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', 'Excel', '$timeout']
    function ScheduleReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, Excel, $timeout) {

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
        if (ivrmcofigsettings != null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
                copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            }
        }
        
        $scope.coptyright = copty;
        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null) {
            if (admfigsettings.length > 0) {
                var logopath = admfigsettings[0].asC_Logo_Path;
            }

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
            $scope.paginate1 = "paginate1";
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = paginationformasters;
            $scope.itemsPerPage2 = 20;
            var pageid = 2;
            apiService.getURI("ScheduleReport/getdetails", pageid).
            then(function (promise) {
                $scope.yearlst = promise.fillyear;
                $scope.classlist = promise.fillclass;
            })
        };
        //search
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return angular.lowercase(obj.name).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase(obj.regno).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase(obj.gender).indexOf(angular.lowercase($scope.searchValue)) >= 0 || ($filter('date')(obj.ScheduleTime, 'HH:mm').indexOf($scope.searchValue) >= 0) || ($filter('date')(obj.ScheduleTimeTo, 'HH:mm').indexOf($scope.searchValue) >= 0) || ($filter('date')(obj.ScheduleDate, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0) || ($filter('date')(obj.entrydate, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0) || angular.lowercase(obj.PAOTS_Remarks).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        }

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
            $scope.printdatatable = [];
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
           // $scope.printdatatable = [];
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
        //for print schedule
        $scope.printScheduleData = function (printSchedule_data) {
            
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var innerContents = document.getElementById("printScheduleSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
               '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                      '<link type="text/css" media="print" rel="stylesheet" href="css/print/RAMAKRISHNA_VIDYASHALA/RegistrationReceiptPdf.css" />' +
                  '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
            else {
                swal("Please Select Records to be Printed");
            }
        }
        //for Export excel
        //for print schedule
        $scope.printSchedulelist = function (printSchedule_data) {

            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {

               
                var innerContents = document.getElementById("print_data_srkvs").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/RAMAKRISHNA_VIDYASHALA/RegistrationReceiptPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
            else {
                swal("Please Select Records to be Printed");
            }
        }

        $scope.primtsDD = function () {

        }
        $scope.printOrallist = function (printSchedule_data) {

            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                //$scope.printdatatablearray = [];
                //for (var i = 0; i <= $scope.printdatatable.length; i++) {
                //    if (i = 5) {
                //        $scope.printdatatablearray.push($scope.printdatatable[i]);
                        
                      
                //    }
                //}
                var innerContents = document.getElementById("print_oral_data").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/RAMAKRISHNA_VIDYASHALA/RegistrationReceiptPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();

            }
            else {
                swal("Please Select Records to be Printed");
            }
        }

        

        $scope.printScheduleverfication = function (printSchedule_data) {

            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {


                var innerContents = document.getElementById("print_data_verifiication").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/RAMAKRISHNA_VIDYASHALA/RegistrationReceiptPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
            else {
                swal("Please Select Records to be Printed");
            }
        }
        //for Export excel


        $scope.exportToExcel = function (tableId) {
            var excelname = 'Schedule Report.xls';
            if ($scope.obj.oralwrittenschedule == 'oral')
            {
                excelname = 'Oraltest Schedule Report.xls';
            }
            else if ($scope.obj.oralwrittenschedule == 'written') {
                excelname = 'Written Schedule Report.xls';
            }
            else if ($scope.obj.oralwrittenschedule == 'statusschedule') {
                excelname = 'IntimationSchedule Schedule Report.xls';
            }
            
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'Schedule Report');
                $timeout(function () {
                    var a = document.createElement('a');
                    a.href = exportHref;
                    a.download = excelname;
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                }, 100);
            }
            else {
                swal("Please select records to be Exported");
            }

        };
        //for print
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
        // end for print


        $scope.order = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        };

        $scope.setTodate = function (data) {
            $scope.obj.todate = data;
            $scope.minDate = new Date(
            $scope.obj.todate.getFullYear(),
            $scope.obj.todate.getMonth(),
            $scope.obj.todate.getDate());
        };

        $scope.oralwrittenschedule1 = function () {
            if ($scope.obj.oralwrittenschedule == "oral") {
                $scope.nameschedule = false;
                $scope.dishr = true;
                $scope.screport = false;
            }
            else if ($scope.obj.oralwrittenschedule == "written") {
                $scope.nameschedule = false;
                $scope.hrsrt = "24hrrt";
                $scope.dishr = false;
                $scope.screport = false;
            }
            else if ($scope.obj.oralwrittenschedule == "statusschedule") {
                $scope.nameschedule = false;
                $scope.dishr = true;
                $scope.screport = false;
            }

            var data = {
                "oralwrittenscheduleflag": $scope.obj.oralwrittenschedule,
                "yearorbtwndates": $scope.obj.yearwiseorbtwdates,
                "asmay_id": $scope.obj.ASMAY
            }
            apiService.create("ScheduleReport/schedulelist", data).then(function (promise) {
                $scope.schelist = promise.writentestlist;
            });
        };

        $scope.checkErr = function (fromdate, todate) {
            $scope.errMessage = '';
            var curDate = new Date();
            if (new Date(fromdate) > new Date(todate)) {
                $scope.errMessage = 'To Date should be greater than from date';
                return false;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.schedulechnage = function (schelist,schid) {
            angular.forEach(schelist, function (obj) {
                if (obj.disid == schid) {
                    $scope.schedulename = obj.disname;
                }
            });
        };



        $scope.submitted = false;
        $scope.showreport = function (obj) {

            $scope.all = false;
            var flagcheck;
            //angular.forEach($scope.all, function (obj) {
            //    obj.cheked = false;
            //});

            if ($scope.myForm.$valid) {
                if ($scope.obj.yearwiseorbtwdates == "yearwise") {
                    $scope.obj.fromdate = new Date();
                    $scope.obj.todate = new Date();

                }
                else if ($scope.obj.yearwiseorbtwdates == "btwdates") {
                    $scope.obj.fromdate = new Date($scope.obj.fromdate).toDateString();
                    $scope.obj.todate = new Date($scope.obj.todate).toDateString();
                    $scope.obj.ASMAY = '0';
                }
                if ($scope.autostudent == true) {
                    flagcheck = 2;
                }
                else {
                    flagcheck = 1;
                }

                var data = {
                    "asmayid": $scope.obj.ASMAY,
                    "asmclid": $scope.obj.ASMCL,
                    "fromdate": $scope.obj.fromdate,
                    "todate": $scope.obj.todate,
                    "flagows": $scope.obj.oralwrittenschedule,
                    "schids": $scope.obj.disid,
                    "stype": flagcheck
                }
                apiService.create("ScheduleReport/Getreportdetails", data).
                then(function (promise) {

                    $scope.students = promise.allreports;
                    if ($scope.hrsrt == "12hrrt") {
                        angular.forEach($scope.students, function (obj) {
                            if (obj.ScheduleTime.substring(0, 2) > 12) {
                                obj.ScheduleTimeonly = (Number(obj.ScheduleTime.substring(0, 2)) - 12).toString();
                                if (obj.ScheduleTimeonly.length == 1) {
                                    obj.ScheduleTimeoly = '0' + obj.ScheduleTimeonly;
                                    obj.ScheduleTime = obj.ScheduleTimeoly + ':' + obj.ScheduleTime.substring(3, 5);
                                    obj.ScheduleTime = obj.ScheduleTime + 'PM';
                                }
                                else if (obj.ScheduleTimeonly.length > 1) {
                                    obj.ScheduleTime = obj.ScheduleTimeonly + ':' + obj.ScheduleTime.substring(3, 5);
                                    obj.ScheduleTime = obj.ScheduleTime + 'PM';
                                }
                            }
                            else if (obj.ScheduleTime.substring(0, 2) == 12) {
                                obj.ScheduleTime = obj.ScheduleTime + 'PM';
                            }
                            else {
                                if (obj.ScheduleTime.length == 1) {
                                    obj.ScheduleTime = '0' + obj.ScheduleTime;
                                    obj.ScheduleTime = obj.ScheduleTime + 'AM';
                                }
                                else {
                                    obj.ScheduleTime = obj.ScheduleTime + 'AM';
                                }
                            }
                        })

                        angular.forEach($scope.students, function (obj) {
                            if (obj.ScheduleTimeTo.substring(0, 2) > 12) {
                                obj.ScheduleTimeToonly = (Number(obj.ScheduleTimeTo.substring(0, 2)) - 12).toString();
                                if (obj.ScheduleTimeToonly.length == 1) {
                                    obj.ScheduleTimeTooly = '0' + obj.ScheduleTimeToonly;
                                    obj.ScheduleTimeTo = obj.ScheduleTimeTooly + ':' + obj.ScheduleTimeTo.substring(3, 5);
                                    obj.ScheduleTimeTo = obj.ScheduleTimeTo + 'PM';
                                }
                                else if (obj.ScheduleTimeToonly.length > 1) {
                                    obj.ScheduleTimeTo = obj.ScheduleTimeToonly + ':' + obj.ScheduleTimeTo.substring(3, 5);
                                    obj.ScheduleTimeTo = obj.ScheduleTimeTo + 'PM';
                                }
                            }
                            else if (obj.ScheduleTimeTo.substring(0, 2) == 12) {
                                obj.ScheduleTimeTo = obj.ScheduleTimeTo + 'PM';
                            }
                            else {

                                if (obj.ScheduleTimeTo.length == 1) {
                                    obj.ScheduleTimeTo = '0' + obj.ScheduleTimeTo;
                                    obj.ScheduleTimeTo = obj.ScheduleTimeTo + 'AM';
                                }
                                else {
                                    obj.ScheduleTimeTo = obj.ScheduleTimeTo + 'AM';
                                }
                            }
                        })
                    }
                    if (promise.allreports.length > 0) {
                        $scope.dateofschedule = promise.allreports[0].ScheduleDate;
                        $scope.timeofschedule = promise.allreports[0].ScheduleTime;
                    }
                    
                    $scope.presentCountgrid = promise.allreports.length;
                    if ($scope.students.length > 0 || $scope.students == null) {
                        $scope.screport = true;
                    } else {
                        swal("No Records Found");
                        $scope.screport = false;
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };




        $scope.sendmail = function () {
            
            $scope.albumNameArray = [];
            angular.forEach($scope.students, function (user) {
                if (!!user.selected) $scope.albumNameArray.push({
                    name: user.name, ScheduleDate: $filter('date')(user.ScheduleDate, 'dd/MM/yyyy'), ScheduleTime:
                        user.ScheduleTime, ScheduleTimeTo: user.ScheduleTimeTo, regno: user.regno
                });

            })

            if ($scope.albumNameArray.length > 0) {
                swal({
                    title: "Are you sure?",
                    text: "Do you want to Send MAIL?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Send it!",
                    cancelButtonText: "Cancel!!!!!!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
             function (isConfirm) {
                 if (isConfirm) {
                     var data = {
                         SmsMailStudentDetailst: $scope.albumNameArray,

                     };
                     apiService.create("ScheduleReport/SendMail", data).
                then(function (promise) {
                    $scope.$apply();
                    $scope.PostDataResponse = data;
                    swal('Mail Sent Successfully');
                    $state.reload();
                });
                 }
                 else {
                     swal("MAIL Sending Cancelled");

                 }
             });

            } else {
                swal("Kindly select atleast a record to procced..!");
                return;
            }


        }

        $scope.Clearid = function () {
            $state.reload();
            //$scope.obj.fromdate = '';
            //$scope.obj.todate = '';
            //$scope.obj.ASMCL = '';
            //$scope.obj.ASMAY = '';
            //$scope.obj.oralwrittenschedule = '';
            //$scope.obj.disid = '';
            //$scope.obj.yearwiseorbtwdates = 'yearwise';
            //$scope.nameschedule = true;
            //$scope.screport = false;
            //$scope.submitted = false;
            //$scope.myForm.$setPristine();
            //$scope.myForm.$setUntouched();
        };
    }
})();