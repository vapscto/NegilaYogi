
(function () {
    'use strict';
    angular
.module('app')
.controller('TTGenerationFromPreviousVersionController', TTGenerationFromPreviousVersionController)

    TTGenerationFromPreviousVersionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams']
    function TTGenerationFromPreviousVersionController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams) {
        $scope.editEmployee = {};
       

        //Ui Grid view rendering data from data base
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                  { name: 'SNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                     //{ name: 'yearName', displayName: 'Academic Year' },
              { name: 'staffName', displayName: 'Staff Name' },
            { name: 'ttmsaB_Abbreviation', displayName: 'Staff Abbreviation' },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                 //'<div class="grid-action-cell">' +
                 //'<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <md-tooltip md-direction="down">Edit</md-tooltip><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' +
                 //'<a ng-if="row.entity.ttmsaB_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.switch(row.entity);"><md-tooltip md-direction="down">Active Now</md-tooltip> <i class="fa fa-toggle-on text-red" aria-hidden="true"></i></a>' +
                 // '<span ng-if="row.entity.ttmsaB_ActiveFlag === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.switch(row.entity);"> <md-tooltip md-direction="down">Deactive Now</md-tooltip> <i class="fa fa-toggle-off text-green" aria-hidden="true"></i></a><span>' +
                 //'</div>'
                 //'<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>' +
                 //'<a href="javascript:void(0)" ng-click="grid.appScope.deletedata(row.entity);"> <i class="fa fa-trash text-danger"></i></a>' +
                   //'</div>'
                    '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                '<a ng-if="row.entity.ttmsaB_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.switch(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.ttmsaB_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.switch(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
          
                '</div>'
               }
            ]          

        };



        // TO Save The Data
        $scope.submitted = false;
        $scope.savestaffabbreviationdata = function () {
          

            if ($scope.myForm.$valid) {

                var data = {
                   // "ASMAY_Id": $scope.asmaY_Id,
                    "TTMSAB_Id":$scope.TTMSAB_Id,
                    "HRME_Id": $scope.Staff_Id,
                    "TTMSAB_Abbreviation": $scope.StaffAbbreviation,
                   // "TTMC_CategoryName": $scope.TTMC_CategoryName,   
                }
                apiService.create("TTGenerationFromPreviousVersion/savedetail", data).
                    then(function (promise) {
                        if (promise.returnval === true)
                        {
                            swal('Data successfully Saved');
                            $state.reload();
                        }
                        else if (promise.returnduplicatestatus === 'Duplicate')
                        {
                            swal('Records Already Exist !');
                            $state.reload();
                        }
                        else if (promise.returnval === false)
                        {
                            swal('Data Not Saved !');
                            $state.reload();
                        }
                        $scope.BindData();
                    })
              
                $scope.BindData();
               $scope.clearid();
            }
            else {
                $scope.submitted = true;

            }

        };

        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            apiService.getDATA("TTGenerationFromPreviousVersion/getalldetails").
       then(function (promise) {
           
           $scope.currentPage = 1;
           $scope.itemsPerPage = 5;
         //  $scope.yearlist = promise.acayear;
           $scope.version_list = promise.versionlist;
           $scope.gridOptions.data = promise.ttstafflist;
       })
        };
        //TO clear  data
        $scope.clearid= function () {      
           // $scope.TTMC_CategoryName = "";
           //$scope.TTMC_Id = 0;
            $scope.submitted = false;
            $scope.myForm.$setPristine(); 
            $scope.myForm.$setUntouched();
            $scope.search = "";
            $scope.replace_from = "";
            $scope.TTFG_Id = 0;
            $scope.replace_to = "";
           // $scope.StaffAbbreviation = "";

        };

        //TO  Edit Record
        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.ttmsaB_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("TTGenerationFromPreviousVersion/getdetails", pageid).
            then(function (promise) {
                $scope.TTMSAB_Id = promise.sujectslistedit[0].ttmsaB_Id;
               // $scope.asmaY_Id = promise.sujectslistedit[0].asmaY_Id;
                $scope.Staff_Id = promise.sujectslistedit[0].hrmE_Id;
                $scope.StaffAbbreviation = promise.sujectslistedit[0].ttmsaB_Abbreviation;
               // $scope.TTMC_CategoryName = promise.categorylistedit[0].ttmC_CategoryName;
              //  $scope.TTMC_Id = promise.categorylistedit[0].ttmC_Id;
            })
        }
        //TO  delete Record
        $scope.deletedata = function (employee, SweetAlert) {
            $scope.editEmployee = employee.ttmsaB_Id;
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
                    apiService.DeleteURI("TTGenerationFromPreviousVersion/deletepages", pageid).
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
                    $scope.BindData();
                }
            });
        };

        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };

        //active switch
        $scope.switch = function (employee, SweetAlert) {
            
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.ttmsaB_ActiveFlag === true) {
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

                apiService.create("TTGenerationFromPreviousVersion/deactivate", employee).
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