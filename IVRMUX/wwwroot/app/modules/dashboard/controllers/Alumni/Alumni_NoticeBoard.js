(function () {
    'use strict';
    angular
        .module('app')
        .controller('Alumni_NoticeBoardController', Alumni_NoticeBoardController);
    Alumni_NoticeBoardController.$inject = ['$window', '$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', '$sce'];
    function Alumni_NoticeBoardController($window, $rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, $sce) {

        $scope.UploadFile = [];
        // $scope.file_temp = [];
        $scope.search = "";
        $scope.intB_DispalyDisableFlg = false;
        //$scope.minDate = Date.now();

        //------------for sorting.........
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };



        //----------load data into page.............
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = 10;
            $scope.itemsPerPage = 10;
            var id = 1;
            apiService.getURI("Alumni_NoticeBoard/loaddata", id).
                then(function (promise) {
                    $scope.alumninoticeboardlist = promise.alumninoticeboardlist;
                });
        };
        var imagedownload = "";
        var docname = "";
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

        var imagedownloadppt = "";
        var docname = "";
        $scope.downloaddirectppt = function (data, idd) {

            var studentreg = idd;

            $scope.imagedownloadppt = data;
            imagedownloadppt = data;

            $http.get(imagedownloadppt, {
                responseType: "arraybuffer"
            })
                .success(function (data) {
                    var anchor = angular.element('<a/>');
                    var blob = new Blob([data]);
                    anchor.attr({
                        href: window.URL.createObjectURL(blob),
                        target: '_blank',
                        download: studentreg + '.ppt || .pptx || .ppsx'
                    })[0].click();
                });
        };
        $scope.downloaddirectimage = function (data, idd) {

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
                        download: studentreg + '.jpg'
                    })[0].click();
                });
        };


        //-----------upload file/photo.............
        $scope.uploadFile = function (input, document) {

            $scope.UploadFile = input.files;
            if (input.files && input.files[0]) {
                if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" || input.files[0].type === "image/jpg" || input.files[0].type === "application/pdf" || input.files[0].type === "application/msword" || input.files[0].type === "application/vnd.ms-excel" || input.files[0].type === "application/pptx" || input.files[0].type === "application/ppsx" || input.files[0].type === "application/vnd.ms-powerpoint" || input.files[0].type === "application/vnd.openxmlformats-officedocument.presentationml.presentation" || input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document" || input.files[0].type === "application/vnd.openxmlformats-officedocument.presentationml.slideshow" || input.files[0].type === "application/vnd.openxmlformats-officedocument.presentationml.presentation") {

                    var reader = new FileReader();

                    reader.onload = function (e) {
                        $('#blahD')
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofile();
                }

            }
        };
        function Uploadprofile() {
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadFile.length; i++) {
                formData.append("File", $scope.UploadFile[i]);

            }
            //We can send more data to server using append         
            formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadnaacdocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    $scope.notice = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }
        //----------End..........!
        $scope.content1 = "";
        ///=====================show pdf, img
        $scope.previewpdf = function (filepath1, filename) {
            $('#showpdf').modal('hide');
            var imagedownload1 = "";
            imagedownload1 = filepath1;


            $http.get(imagedownload1, { responseType: 'arraybuffer' })
                .success(function (response) {
                    var fileURL = "";
                    var file = "";
                    var embed = "";
                    var pdfId = "";
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);

                    pdfId = document.getElementById("pdfIdzz");
                    pdfId.removeChild(pdfId.childNodes[0]);
                    embed = document.createElement('embed');
                    embed.setAttribute('src', fileURL);
                    embed.setAttribute('type', 'application/pdf');
                    embed.setAttribute('width', '100%');
                    embed.setAttribute('height', '1000');
                    pdfId.appendChild(embed);
                    $('#showpdf').modal('show');



                });
        };


        $scope.previewimg = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };
        ///=====================show pdf, img end

        ///=========clear upload field data......
        $scope.remove_file = function () {
            $scope.file_detail = "";
            $scope.notice = "";
        };
        //-------check date
        $scope.display_Date = function () {
            if ($scope.ALNTB_DisplayDate > $scope.ALNTB_EndDate) {
                swal("Display Date Cannot Greater than End date ");
                $scope.intB_DisplayDate = "";
            }
            else if ($scope.ALNTB_StartDate > $scope.ALNTB_EndDate) {
                swal("Start Date Cannot Greater than End date ");
                $scope.intB_EndDate = "";
            }
        };



        //========classlist CheckBox Field Validation===========//
        $scope.isOptionsRequireddesc = function () {
            return !$scope.designationdropdown.some(function (item) {
                return item.selected;
            });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.sectionlist.some(function (item) {
                return item.selected;
            });
        };
        $scope.isOptionsRequireddept = function () {
            return !$scope.departmentdropdown.some(function (item) {
                return item.selected;
            });
        };

        //------------save data.....
        
        $scope.savedata = function () {
         
            var displaydate = "";
            if ($scope.myForm.$valid) {
                if ($scope.intB_DispalyDisableFlg === false) {
                    displaydate = null;
                }
                else if ($scope.intB_DispalyDisableFlg === true) {
                    displaydate = $scope.ALNTB_DisplayDate === null ? "" : $filter('date')($scope.ALNTB_DisplayDate, "yyyy-MM-dd");
                }

                $scope.filedoc = [];
                $scope.filedoc1 = [];
                $scope.filedoc2 = [];
                $scope.documentListOtherDetails11 = [];
                if ($scope.documentListOtherDetails !== null) {
                    angular.forEach($scope.documentListOtherDetails, function (qq) {
                        if (qq.ALNTBFL_FilePath !== null) {
                            $scope.documentListOtherDetails11.push({ ALNTBFL_FilePath: qq.ALNTBFL_FilePath, ALNTBFL_FileName: qq.FileName });
                        }
                    })
                    $scope.filedoc1 = $scope.documentListOtherDetails11;
                }

                angular.forEach($scope.filedoc1, function (aa) {
                    if (aa.ALNTBFL_FilePath !== undefined) {
                        $scope.filedoc2.push({ ALNTBFL_FilePath: aa.ALNTBFL_FilePath, ALNTBFL_FileName: aa.ALNTBFL_FileName });
                    }

                });
                $scope.filedoc = $scope.filedoc2;



                var startdate = $scope.ALNTB_StartDate === null ? "" : $filter('date')($scope.ALNTB_StartDate, "yyyy-MM-dd");
                var enddate = $scope.ALNTB_EndDate === null ? "" : $filter('date')($scope.ALNTB_EndDate, "yyyy-MM-dd");


               var data = {
                    "ALNTB_Id": $scope.ALNTB_Id,
                    "ALNTB_Title": $scope.ALNTB_Title,
                    "ALNTB_Description": $scope.ALNTB_Description,
                    "ALNTB_StartDate": startdate,
                    "ALNTB_EndDate": enddate,
                    "ALNTB_DisplayDate": displaydate,
                    "Attachment_Array": $scope.filedoc

                };


                apiService.create("Alumni_NoticeBoard/savedetail", data).
                    then(function (promise) {
                        if (promise.message === 'Add') {
                            swal('Notice Saved Successfully...');
                        }
                        else if (promise.message === 'Update') {
                            swal('Notice Updated Successfully...');
                        }
                        else {
                            swal('Operation Failed...');
                        }
                        $state.reload();
                    });
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        //==============view data=========

        $scope.viewData = function (option) {
            $scope.attachementlist = [];
            var data = {
                "ALNTB_Id": option.ALNTB_Id

            };
            apiService.create("Alumni_NoticeBoard/viewData", data)
                .then(function (promise) {

                    if (promise.attachementlist.length > 0) {

                        $scope.attachementlist = promise.attachementlist;

                        $('#myModalCoverview').modal('show');

                    }
                    else {
                        swal("No Data Found.");

                    }

                });
        };
        //============


        //-------------for active and deactive
        $scope.deactiveY = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            if (employee.ALNTB_ActiveFlag === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Alumni_NoticeBoard/deactivate", employee).
                            then(function (promise) {
                                if (promise.message === "true") {
                                    swal(confirmmgs + " " + "successfully.");
                                }
                                else if (promise.message === "false") {
                                    swal(confirmmgs + " " + " successfully");
                                }
                                else {
                                    swal('Operation Failde..!!');
                                }
                                $state.reload();
                            });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        //----------------edit data.......
        $scope.editnotice = function (id) {
            $scope.checklink = false;
            var data = {
                "ALNTB_Id": id.ALNTB_Id
            };
            apiService.create("Alumni_NoticeBoard/editdetails", data).then(function (promise) {
                $scope.editdetails = [];
                if (promise.editdetails.length > 0) {
                    $scope.ALNTB_Id = promise.editdetails[0].alntB_Id;
                    $scope.ALNTB_Title = promise.editdetails[0].alntB_Title;
                    $scope.ALNTB_Description = promise.editdetails[0].alntB_Description;
                    $scope.ALNTB_StartDate = new Date(promise.editdetails[0].alntB_StartDate);
                    $scope.ALNTB_EndDate = new Date(promise.editdetails[0].alntB_EndDate);

                    if (promise.editdetails[0].alntB_DisplayDate === null) {
                        $scope.intB_DispalyDisableFlg = false;
                        $scope.ALNTB_DisplayDate = "";
                    }
                    else {
                        $scope.intB_DispalyDisableFlg = true;
                        $scope.ALNTB_DisplayDate = new Date(promise.editdetails[0].alntB_DisplayDate);
                    }

                    if (promise.editdetailsfiles !== null && promise.editdetailsfiles.length > 0) {
                        $scope.documentListOtherDetails = [];
                        angular.forEach(promise.editdetailsfiles, function (aa) {
                            $scope.documentListOtherDetails.push({
                                id: 1, ALNTBFL_FilePath: aa.alntbfL_FilePath,
                                FileName: aa.alntbfL_FileName
                            });
                        });



                    }

                }

            });
        };
        //-----------------------------------


        $scope.clear = function () {
            $state.reload();
        };

        //===========================ADD==================================
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
        //-------------------ADD------------------
        $scope.urldocumentlist = [{ id: 'document' }];
        $scope.addNewurl = function () {
            var newItemNo = $scope.urldocumentlist.length + 1;
            if (newItemNo <= 30) {
                $scope.urldocumentlist.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewurl = function (index, data) {
            var newItemNo = $scope.urldocumentlist.length - 1;
            $scope.urldocumentlist.splice(index, 1);
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
            formData.append("folder", "Alumni_Notice");

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

                        data.ALNTBFL_FilePath = d[0].path;
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


        $scope.previewimg_new = function (img1) {
            $scope.imagepreview = img1;
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

                        pdfId = document.getElementById("pdfIdzz");
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

        $scope.previewimg_url = function (url) {
            $scope.urlnew = url;
            $window.open($scope.urlnew)
        }
        //====================end==================
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
    $scope.onyearchange = function (ASMAY_Id) {
        $scope.sectionlist = [];
        $scope.asmcL_Id = "";
        $scope.sectionlist = [];
        $scope.asmS_Id = "";
        var data = {
            "HRME_Id": $scope.hrmE_Id,
            "ASMAY_Id": $scope.asmaY_Id
        };

        apiService.create("NoticeBoard/get_noticelist", data).then(function (promise) {
            if (promise.classlist.length > 0) {

                $scope.classwork = promise.classlist;

            }
            else {
                swal('Data Not Available');
                $scope.asmcL_Id = "";
            }
        });
    };







})();
