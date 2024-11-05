
(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClgBirthdayController', ClgBirthdayController)

    ClgBirthdayController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache']
    function ClgBirthdayController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache) {

        var paginationformasters;
        
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !=null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;

        $scope.sms = false;
        $scope.email = false;

        //==================================================================================
        $scope.clgloaddata = function () {
            apiService.getDATA("ClgBirthday/").
                then(function (promise) {

                });
        }

        $scope.radiochange = function () {
            var data = {
                "typeflag": $scope.radioption,
            }
            apiService.create("ClgBirthday/radiochange", data).
                then(function (promise) {
                    if (promise.birthdaylist.length > 0) {
                        $scope.birthdaylist = promise.birthdaylist;
                        $scope.presentCountgrid = $scope.birthdaylist.length;
                    }
                    else {
                        swal("No Record Found...!!")
                        $scope.birthdaylist = "";
                    }
                })
        }

        //===========================================Grid Select All 


        $scope.toggleAll = function () {
            
            var toggleStatus = $scope.details1;
            angular.forEach($scope.birthdaylist, function (itm) { itm.Selected = toggleStatus; });
        }

        $scope.optionToggled = function () {

            if ($scope.radioption == "Student" || $scope.radioption == "Alumni") {
                $scope.details1 = $scope.birthdaylist.every(function (itm) { return itm.Selected; })
            }
            else if ($scope.radioption == "Staff") {
                $scope.details2 = $scope.birthdaylist.every(function (itm) { return itm.Selected2; })
            }
          
        }

        //===========================================Send and Clear 
        $scope.sendMsg = function () {
            
            $scope.selectedarray = [];
            if ($scope.radioption == "Student" || $scope.radioption == "Alumni") {
                angular.forEach($scope.birthdaylist, function (bday) {
                    if (bday.Selected) {
                        $scope.selectedarray.push(bday);
                    }
                })
            }
            else {
                angular.forEach($scope.birthdaylist, function (bday) {
                    if (bday.Selected2) {
                        $scope.selectedarray.push(bday);
                    }
                })
            }
            if ($scope.selectedarray.length > 0) {
                var data = {
                    "selectedarray": $scope.selectedarray,
                    "typeflag": $scope.radioption,
                    "SMSFlag": $scope.sms,
                    "EmailFlag": $scope.email,
                }
            }
            else {
                swal("Select Atleast one checkbox....!!")
            }

            apiService.create("ClgBirthday/sendmsg", data).
                then(function (promise) {
                    
                    if (promise.returnval == true) {
                        swal('Sent successfully');
                    }
                    else {
                        swal('Failed to Send, please contact administrator');
                    }
                    $state.reload();
                })
        }

        $scope.cancel = function () {
            $state.reload();
        }


    }

})();