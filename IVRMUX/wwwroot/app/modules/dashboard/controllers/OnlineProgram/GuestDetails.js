
(function () {
    'use strict';
    angular
        .module('app')
        .controller('GuestDetailsController', GuestDetailsController)

    GuestDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', '$sce']
    function GuestDetailsController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, $sce) {
        $scope.editEmployee = {};

        $scope.hstep = 1;
        $scope.mstep = 1;

        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };

        //Ui Grid view rendering data from data base


        //$scope.gridOptions2 = {
        //    enableColumnMenus: false,
        //    enableFiltering: true,
        //    paginationPageSizes: [5, 10, 15],
        //    paginationPageSize: 5,

        //    columnDefs: [
        //        { name: 'SNO', width: '5%', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
        //        { name: 'programname', width: '15%', displayName: 'Program Name' },
        //        { name: 'pryrG_GuestType', width: '15%', displayName: 'Guest Type' },
        //        { name: 'pryrG_GuestName', width: '15%', displayName: 'Guest Name' },
        //        { name: 'pryrG_GuestPhoneNo', width: '15%', displayName: 'Phone No' },
        //        { name: 'pryrG_GuestEmailId', width: '15%', displayName: 'Email Id' },
        //        {
        //            name: 'pryrG_GuestProfile', width: '10%', displayName: 'Guest Profile', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="grid-action-cell">' +
        //                '<a   href="javascript:void(0)" data-toggle="modal" data-target="#popup11" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup2(row.entity);"> <i class="fa fa-file-pdf-o" ></i></a>  &nbsp; &nbsp;' + '</div>'
        //        },
        //        {
        //            name: 'images', width: '10%', displayName: 'Images', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="grid-action-cell">' +
        //                '<a   href="javascript:void(0)" data-toggle="modal" data-target="#popup11" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup2(row.entity);"> <i class="fa fa-picture-o text-green" ></i></a>  &nbsp; &nbsp;' + '</div>'
        //        },
        //        {
        //            name: 'videos', width: '10%', displayName: 'Videos', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="grid-action-cell">' +
        //                '<a  href="javascript:void(0)" data-toggle="modal" data-target="#popup15" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup2(row.entity);"> <i class="fa fa-film text-blue" ></i></a>  &nbsp; &nbsp;' + '</div>'
        //        },

        //        {
        //            field: 'id', name: '', width: '10%',
        //            displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:

        //                '<div class="grid-action-cell">' +
        //                //'<a  href="javascript:void(0)" data-toggle="modal" data-target="#popup2" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup2(row.entity);"> <i class="fa fa-eye text-purple" ></i></a>  &nbsp; &nbsp;' +
        //                '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue2(row.entity);"> <md-tooltip md-direction="down">Edit Now</md-tooltip> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
        //                '<a ng-if="row.entity.coeE_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.switch2(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
        //                '<span ng-if="row.entity.coeE_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.switch2(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +

        //                '</div>'
        //        }
        //    ]

        //};



        // TO Save The Data
        $scope.submitted1 = false;

        $scope.coeE_EStartDate = "";

        $scope.submitted2 = false;

        $scope.savemappedeventsdata = function () {

            if ($scope.myForm.$valid) {
                $scope.albumNameArray1 = [];
                $scope.albumNameArray2 = [];

                if ($scope.images_paths == null || $scope.images_paths == undefined || $scope.images_paths == "") {
                    $scope.images_paths = [];
                }
                if ($scope.videos_paths == null || $scope.videos_paths == undefined || $scope.videos_paths == "") {
                    $scope.videos_paths = [];
                }
                var data = {
                    "PRYR_Id": $scope.pryR_Id,
                    images_list: $scope.images_paths,
                    videos_list: $scope.videos_paths,
                    "PRYRG_GuestType": $scope.gtype,
                    "PRYRG_GuestName": $scope.gname,
                    "PRYRG_GuestPhoneNo": $scope.gno,
                    "PRYRG_GuestEmailId": $scope.emailid,
                    "PRYRG_GuestProfile": $scope.gprofile,
                    "PRYRG_GuestSpeech": $scope.gspeech,
                    
                }
                apiService.create("GuestDetails/savedetail2", data).
                    then(function (promise) {

                        if (promise.returnresult == true) {
                            swal('Record successfully Saved');
                             $state.reload();
                        }
                        else if (promise.returnresult == false) {
                            swal('Record Not Saved !');
                            $state.reload();
                        }

                    })
                $scope.images_paths = [];
                $scope.videos_paths = [];
                $scope.loadData();
                $scope.clear2();
            }
            else {
                $scope.submitted2 = true;

            }

        };

        //TO  GEt The Values iN Grid
        $scope.loadData = function () {

            $scope.usercheck = true;
            $scope.usercheck1 = true;
            $scope.files_flag = false;
            $scope.parameter_list = [];
            var pageid = 2;
            //$scope.all_check();
            //$scope.all_check1();
            apiService.getURI("GuestDetails/getloaddata", pageid).
                then(function (promise) {

                    $scope.currentPage = 1;
                    $scope.itemsPerPage = 5;
                    $scope.programlist = promise.programlist;
                    $scope.gridOptions2 = promise.alllist;
                })
        };
  
        //TO clear  data
        $scope.clear1 = function () {
            $scope.COEME_Id = 0;
            $scope.coemE_EventName = "";
            $scope.coemE_EventDesc = "";
            $scope.coemE_SMSMessage = "";
            $scope.coemE_MailSubject = "";
            $scope.coemE_MailHeader = "";
            $scope.coemE_MailFooter = "";
            $scope.coemE_Mail_Message = "";
            $scope.coemE_MailHTMLTemplate = "";
            $scope.submitted1 = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();
            $scope.search = "";
        };

        $scope.clear2 = function () {
            $scope.files_flag = false;
            $scope.PRYR_Id = 0;
            $scope.usercheck = true;
            $scope.usercheck1 = true;
            $scope.all_check();
            $scope.all_check1();

            $scope.asmaY_Id = "";
            $scope.coemE_Id_f = "";

            $scope.evt_flg = true;

            $scope.coeE_EventDesc = "";
            $scope.coeE_MailHTMLTemplate = "";
            $scope.coeE_SMSMessage = "";
            $scope.coeE_MailSubject = "";
            $scope.coeE_MailHeader = "";
            $scope.coeE_MailFooter = "";
            $scope.coeE_Mail_Message = "";
            $scope.coeE_EStartDate = "";

            $scope.coeE_EEndDate = "";
            $scope.reminder_days = "";
            $scope.coeE_ReminderDate = "";
            $scope.coeE_EStartTime = "";
            $scope.coeE_EEndTime = "";
            $scope.email_flag = "No";
            $scope.sms_flag = "No";
            $scope.repeate_flag = "No";
            $scope.coeE_ReminderSchedule = "";
            $scope.stud_chk = "0";
            $scope.stf_chk = "0";
            $scope.oldstud_chk = "0";
            $scope.oth_chk = "0";
            $scope.asmaY_Id_o_s = "";
            $scope.images_temp = [];
            $scope.videos_temp = [];
            $scope.mobilenos_temp = [];
            $scope.Others_list = [];
            $scope.mobilecount = 0;
            $scope.c1 = false;
            $scope.c2 = false;
            $scope.c3 = false;
            $scope.c4 = false;

            $scope.submitted2 = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
        };




        //TO clear  data
        $scope.clearid = function () {
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
            $scope.asmaY_Id = "";
            $scope.TTMSAB_Id = 0;
            $scope.Staff_Id = "";
            $scope.StaffAbbreviation = "";

        };

        //TO  Edit Record
        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.pryR_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("GuestDetails/getdetails", pageid).
                then(function (promise) {
                    $scope.files_flag = true;
                    $scope.pryR_Id = promise.alllist[0].pryR_Id;
                    $scope.gtype = promise.alllist[0].pryrG_GuestType;
                    $scope.gname = promise.alllist[0].pryrG_GuestName;
                    $scope.gno = promise.alllist[0].pryrG_GuestPhoneNo;
                    $scope.emailid = promise.alllist[0].pryrG_GuestEmailId;
                    $scope.gspeech = promise.alllist[0].pryrG_GuestSpeech;
                   
                })
        }
        //TO  delete Record
        $scope.DeletRecord = function (employee, SweetAlert) {
            $scope.editEmployee = employee.pryR_Id;
            var pageid = $scope.editEmployee;
            swal({
                title: "Are you sure",
                text: "Do you want to delete record????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.DeleteURI("GuestDetails/deleterecord", pageid).
                            then(function (promise) {
                                if (promise.returnresult === true) {
                                    swal('Record Deleted Successfully');
                                }
                                else {
                                    swal('Record Not Deleted Successfully!');
                                }
                                
                            })
                        $state.reload();
                    }
                    else {
                        swal("Record Deletion Cancelled");
                        $state.reload();
                    }
                });
        };

        //TO  View Record
        $scope.viewrecordspopup2 = function (employee, SweetAlert) {

            $scope.editEmployee = employee.PRYR_Id;
            var pageid = $scope.editEmployee;

            apiService.getURI("GuestDetails/getalldetailsviewrecords", pageid).
                then(function (promise) {
                    $scope.view_imgs = promise.photolist;
                    $scope.view_videos = promise.videolist;
                  
                })

        };

        //TO clear  popupgrid data
        $scope.clearpopupgrid1 = function () {
            $scope.viewrecordspopupdisplay1 = "";
        };
        $scope.clearpopupgrid2 = function () {
            $scope.viewrecordspopupdisplay2 = "";
        };
        $scope.clearpopupgrid11 = function () {
            $scope.view_imgs = "";
        };
        $scope.clearpopupgrid15 = function () {
            $scope.view_videos = "";
        };
        $scope.clearpopupgrid12 = function () {
            $scope.view_classes = "";
        };
        $scope.clearpopupgrid13 = function () {
            $scope.view_employees = "";
        };
        $scope.clearpopupgrid14 = function () {
            $scope.view_others = "";
        };



        $scope.interacted1 = function (field) {

            return $scope.submitted1;
        };
        $scope.interacted = function (field) {

            return $scope.submitted2;
        };

        $scope.asmaY_Id = "";
  
     
        //MB

        $scope.UploadStudentProfilePic = [];
        $scope.images_temp = [];

        $scope.uploadStudentProfilePic = function (input, document) {

            $scope.UploadStudentProfilePic = input.files;

            for (var a = 0; a < $scope.UploadStudentProfilePic.length; a++) {

                if (input.files && input.files[a]) {

                    if ((input.files[a].type == "image/jpeg" || input.files[a].type == "image/png") && input.files[a].size <= 2097152)  // 2097152 bytes = 2MB || input.files[a].type == "image/png"   video/mp4,video/3gpp
                    {


                        var count = 0;
                        angular.forEach($scope.images_temp, function (imgt123) {
                            if (imgt123.name == input.files[a].name) {
                                count += 1;
                            }
                        });

                        if (count == 0) {
                            $scope.images_temp.push(input.files[a]);
                        }
                        var reader = new FileReader();
                        // var id = input.files[a].name;
                        reader.onload = function (e) {


                            $('#blah')
                                .attr('src', e.target.result)

                        };
                        reader.readAsDataURL(input.files[a]);
                        //$scope.images_temp.push(input.files[a]);
                    }
                    else if (input.files[a].type != "image/jpeg") {
                        swal("Please Upload the JPEG file");
                        return;
                    }
                    else if (input.files[a].size > 2097152) {
                        swal("Image size should be less than 2MB");
                        return;
                    }
                }

            }

        }



        $scope.uploadpdf = [];

        $scope.uploadpdf = function (input, document) {

            $scope.uploadpdf = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type !== "application/pdf")  // 2097152 bytes = 2MB 
                {
                    UploaddianPhoto(document);
                }
                else {
                    swal("Upload PDF, Pdf, PDF Files Only");
                }
            }
        };

        function UploaddianPhoto(data) {
            console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadpdf.length; i++) {
                formData.append("File", $scope.uploadpdf[i]);
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
                    $('#').attr('src', data.lpmtR_Resources);
                    var img = data.lpmtR_Resources;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        //$scope.showGuardianPhoto = function (data) {
        //    $('#preview').attr('src', data.lpmtR_Resources);
        //};



        $scope.remove_img = function (sel_img_del) {

            for (var i = 0; i < $scope.images_temp.length; i++) {

                var imgt123 = $scope.images_temp[i];
                if (imgt123.name == sel_img_del.name) {

                    $scope.images_temp.splice(i, 1);
                }
            }
        }


        $scope.UploadStudentProfileVideo = [];
        $scope.videos_temp = [];

        $scope.uploadStudentProfileVideo = function (input, document) {

            $scope.UploadStudentProfileVideo = input.files;

            for (var a = 0; a < $scope.UploadStudentProfileVideo.length; a++) {

                if (input.files && input.files[a]) {

                    if ((input.files[a].type == "video/mp4") && input.files[a].size <= 27262976)  // 2097152 bytes = 2MB || input.files[a].type == "image/png"   video/mp4,video/3gpp,video/x-ms-wmv 4194304  video/x-ms-wmv 27262976  || input.files[a].type == "video/x-ms-wmv"
                    {

                        var reader = new FileReader();

                        reader.onload = function (e) {

                            $('#blah')
                                .attr('src', e.target.result)
                        };
                        reader.readAsDataURL(input.files[a]);
                        //  Uploadprofile();
                        var count = 0;
                        angular.forEach($scope.videos_temp, function (vid123) {
                            if (vid123.name == input.files[a].name) {
                                count += 1;
                            }
                        });

                        if (count == 0) {
                            $scope.videos_temp.push(input.files[a]);
                        }
                        //$scope.images_temp.push(input.files[a]);
                    }
                    else if (input.files[a].type != "video/mp4") {// && input.files[a].type != "video/x-ms-wmv"
                        swal("Please Upload the MP4 file " + a);
                        return;
                    } else if (input.files[a].size > 27262976) {
                        swal("Video size should be less than 27MB ");
                        return;
                    }
                }

            }
            // $scope.images_temp= $scope.UploadStudentProfilePic ;
        }

        $scope.remove_video = function (sel_video_del) {

            for (var i = 0; i < $scope.videos_temp.length; i++) {

                var vide123 = $scope.videos_temp[i];
                if (vide123.name == sel_video_del.name) {

                    $scope.videos_temp.splice(i, 1);
                }
            }
        }

        $scope.mobilenos_temp = [];
        $scope.mobilecount = 0;
        $scope.Others_list = [];

    

        $scope.clear_others = function () {
            $scope.Oters_Name = "";
            $scope.Oters_Mob_No = "";
            $scope.Oters_Mail_Id = "";
        }

        function Uploadprofile1() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.images_temp.length; i++) {
                formData.append("File", $scope.images_temp[i]);
            }

            formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/OnlineProgramdoc", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    swal(d);
                    $scope.images_paths = d;
                    if ($scope.videos_temp.length > 0) {
                        Uploadprofile2();
                    }
                    else if ($scope.videos_temp.length == 0) {
                        $scope.savemappedeventsdata();
                    }

                    // $scope.obj.image = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
            // Uploads1(miid);
        }


        function Uploadprofile2() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.videos_temp.length; i++) {
                formData.append("File", $scope.videos_temp[i]);
            }

            //We can send more data to server using append         
            formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/OnlineProgramdoc", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    swal(d);
                    $scope.videos_paths = d;
                    // if ($scope.videos_temp.length > 0) {
                    $scope.savemappedeventsdata();
                    // }


                    // $scope.obj.image = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
            // Uploads1(miid);
        }

        $scope.submit = function () {

            if ($scope.myForm.$valid) {
                if ($scope.images_temp.length == 0 && $scope.videos_temp.length == 0) {
                    $scope.savemappedeventsdata();
                }
                else if (($scope.images_temp.length != 0 && $scope.videos_temp.length != 0) || $scope.images_temp.length != 0) {
                    Uploadprofile1();


                }

                else if ($scope.images_temp.length == 0 && $scope.videos_temp.length != 0) {

                    Uploadprofile2();
                    // $scope.savemappedeventsdata();
                }
            }
            else {
                $scope.submitted2 = true;

            }
        }
    }

    angular
        .module('app').filter("trustUrl", function ($sce) {
            return function (Url) {
                return $sce.trustAsResourceUrl(Url);
            };
        });

    angular
        .module('app').directive('txtArea', function () {
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