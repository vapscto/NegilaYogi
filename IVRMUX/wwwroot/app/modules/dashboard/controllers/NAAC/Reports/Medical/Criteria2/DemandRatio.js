(function () {
    'use strict';
    angular.module('app').controller('DemandRatioController', DemandRatioController)

    DemandRatioController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout']
    function DemandRatioController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {


        $scope.printflag = false;
        $scope.showflag = false;

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        //$scope.searchValue23 = "";

        $scope.sortReverse = true;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        

        $scope.loaddata = function () {
            $scope.printflag = false;
            $scope.showflag = false;
            var pageid = 2;
            apiService.getURI("DisabilityStudent/getdata", pageid).then(function (promise) {
                $scope.noofyear = 2;
                $scope.yearlist = promise.yearlist;

                //$scope.yearlist = [
                //    { asmaY_Year: "2018-2019", asmaY_Id: 5 },
                //    { asmaY_Year: "2017-2018", asmaY_Id: 4 },
                //    { asmaY_Year: "2016-2017", asmaY_Id: 3 },
                //    { asmaY_Year: "2015-2016", asmaY_Id: 2 },
                //    { asmaY_Year: "2014-2015", asmaY_Id: 1 },
                //]

                var s = 0;
                angular.forEach($scope.yearlist, function (pp) {
                    if (s < $scope.noofyear) {
                        pp.select = true;
                    }
                    s += 1;
                })
            })
        }


        $scope.get_selcetYear = function (data) {

            var nofyear = Number($scope.noofyear);
            angular.forEach($scope.yearlist, function (tt) {
                tt.select = false;
            })
            var s = 0;
            angular.forEach($scope.yearlist, function (pp) {
                if (s < nofyear) {
                    pp.select = true;
                }
                s += 1;
            })

        }

        $scope.togchkbx = function () {

            $scope.noofyear = 0;
            angular.forEach($scope.yearlist, function (ff) {
                if (ff.select == true) {
                    $scope.noofyear += 1;
                }

            })
        }

        //============================For Print Option
        $scope.printData = function () {
            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Library/ExchangableCoursePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
        $scope.ddate = new Date();

        //$scope.usrname = localStorage.getItem('username');
        //var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //var paginationformasters;
        //var copty;
        //var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        //if (ivrmcofigsettings.length > 0) {
        //    paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
        //    copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        //}
        //$scope.coptyright = copty;
        //var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        //if (admfigsettings.length > 0) {
        //    var logopath = admfigsettings[0].asC_Logo_Path;
        //}
        //$scope.imgname = logopath;




        //======================Get Report.

        $scope.get_report = function () {
            $scope.showflag = false;
            $scope.selectedYear = [];

            angular.forEach($scope.yearlist, function (yy) {
                if (yy.select) {
                    $scope.selectedYear.push(yy);
                }

            })

            var data = {
                selectedYear: $scope.selectedYear,
            }
            apiService.create("DisabilityStudent/Demand_Ratio_212_Report", data).then(function (promise) {

                //$scope.reportlist = promise.reportlist;
                if (promise.reportlist.length > 0) {
                    $scope.showflag = true;
                    $scope.printflag = true;
                    $scope.reportlist = promise.reportlist;

                    $scope.mainlist = [];
                    angular.forEach($scope.yearlist, function (y1) {
                        $scope.sublist = [];
                        angular.forEach($scope.reportlist, function (y2) {
                            if (y1.select) {
                                if (y1.asmaY_Year == y2.ASMAY_Year) {
                                    $scope.sublist.push(y2);
                                }
                            }
                        })
                        if (y1.select) {
                            $scope.mainlist.push({ ASMAY_Id: y1.asmaY_Id, yearname: y1.asmaY_Year, sublist1: $scope.sublist });
                        }
                    })

                    console.log("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&");
                    console.log($scope.mainlist);
                    console.log($scope.reportlist);
                    console.log($scope.yearlist);

                }
                else {
                    $scope.showflag = false;
                    $scope.printflag = false;
                    swal('Records Not Found!');
                }
            })
        }


        //======================For Excel sheet
        $scope.exportToExcel = function (table) {

            var excelname = 'Cat 2.1.3.xls';
            var exportHref = Excel.tableToExcel(table, '2.1.3');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        }


        //======================for Cancel Button
        $scope.cancel = function () {
            $state.reload();
        }



    }

})();