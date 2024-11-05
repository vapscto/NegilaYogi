(function () {
    'use strict';
    angular
        .module('app')
        .controller('EmployeeLogReportController', EmployeeLogReportController)

    EmployeeLogReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'dashboardService', 'Flash', 'superCache', '$filter']
    function EmployeeLogReportController($rootScope, $scope, $state, $location, apiService, dashboardService, Flash, superCache, $filter) {
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


                    var reader = new FileReader();
                    reader.readAsDataURL(input.files[0]);
                    $scope.filename = input.files[0].name;

                }
                else {
                    $scope.filename = input.files[0].name;
                }
            }

        }


        $scope.savedata = function (gridOptions) {

            var data = {};
            var valu = gridOptions;
            $scope.albumNameArray = [];
            if (valu.length > 0) {
                $scope.albumNameArray = [];
                for (var i = 0; i < valu.length; i++) {
                    var punchdate = new $filter('date')(valu[i].PunchDate, "yyyy-MM-dd");
                    $scope.albumNameArray.push({ EmployeeCode: valu[i].EmployeeCode, EmployeeFirstName: valu[i].EmployeeFirstName, PunchDate: punchdate, PunchIN_Time: valu[i].PunchIN_Time, PunchOut_Time: valu[i].PunchOut_Time, });
                }

            }

            if ($scope.filename.length > 0) {
                if ($scope.albumNameArray != null && $scope.albumNameArray.length > 0) {

                    for (var i = 0; i < $scope.albumNameArray.length; i++) {
                        if ($scope.albumNameArray[i].EmployeeFirstName == undefined || $scope.albumNameArray[i].EmployeeFirstName == "") {
                            swal("Employee First Name Should Be Upload");
                            return;
                        }
                        if ($scope.albumNameArray[i].EmployeeCode == undefined || $scope.albumNameArray[i].EmployeeCode == "") {
                            swal("Employee Code Should Be Upload");
                            return;
                        }
                        if ($scope.albumNameArray[i].PunchDate == undefined || $scope.albumNameArray[i].PunchDate == "") {
                            swal("Punch Date Should Be Upload");
                            return;
                        }

                        // if ($scope.albumNameArray[i].PunchIN_Time == undefined || $scope.albumNameArray[i].PunchIN_Time == "") {
                        //     swal("PunchIn Time Should Be Upload");
                        //    return;
                        //} 
                        // if ($scope.albumNameArray[i].PunchOut_Time == undefined || $scope.albumNameArray[i].PunchOut_Time == "") {
                        //     swal("PunchOut Time Should Be Upload");
                        //    return;
                        //} 

                    }
                    data = {
                        "empDataimport": $scope.albumNameArray
                    }
                    apiService.create("EmployeeLogImport/Savedata", data).then(function (promise) {
                        if (promise.returnval == true) {
                            swal("Data Saved Successfully");
                        }
                        else if (promise.message == "Exist") {
                            swal("Some Records Arleady Exist");
                        }
                        else {
                            swal("Please Select Correct File Format");
                        }

                        $state.reload();

                    })
                }
                else {

                    swal("Please Attach Exel File");
                }

            }
            else {
                swal("uploaded Exel Format Name is Differing");
            }

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

                                $scope.opts.data = data;


                                console.log($scope.opts.data);
                                var data1 = {
                                    "newlstget1": $scope.opts.data
                                }
                                apiService.create("FrontOfficeLogReport/checkvalidation", data1).then(function (promise) {
                                    if (promise.stuStatus != "" && promise.stuStatus != null) {
                                        swal(promise.stuStatus);
                                        $elm.val(null);
                                        $scope.opts.data = null;
                                    } else {
                                        $scope.opts.data = $scope.opts.data;
                                    }
                                });
                            }
                        });
                    };
                    reader.readAsBinaryString(changeEvent.target.files[0]);

                });
            }
        }
    }]);