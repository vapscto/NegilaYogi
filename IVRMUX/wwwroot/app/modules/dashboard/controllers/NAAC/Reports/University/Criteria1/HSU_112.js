(function () {
    'use strict';
    angular.module('app').controller('HSU_112Controller', HSU_112Controller)

    HSU_112Controller.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout']
    function HSU_112Controller($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {


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
            apiService.getURI("HSU_CR1_Report/getdata", pageid).then(function (promise) {

                $scope.getinstitutioncycle = promise.getinstitutioncycle;
                $scope.cycleid = promise.getinstitutioncycle[0].cycleid;
                $scope.getparentidzero = promise.getinstitution;

                $scope.naacsL_InstitutionTypeFlg = promise.naacsL_InstitutionTypeFlg;

                angular.forEach($scope.getparentidzero, function (options) {
                    options.select = true;
                })
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

            var data = {
                "cycleid": $scope.cycleid,
                selected_Inst: $scope.selected_Inst,
            }
            apiService.create("HSU_CR1_Report/HSU_112_Report", data).then(function (promise) {

                //$scope.reportlist = promise.reportlist;
                if (promise.reportlist.length > 0) {
                    $scope.showflag = true;
                    $scope.printflag = true;
                    $scope.reportlist = promise.reportlist;
                    $scope.yearlist1 = promise.yearlist1;
                    $scope.reportlist2 = promise.reportlist2;
                    $scope.mainlist = [];
                    angular.forEach($scope.yearlist1, function (y1) {
                        $scope.sublist = [];
                        $scope.sublist2 = [];
                        angular.forEach($scope.reportlist, function (y2) {
                            if (y1.asmaY_Year == y2.ASMAY_Year) {
                                $scope.sublist.push(y2);
                                angular.forEach($scope.reportlist2, function (ss) {
                                    if (y2.filefkid == ss.ncacpR112_Id) {
                                        $scope.sublist2.push(ss);
                                    }
                                })
                            }
                        })
                        if ($scope.sublist.length > 0) {
                            $scope.mainlist.push({ ASMAY_Id: y1.asmaY_Id, yearname: y1.asmaY_Year, sublist1: $scope.sublist, sublist2: $scope.sublist2 });
                        }

                    })

                    console.log("===========================");
                    console.log($scope.mainlist);
                    console.log($scope.reportlist);
                    console.log($scope.reportlist2);
                    console.log($scope.yearlist1);

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

            var excelname = 'Cat 1.1.2.xls';
            var exportHref = Excel.tableToExcel(table, '1.1.2');
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