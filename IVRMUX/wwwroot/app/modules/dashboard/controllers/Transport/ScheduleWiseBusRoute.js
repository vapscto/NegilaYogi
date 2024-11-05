(function () {
    'use strict';
    angular
.module('app')
.controller('ScheduleWiseBusRouteController', ScheduleWiseBusRouteController)

    ScheduleWiseBusRouteController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function ScheduleWiseBusRouteController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {
      //  $scope.sortKey = "astA_Id";
        $scope.columnsTest = [];
        $scope.obj = {};
        $scope.tadprint = false;
        $scope.exp_excel_flag = true;
        $scope.print_flag = true;
        $scope.printstudents = [];
        $scope.count12 = false;

        $scope.usrname = localStorage.getItem('username');

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }

        $scope.cancel = function () {
            $scope.asmaY_Id = '';
            $scope.griddata = [];
            $scope.grid = false;

        }

        $scope.tadprint = false;

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
            apiService.getDATA("ScheduleWiseBusRoute/getdata").then(function (promise) {
                if (promise != null) {
                    $scope.YearList = promise.yearList;
                }
                else {
                    swal("No Records Found")
                }
            })
        }      

        $scope.searchValue = '';

        $scope.filterValue = function (obj) {
            return angular.lowercase(obj.asmcL_ClassName).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                 angular.lowercase(obj.trmR_RouteName).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.yearname = '';
        $scope.grid = false;
        $scope.griddata = [];
        $scope.getreport = function (obj) {
            $scope.grid = false;
            $scope.all = "";
            $scope.searchValue = '';          
            if ($scope.myForm.$valid) {
                var data =   {                       
                        "asmaY_Id": $scope.asmaY_Id                           

                    }
                apiService.create("ScheduleWiseBusRoute/Getreportdetails", data).
                 then(function (promise) {
                   
                     $scope.griddata = promise.griddata;
                     debugger;
                     angular.forEach($scope.YearList,function (vv) {
                         if ($scope.asmaY_Id == vv.asmaY_Id) {
                             $scope.yearname = vv.asmaY_Year;
                         }

                     })
                     console.log($scope.griddata)
                     if ($scope.griddata.length > 0) {
                         $scope.TwoWayPickupCounttotal = 0;
                         $scope.OneWayPickupCounttotal = 0;
                         $scope.OneWayDropCounttotal = 0;
                         $scope.schedule2DropCounttotal = 0;
                         $scope.schedule1DropCounttotal = 0;

                         angular.forEach($scope.griddata, function (aa) {
                             $scope.TwoWayPickupCounttotal +=  aa.TwoWayPickupCount;
                             $scope.OneWayPickupCounttotal +=  aa.OneWayPickupCount;
                             $scope.OneWayDropCounttotal +=    aa.OneWayDropCount;
                             $scope.schedule2DropCounttotal += aa.schedule2DropCount;
                             $scope.schedule1DropCounttotal += aa.schedule1DropCount;
                         })

                         $scope.selectedsection = [];
                         //for virtical total

                        



                         angular.forEach($scope.griddata, function (stu2) {

                             if ($scope.selectedsection.length == 0) {
                                 $scope.selectedsection.push({ routeid: stu2.routeid, RouteName: stu2.RouteName});
                             }
                             else if ($scope.selectedsection.length > 0) {
                                 var al_ct = 0;
                                 angular.forEach($scope.selectedsection, function (uf) {
                                     if (uf.routeid == stu2.routeid) {
                                         al_ct += 1;
                                     }
                                 })
                                 if (al_ct == 0) {
                                     $scope.selectedsection.push({ routeid: stu2.routeid, RouteName: stu2.RouteName });
                                 }
                             }


                         })

                         angular.forEach($scope.selectedsection, function (yy) {
                             var varticaltotal = 0;
                             angular.forEach($scope.griddata, function (xx) {
                                 if (xx.routeid == yy.routeid) {

                                     varticaltotal = varticaltotal + xx.TwoWayPickupCount+ xx.OneWayPickupCount + xx.OneWayDropCount + xx.schedule2DropCount + xx.schedule1DropCount;
                                 }
                             })
                             yy.vtotal = varticaltotal;

                         })       

                         console.log($scope.griddata)
                         console.log($scope.selectedsection)

                        $scope.finalvtotal = 0;
                         angular.forEach($scope.griddata, function (ff) {
                             angular.forEach($scope.selectedsection, function (zz) {
                                 if (zz.routeid == ff.routeid) {
                                     ff.vartotal = zz.vtotal;
                                     $scope.finalvtotal += zz.vtotal;
                                 }

                             })

                         })

                         $scope.grid = true;
                     }
                     else {
                         swal('No Record Found');
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
            '<link type="text/css" media="print" href="css/print/RegReportPdf.css" rel="stylesheet" />' +
           '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
           );
                popupWinindow.document.close();  
         
        }
        
        $scope.exportToExcel = function (export_id) {
            var exportHref = Excel.tableToExcel(export_id, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }

        $scope.validreport = function () {

            $scope.students = [];

        }
   };

})();