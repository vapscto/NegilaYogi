
(function () {
    'use strict';

    angular
        .module('app')
        .controller('StudentParticipationController', StudentParticipationController);

    StudentParticipationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q', 'Excel', '$timeout', '$sce','myFactorynaac']
    function StudentParticipationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q, Excel, $timeout, $sce, myFactorynaac) {
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
            apiService.getURI("StudentParticipation/loaddata", $scope.mI_Id).then(function (promise) {

                $scope.institutionlist = promise.institutionlist;
                $scope.mI_Id = promise.mI_Id;

                $scope.yearlist = promise.yearlist;
              
                $scope.alldata = promise.alldata;
                $scope.reportlist = promise.reportlist;
            })
        }
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        //====================Save Data
        $scope.selectedSectionList = [];
        $scope.savedata = function () {
            $scope.studentlstdata = [];
            if ($scope.myForm.$valid) {
                var Pdate = $scope.NCACSP123_Date == null ? "" : $filter('date')($scope.NCACSP123_Date, "yyyy-MM-dd");
                angular.forEach($scope.studentlist, function (cls) {
                    if (cls.selected == true) {
                        $scope.studentlstdata.push(cls);
                    }
                });
                var data = {
                    "NCACSP123_Id": $scope.NCACSP123_Id,
                    "MI_Id": $scope.mI_Id,
                    "NCACSP123S_Id": $scope.NCACSP123S_Id,
                    "NCACSP123_NoOfStudParticipated": $scope.NCACSP123_NoOfStudParticipated,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "NCACSP123_AddOnProgramName": $scope.NCACSP123_AddOnProgramName,
                    "NCACSP123_Date": Pdate,
                    "studentlstdata": $scope.studentlstdata,
                    "filelist": $scope.materaldocuupload,
                }
                apiService.create("StudentParticipation/savedata", data).then(function (promise) {
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
            var data = { "NCACSP123_Id": user.ncacsP123_Id }
            apiService.create("StudentParticipation/editdata", user).then(function (promise) {
                
                if (promise.editlist.length > 0) {
                    $scope.institute_flag = true;
                    $scope.editflag = true;

                    $scope.editlist = promise.editlist;
                    $scope.mI_Id = promise.editlist[0].MI_Id;
                    $scope.NCACSP123_Id = promise.editlist[0].NCACSP123_Id;
                    $scope.NCACSP123S_Id = promise.editlist[0].NCACSP123S_Id;
                    $scope.ASMAY_Id = promise.editlist[0].ASMAY_Id;
                    $scope.NCACSP123_AddOnProgramName = promise.editlist[0].NCACSP123_AddOnProgramName;
                    $scope.NCACSP123_NoOfStudParticipated = promise.editlist[0].NCACSP123_NoOfStudParticipated;
                    $scope.NCACSP123_Date = new Date(promise.editlist[0].NCACSP123_Date);
                    //$scope.AMCO_Id = promise.editlist[0].AMCO_Id;
                    $scope.Masterbranch = promise.branchlist; 

                    $scope.courselist = promise.courselist;
                 
                    $scope.AMB_Id = promise.editlist[0].AMB_Id;

                    $scope.branch_id = promise.editlist[0].AMB_Id;

                    $scope.studentlist = promise.studentlist;
                    angular.forEach($scope.studentlist, function (ss) {
                        angular.forEach(promise.editlist, function (tt) {
                            if (ss.amcsT_Id == tt.AMCST_Id) {
                                ss.selected = true;
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
                };
            });
        }

        //===========deactive and active 
        $scope.deactiveStudent = function (usersem, SweetAlert) {
            //  $scope.NCACSP123S_Id = usersem.NCACSP123S_Id
            var dystring = "";
            if (usersem.NCACSP123S_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (usersem.NCACSP123S_ActiveFlg == false) {
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
                        apiService.create("StudentParticipation/deactivY", usersem).
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
        //========Studentlist CheckBox Field Validation===========//
        $scope.isOptionsRequired = function () {
            return !$scope.studentlist.some(function (item) {
                return item.selected;
            });
        }
        //=======selection of checkbox....
        $scope.togchkbxC = function () {
            $scope.usercheckC = $scope.studentlist.every(function (role) {
                return role.selected;
            });
            var count2 = 0;
            angular.forEach($scope.studentlist, function (itm) {
                if (itm.selected == true) {
                    count2 = count2 + 1;
                }
            });
            $scope.NCACSP123_NoOfStudParticipated = count2;
        }


        //---------all checkbox Select...
        $scope.NCACSP123_NoOfStudParticipated = 0;
        $scope.all_checkC = function (all) {
            $scope.usercheckC = all;
            var toggleStatus = $scope.usercheckC;
            var count2 = 0;
            angular.forEach($scope.studentlist, function (role) {
                role.selected = toggleStatus;
                if (role.selected == true) {
                    count2 += 1;
                }
                else {
                    count2 = 0;
                }
            })
            $scope.NCACSP123_NoOfStudParticipated = count2;
        }

        //========================get Program(Course + Branch)
        $scope.Get_program = function () {
            $scope.studentlist = [];
            $scope.Masterbranch = [];
            $scope.AMB_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,               
                "MI_Id": $scope.mI_Id,
            }
            apiService.create("StudentParticipation/get_coursebrnch", data).then(function (promise) {
                $scope.courselist = promise.courselist;
            })
        }

        //===================Get Branch
        $scope.get_branch = function () {
            $scope.studentlist = [];            
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMB_Id": $scope.AMB_Id,
                "MI_Id": $scope.mI_Id,
            }
            apiService.create("StudentParticipation/get_branch", data).then(function (promise) {
                $scope.Masterbranch = promise.branchlist;
                //$scope.branch_id = $scope.Masterbranch[0].amB_Id;
            })
        }

        // ====================== Get Student List
        $scope.get_student = function () {
            $scope.studentlist = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                //"AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.branch_id,
                "MI_Id": $scope.mI_Id,
            }
            apiService.create("StudentParticipation/get_student", data).then(function (promise) {
                $scope.studentlist = promise.studentlist;
            })
        };
        //===========view record
        $scope.viewdocument = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            $scope.objdata = obj;

            apiService.create("StudentParticipation/viewuploadflies", obj).then(function (promise) {
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

            //var dystring = "";
            //var data = {
            //    "NCACSP123F_Id": usersem.ncacsP123F_Id,
            //    "NCACSP123_Id": usersem.ncacsP123_Id,
            //}
            //if (usersem.ncacsP123F_Id > 0) {
            //    dystring = "Delete";
            //}
            //else {
            //    dystring = "Not Delete";
            //}
            //swal({
            //    title: "Are you sure?",
            //    text: "Do You Want To " + dystring + " Record?",
            //    type: "warning",
            //    showCancelButton: true,
            //    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
            //    cancelButtonText: "Cancel",
            //    closeOnConfirm: false,
            //    closeOnCancel: false
            //},
            //    function (isConfirm) {
            //        if (isConfirm) {

            var data = {
                "NCACSP123F_Id": docfile.ncacsP123F_Id,
                "NCACSP123_Id": docfile.ncacsP123_Id,
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
                        apiService.create("StudentParticipation/deleteuploadfile", data).then(function (promise) {
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



        // view row wise comments
        $scope.getorganizationdata = function (obj) {
            debugger;
            apiService.create("StudentParticipation/getcomment", obj).then(function (promise) {
                if (promise !== null) {
                    $scope.commentlist = promise.commentlist;
                }
            });
        };

        // view file wise comments
        $scope.getorganizationdata1 = function (obj) {
            debugger;
            apiService.create("StudentParticipation/getfilecomment", obj).then(function (promise) {
                if (promise !== null) {
                    $scope.commentlist1 = promise.commentlist1;
                }
            });
        };

        $scope.addcomments = function (obje) {
            debugger;
            $scope.ccc = obje.NCACSP123_Id;
            $scope.generalcomments = '';
            $scope.obj.generalcomments = $scope.generalcomments;
            var bb;
        };

        // for file comment
        $scope.addcomments1 = function (obje) {
            debugger;
            $scope.cc = obje.ncacsP123F_Id;
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
            apiService.create("StudentParticipation/savemedicaldatawisecomments", data).then(function (promise) {
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
            apiService.create("StudentParticipation/savefilewisecomments", data).then(function (promise) {
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
