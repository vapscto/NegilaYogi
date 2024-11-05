(function () {
    'use strict';
    angular
        .module('app')
        .controller('HSU521Controller', HSU521Controller)

    HSU521Controller.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function HSU521Controller($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {

        $scope.printflag = false;

        $scope.currentPage = 1;
        $scope.itemsPerPage = 15;
        //$scope.searchValue23 = "";

        $scope.sortReverse = true;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }


        $scope.noofyear = 5;
        

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
            }, 1000);
        }



        $scope.printData = function () {
            var innerContents = document.getElementById("printSectionId1").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Library/ExchangableCoursePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 1000);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.exportToExcel = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, '5.2.1');
            $timeout(function () { location.href = exportHref; }, 1000);
        }

        //$scope.exportToExcel = function (table) {

        //    var excelname = 'Cat 1.1.3.xls';
        //    var exportHref = Excel.tableToExcel(table, '1.1.3');
        //    $timeout(function () {
        //        var a = document.createElement('a');
        //        a.href = exportHref;
        //        a.download = excelname;
        //        document.body.appendChild(a);
        //        a.click();
        //        a.remove();
        //    }, 100);
        //}
        $scope.getinstitutioncycle = [];
        $scope.loaddata = function () {
            $scope.printflag = false;
            var pageid = 2;
            apiService.getURI("NAACCriteriaFiveReport/getdata", pageid).then(function (promise) {
               
                $scope.getinstitutioncycle = promise.getinstitutioncycle;
                $scope.getparentidzero = promise.getinstitution;

                angular.forEach($scope.getparentidzero, function (options) {
                    options.select = true;
                })
                if ($scope.getinstitutioncycle.length > 0) {
                    $scope.cycleid = $scope.getinstitutioncycle[0].cycleid;
                }
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
        $scope.isOptionsRequired = function () {
            return !$scope.getparentidzero.some(function (options) {
                return options.select;
            });
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

        $scope.interacted = function () {
            return $scope.submitted;
        }
        //======================report.
        $scope.showflag = false;
        $scope.showdetails = function () {
            $scope.govtsclist = [];
            $scope.showflag = false;
            $scope.printflag = false;
            $scope.govtsclistfiles = [];
            if ($scope.myForm.$valid) {

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
                apiService.create("NAACCriteriaFiveReport/get_report523", data).then(function (promise) {

                $scope.govtsclist = promise.govtsclist;
                $scope.govtsclistfiles = promise.govtsclistfiles;
                    $scope.examlist = promise.examlist;
               
                    if (promise.govtsclist.length > 0 && promise.govtsclist!=null) {
                    $scope.showflag = true;
                    $scope.printflag = true;
                        $scope.yearlist = promise.yearlist;
                    $scope.finalarray = [];

                        angular.forEach($scope.yearlist, function (yy) {
                            $scope.mainarray = [];
                            var totalcnt = 0;
                            var totalcnt1 = 0;
                            angular.forEach($scope.govtsclist, function (ee) {
                                if (yy.asmaY_Year == ee.asmaY_Year) {

                                    $scope.mainarray.push(ee);
                                    totalcnt = totalcnt + ee.noofstd;
                                    totalcnt1 = totalcnt1 + ee.ncaC523QE_NoOfStudentsappearing;

                                }

                            })
                            if ($scope.mainarray.length>0) {
                                $scope.finalarray.push({ YEAR: yy.asmaY_Year, ncaC523QE_NoOfStudentsappearing: totalcnt1, TT: totalcnt, list: $scope.mainarray })
                            }  
                           

                        })

                       
                     



                        $scope.examtt = [];
                        $scope.gtt = 0;
                        angular.forEach($scope.examlist, function (gg) {
                            var cnnnt = 0;
                            angular.forEach($scope.govtsclist, function (dd) {

                                if (gg.ncaC523QAMA_Id == dd.ncaC523QAMA_Id) {
                                    cnnnt = cnnnt + dd.noofstd;
                                    $scope.gtt = $scope.gtt + dd.noofstd;
                                }



                            })


                            $scope.examtt.push({ ncaC523QAMA_Id: gg.ncaC523QAMA_Id, cnt: cnnnt });

                        })

                    
                }
                else {
                    $scope.showflag = false;
                        $scope.printflag = false;
                        swal('No Record Found')
                }
            })

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

