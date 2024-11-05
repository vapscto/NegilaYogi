
(function () {
    'use strict';
    angular
.module('app')
.controller('FeeActivityApprovalController', fee12)

    fee12.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', 'superCache']
    function fee12($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, superCache) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.cfg = {};
        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        $scope.today = new Date();
        $scope.FYP_Date_From = new Date();
        $scope.FYP_Date_To = new Date();

        $scope.loadapproval = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;

            var pageid = 1;

            var data = {
                "pageid": pageid,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("FeeActivityRequest/loadapproval", data).
                then(function (promise) {
                    $scope.yearlst = promise.yearlist;
                    $scope.cfg.ASMAY_Id = academicyrlst[0].asmaY_Id;
                    $scope.classcount = promise.classlist;
                    $scope.sectiondrpre = promise.sectionlist;
                    $scope.receiptgrid = promise.getstusaveddata;
                    $scope.savedarrlistchk = promise.getstusaveddataheadview;

                    $scope.totcountsearch = $scope.receiptgrid.length;

                });
        };

        $scope.onselectacademic = function (yearlst) {
            angular.forEach(yearlst, function (op_m) {
                if (op_m.asmaY_Id == $scope.cfg.ASMAY_Id) {
                    $scope.asmaY_Year = op_m.asmaY_Year
                }
            })

            var data = {
                "ASMAY_ID": $scope.cfg.ASMAY_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("FeeActivityRequest/viewacaclslst", data).
                then(function (promise) {
                    $scope.classcount = promise.classlist;
                    $scope.sectiondrpre = promise.sectionlist;
                })
        }

        $scope.headdata = [];
        $scope.viewstudentlist = function () {
            if ($scope.myForm.$valid) {

                var data = {
                    "ASMAY_ID": $scope.cfg.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.sectiondrp,
                    "FYP_Date_From": new Date($scope.FYP_Date_From).toDateString(),
                    "FYP_Date_To": new Date($scope.FYP_Date_To).toDateString()
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };

                apiService.create("FeeActivityRequest/viewstudentlist", data).
                    then(function (promise) {
                        if (promise.fetheaddata.length > 0) {
                            $scope.feeheadgrid = promise.fetheaddata;
                            $scope.arrlistchk = promise.fetheadstudata;
                        }
                        else {
                            swal("No Records Found");
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.resultData = [];
        $scope.saveGroupdata = function (arrlistchk) {
            debugger;
          
            if ($scope.myForm.$valid) {

                var checkboxval = false;
                angular.forEach(arrlistchk, function (itm) {
                    if (itm.selected === true) {
                        $scope.resultData.push(itm);
                    }
                })

                if ($scope.resultData.length >0) {
                    var data = {
                        "headnames": $scope.resultData,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("FeeActivityRequest/saveGroupdata", data).
                        then(function (promise) {
                            debugger;
                            if (promise.returnval === true) {
                                swal("Approved  Sucessfully!");
                                $state.reload();
                            }
                            else {
                                swal("Kindly contact Administrator");
                            }
                        })
                }
                else {
                    swal("Kindly select atleast one head");
                }
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.checkselected = true;
        $scope.toggleAll = function (allchkdata) {
            if (allchkdata === true) {
               // $scope.showsavebtn = true;
            }
            else if (allchkdata === false) {
                //$scope.showsavebtn = false;
            }

            var toggleStatus = allchkdata;
            angular.forEach($scope.feeheadgrid, function (itm) {
                itm.selected = toggleStatus;
            });
        }

        $scope.toggleAllsingle = function (feeheadgrid, user1) {
         
            //var enablesavebtncnt = 0;
            //angular.forEach($scope.feeheadgrid, function (itm) {
            //    if (itm.selected === true) {
            //        enablesavebtncnt = enablesavebtncnt + 1;
            //    }
            //});

            //if (enablesavebtncnt >= 1) {
            //     $scope.showsavebtn = true;
            //}
            //else if (enablesavebtncnt === 0) {
            //     $scope.showsavebtn = false;
            //}
            
            if (user1.selected === true) {
                $scope.showsavebtn = true;
                angular.forEach(feeheadgrid, function (itm) {
                    itm.selected = true;
                });
            }
            else {
                angular.forEach(feeheadgrid, function (itm) {
                    itm.selected = false;
                });       
                $scope.showsavebtn = false;
            }

            $scope.showbutton();
        };

        $scope.submitted = false;
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.resultData1 = [];
        $scope.showbutton = function (arrlistchk1, user1) {
            $scope.resultData1 = [];
            angular.forEach($scope.arrlistchk, function (itm) {
                if (itm.selected === true) {
                    $scope.resultData1.push(itm);
                }
            });
            //var cnt = 0;
            //angular.forEach(arrlistchk, function (itm) {
            //    if (itm.selected === true) {
            //        cnt += 1;
            //    }
            //});
            if ($scope.resultData1.length>0) {
                user1.selected = true;
            }
            else {
                user1.selected = false;
            }
        };


        $scope.search_flag = false;
        $scope.search123 = "";
        var search_s = "";
        var list_s = [];
        $scope.onselectsearch = function () {
            search_s = $scope.search123;
            list_s = $scope.receiptgrid;
            if (search_s == "" || search_s == undefined) {
                swal("Select Any Field For Search");
                $scope.search_flag = false;
            }
            else {
                $scope.search_flag = true;

                if ($scope.search123 == "4") {
                    $scope.txt = false;
                    $scope.numbr = false;
                    $scope.dat = true;

                }
                else {
                    $scope.txt = true;
                    $scope.numbr = false;
                    $scope.dat = false;

                }
                $scope.searchtxt = "";
                $scope.searchdat = "";
                $scope.searchnumbr = "";

            }
        }

        $scope.clearsearch = function () {
            $state.reload();
        }


        $scope.ShowSearch_Report = function () {
            if ($scope.searchtxt !== "" || $scope.searchnumbr !== "" || $scope.searchdat !== "") {
               if ($scope.search123 == "4") {
                    var date = new Date($scope.searchdat).toDateString("dd/MM/yyyy");
                    var data = {
                        "searchType": $scope.search123,
                        "searchdate": date,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id
                    }
                }
                else {
                    var data = {
                        "searchType": $scope.search123,
                        "searchtext": $scope.searchtxt,
                        "ASMAY_ID": $scope.cfg.ASMAY_Id
                    }

                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("FeeActivityRequest/searching", data).
                    then(function (promise) {
                        $scope.receiptgrid = promise.searcharray;
                        $scope.totcountsearch = promise.searcharray.length;

                        if (promise.searcharray == null || promise.searcharray == "") {
                            swal("Record Does Not Exist For Searched Data !!!!!!")
                        }
                    })
            }
            else {
                swal("Data Is Needed For Search ");
            }

        }

        $scope.Clearid = function () {
            $state.reload();
        }


        $scope.printdatatable = [];
        $scope.printData = function (printSectionId) {
                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                    '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                    '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
                $state.reload();
        };

      
        $scope.exportToExcel = function (tableId) {
            var excelname = "Activity Approval Report";
            excelname = excelname.toUpperCase() + '.xls';

                var exportHref = Excel.tableToExcel(tableId, 'WireWorkbenchDataExport');

            //$timeout(function () { location.href = exportHref; }, 100);

            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);

                //$timeout(function () {
                //    location.href = exportHref;
                //}, 100);
        };
      
    };
})();
