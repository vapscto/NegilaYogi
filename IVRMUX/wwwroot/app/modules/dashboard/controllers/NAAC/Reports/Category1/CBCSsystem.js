(function () {
    'use strict';
    angular
        .module('app')
        .controller('CBCSsystemController', CBCSsystemController)

    CBCSsystemController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function CBCSsystemController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {

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
            { name: "BC0032", program: "BE002", certificateName: " Certificate-D", year: "2017-2018", course: "BB0088" },
            { name: "BC0033", program: "BE0021", certificateName: " Certificate-C", year: "2016-2017", course: "BB0092" },
            { name: "BC0034", program: "BE0025", certificateName: " Certificate-B", year: "2015-2016", course: "BB0077" },
            { name: "BC0035", program: "BE0032", certificateName: " Certificate-A", year: "2014-2015", course: "BB0082" },
        ]

        $scope.exportToExcel = function (table) {

            var excelname = 'Cat 1.2.2.xls';
            var exportHref = Excel.tableToExcel(table, '1.2.2');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        }


        $scope.cancel = function () {
            $state.reload();
        }
        $scope.printData = function () {

            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ExchangableCoursePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        
        //=======================load data
        $scope.loaddata = function () {

            var pageid = 2;
            apiService.getURI("CurricularAspects/getdata", pageid).then(function (promise) {
                $scope.getinstitutioncycle = promise.getinstitutioncycle;
                $scope.cycleid = promise.getinstitutioncycle[0].cycleid;
                $scope.getparentidzero = promise.getinstitution;

                angular.forEach($scope.getparentidzero, function (options) {
                    options.select = true;
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
            $scope.getparentidzero.every(function (options) {
                return options.select;
            });
        }

        $scope.isOptionsRequired = function () {
            return !$scope.getparentidzero.some(function (options) {
                return options.select;
            });
        }


        $scope.submitted = false;
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.reportdata = false;
        //======================report.
        $scope.get_report = function () {
            $scope.showflag = false;
            //$scope.selectedYear = [];
            //angular.forEach($scope.yearlist, function (yy) {
            //    if (yy.select) {
            //        $scope.selectedYear.push(yy);
            //    }
            //})

            $scope.selected_Inst = [];
            angular.forEach($scope.getparentidzero, function (mm) {
                if (mm.select) {
                    $scope.selected_Inst.push(mm);
                }
            })

            if ($scope.myForm.$valid) {
                var data = {
                    //selectedYear: $scope.selectedYear,
                    "cycleid": $scope.cycleid,
                    selected_Inst: $scope.selected_Inst,
                }
                apiService.create("CurricularAspects/get_122CBCSsystemReport", data).then(function (promise) {
                    
                    if (promise.reportlist.length > 0) {
                        $scope.reportdata = true;
                        $scope.reportlist = promise.reportlist;
                        $scope.yearlist = promise.yearlist;

                        //angular.forEach($scope.reportlist, function (rp) {
                        //    $scope.mainlist = [];
                        //    angular.forEach($scope.yearlist, function (yr) {
                        //        if (rp.ASMAY_Id == yr.asmaY_Id) {
                        //            $scope.mainlist.push(rp);
                        //        }
                        //    })
                        //})
                        //console.log($scope.mainlist);
                    } 
                    else {
                        $scope.reportdata = false;
                        swal('Record Not Found!');
                    }                   
                })
            }
            else {
                $scope.submitted = true;
            }

        }

        $scope.isOptionsRequired = function () {
            return !$scope.getparentidzero.some(function (options) {
                return options.select;
            });
        }
        $scope.togchkbx = function () {
            $scope.getparentidzero.every(function (options) {
                return options.select;
            });
        }


    }
})();

