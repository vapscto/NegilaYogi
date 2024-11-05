(function () {
    'use strict';
    angular
.module('app')
.controller('TransportReportController', TransportReportController)

    TransportReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function TransportReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {

        $scope.columnsTest = [];
        $scope.obj = {};
        $scope.tadprint = false;
        $scope.exp_excel_flag = true;
        $scope.print_flag = true;
        $scope.printstudents = [];

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
        if ($scope.itemsPerPage == undefined || $scope.itemsPerPage == null) {
            $scope.itemsPerPage = 5;
        }
        $scope.currentPage = 1;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        $scope.reporsmart = false;


        $scope.cancel = function () {
            $state.reload();
        }
        $scope.onclickloaddata = function () {
            if ($scope.allorindiv == "All") {

                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.regnonamestd = true;
                $scope.reporsmart = false;
            }
            else if ($scope.allorindiv === "indi") {
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
                $scope.regnonamestd = false;
                $scope.reporsmart = false;
            }
            $scope.students = [];
            $scope.printstudents = [];
        };


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
            if ($scope.students.length === 0 && $scope.printstudents.length > 0) {
                angular.forEach($scope.printstudents, function (itm) {
                    $scope.printstudents.splice(itm);
                });
            }
        }

        $scope.searchValue = '';

        $scope.filterValue = function () {
            if (trmA_ActiveFlg == true) {
                $scope.test = "Active";
            } else if (trmA_ActiveFlg == false) {
                $scope.test = "Deactive";
            }
            return angular.lowercase(trmA_AreaName).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        }


        $scope.getreport = function (obj) {
            
            $scope.all = "";
            $scope.searchValue = '';
           // $scope.toggleAll();
            $scope.students = [];
            if ($scope.myForm.$valid) {
                if ($scope.allorindiv == null || $scope.allorindiv == undefined) {
                    swal("Kindly Select The Options !!");
                }
                else {

                    var data =
                        {
                            "onclickloaddata": $scope.allorindiv,
                            "regorname_m": $scope.regorname_m,
                            "regorname_map": $scope.regorname_map,

                        }

                    if ($scope.allorindiv == "All") {
                        if ($scope.regorname_m == null || $scope.regorname_m == undefined) {
                            swal("Select The Required Fields !!");
                            $scope.reporsmart = false;
                        }
                        else {
                            apiService.create("TransportReport/Getreportdetails", data).
                          then(function (promise) {
                              if (promise.messagelist.length == 0) {
                                  $scope.reporsmart = false;
                                  swal("Record Not Found !!");
                                  $state.reload();

                              }
                              else {
                                  
                                  $scope.students = promise.messagelist;
                                  $scope.presentCountgrid = $scope.students.length;
                                  $scope.reporsmart = true;
                                  $scope.exp_excel_flag = false;
                                  $scope.print_flag = false;
                                  $scope.name_display = $scope.regorname_m;
                              }
                          })
                        }
                    }

                    else if ($scope.allorindiv === "indi") {

                        if ($scope.regorname_map == null || $scope.regorname_map == undefined) {
                            swal("Select The Required Fields !!");
                            $scope.reporsmart = false;
                        }
                        else {
                            apiService.create("TransportReport/Getreportdetails", data).
                              then(function (promise) {
                                  if (promise.messagelist.length > 0) {
                                      $scope.reporsmart = true;
                                      $scope.students = promise.messagelist;
                                      $scope.presentCountgrid = $scope.students.length;
                                      $scope.exp_excel_flag = false;
                                      $scope.print_flag = false;
                                      $scope.name_display = $scope.regorname_map;
                                  }
                                  else {
                                      swal("Record Not Found !!");
                                      $state.reload();
                                      $scope.reporsmart = false;
                                  }
                              })
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

            else {
                $scope.reporsmart = false;
                $state.reload();
                swal("Record Not Found !!");
            }

        }

        $scope.printData = function () {
            if ($scope.printstudents !== null && $scope.students.length > 0) {
                var innerContents = "";

                if ($scope.allorindiv == "All") {
                    if ($scope.allorindiv == "All" && $scope.regorname_m == "masterarea") {
                        innerContents = document.getElementById("printareaId").innerHTML;
                    }
                    else if ($scope.allorindiv == "All" && $scope.regorname_m == "masterdriver") {
                        innerContents = document.getElementById("printdriverid").innerHTML;
                    }
                    else if ($scope.allorindiv == "All" && $scope.regorname_m == "masterfueltype") {
                        innerContents = document.getElementById("printfuelid").innerHTML;
                    }

                    else if ($scope.allorindiv == "All" && $scope.regorname_m == "masterlocation") {
                        innerContents = document.getElementById("printLocationid").innerHTML;
                    }
                    else if ($scope.allorindiv == "All" && $scope.regorname_m == "masterroute") {
                        innerContents = document.getElementById("printrid").innerHTML;
                    }
                    else if ($scope.allorindiv == "All" && $scope.regorname_m == "mastervehical") {
                        innerContents = document.getElementById("printvid").innerHTML;
                    }

                    else if ($scope.allorindiv == "All" && $scope.regorname_m == "mastervehicaltype") {
                        innerContents = document.getElementById("printvtid").innerHTML;
                    }

                    else if ($scope.allorindiv == "All" && $scope.regorname_m == "mastersession") {
                        innerContents = document.getElementById("printsid").innerHTML;
                    }
                    else if ($scope.allorindiv == "All" && $scope.regorname_m == "masterrouteschedule") {
                        innerContents = document.getElementById("printrsid").innerHTML;
                    }
                }

                else if ($scope.allorindiv == "indi") {
                    if ($scope.regorname_map == 'routelocmap') {
                        innerContents = document.getElementById("printrlid").innerHTML;
                    }
                    else if ($scope.regorname_map == "vehicaldrivermap") {
                        innerContents = document.getElementById("printVDid").innerHTML;
                    }

                    else if ($scope.regorname_map == "vehicaldriversubstitute") {
                        innerContents = document.getElementById("printVDSid").innerHTML;
                    }

                    else if ($scope.regorname_map == "vehicalroutemap") {
                        innerContents = document.getElementById("printVRid").innerHTML;
                    }

                }

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

        $scope.exportToExcel = function () {
            var printSectionId = '';

            if ($scope.allorindiv == 'All') {
                if ($scope.allorindiv == 'All' && $scope.regorname_m == 'masterarea' && $scope.students.length > 0) {
                    printSectionId = '#table1';
                }

                else if ($scope.allorindiv == 'All' && $scope.regorname_m == 'masterdriver' && $scope.students.length > 0) {
                    printSectionId = '#table2';
                }

                else if ($scope.allorindiv == 'All' && $scope.regorname_m == 'masterfueltype' && $scope.students.length > 0) {
                    printSectionId = '#table3';
                }
                else if ($scope.allorindiv == 'All' && $scope.regorname_m == 'masterlocation' && $scope.students.length > 0) {
                    printSectionId = '#table4';
                }
                else if ($scope.allorindiv == 'All' && $scope.regorname_m == 'masterroute' && $scope.students.length > 0) {
                    printSectionId = '#table5';
                }

                else if ($scope.allorindiv == 'All' && $scope.regorname_m == 'mastervehical' && $scope.students.length > 0) {
                    printSectionId = '#table6';
                }

                else if ($scope.allorindiv == 'All' && $scope.regorname_m == 'mastervehicaltype' && $scope.students.length > 0) {
                    printSectionId = '#table7';
                }
                else if ($scope.allorindiv == 'All' && $scope.regorname_m == 'mastersession' && $scope.students.length > 0) {
                    printSectionId = '#table8';
                }
                else if ($scope.allorindiv == 'All' && $scope.regorname_m == 'masterrouteschedule' && $scope.students.length > 0) {
                    printSectionId = '#table9';
                }
            }

            else if ($scope.allorindiv == 'indi') {
                if ($scope.regorname_map == 'routelocmap' && $scope.students.length > 0) {
                    printSectionId = '#table10';
                }
                else if ($scope.regorname_map == 'vehicaldrivermap' && $scope.students.length > 0) {
                    printSectionId = '#table11';
                }
                else if ($scope.regorname_map == 'vehicaldriversubstitute' && $scope.students.length > 0) {
                    printSectionId = '#table12';
                }
                else if ($scope.regorname_map == 'vehicalroutemap' && $scope.students.length > 0) {
                    printSectionId = '#table13';
                }

            }

            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
                var exportHref = Excel.tableToExcel(printSectionId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
                //$state.reload();
            }
            else {
                swal("Please Select Records to be Exported");
            }

        }

        $scope.validreport = function () {

            $scope.students = [];
            $scope.printstudents = [];


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
    };

})();