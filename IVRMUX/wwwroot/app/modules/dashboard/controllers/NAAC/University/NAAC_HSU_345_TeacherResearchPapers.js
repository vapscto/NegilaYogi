(function () {
    'use strict';
    angular
        .module('app')
        .controller('NAAC_HSU_345_TeacherResearchPapers', NAAC_HSU_345_TeacherResearchPapers)

    NAAC_HSU_345_TeacherResearchPapers.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter', '$sce', 'myFactorynaac'];
    function NAAC_HSU_345_TeacherResearchPapers($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter, $sce, myFactorynaac) {

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.submitted = false;
        var miid = myFactorynaac.get();
        if (miid > 0) {
            $scope.mI_Id = miid;
        } else {
            $scope.mI_Id = 0;
        }
        $scope.NCHSU345TRP_Id = 0;
        $scope.instit = false;
        //======================page load
        $scope.loaddata = function () {
       
            $scope.NCHSU345TRP_Id = 0;
            $scope.change_institution();
            if ($scope.mI_Id == undefined || $scope.mI_Id == null || $scope.mI_Id == '') {
                $scope.mI_Id = 0;
            }
            //var pageid = 2;
            apiService.getURI("NAAC_HSU_345_TeacherResearchPapers/loaddata", $scope.mI_Id).then(function (promise) {
                $scope.institutionlist = promise.institutionlist;
                $scope.mI_Id = promise.mI_Id;
                $scope.allacademicyear = promise.allacademicyear;
                $scope.alldata1 = promise.alldata1;
            })
        };

        //======================Record Save
        $scope.search = "";
        $scope.save = function () {
            if ($scope.myForm.$valid) {

              
                var data = {
                    "NCHSU345TRP_Id": $scope.NCHSU345TRP_Id,
                    "asmaY_Id": $scope.NCHSU345TRP_Year,
                    "NCHSU345TRP_PaperTitle": $scope.NCHSU345TRP_PaperTitle,
                    "NCHSU345TRP_AuthorName": $scope.NCHSU345TRP_AuthorName,
                    "NCHSU345TRP_DepartmentName": $scope.NCHSU345TRP_DepartmentName,
                    "NCHSU345TRP_JournalName": $scope.NCHSU345TRP_JournalName,
                    "NCHSU345TRP_ISSNNumber": $scope.NCHSU345TRP_ISSNNumber,
                    "NCHSU345TRP_link": $scope.NCHSU345TRP_link,
                    "NCHSU345TRP_NamesOfIndexingDatabases": $scope.NCHSU345TRP_NamesOfIndexingDatabases,
                    filelist: $scope.materaldocuupload,
                    "MI_Id": $scope.mI_Id
                }

                apiService.create("NAAC_HSU_345_TeacherResearchPapers/save", data).then(function (promise) {
                    if (promise.msg == 'saved') {
                        swal("Data Saved Successfully...!!!")
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
                    else if (promise.returnval == true) {
                        swal("Data Already Exists");
                    }
                    else {
                        swal("Something is Wrong......");
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.deactivYTab1 = function (usersem, SweetAlert) {
          
            var dystring = "";
            if (usersem.nchsU345TRP_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (usersem.nchsU345TRP_ActiveFlag == false) {
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
                        apiService.create("NAAC_HSU_345_TeacherResearchPapers/deactive", usersem).
                            then(function (promise) {
                                if (promise.ret == true) {
                                    swal("Record " + dystring + "d" + " Successfully!!!");
                                    $state.reload();
                                }
                                else {

                                    swal("Record Not " + dystring + "d " + "Successfully!!!");
                                    $state.reload();
                                }
                            })
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }

                });
        }
        $scope.Clearid = function () {
            $state.reload();
        }
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;



        ///=========clear upload field data......
        $scope.remove_file = function () {
            $scope.file_detail = "";
            $scope.notice = "";
        }

        $scope.sort = function (keyname) {
            $scope.propertyName = keyname;   //set the propertyName to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        //================================edit data
        $scope.edittab1 = function (user) {
           
            var data = {
                "NCHSU345TRP_Id": user.nchsU345TRP_Id,
                "MI_Id": user.mI_Id
            }
            apiService.create("NAAC_HSU_345_TeacherResearchPapers/EditData", data).then(function (promise) {
                if (promise.editlist.length > 0) {
                    $scope.instit = true;
                    $scope.NCHSU345TRP_Id = promise.editlist[0].nchsU345TRP_Id;
                    $scope.NCHSU345TRP_Year = promise.editlist[0].nchsU345TRP_Year;
                    $scope.NCHSU345TRP_PaperTitle = promise.editlist[0].nchsU345TRP_PaperTitle;
                    $scope.NCHSU345TRP_AuthorName = promise.editlist[0].nchsU345TRP_AuthorName;
                    $scope.NCHSU345TRP_DepartmentName = promise.editlist[0].nchsU345TRP_DepartmentName;
                    $scope.NCHSU345TRP_JournalName = promise.editlist[0].nchsU345TRP_JournalName;
                    $scope.NCHSU345TRP_ISSNNumber = promise.editlist[0].nchsU345TRP_ISSNNumber;
                    $scope.NCHSU345TRP_link = promise.editlist[0].nchsU345TRP_link;
                    $scope.NCHSU345TRP_NamesOfIndexingDatabases = promise.editlist[0].nchsU345TRP_NamesOfIndexingDatabases;
                    $scope.mI_Id = promise.editlist[0].mI_Id;
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

        $scope.viewdocument = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            $scope.objdata = obj;

            apiService.create("NAAC_HSU_345_TeacherResearchPapers/viewuploadflies", obj).then(function (promise) {
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
                "NCHSU345TRPF_Id": docfile.nchsU345TRPF_Id,
                "NCHSU345TRP_Id": docfile.nchsU345TRP_Id,
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
                        apiService.create("NAAC_HSU_345_TeacherResearchPapers/deleteuploadfile", data).then(function (promise) {

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
            $scope.NCHSU345TRP_Year = '';
            $scope.NCHSU345TRP_PaperTitle = '';
            $scope.NCHSU345TRP_AuthorName = '';
            $scope.NCHSU345TRP_DepartmentName = '';
            $scope.NCHSU345TRP_JournalName = '';
            $scope.NCHSU345TRP_ISSNNumber = '';
            $scope.NCHSU345TRP_link = '';
            $scope.NCHSU345TRP_NamesOfIndexingDatabases = '';
            $scope.NCHSU345TRP_Id = 0;


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







