
(function () {
    'use strict';
    angular
        .module('app')
        .controller('CollegeMasterDocumentController', CollegeMasterDocumentController)

    CollegeMasterDocumentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$window', 'superCache']
    function CollegeMasterDocumentController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $window, superCache) {

        $scope.sortKey = 'amsmD_Id';
        $scope.sortReverse = true;

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.searchValue = "";
        $scope.obj = {};
        $scope.docmandotaryornot = false;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            } else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
        }



        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null) {
            if (admfigsettings.length > 0) {
                var logopath = admfigsettings[0].asC_Logo_Path;
            } else {
                var logopath = "";
            }
        } else {
            var logopath = "";
        }


        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        // First Tab Master Document

        $scope.Getdetails = function () {

            $scope.currentPage = 1;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.itemsPerPage1 = paginationformasters;

            apiService.getDATA("CollegeMasterDocument/Getdetails").then(function (promise) {
                $scope.gridviewDetails = promise.getdetails;
                $scope.presentCountgrid = $scope.gridviewDetails.length;

                $scope.documentlist = promise.documentlist;
                $scope.quotalist = promise.quotalist;
                $scope.courselist = promise.courselist;

                $scope.getdetails1 = promise.getmappeddetails;
                $scope.presentCountgrid1 = $scope.getdetails1.length;

                $scope.branchlist = [];
                $scope.getsavedbranchlist = [];
                $scope.detail_checked = false;

                $scope.docmandotaryornot = false;

                if ($scope.presentCountgrid > 0) {
                    $scope.IsHidden2 = true;
                } else {
                    $scope.IsHidden2 = false;
                }
                if ($scope.presentCountgrid1 > 0) {
                    $scope.IsHidden1 = true;
                } else {
                    $scope.IsHidden1 = false;
                }
            });
        };
        $scope.imagepath = [];
        var filenamee
        //================================ Image  
        $scope.stepsModel = [];
        $scope.imageUpload = function (event) {
            $scope.stepsModel = [];
            $scope.files = event.files;
          
            for (var i = 0; i < 1; i++) {
                var file = $scope.files[i];
                filenamee = $scope.files[i].name;
                $scope.fileimg = file;
                var reader = new FileReader();
                reader.onload = $scope.imageIsLoaded;
                reader.readAsDataURL(file);
            }
            UploadImgs()
            
        };
        $scope.imageIsLoaded = function (e) {
            $scope.$apply(function () {
                $scope.stepsModel.push(e.target.result);
            });
        };
        //$scope.imagepath = $scope.stepsModel[0];
        //$scope.view_videos = [];
        //function UploadImgs() {
        //    var formData = new FormData();
        //    for (var i = 0; i <= 1; i++) {
        //        formData.append("File", $scope.files[i]);
        //        $scope.filenames = "Videos";
        //        $scope.fileflg = true;
        //    }
          
        //    formData.append("Id", 0);
        //    var defer = $q.defer();
        //    $http.post("/api/ImageUpload/Upload_GalleryImgVideos", formData,
        //        {
        //            withCredentials: true,
        //            headers: { 'Content-Type': undefined },
        //            transformRequest: angular.identity
        //        })
        //        .success(function (d) {
        //            defer.resolve(d);
        //            swal(d);
        //            $scope.images_paths = d;

        //        })
        //        .error(function () {
        //            defer.reject("File Upload Failed!");
        //        });

        //}
        //$scope.previewimg = function (item) {
        //    //  $scope.imagepreview = img;

        //    // $scope.imagepreview = img;
        //    $scope.imagepreview = $scope.images_paths[0];
        //    $scope.view_videos = [];
        //    var img = $scope.imagepreview;
        //    if (img != null) {
        //        var imagarr = img.split('.');
        //        var lastelement = imagarr[imagarr.length - 1];
        //        $scope.filetype2 = lastelement;
        //    }
        //    if ($scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png') {


        //        $('#preview').attr('src', $scope.imagepreview);
        //        $('#myModalPreview').modal('show');

        //    }

        //    else if ($scope.filetype2 == 'doc' || $scope.filetype2 == 'docx' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx' || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx') {
        //        $window.open($scope.imagepreview)
        //    }
        //};

        $scope.submitted = false;

        $scope.savedata = function () {

            $scope.submitted = true;
            $scope.filedoc = [];
            $scope.documentListOtherDetails11 = [];
            if ($scope.documentListOtherDetails != null) {
                angular.forEach($scope.documentListOtherDetails, function (qq) {
                    if (qq.AMSMD_FilePath != null) {
                        $scope.documentListOtherDetails11.push({ AMSMD_FilePath: qq.AMSMD_FilePath, FileName: qq.FileName });
                    }
                })
                $scope.filedoc = $scope.documentListOtherDetails11;
            }

            if ($scope.myForm.$valid) {
                var data = {
                    "AMSMD_Id": $scope.amsmD_Id,
                    "AMSMD_DocumentName": $scope.DocumentName ,
                    "AMSMD_Description": $scope.Description === undefined || $scope.Description === null ? "" : $scope.Description,
                    "AMSMD_FLAG": $scope.checkoruncheck, 
                    //"images_paths": $scope.images_paths,
                    //"images_name": filenamee,
                    "FilePath_Array": $scope.filedoc,
                    
               
                }

                apiService.create("CollegeMasterDocument/savedata/", data).then(function (promise) {

                    if (promise.message == "Already Exists") {
                        swal("Document Name Already Exists");
                        $state.reload();
                        return;
                    }
                    else {
                        if (promise.returnval == true) {
                            if (promise.message == "Update") {
                                swal('Record Updated Successfully');
                            }
                            else {
                                swal('Record Saved Successfully');
                            }

                            $state.reload();
                        }
                        else if (promise.returnval == false) {
                            if (promise.message == "Update") {
                                swal('Failed To Update Record');
                            }
                            else {
                                swal('Failed To Save Record');
                            }
                            $state.reload();
                        }
                    }
                })
            }
        };



        //Edit Record 
        $scope.Editdata = function (AMSMD_Id) {

            var data = {
                "AMSMD_Id": AMSMD_Id
            }

            apiService.create("CollegeMasterDocument/Editdata/", data).
                then(function (promise) {
                    $scope.stepsModel = [];
                    $scope.imagepath = [];
                    $scope.amsmD_Id = promise.selectedRowDetails[0].amsmD_Id;
                    $scope.DocumentName = promise.selectedRowDetails[0].amsmD_DocumentName;
                    $scope.Description = promise.selectedRowDetails[0].amsmD_Description;
                    $scope.checkoruncheck = promise.selectedRowDetails[0].amsmD_FLAG;
                    $scope.amsmD_FileName = promise.selectedRowDetails[0].amsmD_FileName;
                   //var dd = promise.selectedRowDetails[0].amsmD_FileName;
                   // $scope.imageUpload(dd);
                   
                    //$scope.imagepath = $scope.stepsModel[0];
                   // $scope.imageUpload(dd);

                  //  $scope.previewimg(dd);

                    $scope.urldocumentlist = [];
                    $scope.documentListOtherDetails = [];
                    if (promise.selectedRowDetails[0].amsmD_FilePath != null ) {
                        angular.forEach(promise.selectedRowDetails, function (aa) {
                            $scope.img = aa.amsmD_FilePath;
                            if ($scope.img != null) {
                                var imagarr = $scope.img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                $scope.filetype2 = lastelement;
                            }

                            if ($scope.filetype2 == 'mp4' || $scope.filetype2 == 'wmv' || $scope.filetype2 == 'mp3'
                                || $scope.filetype2 == 'jpg' || $scope.filetype2 == 'jpeg' || $scope.filetype2 == 'png' ||
                                $scope.filetype2 == 'pdf' || $scope.filetype2 == 'xls' || $scope.filetype2 == 'xlsx'
                                || $scope.filetype2 == 'ppt' || $scope.filetype2 == 'pptx' || $scope.filetype2 == 'ppsx'
                                || $scope.filetype2 == 'doc' || $scope.filetype2 == 'docx') {
                                $scope.documentListOtherDetails.push({
                                    id: 1, AMSMD_FilePath: aa.amsmD_FilePath,
                                    FileName: aa.amsmD_FileName
                                });
                            }
                            else {
                                $scope.urldocumentlist.push({
                                    id: 1, AMSMD_FilePath: aa.amsmD_FilePath,
                                    FileName: aa.amsmD_FileName
                                });

                            }
                        });
                    }
                    else {
                        $scope.documentListOtherDetails.push({
                            id: 1, AMSMD_FileName: '',
                            FileName: ''
                        });
                        //$scope.urldocumentlist.push({
                        //    id: 1, INTBFL_FilePath: '',
                        //    FileName: ''
                        //});
                    }


                })
        };


        //delete record
        $scope.Deletedata = function (id, SweetAlert) {
            // swal(id);

            var data = {
                "AMSMD_Id": id
            }

            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete The Record ..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("CollegeMasterDocument/DeleteData", data).
                            then(function (promise) {
                                if (promise.message == "Delete") {
                                    swal('You Can Not Delete This Record It Is Already Mapped');
                                } else {
                                    if (promise.returnval == true) {
                                        swal('Record Deleted Successfully');
                                    } else {
                                        swal('Failed To Delete Record');
                                    }
                                }
                                $state.reload();
                            });
                    }
                    else {
                        swal("Cancelled");
                    }
                });
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        }

        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.amsmD_DocumentName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.amsmD_Description)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        };

        // End Of First Tab Master Document

        $scope.all_check = function () {
            var toggleStatus = $scope.detail_checked;
            angular.forEach($scope.branchlist, function (itm) {
                itm.ambid = toggleStatus;
            });

            $scope.albumNameArraycolumn1 = [];
            angular.forEach($scope.branchlist, function (role) {
                if (!!role.ambid) $scope.albumNameArraycolumn1.push(role);
            });
        };

        $scope.addColumn2 = function (role1) {

            $scope.detail_checked = $scope.branchlist.every(function (itm) { return itm.selected; });

            if (role1.selected === true) {
                $scope.albumNameArraycolumn.push(role1);
                var newCol = { AMB_Id: role1.amB_Id, checked: true, AMB_BranchName: role1.amB_BranchName };
                $scope.columnsTest1.push(newCol);
            }
            else {
                var som = $scope.albumNameArraycolumn.indexOf(role1);
                $scope.columnsTest1.splice($scope.albumNameArraycolumn.indexOf(role1), 1);
                $scope.albumNameArraycolumn.splice($scope.albumNameArraycolumn.indexOf(role1), 1);
            }
        };

        $scope.isOptionsRequired = function () {
            return !$scope.branchlist.some(function (options) {
                return options.ambid;
            });
        };


        $scope.onchangecourse = function () {
            var data = {
                "AMCO_Id": $scope.amcO_Id,
                "ACQ_Id": $scope.acQ_Id,
                "AMSMD_Id": $scope.AMSMD_Id_New
            }
            apiService.create("CollegeMasterDocument/onchangecourse", data).then(function (promise) {

                $scope.branchlist = promise.branchlist;

                if (promise.getsavedbranchlist != null) {
                    $scope.getsavedbranchlist = promise.getsavedbranchlist;
                    angular.forEach($scope.getsavedbranchlist, function (dd) {
                        angular.forEach($scope.branchlist, function (ddd) {

                            if (ddd.amB_Id == dd.amB_Id) {
                                ddd.ambid = true;
                            } else {
                                ddd.ambid = false;
                            }
                        })
                    })
                }
            });
        };



        $scope.submitted1 = false;


        $scope.savedata1 = function () {

            if ($scope.myForm1.$valid) {

                $scope.albumNameArraycolumn1 = [];

                angular.forEach($scope.branchlist, function (role) {
                    if (!!role.ambid) $scope.albumNameArraycolumn1.push(role);
                });

                var data = {

                    "AMCO_Id": $scope.amcO_Id,
                    "AMSMD_Id": $scope.AMSMD_Id_New,
                    "ACQ_Id": $scope.acQ_Id,
                    "temp_branchDTO": $scope.albumNameArraycolumn1,
                    "ACQCD_CompulsoryFlg": $scope.docmandotaryornot
                }
                apiService.create("CollegeMasterDocument/savedata1", data).then(function (promise) {

                    if (promise != null) {
                        if (promise.message == "Add") {
                            if (promise.returnval == true) {
                                swal("Record Saved Successfully");
                            } else {
                                swal("Failed To Save Record");
                            }
                            $scope.Getdetails();
                        } else if (promise.message == "Update") {
                            if (promise.returnval == true) {
                                swal("Record Updated Successfully");
                            } else {
                                swal("Failed To Update Record");
                            }
                            $scope.Getdetails();
                        } else {
                            swal("Something Went Wrong Kindly Contact Administrator");
                        }
                    } else {
                        swal("Something Went Wrong Kindly Contact Administrator");
                    }
                })

            } else {
                $scope.submitted1 = true;
            }
        }

        $scope.getdoc = function (user) {

            var data = {
                "AMCO_Id": user.amcO_Id,
                "AMB_Id": user.amB_Id,
                "ACQ_Id": user.acQ_Id
            }

            apiService.create("CollegeMasterDocument/getdoc", data).then(function (promise) {

                if (promise != null) {

                    $scope.doclist = promise.doclist;

                    $scope.Category_Name = "Quota : " + $scope.doclist[0].acQ_QuotaName + " Course : " + $scope.doclist[0].amcO_CourseName + " Branch : " + $scope.doclist[0].amB_BranchName

                    $('#myModal').modal('show');

                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }

            })
        }

        $scope.deactive_sub = function (data, SweetAlert) {
            var mgs = "";
            if (data.acqcD_ActiveFlg == false) {
                mgs = "Activate";
            }
            else {
                mgs = "De-Activate";
            }
            swal({
                title: "Are You Sure?",
                text: "Do You Want to " + mgs + " Quota?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("CollegeMasterDocument/deactive_sub", data).then(function (promise) {

                            if (promise.message == "Mapped") {
                                swal("You Can Not Deactive This Record Its Already Mapped");
                            } else if (promise.message == "Error") {
                                swal("Something Went Wrong please contact administrator");
                            } else {
                                if (promise.returnval == true) {
                                    swal("Quota " + mgs + "Successfully");
                                } else {
                                    swal("Quota " + mgs + "Successfully");
                                }
                            }
                        })
                    }
                    else {
                        swal("Cancelled");
                        $state.reload();
                    }
                });
        }

        //image upload

      $scope.documentListOtherDetails = [{ id: 'document' }];
        $scope.addNewDocumentOtherDetail = function () {
            var newItemNo = $scope.documentListOtherDetails.length + 1;
            if (newItemNo <= 30) {
                $scope.documentListOtherDetails.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewDocumentOtherDetail = function (index, data) {
            var newItemNo = $scope.documentListOtherDetails.length - 1;
            $scope.documentListOtherDetails.splice(index, 1);
            if (data.hreothdeT_Id > 0) {
                $scope.DeleteDocumentDataOthers(data);
            }
        };

        //======================= file upload
        $scope.selectFileforUploadzdOtherDetail = function (input, document) {
            $scope.SelectedFileForUploadzdOtherDetail = input.files;
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#blahD')
                        .attr('src', e.target.result);
                };
                reader.readAsDataURL(input.files[0]);
                UploadEmployeeDocumentOtherDetail(document);

            }
        };

        function UploadEmployeeDocumentOtherDetail(data) {
            var formData = new FormData();
            for (var i = 0; i <= $scope.selectFileforUploadzdOtherDetail.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdOtherDetail[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/NoticeUpload", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (d !== "PathNotFound") {

                        data.AMSMD_FilePath = d[0].path;
                        data.FileName = d[0].name;

                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }


        $scope.previewimg_new = function (img) {
            $scope.imagepreview = img;
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
        $scope.downloaddirectpdf = function (data, idd) {

            var studentreg = idd;

            $scope.imagedownload = data;
            imagedownload = data;

            $http.get(imagedownload, {
                responseType: "arraybuffer"
            })
                .success(function (data) {
                    var anchor = angular.element('<a/>');
                    var blob = new Blob([data]);
                    anchor.attr({
                        href: window.URL.createObjectURL(blob),
                        target: '_blank',
                        download: studentreg + '.pdf'
                    })[0].click();
                });
        };
        $scope.previewimg_url = function (url) {
            $scope.urlnew = url;
            $window.open($scope.urlnew)
        }

        $scope.cancel1 = function () {
            $scope.Getdetails();
        }

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        $scope.searchValue1 = "";
        $scope.filterValue01 = function (obj) {
            return (angular.lowercase(obj.acQ_QuotaName)).indexOf(angular.lowercase($scope.searchValue1)) >= 0 ||
                (angular.lowercase(obj.amcO_CourseName)).indexOf(angular.lowercase($scope.searchValue1)) >= 0 ||
                (JSON.stringify(obj.amB_BranchName)).indexOf($scope.searchValue1) >= 0;
        }
    }

})();