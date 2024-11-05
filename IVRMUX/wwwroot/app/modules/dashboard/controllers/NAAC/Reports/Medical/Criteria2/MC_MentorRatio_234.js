(function () {
    'use strict';
    angular.module('app').controller('MC_MentorRatio_234', MC_MentorRatio_234)

    MC_MentorRatio_234.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout']
    function MC_MentorRatio_234($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {


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
            apiService.getURI("Medical_Criteria2Reports/getdata", pageid).then(function (promise) {
                $scope.getinstitutioncycle = promise.getinstitutioncycle;
                $scope.cycleid = promise.getinstitutioncycle[0].cycleid;
                $scope.getparentidzero = promise.getinstitution;

                $scope.naacsL_InstitutionTypeFlg = promise.naacsL_InstitutionTypeFlg;

                angular.forEach($scope.getparentidzero, function (options) {
                    options.select = true;
                })
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

        //======================Get Report.

        $scope.get_report = function () {
            $scope.showflag = false;
           
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
            apiService.create("Medical_Criteria2Reports/MC_234_Report", data).then(function (promise) {

                //$scope.reportlist = promise.reportlist;
                if (promise.reportlist.length > 0) {
                    $scope.reportlist = promise.reportlist;
                    $scope.showflag = true;
                    $scope.printflag = true;
                }
                else {
                    $scope.showflag = false;
                    $scope.printflag = false;
                    swal('Records Not Found!');
                }
                })
            }
            else {
                $scope.submitted = true;
            }
        }


        //======================For Excel sheet
        $scope.exportToExcel = function (table) {

            var excelname = 'Cat 2.3.4.xls';
            var exportHref = Excel.tableToExcel(table, '2.3.4');
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


    }

})();