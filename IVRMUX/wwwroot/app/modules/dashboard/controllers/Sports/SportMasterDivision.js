
(function () {
    'use strict';
    angular
.module('app')
.controller('SportMasterDivisionController', SportMasterDivisionController)

    SportMasterDivisionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache']
    function SportMasterDivisionController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache) {

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};

        $scope.sortKey = 'SPCCMD_Id';
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
            apiService.getDATA("SportMasterDivision/Getdetails").
       then(function (promise) {
           
           $scope.currentPage = 1;
           $scope.itemsPerPage = 10;
           if (promise.count > 0) {
               $scope.newuser = promise.gridviewDetails;
               $scope.presentCountgrid = $scope.newuser.length;
           }
           $scope.cancel();

           //$scope.Categaries = promise.SportMasterDivisionname;
       })
        };

        //delete record
        //$scope.DeleteSportMasterDivisiondata = function (DeleteRecord, SweetAlert) {
        //    
        //    $scope.deleteId = DeleteRecord.SPCCMD_Id;
        //    var MdeleteId = $scope.deleteId;

        //    // swal(id);
        //    swal({
        //        title: "Are you sure?",
        //        text: "Do you want to delete the Caste ..!?",
        //        type: "warning",
        //        showCancelButton: true,
        //        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
        //        cancelButtonText: "Cancel..!",
        //        closeOnConfirm: false,
        //        closeOnCancel: false
        //    },
        //    function (isConfirm) {
        //        if (isConfirm) {
        //            apiService.DeleteURI("SportMasterDivision/MasterDeleteModulesDTO", MdeleteId).
        //            then(function (promise) {
        //                if (promise.msg != "" && promise.msg != null) {
        //                    swal(promise.msg);
        //                    return;
        //                }
        //                if (promise.returnVal == true) {
        //                    swal("Caste Deleted Successfully");
        //                    $state.reload();
        //                }
        //                else {
        //                    swal("Failed to Delete the Caste");
        //                }
        //            })
        //        }
        //        else {
        //            swal("Caste Deletion Cancelled");
        //        }

        //    });
        //}

        // to Edit Data
        $scope.EditSportMasterDivisiondata = function (EditRecord) {
            
            $scope.EditId = EditRecord.spccmD_Id;
            var MEditId = $scope.EditId;
            apiService.getURI("SportMasterDivision/GetSelectedRowDetails/", MEditId).
            then(function (promise) {
                
                $scope.name = promise.gridviewDetails[0].spccmD_DivisionName;
                $scope.description = promise.gridviewDetails[0].spccmD_DivisionDescription;
                $scope.SPCCMD_Id = promise.gridviewDetails[0].spccmD_Id;
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
        $scope.saveSportMasterDivisiondata = function () {
            
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                
                var data = {
                    "SPCCMD_Id": $scope.SPCCMD_Id,
                    "SPCCMD_DivisionName": $scope.name,
                    "SPCCMD_DivisionDescription": $scope.description
                    
                }
                apiService.create("SportMasterDivision/", data).
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
                        else if (promise.duplicate_caste_name_bool == true) {
                            swal("Divison Name Already Exists");
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
            if (newuser1.spccmD_ActiveFlag == false) {

                mgs = "Activate";

            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Divison?",
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
                     apiService.create("SportMasterDivision/deactivate", newuser1).
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
            $scope.SPCCMD_Id = 0;
            $scope.name = "";
            $scope.description = "";
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        }

        $scope.searchValue = "";
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.spccmD_DivisionDescription)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.spccmD_DivisionName)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }

    }

})();