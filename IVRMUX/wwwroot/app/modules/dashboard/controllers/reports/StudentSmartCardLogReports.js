


(function () {
    'use strict';
    angular
.module('app')
.controller('StudentSmartCardLogReportsController', StudentSmartCardLogReportsController)

    StudentSmartCardLogReportsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', 'superCache', 'Excel', '$timeout']
    function StudentSmartCardLogReportsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, superCache, Excel, $timeout) {
        $scope.dailydate = new Date();
        $scope.fromdate = new Date();
        $scope.todate = new Date();
        $scope.albumNameArraycolumn = [];
        $scope.columnsTest = [];
        $scope.obj = {};
        $scope.tadprint = false;

        $scope.exp_excel_flag = true;
        $scope.print_flag = true;
        $scope.printstudents = [];

        $scope.loaddata = function () {
            $scope.columnsTest = [];
            $scope.todate3 = new Date();
            $scope.maxDateftodate = new Date(
           $scope.todate3.getFullYear(),
           $scope.todate3.getMonth(),
          $scope.todate3.getDate());
            $scope.currentPage = 1;
            //$scope.itemsPerPage = 5
            var id = 1;
            $scope.module = [{ id: '1', checked: true, value: 'kiosk' }, { id: '2', checked: true, value: 'Fees' },
               { id: '3', checked: true, value: 'Library' }]
            apiService.getURI("StudentSmartCardLogReports/getalldetails", id).
        then(function (promise) {

            //  $scope.arrlistchkhead = promise.alldata;
            $scope.columnsTest = [];            // 
        })
        }

        $scope.betweendates = true;
        $scope.daily = true;

        $scope.usrname = localStorage.getItem('username');

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        $scope.reporsmart = false;
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.students, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                    }
                }
                else {
                    $scope.printstudents.splice(itm);
                }
            });
        }

        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.all = $scope.students.every(function (itm)
            { return itm.selected; });
            if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
                $scope.printstudents.push(SelectedStudentRecord);
            }
            else {
                $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            }

        }
        //all or individual
        $scope.onclickloaddata = function () {
            if ($scope.allorindiv == "All") {
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.regnonamestd = true;
                $scope.reporsmart = false;

            }
            else if ($scope.allorindiv === "indi") {
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.regnonamestd = false;
                $scope.reporsmart = false;
            }
            $scope.students = [];
        };

        //datewise or between dates
        $scope.onclickdates = function () {
            if ($scope.dailybtedates == "daily") {
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.betweendates = true;
                $scope.daily = false;
                $scope.reporsmart = false;
            }
            else if ($scope.dailybtedates === "btwdates") {
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.todatef = true;
                $scope.betweendates = false;
                $scope.daily = true;
                $scope.reporsmart = false;

            }
        };

        //modules radio button
        $scope.onclickmodule = function () {
            if ($scope.mallorindi == "mall") {
                $scope.moduleallorind = true;
                $scope.reporsmart = false;
            }
            else if ($scope.mallorindi === "mindi") {
                $scope.moduleallorind = false;
                $scope.reporsmart = false;
            }
        };

        //reg or name wise binding 
        $scope.onclickregnoname = function (obj) {
            var data = {
                "regornamedetails": obj.regorname
            }
            apiService.create("StudentSmartCardLogReports/getnameregno", data).
       then(function (promise) {
           $scope.studentlst = promise.studentlist;
           $scope.reporsmart = false;
       })
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }


        $scope.submitted = false;

        //report
        $scope.getreport = function (obj) {
            
            $scope.students = [];
            if ($scope.myForm.$valid) {
                if ($scope.allorindiv == null || $scope.allorindiv == undefined || $scope.mallorindi == null || $scope.allorindiv == undefined || $scope.dailybtedates == null || $scope.dailybtedates == undefined) {
                    swal("Kindly Select The Options !!");
                }
                else {
                    if ($scope.mallorindi === 'mall') {
                        $scope.ASModule = '';
                        obj.ASModule = "all";
                    }
                    else {
                        if (obj.ASModule == null || obj.ASModule == undefined) {
                            swal("Kindly Select The Module !!");
                            return;
                        }
                        else {
                            obj.ASModule = obj.ASModule;
                        }
                    }
                    var dailydate1 = "";
                    var fromdate1 = "";
                    var todate1 = "";

                    if ($scope.dailybtedates == "daily") {
                        fromdate1 = new Date().toDateString();
                        todate1 = new Date().toDateString();
                        dailydate1 = new Date(obj.dailydate).toDateString();
                    }
                    else {
                        fromdate1 = new Date(obj.fromdate).toDateString();
                        todate1 = new Date(obj.todate).toDateString();
                        dailydate1 = new Date().toDateString();
                    }

                    var data = {
                        "allorindiv": $scope.allorindiv,
                        "mallorindi": $scope.mallorindi,
                        "regorname": $scope.regorname,
                        "dailydate": dailydate1,
                        "dailybtedates": $scope.dailybtedates,
                        "fromdate": fromdate1,
                        "todate": todate1,
                        "Amst_Id": obj.amst_Id,
                        "ASMODULE": obj.ASModule,
                    }

                    if ($scope.allorindiv == "All") {
                        if ($scope.dailybtedates == "daily") {
                            if ($scope.mallorindi == "mall") {
                                if (obj.dailydate == null || obj.dailydate == undefined) {
                                    swal("Select The Required Fields !!");
                                    $scope.reporsmart = false;
                                }
                                else {
                                    apiService.create("StudentSmartCardLogReports/Getreportdetails", data).
                                  then(function (promise) {
                                      if (promise.alldatagridreport.length == 0) {
                                          $scope.reporsmart = false;
                                          swal("Record Not Found !!");
                                          $state.reload();
                                      }
                                      else {
                                          $scope.students = promise.alldatagridreport;
                                          $scope.presentCountgrid = $scope.students.length;
                                          $scope.reporsmart = true;
                                          $scope.exp_excel_flag = false;
                                          $scope.print_flag = false;
                                      }
                                  })
                                }
                            }
                            else if ($scope.mallorindi === "mindi") {
                                if (obj.dailydate == null || obj.dailydate == undefined || obj.ASModule == null || obj.ASModule == undefined) {
                                    swal("Select The Required Fields !!");
                                    $scope.reporsmart = false;
                                }
                                else {
                                    apiService.create("StudentSmartCardLogReports/Getreportdetails", data).
                                  then(function (promise) {
                                      if (promise.alldatagridreport.length > 0) {
                                          $scope.reporsmart = true;
                                          $scope.students = promise.alldatagridreport;
                                          $scope.presentCountgrid = $scope.students.length;
                                          $scope.exp_excel_flag = false;
                                          $scope.print_flag = false;
                                      }
                                      else {
                                          $scope.reporsmart = false;
                                          swal("Record Not Found !!");
                                          $state.reload();
                                      }
                                  })
                                }
                            }
                        }
                        else if ($scope.dailybtedates === "btwdates") {
                            if ($scope.mallorindi == "mall") {
                                if (obj.todate == null || obj.todate == undefined || obj.fromdate == null || obj.fromdate == undefined) {
                                    $scope.reporsmart = false;
                                    swal("Select The Required Fields !!");
                                }
                                else {
                                    apiService.create("StudentSmartCardLogReports/Getreportdetails", data).
                                  then(function (promise) {
                                      if (promise.alldatagridreport.length > 0) {
                                          $scope.reporsmart = true;
                                          $scope.students = promise.alldatagridreport;
                                          $scope.presentCountgrid = $scope.students.length;
                                          $scope.exp_excel_flag = false;
                                          $scope.print_flag = false;
                                      }
                                      else {
                                          $scope.reporsmart = false;
                                          swal("Record Not Found !!");
                                          $state.reload();
                                      }
                                  })
                                }
                            }
                            else if ($scope.mallorindi === "mindi") {
                                if (obj.todate == null || obj.todate == undefined || obj.fromdate == null || obj.fromdate == undefined || obj.ASModule == null || obj.ASModule == undefined) {
                                    swal("Select The Required Fields !!");
                                    $scope.reporsmart = false;
                                }
                                else {
                                    apiService.create("StudentSmartCardLogReports/Getreportdetails", data).
                                  then(function (promise) {
                                      if (promise.alldatagridreport.length > 0) {
                                          $scope.reporsmart = true;
                                          $scope.students = promise.alldatagridreport;
                                          $scope.presentCountgrid = $scope.students.length;
                                          $scope.exp_excel_flag = false;
                                          $scope.print_flag = false;
                                      }
                                      else {
                                          $scope.reporsmart = false;
                                          $state.reload();
                                          swal("Record Not Found !!");
                                      }
                                  })
                                }
                            }
                        }

                        else {
                            swal("Record Not Found !!");
                            $scope.reporsmart = false;
                        }
                    }
                    else if ($scope.allorindiv === "indi") {
                        if ($scope.dailybtedates == "daily") {
                            if ($scope.mallorindi == "mall") {
                                if (obj.dailydate == null || obj.dailydate == undefined) {
                                    swal("Select The Required Fields !!");
                                    $scope.reporsmart = false;
                                }
                                else {
                                    apiService.create("StudentSmartCardLogReports/Getreportdetails", data).
                                  then(function (promise) {
                                      if (promise.alldatagridreport.length > 0) {
                                          $scope.reporsmart = true;
                                          $scope.students = promise.alldatagridreport;
                                          $scope.presentCountgrid = $scope.students.length;
                                          $scope.exp_excel_flag = false;
                                          $scope.print_flag = false;
                                      }
                                      else {
                                          swal("Record Not Found !!");
                                          $state.reload();
                                          $scope.reporsmart = false;
                                      }
                                  })
                                }
                            }
                            else if ($scope.mallorindi === "mindi") {
                                if (obj.dailydate == null || obj.dailydate == undefined || obj.ASModule == null || obj.ASModule == undefined) {
                                    swal("Select The Required Fields !!");
                                    $scope.reporsmart = false;
                                }
                                else {
                                    apiService.create("StudentSmartCardLogReports/Getreportdetails", data).
                                  then(function (promise) {

                                      if (promise.alldatagridreport.length > 0) {
                                          $scope.reporsmart = true;
                                          $scope.students = promise.alldatagridreport;
                                          $scope.presentCountgrid = $scope.students.length;
                                          $scope.exp_excel_flag = false;
                                          $scope.print_flag = false;
                                      }
                                      else {
                                          swal("Record Not Found !!");
                                          $state.reload();
                                          $scope.reporsmart = false;
                                      }
                                  })
                                }
                            }
                        }
                        else if ($scope.dailybtedates === "btwdates") {

                            if ($scope.mallorindi == "mall") {
                                if (obj.todate == null || obj.todate == undefined || obj.fromdate == null || obj.fromdate == undefined) {
                                    swal("Select The Required Fields !!");
                                    $scope.reporsmart = false;
                                }
                                else {
                                    apiService.create("StudentSmartCardLogReports/Getreportdetails", data).
                                  then(function (promise) {

                                      if (promise.alldatagridreport.length > 0) {
                                          $scope.reporsmart = true;
                                          $scope.students = promise.alldatagridreport;
                                          $scope.presentCountgrid = $scope.students.length;
                                          $scope.exp_excel_flag = false;
                                          $scope.print_flag = false;
                                      }
                                      else {
                                          swal("Record Not Found !!");
                                          $state.reload();
                                          $scope.reporsmart = false;
                                      }
                                  })
                                }
                            }
                            else if ($scope.mallorindi === "mindi") {

                                if (obj.todate == null || obj.todate == undefined || obj.fromdate == null || obj.fromdate == undefined || obj.ASModule == null || obj.ASModule == undefined) {
                                    swal("Select The Required Fields !!");
                                    $scope.reporsmart = false;
                                }
                                else {
                                    apiService.create("StudentSmartCardLogReports/Getreportdetails", data).
                                  then(function (promise) {

                                      if (promise.alldatagridreport.length > 0) {
                                          $scope.reporsmart = true;
                                          $scope.students = promise.alldatagridreport;
                                          $scope.presentCountgrid = $scope.students.length;
                                          $scope.exp_excel_flag = false;
                                          $scope.print_flag = false;
                                      }
                                      else {
                                          $scope.reporsmart = false;
                                          swal("Record Not Found !!");
                                          $state.reload();
                                      }
                                  })
                                }
                            }
                        }

                        else {
                            swal("Record Not Found !!");
                            $scope.reporsmart = false;
                            $state.reload();
                            $scope.exp_excel_flag = false;
                            $scope.print_flag = false;
                        }
                    }
                }
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        };

        //fromdate to date validation
        $scope.validatetodate = function (data1) {
            $scope.todatef = false;
            $scope.todate1 = data1.fromdate;
            $scope.minDatet = new Date(
           $scope.todate1.getFullYear(),
           $scope.todate1.getMonth(),
          $scope.todate1.getDate() + 1);

            $scope.maxDatet = new Date(
          $scope.todate1.getFullYear(),
          $scope.todate1.getMonth(),
         $scope.todate1.getDate() + 1);

        }

        $scope.validatetodatetoo = function (datato) {

            $scope.todate2 = datato.todate;
            $scope.minDatet = new Date(
           $scope.todate2.getFullYear(),
           $scope.todate2.getMonth(),
          $scope.todate2.getDate() + 1);
        }

        $scope.validatetoday = function (datatodate) {
            $scope.todate3 = datatodate.dailydate;
            $scope.maxDateftodate = new Date(
           $scope.todate3.getFullYear(),
           $scope.todate3.getMonth(),
          $scope.todate3.getDate() + 1);
        }

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return angular.lowercase(obj.Name).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase(obj.Class).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                angular.lowercase(obj.Section).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase(obj.Modulename).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                ($filter('date')(obj.Mdate, 'dd/MM/yyyy').indexOf($scope.searchValue) >= 0);
        }

        $scope.printData = function (printSectionId) {
            if ($scope.printstudents !== null && $scope.students.length > 0) {
                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
              '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
          '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
           '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
          '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
          );
                popupWinindow.document.close();
            }
            else {
                swal("Please Select Records to be Printed");
            }
        }

        $scope.exportToExcel = function (tableId) {
            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
                //$state.reload();
            }
            else {
                swal("Please Select Records to be Exported");
            }

        }
    }
}
)();