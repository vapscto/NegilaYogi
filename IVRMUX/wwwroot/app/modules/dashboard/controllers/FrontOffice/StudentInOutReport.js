(function () {
    'use strict';
    angular.module('app').controller('StudentInOutReportController', StudentInOutReportController)

    StudentInOutReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$timeout', 'Excel', '$q']
    function StudentInOutReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $timeout, Excel, $q) {



        $scope.submitted1 = false;
        $scope.indentapproveddetais = [];
        $scope.maxdate = new Date();
        $scope.obj = {};
        var data = {}
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        var temp = [];
        var year = "";
        $scope.yearfromdate = "";
        $scope.monthlist_temp = [];
        $scope.consolidata_id = [];
        $scope.get_deviationreport = [];
        $scope.imgname = logopath;
        $scope.data = [];


        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            $scope.flags = "";

            var pageid = 2;
            apiService.getURI("StudentInOutReport/loaddata", pageid).then(function (promise) {

                $scope.classlist = promise.classlist;
                $scope.All_Individual();



            });
        };


        $scope.al_checkclass = function (all, ASMCL_Id) {



            $scope.classlistarray = [];
            $scope.obj.usercheckCC = all;

            var toggleStatus = $scope.obj.usercheckCC;
            angular.forEach($scope.classlist, function (role) {
                role.selected = toggleStatus;
            });


            $scope.classlistarray = [];
            angular.forEach($scope.classlist, function (qq) {
                if (qq.selected == true) {
                    $scope.classlistarray.push({ ASMCL_Id: qq.asmcL_Id, ASMCL_Id: qq.asmcL_Id })
                }
            });


            if ($scope.obj.usercheckCC == true) {
                $scope.getsection();
                $scope.classflag = true;
            }
            else {
                $scope.sectionlist = [];

            }

        }

        $scope.classlistarray = [];
        $scope.getsection = function (ASMCL_Id) {


            

            angular.forEach($scope.classlist, function (aa) {
                if (aa.selected == true) {
                    $scope.classlistarray.push({ ASMCL_Id: aa.asmcL_Id })
                }

            });

            if ($scope.classlistarray != null) {
                $scope.classflag = true;
            }

            var data = {
                "classlsttwo": $scope.classlistarray
            }
            apiService.create("StudentInOutReport/getsection", data).then(function (promise) {
                if (promise.sectionlist.length > 0 || promise.sectionlist != null) {
                    $scope.sectionlist = promise.sectionlist;
                    $scope.getclass = false;
                }
                else {
                    swal('No data Found!!!');
                }
            });
        }
            $scope.all_checkC = function (all, ASMCL_Id) {
                $scope.sectionlistarray = [];
                $scope.obj.usercheckC = all;
                var toggleStatus = $scope.obj.usercheckC;
                angular.forEach($scope.sectionlist, function (role) {
                    role.selected = toggleStatus;
                });

                $scope.sectionlistarray = [];
                angular.forEach($scope.sectionlist, function (qq) {
                    if (qq.selected == true) {
                        $scope.sectionlistarray.push({ ASMS_Id: qq.asmS_Id, ASMCL_Id: qq.asmcL_Id })

                       
                    }
                });

                $scope.getstudent();
        }



            $scope.getstudent = function () {
                $scope.studentlist = [];
                if ($scope.obj.fee_def == true) {
                    $scope.defgrparray = [];
                    angular.forEach($scope.feegrplist, function (qq) {
                        if (qq.selected == true) {
                            $scope.defgrparray.push({ FMG_Id: qq.fmG_Id })
                        }
                    });

                    $scope.deflistarray = [];
                    angular.forEach($scope.feedeflist, function (qq) {
                        if (qq.selected == true) {
                            $scope.deflistarray.push({ FMT_Id: qq.fmT_Id })
                        }
                    });
                }

               
                $scope.classlistarray = [];

                if ($scope.classlistarray.length == 0 || $scope.classlistarray == null) {
                    angular.forEach($scope.classlist, function (qq) {
                        if (qq.selected == true) {
                            $scope.classlistarray.push({ ASMCL_Id: qq.asmcL_Id })

                        }
                    });
                }

                if ($scope.sectionlist != null && $scope.sectionlist.length > 0) {
                    $scope.sectionlistarray = [];
                    angular.forEach($scope.sectionlist, function (qq) {
                        if (qq.selected == true) {
                            $scope.sectionlistarray.push({ ASMS_Id: qq.asmS_Id, ASMCL_Id: qq.asmcL_Id })
                           
                        }
                    });
                }

               
                var data = {
                   
                    "classlsttwo": $scope.classlistarray,
                    "sectionlistarray": $scope.sectionlistarray,
                    
                }
                apiService.create("StudentInOutReport/getstudent", data).then(function (promise) {
                    $scope.studentlist1 = [];
                    $scope.studentlist = [];
                    $scope.studentlist1 = promise.studentlist1;
                    if ($scope.studentlist1.length > 0 || $scope.studentlist1 != null) {
                        $scope.studentlist = $scope.studentlist1;
                    }
                    else {
                        swal('No Data Found!!!')
                    }
                })
              
            }

        $scope.All_Individual = function () {

            if ($scope.allind == 'individual')
                $scope.disabledata = false;
            else
               $scope.disabledata = true;

        }


        $scope.interacted = function (field) {            return $scope.submitted;        };

        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.exportToExceldetails = function (export_id) {
            var exportHref = Excel.tableToExcel(export_id, 'Student Report');
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };

        $scope.isOptionsRequired3 = function () {

            return !$scope.classlist.some(function (options) {
                return options.selected;
            });
        }

        $scope.isOptionsRequired1 = function () {

            return !$scope.sectionlist.some(function (options) {
                return options.selected;
            });
        }


        $scope.get_Report = function () {


           


            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var AMST_Id = 0;
                $scope.from_date = new Date($scope.fromdate).toDateString();
                $scope.to_date = new Date($scope.todate).toDateString();

                if ($scope.allind=="All") {
                    AMST_Id = 0;
                }
                else {
                   
                    AMST_Id = $scope.AMST_Id.amsT_Id;
                }

                var data = {
                    "classlsttwo": $scope.classlistarray,
                    "sectionlistarray": $scope.sectionlistarray,
                    "optionflag": $scope.allind,
                    "inoutflag": $scope.rdopunch,
                    "AMST_Id": AMST_Id,
                    "fromdate": $scope.from_date,
                    "todate": $scope.to_date
                } 

                apiService.create("StudentInOutReport/report", data).then(function (promise) {
                    if (promise.viewlist != null && promise.viewlist.length > 0) {
                        
                        $scope.viewlist = promise.viewlist;


                    }

                    else {
                        swal("No Record  Found..... !!");

                    }

                });

            }

            else {
                $scope.submitted = true;
            }

        }

        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("printDeviation").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };



    }
})();