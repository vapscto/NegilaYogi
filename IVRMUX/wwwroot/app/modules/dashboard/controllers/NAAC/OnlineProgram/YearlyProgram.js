(function () {
    'use strict';
    angular.module('app').controller('YearlyProgramController', YearlyProgramController)
    YearlyProgramController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', '$sce']
    function YearlyProgramController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, $sce) {
        //$rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, $sce

        var paginationformasters;
        $scope.studentGuardianDetails = {};
        $scope.Participants = {};
        $scope.Account = {};
        $scope.Invitation = {};
        $scope.Winner = {};
        $scope.Speech = {};
        $scope.Profile = {};
        $scope.ProgramDetails = {};
        $scope.Programgst = {};
        $scope.guestdetails = {};
        $scope.studentGuardianDetails1 = [];
        var imagedownload = "";
        var studentreg = "";
        var docname = "";

        /* loading start*/
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            var pageid = 2;
            apiService.getURI("YearlyProgram/getloaddata", pageid).then(function (promise) {
                $scope.fillyear = promise.fillyear;
                $scope.programlist = promise.programlist;
                $scope.gridOptions.data = promise.programlist;
                $scope.Typelist = promise.typelist;
                $scope.levellist = promise.levellist;
            });
        };
        /* loading end*/

        $scope.editguest = function (guestdetailsgst) {
            var data = {
                "PRYRG_Id": guestdetailsgst.pryrG_Id
            };

            apiService.create("YearlyProgram/editguest", data).then(function (promise) {
                if (promise.listg != null) {
                    $scope.emailid = promise.listg[0].pryrG_GuestEmailId;
                    $scope.gtype = promise.listg[0].pryrG_GuestType;
                    $scope.gaddress = promise.listg[0].pryrG_GuestAddress;
                    $scope.gno = promise.listg[0].pryrG_GuestPhoneNo;
                    $scope.GuestName = promise.listg[0].pryrG_GuestName;
                    $scope.Profile = promise.uploadfiles77;
                    $scope.Speech = promise.uploadfiles88;
                    $scope.Programgst = promise.uploadfiles99;
                    $scope.PRYRG_Id = promise.listg[0].pryrG_Id;

                    $scope.Programgst = promise.uploadfiles99;
                    if ($scope.Programgst !== null && $scope.Programgst.length > 0) {
                        angular.forEach($scope.Programgst, function (dd) {
                            var img = dd.lpmtR_ResourceType;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            dd.lpmtR_Resources = dd.lpmtR_ResourceType;
                            dd.lpmtR_FileName = dd.lpmtR_FileName;
                            dd.file_name = dd.lpmtR_FileName;
                            dd.filename = dd.lpmtR_FileName;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls' || lastelement === 'png' || lastelement === 'jpeg' || lastelement === 'jpg') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.lpmtR_ResourceType;
                            }
                        });
                    }
                    else {
                        $scope.Programgst = [{ id: 'guest1' }];
                    }

                    $scope.Profile = promise.uploadfiles77;
                    if ($scope.Profile !== null && $scope.Profile.length > 0) {
                        angular.forEach($scope.Profile, function (dd) {
                            var img = dd.lpmtR_ResourceType;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            dd.lpmtR_ResourceType = dd.lpmtR_ResourceType;
                            dd.lpmtR_Resources = dd.lpmtR_ResourceType;
                            dd.lpmtR_FileName = dd.lpmtR_FileName;
                            dd.file_name = dd.lpmtR_FileName;
                            dd.filename = dd.lpmtR_FileName;

                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls' || lastelement === 'png' || lastelement === 'jpeg' || lastelement === 'jpg') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.lpmtR_ResourceType;
                            }
                            // $('#popup15').modal('show');
                        });
                    }
                    else {
                        $scope.Profile = [{ id: 'Guardian7' }];
                    }

                    $scope.Speech = promise.uploadfiles88;
                    if ($scope.Speech !== null && $scope.Speech.length > 0) {
                        angular.forEach($scope.Speech, function (dd) {
                            if (dd.lpmtR_ResourceType !== null && dd.lpmtR_ResourceType !== "") {
                                var img = dd.lpmtR_ResourceType;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                dd.lpmtR_ResourceType = dd.lpmtR_ResourceType;
                                dd.lpmtR_Resources = dd.lpmtR_ResourceType;
                                dd.lpmtR_FileName = dd.lpmtR_FileName;
                                dd.file_name = dd.lpmtR_FileName;
                                dd.filename = dd.lpmtR_FileName;

                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls' || lastelement === 'png' || lastelement === 'jpeg' || lastelement === 'jpg') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.lpmtR_ResourceType;
                                }
                            }
                            // $('#popup15').modal('show');
                        });
                    }
                    else {
                        $scope.Speech = [{ id: 'Guardian6' }];
                    }
                }
            });
        };

        $scope.onselectyear = function () {
            angular.forEach($scope.fillyear, function (yea) {
                if (parseInt(yea.asmaY_Id) === parseInt($scope.ASMAY_Id)) {
                    $scope.from_date = new Date(yea.asmaY_From_Date);
                    $scope.to_date = new Date(yea.asmaY_To_Date);
                }
            });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            resizable: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'asmaY_Year', width: '100', displayName: 'Academic Year' },
                { name: 'programname', width: '100', displayName: 'Program Name' },
                { name: 'club', width: '100', displayName: 'Start Date' },
                { name: 'org_name', width: '100', displayName: 'To Date' },
                { name: 'start_time', width: '100', displayName: 'Start Time' },
                { name: 'end_time', width: '100', displayName: 'To Time' },
                {
                    name: 'Program Chart', width: '100', displayName: 'Chart', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="grid-action-cell">' +
                        '<a   href="javascript:void(0)" data-toggle="modal" data-target="#popup11" data-backdrop="static" ng-click="grid.appScope.viewteacherguides(row.entity);"> <i class="fa fa-eye text-blue" ></i></a>  &nbsp; &nbsp;' + '</div>'
                },
                {
                    name: 'Program Invitation', width: '100', displayName: 'Invitation', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="grid-action-cell">' +
                        '<a   href="javascript:void(0)" data-toggle="modal" data-target="#popup11" data-backdrop="static" ng-click="grid.appScope.viewInvitation(row.entity);"> <i class="fa fa-eye text-blue" ></i></a>  &nbsp; &nbsp;' + '</div>'
                },
                {
                    name: 'Participant List', width: '100', displayName: 'Participant', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="grid-action-cell">' +
                        '<a   href="javascript:void(0)" data-toggle="modal" data-target="#popup11" data-backdrop="static" ng-click="grid.appScope.viewList(row.entity);"> <i class="fa fa-eye text-blue" ></i></a>  &nbsp; &nbsp;' + '</div>'
                },
                {
                    name: 'Account Statement', width: '100', displayName: 'Account', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="grid-action-cell">' +
                        '<a   href="javascript:void(0)" data-toggle="modal" data-target="#popup11" data-backdrop="static" ng-click="grid.appScope.viewAccount(row.entity);"> <i class="fa fa-eye text-blue" ></i></a>  &nbsp; &nbsp;' + '</div>'
                },
                {
                    name: 'Winner List', width: '100', displayName: 'Winner', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="grid-action-cell">' +
                        '<a   href="javascript:void(0)" data-toggle="modal" data-target="#popup11" data-backdrop="static" ng-click="grid.appScope.viewWinner(row.entity);"> <i class="fa fa-eye text-blue" ></i></a>  &nbsp; &nbsp;' + '</div>'
                },
                {
                    name: 'Program Details', width: '100', displayName: 'Details', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="grid-action-cell">' +
                        '<a   href="javascript:void(0)" data-toggle="modal" data-target="#popup11" data-backdrop="static" ng-click="grid.appScope.viewDetails(row.entity);"> <i class="fa fa-eye text-blue" ></i></a>  &nbsp; &nbsp;' + '</div>'
                },
                {
                    name: 'Guest Profile', width: '100', displayName: 'Guest Profile', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="grid-action-cell">' +
                        '<a   href="javascript:void(0)" data-toggle="modal" data-target="#popup11" data-backdrop="static" ng-click="grid.appScope.viewProfile(row.entity);"> <i class="fa fa-eye text-blue" ></i></a>  &nbsp; &nbsp;' + '</div>'
                },
                {
                    name: 'Guest Speech', width: '100', displayName: 'Guest Speech', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="grid-action-cell">' +
                        '<a   href="javascript:void(0)" data-toggle="modal" data-target="#popup11" data-backdrop="static" ng-click="grid.appScope.viewSpeech(row.entity);"> <i class="fa fa-eye text-blue" ></i></a>  &nbsp; &nbsp;' + '</div>'
                },
                {
                    name: 'Guest Photo/Video', width: '100', displayName: 'Guest Photo/Video', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="grid-action-cell">' +
                        '<a   href="javascript:void(0)" data-toggle="modal" data-target="#popup11" data-backdrop="static" ng-click="grid.appScope.viewGuest(row.entity);"> <i class="fa fa-eye text-blue" ></i></a>  &nbsp; &nbsp;' + '</div>'
                },
                {
                    field: 'id', name: '', width: '100',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <md-tooltip md-direction="down">Edit</md-tooltip><i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                        '<a href="javascript:void(0)" style="color:red;" ng-click="grid.appScope.delete(row.entity);"> <md-tooltip md-direction="down">Delete Now</md-tooltip> <i class="fa fa-trash"></i></a>' +
                        '</div>'
                }
            ],

        };

        $scope.deactive = function (employee) {
            $scope.editEmployee = employee.pryR_Id;
            //var pageid = $scope.editEmployee;

            var data = {
                "PRYR_Id": $scope.editEmployee,
            };


            swal({
                title: "Are you sure?",
                text: "Do You Want To delete the Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("YearlyProgram/deactivate", data).then(function (promise) {
                            if (promise.returnval === "true") {
                                swal("Record " + confirmmgs + " Successfully");
                            }
                            else if (promise.message === "used") {
                                swal("Record already Used");
                            }
                            else {
                                swal("Record " + confirmmgs + " Successfully");
                            }

                            $state.reload();
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };

        $scope.viewteacherguides = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            var data = {
                "description": 'chart',
                "PRYR_Id": obj.pryR_Id,
                "ASMAY_Id": obj.asmaY_Id
            };

            apiService.create("YearlyProgram/viewuploadflies", data).then(function (promise) {
                if (promise !== null) {
                    $scope.uploadfilesdetails = promise.uploadfiles;
                    if ($scope.uploadfilesdetails !== null && $scope.uploadfilesdetails.length > 0) {
                        $scope.uploaddocfiles = promise.uploadfiles;
                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            if (dd.lpmtR_ResourceType !== null && dd.lpmtR_ResourceType !== "") {
                                var img = dd.lpmtR_ResourceType;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                dd.fileName = dd.lpmtR_FileName;
                                dd.filepath = dd.lpmtR_ResourceType;
                                if (lastelement === 'doc' || lastelement === 'xlsx' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'image/png' || lastelement === 'image/jpeg' || lastelement === 'image/jpg' || lastelement === 'video/mp4') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.lpmtR_ResourceType;
                                }
                            }
                        });
                    } else {
                        $('#popup11').modal('hide');
                        swal("No Documents Found");
                    }
                }
            });
        };

        $scope.viewInvitation = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            var data = {
                "description": 'invitation',
                "PRYR_Id": obj.pryR_Id,
                "ASMAY_Id": obj.asmaY_Id
            };

            apiService.create("YearlyProgram/viewuploadflies", data).then(function (promise) {
                if (promise !== null) {
                    $scope.uploadfilesdetails = promise.uploadfiles;
                    if ($scope.uploadfilesdetails !== null && $scope.uploadfilesdetails.length > 0) {
                        $scope.uploaddocfiles = promise.uploadfiles;

                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            if (dd.lpmtR_ResourceType !== null && dd.lpmtR_ResourceType !== "") {
                                var img = dd.lpmtR_ResourceType;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                dd.fileName = dd.lpmtR_FileName;
                                dd.filepath = dd.lpmtR_ResourceType;
                                if (lastelement === 'doc' || lastelement === 'xlsx' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'image/png' || lastelement === 'image/jpeg' || lastelement === 'image/jpg' || lastelement === 'video/mp4') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.lpmtR_ResourceType;
                                }
                            }
                        });
                    } else {
                        $('#popup11').modal('hide');
                        swal("No Documents Found");
                    }
                }
            });
        };

        $scope.viewList = function (obj) {

            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            var data = {
                "description": 'list',
                "PRYR_Id": obj.pryR_Id,
                "ASMAY_Id": obj.asmaY_Id
            };

            apiService.create("YearlyProgram/viewuploadflies", data).then(function (promise) {

                if (promise !== null) {
                    $scope.uploadfilesdetails = promise.uploadfiles;
                    if ($scope.uploadfilesdetails !== null && $scope.uploadfilesdetails.length > 0) {
                        $scope.uploaddocfiles = promise.uploadfiles;

                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            if (dd.lpmtR_ResourceType !== null && dd.lpmtR_ResourceType !== "") {
                                var img = dd.lpmtR_ResourceType;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                dd.fileName = dd.lpmtR_FileName;
                                dd.filepath = dd.lpmtR_ResourceType;
                                if (lastelement === 'doc' || lastelement === 'xlsx' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'image/png' || lastelement === 'image/jpeg' || lastelement === 'image/jpg' || lastelement === 'video/mp4') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.lpmtR_ResourceType;
                                }
                            }
                        });
                    } else {
                        $('#popup11').modal('hide');
                        swal("No Documents Found");
                    }
                }
            });
        };

        $scope.viewAccount = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            var data = {
                "description": 'account',
                "PRYR_Id": obj.pryR_Id,
                "ASMAY_Id": obj.asmaY_Id
            };

            apiService.create("YearlyProgram/viewuploadflies", data).then(function (promise) {
                if (promise !== null) {
                    $scope.uploadfilesdetails = promise.uploadfiles;
                    if ($scope.uploadfilesdetails !== null && $scope.uploadfilesdetails.length > 0) {
                        $scope.uploaddocfiles = promise.uploadfiles;

                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            if (dd.lpmtR_ResourceType !== null && dd.lpmtR_ResourceType !== "") {
                                var img = dd.lpmtR_ResourceType;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                dd.fileName = dd.lpmtR_FileName;
                                dd.filepath = dd.lpmtR_ResourceType;
                                if (lastelement === 'doc' || lastelement === 'xlsx' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'image/png' || lastelement === 'image/jpeg' || lastelement === 'image/jpg' || lastelement === 'video/mp4') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.lpmtR_ResourceType;
                                }
                            }
                        });
                    } else {
                        $('#popup11').modal('hide');
                        swal("No Documents Found");
                    }
                }
            });
        };

        $scope.viewWinner = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            var data = {
                "description": 'winner',
                "PRYR_Id": obj.pryR_Id,
                "ASMAY_Id": obj.asmaY_Id
            };

            apiService.create("YearlyProgram/viewuploadflies", data).then(function (promise) {
                if (promise !== null) {

                    $scope.uploadfilesdetails = promise.uploadfiles;
                    if ($scope.uploadfilesdetails !== null && $scope.uploadfilesdetails.length > 0) {
                        $scope.uploaddocfiles = promise.uploadfiles;
                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            if (dd.lpmtR_ResourceType !== null && dd.lpmtR_ResourceType !== "") {
                                var img = dd.lpmtR_ResourceType;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                dd.fileName = dd.lpmtR_FileName;
                                dd.filepath = dd.lpmtR_ResourceType;
                                if (lastelement === 'doc' || lastelement === 'xlsx' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'image/png' || lastelement === 'image/jpeg' || lastelement === 'image/jpg' || lastelement === 'video/mp4') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.lpmtR_ResourceType;
                                }
                            }
                        });
                    } else {
                        $('#popup11').modal('hide');
                        swal("No Documents Found");
                    }
                }
            });
        };

        $scope.viewDetails = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            var data = {
                "description": 'programphoto',
                "PRYR_Id": obj.pryR_Id,
                "ASMAY_Id": obj.asmaY_Id
            };

            apiService.create("YearlyProgram/viewuploadflies", data).then(function (promise) {
                if (promise !== null) {
                    $scope.uploadfilesdetails = promise.uploadfiles;
                    if ($scope.uploadfilesdetails !== null && $scope.uploadfilesdetails.length > 0) {
                        $scope.uploaddocfiles = promise.uploadfiles;

                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            if (dd.lpmtR_ResourceType !== null && dd.lpmtR_ResourceType !== "") {
                                var img = dd.lpmtR_ResourceType;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                dd.fileName = dd.lpmtR_FileName;
                                dd.filepath = dd.lpmtR_ResourceType;
                                if (lastelement === 'doc' || lastelement === 'xlsx' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'image/png' || lastelement === 'image/jpeg' || lastelement === 'image/jpg' || lastelement === 'video/mp4') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.lpmtR_ResourceType;
                                }
                            }
                        });
                    } else {
                        $('#popup11').modal('hide');
                        swal("No Documents Found");
                    }
                }
            });
        };

        $scope.viewProfile = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            var data = {
                "description": 'gprofile',
                "PRYR_Id": obj.pryR_Id,
                "ASMAY_Id": obj.asmaY_Id
            };

            apiService.create("YearlyProgram/viewuploadflies", data).then(function (promise) {
                if (promise !== null) {
                    $scope.uploadfilesdetails = promise.uploadfiles;
                    if ($scope.uploadfilesdetails !== null && $scope.uploadfilesdetails.length > 0) {
                        $scope.uploaddocfiles = promise.uploadfiles;
                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            if (dd.lpmtR_ResourceType !== null && dd.lpmtR_ResourceType !== "") {
                                var img = dd.lpmtR_ResourceType;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                dd.fileName = dd.lpmtR_FileName;
                                dd.filepath = dd.lpmtR_ResourceType;
                                if (lastelement === 'doc' || lastelement === 'xlsx' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'image/png' || lastelement === 'image/jpeg' || lastelement === 'image/jpg' || lastelement === 'video/mp4') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.lpmtR_ResourceType;
                                }
                            }
                        });
                    } else {
                        $('#popup11').modal('hide');
                        swal("No Documents Found");
                    }
                }
            });
        };

        $scope.viewSpeech = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            var data = {
                "description": 'gspeech',
                "PRYR_Id": obj.pryR_Id,
                "ASMAY_Id": obj.asmaY_Id
            };

            apiService.create("YearlyProgram/viewuploadflies", data).then(function (promise) {
                if (promise !== null) {
                    $scope.uploadfilesdetails = promise.uploadfiles;
                    if ($scope.uploadfilesdetails !== null && $scope.uploadfilesdetails.length > 0) {
                        $scope.uploaddocfiles = promise.uploadfiles;

                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            if (dd.lpmtR_ResourceType !== null && dd.lpmtR_ResourceType !== "") {
                                var img = dd.lpmtR_ResourceType;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                dd.fileName = dd.lpmtR_FileName;
                                dd.filepath = dd.lpmtR_ResourceType;
                                if (lastelement === 'doc' || lastelement === 'xlsx' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'image/png' || lastelement === 'image/jpeg' || lastelement === 'image/jpg' || lastelement === 'video/mp4') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.lpmtR_ResourceType;
                                }
                            }
                        });
                    } else {
                        $('#popup11').modal('hide');
                        swal("No Documents Found");
                    }
                }
            });
        };

        //guest photo / video

        $scope.viewGuest = function (obj) {

            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            var data = {
                "description": 'photo',
                "PRYR_Id": obj.pryR_Id,
                "ASMAY_Id": obj.asmaY_Id
            };

            apiService.create("YearlyProgram/viewuploadflies", data).then(function (promise) {
                if (promise !== null) {
                    $scope.uploadfilesdetails = promise.uploadfiles;
                    if ($scope.uploadfilesdetails !== null && $scope.uploadfilesdetails.length > 0) {
                        $scope.uploaddocfiles = promise.uploadfiles;
                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            if (dd.lpmtR_ResourceType !== null && dd.lpmtR_ResourceType !== "") {
                                var img = dd.lpmtR_ResourceType;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                dd.fileName = dd.lpmtR_FileName;
                                dd.filepath = dd.lpmtR_ResourceType;
                                if (lastelement === 'doc' || lastelement === 'xlsx' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'image/png' || lastelement === 'image/jpeg' || lastelement === 'image/jpg' || lastelement === 'video/mp4') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.lpmtR_ResourceType;
                                }
                            }
                        });
                    } else {
                        $('#popup11').modal('hide');
                        swal("No Documents Found");
                    }
                }
            });
        };

        $scope.submitted = false;
        $scope.UploadGuardianPhoto = [];
        $scope.studentGuardianDetails = [{ id: 'Guardian1' }];
        $scope.Participants = [{ id: 'Guardian2' }];
        $scope.Account = [{ id: 'Guardian3' }];
        $scope.Invitation = [{ id: 'Guardian4' }];
        $scope.Winner = [{ id: 'Guardian5' }];
        $scope.Speech = [{ id: 'Guardian6' }];
        $scope.Profile = [{ id: 'Guardian7' }];
        //shilpa added newly
        $scope.Programgst = [{ id: 'guest1' }];

        $scope.uploadGuardianPhoto = function (input, document, value) {

            $scope.UploadGuardianPhoto = input.files;

            if (input.files && input.files[0]) {
                if ((input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    || input.files[0].type === "application/pdf" || input.files[0].type === "application/msword" || input.files[0].type === "application/vnd.ms-excel"
                    || input.files[0].type === 'image/png' || input.files[0].type === 'image/jpeg' || input.files[0].type === 'image/jpg' || input.files[0].type === 'video/mp4')) {
                    if (input.files[0].size <= 2097152) {
                        $scope.filename = input.files[0].name;
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            $('#').attr('src', e.target.result);
                        };
                        reader.readAsDataURL(input.files[0]);
                        UploaddianPhoto(document);
                    } else {
                        swal("File Size Should Be Less Than 2 MB"); // 2097152 bytes = 2MB 
                    }
                }
                else {
                    swal("Upload Only Pdf, Doc And Excel File Only");
                    return;
                }
            }

        };
        function UploaddianPhoto(data) {
            console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadGuardianPhoto.length; i++) {
                formData.append("File", $scope.UploadGuardianPhoto[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/OnlineProgramdoc", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    data.lpmtR_Resources = d;
                    data.file_name = $scope.filename;
                    $('#').attr('src', data.lpmtR_Resources);
                    var img = data.lpmtR_Resources;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    // swal(d);
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

            // Uploads1(miid);
        }

        $scope.clear = function () {
            $scope.GuestName = '';
            $scope.gno = '';
            $scope.gaddress = '';
            $scope.gtype = '';
            $scope.emailid = '';

        }

        //  $scope.guestdetails = [{ id: 'gstdetails1' }];
        $scope.guestdetails = [];
        $scope.addguest = function () {


            if ($scope.GuestName != null && $scope.GuestName != "" && $scope.gno != null && $scope.gaddress != null && $scope.gtype != null && $scope.emailid != null) {
                if ($scope.PRYRG_Id > 0) {
                    for (var i = $scope.guestdetails.length - 1; i >= 0; i--) {
                        if ($scope.guestdetails[i].PRYRG_Id == $scope.PRYRG_Id) {
                            $scope.guestdetails.splice(i, 1);
                        }
                    }

                    var a = $scope.guestdetails.length;
                    $scope.guestdetails.push({ gid: a + 1, PRYRG_Id: $scope.PRYRG_Id, name: $scope.GuestName, number: $scope.gno, address: $scope.gaddress, type: $scope.gtype, email: $scope.emailid, Programgst: $scope.Programgst, Speech: $scope.Speech, profile: $scope.Profile });
                    console.log($scope.guestdetails);
                    $scope.Programgst = [{ id: 'guest1' }];
                    $scope.Speech = [{ id: 'Guardian6' }];
                    $scope.Profile = [{ id: 'Guardian7' }];
                    $scope.details = true;
                    $scope.clear();
                    //for (var i = $scope.guestdetails.length - 1; i >= 0; i--) {
                    //    if ($scope.guestdetails[i].gid == guestdetailsgst.gid) {
                    //        $scope.guestdetails.splice(i, 1);
                    //    }
                    //}

                }
                else {
                    // var newItemNo = $scope.guestdetails.length + 1;
                    var a = $scope.guestdetails.length;
                    $scope.guestdetails.push({ gid: a + 1, name: $scope.GuestName, number: $scope.gno, address: $scope.gaddress, type: $scope.gtype, email: $scope.emailid, Programgst: $scope.Programgst, Speech: $scope.Speech, profile: $scope.Profile });
                    /*details: $scope.Programgst*/
                    console.log($scope.guestdetails);
                    $scope.Programgst = [{ id: 'guest1' }];
                    $scope.Speech = [{ id: 'Guardian6' }];
                    $scope.Profile = [{ id: 'Guardian7' }];
                    $scope.details = true;
                    $scope.clear();
                }
            }
            else {
                swal("Add Guest Details");
            }
        };

        $scope.removeNewsiblinguest = function (guestdetailsgst) {

            if ($scope.editEmployee > 0) {
                //if ($scope.PRYRG_Id > 0) {
                if (guestdetailsgst.pryrG_Id > 0) {
                    var data = {
                        "PRYRG_Id": guestdetailsgst.pryrG_Id
                    };
                    swal({
                        title: "Are you sure?",
                        text: "Do You Want To delete the Record?",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it",
                        cancelButtonText: "Cancel",
                        closeOnConfirm: false,
                        closeOnCancel: false
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                apiService.create("YearlyProgram/removeNewsiblinguest", data).then(function (promise) {
                                    if (promise.returnval == 'true') {
                                        swal("Record Deleted Successfully...");
                                        for (var i = $scope.guestdetails.length - 1; i >= 0; i--) {
                                            if ($scope.guestdetails[i].gid == guestdetailsgst.gid) {
                                                $scope.guestdetails.splice(i, 1);
                                            }
                                        }
                                    }
                                });
                                if ($scope.guestdetails.length == 0) {
                                    $scope.details = false;
                                }
                            }
                            else {
                                swal("Record Deletion Cancelled");
                            }
                        });
                }
            }
            else {
                for (var i = $scope.guestdetails.length - 1; i >= 0; i--) {
                    if ($scope.guestdetails[i].gid == guestdetailsgst.gid) {
                        $scope.guestdetails.splice(i, 1);
                    }
                }
            }
        };

        // For File Upload

        $scope.ProgramDetails = [{ id: 'photo1' }];
        $scope.Programgst = [{ id: 'guest1' }];

        $scope.addNewsiblingguard = function () {

            var newItemNo = $scope.ProgramDetails.length + 1;

            if (newItemNo <= 5) {
                $scope.ProgramDetails.push({ 'id': 'photo' + newItemNo });
            }
        };

        $scope.removeNewsiblingguard = function (index) {
            var newItemNo = $scope.ProgramDetails.length - 1;
            $scope.ProgramDetails.splice(index, 1);

            if ($scope.ProgramDetails.length === 0) {
                //data
            }
        };

        $scope.addNewsiblingguard1 = function () {

            var newItemNo = $scope.Programgst.length + 1;

            if (newItemNo <= 5) {
                $scope.Programgst.push({ 'id': 'guest' + newItemNo });
            }
        };

        $scope.removeNewsiblingguard1 = function (index) {
            var newItemNo = $scope.Programgst.length - 1;
            $scope.Programgst.splice(index, 1);

            if ($scope.Programgst.length === 0) {
                //data
            }
        };

        $scope.UploadPhoto1 = [];

        $scope.UploadPhoto = function (input, document) {

            $scope.UploadPhoto1 = input.files;

            if (input.files && input.files[0]) {
                $scope.filename = input.files[0].name;
                if (input.files[0].type === "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploaddianPhotovideo(document);

                }
                else if (input.files[0].type === "video/mp4") {
                    UploaddianPhotovideo(document);
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                } else {
                    swal("Upload MP4, Pdf, Image Files Only");
                }

            }
        };

        function UploaddianPhotovideo(data) {
            console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.UploadPhoto1.length; i++) {
                formData.append("File", $scope.UploadPhoto1[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/OnlineProgramdoc", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    data.lpmtR_Resources = d;
                    data.file_name = $scope.filename;
                    $('#').attr('src', data.lpmtR_Resources);
                    var img = data.lpmtR_Resources;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    // swal(d);
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

            // Uploads1(miid);
        }

        $scope.showGuardianPhoto = function (data) {

            imagedownload = data.lpmtR_Resources;
            studentreg = data.ismS_SubjectName;
            docname = data.lpmmT_TopicName + ' ' + data.lpmT_TopicName;

            $('#preview').attr('src', data.lpmtR_Resources);
            $('#myModalimg').modal('show');
        };

        $scope.downloadview = function (pdfview) {
            $scope.pdfurl = pdfview;
            $scope.showpdf = true;
            $('#showpdf').modal('show');
        };

        //conference
        $scope.pauseOrPlay = function (ele) {
            $('#popup15').modal({
                show: false
            }).on('hidden.bs.modal', function () {
                $(this).find('video')[0].pause();
            });
        };

        $scope.showGuardianPhotonew = function (data) {
            $scope.ggg = data.filepath;
            if (data.filepath != null) {
                $scope.view_videos = [];
                $scope.videoSources = [];
                $scope.preview1 = data.lpmtR_Resources;
                $scope.videdfd = data.lpmtR_Resources;
                $scope.movie = { src: data.lpmtR_Resources };
                $scope.movie1 = { src: data.lpmtR_Resources };
                $scope.view_videos.push({ id: 1, coeeV_Videos: $scope.ggg });
                $('#popup15').modal('show');
                console.log($scope.view_videos);
            }
            else {
                $scope.view_videos = [];
                $scope.videoSources = [];
                $scope.preview1 = data.lpmtR_Resources;
                $scope.videdfd = data.lpmtR_Resources;
                $scope.movie = { src: data.lpmtR_Resources };
                $scope.movie1 = { src: data.lpmtR_Resources };
                $scope.view_videos.push({ id: 1, coeeV_Videos: data.lpmtR_Resources });
                $('#popup15').modal('show');
                console.log($scope.view_videos);
            }

        };
        $scope.download = function () {

            $http.get(imagedownload, {
                responseType: "arraybuffer"
            })
                .success(function (data) {
                    var anchor = angular.element('<a/>');
                    var blob = new Blob([data]);
                    anchor.attr({
                        href: window.URL.createObjectURL(blob),
                        target: '_blank',
                        download: studentreg + '-' + docname + '.jpg'
                    })[0].click();
                });
        };

        $scope.savedata = function () {

            if ($scope.myForm.$valid) {

                $scope.guestdetailsnew = [];
                if ($scope.guestdetails.length > 0) {
                    $scope.guestdetailsnew = $scope.guestdetails.slice(0);
                    //$scope.guestdetails.shift();
                }


                $scope.from_date = new Date($scope.from_date).toDateString();
                $scope.to_date = new Date($scope.to_date).toDateString();
                var data = {
                    //"PRYR_Id": $scope.pryR_Id,
                    "PRYR_Id": $scope.editEmployee,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "programname": $scope.pgname,
                    "Fromdate": $scope.from_date,
                    "Todate": $scope.to_date,
                    "PRMTLE_Id": $scope.PRMTLE_Id,
                    "description": $scope.breport,
                    "PRMTY_Id": $scope.PRMTY_Id,
                    "PRYRG_GuestName": $scope.pgorg,
                    "club": $scope.clubactivity,
                    "strength": $scope.tpaticipation,
                    "start_time": $filter('date')($scope.srtime, "h:mm a"),
                    "end_time": $filter('date')($scope.edtime, "h:mm a"),
                    "pgTempDTO": $scope.Participants,
                    "pgTempDTO2": $scope.Account,
                    "pgTempDTO3": $scope.Invitation,
                    "pgTempDTO4": $scope.Winner,
                    "pgTempDTO1": $scope.studentGuardianDetails,
                    "pgTempDTO7": $scope.ProgramDetails,
                    "pgTempDTO6": $scope.Profile,
                    "pgTempDTO5": $scope.Speech,
                    "pgTempDTO99": $scope.Programgst,
                    "GuestName": $scope.GuestName,
                    "gno": $scope.gno,
                    "gaddress": $scope.gaddress,
                    "gtype": $scope.gtype,
                    "emailid": $scope.emailid,
                    "pgTempDTO8": $scope.guestdetailsnew,
                    //"guestidsDTO": $scope.guestids

                };

                apiService.create("YearlyProgram/Savedata", data).then(function (promise) {

                    if (promise.returnresult === true) {
                        if (promise.message == "Saved") {
                            swal('Record Saved Successfully');
                            $state.reload();
                        }
                        else if (promise.message == "Update") {
                            swal('Record Updated Successfully');
                            $state.reload();
                        }
                        else if (promise.message == "Duplicate") {
                            swal('Record Already Exist');
                            $state.reload();
                        }
                        else {
                            swal('Record Not Saved Successfully');
                            $state.reload();
                        }
                    }
                    else {
                        swal('Record Not Saved Successfully');
                        $state.reload();

                    }
                    //   $scope.loaddata();

                });
            }
            else {

                $scope.submitted = true;
            }

        };
        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };

        $scope.hstep = 1;
        $scope.mstep = 1;
        //$scope.editEmployee= $scope.editEmployee;
        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };
        $scope.getorgvalue = function (employee) {

            $scope.editEmployee = employee.pryR_Id;
            //var pageid = $scope.editEmployee;

            var data = {
                "PRYR_Id": $scope.editEmployee,
            };
            apiService.create("YearlyProgram/getdetails", data).
                then(function (promise) {
                    $scope.abcd = promise.programlist[0];
                    $scope.editEmployee = promise.programlist[0].pryR_Id;
                    $scope.asmaY_Id = promise.programlist[0].asmaY_Id;
                    $scope.breport = promise.programlist[0].description;
                    $scope.PRMTLE_Id = promise.programlist[0].prmtlE_Id;

                    $scope.srtime = moment(promise.programlist[0].start_time, 'h:mm a').format();
                    $scope.edtime = moment(promise.programlist[0].end_time, 'h:mm a').format();
                    $scope.pgorg = promise.programlist[0].pryR_SponsorAgency;

                    $scope.pgname = promise.programlist[0].programname;
                    $scope.PRMTY_Id = promise.programlist[0].prmtY_Id;
                    $scope.clubactivity = promise.fillActivities1[0].pryrA_ActivityName;
                    //$scope.edtime = promise.programlist[0].end_time;
                    $scope.tpaticipation = promise.programlist[0].strength;

                    $scope.pgorg = promise.programlist[0].org_name;
                    $scope.breport = promise.programlist[0].description;
                    $scope.from_date = new Date(promise.programlist[0].fromdate);
                    $scope.to_date = new Date(promise.programlist[0].todate);

                    $scope.Participants = promise.uploadfiles11;

                    if ($scope.Participants !== null && $scope.Participants.length > 0) {

                        angular.forEach($scope.Participants, function (dd) {
                            if (dd.lpmtR_ResourceType !== null && dd.lpmtR_ResourceType !== "") {
                                var img = dd.lpmtR_ResourceType;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                dd.lpmtR_ResourceType = dd.lpmtR_ResourceType;
                                dd.lpmtR_Resources = dd.lpmtR_ResourceType;
                                dd.lpmtR_FileName = dd.lpmtR_FileName;
                                dd.file_name = dd.lpmtR_FileName;
                                dd.filename = dd.lpmtR_FileName;

                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls' || lastelement === 'png' || lastelement === 'jpeg' || lastelement === 'jpg') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.lpmtR_ResourceType;
                                }
                            }
                            // $('#popup15').modal('show');
                        });
                    }
                    else {
                        $scope.Participants = [{ id: 'Guardian2' }];
                    }

                    $scope.Winner = promise.uploadfiles22;
                    if ($scope.Winner !== null && $scope.Winner.length > 0) {
                        angular.forEach($scope.Winner, function (dd) {
                            if (dd.lpmtR_ResourceType !== null && dd.lpmtR_ResourceType !== "") {
                                var img = dd.lpmtR_ResourceType;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                dd.lpmtR_ResourceType = dd.lpmtR_ResourceType;
                                dd.lpmtR_Resources = dd.lpmtR_ResourceType;
                                dd.lpmtR_FileName = dd.lpmtR_FileName;
                                dd.file_name = dd.lpmtR_FileName;
                                dd.filename = dd.lpmtR_FileName;

                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls' || lastelement === 'png' || lastelement === 'jpeg' || lastelement === 'jpg') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.lpmtR_ResourceType;
                                }
                            }
                            // $('#popup15').modal('show');
                        });
                    }
                    else {
                        $scope.Winner = [{ id: 'Guardian5' }];
                    }

                    $scope.Account = promise.uploadfiles33;
                    if ($scope.Account !== null && $scope.Account.length > 0) {
                        angular.forEach($scope.Account, function (dd) {
                            if (dd.lpmtR_ResourceType !== null && dd.lpmtR_ResourceType !== "") {
                                var img = dd.lpmtR_ResourceType;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                dd.lpmtR_ResourceType = dd.lpmtR_ResourceType;
                                dd.lpmtR_Resources = dd.lpmtR_ResourceType;
                                dd.lpmtR_FileName = dd.lpmtR_FileName;
                                dd.file_name = dd.lpmtR_FileName;
                                dd.filename = dd.lpmtR_FileName;

                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls' || lastelement === 'png' || lastelement === 'jpeg' || lastelement === 'jpg') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.lpmtR_ResourceType;
                                }
                            }
                            // $('#popup15').modal('show');
                        });
                    }
                    else {
                        $scope.Account = [{ id: 'Guardian3' }];
                    }
                    $scope.studentGuardianDetails = promise.uploadfiles44;
                    if ($scope.studentGuardianDetails !== null && $scope.studentGuardianDetails.length > 0) {
                        angular.forEach($scope.studentGuardianDetails, function (dd) {
                            if (dd.lpmtR_ResourceType !== null && dd.lpmtR_ResourceType !== "") {
                                var img = dd.lpmtR_ResourceType;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                dd.lpmtR_ResourceType = dd.lpmtR_ResourceType;
                                dd.lpmtR_Resources = dd.lpmtR_ResourceType;
                                dd.lpmtR_FileName = dd.lpmtR_FileName;
                                dd.file_name = dd.lpmtR_FileName;
                                dd.filename = dd.lpmtR_FileName;

                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls' || lastelement === 'png' || lastelement === 'jpeg' || lastelement === 'jpg') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.lpmtR_ResourceType;
                                }
                            }
                            // $('#popup15').modal('show');
                        });
                    }
                    else {
                        $scope.studentGuardianDetails = [{ id: 'Guardian1' }];
                    }
                    $scope.Invitation = promise.uploadfiles55;
                    if ($scope.Invitation !== null && $scope.Invitation.length > 0) {
                        angular.forEach($scope.Invitation, function (dd) {
                            if (dd.lpmtR_ResourceType !== null && dd.lpmtR_ResourceType !== "") {
                                var img = dd.lpmtR_ResourceType;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                dd.lpmtR_ResourceType = dd.lpmtR_ResourceType;
                                dd.lpmtR_Resources = dd.lpmtR_ResourceType;
                                dd.lpmtR_FileName = dd.lpmtR_FileName;
                                dd.file_name = dd.lpmtR_FileName;
                                dd.filename = dd.lpmtR_FileName;

                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls' || lastelement === 'png' || lastelement === 'jpeg' || lastelement === 'jpg') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.lpmtR_ResourceType;
                                }
                            }
                            // $('#popup15').modal('show');
                        });
                    }
                    else {
                        $scope.Invitation = [{ id: 'Guardian4' }];
                    }
                    $scope.ProgramDetails = promise.uploadfiles66;
                    if ($scope.ProgramDetails !== null && $scope.ProgramDetails.length > 0) {
                        angular.forEach($scope.ProgramDetails, function (dd) {
                            if (dd.lpmtR_ResourceType !== null && dd.lpmtR_ResourceType !== "") {
                                var img = dd.lpmtR_ResourceType;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                dd.lpmtR_Resources = dd.lpmtR_ResourceType;
                                dd.lpmtR_FileName = dd.lpmtR_FileName;
                                dd.file_name = dd.lpmtR_FileName;
                                dd.filename = dd.lpmtR_FileName;
                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls' || lastelement === 'png' || lastelement === 'jpeg' || lastelement === 'jpg') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.lpmtR_ResourceType;
                                }
                            }
                        });
                    }
                    else {
                        $scope.ProgramDetails = [{ id: 'photo1' }];
                    }

                    if (promise.msg22 == "common") {
                        $scope.GuestName = promise.guest[0].pryrG_GuestName;
                        $scope.gno = promise.guest[0].pryrG_GuestPhoneNo;
                        $scope.gaddress = promise.guest[0].pryrG_GuestAddress;
                        $scope.gtype = promise.guest[0].pryrG_GuestType;
                        $scope.emailid = promise.guest[0].pryrG_GuestEmailId;


                        $scope.Programgst = promise.uploadfiles99;
                        if ($scope.Programgst !== null && $scope.Programgst.length > 0) {

                            angular.forEach($scope.Programgst, function (dd) {
                                var img = dd.lpmtR_ResourceType;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                dd.lpmtR_Resources = dd.lpmtR_ResourceType;
                                dd.lpmtR_FileName = dd.lpmtR_FileName;
                                dd.file_name = dd.lpmtR_FileName;
                                dd.filename = dd.lpmtR_FileName;
                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls' || lastelement === 'png' || lastelement === 'jpeg' || lastelement === 'jpg') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.lpmtR_ResourceType;
                                }
                            });
                        }
                        else {
                            $scope.Programgst = [{ id: 'guest1' }];
                        }
                        $scope.Profile = promise.uploadfiles77;
                        if ($scope.Profile !== null && $scope.Profile.length > 0) {
                            angular.forEach($scope.Profile, function (dd) {
                                if (dd.lpmtR_ResourceType !== null && dd.lpmtR_ResourceType !== "") {
                                    var img = dd.lpmtR_ResourceType;
                                    var imagarr = img.split('.');
                                    var lastelement = imagarr[imagarr.length - 1];
                                    dd.filetype = lastelement;
                                    dd.lpmtR_ResourceType = dd.lpmtR_ResourceType;
                                    dd.lpmtR_Resources = dd.lpmtR_ResourceType;
                                    dd.lpmtR_FileName = dd.lpmtR_FileName;
                                    dd.file_name = dd.lpmtR_FileName;
                                    dd.filename = dd.lpmtR_FileName;

                                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls' || lastelement === 'png' || lastelement === 'jpeg' || lastelement === 'jpg') {
                                        dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.lpmtR_ResourceType;
                                    }
                                }
                                // $('#popup15').modal('show');
                            });
                        }
                        else {
                            $scope.Profile = [{ id: 'Guardian7' }];
                        }
                        $scope.Speech = promise.uploadfiles88;
                        if ($scope.Speech !== null && $scope.Speech.length > 0) {
                            angular.forEach($scope.Speech, function (dd) {
                                if (dd.lpmtR_ResourceType !== null && dd.lpmtR_ResourceType !== "") {
                                    var img = dd.lpmtR_ResourceType;
                                    var imagarr = img.split('.');
                                    var lastelement = imagarr[imagarr.length - 1];
                                    dd.filetype = lastelement;
                                    dd.lpmtR_ResourceType = dd.lpmtR_ResourceType;
                                    dd.lpmtR_Resources = dd.lpmtR_ResourceType;
                                    dd.lpmtR_FileName = dd.lpmtR_FileName;
                                    dd.file_name = dd.lpmtR_FileName;
                                    dd.filename = dd.lpmtR_FileName;

                                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls' || lastelement === 'png' || lastelement === 'jpeg' || lastelement === 'jpg') {
                                        dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.lpmtR_ResourceType;
                                    }
                                }
                                // $('#popup15').modal('show');
                            });
                        }
                        else {
                            $scope.Speech = [{ id: 'Guardian6' }];
                        }
                    }

                    else if (promise.msg22 == "grid") {

                        $scope.guestdetails = promise.guestgrid;

                        if ($scope.guestdetails !== null && $scope.guestdetails.length > 0) {
                            $scope.details = true;
                        } else {
                            $scope.details = false;
                        }

                        $scope.guestids = [];
                        var count = 0;
                        angular.forEach($scope.guestdetails, function (tt) {
                            count = count + 1;
                            tt.gid = count;
                            tt.PRYRG_Id = tt.pryrG_Id;
                        });


                        $scope.Profileedit = promise.uploadfiles77;
                        $scope.Speechedit = promise.uploadfiles88;
                        $scope.Programgstedit = promise.uploadfiles99;
                    }
                });
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.delete = function (employee) {
            $scope.editEmployee = employee.pryR_Id;
            //var pageid = $scope.editEmployee;
            var data = {
                "PRYR_Id": $scope.editEmployee
            };
            swal({
                title: "Are you sure?",
                text: "Do You Want To delete the Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("YearlyProgram/delete", data).then(function (promise) {

                            if (promise.returnval === "true") {
                                swal("Record Deleted Successfully...");
                                //swal("Record " + confirmmgs + " Successfully");
                            }
                            else {
                                swal("Record Not Deleted Successfully...");
                            }
                            $state.reload();
                        });
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        };
    }


    angular.module('app').filter("trustUrl", function ($sce) {
        return function (Url) { return $sce.trustAsResourceUrl(Url); };
    });

    angular.module('app').directive('txtArea', function () {
        return {
            restrict: 'AE',
            replace: 'true',
            scope: { data: '=', model: '=ngModel' },
            template: "<textarea></textarea>",
            link: function (scope, elem) {
                scope.$watch('data', function (newVal) {
                    if (newVal) {
                        scope.model += newVal[0];
                    }
                });
            }
        };
    });
})();







