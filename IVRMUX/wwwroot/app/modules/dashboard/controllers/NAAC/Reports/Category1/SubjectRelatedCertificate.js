(function () {
    'use strict';
    angular
        .module('app')
        .controller('SubjectRelatedCertificateController', SubjectRelatedCertificateController)
    SubjectRelatedCertificateController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function SubjectRelatedCertificateController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
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
            var excelname = 'Cat 1.2.3.xls';
            var exportHref = Excel.tableToExcel(table, '1.2.3');
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
            var pageid = 2;
            apiService.getURI("CurricularAspects/getdata", pageid).then(function (promise) {
                $scope.getinstitutioncycle = promise.getinstitutioncycle;
                $scope.cycleid = promise.getinstitutioncycle[0].cycleid;
                $scope.getparentidzero = promise.getinstitution;
                angular.forEach($scope.getparentidzero, function (pp) {
                    pp.select = true;
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
        //$scope.togchkbx = function () {
        //    $scope.noofyear = 0;
        //    angular.forEach($scope.yearlist, function (ff) {
        //        if (ff.select == true) {
        //            $scope.noofyear += 1;
        //        }
        //    })
        //}
        $scope.togchkbx = function () {
            $scope.getparentidzero.every(function (options) {
                return options.select;
            });
        }
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
        //======================report.
        $scope.showflag = false;
        $scope.showdetails = function () {
            $scope.selected_Inst = [];
            angular.forEach($scope.getparentidzero, function (yy) {
                if (yy.select) {
                    $scope.selected_Inst.push(yy);
                }
            })
            if ($scope.myForm.$valid) {
                var data = {
                    "cycleid": $scope.cycleid,
                    selected_Inst: $scope.selected_Inst,
                }
                apiService.create("CurricularAspects/get_report_123", data).then(function (promise) {
                    $scope.reportlist = promise.reportlist;
                    $scope.reportlist2 = promise.reportlist2;
                    if (promise.reportlist.length > 0) {
                        angular.forEach($scope.reportlist, function (tt) {
                            $scope.mainArray = [];
                            var count = 0;
                            angular.forEach($scope.reportlist2, function (ss) {
                                if (tt.NCACSP123_Id == ss.ncacsP123_Id) {
                                    $scope.mainArray.push(ss);
                                }
                            })
                            tt.listdata = $scope.mainArray;
                        })
                        console.log($scope.reportlist);

                        $scope.showflag = true;
                    }
                    else {
                        swal('No Records Found!')
                        $scope.showflag = false;
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        }

        //$scope.isOptionsRequired = function () {
        //    return !$scope.yearlist.some(function (options) {
        //        return options.select;
        //    });
        //}
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.submitted = false;

        //====cancel
        $scope.cancel = function () {
            $state.reload();
        }
    }
})();

