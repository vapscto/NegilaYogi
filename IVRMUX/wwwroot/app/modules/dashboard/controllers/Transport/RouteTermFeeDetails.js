(function () {
    'use strict';
    angular
.module('app')
.controller('RouteTermFeeDetailsController', RouteTermFeeDetailsController)

    RouteTermFeeDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function RouteTermFeeDetailsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {
        $scope.sortKey = "astA_Id";
        $scope.columnsTest = [];
        $scope.obj = {};
        $scope.tadprint = false;
        $scope.exp_excel_flag = true;
        $scope.print_flag = true;
        $scope.printstudents = [];
        $scope.count12 = false;
        $scope.datecheck = false;
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

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.BindData = function () {
            apiService.getDATA("RouteTermFeeDetails/getdata").then(function (promise) {
                if (promise != null) {
                    $scope.YearList = promise.yearList;
                       $scope.termlist = promise.termlist;
                }
                else {
                    swal("No Records Found")
                }
            })
        }

        $scope.searchValue = '';
        $scope.griddeatails = false;

        $scope.filterValue = function (obj) {
            return angular.lowercase(obj.asmcL_ClassName).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                 angular.lowercase(obj.trmR_RouteName).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.griddata = [];
       
        $scope.cdeposit = [];
        $scope.getreport = function (obj) {

            $scope.all = "";
            $scope.searchValue = '';
            if ($scope.myForm.$valid) {
                if ($scope.datecheck==true) {
                    var data = {
                        "asmaY_Id": $scope.asmaY_Id,
                        "frmdate": $scope.frmdate,
                        "todate": $scope.todate,
                        "checkdate": $scope.datecheck,
                    }
                }
                else {
                    var data = {
                        "asmaY_Id": $scope.asmaY_Id
                    }
                }
                
                apiService.create("RouteTermFeeDetails/Getreportdetails", data).
                 then(function (promise) {
                     debugger;
                     if (promise.messagelist == null) {
                         $scope.reporsmart = false;
                         swal("Record Not Found !!");
                         $state.reload();
                     }
                     else {
                         $scope.students = promise.messagelist;
                         $scope.classarray = promise.classdata;
                         $scope.griddata = promise.griddata;
                         $scope.termlist = promise.termlist;
                         $scope.cdeposit = promise.cdeposit;
                         $scope.presentCountgrid = $scope.students.length;
                         $scope.exp_excel_flag = false;
                         $scope.print_flag = false;
                         $scope.temparray = [];
                         $scope.temparray2 = [];


                         $scope.chargearray1 = [];
                         $scope.recivedamtarray2 = [];
                         $scope.balancearray3 = [];
                         $scope.chargearray2 = [];
                         $scope.recivedamtarray3 = [];
                         $scope.balancearray4 = [];


                         if ($scope.griddata.length>0) {
                             $scope.griddeatails = true;
                         }
                         //if ($scope.cdeposit.length>0) {
                         //    $scope.griddeatails = true;
                         //}
                         $scope.presentCountgrid = 0;
                         angular.forEach($scope.griddata, function (t3) {
                             debugger;
                             $scope.chargearray1.push({ fmT_Id: t3.fmT_Id, trmR_Id: t3.trmR_Id, fsS_TotalToBePaid: t3.fsS_TotalToBePaid })
                             $scope.recivedamtarray2.push({ fmT_Id: t3.fmT_Id, trmR_Id: t3.trmR_Id, fsS_PaidAmount: t3.fsS_PaidAmount })
                             $scope.balancearray3.push({ fmT_Id: t3.fmT_Id, trmR_Id: t3.trmR_Id, fsS_ToBePaid: t3.fsS_ToBePaid })

                         })
                         console.log($scope.termlist)
                         console.log($scope.griddata)
                         console.log($scope.chargearray1)
                         angular.forEach($scope.cdeposit, function (t3) {
                             debugger;
                             $scope.chargearray2.push({  trmR_Id: t3.trmR_Id, fsS_TotalToBePaid: t3.fsS_TotalToBePaid })
                             $scope.recivedamtarray3.push({ trmR_Id: t3.trmR_Id, fsS_PaidAmount: t3.fsS_PaidAmount })
                             $scope.balancearray4.push({  trmR_Id: t3.trmR_Id, fsS_ToBePaid: t3.fsS_ToBePaid })

                         })                   

                         $scope.finalgriddata = [];
                         angular.forEach($scope.students, function (xx) {
                             var charge = [];
                             var recv = [];
                             var bal = [];
                             angular.forEach($scope.griddata, function (t1) {
                                
                                 if (xx.trmR_Id == t1.trmR_Id) {

                                     angular.forEach($scope.termlist, function (trm) {

                                         if (trm.fmT_Id==t1.fmT_Id) {
                                             charge.push({ fmT_Id: t1.fmT_Id, fsS_TotalToBePaid: t1.fsS_TotalToBePaid });
                                             recv.push({ fmT_Id: t1.fmT_Id, fsS_PaidAmount: t1.fsS_PaidAmount });
                                             bal.push({ fmT_Id: t1.fmT_Id, fsS_ToBePaid: t1.fsS_ToBePaid });


                                         }
                                     })

                                 }
                             })

                             var cdcharge = '';
                             var cdrecv = '';
                             var cdbale = '';

                             angular.forEach($scope.cdeposit, function (cd1) {
                               
                                 if (cd1.trmR_Id == xx.trmR_Id) {
                                     cdcharge = cd1.fsS_TotalToBePaid;
                                     cdrecv = cd1.fsS_PaidAmount;
                                     cdbale = cd1.fsS_ToBePaid;
                                 }
                             })

                             if (charge.length != 0 && recv.length != 0 && bal.length != 0) {
                                 $scope.presentCountgrid += 1;
                                 $scope.finalgriddata.push({ trmR_Id: xx.trmR_Id, trmR_RouteName: xx.trmR_RouteName, charge: charge, cdcharge: cdcharge, recv: recv, cdrecv: cdrecv, bal: bal, cdbale: cdbale })
                             }
                           

                         })


                         console.log($scope.finalgriddata);

                         angular.forEach($scope.termlist, function (t1) {
                             var rtcnt1 = 0;
                             var rtcnt2 = 0;
                             var rtcnt3 = 0;
                             angular.forEach($scope.griddata, function (t2) {

                                 if (t1.fmT_Id == t2.fmT_Id) {
                                     rtcnt1 += t2.fsS_TotalToBePaid;
                                     rtcnt2 += t2.fsS_PaidAmount;
                                     rtcnt3 += t2.fsS_ToBePaid;
                                 }

                             })
                             $scope.temparray2.push({ fmT_Id: t1.fmT_Id, total: rtcnt1, total1: rtcnt2, total2: rtcnt3})

                         })
                      
                         $scope.rtcnt1 = 0;
                         $scope.rtcnt2 = 0;
                         $scope.rtcnt3 = 0;
                             angular.forEach($scope.cdeposit, function (t2) {

                                 $scope.rtcnt1 += t2.fsS_TotalToBePaid;
                                 $scope.rtcnt2 += t2.fsS_PaidAmount;
                                 $scope.rtcnt3 += t2.fsS_ToBePaid;                      

                             })                         
                     }
                 })
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.printData = function () {

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

        $scope.exportToExcel = function (tabel1) {

            var exportHref = Excel.tableToExcel(tabel1, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        }

        $scope.validreport = function () {

            $scope.students = [];

        }
    };

})();