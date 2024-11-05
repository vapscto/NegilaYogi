(function () {
    'use strict';
    angular
        .module('app')
        .controller('DeclarationOfResultsController', DeclarationOfResultsController)

    DeclarationOfResultsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function DeclarationOfResultsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {

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


        //================= Load Page
        $scope.loaddata = function () {
           
            var pageid = 2;
            apiService.getURI("DisabilityStudent/getdata", pageid).then(function (promise) {
                $scope.getinstitutioncycle = promise.getinstitutioncycle;
                $scope.cycleid = promise.getinstitutioncycle[0].cycleid;
                $scope.getparentidzero = promise.getinstitution;

                $scope.naacsL_InstitutionTypeFlg = promise.naacsL_InstitutionTypeFlg;

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

        //======================Get Report.
        $scope.get_report = function () {
          
            $scope.showflag = false;
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
                apiService.create("DisabilityStudent/DeclrofResult_Report251", data).then(function (promise) {
                    $scope.reportlist = promise.reportlist;
                    if ($scope.reportlist.length > 0) {
                        angular.forEach($scope.reportlist, function (tt) {
                            $scope.mainArray = [];
                            tt.listdata = $scope.mainArray;
                        })                   
                console.log($scope.reportlist);
                $scope.showflag = true;                         
                    }
                    else {
                        $scope.showflag = false;
                       // $scope.printflag = false;
                        swal('Records Not Found!');
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
        $scope.isOptionsRequired = function () {
            return !$scope.yearlist.some(function (options) {
                return options.select;
            });
        }
        //======================For Excel sheet
        $scope.exportToExcel = function (table) {
            var excelname = 'Cat 2.5.1xls';
            var exportHref = Excel.tableToExcel(table, '2.5.1');
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

