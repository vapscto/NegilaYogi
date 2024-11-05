(function () {
    'use strict';
    angular
        .module('app')
        .controller('ECSBulkImportController', ECSBulkImportController)

    ECSBulkImportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'dashboardService', 'Flash', 'superCache']
    function ECSBulkImportController($rootScope, $scope, $state, $location, apiService, dashboardService, Flash, superCache) {

    //var ECS = this;

    //ECS.gridOptions = {};

    var vm = this;

    vm.gridOptions = {};


    $scope.Clearid = function () {
        $state.reload();
    }

    $scope.BindData = function () {
        var pageid = 1;
        apiService.getURI("ECSBulkImport/getalldetails123/",pageid).then(function (promise) {
            $scope.acdlist = promise.yearlist;

   })
    };

  

    $scope.SelectedFileForUploadzd = [];

    $scope.selectFileforUploadzd = function (input) {
        

        $scope.SelectedFileForUploadzd = input.files;

        if (input.files && input.files[0]) {

            if (input.files[0].type == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                var reader = new FileReader();
                reader.readAsDataURL(input.files[0]);
            }
            //else {
            //    swal("Please Upload the .xlsx file");
            //    return;
            //}
        }


    }





    $rootScope.validateForm = function () {
        
        console.log($scope.opts.data);
        var data1 = {
            "newlstget1": $scope.opts.data
        }
        apiService.create("ECSBulkImport/checkvalidation", data1).then(function (promise) {
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
            //var clmns = ECS.gridOptions.columnDefs.length;
            for (var i = 0; i < valu.length; i++) {
                $scope.albumNameArray.push(valu[i]);
            }
        }
        var data1 = {
            "SMS": $scope.active,
            "Email": $scope.deactive,
            "newlstget1": $scope.albumNameArray
        }
        //var data1 = {
        //    "newlstget1": $scope.opts.data
        //}
        apiService.create("ECSBulkImport/checkvalidation", data1).then(function (promise) {
            if (promise.stuStatus != "" && promise.stuStatus != null) {
                swal(promise.stuStatus);
                $elm.val(null);
                $scope.opts.data = null;
            } else {
                $scope.opts.data = $scope.opts.data;
            }
        });
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
    .module('app').directive("fileread1", ['$rootScope', 'apiService', function ($rootScope, apiService) {
    
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
                            //var data1 = {
                            //    "newlstget1": $scope.opts.data
                            //}
                            //apiService.create("ECSBulkImport/checkvalidation", data1).then(function (promise) {
                            //    if (promise.stuStatus != "" && promise.stuStatus != null) {
                            //        swal(promise.stuStatus);
                            //        $elm.val(null);
                            //        $scope.opts.data = null;
                            //    } else {
                            //        $scope.opts.data = $scope.opts.data;
                            //    }
                            //});
                        }
                    });
                };
                reader.readAsBinaryString(changeEvent.target.files[0]);

            });
        }
    }
}]);





