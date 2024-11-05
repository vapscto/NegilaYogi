

(function () {
    'use strict';
    angular
        .module('app')
        .controller('alumni_GalleryController', alumni_GalleryController);

    alumni_GalleryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', '$http', '$q', 'apiService', '$stateParams', '$filter', 'superCache', '$window', '$interval', '$sce'];
    function alumni_GalleryController($rootScope, $scope, $state, $location, dashboardService, Flash, $http, $q, apiService, $stateParams, $filter, superCache, $window, $interval, $sce) {


        $scope.coverflg = false;
        $scope.searchValue = '';
        var date = new Date();
        $scope.igA_Date = date;
        $scope.igA_Time = date;
        //===================Page Load
        $scope.loaddata = function () {
            $scope.pview = false;
            var pageid = 2;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            apiService.getURI("Alumni_Gallery/getloaddata", pageid).
                then(function (promise) {

                    $scope.roleflg = promise.roleflg;
                    $scope.get_galleryimg = promise.get_galleryimg;
                    $scope.presentCountgrid = $scope.get_galleryimg.length;
                });
        };

        //================================ Section Selection
        $scope.mediachange = function () {
            $scope.documentListOtherDetails = [{ id: 'document' }];
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
                if (input.files[0].type === "video/mp4" || input.files[0].type === "video/x-ms-wmv" && input.files[0].size <= 31457280) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#blahD')
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadImgs();
                }
                else if (input.files[0].size > 31457280) {
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
            $http.post("/api/ImageUpload/Alumni_Gallery", formData,
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
        //============================
        $scope.setcover = function (ww) {
            var data = {
                "IGA_Id": ww.IGA_Id,
                "IGAP_Id": ww.IGAP_Id
            }
            apiService.create("Alumni_Gallery/savecover", data).then(function (promise) {
                if (promise.returnval === true) {
                    swal('Cover Photo Set Successfully.');
                    $scope.getmodeldetails($scope.imgnew);
                }
                else {
                    swal('Cover Photo Not Set Successfully!!!');
                }
            })

        }
        //=============================== Preview Image
        $scope.previewimg = function (img) {
            $scope.imagepreview = img;
            $('#preview').attr('src', $scope.imagepreview);
            $('#myModalPreview').modal('show');
        };
        $scope.previewimgnew = function (img) {
            $scope.slides = [];
            $scope.myInterval = 3000;
            $scope.slides = $scope.view_galleryimg;
            // $scope.pview = true;

            $('#slides_control').modal('show');
            // $('#myModalPreviewnew').modal('show');
        };

        $scope.checkCover = function (ig) {
            $scope.imgcover = "";
            $scope.imgcover = ig.coverid;
        };
        //================================ Set as Cover 
        $scope.getmodeldetails = function (img) {
            $scope.imgnew = "";
            $scope.imgnew = img;
            var data = {
                "ALGA_Id": img
            };
            apiService.create("Alumni_Gallery/getcovermodel", data).then(function (promise) {
                $scope.view_galleryimg = promise.view_galleryimg;
                //$scope.pview = true;
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
                $scope.images_paths = [];
                
                if ($scope.documentListOtherDetails != null) {
                    angular.forEach($scope.documentListOtherDetails, function (qq) {
                        if (qq.FilePath != null) {
                            $scope.images_paths.push({ FilePath: qq.FilePath, FileName: qq.FileName });
                        }
                    })
                    
                }

                var data = {
                    "ALGA_Id": $scope.ALGA_Id,
                    "ALGA_GalleryName": $scope.ALGA_GalleryName,
                    "ALGA_Date": $scope.ALGA_Date,
                    "ALGA_CommonGalleryFlg": "Alumni",

                    "mediatype": $scope.mediatype,
                    "ALGA_Time": $filter('date')($scope.igA_Time, "h:mm a"),
                    images_list: $scope.images_paths
                };
                apiService.create("Alumni_Gallery/savedata", data).then(function (promise) {

                    if (promise.returnval === true) {
                        swal('Record Saved successfully');
                        $state.reload();
                    }
                    else {
                        swal('Failed to Upload, please contact administrator');
                        $state.reload();
                    }
                });


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
            apiService.create("IVRM_Gallery/savecover", data).then(function (promise) {
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
                        apiService.create("Alumni_Gallery/deactive", item).
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


        //===============================
        
        //===========================ADD==================================
        $scope.documentListOtherDetails = [{ id: 'document' }];
        $scope.addNewDocumentOtherDetail = function () {
            var newItemNo = $scope.documentListOtherDetails.length + 1;
            if (newItemNo <= 30) {
                $scope.documentListOtherDetails.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewDocumentOtherDetail = function (index, data) {
            var newItemNo = $scope.documentListOtherDetails.length - 1;
            $scope.documentListOtherDetails.splice(index, 1);
            if (data.hreothdeT_Id > 0) {
                $scope.DeleteDocumentDataOthers(data);
            }
        };

        $scope.selectFileforUploadzdOtherDetail = function (input, document) {


            $scope.ldr = true;
            //$('#' + document.id).removeAttr('src');
            $scope.SelectedFileForUploadzdOtherDetail = input.files;
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#blahD')
                        .attr('src', e.target.result);
                };
                reader.readAsDataURL(input.files[0]);
                UploadEmployeeDocumentOtherDetail(document);

            }
        };
        function UploadEmployeeDocumentOtherDetail(data) {
            // console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzdOtherDetail.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdOtherDetail[i]);
            }
            // We can send more data to server using append         
            //formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Alumni_Gallery", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (d !== "PathNotFound") {

                        data.FilePath = d[0].path;
                        data.FileName = d[0].name;
                        $scope.ldr = false;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.previewimg_new = function (img) {
            $scope.imagepreview = img;
            $scope.view_videos1 = [];
            var img = $scope.imagepreview;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                $scope.filetype2 = lastelement;
            }
            if ($scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp4') {

                $scope.view_videos1.push({ id: 1, ihw_video: img });
                $('#myvideoPreview').modal('show');

            }
            else if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {

                $('#preview1').attr('src', $scope.imagepreview);
                $('#myimagePreview').modal('show');

            }
            else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                $window.open($scope.imagepreview)
            }
            else if ($scope.filetype2 == 'mp3') {
                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myaudioPreview').modal('show');

            }
            else if ($scope.filetype2 == 'pdf') {

                ///=====================show pdf, img

                $('#showpdf').modal('hide');
                var imagedownload1 = "";
                imagedownload1 = $scope.imagepreview;


                $http.get(imagedownload1, { responseType: 'arraybuffer' })
                    .success(function (response) {
                        var fileURL = "";
                        var file = "";
                        var embed = "";
                        var pdfId = "";
                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);

                        pdfId = document.getElementById("pdfIdzz");
                        pdfId.removeChild(pdfId.childNodes[0]);
                        embed = document.createElement('embed');
                        embed.setAttribute('src', fileURL);
                        embed.setAttribute('type', 'application/pdf');
                        embed.setAttribute('width', '100%');
                        embed.setAttribute('height', '1000');
                        pdfId.appendChild(embed);
                        $('#showpdf').modal('show');



                    });



            }
            else {
                $window.open($scope.imagepreview)
            }

        };
        //==============================

     


        $scope.currentIndex = 0; // Initially the index is at the first image

        $scope.next = function () {
            $scope.currentIndex < $scope.slides.length - 1 ? $scope.currentIndex++ : $scope.currentIndex = 0;
        };

        $scope.prev = function () {
            $scope.currentIndex > 0 ? $scope.currentIndex-- : $scope.currentIndex = $scope.slides.length - 1;
        };
        $scope.$watch('currentIndex', function () {
            $scope.slides.forEach(function (image) {
                image.visible = false; // make every image invisible
            });

            $scope.slides[$scope.currentIndex].visible = true; // make the current image visible
        });

        //===========================================
    }
    
    angular.module('app').filter("trustUrl", function ($sce) {
        return function (Url) { return $sce.trustAsResourceUrl(Url); };
    });
})();