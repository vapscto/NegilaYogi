(function () {
    'use strict';
    angular.module('app').controller('DiffrentlyAbledStudentController', DiffrentlyAbledStudentController)

    DiffrentlyAbledStudentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout']
    function DiffrentlyAbledStudentController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {
        $scope.printflag = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        //$scope.searchValue23 = "";
        $scope.sortReverse = true;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        //$scope.arraylist = [];
        //$scope.arraylist = [{  Name: "test",gender: "Male",ID: "RTD123",TypeOfDis: "Add",PerOfDia: "10",Prgenrld: "Running",YearOfEnr:"2017-2018" },{ Name: "test1",
        //        gender: "Male",ID: "UPD123",TypeOfDis: "Add",PerOfDia: "10",Prgenrld: "Running",YearOfEnr: "2018-2019"},{  Name: "test2",gender: "Male",               ID: "UDD123",TypeOfDis: "Add",PerOfDia: "10",Prgenrld: "Running",YearOfEnr: "2019-2020" },{Name: "test3",gender: "Male",ID: "STD123",
        //        TypeOfDis: "Add",PerOfDia: "10",Prgenrld: "Running",YearOfEnr: "2016-2017"},{Name: "test4",gender: "Male",ID: "FDS123",TypeOfDis: "Add",
        //        PerOfDia: "10",Prgenrld: "Running",YearOfEnr: "2015-2016"},{Name: "test5",gender: "Male",ID: "BVC123",TypeOfDis: "Add",PerOfDia: "10",
        //        Prgenrld: "Running",YearOfEnr: "2014-2015"}];

        $scope.loaddata = function () {
            $scope.printflag = false;
            $scope.showflag = false;
            var pageid = 2;
            apiService.getURI("DisabilityStudent/getdata", pageid).then(function (promise) {              
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
                apiService.create("DisabilityStudent/get_report", data).then(function (promise) {
                    $scope.reportlist = promise.reportlist;
                    if ($scope.reportlist.length > 0) {
                        angular.forEach($scope.reportlist, function (tt) {
                            $scope.mainArray = [];
                            var count = 0;
                            tt.listdata = $scope.mainArray;
                        })
                        console.log($scope.reportlist);
                        $scope.showflag = true;
                    }
                    else {
                        $scope.showflag = false;
                       // $scope.printflag = false;
                        swal('Record Not Found!');
                    }
                })
            }
             else {
                $scope.submitted = true;
            }
        }        
        //======================For Excel sheet
        $scope.exportToExcel = function (table) {
            var excelname = 'Cat 2.2.3.xls';
            var exportHref = Excel.tableToExcel(table, '2.2.3');
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