(function () {
    'use strict';
    angular
        .module('app')
        .controller('InterDepartmentalCourses121', InterDepartmentalCourses121)

    InterDepartmentalCourses121.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function InterDepartmentalCourses121($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        //$scope.searchValue23 = "";

        $scope.sortReverse = true;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        

        $scope.exportToExcel = function (table) {

            var excelname = 'Cat 1.2.1.xls';
            var exportHref = Excel.tableToExcel(table, '1.2.1');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        }

        $scope.isOptionsRequired = function () {
            return !$scope.getparentidzero.some(function (options) {
                return options.select;
            });
        }


        $scope.loaddata = function () {

            var pageid = 2;
            apiService.getURI("Medical_Criteria1Reports/getdata", pageid).then(function (promise) {

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


        $scope.showflag = false;
        $scope.get_nCourse_report = function () {
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

            apiService.create("Medical_Criteria1Reports/M_IDC121_Report", data).then(function (promise) {

                $scope.reportlist = promise.reportlist;
                $scope.reportlist2 = promise.reportlist2;

                if (promise.reportlist.length > 0) {
                    angular.forEach($scope.reportlist, function (tt) {
                        $scope.mainArray = [];                        
                        angular.forEach($scope.reportlist2, function (ss) {
                            if (tt.filefkid == ss.nmC121IDC_Id) {
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
            });
        }

        $scope.cancel = function () {
            $state.reload();
        }



    }
})();

