﻿
(function () {
    'use strict';
    angular.module('app').controller('DifferentlyAbledMCController', DifferentlyAbledMCController);

    DifferentlyAbledMCController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter', '$sce', 'myFactorynaac'];

    function DifferentlyAbledMCController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter, $sce, myFactorynaac) {

        $scope.mI_Id = 0;
        $scope.equityid = 0;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";

        var miid = myFactorynaac.get();
        if (miid > 0) {
            $scope.mI_Id = miid;
        } else {
            $scope.mI_Id = 0;
        }

        ///======================================Load Data.
        $scope.loaddata = function () {
            $scope.equityid = 0;
            var id = 2;
            apiService.getURI("DifferentlyAbled/loaddata", id).then(function (promise) {
                $scope.institutionlist = promise.institutionlist;
                if (promise.institutionlist.length == 1) {
                    $scope.mI_Id = promise.institutionlist[0].mI_Id;
                    $scope.getData($scope.mI_Id);
                }
            });
        };

        $scope.getData = function (mI_Id) {
            $scope.equityid = 0;
            var id = mI_Id;
            apiService.getURI("DifferentlyAbled/getDataMC", id).then(function (promise) {
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
            if ($scope.myForm.$valid) {
                if ($scope.page.mI_Id == undefined) { $scope.page.mI_Id = $scope.mI_Id; }

                var data = {
                    "MI_Id": $scope.page.mI_Id,
                    "NCAC719DIFFAB_Id": $scope.equityid,
                    "NCAC719DIFFAB_Year": $scope.page.ncaC719DIFFAB_Year,
                    "NCAC719DIFFAB_CodeOfConductDisplayedWebsiteFlag": $scope.page.ncaC719DIFFAB_CodeOfConductDisplayedWebsiteFlag,
                    "NCAC719DIFFAB_CommitteeMonitorAdherenceCodeConductFlag": $scope.page.ncaC719DIFFAB_CommitteeMonitorAdherenceCodeConductFlag,
                    "NCAC719DIFFAB_ProfProgOrgStuStaffFlag": $scope.page.ncaC719DIFFAB_ProfProgOrgStuStaffFlag,
                    "NCAC719DIFFAB_AnnualAwsProgConductOrganizedFlag": $scope.page.ncaC719DIFFAB_AnnualAwsProgConductOrganizedFlag
                };
                apiService.create("DifferentlyAbled/saveMC", data).then(function (promise) {
                    if (promise.duplicate != null && promise.returnval != null) {
                        if (promise.duplicate == false) {
                            if (promise.returnval == true) {
                                swal('Data Saved Successfully!');
                                //$scope.loaddata();
                                $state.reload();
                            }
                            else {
                                swal('Data Not Saved Successfully!');
                            }
                        }
                        else {
                            swal('Record already Exist!');
                        }
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        //==========================edit data for tab1
        $scope.edittab1 = function (data) {
            apiService.create("DifferentlyAbled/EditDataMC", data).then(function (promise) {
                $scope.equityid = data.ncaC719DIFFAB_Id;
                $scope.page = promise.editlisttab1[0];
            });
        };


        //===========deactive and active for Tab1
        $scope.deactivYTab1 = function (usersem, SweetAlert) {
            //$scope.NCAC511GSCH_Id = usersem.ncaC511GSCH_Id;
            var dystring = "";
            if (usersem.ncaC719DIFFAB_ActiveFlg == true) {
                dystring = "Deactivated";
            }
            else if (usersem.ncaC719DIFFAB_ActiveFlg == false) {
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
                        apiService.create("DifferentlyAbled/deactivate", usersem).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + " Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + " Successfully!!!");
                                }
                                $scope.loaddata();
                            });
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        };

        //For Cancel data record 
        $scope.Clearid = function () {
            $scope.equityid = 0;
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //==================filter Name
        $scope.searchValue = "";




        ///=========clear upload field data......
        $scope.remove_file = function () {

            $scope.file_detail = "";
            $scope.notice = "";
        };
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
                    data.ncaC719DIFFABF_FilePath = d;
                    data.ncaC719DIFFABF_FileName = $scope.filename;
                    $('#').attr('src', data.ncaC719DIFFABF_FilePath);
                    var img = data.ncaC719DIFFABF_FilePath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.ncaC719DIFFABF_FilePath;
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
            $scope.preview1 = data.ncaC719DIFFABF_FilePath;
            $scope.videdfd = data.ncaC719DIFFABF_FilePath;
            $scope.movie = { src: data.ncaC719DIFFABF_FilePath };
            $scope.movie1 = { src: data.ncaC719DIFFABF_FilePath };
            $scope.view_videos.push({ id: 1, coeeV_Videos: data.ncaC719DIFFABF_FilePath });
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

            apiService.create("DifferentlyAbled/viewuploadflies", obj).then(function (promise) {
                if (promise !== null) {
                    $scope.uploadfilesdetails = promise.editfiles;
                    if (promise.editfiles !== null && promise.editfiles.length > 0) {
                        $scope.uploaddocfiles = promise.editfiles;

                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            var img = dd.ncaC719DIFFABF_FilePath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.ncaC719DIFFABF_FilePath;
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



        $scope.deleteuploadfile = function (obj) {

            var data = {
                "NCAC719DIFFABF_Id": obj.ncaC719DIFFABF_Id

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
                        apiService.create("DifferentlyAbled/deleteuploadfile", data).then(function (promise) {
                            //if (promise.already_cnt === true) {
                            //    swal("You Can Not Deactivate This Record,It Has Dependency");
                            //}
                            //else {
                            if (promise.returnval === true) {
                                swal("Record Deleted successfully");

                                var data = { "ncaC719DIFFAB_Id": $scope.equityid };

                                $scope.edittab1(data);
                            }
                            else {
                                swal("Record Deletion Failed");
                            }
                            //}
                            //$('#popup11').modal('hide');

                            //$scope.viewdocument()
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