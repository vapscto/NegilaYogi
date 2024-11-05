(function () {
    'use strict';

    angular
        .module('app')
        .controller('MasterCompetitionCategory', MasterCompetitionCategory);

    MasterCompetitionCategory.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];

    function MasterCompetitionCategory($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.searchValue = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        //=====================================Load Data
        $scope.loadgrid = function () {
            apiService.getURI("MasterCompetitionCategory/loadgrid/", 1).then(function (promise) {
                if (promise.count > 0) {
                    $scope.competitionCategoryList = promise.competitionCategoryList;
                    $scope.presentCountgrid = $scope.competitionCategoryList.length;
                }
               
            });
        }
       //========================================================================================

        //======================================Save Data
        $scope.submitted = false;
        $scope.SPCCMCC_CCAgeFlag = false;
        $scope.SPCCMCC_CCWeightFlag = false;
        $scope.saveRecord = function () {
            debugger
           
            if ($scope.myForm.$valid) {
                //if (($scope.SPCCMCC_CCAgeFlag != undefined) || ($scope.SPCCMCC_CCAgeFlag != null) || ($scope.SPCCMCC_CCWeightFlag != undefined) || ($scope.SPCCMCC_CCWeightFlag != null)) {
               
                    //if ($scope.SPCCMCC_CCAgeFlag == false && $scope.SPCCMCC_CCWeightFlag == false) {
                       
                    //        var obj = {
                    //            "SPCCMCC_Id": $scope.SPCCMCC_Id,
                    //            "SPCCMCC_CompitionCategory": $scope.SPCCMCC_CompitionCategory,
                    //            "SPCCMCC_CCDesc": $scope.SPCCMCC_CCDesc
                    //        }
                    //}
                    //else if ($scope.SPCCMCC_CCAgeFlag == true && $scope.SPCCMCC_CCWeightFlag == true) {
                    //    var obj = {
                    //        "SPCCMCC_Id": $scope.SPCCMCC_Id,
                    //        "SPCCMCC_CompitionCategory": $scope.SPCCMCC_CompitionCategory,
                    //        "SPCCMCC_CCDesc": $scope.SPCCMCC_CCDesc,
                    //        "SPCCMCC_CCAgeFlag": $scope.SPCCMCC_CCAgeFlag,
                    //        "SPCCMCC_FromCCAgeYear": $scope.SPCCMCC_FromCCAgeYear,
                    //        "SPCCMCC_FromCCAgeMonth": $scope.SPCCMCC_FromCCAgeMonth,
                    //        "SPCCMCC_FromCCAgeDays": $scope.SPCCMCC_FromCCAgeDays,

                    //        "SPCCMCC_ToCCAgeYear": $scope.SPCCMCC_ToCCAgeYear,
                    //        "SPCCMCC_ToCCAgeMonth": $scope.SPCCMCC_ToCCAgeMonth,
                    //        "SPCCMCC_ToCCAgeDays": $scope.SPCCMCC_ToCCAgeDays,
                    //        "SPCCMCC_CCWeightFlag": $scope.SPCCMCC_CCWeightFlag,
                    //        "SPCCMCC_CCWeight": $scope.SPCCMCC_CCWeight
                    //    }
                    //}
                    //else if ($scope.SPCCMCC_CCWeightFlag == true) {
                    //    var obj = {
                    //        "SPCCMCC_Id": $scope.SPCCMCC_Id,
                    //        "SPCCMCC_CompitionCategory": $scope.SPCCMCC_CompitionCategory,
                    //        "SPCCMCC_CCDesc": $scope.SPCCMCC_CCDesc,
                    //        "SPCCMCC_CCWeightFlag": $scope.SPCCMCC_CCWeightFlag,
                    //        "SPCCMCC_CCWeight": $scope.SPCCMCC_CCWeight

                    //    }
                    //}
                    //else if ($scope.SPCCMCC_CCAgeFlag == true) {
                    //    var obj = {
                    //        "SPCCMCC_Id": $scope.SPCCMCC_Id,
                    //        "SPCCMCC_CompitionCategory": $scope.SPCCMCC_CompitionCategory,
                    //        "SPCCMCC_CCDesc": $scope.SPCCMCC_CCDesc,
                    //        "SPCCMCC_CCAgeFlag": $scope.SPCCMCC_CCAgeFlag,

                    //        "SPCCMCC_FromCCAgeYear": $scope.SPCCMCC_FromCCAgeYear,
                    //        "SPCCMCC_FromCCAgeMonth": $scope.SPCCMCC_FromCCAgeMonth,
                    //        "SPCCMCC_FromCCAgeDays": $scope.SPCCMCC_FromCCAgeDays,

                    //        "SPCCMCC_ToCCAgeYear": $scope.SPCCMCC_ToCCAgeYear,
                    //        "SPCCMCC_ToCCAgeMonth": $scope.SPCCMCC_ToCCAgeMonth,
                    //        "SPCCMCC_ToCCAgeDays": $scope.SPCCMCC_ToCCAgeDays,

                    //    }
                    //}

                if ($scope.SPCCMCC_CCAgeFlag == true) {
                    var obj = {
                        "SPCCMCC_Id": $scope.SPCCMCC_Id,
                        "SPCCMCC_CompitionCategory": $scope.SPCCMCC_CompitionCategory,
                        "SPCCMCC_CCDesc": $scope.SPCCMCC_CCDesc,
                        "SPCCMCC_CCAgeFlag": $scope.SPCCMCC_CCAgeFlag,

                        "SPCCMCC_FromCCAgeYear": $scope.SPCCMCC_FromCCAgeYear,
                        "SPCCMCC_FromCCAgeMonth": $scope.SPCCMCC_FromCCAgeMonth,
                        "SPCCMCC_FromCCAgeDays": $scope.SPCCMCC_FromCCAgeDays,

                        "SPCCMCC_ToCCAgeYear": $scope.SPCCMCC_ToCCAgeYear,
                        "SPCCMCC_ToCCAgeMonth": $scope.SPCCMCC_ToCCAgeMonth,
                        "SPCCMCC_ToCCAgeDays": $scope.SPCCMCC_ToCCAgeDays,

                    }
                }
                else if ($scope.SPCCMCC_CCWeightFlag == true) {
                    var obj = {
                        "SPCCMCC_Id": $scope.SPCCMCC_Id,
                        "SPCCMCC_CompitionCategory": $scope.SPCCMCC_CompitionCategory,
                        "SPCCMCC_CCDesc": $scope.SPCCMCC_CCDesc,
                        "SPCCMCC_CCWeightFlag": $scope.SPCCMCC_CCWeightFlag,
                        "SPCCMCC_CCWeight": $scope.SPCCMCC_CCWeight

                    }
                }
                else {
                    var obj = {
                        "SPCCMCC_Id": $scope.SPCCMCC_Id,
                        "SPCCMCC_CompitionCategory": $scope.SPCCMCC_CompitionCategory,
                        "SPCCMCC_CCDesc": $scope.SPCCMCC_CCDesc,
                        "SPCCMCC_CCAgeFlag": $scope.SPCCMCC_CCAgeFlag,
                        "SPCCMCC_CCWeightFlag": $scope.SPCCMCC_CCWeightFlag,
                    }
                }

                    apiService.create("MasterCompetitionCategory/saveRecord", obj).then(function (promise) {
                        if (promise.returnVal == 'saved') {
                            swal("Record Saved Successfully");
                            $state.reload();
                        }
                        else if (promise.returnVal == 'updated') {
                            swal("Record Updated Successfully");
                            $state.reload();
                        }
                        else if (promise.returnVal == 'duplicate') {
                            swal("Record already exist");
                        }
                        else if (promise.returnVal == "savingFailed") {
                            swal("Failed to save record");
                        }
                        else if (promise.returnVal == "updateFailed") {
                            swal("Failed to update record");
                        }
                        else {
                            swal("Sorry...something went wrong");
                        }
                    });
                //}
                //else {
                //    swal('Please Select Checkbox....');
                //}
            
              
            }
            else {
                $scope.submitted = true;
            }
           
           
        }
       //========================================================================================

        //==============================================For Edit
        $scope.edit = function (data) {
            $scope.SPCCMCC_Id = data;
            apiService.getURI("MasterCompetitionCategory/Edit/", $scope.SPCCMCC_Id).then(function (promise) {
                $scope.editDetails = promise.editDetails;
                $scope.SPCCMCC_CompitionCategory = promise.editDetails[0].spccmcC_CompitionCategory
                $scope.SPCCMCC_CCDesc = promise.editDetails[0].spccmcC_CCDesc;
                $scope.SPCCMCC_CCAgeFlag = promise.editDetails[0].spccmcC_CCAgeFlag
                $scope.SPCCMCC_FromCCAgeYear = promise.editDetails[0].spccmcC_FromCCAgeYear;
                $scope.SPCCMCC_FromCCAgeMonth = promise.editDetails[0].spccmcC_FromCCAgeMonth
                $scope.SPCCMCC_FromCCAgeDays = promise.editDetails[0].spccmcC_FromCCAgeDays;
                $scope.SPCCMCC_CCWeightFlag = promise.editDetails[0].spccmcC_CCWeightFlag
                $scope.SPCCMCC_CCWeight = promise.editDetails[0].spccmcC_CCWeight;
                
                $scope.SPCCMCC_ToCCAgeYear = promise.editDetails[0].spccmcC_ToCCAgeYear;
                $scope.SPCCMCC_ToCCAgeMonth = promise.editDetails[0].spccmcC_ToCCAgeMonth
                $scope.SPCCMCC_ToCCAgeDays = promise.editDetails[0].spccmcC_ToCCAgeDays;
            });
        }
      //========================================================================================

        
        //==============================================Deactivate
        $scope.deactive = function (newuser1, SweetAlertt) {
            debugger;
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            var mgs = "";
            if (newuser1.spccmcC_ActiveFlag == false) {

                mgs = "Activate";

            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " Record?",
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
                        apiService.create("MasterCompetitionCategory/deactivate", newuser1).
                            then(function (promise) {
                                debugger;
                                if (promise.retval == true) {
                                    swal("Record " + mgs + "d Successfully!!!");
                                    $scope.loadgrid();
                                }
                                else {
                                    swal("Record Not " + mgs + "d Successfully!!!");
                                    $scope.loadgrid();
                                }

                            })
                    }
                    else {
                        swal("Cancelled");
                    }
                })
        }
        //================================================================


        $scope.get_fromage = function () {
            debugger;
            if (($scope.SPCCMCC_FromCCAgeYear == 0) || ($scope.SPCCMCC_FromCCAgeYear < 0)) {
                $scope.SPCCMCC_FromCCAgeYear = '';
            }
        }
        $scope.get_toage = function () {
            if (($scope.SPCCMCC_ToCCAgeYear == 0) || ($scope.SPCCMCC_ToCCAgeYear < 0)) {
                $scope.SPCCMCC_ToCCAgeYear = '';
            }            
        }
        $scope.get_frommonth = function () {
            if (($scope.SPCCMCC_FromCCAgeMonth == 0) || ($scope.SPCCMCC_FromCCAgeMonth < 0)) {
                $scope.SPCCMCC_FromCCAgeMonth = '';
            }
        }
        $scope.get_tomonth = function () {
            if (($scope.SPCCMCC_ToCCAgeMonth == 0) || ($scope.SPCCMCC_ToCCAgeMonth < 0)) {
                $scope.SPCCMCC_ToCCAgeMonth = '';
            }
        }
        $scope.get_fromday = function () {
            if (($scope.SPCCMCC_FromCCAgeDays == 0) || ($scope.SPCCMCC_FromCCAgeDays < 0)) {
                $scope.SPCCMCC_FromCCAgeDays = '';
            }
        }
        $scope.get_todays = function () {
            if (($scope.SPCCMCC_ToCCAgeDays == 0) || ($scope.SPCCMCC_ToCCAgeDays < 0)) {
                $scope.SPCCMCC_ToCCAgeDays = '';
            }
        }
        $scope.get_weight = function () {
            if (($scope.SPCCMCC_CCWeight == 0) || ($scope.SPCCMCC_CCWeight < 0)) {
                $scope.SPCCMCC_CCWeight = '';
            }
        }

        $scope.get_change = function () {
            debugger
            var fromage = Number($scope.SPCCMCC_FromCCAgeYear);
            var todage = Number($scope.SPCCMCC_ToCCAgeYear);

            if ((fromage != undefined) || (todage != null)) {
                if ((todage < fromage) || (todage == fromage) ) {
                    $scope.SPCCMCC_ToCCAgeYear = "";
                    swal('To age is always Greater .');
                }
            }
        }

        $scope.get_change2 = function () {
            debugger
            if (($scope.SPCCMCC_ToCCAgeMonth != undefined) || ($scope.SPCCMCC_ToCCAgeMonth != null)) {
                if (Number($scope.SPCCMCC_FromCCAgeMonth) > Number($scope.SPCCMCC_ToCCAgeMonth)) {
                    $scope.SPCCMCC_ToCCAgeMonth = "";
                    swal('To Month is always Greater & Equals to from Month.');
                }
            }
        }

        $scope.get_change4 = function () {
            debugger
            if (($scope.SPCCMCC_ToCCAgeDays != undefined) || ($scope.SPCCMCC_ToCCAgeDays != null)) {
                if (Number($scope.SPCCMCC_FromCCAgeMonth) > Number($scope.SPCCMCC_ToCCAgeDays)) {
                    $scope.SPCCMCC_ToCCAgeDays = "";
                    swal('To day is always Greater & Equals to from day.');
                }
            }
        }


       
        //=============================================================================
        $scope.cancel = function () {
            //$scope.SPCCMCC_Id = 0;
            //$scope.SPCCMCC_CompitionCategory = "";
            //$scope.SPCCMCC_CCDesc = "";
            //$scope.SPCCMCC_CCAgeFlag = "";
            //$scope.SPCCMCC_CCAgeYear = "";
            //$scope.SPCCMCC_CCAgeMonth = 0;
            //$scope.SPCCMCC_CCAgeDays = 0;
            //$scope.SPCCMCC_CCWeightFlag = "";
            //$scope.SPCCMCC_CCWeight = "";
            //$scope.submitted = false;
            //$scope.myForm.$setPristine();
            //$scope.myForm.$setUntouched();
            $state.reload();
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }
})();
