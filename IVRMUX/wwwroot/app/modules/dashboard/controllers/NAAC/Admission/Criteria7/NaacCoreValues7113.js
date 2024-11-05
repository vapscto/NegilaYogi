
(function () {
    'use strict';
    angular.module('app').controller('NaacCoreValues7113', NaacCoreValues7113);

    NaacCoreValues7113.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter', '$sce', 'myFactorynaac'];

    function NaacCoreValues7113($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter, $sce, myFactorynaac) {
        //======================for pagination
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.mI_Id = 0;
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.search = "";

        var miid = myFactorynaac.get();
        if (miid > 0) {
            $scope.mI_Id = miid;
        } else {
            $scope.mI_Id = 0;
        }
        ///======================================Load Data.
        $scope.loaddata = function () {
         
            var id = 2;
            apiService.getURI("NaacCoreValues7113/loaddata", id).then(function (promise) {
                $scope.institutionlist = promise.institutionlist;
                if (promise.institutionlist.length == 1) {
                    $scope.mI_Id = promise.institutionlist[0].mI_Id;
                    $scope.getData($scope.mI_Id);
                }
            })
        }

        $scope.getData = function (mI_Id) {
            $scope.equityid = 0;
            var id = mI_Id;
            apiService.getURI("NaacCoreValues7113/getData", id).then(function (promise) {
                $scope.allacademicyear = promise.allacademicyear;
                $scope.alldata = promise.alldatalist;
            });
        };

        //=====================Sorting
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        //==============================save data For Tab1

        $scope.savedatatab1 = function () {
       
            $scope.studentlstdata = [];
            if ($scope.myForm.$valid) {
                $scope.materaldocuupload22 = [];
                if ($scope.materaldocuupload.length > 0) {
                    angular.forEach($scope.materaldocuupload, function (val) {
                        if (val.ncaC7113CORVALF_FilePath !== null && val.ncaC7113CORVALF_FilePath !== undefined) {
                            $scope.materaldocuupload22.push(val);
                        }
                    });
                }

                if ($scope.mI_Id == undefined) { $scope.mI_Id = $scope.mI_Id; }

                var data = {
                    "MI_Id": $scope.mI_Id,
                    "NCAC7113CORVAL_Id": $scope.NCAC7113CORVAL_Id,
                    "ASMAY_Id": $scope.NCAC7113CORVAL_Year,
                    "NCAC7113CORVAL_URL": $scope.NCAC7113CORVAL_URL,
                    filelist: $scope.materaldocuupload22
                };
                apiService.create("NaacCoreValues7113/save", data).then(function (promise) {
                    if (promise.duplicate != null && promise.returnval != null) {
                        if (promise.duplicate == false) {
                            if (promise.returnval == true) {
                                swal('Data Saved/Updated Successfully!');
                                $state.reload();
                            }
                            else {
                                swal('Data Not Updated Successfully!');
                            }
                        }
                        else {
                            swal('Record Already Exists!');
                        }
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        //==========================edit data for tab1
        $scope.instit = false;
        $scope.edittab1 = function (data) {

            apiService.create("NaacCoreValues7113/EditData", data).then(function (promise) {
                $scope.instit = true;
                $scope.materaldocuupload = [{ id: 'Materal1' }];
                $scope.NCAC7113CORVAL_URL = promise.editlisttab1[0].ncaC7113CORVAL_URL;
                $scope.NCAC7113CORVAL_Year = promise.editlisttab1[0].ncaC7113CORVAL_Year;
                $scope.NCAC7113CORVAL_Id = promise.editlisttab1[0].ncaC7113CORVAL_Id;
                $scope.materaldocuupload = promise.editfilelist;

                if (promise.editfilelist.length == 0) {
                    $scope.materaldocuupload = [{ id: 'Materal1' }];
                }

                if (promise.editfilelist != null && promise.editfilelist.length > 0) {
                    $scope.materaldocuupload = promise.editfilelist;
                    angular.forEach($scope.materaldocuupload, function (ddd) {
                        var img = ddd.ncaC7113CORVALF_FilePath;
                        var imagarr = img.split('.');
                        var lastelement = imagarr[imagarr.length - 1];
                        ddd.filetype = lastelement;
                        if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                            ddd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + ddd.ncaC7113CORVALF_FilePath;
                        }
                    });
                }
            });
        };

        //===========deactive and active for Tab1
        $scope.deactivYTab1 = function (usersem, SweetAlert) {
            $scope.NCAC7113CORVAL_Id = usersem.ncaC7113CORVAL_Id
            var dystring = "";
            if (usersem.ncaC7113CORVAL_ActiveFlg == true) {
                dystring = "Deactivated";
            }
            else if (usersem.ncaC7113CORVAL_ActiveFlg == false) {
                dystring = "Activated";
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
                        apiService.create("NaacCoreValues7113/deactivate", usersem).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + " Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + " Successfully!!!");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        }

        //For Cancel data record 
        $scope.Clearid = function () {
            $state.reload();
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        // for comments

        $scope.getorganizationdata = function (obj) {


            apiService.create("NaacCoreValues7113/getcomment", obj).then(function (promise) {

                if (promise !== null) {


                    $scope.commentlist = promise.commentlist;
                }
            });
        };
        // for file shilpa
        $scope.getorganizationdata1 = function (obj) {


            apiService.create("NaacCoreValues7113/getfilecomment", obj).then(function (promise) {

                if (promise !== null) {


                    $scope.commentlist1 = promise.commentlist1;

                }
            });
        };

        $scope.addcomments = function (obje) {

            $scope.ccc = obje.ncaC7113CORVAL_Id;
            $scope.generalcomments = '';
            $scope.obj.generalcomments = $scope.generalcomments;
            var bb;
        };

        // for file comment
        $scope.addcomments1 = function (obje) {


            //obj.ncaC7112CODCON_Id = obje.ncaC7112CODCON_Id;
            $scope.cc = obje.ncaC7113CORVALF_Id;
            $scope.generalcomments = '';
            $scope.obj.generalcomments = $scope.generalcomments;
            var bb;
        };

        //*************** Save DATA WISE Comments ***************//
        $scope.savedatawisecomments = function (obj) {

            console.log("Save Comments");
            console.log(obj);

            var data = {
                "Remarks": obj.generalcomments,
                "filefkid": $scope.ccc
            };

            apiService.create("NaacCoreValues7113/savemedicaldatawisecomments", data).then(function (promise) {

                if (promise !== null) {
                    if (promise.returnval === true) {
                        swal("Comments Saved Successfully");
                    } else {
                        swal("Failed To Save Comments");
                    }
                    $('#mymodaladdcomments').modal('hide');
                    $('#mymodalviewuploaddocument').modal('hide');
                    $scope.valued = "2";
                    $scope.onload();
                }
            });
        };
        // fr add file comment shilpa

        // $scope.obj.generalcomments = "";
        $scope.savedatawisecomments1 = function (obj) {

            console.log("Save Comments");
            console.log(obj);

            var data = {
                "Remarks": obj.generalcomments,
                "filefkid": $scope.cc
            };

            apiService.create("NaacCoreValues7113/savefilewisecomments", data).then(function (promise) {

                if (promise !== null) {
                    if (promise.returnval === true) {
                        swal("Comments Saved Successfully");
                    } else {
                        swal("Failed To Save Comments");
                    }
                    $('#mymodaladdcomments1').modal('hide');
                    $('#mymodalviewuploaddocument1').modal('hide');
                    $scope.valued = "2";
                    $scope.onload();
                }
            });
        };

        //==================filter Name
        $scope.searchValue = "";




        ///=========clear upload field data......
        $scope.remove_file = function () {

            $scope.file_detail = "";
            $scope.notice = "";
        }
        ////Multiple File Upload

        $scope.materaldocuupload = [{ id: 'Materal1' }];

        $scope.addmateral = function () {
            var newItemNo = $scope.materaldocuupload.length + 1;

            if (newItemNo <= 10) {
                $scope.materaldocuupload.push({ 'id': 'Materal' + newItemNo });
            }
        };

        $scope.removemateral = function (index) {
            var newItemNo = $scope.materaldocuupload.length - 1;
            $scope.materaldocuupload.splice(index, 1);

            if ($scope.materaldocuupload.length === 0) {
                //data
            }
        };
        // Save Function For Materal Guide Upload

        $scope.uploadmateraldocuments1 = [];

        $scope.uploadmateraldocuments = function (input, document) {

            $scope.uploadmateraldocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {

                if (input.files[0].type === "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "video/mp4") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/pdf") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/msword") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document,") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                } else {
                    swal("Upload MP4, Pdf, Image Files Only");
                }
            }
        };

        function UploaddianmateralPhoto(data) {
            console.log("Student Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadmateraldocuments1.length; i++) {
                formData.append("File", $scope.uploadmateraldocuments1[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadnaacdocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    data.ncaC7113CORVALF_FilePath = d;
                    data.ncaC7113CORVALF_FileName = $scope.filename;
                    $('#').attr('src', data.ncaC7113CORVALF_FilePath);
                    var img = data.ncaC7113CORVALF_FilePath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.ncaC7113CORVALF_FilePath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };


        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };

        $scope.showGuardianPhotonew = function (data) {
            $scope.view_videos = [];
            $scope.videoSources = [];
            $scope.preview1 = data.ncaC7113CORVALF_FilePath;
            $scope.videdfd = data.ncaC7113CORVALF_FilePath;
            $scope.movie = { src: data.ncaC7113CORVALF_FilePath };
            $scope.movie1 = { src: data.ncaC7113CORVALF_FilePath };
            $scope.view_videos.push({ id: 1, coeeV_Videos: data.ncaC7113CORVALF_FilePath });
            console.log($scope.view_videos);
        };

        $scope.showpdf = false;
        $scope.downloadview = function (pdfview) {
            $scope.pdfurl = pdfview;
            $scope.showpdf = true;
            $('#showpdf').modal('show');
        };

        $scope.backtoview = function () {
            $scope.showpdf = false;
        };



        $scope.viewdocument = function (obj) {

            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            apiService.create("NaacCoreValues7113/viewuploadflies", obj).then(function (promise) {

                if (promise !== null) {

                    $scope.uploadfilesdetails = promise.view;
                    if (promise.view !== null && promise.view.length > 0) {
                        $scope.uploaddocfiles = promise.view;

                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            var img = dd.ncaC7113CORVALF_FilePath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.ncaC7113CORVALF_FilePath;
                            }
                        });
                    } else {
                        $('#popup11').modal('hide');
                        swal("No Documents Found");
                    }
                }
            });
        };


        $scope.onview = function (filepath, filename) {
            //var myPdfUrl = 'https://bdcampusstrg.blob.core.windows.net/files/6/LessonPlanner/1e55cab1-2e21-4878-802d-ad87b8507e6b.pdf';
            var imagedownload = filepath;
            $scope.content = "";
            var fileURL = "";
            var file = "";
            $http.get(imagedownload, { responseType: 'arraybuffer' })
                .success(function (response) {
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);
                    $scope.content = $sce.trustAsResourceUrl(fileURL);
                    $('#showpdf').modal('show');
                });
        };



        //$scope.deleteuploadfile = function (index,obj) {
        //    if (obj.ncaC7113CORVALF_Id == undefined) {
        //        $scope.materaldocuupload.splice(index, 1);
        //        swal("Record Deleted successfully");
        //    }
        //    else {
        //        var data = {
        //            "NCAC7113CORVALF_Id": obj.ncaC7113CORVALF_Id
        //        };

        //        swal({
        //            title: "Are You Sure",
        //            text: "Do You Want To Delete The Record ?",
        //            type: "warning",
        //            showCancelButton: true,
        //            confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete It!",
        //            cancelButtonText: "Cancel",
        //            closeOnConfirm: false,
        //            closeOnCancel: false
        //        },
        //            function (isConfirm) {
        //                if (isConfirm) {
        //                    apiService.create("NaacCoreValues7113/deleteuploadfile", data).then(function (promise) {
        //                        if (promise.returnval === true) {
        //                            swal("Record Deleted successfully");
        //                            var data = { "ncaC7113CORVAL_Id": $scope.equityid };
        //                            $scope.edittab1(data);
        //                        }
        //                        else {
        //                            swal("Record Deletion Failed");
        //                        }
        //                    });
        //                }
        //                else {
        //                    swal("Record Deletion Cancelled");
        //                }
        //            });
        //    }
        //};

        $scope.deleteuploadfile = function (docfile) {
            var data = {
                "NCAC7113CORVALF_Id": docfile.ncaC7113CORVALF_Id,
                "NCAC7113CORVAL_Id": docfile.ncaC7113CORVAL_Id,
                "MI_Id": docfile.mI_Id
            };
            swal({
                title: "Are You Sure",
                text: "Do You Want To Delete The Record ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("NaacCoreValues7113/deleteuploadfile", data).then(function (promise) {
                            if (promise.returnval === true) {
                                swal("Record Deleted successfully");
                                $scope.uploaddocfiles = promise.view;
                                $scope.uploadfilesdetails = promise.view;
                                $scope.uploadfilesdetails = promise.view;
                                if (promise.view !== null && promise.view.length > 0) {
                                    $scope.uploaddocfiles = promise.view;
                                    angular.forEach($scope.uploaddocfiles, function (dd) {
                                        var img = dd.ncaC7113CORVALF_FilePath;
                                        var imagarr = img.split('.');
                                        var lastelement = imagarr[imagarr.length - 1];
                                        dd.filetype = lastelement;
                                        if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                            dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.ncaC7112CODCONF_FilePath;
                                        }
                                    });
                                    //angular.forEach($scope.uploaddocfiles, function (dd) {
                                    //    var img = dd.cfilepath;
                                    //    var imagarr = img.split('.');
                                    //    var lastelement = imagarr[imagarr.length - 1];
                                    //    dd.filetype = lastelement;
                                    //    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                    //        dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.cfilepath;
                                    //    }
                                    //});
                                }
                            }
                            else {
                                swal("Record Deletion Failed");
                            }
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