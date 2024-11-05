
(function () {
    'use strict';
    angular
.module('app')
        .controller('collegeimportController', collegeimportController)

    collegeimportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'dashboardService', 'Flash', 'superCache']
    function collegeimportController($rootScope, $scope, $state, $location, apiService, dashboardService, Flash, superCache) {

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
               
                }
                else {
                    swal("Please Upload the .xlsx file");
                    return;
                }
            }     
       
        }
        $rootScope.validateForm = function () {
            
            console.log($scope.opts.data);
            var data1 = {
                "newlstget1": $scope.opts.data
            }
            apiService.create("collegeadmissionImport/checkvalidation", data1).then(function (promise) {
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

            var valu = gridOptions;
            $scope.albumNameArray = [];
            if (valu.length > 0) {
                $scope.albumNameArray = [];
                var clmns = vm.gridOptions.columnDefs.length;
                for (var i = 0; i < valu.length; i++) {
                    $scope.albumNameArray.push(valu[i]);
                }

            }
            var data = {

                "newlstget1": $scope.albumNameArray
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("collegeadmissionImport/", data).
            then(function (promise) {
                //if (promise.returnMsg != null || promise.returnMsg != "") {
                //    alert(promise.returnMsg);
                //}
                if (promise.stuStatus == "true") {
                    swal("Data Saved Successfully", "Failed And Saved Count" + promise.returnMsg);
                    //swal("Data Saved Successfully", "Except These Records" + promise.returnMsg);
                }
                else {
                    swal(promise.stuStatus);

                }

                //if (promise.stuStatus == "true")
                //{
                //    swal("Records Saved Successfully");
                //}
                //else
                //{
                //    swal("Records not Saved");
                //}

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
                                apiService.create("collegeadmissionImport/checkvalidation", data1).then(function (promise) {
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