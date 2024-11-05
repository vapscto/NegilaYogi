

(function () {
    'use strict';
    angular
        .module('app')
        .controller('LostBookController', LostBookController)

    LostBookController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$filter']
    function LostBookController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $filter) {

        $scope.search = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }


        $scope.booktype = 'issue';
        $scope.bookornonbook = 'book';
        //==================================Page Load
        $scope.Loaddata = function () {
            debugger

            var data = {
                "booktype": $scope.booktype,
                "bookcat_type": $scope.bookornonbook,
            }
            
            var pageid = 2;
            apiService.create("LostBook/getdetails", data).then(function (promise) {
                $scope.booktitle = promise.booktitle;
                $scope.lostbooks = promise.lostbooks;

            })
        }
        //===================================================END Load


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

        //=====================Start Book name Search ==================//
        $scope.searchfilter = function (objj) {

            if (objj.search.length >= '2') {

                var data = {
                    "searchfilter": objj.search,
                    "booktype": $scope.booktype,
                   // "issuertype": $scope.issuertype1,
                    "bookcat_type": $scope.bookornonbook,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("LostBook/searchfilter", data).
                    then(function (promise) {

                        $scope.booktitle = promise.booktitle;

                        angular.forEach($scope.booktitle, function (objectt) {
                            if (objectt.amsT_FirstName.length > 0) {
                                var string = objectt.amsT_FirstName;
                                objectt.amsT_FirstName = string.replace(/  +/g, ' ');
                            }
                        })
                    })
            }

        }
        ////==========================================================End


        //====================================Get Author Name
        $scope.get_authorNm = function (data) {
            debugger;
            var obj = {
                "LMB_Id": data.lmB_Id,
                "LMBANO_Id": data.lmbanO_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("LostBook/get_authorNm", obj).
                then(function (promise) {

                    $scope.authorlist = promise.authorlist;

                })
        }
        //==========================================================End


        ///======================================Get Data Based On Radio button Change
        $scope.get_radiochange = function () {
            debugger

            var data = {
                "booktype": $scope.booktype,
                "bookcat_type": $scope.bookornonbook,
            }

            debugger;
            var pageid = 2;
            apiService.create("LostBook/get_radiochange", data).then(function (promise) {

                $scope.booktitle = promise.booktitle;
           
            })
        }
        

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        //-----------clear-field
        $scope.Clearid = function () {
            $state.reload();
        }



    }
})();

