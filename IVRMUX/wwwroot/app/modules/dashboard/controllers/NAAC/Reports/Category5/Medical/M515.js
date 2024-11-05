(function () {
    'use strict';
    angular
        .module('app')
        .controller('M515Controller', M515Controller)

    M515Controller.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function M515Controller($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {

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
            var exportHref = Excel.tableToExcel(tableId, '5.1.5');
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
                apiService.create("NAACCriteriaFiveReport/get_report515med", data).then(function (promise) {
                    debugger;
                $scope.govtsclist = promise.govtsclist;
                    $scope.govtsclistfiles = promise.govtsclistfiles;
                    if (promise.govtsclist.length > 0 && promise.govtsclist != null) {
                                 $scope.showflag = true;
                    $scope.printflag = true;
                        if (promise.govtsclist[0].ncaC516GRI_AdpOfguidelinesofRegbodiesFlg != undefined && promise.govtsclist[0].ncaC516GRI_AdpOfguidelinesofRegbodiesFlg != false && promise.govtsclist[0].ncaC516GRI_AdpOfguidelinesofRegbodiesFlg != null) {
                            $scope.AdpOfguide = 'YES';

                        } else {
                            $scope.AdpOfguide = 'NO';
                        }


                        if (promise.govtsclist[0].ncaC516GRI_StusgrvOnline_OR_offlineFlg != undefined && promise.govtsclist[0].ncaC516GRI_StusgrvOnline_OR_offlineFlg != false && promise.govtsclist[0].ncaC516GRI_StusgrvOnline_OR_offlineFlg != null) {
                            $scope.grvOnline = 'YES';

                        } else {
                            $scope.grvOnline = 'NO';
                        }

                        if (promise.govtsclist[0].ncaC516GRI_CommitteewithminutesFlg != undefined && promise.govtsclist[0].ncaC516GRI_CommitteewithminutesFlg != false && promise.govtsclist[0].ncaC516GRI_CommitteewithminutesFlg != null) {
                            $scope.meeting = 'YES';

                        } else {
                            $scope.meeting = 'NO';
                        }


                        if (promise.govtsclist[0].ncaC516GRI_RecordOfActionTakenFlg != undefined && promise.govtsclist[0].ncaC516GRI_RecordOfActionTakenFlg != false && promise.govtsclist[0].ncaC516GRI_RecordOfActionTakenFlg != null) {
                            $scope.record = 'YES';

                        } else {
                            $scope.record = 'NO';
                        }
                    }
                    else {
                        $scope.showflag = false;
                        $scope.printflag = false;
                        swal('No Record Found')
                    }
               
                //    if (promise.govtsclist.length > 0 && promise.govtsclist!=null) {
                //    $scope.showflag = true;
                //    $scope.printflag = true;
                 
                //    $scope.finalarray = [];
                //    angular.forEach($scope.govtsclist, function (gg) {
                //        var govtfiles = [];

                //        angular.forEach($scope.govtsclistfiles, function (ff) {
                //            if (gg.ncaC513INSCH_Id == ff.ncaC514CPEX_Id) {
                //                govtfiles.push({ filename: ff.ncaC514CPEXF_FileName, filepath: ff.ncaC514CPEXF_FilePath, filedesc: ff.ncaC514CPEXF_Filedesc})
                //            }
                //        })
                //        $scope.finalarray.push({ YEAR: gg.asmaY_Year, SCHEME: gg.ncaC513INSCH_DevSchemeName, GCNT: gg.noofstd, FILES: govtfiles})
                //    })
                   
                //        $scope.yearlist = promise.yearlist;
                //        $scope.finalarray1 = [];
                //        //angular.forEach($scope.yearlist, function (yy) {
                //        //    var temparr = [];
                //        //    angular.forEach($scope.finalarray, function (dd) {
                //        //        if (yy.asmaY_Year == dd.YEAR) {
                //        //            temparr.push(dd);
                //        //        }


                //        //    })

                //        //    if (temparr.length > 0) {
                //        //        $scope.finalarray1.push({ YEAR: yy.asmaY_Year, dlist: temparr })
                //        //    }

                //        //})
                //        var temparr = [];
                //        angular.forEach($scope.yearlist, function (yy) {
                           
                //            angular.forEach($scope.finalarray, function (dd) {
                //                if (yy.asmaY_Year == dd.YEAR) {
                //                    temparr.push(dd);
                //                }


                //            })
                            
                //        })

                //        $scope.stdcntlist = promise.stdcntlist;
                      

                //        $scope.finalarray1 = temparr;

                //        angular.forEach($scope.finalarray1, function (ff) {

                //            angular.forEach($scope.stdcntlist, function (st) {

                //                if (ff.YEAR == st.asmaY_Year) {
                //                    ff.STDCNT = st.stud_count;
                //                }

                //            })

                //        })



                //    console.log($scope.finalarray);
                //}
                //else {
                //    $scope.showflag = false;
                //        $scope.printflag = false;
                //        swal('No Record Found')
                //}
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

