
(function () {
    'use strict';

    angular
        .module('app')
        .controller('UC_312_TeachersResearchController', UC_312_TeachersResearchController);

    UC_312_TeachersResearchController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter', '$sce', 'myFactorynaac'];

    function UC_312_TeachersResearchController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter, $sce, myFactorynaac) {

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.sort = function (keyname) {
            $scope.propertyName = keyname;   //set the propertyName to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.search = "";
        $scope.submitted = false;

        //==============================page load
        var miid = myFactorynaac.get();
        if (miid > 0) {
            $scope.mI_Id = miid;
        } else {
            $scope.mI_Id = 0;
        }
        $scope.NCMCTR312_Id = 0;
        $scope.instit = false;
        $scope.loaddata = function () {
            $scope.NCMCTR312_Id = 0;
            $scope.change_institution();
            if ($scope.mI_Id == undefined || $scope.mI_Id == null || $scope.mI_Id == '') {
                $scope.mI_Id = 0;
            }
            //var pageid = 2;
            apiService.getURI("UC_312_TeachersResearch/loaddata", $scope.mI_Id).
                then(function (promise) {
                    $scope.institutionlist = promise.institutionlist;
                    $scope.mI_Id = promise.mI_Id;
                    $scope.allacademicyear = promise.allacademicyear;
                    $scope.filldepartment = promise.filldepartment;
                    $scope.alldata1 = promise.alldata1;
                });
        };

        ///=================================Clear
        $scope.Clearid = function () {
            $state.reload();
        };

        //==========================================Row Active/Deactive
        $scope.deactive = function (usersem, SweetAlert) {
            var dystring = "";
            if (usersem.ncmctR312_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (usersem.ncmctR312_ActiveFlag == false) {
                dystring = "Activate"
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
                        apiService.create("UC_312_TeachersResearch/deactive", usersem).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d" + " Successfully!!!");
                                    $state.reload();
                                }
                                else {
                                    swal("Record Not " + dystring + "d" + " Successfully!!!");
                                    $state.reload();
                                }
                            })
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        }

        //===================================Record Edit
        $scope.edittab1 = function (user) {
            var data = {
                "NCMCTR312_Id": user.ncmctR312_Id,
                "MI_Id": user.mI_Id
            }

            apiService.create("UC_312_TeachersResearch/EditData", data).then(function (promise) {

                if (promise.editlist.length > 0) {
                    $scope.instit = true;
                    $scope.NCMCTR312_Id = promise.editlist[0].ncmctR312_Id;
                    $scope.NCMCTR312_Year = promise.editlist[0].ncmctR312_Year;
                    $scope.NCMCTR312_ProjectName = promise.editlist[0].ncmctR312_ProjectName;
                    $scope.NCMCTR312_Duration = promise.editlist[0].ncmctR312_Duration;
                    $scope.NCMCTR312_ProjReceivingSeedMoney = promise.editlist[0].ncmctR312_ProjReceivingSeedMoney;
                    $scope.NCMCTR312_amountOfSeedMoneyProvided = promise.editlist[0].ncmctR312_amountOfSeedMoneyProvided;
                    $scope.mI_Id = promise.editlist[0].mI_Id;
                    
                    $scope.HRMD_Id = promise.hrmD_Id;
                    $scope.filldesignation = promise.filldesignation;
                    $scope.HRMDES_Id = promise.hrmdeS_Id;
                    $scope.emplist = promise.emplist;
                    $scope.hrmE_Id = promise.editlist[0].hrmE_Id;

                    $scope.materaldocuupload = promise.editFileslist;
                    if ($scope.materaldocuupload.length > 0) {
                        angular.forEach($scope.materaldocuupload, function (ddd) {
                            var img = ddd.cfilepath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            ddd.filetype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                ddd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + ddd.cfilepath;
                            }
                        })
                    }
                    else {
                        $scope.materaldocuupload = [{ id: 'Materal1' }];
                    }


                }
            });
        }

        //===========================Save Data
        $scope.save = function () {
            
            if ($scope.myForm.$valid) {
                var data = {
                    "NCMCTR312_Id": $scope.NCMCTR312_Id,
                    "ASMAY_Id": $scope.NCMCTR312_Year,
                    "NCMCTR312_ProjectName": $scope.NCMCTR312_ProjectName,
                    "NCMCTR312_Duration": $scope.NCMCTR312_Duration,
                    "NCMCTR312_ProjReceivingSeedMoney": $scope.NCMCTR312_ProjReceivingSeedMoney,
                    "NCMCTR312_amountOfSeedMoneyProvided": $scope.NCMCTR312_amountOfSeedMoneyProvided,
                    "HRME_Id": $scope.hrmE_Id,
                    "MI_Id": $scope.mI_Id,
                    filelist: $scope.materaldocuupload,
                }

                apiService.create("UC_312_TeachersResearch/save", data).then(function (promise) {
                    
                    if (promise.duplicate == true) {
                        swal("Data Already Exists");
                    }
                    else if (promise.msg == 'saved') {
                        swal("Data Saved Successfully...!!!");
                        $state.reload();
                    }
                    else if (promise.msg == 'Failed') {
                        swal("Data Not Saved Successfully...!!!");
                    }
                    else if (promise.msg == 'updated') {
                        swal("Data Updated Successfully...!!!");
                        $state.reload();
                    }
                    else if (promise.msg == 'failed') {
                        swal("Data Not Updated Successfully...!!!");
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.viewdocument = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            $scope.objdata = obj;
            apiService.create("UC_312_TeachersResearch/viewuploadflies", obj).then(function (promise) {
                if (promise !== null) {
                    $scope.uploadfilesdetails = promise.viewuploadflies;
                    if (promise.viewuploadflies !== null && promise.viewuploadflies.length > 0) {
                        $scope.uploaddocfiles = promise.viewuploadflies;
                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            var img = dd.cfilepath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.cfilepath;
                            }
                        });
                    } else {
                        $('#popup11').modal('hide');
                        swal("No Documents Found");
                    }
                }
            });
        };

        $scope.deleteuploadfile = function (docfile) {

            var data = {
                "NCMCTR312F_Id": docfile.ncmctR312F_Id,
                "NCMCTR312_Id": docfile.ncmctR312_Id,
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
                        apiService.create("UC_312_TeachersResearch/deleteuploadfile", data).then(function (promise) {

                            if (promise.returnval === true) {
                                swal("Record Deleted successfully");

                                $scope.uploaddocfiles = promise.viewuploadflies;
                                $scope.uploadfilesdetails = promise.viewuploadflies;
                                if (promise.viewuploadflies !== null && promise.viewuploadflies.length > 0) {
                                    $scope.uploaddocfiles = promise.viewuploadflies;

                                    angular.forEach($scope.uploaddocfiles, function (dd) {
                                        var img = dd.cfilepath;
                                        var imagarr = img.split('.');
                                        var lastelement = imagarr[imagarr.length - 1];
                                        dd.filetype = lastelement;
                                        if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                            dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.cfilepath;
                                        }
                                    });
                                }

                                //else {
                                //    $('#popup11').modal('hide');
                                //    swal("No Documents Found");
                                //}
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

        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;

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

                if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
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
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].size > 2097152) {
                    swal("size should be less than 2MB");
                    return;
                }
                else {
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
                    data.cfilepath = d;
                    data.cfilename = $scope.filename;
                    $('#').attr('src', data.cfilepath);
                    var img = data.cfilepath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.cfilepath;
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
            $scope.preview1 = data.cfilepath;
            $scope.videdfd = data.cfilepath;
            $scope.movie = { src: data.cfilepath };
            $scope.movie1 = { src: data.cfilepath };
            $scope.view_videos.push({ id: 1, coeeV_Videos: data.cfilepath });
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

        $scope.onview = function (filepath, filename) {
            var imagedownload = filepath;
            $scope.content = "";
            var fileURL = "";
            var file = "";
            $http.get(imagedownload, { responseType: 'arraybuffer' })
                .success(function (response) {
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);
                    //  $scope.content = $sce.trustAsResourceUrl(fileURL);
                    var pdfId = document.getElementById("pdfIdzz");
                    pdfId.removeChild(pdfId.childNodes[0]);
                    var embed = document.createElement('embed');
                    embed.setAttribute('src', fileURL);
                    embed.setAttribute('type', 'application/pdf');
                    embed.setAttribute('width', '100%');
                    embed.setAttribute('height', '1000');
                    pdfId.appendChild(embed);
                    $('#showpdf').modal('show');
                });
        };

        $scope.change_institution = function () {
            $scope.NCMCTR312_Id = 0;
            $scope.asmaY_Id = '';
            $scope.NCMCTR312_ProjectName = '';
            $scope.NCMCTR312_Duration = '';
            $scope.NCMCTR312_ProjReceivingSeedMoney = '';
            $scope.NCMCTR312_amountOfSeedMoneyProvided = '';

            $scope.usercheckC = '';
            $scope.materaldocuupload = [];
            $scope.materaldocuupload = [{ id: 'Materal1' }];

        }

        $scope.get_department = function () {
            $scope.filldesignation = [];
            $scope.emplist = [];
            $scope.HRMDES_Id = "";
            $scope.hrmE_Id = "";
            var data = {
                "HRMD_Id": $scope.HRMD_Id,
                "MI_Id": $scope.mI_Id,
            }
            apiService.create("UC_312_TeachersResearch/get_dept", data).then(function (promise) {
                $scope.filldesignation = promise.filldesignation;
            })
        };


        $scope.get_emp = function () {
            $scope.emplist = [];         
            $scope.hrmE_Id = "";
            var data = {
                "HRMDES_Id": $scope.HRMDES_Id,
                "HRMD_Id": $scope.HRMD_Id,
                "MI_Id": $scope.mI_Id,
            }

            apiService.create("UC_312_TeachersResearch/get_emp", data).then(function (promise) {

                $scope.emplist = promise.emplist;

            })
        }

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

