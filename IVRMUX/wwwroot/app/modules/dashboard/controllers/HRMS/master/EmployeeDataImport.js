
(function () {
    'use strict';
    angular
        .module('app')
        .controller('EmployeeDataImportController', EmployeeDataImportController)

    EmployeeDataImportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'dashboardService', 'Flash', 'superCache', '$filter']
    function EmployeeDataImportController($rootScope, $scope, $state, $location, apiService, dashboardService, Flash, superCache, $filter) {
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
                    $scope.filename = input.files[0].name;
                }
            }

        }


        $scope.savedata1 = function () {



        }
        $rootScope.validateForm = function () {

            console.log($scope.opts.data);
            var data1 = {
                "newlstget1": $scope.opts.data
            }
            apiService.create("EmployeeDataImport/checkvalidation", data1).then(function (promise) {
                if (promise.stuStatus != "" && promise.stuStatus != null) {
                    swal(promise.stuStatus);
                    $elm.val(null);
                    $scope.opts.data = null;
                } else {
                    $scope.opts.data = $scope.opts.data;
                }
            });
        }
        $scope.exptoex = function () {

            window.open("https://dcampusstrg.blob.core.windows.net/files/17/Trnsportdocuments/7f3f4adf-0f9d-43f9-8ce7-4757bfe66ca7.xlsx");                    

        };
        $scope.savedata = function (gridOptions) {
          
            var data = {};
            var valu = gridOptions;
            $scope.albumNameArray = [];
            if (valu.length > 0) {
                $scope.albumNameArray = [];
                for (var i = 0; i < valu.length; i++) {
                    $scope.albumNameArray.push(valu[i]);


                }

            }

            
            //if ($scope.filename == 'HRMS_EMP.xlsx') {
            if ($scope.filename.length>0) {
                if ($scope.albumNameArray != null && $scope.albumNameArray.length > 0) {

                    for (var i = 0; i < $scope.albumNameArray.length; i++) {
                        if ($scope.albumNameArray[i].EmployeeType == undefined || $scope.albumNameArray[i].EmployeeType == "") {
                            swal("Employee Type Should Be Upload ");
                            return;
                        }
                        if ($scope.albumNameArray[i].EmployeeGroupType == undefined || $scope.albumNameArray[i].EmployeeGroupType == "") {
                            swal("Employee Group Type Should Be Upload ");
                            return;
                        }
                        if ($scope.albumNameArray[i].DepartmentName == undefined || $scope.albumNameArray[i].DepartmentName == "") {
                            swal("Department Name Should Be Upload ");
                            return;
                        }
                        if ($scope.albumNameArray[i].DesignationName == undefined || $scope.albumNameArray[i].DesignationName == "") {
                            swal("Designation Name Should Be Upload ");
                            return;
                        }
                        if ($scope.albumNameArray[i].GradeName == undefined || $scope.albumNameArray[i].GradeName == "") {
                            swal("Grade Name Should Be Upload ");
                            return;
                        }
                        if ($scope.albumNameArray[i].EmployeeFirstName == undefined || $scope.albumNameArray[i].EmployeeFirstName == "") {
                            swal("Employee First Name Should Be Upload ");
                            return;
                        }
                        //if ($scope.albumNameArray[i].EmployeeMiddleName == undefined || $scope.albumNameArray[i].EmployeeMiddleName == "") {
                        //    swal("Employee Middle Name Should Be Upload ");
                        //    return;
                        //}
                        //if ($scope.albumNameArray[i].EmployeeLastName == undefined || $scope.albumNameArray[i].EmployeeLastName == "") {
                        //    swal("Employee Last Name Should Be Upload ");
                        //    return;
                        //}
                        if ($scope.albumNameArray[i].EmployeeCode == undefined || $scope.albumNameArray[i].EmployeeCode == "") {
                            swal("Employee Code Should Be Upload ");
                            return;
                        }
                        if ($scope.albumNameArray[i].Marital_Status == undefined || $scope.albumNameArray[i].Marital_Status == "") {
                            swal("Marital Status Should Be Upload ");
                            return;
                        }
                        if ($scope.albumNameArray[i].employeeDOJ == undefined || $scope.albumNameArray[i].employeeDOJ == "") {
                            swal("DOJ Should Be Upload ");
                            return;
                        }
                        else {
                            $scope.albumNameArray[i].employeeDOJ = new Date($scope.albumNameArray[i].employeeDOJ).toDateString();
                        }

                        if ($scope.albumNameArray[i].employeeDOB == undefined || $scope.albumNameArray[i].employeeDOB == "") {
                            swal("DOB Should Be Upload ");
                            return;
                        }
                        else {
                            $scope.albumNameArray[i].employeeDOB = new Date($scope.albumNameArray[i].employeeDOB).toDateString();
                        }

                        if ($scope.albumNameArray[i].pincode == undefined || $scope.albumNameArray[i].pincode == "") {
                            swal("PinCode Should Be Upload ");
                            return;
                        }
                        if ($scope.albumNameArray[i].employeeaddress1 == undefined || $scope.albumNameArray[i].employeeaddress1 == "") {
                            swal("Address Should Be Upload ");
                            return;
                        }
                      
                        //if ($scope.albumNameArray[i].CasteCategory_Name == undefined || $scope.albumNameArray[i].CasteCategory_Name == "") {
                        //    swal("Caste Category Name Should Be Upload ");
                        //    return;
                        //}
                        //if ($scope.albumNameArray[i].Caste_Name == undefined || $scope.albumNameArray[i].Caste_Name == "") {
                        //    swal("Caste Name Should Be Upload ");
                        //    return;
                       // }
                        if ($scope.albumNameArray[i].Religion_Name == undefined || $scope.albumNameArray[i].Religion_Name == "") {
                            swal("Religion Name Should Be Upload ");
                            return;
                        }
                        if ($scope.albumNameArray[i].MobileNo == undefined || $scope.albumNameArray[i].MobileNo == "") {
                            swal("Mobile No Should Be Upload ");
                            return;
                        }
                        if ($scope.albumNameArray[i].EmailID == undefined || $scope.albumNameArray[i].EmailID == "") {
                            swal("Email ID Should Be Upload ");
                            return;
                        }

                    }
                    data = {
                        "empDataimport": $scope.albumNameArray
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }

                    apiService.create("EmployeeDataImport/ ", data).then(function (promise) {
                        if (promise.returnval == true) {
                            // swal("Row  Saved --" + promise.suscnt + " " + "Failed Row --" + promise.failcnt + " " + "Existing Row --" + + promise.dupcnt);
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
                                //dfsd
                                $scope.opts.data = data;
                                // $rootScope.validateForm();

                                console.log($scope.opts.data);
                                var data1 = {
                                    "newlstget1": $scope.opts.data
                                }
                                apiService.create("EmployeeDataImport/checkvalidation", data1).then(function (promise) {
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