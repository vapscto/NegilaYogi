(function () {
    'use strict';
    angular
.module('app')
        .controller('FeeTallyTransactionController', FeeTallyTransactionController)
    FeeTallyTransactionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout']
    function FeeTallyTransactionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }



        $scope.temp = [
            {
                "Tally_VchType": "RECEIPTVOUCHER",
                "Tally_Vchno": "TEST1/18-19",
                "Tally_MasterID": "0",
                "Vaps_ID": "1457",
                "Tally_VchDate": "28-2-2019",
                "Tally_VchImportTime": "26-Mar-2019 | 12:36",
                "Tally_Status": "FAILED",
                "Tally_Status_Description": "Voucher Number\n'TEST1/18-19'\nalready exists!"
            },
            {
                "Tally_VchType": "RECEIPTVOUCHER",
                "Tally_Vchno": "TEST2/18-19",
                "Tally_MasterID": "252",
                "Vaps_ID": "1458",
                "Tally_VchDate": "28-2-2019",
                "Tally_VchImportTime": "26-Mar-2019 | 12:36",
                "Tally_Status": "SUCCESS",
                "Tally_Status_Description": "Imported Successfully"
            },
            {
                "Tally_VchType": "PAYMENTVOUCHER",
                "Tally_Vchno": "Refund1",
                "Tally_MasterID": "0",
                "Vaps_ID": "1459",
                "Tally_VchDate": "28-2-2019",
                "Tally_VchImportTime": "26-Mar-2019 | 12:36",
                "Tally_Status": "FAILED",
                "Tally_Status_Description": "Voucher Number\n'Refund1'\nalready exists!"
            },
            {
                "Tally_VchType": "JOURNALVOUCHER",
                "Tally_Vchno": "ADM/2019-2020/1",
                "Tally_MasterID": "253",
                "Vaps_ID": "1452",
                "Tally_VchDate": "28-2-2019",
                "Tally_VchImportTime": "26-Mar-2019 | 12:36",
                "Tally_Status": "SUCCESS",
                "Tally_Status_Description": "Imported Successfully"
            }
        ];

        $scope.changevouchertype = function (vouchertype) {

            $scope.Grid_view = false;

            var data = {
                "TMT_VoucherType": vouchertype +  'VOUCHER'
            }

            apiService.create("FeeTallyTransaction/getvouchertypedetails", data).
                then(function (promise) {

                    $scope.students = promise.tempararyArrayList;

                })

        }

        $scope.cfg = {};
        $scope.stuwiseorheadwise = "";
        $scope.formload = function () {
          
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters

            var data = {
                "TMT_VoucherTypeFlg": $scope.Vchtype
            }

            apiService.create("FeeTallyTransaction/getalldetails", data).
                then(function (promise) {

                    $scope.yearlst = promise.academicdrp;

                    $scope.cfg.ASMAY_Id = academicyrlst[0].asmaY_Id;

                    $scope.classcount = promise.allclsdata;

                    $scope.inscount = promise.allinsdata;
                    $scope.students = promise.tempararyArrayList;

                })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        $scope.submitted = false;
        $scope.SHOWSTU = function (inscount) {

            $scope.albumNameArray = [];
            angular.forEach($scope.inscount, function (option) {
                if (!!option.selected) $scope.albumNameArray.push(option);
            })

            $scope.submitted = true;
            if ($scope.myform.$valid) {

                if ($scope.albumNameArray.length>0) {

                var data = {
                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id
                    //TempararyArrayList: $scope.albumNameArray,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("FeeTallyTransaction/SHOWSTUDENT", data).
                    then(function (promise) {
                        if (promise.fillstudent.length > 0) {
                            $scope.totcountfirst = promise.fillstudent.length;
                            $scope.totalgrid = promise.fillstudent;
                        }

                    })
            }
            else {
                swal("Atleast Select any one Term")
            }
            }
            else {
                $scope.submitted = true;
            }
        }


        $scope.cleardata = function () {
            $state.reload();
        }


        $scope.toggleAll = function () {
            debugger;
            $scope.printdatatable = [];
            var toggleStatus = $scope.all;
            angular.forEach($scope.totalgrid, function (itm) {
                itm.isSelected = toggleStatus;
                if ($scope.all == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
          
        }

        $scope.toggleAllEX = function () {

            var exportHref = Excel.tableToExcel(Table, 'WireWorkbenchDataExport');

            $timeout(function () { location.href = exportHref; }, 100);
            //$scope.printdata = [];
            //var toggle = $scope.allexp;
            //angular.forEach($scope.students, function (ex) {
            //    ex.selected = toggle;
            //    if ($scope.allexp == true) {
            //        $scope.printdata.push(ex);
            //    }
            //    else {
            //        $scope.printdata.splice(ex);
            //    }
            //});
            //$scope.exporttoexcel();
        }

        
        $scope.submitted = false;
        $scope.GENERATEJV = function (totalgrid) {
            
            $scope.abc = 1;
            $scope.submitted = true;

            $scope.albumNameArray = [];
            var ftiidss = "0";
            angular.forEach($scope.inscount, function (option) {
                if (!!option.selected) {
                    ftiidss = ftiidss + "," + option.fmT_Id
                    $scope.albumNameArray.push(option);
                } 
            })


            var AMST_id = [];
            angular.forEach($scope.totalgrid, function (ty) {
                if (ty.isSelected) {
                    AMST_id.push(ty.amsT_Id);
                }
            })
          

            if ($scope.myform.$valid) {
                var data = {
                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ftiidss": ftiidss,
                    Amst_Ids: AMST_id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("FeeTallyTransaction/savedata", data).
                    then(function (promise) {

                        if (promise.returnval == "true") {
                            swal("Journal Vouchers Created Sucessfully");
                        }
                        else {
                            swal("Journal Vouchers Not Created Sucessfully")
                        }
                        
                        $state.reload();

                    })
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.GENERATERV = function () {
            var data = {
                "ASMAY_Id": $scope.cfg.ASMAY_Id,
                "From_Date": new Date($scope.FMCB_fromDATE).toDateString(),
                "To_Date": new Date($scope.FMCB_toDATE).toDateString(),
            }
            apiService.create("FeeTallyTransaction/deletedata", data).
                then(function (promise) {

                    if (promise.returnval == "true") {
                        swal("Receipt Vouchers Generated Sucessfully");
                    }
                    else {
                        swal("Receipt Vouchers Not Generated Sucessfully")
                    }

                    $state.reload();

                })
        }


        $scope.GENERATECONCESSIONJV = function (totalgrid) {

            $scope.abc = 1;
            $scope.submitted = true;

            $scope.albumNameArray = [];
            var ftiidss = "0";
            angular.forEach($scope.inscount, function (option) {
                if (!!option.selected) {
                    ftiidss = ftiidss + "," + option.fmT_Id
                    $scope.albumNameArray.push(option);
                }
            })


            var AMST_id = [];
            angular.forEach($scope.totalgrid, function (ty) {
                if (ty.isSelected) {
                    AMST_id.push(ty.amsT_Id);
                }
            })


            if ($scope.myform.$valid) {
                var data = {
                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ftiidss": ftiidss,
                    Amst_Ids: AMST_id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("FeeTallyTransaction/Concessiondata", data).
                    then(function (promise) {

                        if (promise.returnval == "true") {
                            swal("Concession Vouchers Created Sucessfully");
                        }
                        else {
                            swal("Concession Vouchers Not Created Sucessfully")
                        }

                        $state.reload();

                    })
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.GENERATEPAYMENTRV = function () {
            var data = {
                "ASMAY_Id": $scope.cfg.ASMAY_Id,
                "From_Date": new Date($scope.FMCB_fromDATE).toDateString(),
                "To_Date": new Date($scope.FMCB_toDATE).toDateString(),
            }
            apiService.create("FeeTallyTransaction/Paymentdata", data).
                then(function (promise) {

                    if (promise.returnval == "true") {
                        swal("Payment Vouchers Generated Sucessfully");
                    }
                    else {
                        swal("Payment Vouchers Not Generated Sucessfully")
                    }

                    $state.reload();

                })
        }
        
        $scope.Import = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 5
            var pageid = 1;
            $scope.tabel12 = false;
            $scope.tabel123 = false;

            var data = {
                tallyoutput: $scope.temp
            }

            apiService.create("FeeTallyTransaction/gettallydetails", data).
                then(function (promise) {
                    $scope.yearlst = promise.fillyear;

                })
        }

        $scope.Export = function () {
            debugger;
            $scope.Grid_view = true;
            $scope.students = $scope.students;
           // $scope.exporttoexcel();
        }

        $scope.exporttoexcel = function () {
            if ($scope.printdata != null && $scope.printdata.length > 0) {
                var exportHref = Excel.tableToExcel(Table, 'WireWorkbenchDataExport');
            }
            $timeout(function () { location.href = exportHref;}, 100);
        }
    }

})();



