
(function () {
    'use strict';
    angular
.module('app')
        .controller('TRServiceBillReportController', TRServiceBillReportController)

    TRServiceBillReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', 'Excel','$timeout']
    function TRServiceBillReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, Excel, $timeout) {
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.masterlist = false;
       // $scope.sortKey = 'trdC_Id';
        $scope.sortReverse = true;
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };
        $scope.feeorder = false;
        $scope.feetext = true;
        $scope.searchValue = "";
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        $scope.tadprint = false;
        $scope.yerlist = [{ yearid: 1, name: '2016', start: new Date('01/01/1016'), end: new Date('12/31/1016') },
            { yearid: 2, name: '2017', start: new Date('01/01/2017'), end: new Date('12/31/2017') },
        { yearid: 3, name: '2018', start: new Date('01/01/2018'), end: new Date('12/31/2018') },
        { yearid: 4, name: '2019', start: new Date('01/01/2019'), end: new Date('12/31/2019') },
        { yearid: 5, name: '2020', start: new Date('01/01/2020'), end: new Date('12/31/2020') },
        { yearid: 6, name: '2021', start: new Date('01/01/2021'), end: new Date('12/31/2021') },
        { yearid: 7, name: '2022', start: new Date('01/01/2022'), end: new Date('12/31/2022') },
        { yearid: 8, name: '2023', start: new Date('01/01/2023'), end: new Date('12/31/2023') },
        { yearid: 9, name: '2024', start: new Date('01/01/2024'), end: new Date('12/31/2024') }]

        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("KMLogBook/getreportdata/", pageid).then(function (promise) {
                $scope.fillvehicletype = promise.fillvehicletype;
                $scope.monthlist = promise.monthlist;
                $scope.servnamelist = promise.servicestlist;
            })
        }


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

        $scope.firstfnc1 = function (aa) {

            $scope.allstdcheck = $scope.fillvahicleno.every(function (itm) { return itm.checkedgrplst1; });

        }

        $scope.headname = '';
        $scope.vehicletypechange = function ()
        {
            var data = {
                "TRMVT_Id": $scope.trmvT_Id,
            }
            apiService.create("DriverChartReport/vehicletypechange", data).
                then(function (promise) {

                    $scope.fillvahicleno = promise.fillvahicleno;


                });
               
            
        }
        $scope.TRMSST_Id = 0;
        $scope.perchasett = 0;
        $scope.labourtt = 0;
        $scope.totaltt = 0;
        $scope.tdsamttt = 0;
        $scope.disctt = 0;
        $scope.dedcntt = 0;
        $scope.grandtt = 0;
        $scope.masterdistancerate = [];
        $scope.valsstd = [];
        $scope.submitted = false;
        $scope.srvname = '';
        $scope.savedata = function () {

            $scope.srvname = '';
            $scope.perchasett = 0;
            $scope.labourtt = 0;
            $scope.totaltt = 0;
            $scope.tdsamttt = 0;
            $scope.disctt = 0;
            $scope.dedcntt = 0;
            $scope.grandtt = 0;
            $scope.valsstd = [];
            $scope.masterdistancerate = [];
            $scope.headname = '';
            if ($scope.myForm.$valid) {
                $scope.requisitionlistold = [];
                var startDateTime = new Date();
                var endDatetime = new Date();

                if ($scope.issuertype1 == 'yrw') {
                    angular.forEach($scope.yerlist, function (ff) {
                       
                        if (ff.yearid == $scope.asmay_id) {
                            startDateTime = new Date(ff.start);
                            endDatetime = new Date(ff.end);
                        }
                    })

                }
                else
                    if ($scope.issuertype1 == 'mnw') {
                        angular.forEach($scope.yerlist, function (ff) {
                         
                            if (ff.yearid == $scope.asmay_id) {

                                angular.forEach($scope.monthlist, function (zz) {
                                 
                                    if (zz.ivrM_Month_Name.trim() == $scope.ivrM_Month_Name.trim()) {
                                        startDateTime = $scope.ivrM_Month_Name + '/01/' + + ff.name;
                                        endDatetime = $scope.ivrM_Month_Name + '/' + zz.ivrM_Month_Max_Days + '/' + + ff.name;

                                        startDateTime = new Date(startDateTime);
                                        endDatetime = new Date(endDatetime);
                                        // alert(startDateTime)
                                        /// alert(endDatetime)
                                    }
                                }
                                )
                            }
                        })

                    }

                    else {
                        startDateTime = new Date($scope.TRKMLB_FromDate);
                        endDatetime = new Date($scope.TRKMLB_ToDate);
                    }


                var entrydate = startDateTime == null ? "" : $filter('date')(startDateTime, "yyyy-MM-dd");
                var fromdate1 = endDatetime == null ? "" : $filter('date')(endDatetime, "yyyy-MM-dd");
                $scope.valsstd = [];
                for (var u = 0; u < $scope.fillvahicleno.length; u++) {
                    if ($scope.fillvahicleno[u].checkedgrplst1 == true) {
                        $scope.valsstd.push($scope.fillvahicleno[u]);
                    }
                }
                if ($scope.valsstd.length > 0) {

                    var data = {
                        "TRKMLB_FromDate": entrydate,
                        "TRKMLB_ToDate": fromdate1,
                        "TRMSST_Id": $scope.TRMSST_Id,
                        "vhlid": $scope.valsstd,
                    }
                    apiService.create("MasterServiceStation/getbillreport/", data).then(function (promise) {

                        if (promise.requisitionlistold.length >0) {
                            $scope.requisitionlistold = promise.requisitionlistold;

                            angular.forEach($scope.requisitionlistold, function (yy) {

                                $scope.perchasett += yy.PurchaseBill;
                                $scope.labourtt += yy.Labourbill;
                                $scope.totaltt += yy.Total;
                                $scope.tdsamttt += yy.TDSAMOUNT;
                                $scope.disctt += yy.Discount;
                                $scope.dedcntt += yy.Totaldedn;
                                $scope.grandtt += yy.GrandTotal;
                            }
                                
                            )

                            angular.forEach($scope.servnamelist, function (ss) {
                                if ($scope.TRMSST_Id=='0') {
                                    $scope.srvname = 'ALL';
                                }

                                if (ss.trmssT_Id== $scope.TRMSST_Id) {
                                    $scope.srvname = ss.trmssT_ServiceStationName;
                                }
                                
                            })



                            if ($scope.issuertype1 == 'yrw') {

                                angular.forEach($scope.yerlist, function (gg) {
                                    if (gg.yearid == $scope.asmay_id) {
                                        $scope.headname = 'YEAR :' + ' ' + gg.name;

                                    }

                                })

                            }
                            else if($scope.issuertype1 == 'mnw') {

                                angular.forEach($scope.yerlist, function (gg) {
                                    if (gg.yearid == $scope.asmay_id) {
                                        $scope.headname = 'YEAR :' + ' ' + gg.name;

                                    }

                                })

                                angular.forEach($scope.monthlist, function (hh) {
                                    if (hh.ivrM_Month_Name.trim() == $scope.ivrM_Month_Name.trim()) {
                                        $scope.headname = $scope.headname +' , '+ 'MONTH :' + ' ' + hh.ivrM_Month_Name;

                                    }

                                })

                            }
                            else {

                                var fromhddate = $scope.TRKMLB_FromDate == null ? "" : $filter('date')($scope.TRKMLB_FromDate, "yyyy-MM-dd");
                                var fromhddate1 = $scope.TRKMLB_ToDate == null ? "" : $filter('date')($scope.TRKMLB_ToDate, "yyyy-MM-dd");
                                $scope.headname = 'FROM DATE BETWEEN' + '  ' + fromhddate +' ' +'  ' + 'AND' + '  '  + fromhddate1;
                            }

                        }
                        else {
                            swal('No Record Found')
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

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

      

        $scope.checkvalid1 = function () {
            $scope.TRDC_ToKM = parseInt($scope.TRDC_FromKM) + 1;
        }

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }


        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            
            return (JSON.stringify(obj.trdC_FromKM)).indexOf($scope.searchValue) >= 0 ||
                (JSON.stringify(obj.trdC_ToKM)).indexOf($scope.searchValue) >= 0 ||
                (JSON.stringify(obj.trdC_RateKm)).indexOf($scope.searchValue) >= 0
        }

        $scope.clear = function () {
            $state.reload();
        }


        $scope.printData = function () {

            var innerContents = document.getElementById("printareaId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 22100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();

        }

        $scope.exportToExcel = function (tabel1) {
            debugger;
            var exportHref = Excel.tableToExcel(tabel1, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        }
    };
})();


