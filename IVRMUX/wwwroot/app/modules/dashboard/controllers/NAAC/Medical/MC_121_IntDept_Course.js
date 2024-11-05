
(function () {
    'use strict';

    angular
        .module('app')
        .controller('MC_121_IntDept_Course', MC_121_IntDept_Course);

    MC_121_IntDept_Course.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q', 'Excel', '$timeout', '$sce', 'myFactorynaac']
    function MC_121_IntDept_Course($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q, Excel, $timeout, $sce, myFactorynaac) {
        $scope.searc_button = true;

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.sort = function (keyname) {
            $scope.propertyName = keyname;   //set the propertyName to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        $scope.submitted = false;
        $scope.Clearid = function () {
            $state.reload();
        };
        $scope.searchValue = "";
        $scope.search = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        var miid = myFactorynaac.get();
        if (miid > 0) {
            $scope.mI_Id = miid;
        } else {
            $scope.mI_Id = 0;
        }
        //==================Page Load
        $scope.institute_flag = false;
        $scope.loaddata = function () {
            $scope.submitted = false;
            $scope.ASMAY_Id = "";
            $scope.AMCO_Id = "";
            $scope.AMB_Id = "";
            $scope.studentlist = [];
            $scope.materaldocuupload = [{ id: 'Materal1' }];
            if ($scope.mI_Id == undefined || $scope.mI_Id == null || $scope.mI_Id == '') {
                $scope.mI_Id = 0;
            }
            apiService.getURI("MC_121_IntDept_Course/loaddata", $scope.mI_Id).then(function (promise) {

                $scope.institutionlist = promise.institutionlist;
                $scope.mI_Id = promise.mI_Id;

                $scope.yearlist = promise.yearlist;
                $scope.departmentlist = promise.departmentlist;
                $scope.alldata = promise.alldata;
                
            })
        }
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        //====================Save Data
        $scope.selectedSectionList = [];
        $scope.savedata = function () {
            
            if ($scope.myForm.$valid) {               
                
                var data = {
                    "NMC121IDC_Id": $scope.NMC121IDC_Id,
                    "MI_Id": $scope.mI_Id,                    
                    "NMC121IDC_NoOfCourse": $scope.NMC121IDC_NoOfCourse,
                    "ASMAY_Id": $scope.ASMAY_Id, 
                    "HRMD_Id": $scope.HRMD_Id,                     
                    "AMCO_Id": $scope.AMCO_Id,                     
                    "filelist": $scope.materaldocuupload,
                }
                apiService.create("MC_121_IntDept_Course/savedata", data).then(function (promise) {
                    if (promise.msg == 'saved') {
                        swal("Record Saved Successfully...!!!")
                        $state.reload();
                    }
                    else if (promise.msg == 'notsaved') {
                        swal("Record Not Saved Successfully...!!!")
                    }
                    else if (promise.msg == 'updated') {
                        swal("Record Updated Successfully...!!!")
                        $state.reload();
                    }
                    else if (promise.msg == 'notupdated') {
                        swal("Record Not Updated Successfully...!!!")
                    }
                    else {
                        swal('Record Not Save/update');
                    }
                })
            }

            else {
                $scope.submitted = true;
            }
        };
        //======================= to edit
        $scope.editflag = false;
        $scope.edit = function (user) {
            var data = { "NMC121IDC_Id": user.NMC121IDC_Id }
            apiService.create("MC_121_IntDept_Course/editdata", user).then(function (promise) {

                if (promise.editlist.length > 0) {
                    $scope.institute_flag = true;
                    $scope.editflag = true;

                    $scope.editlist = promise.editlist;
                    $scope.mI_Id = promise.editlist[0].mI_Id;
                    $scope.NMC121IDC_Id = promise.editlist[0].nmC121IDC_Id;                    
                    $scope.ASMAY_Id = promise.editlist[0].asmaY_Id;                   
                    $scope.NMC121IDC_NoOfCourse = promise.editlist[0].nmC121IDC_NoOfCourse;                    
                    
                    $scope.courselist = promise.courselist;                  
                    $scope.HRMD_Id = promise.editlist[0].hrmD_Id;   

                    $scope.get_Course($scope.ASMAY_Id, $scope.mI_Id);
                    $scope.courselist = promise.courselist;

                    $scope.AMCO_Id = promise.editlist[0].amcO_Id;

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
                };
            });
        }

        //===========deactive and active 
        $scope.deactiveStudent = function (usersem, SweetAlert) {
            //  $scope.NCACSP123S_Id = usersem.NCACSP123S_Id
            var dystring = "";
            if (usersem.nmC121IDC_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (usersem.nmC121IDC_ActiveFlag == false) {
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
                        apiService.create("MC_121_IntDept_Course/deactivY", usersem).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d " + "Successfully!!!");
                                    $state.reload();
                                }
                                else {
                                    swal("Record Not " + dystring + "d" + " Successfully!!!");
                                }
                                // $scope.mappedstudentlist = promise.mappedstudentlist;
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
        
        //===================Get Branch
        $scope.get_Course = function () {
            $scope.studentlist = [];
            $scope.AMB_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,                
                "MI_Id": $scope.mI_Id,
            }
            apiService.create("MC_121_IntDept_Course/get_Course", data).then(function (promise) {
                $scope.courselist = promise.courselist;
            })
        }

        
        //===========view record
        $scope.viewdocument = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            $scope.objdata = obj;

            apiService.create("MC_121_IntDept_Course/viewuploadflies", obj).then(function (promise) {
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


        //delete/upload file
        $scope.deleteuploadfile = function (docfile) {
            var data = {
                "NMC121IDCF_Id": docfile.nmC121IDCF_Id,
                "NMC121IDC_Id": docfile.nmC121IDC_Id,
                "MI_Id": docfile.mI_Id,
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
                        apiService.create("MC_121_IntDept_Course/deleteuploadfile", data).then(function (promise) {
                            if (promise.returnval == true) {
                                swal("Record Deleted successfully");
                                $scope.viewuploadflies = promise.viewuploadflies;
                                //$scope.ncaC414BD_Id = promise.viewuploadflies[0].ncaC414BD_Id;
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
                                else {
                                    $scope.uploaddocfiles = [];
                                }
                            }
                        })
                    }
                    else {
                        swal("Record Deletation Cancelled!!!");
                    }
                });
        }

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



        // view row wise comments
        $scope.getorganizationdata = function (obj) {
            debugger;
            apiService.create("MC_121_IntDept_Course/getcomment", obj).then(function (promise) {
                if (promise !== null) {
                    $scope.commentlist = promise.commentlist;
                }
            });
        };

        // view file wise comments
        $scope.getorganizationdata1 = function (obj) {
            debugger;
            apiService.create("MC_121_IntDept_Course/getfilecomment", obj).then(function (promise) {
                if (promise !== null) {
                    $scope.commentlist1 = promise.commentlist1;
                }
            });
        };

        $scope.addcomments = function (obje) {
            debugger;
            $scope.ccc = obje.nmC121IDC_Id;
            $scope.generalcomments = '';
            $scope.obj.generalcomments = $scope.generalcomments;
            var bb;
        };

        // for file comment
        $scope.addcomments1 = function (obje) {
            debugger;
            $scope.cc = obje.nmC121IDCF_Id;
            $scope.generalcomments = '';
            $scope.obj.generalcomments = $scope.generalcomments;
            var bb;
        };

        //*************** save row wise comments ***************//
        $scope.savedatawisecomments = function (obj) {
            debugger;
            console.log("Save Comments");
            console.log(obj);
            var data = {
                "Remarks": obj.generalcomments,
                "filefkid": $scope.ccc
            };
            apiService.create("MC_121_IntDept_Course/savemedicaldatawisecomments", data).then(function (promise) {
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


        // save file wise comments 

        // $scope.obj.generalcomments = "";
        $scope.savedatawisecomments1 = function (obj) {
            debugger;
            console.log("Save Comments");
            console.log(obj);
            var data = {
                "Remarks": obj.generalcomments,
                "filefkid": $scope.cc
            };
            apiService.create("MC_121_IntDept_Course/savefilewisecomments", data).then(function (promise) {
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
