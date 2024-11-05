
(function () {
    'use strict';
    angular
.module('app')
.controller('CategoryController', CategoryController)

    CategoryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams','superCache']
    function CategoryController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache) {
        $scope.editEmployee = {};
        //Ui Grid view rendering data from data base
        $scope.gridOptions = {
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                  { name: 'SlNo', field: 'name', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'ttmC_CategoryName', displayName: 'CategoryName' },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:                    
                       '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                '<a ng-if="row.entity.ttmC_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.ttmC_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +          
                '</div>'
               }
            ]
        };


        
        // TO Save The Data
        $scope.submitted = false;
        $scope.savecategoryddata = function () {
            $scope.submitted = true;

            if ($scope.myForm.$valid) {

                var data = {
                    "TTMC_Id": $scope.TTMC_Id,
                    "TTMC_CategoryName": $scope.TTMC_CategoryName,
                    "TTMC_ActiveFlag":true,
                }
                apiService.create("Category/savedetail", data).
                    then(function (promise) {
                        if (promise.returnval === true)
                        {
                            swal('Data successfully Saved');
                            $scope.BindData();
                            $scope.clearid();
                        }
                        else if (promise.returnval === false && promise.returnduplicatestatus === 'Duplicate')
                        {
                            swal('Records Already Exist !');                            
                        }
                        else
                        {
                            swal('Data Not Saved !');
                        }
                    })
            }

        };

        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            apiService.getDATA("Category/getalldetails").
       then(function (promise) {
           
           $scope.currentPage = 1;
           $scope.itemsPerPage = 5;        
           $scope.gridOptions.data = promise.categorylist;
       })
        };
        //TO clear  data
        $scope.clearid= function () {      
            $scope.TTMC_CategoryName = "";         
            $scope.TTMC_Id = 0;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";


        };
        //TO clear  data
        $scope.clearCategory = function () {
            $scope.submitted = false;
            $scope.ttmC_Id = "";

        };
        //TO  Edit Record
        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.ttmC_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("Category/getdetails", pageid).
            then(function (promise) {
                $scope.TTMC_CategoryName = promise.categorylistedit[0].ttmC_CategoryName;
                $scope.TTMC_Id = promise.categorylistedit[0].ttmC_Id;
            })
        }
        //TO  delete Record
        $scope.deletedata = function (employee, SweetAlert) {
            $scope.editEmployee = employee.ttmC_Id;
            var pageid = $scope.editEmployee;
            swal({
                title: "Are you sure",
                text: "Do you want to delete record????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("Category/deletepages", pageid).
                    then(function (promise) {
                        if (promise.returnval === true) {
                            swal('Record Deleted Successfully');                           
                        }
                        else {
                            swal('Record Not Deleted Successfully!');                           
                        }
                        $scope.BindData();
                    })
                    $scope.BindData();
                }
                else {
                    swal("Record Deletion Cancelled");
                }
            });
        };

        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };

        $scope.deactive = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.ttmC_ActiveFlag === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";

            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";

            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
        function (isConfirm) {
            if (isConfirm) {

                apiService.create("Category/deactivate", employee).
                then(function (promise) {
                    if (promise.returnval == true) {
                        swal(confirmmgs + " " + "successfully.");
                    }
                    else {
                        swal(confirmmgs + " " + " successfully");
                    }
                    $scope.BindData();
                })

            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
        });
        }
    }

})();