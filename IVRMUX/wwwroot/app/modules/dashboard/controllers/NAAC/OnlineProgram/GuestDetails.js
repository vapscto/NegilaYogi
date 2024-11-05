
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
        $scope.studentGuardianDetails = {};
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
                    "PRYRG_Id": $scope.PRYRG_Id,
                    "PRYR_Id": $scope.pryR_Id,
                   // images_list: $scope.images_paths,
                  //  videos_list: $scope.videos_paths,
                    "PRYRG_GuestType": $scope.gtype,
                    "PRYRG_GuestName": $scope.gname,
                    "PRYRG_GuestPhoneNo": $scope.gno,
                    "PRYRG_GuestEmailId": $scope.emailid,
                    "PRYRG_GuestProfile": $scope.gprofile,
                    "PRYRG_GuestSpeech": $scope.gspeech,
                    "PDFfile": $scope.studentGuardianDetails[0].lpmtR_Resources,
                    "Imagefile": $scope.studentGuardianDetailsimg[0].lpmtR_Resources,
                    "Videofile": $scope.studentGuardianvideo[0].lpmtR_Resources,
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
            $state.reload();
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
                    $scope.PRYRG_Id = promise.alllist[0].pryrG_Id;
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
                       
                    }
                    else {
                        swal("Record Deletion Cancelled");
                        $state.reload();
                    }
                    $scope.loadData();
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

    

        // For File Upload

        $scope.studentGuardianDetails = [{ id: 'Guardian1' }];

        $scope.studentGuardianDetailsimg = [{ id: 'Guardianimg1' }];

        $scope.studentGuardianvideo = [{ id: 'Guardianvideo1' }];
        // PDF
        $scope.UploadGuardianPDF = [];

        $scope.UploaddianPhoto1 = function (input, document) {

            $scope.UploadGuardianPDF = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type === "application/pdf")  // 2097152 bytes = 2MB 
                {
                    UploaddianPhoto(document);
                }
               else {
                    swal("Upload MP4, Pdf, Image Files Only");
                }

            }
        };

        function UploaddianPhoto(data) {
            console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.UploadGuardianPDF.length; i++) {
                formData.append("File", $scope.UploadGuardianPDF[i]);
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
                    var pdf = data.lpmtR_Resources;
                    var pdfarr = pdf.split('.');
                    var lastelement = pdfarr[pdfarr.length - 1];
                    data.filetype = lastelement;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }


        //image

        $scope.UploadGuardianimage = [];

        $scope.uploadimage = function (input, document) {

            $scope.UploadGuardianimage = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type === "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    Uploaddianimage(document);
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                } else {
                    swal("Upload Image Files Only");
                }

            }
        };

        function Uploaddianimage(data) {
            console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.UploadGuardianimage.length; i++) {
                formData.append("File", $scope.UploadGuardianimage[i]);
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
                    var pdf = data.lpmtR_Resources;
                    var pdfarr = pdf.split('.');
                    var lastelement = pdfarr[pdfarr.length - 1];
                    data.filetype = lastelement;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }



        //video

        $scope.UploadGuardianvideo = [];

        $scope.uploadGuardianvideo = function (input, document) {

            $scope.UploadGuardianvideo = input.files;

            if (input.files && input.files[0]) {

                //if (input.files[0].type === "video/mp4/3gp" && input.files[0].size <= 5097152)  // 2097152 bytes = 2MB 
                //{
                    Uploaddianvideo(document);
                //}
                //else if (input.files[0].size > 5097152) {
                //    swal("Video size should be less than 2MB");
                //    return;
                //} else {
                //    swal("Upload MP4 Files Only");
                //}

            }
        };

        function Uploaddianvideo(data) {
            console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.UploadGuardianvideo.length; i++) {
                formData.append("File", $scope.UploadGuardianvideo[i]);
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
                    var pdf = data.lpmtR_Resources;
                    var pdfarr = pdf.split('.');
                    var lastelement = pdfarr[pdfarr.length - 1];
                    data.filetype = lastelement;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        
        //Video

        $scope.showGuardianPhoto = function (data) {
            $('#preview').attr('src', data.lpmtR_Resources);
        };


        $scope.showGuardianPhotonew = function (data) {
            $scope.videoSources = [];
            $scope.preview1 = data.lpmtR_Resources;
            $scope.videdfd = data.lpmtR_Resources;
            $scope.movie = { src: data.lpmtR_Resources };
            $scope.movie1 = { src: data.lpmtR_Resources };
            console.log($scope.movie);
        };

        $scope.showGuardianPhoto1 = function (data) {
            $('#preview').attr('src', data.pryrG_GuestPhoto);
        };

        $scope.showGuardianPhotonew1 = function (data) {
            $scope.videoSources = [];
            $scope.preview1 = data.pryrG_GuestVideo;
            $scope.videdfd = data.pryrG_GuestVideo;
            $scope.movie = { src: data.pryrG_GuestVideo };
            $scope.movie1 = { src: data.pryrG_GuestVideo };
            console.log($scope.movie);
        };

        $scope.mobilenos_temp = [];
        $scope.mobilecount = 0;
        $scope.Others_list = [];

    

        $scope.clear_others = function () {
            $scope.Oters_Name = "";
            $scope.Oters_Mob_No = "";
            $scope.Oters_Mail_Id = "";
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