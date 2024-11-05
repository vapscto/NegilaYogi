(function () {
    'use strict';
    angular
        .module('app')
        .controller('NAAC_Criteria_6_ReportController', NAAC_Criteria_6_ReportController)

    NAAC_Criteria_6_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function NAAC_Criteria_6_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        //$scope.searchValue23 = "";

        $scope.sortReverse = true;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }


        $scope.ListofItems = [];
        $scope.ListofItems = [
            { name: "BC0031", program: "BE0023", certificateName: "Automation Certificate", year: "2018-2019", course: "BB0090" },
            { name: "BC0032", program: "BE002", certificateName: " Certificate-D", year: "2017-2018", course: "BB0088"},
            { name: "BC0033", program: "BE0021", certificateName: " Certificate-C", year: "2016-2017", course: "BB0092"},
            { name: "BC0034", program: "BE0025", certificateName: " Certificate-B", year: "2015-2016", course: "BB0077"},
            { name: "BC0035", program: "BE0032", certificateName: " Certificate-A", year: "2014-2015", course: "BB0082" },
        ]

        $scope.exportToExcel = function (table) {

            var excelname = 'Criteria 6.5.3.xls';
            var exportHref = Excel.tableToExcel(printSection_653, '1.2.1');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        }


        $scope.loaddata = function () {
            $scope.show_653_t = true;
            $scope.showflag = false;
            var pageid = 2;
            apiService.getURI("NAAC_Criteria_6_Report/loaddata1", pageid).then(function (promise) {

                $scope.getinstitutioncycle = promise.getinstitutioncycle;
                $scope.cycleid = promise.getinstitutioncycle[0].cycleid;
                $scope.getparentidzero = promise.getinstitution;

                angular.forEach($scope.getparentidzero, function (options) {
                    options.select = true;
                })

                $scope.yearlist = promise.allacademicyear;

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
            //$scope.usercheck = $scope.yearlist.every(function (options) {
            //    return options.select;
            //});
            $scope.noofyear = 0;
            angular.forEach($scope.yearlist, function (ff) {
                if (ff.select == true) {
                    $scope.noofyear += 1;
                }

            })
        }

        $scope.printData = function () {
            var innerContents = document.getElementById("printSection_653").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Library/ExchangableCoursePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.submitted = false;
        $scope.show_653 = false;
        $scope.get_nCourse_report = function () {

            $scope.selected_Inst = [];
            angular.forEach($scope.getparentidzero, function (mm) {
                if (mm.select) {
                    $scope.selected_Inst.push(mm);
                }
            })
            if ($scope.myForm.$valid) {
                var data = {
                    //yerlistdata: $scope.yerlistdata,
                    yerlistdata: $scope.selected_Inst,
                    "cycleid": $scope.cycleid,
                    "org": '653',
                }

                apiService.create("NAAC_Criteria_6_Report/get_report", data).then(function (promise) {

                    $scope.reportlist = promise.alldata1;
                    $scope.filelist = promise.savedresult;
                    if ($scope.reportlist.length > 0) {
                        $scope.show_653 = true;
                        $scope.showflag = true;
                    }
                    else {
                        $scope.show_653 = false;
                        $scope.showflag = false;
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
           
        }

        $scope.cancel = function () {
            $state.reload();
        }


    }
})();

