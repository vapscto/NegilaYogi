(function () {
    'use strict';
    angular
        .module('app')
        .controller('CLGIVRM_GalleryController', CLGIVRM_GalleryController);

    CLGIVRM_GalleryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', '$http', '$q', 'apiService', '$stateParams', '$filter', 'superCache', '$window', '$interval', '$sce'];
    function CLGIVRM_GalleryController($rootScope, $scope, $state, $location, dashboardService, Flash, $http, $q, apiService, $stateParams, $filter, superCache, $window, $interval, $sce) {

        $scope.coverflg = false;
        var date = new Date();
        $scope.igA_Date = date;
        $scope.igA_Time = date;
        //===================Page Load
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("Clg_IVRM_Gallery/getloaddata", pageid).
                then(function (promise) {
                    $scope.courselist = promise.courselist;
                    $scope.coursename = $scope.courselist[0].amcO_CourseName;
                    $scope.branchname = $scope.courselist[0].amB_BranchName;
                    $scope.roleflg = promise.roleflg;
                    $scope.get_galleryimg = promise.get_galleryimg;
                    $scope.presentCountgrid = $scope.get_galleryimg.length;
                });
        };
        //================================ Branch Selection
        $scope.coursechange = function () {
            var data = {
                "AMCO_Id": $scope.amcO_Id
            };
            apiService.create("Clg_IVRM_Gallery/get_branch", data).then(function (promise) {
                $scope.branchlist = promise.branchlist;
            });
        };
        $scope.branchchange = function () {
            var data = {
                "AMB_Id": $scope.amB_Id
            };
            apiService.create("Clg_IVRM_Gallery/get_semester", data).then(function (promise) {
                $scope.semesterlist = promise.semesterlist;
            });
        };
        $scope.semesterchange = function () {
            var data = {
                "AMSE_Id": $scope.amsE_Id
            };
            apiService.create("Clg_IVRM_Gallery/get_Section", data).then(function (promise) {
                $scope.sectionlist = promise.sectionlist;
            });
        };
        //================================ Section Selection
        $scope.mediachange = function () {
            $scope.images_paths = [];
            $scope.stepsModel = [];
            $scope.filenames = "Video";
            $scope.fileflg = false;
        };
        //================================ Image  
        $scope.stepsModel = [];
        $scope.imageUpload = function (event) {
            $scope.files = event.files;
            for (var i = 0; i < $scope.files.length; i++) {
                var file = $scope.files[i];
                $scope.fileimg = file;
                var reader = new FileReader();
                reader.onload = $scope.imageIsLoaded;
                reader.readAsDataURL(file);
            }
        };
        $scope.imageIsLoaded = function (e) {
            $scope.$apply(function () {
                $scope.stepsModel.push(e.target.result);
            });
        };
        $scope.remove_img = function (reimg) {
            for (var i = 0; i < $scope.files.length; i++) {
                var imgt1 = $scope.files[i];
                if (imgt1.name === reimg.name) {
                    $scope.stepsModel.splice(i, 1);
                }
            }
        };
        //================================ Video 
        $scope.videoUpload = function (input, document) {
            $scope.files = input.files;
            if (input.files && input.files[0]) {
                if (input.files[0].type === "video/mp4" || input.files[0].type === "video/x-ms-wmv" && input.files[0].size <= 2097152) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#blahD')
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadImgs();
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }
            }
        };
        //================================ Upload       
        $scope.view_videos = [];
        function UploadImgs() {
            var formData = new FormData();
            for (var i = 0; i <= $scope.files.length; i++) {
                formData.append("File", $scope.files[i]);
                $scope.filenames = "Videos";
                $scope.fileflg = true;
            }
            formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Upload_GalleryImgVideos", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    swal(d);
                    $scope.images_paths = d;
                    if ($scope.images_paths.length > 0) {
                        if ($scope.mediatype === "V") {
                            angular.forEach($scope.images_paths, function (imgp) {
                                $scope.view_videos.push({ id: 1, video_path: imgp });
                            });
                        }
                        if ($scope.mediatype === "I") {
                            $scope.savedata();
                        }
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

        }

        //=============================== Preview Image
        $scope.previewimg = function (img) {
            $scope.imagepreview = img;
            $('#preview').attr('src', $scope.imagepreview);
            $('#myModalPreview').modal('show');
        };

        $scope.checkCover = function (ig) {
            $scope.imgcover = "";
            $scope.imgcover = ig.coverid;
        };
        //================================ Set as Cover 
        $scope.getmodeldetails = function (img) {
            var data = {
                "IGA_Id": img
            };
            apiService.create("Clg_IVRM_Gallery/getcovermodel", data).then(function (promise) {
                $scope.covermodel = promise.covermodel;
                $('#myModalCover').modal('show');
            });
        };

        //=============================== Preview Video       
        $scope.viewVideo = function (videoRes) {
            $scope.view_videos = [];
            $scope.videoSources = [];
            $scope.movie = { src: videoRes };
            $scope.movie1 = { src: videoRes };
            $scope.view_videos.push({ id: 1, file_Videos: videoRes });
            console.log($scope.movie);
        };

        //================================== Select Class
        $scope.all_checkCls = function (studt) {
            $scope.usercheckCls = studt;
            var toggleStatus = $scope.usercheckCls;
            angular.forEach($scope.sectionlist, function (cl) {
                cl.clsck = toggleStatus;
            });
        };
        $scope.onselect = function () {
            $scope.usercheckCls = $scope.sectionlist.every(function (options) {
                return options.clsck = toggleStatus;
            });
        };
        $scope.isOptionsRequiredCls = function () {
            return !$scope.sectionlist.some(function (options) {
                return options.clsck;
            });
        };
        //================================== Save  
        $scope.uploaddata = function () {
            if ($scope.myForm.$valid) {
                if ($scope.mediatype === "I") {
                    UploadImgs();
                }
                else {
                    $scope.savedata();
                }
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                if ($scope.images_paths === null || $scope.images_paths === undefined || $scope.images_paths === "") {
                    $scope.images_paths = [];
                }
                $scope.arraySection = [];
                if ($scope.typeflag === 'CO') {
                    $scope.commonflag = true;
                    $scope.arraySection = [];
                    $scope.AMCO_Id = $scope.amcO_Id;
                    $scope.AMB_Id = $scope.amB_Id;
                }
                else {
                    $scope.commonflag = false;
                    angular.forEach($scope.sectionlist, function (cls) {
                        if (cls.clsck === true) {
                            $scope.arraySection.push(cls);
                        }
                    });
                }
                var data = {
                    "IGA_Id": $scope.igA_Id,
                    "IGA_GalleryName": $scope.igA_GalleryName,
                    "IGA_Date": $scope.igA_Date,
                    "IGA_CommonGalleryFlg": $scope.commonflag,
                    "AMCO_Id": $scope.amcO_Id,
                    "AMB_Id": $scope.amB_Id,
                    "AMSE_Id": $scope.amsE_Id,
                    "arraySection": $scope.arraySection,
                    "mediatype": $scope.mediatype,
                    "IGA_Time": $filter('date')($scope.igA_Time, "h:mm a"),
                    images_list: $scope.images_paths
                };
                apiService.create("Clg_IVRM_Gallery/savedata", data).then(function (promise) {
                    if ($scope.mediatype === "I") {
                        $scope.covermodel(promise.igaId);
                    }
                    else {
                        if (promise.returnval === true) {
                            swal('Record Saved successfully');
                            $state.reload();
                        }
                        else {
                            swal('Failed to Upload, please contact administrator');
                        }
                    }
                });
                $scope.images_paths = [];
                $scope.videos_paths = [];
            }
            else {
                $scope.submitted = true;
            }
        };

        //============================== Save Set cover Image
        $scope.savecover = function () {
            if ($scope.imgcover === "" || $scope.imgcover === null || $scope.imgcover === 0 || $scope.imgcover === undefined) {
                swal("Select any Image for set Cover image....!!");
            }
            else {
                var data = {
                    "IGAP_Id": $scope.imgcover
                };
            }
            apiService.create("Clg_IVRM_Gallery/savecover", data).then(function (promise) {
                if (promise.returnval === true) {
                    swal('Record Saved successfully');
                    $('.modal-backdrop').remove();
                    $state.reload();
                }
                else if (promise.returnduplicatestatus === 'Duplicate') {
                    swal('Record already exist');
                }
                else {
                    swal('Failed to Upload, please contact administrator');
                }
                angular.element('#myModalCover').model('hide');
            });
        };

        $scope.deactive = function (item) {
            $scope.IGA_Id = item.IGA_Id;
            var dystring = "";
            if (item.IGA_ActiveFlag === true) {
                dystring = "Deactivate";
            }
            else if (item.IGA_ActiveFlag === false) {
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
                        apiService.create("Clg_IVRM_Gallery/deactive", item).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            });
                    }
                    else {
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        };

        $scope.cleardata = function () {
            $state.reload();
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };

    }

    angular.module('app').filter("trustUrl", function ($sce) {
        return function (Url) { return $sce.trustAsResourceUrl(Url); };
    });
})();