

(function () {
    'use strict';
    angular
        .module('app')
        .controller('UserClassLibraryController', UserClassLibraryController)

    UserClassLibraryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$filter']
    function UserClassLibraryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $filter) {

        $scope.search = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }


        //==================================Page Load
        $scope.loaddata = function () {
            debugger
            var pageid = 2;
            apiService.getURI("UserClassLibrary/getdetails", pageid).then(
                function (promise) {

                    $scope.stafflist = promise.stafflist;
                    $scope.classlist = promise.classlist;

            })
        }
        //===================================================END Load



        //========classlist CheckBox Field Validation===========//
        $scope.isOptionsRequired = function () {
            return !$scope.classlist.some(function (item) {
                return item.selected;
            });
        }


        //=======selection of checkbox....
        $scope.togchkbxC = function () {

            $scope.usercheckC = $scope.classlist.every(function (role) {
                return role.selected;
            });
        }


        //---------all checkbox Select...
        $scope.all_checkC = function (all) {

            $scope.usercheckC = all;
            var toggleStatus = $scope.usercheckC;
            angular.forEach($scope.classlist, function (role) {
                role.selected = toggleStatus;
            });
        }



        //===================================================Save data
        $scope.saverecord = function () {

            if ($scope.myForm.$valid) {
                debugger
                var obj = {
                    "LMBANO_Id": $scope.LMBANO_Id.lmbanO_Id,
                    "LMBANO_LostDamagedDate": $scope.LMBANO_LostDamagedDate,
                    "LMBANO_LostDamagedReason": $scope.LMBANO_LostDamagedReason,
                    "LMBANO_AmountCollected": $scope.LMBANO_AmountCollected,
                    "LMBANO_ModeOfPayment": $scope.LMBANO_ModeOfPayment,
                    "LMBANO_AvialableStatus": $scope.LMBANO_AvialableStatus,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("LostBook/saverecord", obj).then(function (promise) {
                    if (promise.returnval == true) {
                        swal('Record Saved Successfully!');
                    }
                    else {
                        swal('Record Not Saved Successfully!');
                    }
                    $state.reload();
                });
            } else {
                $scope.submitted = true;
            }
        }
        //===================================================END

        

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        //-----------clear-field
        $scope.clear = function () {
            $state.reload();
        }



    }
})();

