
(function () {
    'use strict';
    angular
.module('app')
.controller('TTSubjectMasterController', TTSubjectMasterController)

    TTSubjectMasterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache']
    function TTSubjectMasterController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache) {
        $scope.editEmployee = {};
       

        //Ui Grid view rendering data from data base
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                  { name: 'SNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },                    
             { name: 'subjectName', displayName: 'Subject Master' },
            { name: 'ttmsuaB_Abbreviation', displayName: 'Subject Abbreviation' },
               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
             
                     '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                '<a ng-if="row.entity.ttmsuaB_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.ttmsuaB_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
          
                '</div>'
               }
            ]          

        };



        // TO Save The Data
        $scope.submitted = false;
        $scope.savesubabbreviationdata = function () {
            $scope.submitted = true;

            if ($scope.myForm.$valid) {

                var data = {
                    "TTMSUAB_Id": $scope.TTMSUAB_Id,
                    "ISMS_Id": $scope.ismS_Id.ismS_Id,
                    "TTMSUAB_Abbreviation": $scope.ttmsuaB_Abbreviation,
                }
                apiService.create("TTSubjectMaster/savedetail", data).
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
            apiService.getDATA("TTSubjectMaster/getalldetails").
       then(function (promise) {
           $scope.currentPage = 1;
           $scope.itemsPerPage = 5;
           $scope.sublst = promise.sujectslist;
           $scope.gridOptions.data = promise.ttsujectslist;
       })
        };
        //TO clear  data
        $scope.clearid= function () {      
            $scope.ttmsuaB_Abbreviation = "";
            $scope.TTMSUAB_Id = 0;
            $scope.ismS_Id = "";
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";


        };
        //TO  Edit Record
        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.ttmsuaB_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("TTSubjectMaster/getdetails", pageid).
            then(function (promise) {

                $scope.ismS_Id = promise.sujectslistedit[0].ismS_Id;
                $scope.ttmsuaB_Abbreviation = promise.sujectslistedit[0].ttmsuaB_Abbreviation;
                $scope.TTMSUAB_Id = promise.sujectslistedit[0].ttmsuaB_Id;
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
                    apiService.DeleteURI("TTSubjectMaster/deletepages", pageid).
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
            if (employee.ttmsuaB_ActiveFlag === true) {
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

                apiService.create("TTSubjectMaster/deactivate", employee).
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