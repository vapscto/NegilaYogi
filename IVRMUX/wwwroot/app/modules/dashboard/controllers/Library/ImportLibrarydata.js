
(function () {
    'use strict';
    angular
        .module('app')
        .controller('ImportLibrarydataController', ImportLibrarydataController)

    ImportLibrarydataController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'dashboardService', 'Flash', 'superCache', '$filter']
    function ImportLibrarydataController($rootScope, $scope, $state, $location, apiService, dashboardService, Flash, superCache, $filter) {
        $scope.filename = '';
        var vm = this;
        vm.gridOptions = {};
        $scope.Clearid = function () {
            $state.reload();
        }
        $scope.reverse = false;
        $scope.order = function (keyname) {

            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        $scope.SelectedFileForUploadzd = [];
        $scope.selectFileforUploadzd = function (input) {


            $scope.SelectedFileForUploadzd = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {

                    //once return success 
                    var reader = new FileReader();
                    reader.readAsDataURL(input.files[0]);
                    $scope.filename = input.files[0].name;
                    // alert($scope.filename);
                }
                else {
                    //swal("Please Upload the .xlsx file");
                    //return;
                    $scope.filename = input.files[0].name;
                }
            }

        }


        $scope.BindData = function () {
            $scope.masterlibarary = [];
            var pageid = 2;
            apiService.getURI("ImportLibrarydata/getdetails", pageid).then(function (promise) {
                if (promise.masterlibarary != null && promise.masterlibarary.length > 0) {
                    $scope.masterlibarary = promise.masterlibarary;
                }

            })


        }
        $rootScope.validateForm = function () {

            console.log($scope.opts.data);
            var data1 = {
                "newlstget1": $scope.opts.data
            }
            apiService.create("ImportLibrarydata/checkvalidation", data1).then(function (promise) {
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

            var data = {};
            var valu = gridOptions;
            $scope.albumNameArray = [];
            if (valu.length > 0) {
                $scope.albumNameArray = [];
                // var clmns = vm.gridOptions.columnDefs.length;
                for (var i = 0; i < valu.length; i++) {
                    $scope.albumNameArray.push(valu[i]);


                }

                angular.forEach($scope.albumNameArray, function (gg) {
                    gg.LBTR_IssuedDate == null ? "NULL" : $filter('date')(gg.LBTR_IssuedDate, "yyyy-MM-dd");
                    gg.LBTR_ReturnedDate == null ? "NULL" : $filter('date')(gg.LBTR_ReturnedDate, "yyyy-MM-dd");

                })

            }
            if ($scope.LMAL_Id > 0) {

            }
            else {
                swal("Libarary Name Is Not Selected !");
                return;
            }
            if ($scope.filename == 'LIB.LIB_Master_Subject.xlsx') {
                data = {
                    "mastersub": $scope.albumNameArray,
                    "LMAL_Id": $scope.LMAL_Id
                }
            }
            if ($scope.filename == 'LIB.LIB_Master_Department.xlsx') {
                data = {
                    "masterdep": $scope.albumNameArray,
                    "LMAL_Id": $scope.LMAL_Id
                }
            }

            if ($scope.filename == 'LIB.LIB_Master_Language.xlsx') {
                data = {
                    "masterlang": $scope.albumNameArray
                }
            }

            if ($scope.filename == 'LIB.LIB_Master_Publisher.xlsx') {
                data = {
                    "masterpubl": $scope.albumNameArray,
                    "LMAL_Id": $scope.LMAL_Id
                }
            }

            if ($scope.filename == 'LIB_Master_Book.xlsx') {
                data = {
                    "masterbook": $scope.albumNameArray,
                    "LMAL_Id": $scope.LMAL_Id
                }
            }
            if ($scope.filename == 'LIB_Master_Rack.xlsx') {
                data = {
                    "masterrack": $scope.albumNameArray,
                    "LMAL_Id": $scope.LMAL_Id
                }
            }
            if ($scope.filename == 'LIB_Master_Book_Author.xlsx') {
                data = {
                    "masterauthor": $scope.albumNameArray,
                    "LMAL_Id": $scope.LMAL_Id
                }
            }
            if ($scope.filename == 'LIB_Master_Book_AccnNo.xlsx') {
                data = {
                    "masteracces": $scope.albumNameArray,
                    "LMAL_Id": $scope.LMAL_Id
                }
            }
            if ($scope.filename == 'LIB_trans.xlsx') {
                data = {
                    "booktransIMP": $scope.albumNameArray,
                    "LMAL_Id": $scope.LMAL_Id
                }
            }
            if ($scope.filename == 'vendor.xlsx') {
                data = {
                    "mastervender": $scope.albumNameArray,
                    "LMAL_Id": $scope.LMAL_Id
                }
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("ImportLibrarydata/ ", data).
                then(function (promise) {
                    //if (promise.returnMsg != null || promise.returnMsg != "") {
                    //    alert(promise.returnMsg);
                    //}

                    if (promise.returnval == true) {
                        swal("Row  Saved --" + promise.suscnt + " " + "Failed Row --" + promise.failcnt + " " + "Existing Row --" + + promise.dupcnt);
                        //swal("Data Saved Successfully", "Except These Records" + promise.returnMsg);
                    }
                    else {
                        swal("Please Select Correct File Format");

                    }

                    //if (promise.stuStatus == "true")
                    //{
                    //    swal("Records Saved Successfully");
                    //}
                    //else
                    //{
                    //    swal("Records not Saved");
                    //}
                    $scope.albumNameArray = [];
                    swal(promise.failcnt);
                    
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
                                apiService.create("ImportLibrarydata/checkvalidation", data1).then(function (promise) {
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