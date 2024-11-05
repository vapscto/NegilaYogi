
(function () {
    'use strict';
    angular
        .module('app')
        .controller('FeeReceiptImportStthomasController', FeeReceiptImportStthomasController)

    FeeReceiptImportStthomasController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'dashboardService', 'Flash', 'superCache', '$filter']
    function FeeReceiptImportStthomasController($rootScope, $scope, $state, $location, apiService, dashboardService, Flash, superCache, $filter) {
        $scope.filename = '';
        var vm = this;
        vm.gridOptions = {};
        $scope.feeHead = [];
        $scope.feeGroup = [];
        $scope.ldr = false;
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
            $scope.ldr = true;
            var pageid = 2;
            apiService.getURI("FeeReceiptImportStthomas/getdetails", pageid).
                then(function (promise) {
                    $scope.ldr = false;

                    $scope.feeGroup = promise.feeGroup;
                    $scope.feeHead = promise.feeHead;
                    $scope.Academicyearlist = promise.academicyearlist;


                })
        }


        $scope.savedata1 = function () {



        }
        $rootScope.validateForm = function () {

            console.log($scope.opts.data);
            var data1 = {
                "newlstget1": $scope.opts.data
            }
            apiService.create("FeeReceiptImportStthomas/checkvalidation", data1).then(function (promise) {
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
            $scope.ldr = true;
            // var data = {};
            var valu = gridOptions;
            $scope.albumNameArray = [];


            if (valu.length > 0) {
                $scope.albumNameArray = [];

                for (var i = 0; i < valu.length; i++) {

                    var dateString = valu[i].FYP_Date;
                    var parts = dateString.split('/');

                    var newDateString = parts[2] + '/' + parts[1] + '/' + parts[0];

                   
                    $scope.albumNameArray.push(
                        {
                            AMST_AdmNo: valu[i].AMST_AdmNo,

                            FYP_Date: newDateString,
                           // FYP_Date: valu[i].FYP_Date,


                            FYP_Tot_Amount: Number(valu[i].FYP_Tot_Amount),
                            Student_Name: valu[i].Student_Name,
                            FYP_Bank_Or_Cash: valu[i].FYP_Bank_Or_Cash,




                        }

                    );


                }


            }

            angular.forEach($scope.albumNameArray, function (gg) {

                gg.FYP_Date == null ? "NULL" : $filter('date')(gg.FYP_Date, "yyyy-MM-dd");

            })




            var data = {
                FeeReceiptimport: $scope.albumNameArray,
                "ASMAY_Id": $scope.ASMAY_Id
            }

            apiService.create("FeeReceiptImportStthomas/ ", data).
                then(function (promise) {
                    $scope.ldr = false;

                    if (promise.returnval == true) {
                        swal("Row  Saved --" + promise.suscnt + " " + "Due Date Wise Fine Amount Mismatch Count --" + promise.dupcnt + " " + "Record Not Exists --" + + promise.failcnt);

                    }
                    else {
                        swal("Please Select Correct File Format");

                    }

                    $state.reload();

                })

        }
    }
})();
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
                                apiService.create("FeeReceiptImportStthomas/checkvalidation", data1).then(function (promise) {
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