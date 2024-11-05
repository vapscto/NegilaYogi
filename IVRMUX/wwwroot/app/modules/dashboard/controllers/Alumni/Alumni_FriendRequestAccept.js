(function () {
    'use strict';
    angular
        .module('app')
        .controller('AlumnifriendrequestacceptController', AlumnifriendrequestacceptController)
    AlumnifriendrequestacceptController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout', '$filter']
    function AlumnifriendrequestacceptController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, $filter) {

        $scope.searchValue = '';
        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = 10;
        $scope.loaddata = function () {
            var id = 1;
            apiService.getURI("Alumni_Student_FriendRequest/getdata_request", id).then(function (promise) {
                if (promise.friendrequestlist != null || promise.friendrequestlist > 0) {
                    $scope.friendrequestlist = [];
                    $scope.friendrequestlist1 = [];
                    $scope.friendrequestlist1 = promise.friendrequestlist;
                    angular.forEach($scope.friendrequestlist1, function (qq) { 
                        if (qq.ALSFRND_AcceptedDate == null) {
                            $scope.friendrequestlist.push({
                                studentname: qq.studentname,
                                ASMAY_Year: qq.ASMAY_Year,
                                ALSFRND_AcceptFlag: qq.ALSFRND_AcceptFlag,
                                checksts: false,
                                ALMST_Id: qq.ALMST_Id,
                                 ALSFRND_RequestDate: qq.ALSFRND_RequestDate,
                                ALSFRND_AcceptedDate: qq.ALSFRND_AcceptedDate
                            })
                        }
                        else {
                            $scope.friendrequestlist.push({
                                studentname: qq.studentname,
                                ASMAY_Year: qq.ASMAY_Year,
                                ALSFRND_AcceptFlag: qq.ALSFRND_AcceptFlag,
                                checksts: true,
                                ALMST_Id: qq.ALMST_Id,
                                ALSFRND_RequestDate: qq.ALSFRND_RequestDate,
                                ALSFRND_AcceptedDate: qq.ALSFRND_AcceptedDate
                            })
                        }
                    })
                }
            })
        }

        $scope.imgname = logopath;

        $scope.Clearid = function () {
            $state.reload();
            // $scope.radioValue = "";
            // $scope.items = "";
        }
        $scope.printstudents = [];
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.friendrequestlist, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                    }
                }
                else {
                    $scope.printstudents.splice(itm);
                }
            });
        }

        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.all = $scope.friendrequestlist.every(function (itm) { return itm.selected; });
            if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
                $scope.printstudents.push(SelectedStudentRecord);
            }
            else {
                $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            }

        }

        $scope.sendrequest = function () {
            $scope.printstudents1 = [];
            angular.forEach($scope.printstudents, function (aa) {
                $scope.printstudents1.push({ ALMST_Id: aa.ALMST_Id, ALSFRND_RequestDate: aa.ALSFRND_RequestDate })
            })

            var data = {
                "requestalumniaccept": $scope.printstudents1
            }
            apiService.create("Alumni_Student_FriendRequest/FriendrequestAccept", data).then(function (promise) {
                if (promise.message == 'Success') {
                    swal('Friend Request Accept Successfully...')
                }
                else {
                    swal('Request is Canceled..!!')
                }
                $state.reload();
            })
        }

        $scope.cancelquest = function () {
            $scope.printstudents1 = [];
            angular.forEach($scope.printstudents, function (aa) {
                $scope.printstudents1.push({ ALMST_Id: aa.ALMST_Id, ALSFRND_RequestDate: aa.ALSFRND_RequestDate })
            })

            var data = {
                "requestalumniaccept": $scope.printstudents1,
                "Cancel": "Cancel"
            }
            apiService.create("Alumni_Student_FriendRequest/FriendrequestAccept", data).then(function (promise) {
                if (promise.message == 'Success') {
                    swal('Friend Request Accept Successfully...')
                }
                else {
                    swal('Request is Canceled..!!')
                }
                $state.reload();
            })
        }

        $scope.viewData = function (ww) {
            var data = {
                "ALMST_Id": ww.ALMST_Id
            }
            apiService.create("Alumni_Student_FriendRequest/viewprofile", data).then(function (promise) {
                if (promise.almst_profile != null || promise.almst_profile > 0) {
                    $scope.alumniname1 = promise.almst_profile[0].alumniname;
                    $scope.almst_profile = promise.almst_profile;

                    $('#profileview').modal('show');
                }
                else {
                    swal('No Data Found..!!!')
                }

            })

        }

        $scope.previewimg = function (img) {
            if (img != 'undefined' || img != null) {
                $('#preview').attr('src', img);
                $('#myimagePreview').modal('show');
            }
            else {
                swal('No Image Found..!!')
            }

        };
    }
})
    ();