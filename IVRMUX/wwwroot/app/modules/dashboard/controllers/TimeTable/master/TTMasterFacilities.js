
(function () {
    'use strict';
    angular
.module('app')
.controller('TTMasterFacilitiesController', TTMasterFacilitiesController)

    TTMasterFacilitiesController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache']
    function TTMasterFacilitiesController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache) {
        $scope.editEmployee = {};
       

        //Ui Grid view rendering data from data base
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                  { name: 'SNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },                    
                { name: 'ttmfA_FacilityName', displayName: 'Facility Name' },
                { name: 'ttmfA_FacilityDesc', displayName: 'Description' },
               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
             
                     '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                '<a ng-if="row.entity.ttmfA_ActiveFlg === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.ttmfA_ActiveFlg === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
          
                '</div>'
               }
            ]          

        };



        // TO Save The Data
        $scope.submitted = false;
        $scope.savedata = function () {
            $scope.submitted = true;

            if ($scope.myForm.$valid) {

                var data = {
                    "TTMFA_Id": $scope.TTMFA_Id,
                    "TTMFA_FacilityName": $scope.TTMFA_FacilityName,
                    "TTMFA_FacilityDesc": $scope.TTMFA_FacilityDesc,
                }
                apiService.create("TTMasterFacilities/savedetail", data).
                    then(function (promise) {
                        if (promise.returnval === true)
                        {
                            swal('Data Saved/Updated successfully');
                           
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
            apiService.getDATA("TTMasterFacilities/getalldetails").
       then(function (promise) {
           $scope.currentPage = 1;
           $scope.itemsPerPage = 5;
       
           $scope.gridOptions.data = promise.alldata;
       })
        };
        //TO clear  data
        $scope.clearid= function () {      
            $state.reload();


        };
        //TO  Edit Record
        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.ttmfA_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("TTMasterFacilities/getdetails", pageid).
            then(function (promise) {

                $scope.TTMFA_Id = promise.editlist[0].ttmfA_Id;
                $scope.TTMFA_FacilityName = promise.editlist[0].ttmfA_FacilityName;
                $scope.TTMFA_FacilityDesc = promise.editlist[0].ttmfA_FacilityDesc;
            })
        }
        //TO  delete Record
        $scope.deletedata = function (employee, SweetAlert) {
            $scope.editEmployee = employee.ttmsuaB_Id;
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
                    apiService.DeleteURI("TTMasterFacilities/deletepages", pageid).
                        then(function (promise) {
                            if (promise.returnval=='MAP') {
                                swal('Facilities Are Already Mapped with Room');
                            }
                            else {
                                if (promise.returnval === true) {
                                    swal('Record Deleted Successfully');
                                }
                                else {
                                    swal('Record Not Deleted Successfully!');
                                }
                                $scope.BindData();
                            }
                        
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
            if (employee.ttmfA_ActiveFlg === true) {
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

                apiService.create("TTMasterFacilities/deactivate", employee).
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