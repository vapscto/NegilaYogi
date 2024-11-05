(function () {
    'use strict';
    angular.module('app').controller('ConferenceDetailsController', ConferenceDetailsController)
    ConferenceDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$sce', '$filter', '$window']
    function ConferenceDetailsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $sce, $filter, $window) {
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
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        /* loading start*/
        $scope.loaddata = function () {

            $scope.currentPage = 1;
            $scope.Oral_1 = 0;
            $scope.Lecture_1 = 0;
            $scope.traning_1 = 0;
            $scope.Poster_p_1 = 0;

            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("ConferenceDetails/getloaddata", pageid).then(function (promise) {
                $scope.fillyear = promise.fillyear;
                $scope.programlist = promise.programlist;
                $scope.gridOptions.data = promise.programlist;
                $scope.Typelist = promise.typelist;
                $scope.levellist = promise.levellist;
                $scope.filldepartment = promise.filldepartment;

            });
        };
        /* loading end*/

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
                { name: 'asmaY_Year', displayName: 'Academic Year' },
                { name: 'programname', displayName: 'Program Name', width: 200 },
                { name: 'departmentname', displayName: 'Department Name', width: 200 },
                { name: 'description', displayName: 'Start Date', width: 100 },
                {
                    name: 'Account Statement', displayName: 'Account Details', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="grid-action-cell">' +
                        '<a   href="javascript:void(0)" data-toggle="modal" data-target="#myModal" data-backdrop="static" ng-click="grid.appScope.viewrefernce(row.entity);"> <i class="fa fa-eye text-blue" > </i></a>  &nbsp; &nbsp;' + '</div>'
                },
                {
                    name: 'Winner List', displayName: 'Winner List', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="grid-action-cell">' +
                        '<a   href="javascript:void(0)" data-toggle="modal" data-target="#myModal" data-backdrop="static" ng-click="grid.appScope.viewrefernce1(row.entity);"> <i class="fa fa-eye text-blue" ></i></a>  &nbsp; &nbsp;' + '</div>'
                },
                {
                    name: 'Guest Photo/Video', displayName: 'Photo/Video', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="grid-action-cell">' +
                        '<a   href="javascript:void(0)" data-toggle="modal" data-target="#myModal" data-backdrop="static" ng-click="grid.appScope.viewphoto(row.entity);"> <i class="fa fa-eye text-blue" ></i></a>  &nbsp; &nbsp;' + '</div>'
                },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <md-tooltip md-direction="down">Edit</md-tooltip><i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                        '<a href="javascript:void(0)" style="color:red;" ng-click="grid.appScope.delete(row.entity);"> <md-tooltip md-direction="down">Delete Now</md-tooltip> <i class="fa fa-trash"></i></a>' +
                        '</div>'
                }
            ],
        };

        //yearly
        $scope.showmothersign = function (path) {

            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };

        $scope.viewrefernce1 = function (obj) {

            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            var data = {
                "description": 'winner',
                "PRYR_Id": obj.pryR_Id,
                "ASMAY_Id": obj.asmaY_Id
            };

            apiService.create("ConferenceDetails/viewuploadflies", data).then(function (promise) {

                if (promise !== null) {
                    $scope.uploadfilesdetails = promise.uploadfiles;
                    if ($scope.uploadfilesdetails !== null && $scope.uploadfilesdetails.length > 0) {
                        $scope.uploaddocfiles = promise.uploadfiles;

                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            var img = dd.lpmtR_ResourceType;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'xlsx' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.lpmtR_ResourceType;
                            }
                            // $('#popup15').modal('show');
                        });
                    } else {
                        $('#popup11').modal('hide');
                        swal("No Documents Found");
                    }
                }
            });
        };

        //yearly program 
        $scope.viewphoto = function (obj) {

            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            var data = {
                "description": 'programphoto',
                "PRYR_Id": obj.pryR_Id,
                "ASMAY_Id": obj.asmaY_Id
            };

            apiService.create("ConferenceDetails/viewuploadflies", data).then(function (promise) {
                if (promise !== null) {
                    $scope.uploadfilesdetails = promise.uploadfiles1;
                    if ($scope.uploadfilesdetails !== null && $scope.uploadfilesdetails.length > 0) {
                        $scope.uploaddocfiles = promise.uploadfiles1;
                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            var img = dd.lpmtR_ResourceType;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            dd.fileName = dd.lpmtR_FileName;
                            dd.filepath = dd.lpmtR_ResourceType;
                            if (lastelement === 'doc' || lastelement === 'xlsx' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'image/png' || lastelement === 'image/jpeg' || lastelement === 'image/jpg' || lastelement === 'video/mp4') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.lpmtR_ResourceType;
                            }
                        });
                    } else {
                        $('#popup11').modal('hide');
                        swal("No Documents Found");
                    }
                }
            });
        };

        $scope.viewrefernce = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            var data = {
                "description": 'account',
                "PRYR_Id": obj.pryR_Id,
                "ASMAY_Id": obj.asmaY_Id
            };

            apiService.create("ConferenceDetails/viewuploadflies", data).then(function (promise) {

                if (promise !== null) {
                    $scope.uploadfilesdetails = promise.uploadfiles2;
                    if ($scope.uploadfilesdetails !== null && $scope.uploadfilesdetails.length > 0) {
                        $scope.uploaddocfiles = promise.uploadfiles2;
                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            if (dd.lpmtR_ResourceType !== null && dd.lpmtR_ResourceType !== "") {
                                var img = dd.lpmtR_ResourceType;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                if (lastelement === 'doc' || lastelement === 'xlsx' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xls' || lastelement === 'image/png' || lastelement === 'image/jpeg' || lastelement === 'image/jpg' || lastelement === 'video/mp4') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.lpmtR_ResourceType;
                                }
                            }
                            // $('#popup15').modal('show');
                        });
                    } else {
                        $('#popup11').modal('hide');
                        swal("No Documents Found");
                    }
                }
            });
        };


        $scope.addNew1 = function (totalgrid1) {
            $scope.totalgrid1 = [];
            var LMBANO_No = '';
            if ($scope.LMB_No1 != null || $scope.LMB_No1 != '') {
                var a = $scope.LMB_No1;
                for (var i = 0; i < a; i++) {
                    $scope.totalgrid1.push({
                        LMBANO_No: '',
                    });
                }
            }
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
        $scope.uploadGuardianPhoto = function (input, document, value) {

            $scope.UploadGuardianPhoto = input.files;

            if (input.files && input.files[0]) {

                if ((input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    || input.files[0].type === "application/pdf" || input.files[0].type === "application/msword" || input.files[0].type === "application/vnd.ms-excel" || input.files[0].type === 'image/png' || input.files[0].type === 'image/jpeg' || input.files[0].type === 'image/jpg' || input.files[0].type === 'video/mp4')) {
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
        };

        $scope.guestdetails = [{ id: 'gstdetails1' }];
        $scope.addguest = function () {
            $scope.details = true;
            var newItemNo = $scope.guestdetails.length + 1;

            $scope.guestdetails.push({ 'id': 'gstdetails' + newItemNo, name: $scope.GuestName, number: $scope.gno, address: $scope.gaddress, type: $scope.gtype, email: $scope.emailid, details: $scope.Programgst, Speech: $scope.Speech, profile: $scope.Profile });
            console.log($scope.guestdetails);
            $scope.Programgst = [{ id: 'guest1' }];
            $scope.Speech = [{ id: 'Guardian6' }];
            $scope.Profile = [{ id: 'Guardian7' }];
            $scope.clear();
        };

        $scope.removeNewsiblinguest = function (index) {
            var newItemNo = $scope.guestdetails.length - 1;
            $scope.guestdetails.splice(index, 1);
            $scope.Programgst = [{ id: 'guest1' }];
            $scope.Speech = [{ id: 'Guardian6' }];
            $scope.Profile = [{ id: 'Guardian7' }];
        };


        // For File Upload

        $scope.ProgramDetails = [{ id: 'photo1' }];
        $scope.Programgst = [{ id: 'guest1' }];

        $scope.addNewsiblingguard = function () {

            var newItemNo = $scope.ProgramDetails.length + 1;

            if (newItemNo <= 10) {
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

        //yearly program
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




        $scope.showpdf = false;
        $scope.downloadview = function (pdfview) {
            $scope.pdfurl = pdfview;
            $scope.showpdf = true;
            $('#showpdf').modal('show');
        };


        $scope.showGuardianPhoto = function (data) {

            imagedownload = data.lpmtR_Resources;
            studentreg = data.ismS_SubjectName;
            docname = data.lpmmT_TopicName + ' ' + data.lpmT_TopicName;

            $('#preview').attr('src', data.lpmtR_Resources);
            //  $('#myModalimg').modal('show');
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

        $scope.change1 = function () {
            if ($scope.Oral_1 == 0) {
                $scope.Oral = "";
            }
            if ($scope.Lecture_1 == 0) {
                $scope.Lecture = "";
            }

            if ($scope.traning_1 == 0) {
                $scope.traning = "";
            }

            if ($scope.Poster_p_1 == 0) {
                $scope.Poster_p = "";
            }
        };

        $scope.savedata = function () {
            if ($scope.myForm.$valid) {
                $scope.from_date = new Date($scope.from_date).toDateString();
                if ($scope.Oral_1 == 0) {
                    $scope.Oral = 0;

                }
                if ($scope.Lecture_1 == 0) {
                    $scope.Lecture = 0;
                }

                if ($scope.traning_1 == 0) {
                    $scope.traning = 0;
                }

                if ($scope.Poster_p_1 == 0) {
                    $scope.Poster_p = 0;
                }
                if ($scope.ProgramDetails.length > 4) {

                    var data = {
                        "PRYR_Id": $scope.editEmployee,
                        "PRMTY_Id": $scope.programtype,
                        "PRMTLE_Id": $scope.programlevel,
                        "HRMD_Id": $scope.Depart,
                        "programname": $scope.programname,
                        "PRYRG_GuestPhoneNo": $scope.programduration,
                        "description": $scope.Brief_report,
                        "PRYR_SponsorAgency": $scope.Sponsoring,
                        "Dept_Id": $scope.Depart,
                        "PRYRG_GuestSpeech": $scope.Conv_org,
                        "start_time": $filter('date')($scope.srtime, "h:mm a"),
                        "pgTempDTO2": $scope.Account,
                        "pgTempDTO4": $scope.Winner,
                        "pgTempDTO7": $scope.ProgramDetails,
                        "participants": $scope.Total_participants,
                        "strength": $scope.Students_col,
                        "Facty": $scope.Facty,
                        "Stud_oth": $scope.Stud_oth,
                        "Nat_part": $scope.Nat_part,
                        "Int_part": $scope.Int_part,
                        "Rch_schl": $scope.Rch_schl,
                        "Oral": $scope.Oral,
                        "Lecture": $scope.Lecture,
                        "traning": $scope.traning,
                        "Poster_p": $scope.Poster_p,
                        "Oral_1": $scope.Oral_1,
                        "Lecture_1": $scope.Lecture_1,
                        "traning_1": $scope.traning_1,
                        "Poster_p_1": $scope.Poster_p_1,
                        "Fromdate": $scope.from_date,
                        "PRYRG_GuestName": $scope.speaker,
                        "PRYRG_GuestType": $scope.title,
                    };

                    apiService.create("ConferenceDetails/Savedata", data).then(function (promise) {
                        // if (promise.returnresult === true) {
                        if (promise.message == "Saved") {
                            swal('Record Saved Successfully');
                            $state.reload();
                        }
                        else if (promise.message == "Update") {
                            swal('Record Updated Successfully');
                            $state.reload();
                        }
                        else if (promise.message == "Duplicate") {
                            swal('Record Already Saved');
                            $state.reload();
                        }
                        else {
                            swal('Record Not Saved Successfully');
                            $state.reload();
                        }
                    });
                }
                else {
                    swal("Upload Atleast 5 Photos for Photo/Video of the Program");
                }
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

        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };
        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.pryR_Id;
            //var pageid = $scope.editEmployee;

            var data = {
                "PRYR_Id": $scope.editEmployee
            };

            apiService.create("ConferenceDetails/getdetails", data).then(function (promise) {
                if (promise.programlist !== null) {
                    $scope.programname = promise.programlist[0].pryR_ProgramName;
                    $scope.programtype = promise.programlist[0].pryR_ProgramTypeId;
                    //$scope.programnamepromise =promise.programlist[0].programnamepromise;
                    $scope.programduration = promise.programlist[0].pryrG_GuestPhoneNo;
                    $scope.Brief_report = promise.programlist[0].pryR_ProgramDescription;
                    $scope.Sponsoring = promise.programlist[0].pryR_SponsorAgency;
                    $scope.Depart = promise.programlist[0].hrmD_Id;
                    $scope.Conv_org = promise.programlist[0].pryR_PrgramConvenor;
                    $scope.srtime = moment(promise.programlist[0].pryR_StartTime, 'h:mm a').format();
                    $scope.Total_participants = promise.programlist[0].pryR_TotalParticipants;
                    $scope.Students_col = promise.programlist[0].pryR_OurCollStudents;
                    $scope.Facty = promise.programlist[0].pryR_Faculty;
                    $scope.Stud_oth = promise.programlist[0].pryR_OthCollStudents;
                    $scope.Nat_part = promise.programlist[0].pryR_NatParticipants;
                    $scope.Int_part = promise.programlist[0].pryR_IntParticipants;
                    $scope.Rch_schl = promise.programlist[0].pryR_ResearchScholars;
                    $scope.Oral = promise.programlist[0].pryR_OralPresentation;
                    $scope.Lecture = promise.programlist[0].pryR_LecturesNo;
                    $scope.traning = promise.programlist[0].pryR_TrainingNo;

                    $scope.editEmployee = promise.programlist[0].pryR_Id;
                    $scope.Poster_p = promise.programlist[0].pryR_PosterPresentation;
                    $scope.Orali_1 = promise.programlist[0].pryR_OralPresentationFlg;
                    if ($scope.Orali_1 != false) {
                        $scope.Oral_1 = 1;
                    }
                    else {
                        $scope.Oral_1 = 0;
                    }
                    $scope.Lecturei_1 = promise.programlist[0].pryR_LecturesFlg;
                    if ($scope.Lecturei_1 != false) {
                        $scope.Lecture_1 = 1;
                    }
                    else {
                        $scope.Lecture_1 = 0;
                    }
                    $scope.traningi_1 = promise.programlist[0].pryR_TrainingFlg;
                    if ($scope.traningi_1 != false) {
                        $scope.traning_1 = 1;
                    }
                    else {
                        $scope.traning_1 = 0;
                    }
                    $scope.Poster_pi_1 = promise.programlist[0].pryR_PosterPresentationFlg;
                    if ($scope.Poster_pi_1 != false) {
                        $scope.Poster_p_1 = 1;
                    }
                    else {
                        $scope.Poster_p_1 = 0;
                    }
                    $scope.from_date = new Date(promise.programlist[0].pryR_StartDate);
                    $scope.programlevel = promise.programlist[0].pryR_PrgramLevel;
                    $scope.title = promise.alllist[0].pryrG_GuestType;
                    $scope.speaker = promise.alllist[0].pryrG_GuestName;
                    $scope.Account = promise.uploadfiles2;
                    if ($scope.Account !== null && $scope.Account.length > 0) {
                        $scope.Account = promise.uploadfiles2;
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
                    //////////////////////////////////////////////winner list
                    $scope.Winner = promise.testarray;
                    if ($scope.Winner !== null && $scope.Winner.length > 0) {
                        $scope.Winner = promise.testarray;
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
                    $scope.ProgramDetails = promise.uploadfiles1;
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
                        apiService.create("ConferenceDetails/delete", data).then(function (promise) {
                            if (promise.returnval === "true") {
                                swal("Record Deleted Successfully...");
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

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
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







