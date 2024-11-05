
(function () {
    'use strict';
    angular
        .module('app')
        .controller('NaacMarksSlabController', NaacMarksSlabController)

    NaacMarksSlabController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache']
    function NaacMarksSlabController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache) {

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.sortReverse = true;
        $scope.searchValue = "";

        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("NaacMarksSlab/Getdetails", pageid).
                then(function (promise) {

                    $scope.criterialist = promise.criterialist;
                    $scope.griddata = promise.griddata;

                })
        };
        
        // to Edit Data
        $scope.editrecord = function (user) {
            debugger;
            var data = {
                "NCACCRMRSLB_Id": user.ncaccrmrslB_Id,               
            }

            apiService.create("NaacMarksSlab/editdata", data).
                then(function (promise) {
                
                    $scope.ncaccrmrslB_Id = promise.editlist[0].ncaccrmrslB_Id;
                    $scope.NCACCRMRSLB_ToSlab = promise.editlist[0].ncaccrmrslB_ToSlab;
                    $scope.NCACCRMRSLB_FromSlab = promise.editlist[0].ncaccrmrslB_FromSlab;
                    $scope.NCACCRMRSLB_Marks = promise.editlist[0].ncaccrmrslB_Marks;
                    $scope.naacsL_Id = promise.editlist[0];
                    $scope.naacsL_SLNo = promise.editlist[0].naacsL_SLNo;
                })
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        //================ TO Save The Data
        $scope.submitted = false;
        $scope.saverecord = function () {          
            if ($scope.myForm.$valid) {
                var data = {
                    "NCACCRMRSlB_Id": $scope.ncaccrmrslB_Id,
                    "NCACCRMRSLB_ToSlab": $scope.NCACCRMRSLB_ToSlab,
                    "NCACCRMRSLB_FromSlab": $scope.NCACCRMRSLB_FromSlab,
                    "NCACCRMRSLB_Marks": $scope.NCACCRMRSLB_Marks,
                    "NAACSL_Id": $scope.naacsL_Id.naacsL_Id,                 

                }
                apiService.create("NaacMarksSlab/savedata", data).
                    then(function (promise) {
                        if (promise.returnval != null && promise.duplicate != null) {
                            if (promise.duplicate == false) {
                                if (promise.returnval == true) {
                                    if ($scope.ncaccrmrslB_Id > 0) {
                                        swal('Record Updated Successfully!!!');
                                    }
                                    else {
                                        swal('Record Saved Successfully!!!');
                                    }
                                }
                                else {
                                    if (promise.returnval == false) {
                                        if ($scope.ncaccrmrslB_Id > 0) {
                                            swal('Record Not Update Successfully!!!');
                                        }
                                        else {
                                            swal('Record Not Saved Successfully!!!');
                                        }
                                    }
                                }
                            }
                            else {
                                swal("Record already exist");
                            }
                            $state.reload();
                        }
                        else {
                            swal("Kindly Contact Administrator!!!");
                        }
                    })
            }
            else
            {
                $scope.submitted = true;
            }
        };

        /////==========================deactive and active
        $scope.deactive = function (newuser1, SweetAlertt) {
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            var mgs = "";
            if (newuser1.ncaccrmrslB_ActiveFlg == false) {
                mgs = "Activate";
            }
            else
            {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the House Committee?",
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
                        apiService.create("NaacMarksSlab/deactive", newuser1).
                            then(function (promise) {

                                if (promise.returnval == true) {
                                    swal("Record " + mgs + "d Successfully!!!");
                                    $state.reload();
                                }
                                else {
                                    swal("Record Not " + mgs + "d Successfully!!!");
                                    $state.reload();
                                }
                            })
                    }
                    else {
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
            $state.reload();
        }

        

    }

})();