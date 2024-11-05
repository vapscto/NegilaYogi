(function () {
    'use strict';
    angular
        .module('app')
        .controller('CLGBusRoutesDetails', CLGBusRoutesDetails)

    CLGBusRoutesDetails.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function CLGBusRoutesDetails($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {

        $scope.loaddata = function () {        
            var pageid = 2;
            apiService.getURI("CLGBusRoutesDetails/loaddata", pageid).then(function (promise) {
                $scope.yearlist = promise.yearlist;
                $scope.courselist = promise.courselist;
                
            })
        }

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
        $scope.reporsmart1 = false;

        $scope.getbranch = function () {
            $scope.all = "";
            $scope.AMB_Id = '';
            $scope.semesterlist = [];
            $scope.sectionlist = [];
            $scope.AMSE_Id = '';
          
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id
            }
            apiService.create("CLGBusRoutesDetails/getbranch", data).then(function (promise) {            
                $scope.branchlist = promise.branchlist;
            })
        }
        $scope.getsemester = function () {
            $scope.all = "";
            $scope.sectionlist = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id
            }
            apiService.create("CLGBusRoutesDetails/getsemester", data).then(function (promise) {             
                $scope.semesterlist = promise.semesterlist;
            })
        }

        $scope.cancel = function () {
            $state.reload();
        }
        $scope.submitted = false;

        $scope.printData = function () {
            
            if ($scope.type == 'stdcount') {
                var innerContents = document.getElementById("printareaId").innerHTML;
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
                var innerContents = document.getElementById("printareaId1").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/RegReportPdf.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
            }
        }
        $scope.exportToExcel = function () {
            var exp = '';
            if ($scope.type == 'stdcount') {
                exp = '#export_id'
            }
            else {
                exp = '#export_id1'
            }
            var exportHref = Excel.tableToExcel(exp, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);          
        }

        $scope.interacted = function () {
            return $scope.submitted;
        }
        $scope.reporsmart1 = false;
        $scope.onchangeradio = function () {
            $scope.studentgriddata = [];
            
            $scope.detailsgrid = false;
        }
        $scope.onchangeradio2 = function () {
          
            $scope.reporsmart1 = false;
            $scope.students = [];
        }
        $scope.getreport = function () {       
            if ($scope.myForm.$valid) {
                if ($scope.type == 'stdcount') {
                    $scope.AMSE_Id = '';
                    $scope.all = "";
                    var data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "AMCO_Id": $scope.AMCO_Id,
                        "AMB_Id": $scope.AMB_Id,
                        "flag": $scope.allorind,
                        "type": $scope.type
                    }
                }
                else {
                    $scope.reporsmart1 = true;
                    $scope.newseclist = [];
                    angular.forEach($scope.semesterlist, function (ss) {
                        if ($scope.AMSE_Id == '0') {
                            $scope.newseclist.push(ss);
                        }
                        else if ($scope.AMSE_Id == ss.amsE_Id) {
                            $scope.newseclist.push(ss);
                        }
                    })

                    var data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "AMCO_Id": $scope.AMCO_Id,
                        "AMB_Id": $scope.AMB_Id,
                        "secidlist": $scope.newseclist,
                        "flag": $scope.allorind,
                        "type": $scope.type
                    }
                }
            apiService.create("CLGBusRoutesDetails/getreport", data).then(function (promise) {      
                if ($scope.type == 'stdcount') {
                    if (promise.griddata == null) {
                        $scope.reporsmart = false;
                        $scope.reporsmart1 = false;
                        swal("Record Not Found !!");
                        //$state.reload();
                    }
                    else {
                        $scope.reporsmart1 = false;
                        angular.forEach($scope.YearList, function (yy) {
                            if (yy.asmaY_Id == $scope.asmaY_Id) {
                                $scope.year = yy.asmaY_Year;
                            }
                        })
                        if ($scope.allorind == "Approved") {
                            $scope.status = " Approved List ";
                        }
                        else if ($scope.allorind == "Waiting") {
                            $scope.status = "Waiting List";
                        }

                        else if ($scope.allorind == "Rejected") {
                            $scope.status = "Rejected List";
                        }

                        else {
                            $scope.status = "";
                        }
                        $scope.reporsmart = true;
                       $scope.reporsmart1 = true;
                        $scope.students = promise.griddata;
                        $scope.semesterarray = promise.semesterlist;

                        $scope.presentCountgrid = $scope.students.length;
                        angular.forEach($scope.students, function (y) {
                       
                            var total_days = 0;
                            angular.forEach($scope.semesterarray, function (z) {
                                total_days += y[z.amsE_SEMName];
                            })
                            y.Total = total_days;
                        })
                        $scope.exp_excel_flag = false;
                        $scope.print_flag = false;
                        $scope.temparray = [];
                        $scope.temparray2 = [];
                        angular.forEach($scope.semesterarray, function (y) {
                            var rtcnt1 = 0;
                            angular.forEach($scope.students, function (z) {
                                rtcnt1 += z[y.amsE_SEMName]
                            })
                              //y.Total = total_days;
                            $scope.temparray2.push({ amsE_SEMName: y.amsE_SEMName, total: rtcnt1 })
                        })
                        $scope.total12 = 0;
                        angular.forEach($scope.temparray2, function (t2) {
                            $scope.total12 += t2.total
                        })
                    }
                }








                else {
                    if ($scope.allorind == "Approved") {
                        $scope.status = " Approved List ";
                    }
                    else if ($scope.allorind == "Waiting") {
                        $scope.status = "Waiting List";
                    }

                    else if ($scope.allorind == "Rejected") {
                        $scope.status = "Rejected List";
                    }

                    else {
                        $scope.status = "";
                    }
                    //added praveen
                    $scope.detailsgrid = true;
                    $scope.studentgriddata = promise.studentgriddata;
                    if ($scope.studentgriddata.length > 0) {
                     
                        $scope.selectedsection = [];
                        $scope.reporsmart = true;
                        $scope.reporsmart1 = false;

                        $scope.reporsmart2 = true;
                        $scope.detailsgrid = true;
                        angular.forEach($scope.studentgriddata, function (stu2) {

                            if ($scope.selectedsection.length == 0) {
                                $scope.selectedsection.push({ TRMR_Id: stu2.TRMR_Id, TRMR_RouteName: stu2.TRMR_RouteName, trmr_order: stu2.trmr_order });
                            }
                            else if ($scope.selectedsection.length > 0) {
                                var al_ct = 0;
                                angular.forEach($scope.selectedsection, function (uf) {
                                    if (uf.TRMR_Id == stu2.TRMR_Id) {
                                        al_ct += 1;
                                    }
                                })
                                if (al_ct == 0) {
                                    $scope.selectedsection.push({ TRMR_Id: stu2.TRMR_Id, TRMR_RouteName: stu2.TRMR_RouteName, trmr_order: stu2.trmr_order });
                                }
                            }
                        })
                        $scope.mainarray = [];
                        angular.forEach($scope.selectedsection, function (ff) {
                            var listtemparray = [];
                            angular.forEach($scope.studentgriddata, function (ac) {
                                if (ac.TRMR_Id == ff.TRMR_Id) {
                                    listtemparray.push(ac)
                                }
                            })
                            $scope.mainarray.push({ TRMR_Id: ff.TRMR_Id, TRMR_RouteName: ff.TRMR_RouteName, trmr_order: ff.trmr_order, arraylist: listtemparray });
                        })
                    }
                    else {
                    
                        $scope.detailsgrid = false;
                        swal('No Record Found');
                    }
                }



                })
            }
            else {
              $scope.submitted = true;
            }
        }

        $scope.onchangeyear = function () {
            $scope.AMCO_Id = '';
            $scope.branchlist = [];
            $scope.semesterlist = [];
            $scope.sectionlist = [];
            $scope.AMB_Id = '';
            $scope.AMSE_Id = '';
           
        }
    }
})();