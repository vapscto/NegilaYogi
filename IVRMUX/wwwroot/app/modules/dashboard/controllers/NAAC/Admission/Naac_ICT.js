(function () {
    'use strict';
    angular
        .module('app')
        .controller('Naac_ICTController', Naac_ICTController)

    Naac_ICTController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$sce','myFactorynaac']
    function Naac_ICTController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $sce, myFactorynaac) {

        $scope.submitted = false;
        $scope.hlmH_BoysGirlsFlg = "Boys";
        var miid = myFactorynaac.get();
        if (miid > 0) {
            $scope.mI_Id = miid;
        } else {
            $scope.mI_Id = 0;
        }
        //=====================Load--data.............
        $scope.ncaC413ICT_Id = 0;
        $scope.disabled = false;
        $scope.instit = false;
        $scope.Loaddata = function () {
          
            $scope.ncaC413ICT_Id = 0;
            $scope.change_institution();
           
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            

            if ($scope.mI_Id == undefined || $scope.mI_Id == null || $scope.mI_Id == '') {
                $scope.mI_Id = 0;
            }

            apiService.getURI("Naac_ICT/loaddata", $scope.mI_Id).then(function (promise) {
        
                $scope.institutionlist = promise.institutionlist;
                $scope.mI_Id = promise.mI_Id;
                $scope.allgridlist = promise.allgridlist;
            })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }    
        //=====================saverecord....
        $scope.savedata = function () {
            if ($scope.myForm.$valid) {
                     var data = {
                         "NCAC413ICT_Id": $scope.ncaC413ICT_Id,
                         "NCAC413ICT_ICTFacility": $scope.ncaC413ICT_ICTFacility,
                         "NCAC413ICT_RoomNo": $scope.ncaC413ICT_RoomNo,
                         "filelist": $scope.materaldocuupload,
                         "MI_Id": $scope.mI_Id,
                     }
                             
                apiService.create("Naac_ICT/savedata", data).then(function (promise) {
               
                    if (promise.returnval !== null && promise.duplicate !== null) {
                        if (promise.duplicate === false) {
                            if (promise.returnval === true) {
                                if ($scope.ncaC413ICT_Id > 0) {
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
                                    if ($scope.ncaC413ICT_Id > 0) {
                                        swal('Record Not Updated Successfully!!!');
                                    }
                                    else {
                                        swal('Record Not Saved Successfully!!!');
                                    }
                                }
                            }
                        }
                        else {
                            swal("Record Already exist");
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
        //=================Activation/Deactivation
        $scope.deactiveY = function (user, SweetAlert) {
            $scope.mI_Id = user.mI_Id;
            $scope.ncaC413ICT_Id = user.ncaC413ICT_Id;

            var dystring = "";
            if (user.ncaC413ICT_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (user.ncaC413ICT_ActiveFlg === false) {
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
                        apiService.create("Naac_ICT/deactivRow", user).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
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
        //==========================Validation Selection For facility check box
        $scope.searchchkbx = "";
        $scope.all_check = function () {
            var checkStatus = $scope.usercheck;
            angular.forEach($scope.facilities_list, function (itm) {
                itm.fselected = checkStatus;
            });
        }
        $scope.togchkbx = function () {
            $scope.usercheck = $scope.facilities_list.every(function (options) {
                return options.fselected;
            });
        }
        $scope.isOptionsRequired = function () {
            return !$scope.facilities_list.some(function (options) {
                return options.fselected;
            });
        }   
        //ncaC413ICTF_StatusFlg
        $scope.editdata = function (obj) {
           
            var data = {
                "ncaC413ICT_Id": obj.ncaC413ICT_Id,
                "MI_Id": $scope.mI_Id,
            }
            apiService.create("Naac_ICT/editdata", data).then(function (promise) {
                if (promise.editlist.length > 0) {
                    $scope.instit = true;
                    $scope.ncaC413ICT_Id = promise.editlist[0].ncaC413ICT_Id;
                    $scope.ncaC413ICT_ICTFacility = promise.editlist[0].ncaC413ICT_ICTFacility;
                    $scope.ncaC413ICT_RoomNo = promise.editlist[0].ncaC413ICT_RoomNo;
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
        //===========view record
        $scope.viewdocument = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            $scope.objdata = obj;

            apiService.create("Naac_ICT/viewuploadflies", obj).then(function (promise) {
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
                "NCAC413ICTF_Id": docfile.ncaC413ICTF_Id,
                "NCAC413ICT_Id": docfile.ncaC413ICT_Id,
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
                        apiService.create("Naac_ICT/deleteuploadfile", data).then(function (promise) {

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


        // for comment
        $scope.getorganizationdata = function (obj) {
            apiService.create("Naac_ICT/getcomment", obj).then(function (promise) {
                if (promise !== null) {
                    $scope.commentlist = promise.commentlist;
                }
            });
        };

        // for file 
        $scope.getorganizationdata1 = function (obj) {
            apiService.create("Naac_ICT/getfilecomment", obj).then(function (promise) {
                if (promise !== null) {
                    $scope.commentlist1 = promise.commentlist1;
                }
            });
        };

        $scope.addcomments = function (obje) {
            $scope.ccc = obje.ncaC413ICT_Id;
            $scope.generalcomments = '';
            $scope.obj.generalcomments = $scope.generalcomments;
            var bb;
        };

        // for file comment
        $scope.addcomments1 = function (obje) {
            $scope.cc = obje.ncaC413ICTF_Id;
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
            apiService.create("Naac_ICT/savemedicaldatawisecomments", data).then(function (promise) {
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
            apiService.create("Naac_ICT/savefilewisecomments", data).then(function (promise) {
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


        $scope.materaldocuupload = [{ id: 'Materal1' }];

        $scope.addmateral = function () {
           
            var newItemNo = $scope.materaldocuupload.length + 1;

            if (newItemNo <= 10) {
                $scope.materaldocuupload.push({ 'id': 'Materal' + newItemNo });
            }
        };
        $scope.removemateral = function (index) {
          
            var newItemNo = $scope.materaldocuupload.length -1;
            $scope.materaldocuupload.splice(index, 1);

            if ($scope.materaldocuupload.length === 0) {
                //data
            }
        };
        //$scope.removemateral = function (index) {
        //    var newItemNo = $scope.materaldocuupload.length - 1;
        //    $scope.materaldocuupload.splice(index, 1);

        //    if ($scope.materaldocuupload.length === 0) {
        //        //data
        //    }
        //};


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
        //$scope.downloadview = function (pdfview) {
        //    $scope.pdfurl = pdfview;
        //    $scope.showpdf = true;
        //    $('#showpdf').modal('show');
        //};
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
        $scope.backtoview = function () {
            $scope.showpdf = false;
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
            $scope.ncaC413ICT_Id = 0;
            $scope.ncaC413ICT_ICTFacility = '';
            $scope.ncaC413ICT_RoomNo = '';

            
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

