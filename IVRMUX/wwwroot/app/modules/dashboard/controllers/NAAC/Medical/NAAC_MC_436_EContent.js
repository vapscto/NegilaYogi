(function () {
    'use strict';
    angular
        .module('app')
        .controller('NAAC_MC_436_EContent', NAAC_MC_436_EContent)

    NAAC_MC_436_EContent.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$sce', 'myFactorynaac', '$filter']
    function NAAC_MC_436_EContent($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $sce, myFactorynaac, $filter) {

        $scope.submitted = false;
        $scope.searchchkBOS = "";

        $scope.hlmH_BoysGirlsFlg = "Boys";
        var miid = myFactorynaac.get();
        if (miid > 0) {
            $scope.mI_Id = miid;
        } else {
            $scope.mI_Id = 0;
        }
        $scope.NCMCMEC436_Id = 0;
        //=====================Load--data.............
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname; //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.instit = false;
        $scope.loaddata = function () {
            $scope.NCMCMEC436_Id = 0;
            $scope.change_institution();
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";


            if ($scope.mI_Id == undefined || $scope.mI_Id == null || $scope.mI_Id == '') {
                $scope.mI_Id = 0;
            }
            //var pageid = 2;
            apiService.getURI("NAAC_MC_436_EContent/loaddata", $scope.mI_Id).then(function (promise) {

                $scope.institutionlist = promise.institutionlist;
                $scope.mI_Id = promise.mI_Id;

                $scope.yearlist = promise.yearlist;
                $scope.emplylist = promise.emplylist;

                $scope.allgridlist = promise.allgridlist;
            })
        }
        //=====================End-----Load--data----//

        //=====================saverecord....
        $scope.savedata = function () {
            $scope.empchecklist = [];

            angular.forEach($scope.emplylist, function (tt) {
                if (tt.selectedbos == true) {
                    $scope.empchecklist.push({ HRME_Id: tt.hrmE_Id });
                }
            })

            if ($scope.myForm.$valid) {
                var startdate = $scope.NCMCMEC436_Date == null ? "" : $filter('date')($scope.NCMCMEC436_Date, "yyyy-MM-dd");

                var data = {
                    "NCMCMEC436_Id": $scope.ncmcmeC436_Id,
                    "NCMCMEC436_ModuleName": $scope.NCMCMEC436_ModuleName,
                    "NCMCMEC436_PlatformModuleUsed": $scope.NCMCMEC436_PlatformModuleUsed,
                    "NCMCMEC436_WebLink": $scope.NCMCMEC436_WebLink,
                    filelist: $scope.materaldocuupload,
                    "MI_Id": $scope.mI_Id,
                    "NCMCMEC436_Year": $scope.ASMAY_Id,
                    "NCMCMEC436_Date": startdate,
                    "empchecklist": $scope.empchecklist,
                }

                apiService.create("NAAC_MC_436_EContent/savedata", data).then(function (promise) {
                    if (promise.returnval !== null && promise.duplicate !== null) {
                        if (promise.msg == 'saved') {
                            swal("Record Saved Successfully!");
                            $state.reload();
                        }
                        else if (promise.msg == 'updated') {
                            swal("Record Updated Successfully!");
                            $state.reload();
                        }
                        else if (promise.msg == 'duplicate') {
                            swal("Record already exist");
                        }
                        else if (promise.msg == "savingFailed") {
                            swal("Failed to save record");
                        }
                        else if (promise.msg == "updateFailed") {
                            swal("Failed to update record");
                        }
                        else {
                            swal("Sorry...something went wrong");
                        }
                    }
                    else {
                        swal("Kindly Contact Administrator!!!");
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        };
        //=====================End---saverecord....

        //=================Activation/Deactivation
        $scope.deactiveStudent = function (usersem, SweetAlert) {
            var dystring = "";
            if (usersem.ncmcmeC436_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (usersem.ncmcmeC436_ActiveFlag == false) {
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
                        apiService.create("NAAC_MC_436_EContent/deactiveStudent", usersem).
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
        //================End----Activation/Deactivation--Record.........

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        //===========----Clear Field
        $scope.Clearid = function () {
            $state.reload();
        }

        //======================edit data.
        $scope.editdata = function (obj) {
            var data = {
                "NCMCMEC436_Id": obj.ncmcmeC436_Id,
                "MI_Id": obj.mI_Id,
            }
            apiService.create("NAAC_MC_436_EContent/editdata", data).then(function (promise) {

                if (promise.editlist.length > 0) {
                    $scope.instit = true;
                    $scope.ncmcmeC436_Id = promise.editlist[0].ncmcmeC436_Id;
                    $scope.NCMCMEC436_ModuleName = promise.editlist[0].ncmcmeC436_ModuleName;
                    $scope.NCMCMEC436_PlatformModuleUsed = promise.editlist[0].ncmcmeC436_PlatformModuleUsed;
                    $scope.NCMCMEC436_WebLink = promise.editlist[0].ncmcmeC436_WebLink;
                    $scope.mI_Id = promise.editlist[0].mI_Id;
                    $scope.ASMAY_Id = promise.editlist[0].ncmcmeC436_Year;
                    $scope.NCMCMEC436_Date = new Date(promise.editlist[0].ncmcmeC436_Date);

                    angular.forEach($scope.emplylist, function (tt) {
                        angular.forEach(promise.editlist, function (ss) {
                            if (tt.hrmE_Id == ss.hrmE_Id) {
                                tt.selectedbos = true;
                            }
                        })
                    })

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
            })
        }


        //===============================view document files
        $scope.viewdocument = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            $scope.objdata = obj;

            apiService.create("NAAC_MC_436_EContent/viewuploadflies", obj).then(function (promise) {
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


        //===============================delete document files
        $scope.deleteuploadfile = function (docfile) {
            var data = {
                "NCMCMEC436F_Id": docfile.ncmcmeC436F_Id,
                "NCMCMEC436_Id": docfile.ncmcmeC436_Id,
            }

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
                        apiService.create("NAAC_MC_436_EContent/deleteuploadfile", data).then(function (promise) {

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

        //=======selection of checkbox....BOS
        $scope.togchkbxBOS = function () {
            $scope.usercheckBOS = $scope.emplylist.every(function (role) {
                return role.selectedbos;
            });
        }

        //---------all checkbox Select...BOS
        $scope.all_checkBOS = function (all) {
            $scope.usercheckC = all;
            var toggleStatus = $scope.usercheckBOS;
            angular.forEach($scope.emplylist, function (role) {
                role.selectedbos = toggleStatus;
            });
        }

        //========classlist CheckBox Field Validation===========//BOS
        $scope.isOptionsRequiredBOS = function () {
            return !$scope.emplylist.some(function (item) {
                return item.selectedbos;
            });
        }

        //=====================================================================

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
                    swal("Image size should be less than 2MB");
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
        //$scope.onview = function (filepath, filename) {
        //    //var myPdfUrl = 'https://bdcampusstrg.blob.core.windows.net/files/6/LessonPlanner/1e55cab1-2e21-4878-802d-ad87b8507e6b.pdf';
        //    var imagedownload = filepath;
        //    $scope.content = "";
        //    var fileURL = "";
        //    var file = "";
        //    $http.get(imagedownload, { responseType: 'arraybuffer' })
        //        .success(function (response) {
        //            file = new Blob([(response)], { type: 'application/pdf' });
        //            fileURL = URL.createObjectURL(file);
        //            $scope.content = $sce.trustAsResourceUrl(fileURL);
        //            $('#showpdf').modal('show');
        //        });
        //};
        $scope.change_institution = function () {
            $scope.NCMCMEC436_Id = 0;
            $scope.ncaC434ECT_DevFacilityName = '';
            $scope.ncaC434ECT_LinkName = '';
            $scope.materaldocuupload = [];
            $scope.materaldocuupload = [{ id: 'Materal1' }];
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