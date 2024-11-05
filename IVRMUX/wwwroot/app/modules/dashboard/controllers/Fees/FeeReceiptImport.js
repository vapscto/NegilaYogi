
(function () {
    'use strict';
    angular
        .module('app')
        .controller('FeeReceiptImportController', FeeReceiptImportController)

    FeeReceiptImportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'dashboardService', 'Flash', 'superCache', '$filter']
    function FeeReceiptImportController($rootScope, $scope, $state, $location, apiService, dashboardService, Flash, superCache, $filter) {
        $scope.filename = '';
        var vm = this;
        vm.gridOptions = {};
        $scope.feeHead = [];
        $scope.feeGroup = [];
        $scope.Clearid = function () {
            $state.reload();
        }
        $scope.reverse = false;
        $scope.order = function (keyname) {

            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        }
        $scope.SelectedFileForUploadzd = [];
        $scope.selectFileforUploadzd = function (input) {

            $scope.SelectedFileForUploadzd = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {

                    var reader = new FileReader();
                    reader.readAsDataURL(input.files[0]);
                    $scope.filename = input.files[0].name;

                }
                else {

                    $scope.filename = input.files[0].name;
                }
            }

        }

        $scope.loaddata = function () {

            var pageid = 2;
            apiService.getURI("FeeReceiptImport/getdetails", pageid).
                then(function (promise) {


                    $scope.feeGroup = promise.feeGroup;
                    $scope.feeHead = promise.feeHead;


                })
        }


        $scope.savedata1 = function () {



        }
        $rootScope.validateForm = function () {

            console.log($scope.opts.data);
            var data1 = {
                "newlstget1": $scope.opts.data
            }
            apiService.create("FeeReceiptImport/checkvalidation", data1).then(function (promise) {
                if (promise.stuStatus != "" && promise.stuStatus != null) {
                    swal(promise.stuStatus);
                    $elm.val(null);
                    $scope.opts.data = null;
                } else {
                    $scope.opts.data = $scope.opts.data;
                }
            });
        }
        $scope.savedata = function (gridOptions) {

           // var data = {};
            var valu = gridOptions;
            $scope.albumNameArray = [];


            if (valu.length > 0) {
                $scope.albumNameArray = [];

                for (var i = 0; i < valu.length; i++) {

                    $scope.feeheadlist = [];
                    var amt = 0;
                    for (var j = 0; j < $scope.feeHead.length; j++) {
                        
                        for (let key of Object.entries(valu[i])) {
                    
                            if ($scope.feeHead[j].fmH_FeeName == key[0] && key[1] > 0) {
                                amt = amt + Number(key[1]);
                                $scope.feeheadlist.push({
                                    FMH_FeeName: $scope.feeHead[j].fmH_FeeName,
                                    Amount: Number(key[1]),
                                    FMH_Id: $scope.feeHead[j].fmH_Id

                                })

                            }
                        }
                    }
                    if (amt == Number(valu[i].FYP_Tot_Amount)) {
                        $scope.albumNameArray.push(
                            {
                                AMST_AdmNo: valu[i].AMST_AdmNo,
                                ASMAY_Year: valu[i].ASMAY_Year,
                               // FYP_Receipt_No: valu[i].FYP_Receipt_No,
                                FYP_Bank_Name: valu[i].FYP_Bank_Name,
                                FYP_Bank_Or_Cash: valu[i].FYP_Bank_Or_Cash,
                                FYP_DD_Cheque_No: valu[i].FYP_DD_Cheque_No,
                                FYP_DD_Cheque_Date: valu[i].FYP_DD_Cheque_Date,
                                FYP_Date: valu[i].FYP_Date,
                                FMG_GroupName: valu[i].FMG_GroupName,
                                FTI_Name: valu[i].FTI_Name,
                                FYP_Tot_Amount: Number(valu[i].FYP_Tot_Amount),
                                Feeheadimport: $scope.feeheadlist



                            }

                        );
                    }
                   
                }
               

            }

            angular.forEach($scope.albumNameArray, function (gg) {
                gg.FYP_DD_Cheque_Date == null ? "NULL" : $filter('date')(gg.FYP_DD_Cheque_Date, "yyyy-MM-dd");
                gg.FYP_Date == null ? "NULL" : $filter('date')(gg.FYP_Date, "yyyy-MM-dd");

            })

        


        var data = {
            FeeReceiptimport: $scope.albumNameArray
        }
        
        apiService.create("FeeReceiptImport/ ", data).
            then(function (promise) {

               
                if (promise.returnval == true) {
                    swal("Row  Saved --" + promise.suscnt + " " + "Failed Row --" + promise.failcnt + " " + "Existing Row --" + + promise.dupcnt);

                }
                else {
                    swal("Please Select Correct File Format");

                }

                $state.reload();

            })

    }
}
}) ();
angular
    .module('app').filter('keys', function () {

        return function (input) {
            if (!input) {
                return [];
            }
            delete input.$$hashKey;
            return Object.keys(input);
        }

    })

angular
    .module('app').directive("fileread", ['$rootScope', 'apiService', function ($rootScope, apiService) {

        return {
            scope: {
                opts: '='

            },
            link: function ($scope, $elm, $attrs) {

                $elm.on('change', function (changeEvent) {

                    var reader = new FileReader();

                    reader.onload = function (evt) {
                        $scope.$apply(function () {
                            var data = evt.target.result;

                            var workbook = XLSX.read(data, { type: 'binary' });

                            var headerNames = XLSX.utils.sheet_to_json(workbook.Sheets[workbook.SheetNames[0]], { header: 1 })[0];

                            var data = XLSX.utils.sheet_to_json(workbook.Sheets[workbook.SheetNames[0]]);
                            if (data.length == 0) {

                                swal("Excel Sheet is Empty");
                                $elm.val(null);
                                $scope.opts.data = null;
                                return;

                            } else {
                                $scope.opts = {};

                                $scope.opts.columnDefs = [];

                                headerNames.forEach(function (h) {
                                    $scope.opts.columnDefs.push({ field: h });
                                });
                                //dfsd
                                $scope.opts.data = data;
                                // $rootScope.validateForm();

                                console.log($scope.opts.data);
                                var data1 = {
                                    "newlstget1": $scope.opts.data
                                }
                                apiService.create("FeeReceiptImport/checkvalidation", data1).then(function (promise) {
                                    if (promise.stuStatus != "" && promise.stuStatus != null) {
                                        swal(promise.stuStatus);
                                        $elm.val(null);
                                        $scope.opts.data = null;
                                    } else {
                                        $scope.opts.data = $scope.opts.data;
                                    }
                                });


                                //$elm.val(null);
                            }
                        });
                    };
                    reader.readAsBinaryString(changeEvent.target.files[0]);

                });
            }
        }
    }]);