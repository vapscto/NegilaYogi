
(function () {
    'use strict';
    angular
.module('app')
.controller('SportMasterHouseController', SportMasterHouseController)

    SportMasterHouseController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache']
    function SportMasterHouseController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache) {

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};

        $scope.sortKey = 'SPCCMH_Id';
        $scope.sortReverse = true;

       
        $scope.ddate = {};
        $scope.ddate = new Date();

        //==================================TO  GEt The Values iN Grid
        $scope.BindData = function () {
            var id = 2;
            apiService.getURI("SportsMasterHouse/getdetails/", id).
       then(function (promise) {
           
           $scope.currentPage = 1;
           $scope.itemsPerPage = 10;
         
               $scope.newuser = promise.gridviewDetails;
       })
        };

      
        //======================================== to Edit Data
        $scope.EditSportMasterHousedata = function (EditRecord) {          
            $scope.name = EditRecord.spccmH_HouseName;
            $scope.description = EditRecord.spccmH_HouseDescription;
            $scope.SPCCMH_Id = EditRecord.spccmH_Id;
            $scope.spccmH_Flag = EditRecord.spccmH_Flag;

        };

        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        //================================================ TO Save The Data
        $scope.submitted = false;
        $scope.saveSportMasterHousedata = function () {
            
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                
                var data = {
                    "SPCCMH_Id": $scope.SPCCMH_Id,
                    "SPCCMH_HouseName": $scope.name,
                    "SPCCMH_HouseDescription": $scope.description,
                    "SPCCMH_Flag": $scope.spccmH_Flag


                }
                apiService.create("SportsMasterHouse/savedata", data).
                    then(function (promise) {
                        
                        if (promise.msg != "" && promise.msg != null) {
                            swal(promise.msg);
                        }
                        else if (promise.returnVal == true) {
                            swal("Record Saved Successfully");
                            $state.reload();
                        }
                        else if (promise.returnVal_update == true) {
                            swal("Record Updated Successfully");
                            $state.reload();
                        }
                      
                        else if (promise.duplicate_caste_name_bool == true) {
                            swal("House Name Already Exists");
                        }
                        else if (promise.returnVal == false) {
                            swal("Failed to Save");
                        }
                        else if (promise.returnVal_update == false) {
                            swal("Failed to Update");
                        }
                        $scope.BindData();
                    })
            }
        };

        $scope.deactive = function (newuser1, SweetAlertt) {
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            
            var mgs = "";
            if (newuser1.spccmH_ActiveFlag == false) {

                mgs = "Activate";

            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the House?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
             function (isConfirm) {
                 if (isConfirm) {
                     apiService.create("SportsMasterHouse/deactivate", newuser1).
                     then(function (promise) {
                         
                         if (promise.returnVal == true) {
                             if (promise.msg != null) {
                                 swal(promise.msg);
                                 $scope.BindData();
                             }
                         }
                         else {
                             swal('Failed to Activate/Deactivate the Record');
                         }
                     })
                 } else {
                     swal("Cancelled");
                 }
             })
        }


        $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        }

        $scope.IsHidden2 = true;
        $scope.ShowHide2 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden2 = $scope.IsHidden2 ? false : true;
        }

        $scope.cancel = function () {
            $scope.SPCCMH_Id = 0;
            $scope.name = "";
            $scope.description = "";
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        }

        $scope.searchValue = "";
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.spccmH_HouseDescription)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.spccmH_HouseName)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }

    }

})();