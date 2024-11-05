

(function () {
    'use strict';
    angular
        .module('app')
        .controller('IVRM_GalleryController', IVRM_GalleryController);

    IVRM_GalleryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', '$http', '$q', 'apiService', '$stateParams', '$filter', 'superCache', '$window', '$interval', '$sce'];
    function IVRM_GalleryController($rootScope, $scope, $state, $location, dashboardService, Flash, $http, $q, apiService, $stateParams, $filter, superCache, $window, $interval, $sce) {


        $scope.coverflg = false;
        var date = new Date();
        $scope.igA_Date = date;
        $scope.igA_Time = date;
        //===================Page Load
        $scope.loaddata = function () {
            var pageid = 2;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            apiService.getURI("IVRM_Gallery/getloaddata", pageid).
                then(function (promise) {
                    $scope.classlist = promise.classlist;
                    $scope.clasname = $scope.classlist[0].asmcL_ClassName;
                    $scope.secname = $scope.classlist[0].asmC_SectionName;
                    $scope.roleflg = promise.roleflg;
                    $scope.get_galleryimg = promise.get_galleryimg;
                    $scope.presentCountgrid = $scope.get_galleryimg.length;
                });
        };
        //================================ Section Selection
        //$scope.classchange = function () {


        //    var data = {
        //        "ASMCL_Id": $scope.asmcL_Id
        //    };
        //    apiService.create("IVRM_Gallery/get_section", data).then(function (promise) {
        //        $scope.sectionlist = promise.sectionlist;
        //    });
        //};


        $scope.classchange = function () {
            $scope.usercheckCls1 = $scope.classlist.every(function (options) {
                return options.clsck1;
            });

            $scope.classlistarray = [];
            angular.forEach($scope.classlist, function (aa) {
                if (aa.clsck1 == true) {
                    $scope.classlistarray.push({ ASMCL_Id: aa.asmcL_Id });
                }
            });
            var data = {
                "classlst": $scope.classlistarray
            }

            apiService.create("IVRM_Gallery/get_section", data).then(function (promise) {
                if (promise.sectionlist.length > 0 || promise.sectionlist != null) {
                    $scope.sectionlist = promise.sectionlist;
                }
                else {
                    swal('No data Found!!!');
                }
            });
        }
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
        //============================
        $scope.setcover = function (ww) {
            var data = {
                "IGA_Id": ww.IGA_Id,
                "IGAP_Id": ww.IGAP_Id
            }
            apiService.create("IVRM_Gallery/savecover", data).then(function (promise) {
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

        $scope.checkCover = function (ig) {
            $scope.imgcover = "";
            $scope.imgcover = ig.coverid;
        };
        //================================ Set as Cover 
        $scope.getmodeldetails = function (img) {
            $scope.imgnew = "";
            $scope.imgnew = img;
            var data = {
                "IGA_Id": img
            };
            apiService.create("IVRM_Gallery/getcovermodel", data).then(function (promise) {
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

        $scope.all_checkCls1 = function (studt) {
            $scope.usercheckCls1 = studt;
            var toggleStatus = $scope.usercheckCls1;
            angular.forEach($scope.classlist, function (cll) {
                cll.clsck1 = toggleStatus;
            });
            $scope.classchange();
        };
        $scope.onselect = function () {
            $scope.usercheckCls = $scope.sectionlist.every(function (options) {
                return options.clsck;
            });
        };
        //$scope.onselect1 = function () {
        //    $scope.usercheckCls1 = $scope.classlist.every(function (options) {
        //        return options.clsck1 = toggleStatus;
        //    });
        //};

        $scope.isOptionsRequiredCls = function () {
            return !$scope.sectionlist.some(function (options) {
                return options.clsck;
            });
        };
        $scope.isOptionsRequiredCls1 = function () {
            return !$scope.classlist.some(function (item) {
                return item.clsck1;
            });
        };


        //================================== Save  
        //$scope.uploaddata = function () {
        //    if ($scope.myForm.$valid) {
        //        if ($scope.mediatype === "I") {
        //            UploadImgs();
        //        }
        //        else {
        //            $scope.savedata();
        //        }
        //    }
        //    else {
        //        $scope.submitted = true;
        //    }
        //};

        //files
        $scope.savedata = function () {
            $scope.submitted = true;
            //var formData = new FormData();
            //for (var i = 0; i <= $scope.files.length; i++) {
            //    formData.append("File", $scope.files[i]);
            //    $scope.filenames = "Videos";
            //    $scope.fileflg = true;
            //}

          //  if ($scope.myForm.$valid) {
                if ($scope.images_paths === null || $scope.images_paths === undefined || $scope.images_paths === "") {
                    $scope.images_paths = [];
                }
                $scope.arraySection = [];
                if ($scope.typeflag === 'CO') {
                    $scope.commonflag = true;
                    $scope.arraySection = [];

                    angular.forEach($scope.classlist, function (cls) {
                        var sectionInfo = {
                            ASMS_Id: cls.asmS_Id,      
                            ASMCL_Id: cls.asmcL_Id   
                        };
                        $scope.arraySection.push(sectionInfo);
                    });
                }
                else {
                    $scope.commonflag = false;
                    angular.forEach($scope.sectionlist, function (cls) {
                        if (cls.clsck === true) {
                            $scope.arraySection.push(cls);
                        }
                    });
                }
                $scope.filepath = [];
                angular.forEach($scope.documentListOtherDetails, function (cls) {
                    $scope.filepath.push(cls.INTBFL_FilePath);
                });
                if ($scope.documentListOtherDetails != null && $scope.documentListOtherDetails.length > 0) {

                }
                var data = {
                    "IGA_Id": $scope.igA_Id,
                    "IGA_GalleryName": $scope.igA_GalleryName,
                    "IGA_Date": $scope.igA_Date,
                    "IGA_CommonGalleryFlg": $scope.commonflag,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "arraySection": $scope.arraySection,
                    "mediatype": $scope.mediatype,
                    "IGA_Time": $filter('date')($scope.igA_Time, "h:mm a"),
                    images_list: $scope.filepath
                };
                apiService.create("IVRM_Gallery/savedata", data).then(function (promise) {

                    if (promise.returnval === true) {
                        swal('Record Saved successfully');
                        $state.reload();
                    }
                    else {
                        swal('Failed to Upload, please contact administrator');
                        $state.reload();
                    }
                });


        //   }
        //   else {
        //       $scope.submitted = true;
        //    }
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
                        apiService.create("IVRM_Gallery/deactive", item).
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
        $scope.edit = function (item) {
            var data = {
                "IGA_Id": item.IGA_Id
            }
            apiService.create("IVRM_Gallery/Editdetails", data).then(function (promise) {
                if (promise.editdata != null && promise.editdata.length > 0) {
                    $scope.editdata = promise.editdata;
                    $scope.igA_GalleryName = $scope.editdata[0].igA_GalleryName;
                    $scope.igA_Date = new Date(promise.editdata[0].igA_Date);
                    $scope.Images = $scope.editdata[0].igA_Id;

                    for (var i = 0; i < $scope.classlist.length; i++) {
                        if ($scope.classlist[i].asmcL_Id == promise.editclass[0].asmcL_Id) {
                            $scope.classlist[i].Selected = true;
                            $scope.classlist.push($scope.classlist[i]);

                        }
                        else {
                            $scope.arrlist4[i].Selected = false;
                        }
                    }

                    $scope.documentListOtherDetails = [];
                    if (promise.attachementlist != null && promise.attachementlist.length > 0) {
                        angular.forEach(promise.attachementlist, function (aa) {
                            $scope.img = aa.intbfL_FilePath;
                            if ($scope.img != null) {
                                var imagarr = $scope.img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                $scope.filetype2 = lastelement;
                            }

                            if ($scope.filetype2 == 'mp4' || $scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp3'
                                || $scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png' ||
                                $scope.filetype2 == 'pdf' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx'
                                || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx' || $scope.filetype2 == 'ppsx'
                                || $scope.filetype2 == 'doc' || $scope.filetype2 == 'docx') {
                                $scope.documentListOtherDetails.push({
                                    id: 1, INTBFL_FilePath: aa.intbfL_FilePath,
                                    FileName: aa.intbfL_FileName
                                });
                            }
                            else {
                                $scope.urldocumentlist.push({
                                    id: 1, INTBFL_FilePath: aa.intbfL_FilePath,
                                    FileName: aa.intbfL_FileName
                                });

                            }
                        });
                    }
                    else {
                        $scope.documentListOtherDetails.push({
                            id: 1, INTBFL_FilePath: '',
                            FileName: ''
                        });
                        //$scope.urldocumentlist.push({
                        //    id: 1, INTBFL_FilePath: '',
                        //    FileName: ''
                        //});
                    }

                    if ($scope.urldocumentlist.length > 0) {
                        $scope.checklink = true;
                    } else {
                        $scope.urldocumentlist = [{ id: 'document' }];
                    }


                }
            })
        }



        //
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

        //======================= file upload
        $scope.SelectedFileForUploadzdOtherDetail = [];
        $scope.selectFileforUploadzdOtherDetail = function (input, document) {
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
            var formData = new FormData();
            for (var i = 0; i <= $scope.selectFileforUploadzdOtherDetail.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdOtherDetail[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/NoticeUpload", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (d !== "PathNotFound") {

                        data.INTBFL_FilePath = d[0].path;
                        data.FileName = d[0].name;

                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }









    }

    angular.module('app').filter("trustUrl", function ($sce) {
        return function (Url) { return $sce.trustAsResourceUrl(Url); };
    });



})();