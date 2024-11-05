
(function () {
    'use strict';
    angular
.module('app')
.controller('SportMasterUOMController', SportMasterUOMController)

    SportMasterUOMController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache']
    function SportMasterUOMController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache) {

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};

        $scope.sortKey = 'SPCCMUOM_Id';
        $scope.sortReverse = true;

        //var paginationformasters;
        //var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        //if (ivrmcofigsettings.length > 0) {
        //    paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        //}

        $scope.ddate = {};
        $scope.ddate = new Date();

        //$scope.usrname = localStorage.getItem('username');
        //var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        //if (admfigsettings.length > 0) {
        //    var logopath = admfigsettings[0].asC_Logo_Path;
        //}

        //$scope.imgname = logopath;

        //var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));



        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            apiService.getDATA("SportMasterUOM/Getdetails").
       then(function (promise) {
           
           $scope.currentPage = 1;
           $scope.itemsPerPage = 10;
           if (promise.count > 0) {
               $scope.newuser = promise.gridviewDetails;
               $scope.presentCountgrid = $scope.newuser.length;
           }
           $scope.cancel();
       })
        };


        // to Edit Data
        $scope.EditSportMasterUOMdata = function (EditRecord) {
            
            $scope.EditId = EditRecord.spccmuoM_Id;
            var MEditId = $scope.EditId;
            apiService.getURI("SportMasterUOM/GetSelectedRowDetails/", MEditId).
            then(function (promise) {
                
                $scope.name = promise.gridviewDetails[0].spccmuoM_UOMName;
                $scope.SPCCMUOM_Id = promise.gridviewDetails[0].spccmuoM_Id;
            })
        };

        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        // TO Save The Data
        $scope.submitted = false;
        $scope.saveSportMasterUOMdata = function () {
            
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                
                var data = {
                    "SPCCMUOM_Id": $scope.SPCCMUOM_Id,
                    "SPCCMUOM_UOMName": $scope.name
                   

                }
                apiService.create("SportMasterUOM/", data).
                    then(function (promise) {
                        
                        if (promise.msg != "" && promise.msg != null) {
                            swal(promise.msg);
                        }
                        else if (promise.returnVal == true) {
                            swal("Record Saved Successfully");
                        }
                        else if (promise.returnVal_update == true) {
                            swal("Record Updated Successfully");
                        }
                        else if (promise.duplicate_UOM_name_bool == true) {
                            swal("UOM Name Already Exists");
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
            if (newuser1.spccmuoM_ActiveFlag == false) {

                mgs = "Activate";

            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the UOM?",
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
                     apiService.create("SportMasterUOM/deactivate", newuser1).
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
            $scope.SPCCMUOM_Id = 0;
            $scope.name = '';
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        }

        $scope.searchValue = "";
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.spccmuoM_UOMName)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }

    }

})();