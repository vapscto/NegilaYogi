(function () {
    'use strict';

    angular
        .module('app')
        .controller('MasterSponser', MasterSponser);

    MasterSponser.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];

    function MasterSponser($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.searchValue = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }


        //===========================================Bind Data
        $scope.loadgrid = function () {
            apiService.getURI("MasterSponser/loadgrid/", 1).then(function (promise) {
                if (promise.count > 0) {
                    $scope.sponserList = promise.sponserList;
                    $scope.presentCountgrid = $scope.sponserList.length;
                }
                $scope.cancel();
            });
        }


        ///=============================================Saving Records..
        $scope.submitted = false;
        $scope.saveRecord = function () {
            if ($scope.myForm.$valid) {
                var obj = {
                    "SPCCMSP_Id": $scope.spccmsP_Id,
                    "SPCCMSP_SponsorName": $scope.SPCCMSP_SponsorName,
                    "SPCCMSP_ContactPerson": $scope.SPCCMSP_ContactPerson,
                    "SPCCMSP_ContactNo": $scope.SPCCMSP_ContactNo,
                    "SPCCMSP_SponsorDetails": $scope.SPCCMSP_SponsorDetails,
                }
                apiService.create("MasterSponser/saveRecord", obj).then(function (promise) {
                    if (promise.returnVal == 'saved') {
                        swal("Record Saved Successfully");
                        $scope.loadgrid();
                    }
                    else if (promise.returnVal == 'updated') {
                        swal("Record Updated Successfully");
                        $scope.loadgrid();
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
            }
            else {
                $scope.submitted = true;
            }
        }
   ////===================================================================End



        ///==========================================edit Records
        $scope.edit = function (data) {
            
            $scope.spccmsP_Id = data;
            apiService.getURI("MasterSponser/EditSponser", $scope.spccmsP_Id).then(function (promise) {
                $scope.editDetails = promise.editDetails;
                $scope.SPCCMSP_SponsorName = promise.editDetails[0].spccmsP_SponsorName
                $scope.SPCCMSP_ContactPerson = promise.editDetails[0].spccmsP_ContactPerson;
                $scope.SPCCMSP_ContactNo = promise.editDetails[0].spccmsP_ContactNo;
                $scope.SPCCMSP_SponsorDetails = promise.editDetails[0].spccmsP_SponsorDetails;
            });
        }


        //=================Activation/Deactivation--Record.........
        $scope.deactive = function (user, SweetAlert) {
            debugger;
            $scope.spccmsP_Id = user.spccmsP_Id;

            var dystring = "";
            if (user.spccmsP_ActiveFlag == 1) {
                dystring = "Deactivate";
            }
            else if (user.spccmsP_ActiveFlag == 0) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("MasterSponser/deactivateSponser", user).
                            then(function (promise) {
                                debugger;
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!");
                                }
                                $scope.loadgrid();
                            })
                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }

                });
        }
        //================================================================End..



        

        ////////============================================
        $scope.cancel = function () {
            $scope.spccmsP_Id = 0;
            $scope.SPCCMSP_SponsorName = "";
            $scope.SPCCMSP_ContactPerson = "";
            $scope.SPCCMSP_ContactNo = "";
            $scope.SPCCMSP_SponsorDetails = "";
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        /////===========================================Filter 
        $scope.searchValue = "";
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.spccmsP_SponsorName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.spccmsP_ContactPerson)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.spccmsP_SponsorDetails)).indexOf(angular.lowercase($scope.searchValue)) >= 0 
            

        }

    }
})();
