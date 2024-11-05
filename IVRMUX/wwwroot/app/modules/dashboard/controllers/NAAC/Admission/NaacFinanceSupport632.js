(function () {
    'use strict';
    angular
        .module('app')
        .controller('NaacFinanceSupport632', NaacFinanceSupport632)

    NaacFinanceSupport632.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter', '$sce','myFactorynaac'];
    function NaacFinanceSupport632($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter, $sce, myFactorynaac) {

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.sort = function (keyname) {
            $scope.propertyName = keyname;   //set the propertyName to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        //==============================page load
        var miid = myFactorynaac.get();
        if (miid > 0) {
            $scope.mI_Id = miid;
        } else {
            $scope.mI_Id = 0;
        }
        $scope.NCAC632FINSUP_Id = 0;
        $scope.instit = false;
        $scope.loaddata = function () {
           
            $scope.NCAC632FINSUP_Id = 0;
            $scope.change_institution();
            if ($scope.mI_Id == undefined || $scope.mI_Id == null || $scope.mI_Id == '') {
                $scope.mI_Id = 0;
            }
            //var pageid = 2;
            apiService.getURI("NaacFinanceSupport632/loaddata", $scope.mI_Id).then(function (promise) {
               
                $scope.institutionlist = promise.institutionlist;
                $scope.mI_Id = promise.mI_Id;
                    $scope.allacademicyear = promise.allacademicyear;
                    $scope.alldata1 = promise.alldata1;
                });
        };

        //=========================Save records
        $scope.submitted = false;
        $scope.save = function () {
           
            if ($scope.NCAC632FINSUP_ConferenceProfBodyFlg == '1') {
                $scope.NCAC632FINSUP_ConferenceProfBodyFlg = true;
            } else {
                $scope.NCAC632FINSUP_ConferenceProfBodyFlg = false;
            }
            if ($scope.myForm.$valid) {
                var data = {
                    "NCAC632FINSUP_Id": $scope.NCAC632FINSUP_Id,
                    "asmaY_Id": $scope.NCAC632FINSUP_Year,
                    "NCAC632FINSUP_ConferenceProfBodyFlg": $scope.NCAC632FINSUP_ConferenceProfBodyFlg,
                    "NCAC632FINSUP_TeacherName": $scope.NCAC632FINSUP_TeacherName,
                    "NCAC632FINSUP_Name": $scope.NCAC632FINSUP_Name,
                    "NCAC632FINSUP_NameOfMembership": $scope.NCAC632FINSUP_NameOfMembership,
                    "NCAC632FINSUP_AmountPaid": $scope.NCAC632FINSUP_AmountPaid,
               
                    "NCAC632FINSUP_PAN": $scope.NCAC632FINSUP_PAN,
                    filelist: $scope.materaldocuupload,
                    "MI_Id": $scope.mI_Id
                }
                apiService.create("NaacFinanceSupport632/save", data).then(function (promise) {
             
                    if (promise.duplicate == true) {
                        swal("Data Already Exists");
                    }
                    else if (promise.msg == 'saved') {
                        swal("Data Saved Successfully...!!!");
                        $state.reload();
                    }
                    else if (promise.msg == 'Failed') {
                        swal("Data not Saved Successfully...!!!");
                    }
                    else if (promise.msg == 'updated') {
                        swal("Data Updated Successfully ...!!!");
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

        //==========================================Row Active/Deactive
        $scope.deactive = function (usersem, SweetAlert) {
            var dystring = "";
            if (usersem.ncaC632FINSUP_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (usersem.ncaC632FINSUP_ActiveFlg == false) {
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
                        apiService.create("NaacFinanceSupport632/deactive", usersem).
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
        ///=================================Clear
        $scope.Clearid = function () {
            $state.reload();
        };

     

   

        ///=========clear upload field data......
        $scope.remove_file = function () {
            $scope.file_detail = "";
            $scope.notice = "";
        }
        $scope.search = "";
        //===================================Record Edit
        $scope.edittab1 = function (user) {
           
            var data = {
                "NCAC632FINSUP_Id": user.ncaC632FINSUP_Id,
                "MI_Id": user.mI_Id
            }
            apiService.create("NaacFinanceSupport632/EditData", data).then(function (promise) {
    
                if (promise.editlist.length > 0) {
                    $scope.instit = true;
                    $scope.NCAC632FINSUP_Id = promise.editlist[0].ncaC632FINSUP_Id;
                    $scope.NCAC632FINSUP_Year = promise.editlist[0].ncaC632FINSUP_Year;
                    $scope.NCAC632FINSUP_PAN = promise.editlist[0].ncaC632FINSUP_PAN;
                    $scope.NCAC632FINSUP_NameOfMembership = promise.editlist[0].ncaC632FINSUP_NameOfMembership;
                    $scope.NCAC632FINSUP_TeacherName = promise.editlist[0].ncaC632FINSUP_TeacherName;
                    $scope.NCAC632FINSUP_Name = promise.editlist[0].ncaC632FINSUP_Name;
                    $scope.NCAC632FINSUP_ConferenceProfBodyFlg = promise.editlist[0].ncaC632FINSUP_ConferenceProfBodyFlg;
                      $scope.NCAC632FINSUP_AmountPaid = promise.editlist[0].ncaC632FINSUP_AmountPaid;
                      $scope.mI_Id = promise.editlist[0].mI_Id;


                      if (promise.editFileslist.length > 0) {
                    $scope.materaldocuupload = promise.editFileslist;
                    angular.forEach($scope.materaldocuupload, function (ddd) {
                        var img = ddd.cfilepath;
                        var imagarr = img.split('.');
                        var lastelement = imagarr[imagarr.length - 1];
                        ddd.filetype = lastelement;
                        if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                            data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + ddd.cfilepath;
                        }
                      })


                  }
                  else {
                      $scope.materaldocuupload = [{ id: 'Materal1' }];
                  }

                }
            });
        }


        $scope.viewdocument = function (obj) {
     
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            $scope.objdata = obj;

            apiService.create("NaacFinanceSupport632/viewuploadflies", obj).then(function (promise) {
         
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
                "NCAC632FINSUPF_Id": docfile.ncaC632FINSUPF_Id,
                "NCAC632FINSUP_Id": docfile.ncaC632FINSUP_Id,
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
                        apiService.create("NaacFinanceSupport632/deleteuploadfile", data).then(function (promise) {
                         
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

            $scope.NCAC632FINSUP_Id = 0;
            $scope.NCAC632FINSUP_Year = '';
            $scope.NCAC632FINSUP_TeacherName = '';
            $scope.NCAC632FINSUP_Name = '';
            $scope.NCAC632FINSUP_ConferenceProfBodyFlg = '';
            $scope.NCAC632FINSUP_AmountPaid = '';
            $scope.NCAC632FINSUP_PAN = '';
            $scope.NCAC632FINSUP_NameOfMembership = '';
           
            $scope.materaldocuupload = [];
          
            $scope.materaldocuupload = [{ id: 'Materal1' }];
        }


        // view row wise comments
        $scope.getorganizationdata = function (obj) {
            debugger;
            apiService.create("NaacFinanceSupport632/getcomment", obj).then(function (promise) {
                if (promise !== null) {
                    $scope.commentlist = promise.commentlist;
                }
            });
        };

        // view file wise comments
        $scope.getorganizationdata1 = function (obj) {
            debugger;
            apiService.create("NaacFinanceSupport632/getfilecomment", obj).then(function (promise) {
                if (promise !== null) {
                    $scope.commentlist1 = promise.commentlist1;
                }
            });
        };

        $scope.addcomments = function (obje) {
            debugger;
            $scope.ccc = obje.ncaC632FINSUP_Id;
            $scope.generalcomments = '';
            $scope.obj.generalcomments = $scope.generalcomments;
            var bb;
        };

        // for file comment
        $scope.addcomments1 = function (obje) {
            debugger;
            $scope.cc = obje.ncaC632FINSUPF_Id;
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
            apiService.create("NaacFinanceSupport632/savemedicaldatawisecomments", data).then(function (promise) {
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
            apiService.create("NaacFinanceSupport632/savefilewisecomments", data).then(function (promise) {
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
