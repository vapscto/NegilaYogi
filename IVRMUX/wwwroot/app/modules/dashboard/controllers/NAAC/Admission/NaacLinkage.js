(function () {
    'use strict';
    angular
        .module('app')
        .controller('NaacLinkage', NaacLinkage)

    NaacLinkage.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter', '$sce','myFactorynaac'];
    function NaacLinkage($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter, $sce, myFactorynaac) {


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.submitted = false;
        $scope.searchValue = "";

        $scope.Clearid = function () {
            $state.reload();
        };
        var miid = myFactorynaac.get();
        if (miid > 0) {
            $scope.mI_Id = miid;
        } else {
            $scope.mI_Id = 0;
        }
        $scope.NCAC351LIN_Id = 0;
        $scope.instit = false;
        $scope.loaddata = function () {
            $scope.NCAC351LIN_Id = 0;
            $scope.change_institution();
            if ($scope.mI_Id == undefined || $scope.mI_Id == null || $scope.mI_Id == '') {
                $scope.mI_Id = 0;
            }
            //var pageid = 2;
            apiService.getURI("NaacLinkage/loaddata", $scope.mI_Id).then(function (promise) {
                $scope.institutionlist = promise.institutionlist;
                $scope.mI_Id = promise.mI_Id;
                $scope.yearlist = promise.yearlist;
                $scope.alldata1 = promise.alldata1;

            })
        }
        //======for deactive/Active record
        $scope.deactivYTab1 = function (usersem, SweetAlert) {

            var dystring = "";

            if (usersem.ncaC351LIN_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (usersem.ncaC351LIN_ActiveFlg == false) {
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
                        apiService.create("NaacLinkage/deactive", usersem).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d" + " Successfully!!!");
                                    $state.reload();
                                }
                                else {
                                    swal("Record " + "Not " + dystring + "d" + " Successfull!!!");
                                    $state.reload();
                                }
                            })
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        }


        //=================for edit
        $scope.edittab1 = function (user) {
            var data = {
                "NCAC351LIN_Id": user.ncaC351LIN_Id,
                "MI_Id": user.mI_Id
            }
            apiService.create("NaacLinkage/EditData", data).then(function (promise) {
                if (promise.editlist.length > 0) {
                    $scope.instit = true;
                    $scope.NCAC351LIN_Id = promise.editlist[0].ncaC351LIN_Id;
                    $scope.NCAC351LIN_CommYear = promise.editlist[0].ncaC351LIN_CommYear;
                    $scope.NCAC351LIN_LinkageTitle = promise.editlist[0].ncaC351LIN_LinkageTitle;
                    $scope.NCAC351LIN_PartnerName = promise.editlist[0].ncaC351LIN_PartnerName;
                    $scope.NCAC351LIN_LinkageNature = promise.editlist[0].ncaC351LIN_LinkageNature;
                    $scope.NCAC351LIN_LinkOfDocument = promise.editlist[0].ncaC351LIN_LinkOfDocument;
                    $scope.mI_Id = promise.editlist[0].mI_Id;

                    $scope.NCAC351LIN_From = new Date(promise.editlist[0].ncaC351LIN_From);
                    $scope.NCAC351LIN_To = new Date(promise.editlist[0].ncaC351LIN_To);
                    $scope.orig = [];
                   
                    if (promise.editFileslist.length > 0) {
                        $scope.materaldocuupload = promise.editFileslist;
                        //$scope.fileoriginal = promise.editFileslist;
                        $scope.orig = promise.orig;
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


        //=============for Save data
        $scope.save = function () {
          
            $scope.remv = [];
            $scope.remv1 = [];
            $scope.remv2 = [];
            if ($scope.myForm.$valid) {
             
                //angular.forEach($scope.orig, function (cc) {
                //    debugger;
                //    angular.forEach($scope.materaldocuupload, function (vv) {
                //        debugger;
                //        if (cc.ncaC351LINF_Id > 0 && cc.ncaC351LINF_Id == vv.ncaC351LINF_Id) {
                //            $scope.remv.push(cc);
                //        }
                //        else if (vv.ncaC351LINF_Id == undefined) {
                //            $scope.remv1.push(cc);
                //        }
                //        else if (vv.ncaC351LINF_Id<0) {
                //            $scope.remv2.push(cc);
                //        }
                //    })
                //})

             
                    var data = {
                        "NCAC351LIN_Id": $scope.NCAC351LIN_Id,
                        "NCAC351LIN_CommYear": $scope.NCAC351LIN_CommYear,
                        "NCAC351LIN_LinkageTitle": $scope.NCAC351LIN_LinkageTitle,
                        "NCAC351LIN_PartnerName": $scope.NCAC351LIN_PartnerName,
                        "NCAC351LIN_LinkageNature": $scope.NCAC351LIN_LinkageNature,
                        "NCAC351LIN_LinkOfDocument": $scope.NCAC351LIN_LinkOfDocument,
                        "NCAC351LIN_From": $scope.NCAC351LIN_From,
                        "NCAC351LIN_To": $scope.NCAC351LIN_To,
                        filelist: $scope.materaldocuupload,
                        

                        "MI_Id": $scope.mI_Id,
                    }
               
                apiService.create("NaacLinkage/save", data).then(function (promise) {

                    if (promise.duplicate == false) {
                        if (promise.returnval === true) {
                            if ($scope.NCAC351LIN_Id > 0) {
                                swal('Record Updated Successfully!!!');
                                $state.reload();
                            }
                            else {
                                swal('Record Saved Successfully!!!');
                                $state.reload();
                            }
                        }
                        else {
                            if (promise.returnval === false) {
                                if ($scope.NCAC351LIN_Id > 0) {
                                    swal('Record Not Updated Successfully!!!');
                                }
                                else {
                                    swal('Record Not Saved Successfully!!!');
                                }
                            }
                        }
                    }
                    else {
                        swal("Data Already Exists");
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        }


        $scope.itemsPerPage = 5;
        $scope.currentPage = 1;

        $scope.viewdocument = function (obj) {
            
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            $scope.objdata = obj;

            apiService.create("NaacLinkage/viewuploadflies", obj).then(function (promise) {
                
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
                "NCAC351LINF_Id": docfile.ncaC351LINF_Id,
                "NCAC351LIN_Id": docfile.ncaC351LIN_Id,
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
                        apiService.create("NaacLinkage/deleteuploadfile", data).then(function (promise) {

                            if (promise.returnval === true) {
                                swal("Record Deleted successfully");

                            //}
                            //else {
                            //    swal("Record Deletion Failed");
                            //}
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

        // for comment
        $scope.getorganizationdata = function (obj) {
            apiService.create("NaacLinkage/getcomment", obj).then(function (promise) {
                if (promise !== null) {
                    $scope.commentlist = promise.commentlist;
                }
            });
        };

        // for file 
        $scope.getorganizationdata1 = function (obj) {
            apiService.create("NaacLinkage/getfilecomment", obj).then(function (promise) {
                if (promise !== null) {
                    $scope.commentlist1 = promise.commentlist1;
                }
            });
        };

        $scope.addcomments = function (obje) {
            $scope.ccc = obje.ncaC351LIN_Id;
            $scope.generalcomments = '';
            $scope.obj.generalcomments = $scope.generalcomments;
            var bb;
        };

        // for file comment
        $scope.addcomments1 = function (obje) {
            $scope.cc = obje.ncaC351LINF_Id;
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
            apiService.create("NaacLinkage/savemedicaldatawisecomments", data).then(function (promise) {
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
        // fr add file comment 

        // $scope.obj.generalcomments = "";
        $scope.savedatawisecomments1 = function (obj) {
            console.log("Save Comments");
            console.log(obj);
            var data = {
                "Remarks": obj.generalcomments,
                "filefkid": $scope.cc
            };
            apiService.create("NaacLinkage/savefilewisecomments", data).then(function (promise) {
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
        // change 
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
            $scope.NCAC351LIN_Id = 0;
            $scope.NCAC351LIN_CommYear = '';
            $scope.NCAC351LIN_PartnerName = '';
            $scope.NCAC351LIN_LinkageTitle = '';
            $scope.NCAC351LIN_LinkageNature = '';
            $scope.NCAC351LIN_LinkOfDocument = '';
            $scope.NCAC351LIN_From = '';
            $scope.NCAC351LIN_To = '';

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
