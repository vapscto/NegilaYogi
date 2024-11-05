(function () {
    'use strict';
    angular
        .module('app')
        .controller('NAAC363ReportController', NAAC363ReportController)

    NAAC363ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function NAAC363ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {

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
            }, 100);
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
            var exportHref = Excel.tableToExcel(tableId, '3.6.3');
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

        //$scope.loaddata = function () {
        //    $scope.printflag = false;
        //    var pageid = 2;
        //    apiService.getURI("NAACCriteria3Report/getdata", pageid).then(function (promise) {
               
        //        $scope.yearlist = promise.yearlist;

        //        var s = 0;
        //        angular.forEach($scope.yearlist, function (pp) {
        //            if (s < $scope.noofyear) {
        //                pp.select = true;
        //            }
        //            s += 1;
        //        })
        //    })
        //}
        $scope.loaddata = function () {
        
            var pageid = 2;
            apiService.getURI("NAACCriteria3Report/getdata", pageid).then(function (promise) {

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
        $scope.isOptionsRequired = function () {
            return !$scope.yearlist.some(function (options) {
                return options.select;
            });
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

            //$scope.selectedYear = [];

            //angular.forEach($scope.yearlist, function (yy) {
            //    if (yy.select) {
            //        $scope.selectedYear.push(yy);
            //    }

            //})
                
            //var data = {
            //    selectedYear: $scope.selectedYear,
            //}
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
                apiService.create("NAACCriteria3Report/get_report", data).then(function (promise) {
                  
                    $scope.govtsclist = promise.reportlist;
                    $scope.govtsclistfiles = promise.reportlist2;
               
                    if (promise.reportlist.length > 0 && promise.reportlist!=null) {
                    $scope.showflag = true;
                    $scope.printflag = true;
                 
                    $scope.finalarray = [];
                    angular.forEach($scope.govtsclist, function (gg) {
                        var govtfiles = [];

                        angular.forEach($scope.govtsclistfiles, function (ff) {
                            if (gg.NCACSA343_Id == ff.ncacsA343_Id) {
                                govtfiles.push({ filename: ff.ncacsA343F_FileName, filepath: ff.ncacsA343F_FilePath, filedesc: ff.ncacsA343F_Filedesc})
                            }
                        })
                        $scope.finalarray.push({ YEAR: gg.ASMAY_Year, SCHEME: gg.NCACSA343_TypeOfActivity, GCNT: gg.NCACSA343_NoOfStudents, AG: gg.NCACSA343_OrgAgency, FILES: govtfiles})
                    })
                   


                    console.log($scope.finalarray);
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

