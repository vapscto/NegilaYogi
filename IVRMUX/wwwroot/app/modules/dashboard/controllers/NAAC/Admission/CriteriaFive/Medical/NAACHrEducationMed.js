
(function () {
    'use strict';

    angular
        .module('app')
        .controller('NAACHrEducationMedController', NAACHrEducationMedController);

    NAACHrEducationMedController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter', '$sce','myFactorynaac'];

    function NAACHrEducationMedController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter, $sce, myFactorynaac) {


        //======================for pagination
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.clrr = function () {
                                    $scope.ASMAY_Id = '';
                                    $scope.AMCO_Id = '';
                                    $scope.AMB_Id = '';
                                    $scope.NCAC522HRED_InstitutionName = '';
                                    $scope.NCAC522HRED_AdmittedProgram = '';
            $scope.NCAC522HRED_AdmittedDept = '';
            $scope.NCAC522HRED_HrEduEnrollStudentNo = '';

                                    $scope.NCAC522HRED_Id = 0;
            $scope.uploadmateraldocuments1 = [];
            $scope.materaldocuupload = [{ id: 'Materal1' }];
            $scope.ASMAY_Id = '';
        }
        var miid = myFactorynaac.get();

        if (miid > 0) {
            $scope.mI_Id = miid;
        } else {
            $scope.mI_Id = 0;
        }

        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.search = "";


        //==================get course
        $scope.get_course = function () {

           
            $scope.AMCO_Id = "";
            $scope.courselist = [];
            $scope.AMB_Id = "";
            $scope.branchlist = [];
           
        
            var data = {
                "MI_Id": $scope.mI_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
            }
            apiService.create("NaacActivity/get_course", data).then(function (promise) {
                if (promise.courselist.length > 0) {
                    $scope.courselist = promise.courselist;
                }
                else {
                    swal("For Selected Year, Course is not Available!");
                }
            });
        }

        //==================get branch
        $scope.get_branch = function () {

          
            $scope.AMB_Id = "";
            $scope.branchlist = [];
          
      

            var data = {
                "MI_Id": $scope.mI_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
            }
            apiService.create("NaacActivity/get_branch", data).then(function (promise) {
                if (promise.branchlist.length > 0) {
                    $scope.branchlist = promise.branchlist;
                }
                else {
                    swal('For Selected Course, Branch is not Available!')
                }

            });
        }


        ///======================================Load Data.
        $scope.loaddata = function () {
            $scope.clrr();

            if ($scope.mI_Id == undefined || $scope.mI_Id == null || $scope.mI_Id == '') {
                $scope.mI_Id = 0;
            }  
            apiService.getURI("NAACHrEducation/loaddata", $scope.mI_Id).then(function (promise) {
                $scope.institutionlist = promise.institutionlist;
                $scope.mI_Id = promise.mI_Id;

                $scope.allacademicyear = promise.allacademicyear;
                $scope.alldata = promise.alldatalist;
            })
        }


        $scope.getorganizationdata = function (obj) {
            apiService.create("NAACHrEducation/getcomment", obj).then(function (promise) {
                if (promise !== null) {
                    $scope.commentlist = promise.commentlist;
                }
            });
        };

        // for file 
        $scope.getorganizationdata1 = function (obj) {
            debugger;
            obj.ncaC522HREDF_Id = obj.cfileid;
            apiService.create("NAACHrEducation/getfilecomment", obj).then(function (promise) {
                if (promise !== null) {
                    $scope.commentlist1 = promise.commentlist;
                }
            });
        };

        $scope.interactedc = function (field) {
            return $scope.submittedc;
        };
        $scope.interactedc1 = function (field) {
            return $scope.submittedc1;
        };
        // for file comment
        $scope.addcomments1 = function (obje) {
            debugger;
            $scope.submittedc1 = false;
            $scope.cc = obje.cfileid;
            $scope.generalcomments = '';
            $scope.obj.generalcomments = $scope.generalcomments;
            var bb;
        };
        $scope.commentlist = [];
        $scope.addcomments = function (obje) {
            $scope.submittedc = false;
            $scope.ccc = obje.ncaC522HRED_Id;
            $scope.generalcomments = '';
            $scope.obj.generalcomments = $scope.generalcomments;
            var bb;
        };

        //*************** Save DATA WISE Comments ***************//
        $scope.savedatawisecomments = function (obj) {
            debugger;
            if ($scope.myFormc.$valid) {
                console.log("Save Comments");
                console.log(obj);
                var data = {
                    "Remarks": obj.generalcomments,
                    "filefkid": $scope.ccc
                };
                apiService.create("NAACHrEducation/savemedicaldatawisecomments", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnval === true) {
                            swal("Comments Saved Successfully");
                        } else {
                            swal("Failed To Save Comments");
                        }
                        $('#mymodaladdcomments').modal('hide');
                        $('#mymodalviewuploaddocument').modal('hide');
                        $scope.valued = "2";

                    }
                });
            }

            else {
                $scope.submittedc = true;
            }
        }
        // fr add file comment 

        // $scope.obj.generalcomments = "";
        $scope.savedatawisecomments1 = function (obj) {
            debugger;
            if ($scope.myFormc1.$valid) {
                var data = {
                    "Remarks": obj.generalcomments,
                    "filefkid": $scope.cc
                };
                apiService.create("NAACHrEducation/savefilewisecomments", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnval === true) {
                            swal("Comments Saved Successfully");
                        } else {
                            swal("Failed To Save Comments");
                        }
                        $('#mymodaladdcomments1').modal('hide');
                        $('#mymodalviewuploaddocument1').modal('hide');
                        $scope.valued = "2";

                    }
                });
            } else {
                $scope.submittedc1 = true;
            }
        };

        //=====================Sorting
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        //==============================save data For Tab1

        $scope.savedatatab1 = function () {
            debugger;
            $scope.studentlstdata = [];
            if ($scope.myForm.$valid) {

      
                var data = {
                    "MI_Id": $scope.mI_Id,
                        "NCAC522HRED_Id": $scope.NCAC522HRED_Id,
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "AMCO_Id": $scope.AMCO_Id,
                        "AMB_Id": $scope.AMB_Id,
                        "NCAC522HRED_InstitutionName": $scope.NCAC522HRED_InstitutionName,
                        "NCAC522HRED_AdmittedProgram": $scope.NCAC522HRED_AdmittedProgram,
                        "NCAC522HRED_AdmittedDept": $scope.NCAC522HRED_AdmittedDept,
                        "NCAC522HRED_HrEduEnrollStudentNo": $scope.NCAC522HRED_HrEduEnrollStudentNo,
                        filelist: $scope.materaldocuupload,
                       
                    }
               

                apiService.create("NAACHrEducation/save", data).then(function (promise) {

                    if (promise.duplicate != null && promise.returnval != null) {
                        if (promise.duplicate == false) {
                            if (promise.returnval == true) {
                                if ($scope.NCAC522HRED_Id > 0) {
                                    swal('Data Updated Successfully!')
                                }
                                else {
                                    swal('Data Saved Successfully!')
                                }
                                $scope.loaddata();
                            }
                            else {
                                if ($scope.NCAC522HRED_Id > 0) {
                                    swal('Data Not Updated Successfully!')
                                }
                                else {
                                    swal('Data Not Saved Successfully!')
                                }
                            }
                        }
                        else {
                            swal('Record already Exist!')
                        }
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        };

        //==========================edit data for tab1
        $scope.edittab1 = function (data) {
            debugger;
            apiService.create("NAACHrEducation/EditData", data).then(function (promise) {

                $scope.editlist = promise.editlist;
                $scope.NCAC522HRED_Id = promise.editlist[0].ncaC522HRED_Id;
                $scope.NCAC522HRED_HrEduEnrollStudentNo = promise.editlist[0].ncaC522HRED_HrEduEnrollStudentNo;
                $scope.ASMAY_Id = promise.editlist[0].asmaY_Id;
                $scope.NCAC522HRED_AdmittedDept = promise.editlist[0].ncaC522HRED_AdmittedDept;
                $scope.NCAC522HRED_AdmittedProgram = promise.editlist[0].ncaC522HRED_AdmittedProgram;
                $scope.NCAC522HRED_InstitutionName = promise.editlist[0].ncaC522HRED_InstitutionName;
                debugger;
                $scope.AMCO_Id = promise.editlist[0].amcO_Id;
                if ($scope.AMCO_Id != "" && $scope.AMCO_Id != undefined && $scope.AMCO_Id != null) {
                    $scope.get_course();
                    $scope.AMCO_Id = promise.editlist[0].amcO_Id;
                }

                $scope.AMB_Id = promise.editlist[0].amB_Id;
                if ($scope.AMB_Id != "" && $scope.AMB_Id != undefined && $scope.AMB_Id != null) {
                    $scope.get_branch();
                    $scope.AMB_Id = promise.editlist[0].amB_Id;
                }

               
                
                $scope.materaldocuupload = [{ id: 'Materal1' }];
                if (promise.editfiles != null && promise.editfiles.length > 0) {
                    $scope.materaldocuupload = promise.editfiles;
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

            })
        }


        //===========deactive and active for Tab1
        $scope.deactivYTab1 = function (usersem, SweetAlert) {
            var dystring = "";
            if (usersem.ncaC522HRED_ActiveFlg == true) {
                dystring = "Deactivated";
            }
            else if (usersem.ncaC522HRED_ActiveFlg == false) {
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
                        apiService.create("NAACHrEducation/deactiveStudent", usersem).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring +  " Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + " Successfully!!!");
                                }
                                $scope.loaddata();
                            })
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        }

        //For Cancel data record 
        $scope.Clearid = function () {
            $scope.submitted = false;
            $scope.loaddata();
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //==================filter Name
        $scope.searchValue = "";
        



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
                $scope.materaldocuupload = [{ id: 'Materal1' }];
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
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document") {
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


        $scope.objdata = [];
        $scope.viewdocument = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            $scope.objdata = obj;

            apiService.create("NAACHrEducation/viewuploadflies", obj).then(function (promise) {
                if (promise !== null) {
                    debugger;
                    $scope.uploadfilesdetails = promise.editfiles;
                    if (promise.editfiles !== null && promise.editfiles.length > 0) {
                        $scope.uploaddocfiles = promise.editfiles;

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



        $scope.deleteuploadfile = function (obj) {

            var data = {
                "NCAC522HREDF_Id": obj.cfileid
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
                        apiService.create("NAACHrEducation/deleteuploadfile", data).then(function (promise) {
                            //if (promise.already_cnt === true) {
                            //    swal("You Can Not Deactivate This Record,It Has Dependency");
                            //}
                            //else {
                            if (promise.returnval === true) {
                                swal("Record Deleted successfully");
                            }
                            else {
                                swal("Record Deletion Failed");
                            }
                            //}
                            //$('#popup11').modal('hide');

                            $scope.viewdocument($scope.objdata)
                        });
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        };



        //////end////

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
