(function () {
    'use strict';
    angular
.module('app')
        .controller('PartParticularReportController', PartParticularReportController)

    PartParticularReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function PartParticularReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {
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
        $scope.ddate = new Date;

        //var id = 1;
        //$scope.itemsPerPage = 10;
        //$scope.currentPage = 1;
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
            var pageid = 1;
            apiService.getURI("MasterServiceStation/getrptparam", pageid).then(function (promise) {
                
                $scope.fillvahicletype = promise.fillvahicletype;
                $scope.servnamelist = promise.servnamelist;
               
            })
        }
        $scope.TRMSST_Id = 0;
        $scope.headname = '';
        $scope.vehicletypechange = function () {
            var data = {
                "TRMVT_Id": $scope.trmvT_Id,
            }
            apiService.create("DriverChartReport/vehicletypechange", data).
                then(function (promise) {

                    $scope.fillvahicleno = promise.fillvahicleno;


                });


        }
        $scope.cancel = function () {
            //$scope.searchValue = '';
            //$scope.frmdate = '';
            //$scope.todate = '';
            //$scope.griddata = [];
            //$scope.griddeatails = false;
            //$scope.griddata.length = 0;
            $state.reload();
            
        }

        $scope.searchValue = '';
        $scope.searchValue1 = '';
        $scope.griddeatails = false;
        $scope.getloaddata = [];

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.griddata = [];
       
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.allstudentcheck = function () {

            angular.forEach($scope.fillvahicleno, function (ff) {
                if ($scope.allstdcheck == true) {
                    ff.checkedgrplst1 = true;

                }
                else {
                    ff.checkedgrplst1 = false;
                }


            })


        }

        $scope.checkboxchange = function () {

            $scope.requisitionlistold = [];
        }

        $scope.firstfnc1 = function (aa) {

            $scope.allstdcheck = $scope.fillvahicleno.every(function (itm) { return itm.checkedgrplst1; });

        }

        $scope.valsstd = [];
        $scope.cdeposit = [];
    $scope.requisitionlistold = [];
        $scope.statuscount = false;
        $scope.getreport = function () {
            debugger;
            $scope.requisitionlistold = [];
            $scope.all = "";
            $scope.searchValue = '';
            if ($scope.myForm.$valid) {
                $scope.valsstd = [];
                for (var u = 0; u < $scope.fillvahicleno.length; u++) {
                    if ($scope.fillvahicleno[u].checkedgrplst1 == true) {
                        $scope.valsstd.push($scope.fillvahicleno[u]);
                    }
                }
                if ($scope.valsstd.length > 0) {
                var fromdate1 = $scope.frmdate == null ? "" : $filter('date')($scope.frmdate, "yyyy-MM-dd");
                var todate1 = $scope.todate == null ? "" : $filter('date')($scope.todate, "yyyy-MM-dd");
        
                    var data = {
                        "FRMDATE": fromdate1,
                        "TODATE": todate1,
                        "TRMSST_Id": $scope.TRMSST_Id,
                        "vhlid": $scope.valsstd,
                        "statuscount": $scope.statuscount,
                    }
            
              
               
              
                
                apiService.create("MasterServiceStation/Getreportdetails", data).
                 then(function (promise) {
                     debugger;
                     $scope.requisitionlistold = promise.requisitionlistold;

                     console.log($scope.requisitionlistold)

                     if ($scope.requisitionlistold.length > 0) {
                         if ($scope.statuscount == true) {

                             $scope.serivicelist = [];
                             $scope.qtytotal = 0;
                             $scope.amttotal = 0;
                             $scope.disctotal = 0;
                             angular.forEach($scope.requisitionlistold, function (st) {
                                 if ($scope.serivicelist.length == 0) {
                                   
                                     $scope.serivicelist.push({ TRSE_Id: st.TRSE_Id, TRSE_ServiceRefNo: st.TRSE_ServiceRefNo, TRSE_ProblemsListed: st.TRSE_ProblemsListed, TRSE_ServiceDate: st.TRSE_ServiceDate, TRSE_ServiceDetails: st.TRSE_ServiceDetails, TRMD_DriverName: st.TRMD_DriverName, TRMV_VehicleNo: st.TRMV_VehicleNo, TRMSST_ServiceStationName: st.TRMSST_ServiceStationName });
                                 }
                                 else if ($scope.serivicelist.length > 0) {
                                     var al_exm_cnt = 0;
                                     angular.forEach($scope.serivicelist, function (exm) {
                                         if (exm.TRSE_Id == st.TRSE_Id) {
                                             al_exm_cnt += 1;
                                         }
                                     })
                                     if (al_exm_cnt == 0) {
                                     
                                         $scope.serivicelist.push({ TRSE_Id: st.TRSE_Id, TRSE_ServiceRefNo: st.TRSE_ServiceRefNo, TRSE_ProblemsListed: st.TRSE_ProblemsListed, TRSE_ServiceDate: st.TRSE_ServiceDate, TRSE_ServiceDetails: st.TRSE_ServiceDetails, TRMD_DriverName: st.TRMD_DriverName, TRMV_VehicleNo: st.TRMV_VehicleNo, TRMSST_ServiceStationName: st.TRMSST_ServiceStationName });
                                     }
                                 }

                                 $scope.qtytotal += st.TRSED_Qty;
                                 $scope.amttotal += st.TRSED_Amount;
                                 $scope.disctotal += st.TRSED_TotalDiscount;

                             })


                             $scope.arraylast = [];
                             angular.forEach($scope.serivicelist, function (pp) {
                                 var items = [];
                                 angular.forEach($scope.requisitionlistold, function (ss) {
                                     if (ss.TRSE_Id == pp.TRSE_Id) {
                                         items.push({ TRSE_Id: ss.TRSE_Id, TRSED_ItemsName: ss.TRSED_ItemsName, TRSED_Qty: ss.TRSED_Qty, TRSED_Amount: ss.TRSED_Amount, TRSED_TotalDiscount: ss.TRSED_TotalDiscount, TRSED_Remarks: ss.TRSED_Remarks })
                                     }

                                 })

                                 $scope.arraylast.push({ TRSE_Id: pp.TRSE_Id, TRSE_ServiceRefNo: pp.TRSE_ServiceRefNo, TRSE_ProblemsListed: pp.TRSE_ProblemsListed, TRSE_ServiceDate: pp.TRSE_ServiceDate, TRSE_ServiceDetails: pp.TRSE_ServiceDetails, TRMD_DriverName: pp.TRMD_DriverName, TRMV_VehicleNo: pp.TRMV_VehicleNo, TRMSST_ServiceStationName: pp.TRMSST_ServiceStationName, itemlists: items})

                             })
                         }

                         console.log($scope.serivicelist);
                         console.log($scope.arraylast);
                         $scope.reporsmart = true;
                         $scope.griddeatails = true;
                     }
                     else {
                         $scope.reporsmart = false;
                         swal("Record Not Found !!");
                         $state.reload();
                     }
                    
                        })
                }
                else {
                    swal('Select Vehicle Number')
                }
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.printData = function () {

            var tableid = '';
            if ($scope.statuscount == true) {
                tableid = 'printareaId1';
            }
            else {
                tableid = 'printareaId';
            }

            var innerContents = document.getElementById(tableid).innerHTML;
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
            var tableid = '';
            if ($scope.statuscount == true) {
                tableid = '#table11';
            }
            else {
                tableid = '#table1';
            }

            var exportHref = Excel.tableToExcel(tableid, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        }

        $scope.validreport = function () {

            $scope.students = [];

        }
    };

})();