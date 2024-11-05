(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClgPushNotificationController', ClgPushNotificationController)

    ClgPushNotificationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$q', '$http', '$filter', 'superCache', '$window']
    function ClgPushNotificationController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $q, $http, $filter, superCache, $window) {

        $scope.ipN_Date = new Date();

        //====================LOAD Data
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            apiService.getDATA("ClgPushNotification/getloaddata").
                then(function (promise) {
                    $scope.roletype = promise.roletype;
                    $scope.roleflag = $scope.roletype[0].ivrmrT_Role;
                    if ($scope.roleflag == "Staff") {
                        $scope.studentlist = promise.namelist;
                        $scope.notificationlist = promise.notificationlist;
                        $scope.presentCountgrid = $scope.notificationlist.length;
                    }
                    else {
                        $scope.employeelist = promise.namelist;
                    }
                })
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        }

        //=========================Student Name All Check
        $scope.toggleAll = function () {
            angular.forEach($scope.studentlist, function (st) {
                st.selected = $scope.all;
            })
            $scope.onbranchchange();
        };
        $scope.togchkbx = function () {
            $scope.all = $scope.studentlist.every(function (stu) {
                return stu.selected;
            });
        }

        //========================= Save Notification
        $scope.submitted = false;
        $scope.savedata = function () {

            if ($scope.myForm.$valid) {
                if ($scope.roleflag == "Staff") {
                    $scope.studentarray = [];
                    angular.forEach($scope.studentlist, function (st) {
                        if (st.selected == true) {
                            $scope.studentarray.push(st);
                        }
                    });
                }
                var pushdate = $scope.ipN_Date == null ? "" : $filter('date')($scope.ipN_Date, "yyyy-MM-dd");
                if ($scope.roleflag == "Student") {
                    var data = {
                        "IPN_Id": $scope.ipN_Id,
                        "IPN_No": $scope.ipN_No,
                        "IPN_Date": pushdate,
                        "IPN_PushNotification": $scope.ipN_PushNotification,
                        "IPN_StuStaffFlg": $scope.roleflag,
                        "HRME_Id": $scope.hrmE_Id
                    }
                }
                else if ($scope.roleflag == "Staff") {
                    var data = {
                        "IPN_Id": $scope.ipN_Id,
                        "IPN_No": $scope.ipN_No,
                        "IPN_Date": pushdate,
                        "IPN_PushNotification": $scope.ipN_PushNotification,
                        "IPN_StuStaffFlg": $scope.roleflag,
                        "studentarray": $scope.studentarray
                    }
                }

                apiService.create("ClgPushNotification/savedata", data).
                    then(function (promise) {

                        if (promise.returnval == true) {
                            if (promise.ipN_Id == 0 || promise.ipN_Id < 0) {
                                swal('Notification sent successfully');
                            }
                        }
                        else {
                            if (promise.ipN_Id == 0 || promise.ipN_Id < 0) {
                                swal('Failed to send, please contact administrator');
                            }
                            else if (promise.ipN_Id > 0) {
                                swal('Failed to update, please contact administrator');
                            }
                        }
                        $state.reload();

                    });


            }
            else {
                $scope.submitted = true;
            }

        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.notificationmodel = function (id) {
            var data = {
                "IPN_Id": id,
            }
            apiService.create("ClgPushNotification/getNotificationdetails", data).
                then(function (promise) {
                    $scope.notificationdetails = promise.notificationdetails;
                    $scope.notification = $scope.notificationdetails[0].ipN_No;
                })
        }



    };
})();

