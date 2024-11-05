(function () {
    'use strict';

    angular
        .module('app')
        .controller('ClgMasterCourseBranchMapController', ClgMasterCourseBranchMapController);

    ClgMasterCourseBranchMapController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q','$window'];

    function ClgMasterCourseBranchMapController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $window) {
        $scope.searc_button = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }


        $scope.chkcurslst = function (oobj) {
            
            angular.forEach($scope.MasterCourseList, function (itm) {
                if (itm.amcO_Id == oobj) {
                    $scope.noofsemlsst = itm.amcO_NoOfSemesters;
                }
            })
        }

        $scope.selcnt = {};
        $scope.chksemlsst = function (objid) {
           
            var cnt = 0;
            angular.forEach($scope.mastersemesterlist, function (itmm) {
                if (itmm.Selected1 == true) {
                    cnt++;
                    $scope.selcnt = cnt;
                    if (cnt > $scope.noofsemlsst) {
                        swal("you can select only" + " " + $scope.noofsemlsst + " " + "semester");
                        angular.forEach($scope.mastersemesterlist, function (tstobj) {
                            if (tstobj.amsE_Id == objid) {
                                tstobj.Selected1 = false;
                            }
                        })
                        return;
                    }
                }
            })
        }

        $scope.getAllDetails = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var id = 2;

            
            apiService.getURI("ClgMasterCourseBranchMap/getalldetails", id).
    then(function (promise) {
        
        $scope.MasterCourseList = promise.masterCourseList;
        $scope.Masterbranch = promise.masterbranchList;
        $scope.mastersemesterlist = promise.mastersemesterlist;
        $scope.griddetails = promise.grid;
        $scope.presentCountgrid = $scope.griddetails.length;


        $scope.cbcsyearlist = promise.cbcsyearlist;
        $scope.electiveyearlist = promise.electiveyearlist;

    })

        }


        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.isOptionsRequired1 = function () {
            return !$scope.mastersemesterlist.some(function (options) {
                return options.Selected1;
            });
        }
        $scope.selectedSectionList = [];

        $scope.savedata = function () {
            
            if ($scope.myForm.$valid) {

                if ($scope.selcnt != $scope.noofsemlsst) {
                    swal("Please select any " + " " + $scope.noofsemlsst + " " + "semester");
                    return;
                }

                if ($scope.mastersemesterlist != "" && $scope.mastersemesterlist != null) {
                    if ($scope.mastersemesterlist.length > 0) {
                        for (var i = 0; i < $scope.mastersemesterlist.length; i++) {
                            if ($scope.mastersemesterlist[i].Selected1 == true) {
                                $scope.selectedSectionList.push($scope.mastersemesterlist[i]);

                            }
                        }
                    }
                }
                if ($scope.notice == undefined || $scope.notice == "") {
                    var data = {
                        "AMCOBM_Id": $scope.AMCOBM_Id,
                        "AMCO_Id": $scope.AMCO_Id,
                        "AMB_Id": $scope.AMB_Id,
                        "semester_list": $scope.selectedSectionList,

                        "AMCOBM_Code": $scope.AMCOBM_Code,
                        "AMCOBM_CBCSFlg": $scope.AMCOBM_CBCSFlg,
                        "AMCOBM_CBCSIntroYear": $scope.AMCOBM_CBCSIntroYear,
                        "AMCOBM_ElectiveFlg": $scope.AMCOBM_ElectiveFlg,
                        "AMCOBM_ElectiveIntroYear": $scope.AMCOBM_ElectiveIntroYear,
                    }
                }
                else {
                    var att_file = "";
                    $scope.filedoc = [];
                    $scope.filedoc = [$scope.notice];
                    if ($scope.filedoc.length > 0) {
                        for (var i = 0; i < $scope.filedoc.length; i++) {
                            att_file = $scope.filedoc[0];
                        }
                    }
                    var att_file11 = att_file.toString();

                    var data = {
                        "AMCOBM_Id": $scope.AMCOBM_Id,
                        "AMCO_Id": $scope.AMCO_Id,
                        "AMB_Id": $scope.AMB_Id,
                        "semester_list": $scope.selectedSectionList,

                        "AMCOBM_Code": $scope.AMCOBM_Code,
                        "AMCOBM_CBCSFlg": $scope.AMCOBM_CBCSFlg,
                        "AMCOBM_CBCSIntroYear": $scope.AMCOBM_CBCSIntroYear,
                        "AMCOBM_ElectiveFlg": $scope.AMCOBM_ElectiveFlg,
                        "AMCOBM_ElectiveIntroYear": $scope.AMCOBM_ElectiveIntroYear,
                        "AMCOBM_FileName": $scope.file_detail,
                        "AMCOBM_FilePath": att_file11,
                       
                        
                    }
                }
               
                apiService.create("ClgMasterCourseBranchMap/Savedetails", data).then(function (promise) {
                    

                    if (promise.message == "Add") {
                        if (promise.returnval == true) {
                            swal("Record Saved Successfully");
                        } else {
                            swal("Failed To Save Record");
                        }
                    }
                    else if (promise.message == "Update") {
                        if (promise.returnval == true) {
                            swal("Record Updated Successfully");
                        } else {
                            swal("Failed To Update Record");
                        }
                    } else if (promise.message == "Exists") {
                        swal("You Can Not Map Same Branch For Multiple Course");
                    }
                    else {
                        swal(" Something Went Wrong . Kindly Contact Administrator!!!");
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.editflag1 = false;
        $scope.edit = function (user) {

            angular.forEach($scope.mastersemesterlist, function (se) {
                se.Selected1 = false;

            })


            apiService.create("ClgMasterCourseBranchMap/edit", user).then(function (promise) {
                $scope.editdata = promise.editdata;
                $scope.editflag1 = true;
                angular.forEach($scope.mastersemesterlist, function (sec1) {
                    sec1.Selected1 = false;
                    sec1.editflag = false;
                })
                var cnt = 0;
                angular.forEach($scope.editdata, function (mas) {
                    angular.forEach($scope.mastersemesterlist, function (sec) {
                        if (sec.amsE_Id == mas.amsE_Id) {
                            sec.Selected1 = true;
                            sec.editflag = true;
                            cnt +=1;
                        }
                    })
                })
                $scope.selcnt = cnt;
             

                $scope.AMCO_Id = user.amcO_Id;
                $scope.AMB_Id = user.amB_Id;
                $scope.AMCOBM_Id = user.amcobM_Id;
                $scope.chkcurslst($scope.AMCO_Id);

                $scope.cbcsyearlist = promise.cbcsyearlist;
                $scope.electiveyearlist = promise.electiveyearlist;

                $scope.AMCOBM_Code = promise.editdata[0].amcobM_Code;
                $scope.AMCOBM_CBCSFlg = promise.editdata[0].amcobM_CBCSFlg;
                if ($scope.AMCOBM_CBCSFlg == true) {
                    $scope.AMCOBM_CBCSFlg = 1;
                }
                else {
                    $scope.AMCOBM_CBCSFlg = 0;
                }
                $scope.AMCOBM_CBCSIntroYear = promise.editdata[0].amcobM_CBCSIntroYear;
                $scope.AMCOBM_ElectiveFlg = promise.editdata[0].amcobM_ElectiveFlg;
                if ($scope.AMCOBM_ElectiveFlg == true) {
                    $scope.AMCOBM_ElectiveFlg = 1;
                }
                else {
                    $scope.AMCOBM_ElectiveFlg = 0;
                }
                $scope.AMCOBM_ElectiveIntroYear = promise.editdata[0].amcobM_ElectiveIntroYear;

                $scope.file_detail = promise.editdata[0].amcobM_FileName;
                $scope.att_file11 = promise.editdata[0].amcobM_FilePath;

                $scope.notice = promise.editdata[0].amcobM_FilePath;

            })
        }

        //view details
        $scope.showmodaldetails = function (user) {
            apiService.create("ClgMasterCourseBranchMap/showmodaldetails", user).then(function (Promise) {
                $scope.semesterbrancklist = Promise.semesterbrancklist;
            })
        }

        //deactive and active semester
        $scope.deactivesem = function (usersem, SweetAlert) {
            
            var dystring = "";
            if (usersem.amcobmS_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (usersem.amcobmS_ActiveFlg == false) {
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
                    apiService.create("ClgMasterCourseBranchMap/deactivesem", usersem).
                   then(function (promise) {
                       if (promise.returnval == true) {
                           

                           swal("Record " + dystring + "Successfully!!!");
                           $state.reload();

                       }
                       else {
                           
                           swal("Record Not " + dystring + " Successfully!!!");
                           $state.reload();
                       }

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
            return $scope.submitted ;
        };


        $scope.searchValue = "";
        $scope.filterValue123 = function (obj) {
            
            return (angular.lowercase(obj.amcO_CourseName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.amB_BranchName)).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        }

        //$scope.resultClick = function () {
        //    $scope.AMCOBM_Id = item.amcobM_Id;
        //}



        //-----------upload file/photo.............
        $scope.uploadFile = function (input, document) {
           
            $scope.UploadFile = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" || input.files[0].type == "image/png" || input.files[0].type == "image/jpg" || input.files[0].type === "application/pdf" || input.files[0].type === "application/msword" || input.files[0].type === "application/vnd.ms-excel" || input.files[0].type == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" && input.files[0].size <= 2097152) {

                    var reader = new FileReader();

                    reader.onload = function (e) {
                        $('#blahD')
                            .attr('src', e.target.result)
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofile();
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }
            }
        }

        $scope.previewimg_new = function (img) {
            $scope.imagepreview = img[0];
            $scope.view_videos = [];
            var img = $scope.imagepreview;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                $scope.filetype2 = lastelement;
            }
            if ($scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp4') {

                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myvideoPreview').modal('show');

            }
            else if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {

                $('#preview').attr('src', $scope.imagepreview);
                $('#myimagePreview').modal('show');

            }
            else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
                $window.open($scope.imagepreview)
            }
            else if ($scope.filetype2 == 'mp3') {
                $scope.view_videos.push({ id: 1, ihw_video: img });
                $('#myaudioPreview').modal('show');

            }
            else if ($scope.filetype2 == 'pdf') {

                ///=====================show pdf, img

                $('#showpdf').modal('hide');
                var imagedownload1 = "";
                imagedownload1 = $scope.imagepreview;


                $http.get(imagedownload1, { responseType: 'arraybuffer' })
                    .success(function (response) {
                        var fileURL = "";
                        var file = "";
                        var embed = "";
                        var pdfId = "";
                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);

                        pdfId = document.getElementById("showpdf");
                        pdfId.removeChild(pdfId.childNodes[0]);
                        embed = document.createElement('embed');
                        embed.setAttribute('src', fileURL);
                        embed.setAttribute('type', 'application/pdf');
                        embed.setAttribute('width', '100%');
                        embed.setAttribute('height', '1000');
                        pdfId.appendChild(embed);
                        $('#showpdf').modal('show');



                    });



            }
            else {
                $window.open($scope.imagepreview)
            }
        };
        function Uploadprofile() {
           
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadFile.length; i++) {

                formData.append("File", $scope.UploadFile[i]);
                $scope.file_detail = $scope.UploadFile[0].name;
            }
            //We can send more data to server using append         
            formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Upload_Noticefiles", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    $scope.notice = d;
                    $scope.intB_FilePath = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }
        //----------End..........!



        ///=========clear upload field data......
        $scope.remove_file = function () {

            $scope.file_detail = "";
            $scope.notice = "";
        }







    }
})();
