

(function () {
    'use strict';
    angular
        .module('app')
        .controller('ADMCasteStrengthController', ADMCasteStrengthController)

    ADMCasteStrengthController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window', '$timeout','Excel']
    function ADMCasteStrengthController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window, $timeout, Excel) {

        $scope.castedetails = [];

        $scope.searchValue = "";
        $scope.imagename = "";


        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.masterlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.searchValue = "";
        if ($scope.itemsPerPage == undefined) {
            $scope.itemsPerPage = 15
        }
        $scope.fields = function () {

            $scope.newadmissionstdtotal = [];
            $scope.datagraph = [];
            $scope.regularstdtotal = [];
            $scope.newadmstdgraphdta = [];


            $scope.Todaydate = new Date();
        }
        $scope.studentdrp = false;
        $scope.Binddata = function () {
            $scope.fields();


            apiService.getDATA("ADMCasteStrength/Getdetails").
                then(function (promise) {


                    $scope.yearlt = promise.yearlist;
                    $scope.castelist = promise.castelist;



                })

        }



        $scope.OnAcdyear = function (asmaY_Id) {
            $scope.asmcL_Id = '';
            var a = $scope.asmaY_Id;
            // alert(asmaY_Id)
            $scope.fields();

            apiService.getURI("ADMCasteStrength/getclass", asmaY_Id).
                then(function (promise) {
                    $scope.classarray = promise.classarray;
                })


        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        $scope.loadchart = function () {

            var cast_list = [];
            if ($scope.castedetails != null) {

                for (var i = 0; i < $scope.castedetails.length; i++) {
                    cast_list.push({ label: $scope.castedetails[i].caste, "y": $scope.castedetails[i].total })
                }
            }

            var chart = new CanvasJS.Chart("columnchart", {
                height: 350,
                width: 1075,
                axisX: {
                    labelFontSize: 12,
                    interval: 1,
                    labelAngle: -20,
                },
                axisY: {
                    labelFontSize: 12,
                },

                data: [
                    {
                        type: "column",
                        showInLegend: true,
                        dataPoints: cast_list
                    }
                ]
            });

            chart.render();
        }
        $scope.clear = function () {
            $state.reload();
        }

        $scope.showstudentGrid = function (castid) {

            var data = {
                "asmcL_Id": $scope.asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id,
                "asmS_Id": $scope.asmS_Id,
                "castid": castid,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("ADMCasteStrength/Getstudentdetails", data).
                then(function (promise) {


                    $scope.studentlist = promise.studentlist;



                })

        }


        $scope.OnClass = function (asmcL_Id) {
            //alert($scope.type)
            $scope.asmS_Id = '';
            $scope.asmcL_Id = asmcL_Id;
            // alert(asmaY_Id)
            $scope.fields();
            var data = {
                "asmcL_Id": asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("ADMCasteStrength/Getsection", data).
                then(function (promise) {


                    $scope.section = promise.fillsection;



                })


        }


        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        $scope.printToCart = function () {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ProgressReport/BGIProgressReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.exportToExcel = function (tableId) {

            var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);

        }
        $scope.showreport = function () {

            $scope.indattendance = false;
            $scope.submitted = true;
            //foreach array added by akash
            $scope.classlistarray = [];
            angular.forEach($scope.classarray, function (qq) {
                if (qq.selected == true) {
                    $scope.classlistarray.push({ ASMCL_Id: qq.asmcL_Id, ASMCL_Id: qq.asmcL_Id })
                }
            });
            //foreach array added by akash
            $scope.albumNameArraycolumn = [];
            angular.forEach($scope.headertest, function (role) {
                if (!!role.selected) $scope.albumNameArraycolumn.push({
                    columnID: role.imC_Id,
                    columnName: role.imC_CasteName
                });
            });
            if ($scope.myForm.$valid) {
                var data = {
                    "classlsttwo": $scope.classlistarray,
                    "ASMAY_Id": $scope.asmaY_Id,
                    //"TempararyArrayheadList": $scope.albumNameArraycolumn
                    
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("ADMCasteStrength/Getreport", data).
                    then(function (promise) {



                        if (promise.castedetails != null || promise.viewlist != null ) {
                            $scope.indattendance = true;
                            $scope.castedetails = promise.castedetails;
                            $scope.castelistnew = promise.castelist_new;
                            // added by akash(viewlist //castelist_new)
                            $scope.viewlist = promise.viewlist;

                            $scope.imagename = promise.imagename;

                            //$scope.castelistnew = [];
                            //angular.forEach(promise.castelist_new, function (role) {
                            //    $scope.castelistnew.push({
                            //        imC_Id: role.imC_Id,
                            //        imC_CasteName: role.imC_CasteName
                            //    });
                            //});

                            $scope.loadchart();
                        }
                        else {
                            swal("No Record Found")
                        }



                    })


            }
            else {
                $scope.submitted = true;
            }

        }



        // added By Akash
        $scope.albumNameArraycolumn = [];
        $scope.searchchkbx = "";
        $scope.all_check = function () {
            var checkStatus = $scope.checkallm;
            angular.forEach($scope.classarray, function (itm) {
                itm.selected = checkStatus;
            });
        }

        $scope.togchkbx = function () {
            $scope.classarray.every(function (options) {
                return options.selected;
            });
        }

        $scope.isOptionsRequired = function () {
            return !$scope.classarray.some(function (options) {
                return options.selected;
            });
        }

        $scope.Toggle_header = function () {

            var toggleStatus = $scope.all2;
            angular.forEach($scope.castelist, function (itm) {
                itm.selected = toggleStatus;
            });
        };

        $scope.addColumn = function (role) {
            $scope.all2 = $scope.castelist.every(function (itm) { return itm.selected; });
            if (role.selected === true) {
                $scope.albumNameArraycolumn.push(role);
            }
            else {
                var som = $scope.albumNameArraycolumn.indexOf(role);
                $scope.albumNameArraycolumn.splice($scope.albumNameArraycolumn.indexOf(role), 1);
            }
        };
        $scope.isOptionsRequired1 = function () {
            return !$scope.castelist.some(function (options) {
                return options.selected;
            });
        };


    };
})();