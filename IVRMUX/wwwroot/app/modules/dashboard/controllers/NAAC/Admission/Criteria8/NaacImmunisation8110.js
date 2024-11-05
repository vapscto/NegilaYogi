
(function () {
    'use strict';

    angular
        .module('app')
        .controller('NaacImmunisation8110', NaacImmunisation8110);

    NaacImmunisation8110.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter', '$sce','myFactorynaac'];

    function NaacImmunisation8110($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter, $sce, myFactorynaac) {
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.sort = function (keyname) {
            $scope.propertyName = keyname;   //set the propertyName to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        //$scope.submitted = false;
        //==============================page load
        var miid = myFactorynaac.get();
        if (miid > 0) {
            $scope.mI_Id = miid;
        } else {
            $scope.mI_Id = 0;
        }
        $scope.NCMC8110IMM_Id = 0;
        $scope.instit = false;
        $scope.loaddata = function () {
         
            $scope.NCMC8110IMM_Id = 0;
            $scope.change_institution();
            if ($scope.mI_Id == undefined || $scope.mI_Id == null || $scope.mI_Id == '') {
                $scope.mI_Id = 0;
            }
            //var pageid = 2;
            apiService.getURI("NaacImmunisation8110/loaddata", $scope.mI_Id).
                then(function (promise) {
                 
                    $scope.institutionlist = promise.institutionlist;
                    $scope.mI_Id = promise.mI_Id;
                    $scope.allacademicyear = promise.allacademicyear;
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
            if (usersem.ncmC8110IMM_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (usersem.ncmC8110IMM_ActiveFlg == false) {
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
                        apiService.create("NaacImmunisation8110/deactive", usersem).
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
                "NCMC8110IMM_Id": user.ncmC8110IMM_Id,
                "MI_Id": user.mI_Id
            }

            apiService.create("NaacImmunisation8110/EditData", data).then(function (promise) {
                             
                if (promise.editlist.length > 0) {
                    $scope.instit = true;
                    $scope.NCMC8110IMM_Id = promise.editlist[0].ncmC8110IMM_Id;
                    $scope.NCMC8110IMM_Year = promise.editlist[0].ncmC8110IMM_Year;
                    $scope.NCMC8110IMM_NoOfAdmStudents = promise.editlist[0].ncmC8110IMM_NoOfAdmStudents;
                    $scope.NCMC8110IMM_NoOfImmuStudents = promise.editlist[0].ncmC8110IMM_NoOfImmuStudents;
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
                    else
                    {
                        $scope.materaldocuupload = [{ id: 'Materal1' }];
                    }                   
                }
            });
        }

        //===========================Save Data
        $scope.save = function () {
            debugger;
            if ($scope.myForm.$valid) {
                var data = {
                    "NCMC8110IMM_Id": $scope.NCMC8110IMM_Id,
                    "asmaY_Id": $scope.NCMC8110IMM_Year,
                    "NCMC8110IMM_NoOfAdmStudents": $scope.NCMC8110IMM_NoOfAdmStudents,
                    "NCMC8110IMM_NoOfImmuStudents": $scope.NCMC8110IMM_NoOfImmuStudents,
                    "MI_Id": $scope.mI_Id,
                    filelist: $scope.materaldocuupload,
                }

                apiService.create("NaacImmunisation8110/save", data).then(function (promise) {
                    debugger;
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
            apiService.create("NaacImmunisation8110/viewuploadflies", obj).then(function (promise) {
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
                "NCMC8110IMMF_Id": docfile.ncmC8110IMMF_Id,
                "NCMC8110IMM_Id": docfile.ncmC8110IMM_Id,
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
                        apiService.create("NaacImmunisation8110/deleteuploadfile", data).then(function (promise) {

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




        $scope.change_institution = function () {
            $scope.NCMC8110IMM_Id = 0;
            $scope.asmaY_Id = '';
            $scope.NCMC8110IMM_NoOfAdmStudents = '';
            $scope.NCMC8110IMM_NoOfImmuStudents = '';


           

            $scope.filldesignation = [];
            $scope.usercheckC = '';
            $scope.materaldocuuploadStaff = [];
            $scope.materaldocuupload = [];
            $scope.stafftlist = [];
            $scope.materaldocuuploadStaff = [{ id: 'Materal1' }];
            $scope.materaldocuupload = [{ id: 'Materal1' }];

            angular.forEach($scope.stafftlist, function (tt) {
                tt.selected = false;
            })
        }

        //=======added comments and status flag =========//
        $scope.getlocationaldata = function (obj) {

            apiService.create("NaacImmunisation8110/getcomment", obj).then(function (promise) {

                if (promise !== null) {

                    $scope.commentlist = promise.commentlist;
                }
            });
        };

        // for file savita
        $scope.getlocationaldata1 = function (obj) {
            apiService.create("NaacImmunisation8110/getfilecomment", obj).then(function (promise) {

                if (promise !== null) {


                    $scope.commentlist1 = promise.commentlist1;

                }
            });
        };
        // for comment
        $scope.addcomments = function (obje) {

            $scope.ccc = obje.ncmC8110IMM_Id;
            $scope.generalcomments = '';
            $scope.obj.generalcomments = $scope.generalcomments;
            var bb;
        };

        // for file comment
        $scope.addcomments1 = function (obje) {

            //obj.ncaC7112CODCON_Id = obje.ncaC7112CODCON_Id;
            $scope.cc = obje.ncmC8110IMMF_Id;
            $scope.generalcomments = '';
            $scope.obj.generalcomments = $scope.generalcomments;
            var bb;
        };
        // Savita
        //========= Save DATA WISE Comments 
        $scope.savedatawisecomments = function (obj) {

            console.log("Save Comments");
            console.log(obj);

            var data = {
                "Remarks": obj.generalcomments,
                "filefkid": $scope.ccc
            };

            apiService.create("NaacImmunisation8110/savecomments", data).then(function (promise) {

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

        //======== for add file comment savita

        $scope.savedatawisecomments1 = function (obj) {

            console.log("Save Comments");
            console.log(obj);

            var data = {
                "Remarks": obj.generalcomments,
                "filefkid": $scope.cc
            };

            apiService.create("NaacImmunisation8110/savefilewisecomments", data).then(function (promise) {

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

        //========== Comments For Record Wise 
        $scope.adddatawisecomments = function (dd) {

            console.log("Add Comments");
            console.log(dd);
            $scope.obj.filefkid = dd.ncmC8110IMM_Id;
            $scope.obj.generalcomments = "";

            $('#mymodaladdcomments').modal('show');
        };

        //=======VIEW DATA WISE SAVED COMMENTS 
        $scope.viewdatawisecomments = function (obj) {

            console.log("VIEW Comments data wise");
            console.log(obj);
            var data = {

                "filefkid": obj.ncmC8110IMM_Id
            };
            apiService.create("NaacImmunisation8110/viewmedicaldatawisecomments", data).then(function (promise) {
                if (promise !== null) {
                    $scope.viewcomments = promise.datawisecomments;
                    $('#mymodalviewcommentslist').modal('show');
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

