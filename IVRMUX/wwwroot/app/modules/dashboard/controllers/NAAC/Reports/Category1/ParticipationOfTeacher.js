(function () {
    'use strict';
    angular
        .module('app')
        .controller('ParticipationOfTeacherController', ParticipationOfTeacherController)
    ParticipationOfTeacherController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function ParticipationOfTeacherController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {
        $scope.printflag = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.sortReverse = true;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        //$scope.ListofItems = [];
        //$scope.ListofItems = [
        //    { year: "2018-2019", noofteacher: "5", participateddept: "Department-E" },
        //    { year: "2017-2018", noofteacher: "3", participateddept: "Department-D" },
        //    { year: "2016-2017", noofteacher: "5", participateddept: "Department-C" },
        //    { year: "2015-2016", noofteacher: "4", participateddept: "Department-B" },
        //    { year: "2014-2015", noofteacher: "4", participateddept: "Department-A" },
        //]
        $scope.exportToExcel = function (table) {
            var excelname = 'Cat 1.1.xls';
            var exportHref = Excel.tableToExcel(table, '1.1');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
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
        $scope.exportToExcel = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        }
        $scope.loaddata = function () {
            $scope.printflag = false;
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
        $scope.togchkbx = function () {
            $scope.noofyear = 0;
            angular.forEach($scope.yearlist, function (ff) {
                if (ff.select == true) {
                    $scope.noofyear += 1;
                }
            })
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
                apiService.create("CurricularAspects/get_report_113", data).then(function (promise) {
                    $scope.reportlist = promise.reportlist;
                    $scope.reportlist2 = promise.reportlist2;
                    if (promise.reportlist.length > 0) {
                        angular.forEach($scope.reportlist, function (tt) {
                            $scope.mainArray = [];
                            angular.forEach($scope.reportlist2, function (ss) {
                                if (tt.NCACTP113_Id == ss.ncactP113_Id) {
                                    $scope.mainArray.push(ss);
                                }
                            })
                            tt.listdata = $scope.mainArray;
                        })
                        console.log($scope.reportlist);
                        $scope.showflag = true;
                    }
                    else {
                        $scope.showflag = false;
                        swal('Record Not Found!');
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        }
        $scope.submitted = false;
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.cancel = function () {
            $state.reload();
        }
    }
})();

