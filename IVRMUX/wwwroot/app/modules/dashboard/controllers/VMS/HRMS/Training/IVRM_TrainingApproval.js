(function () {
    'use strict';
    angular.module('app').controller('IVRM_TrainingApprovalController', IVRM_TrainingApprovalController)

    IVRM_TrainingApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache',  '$q', '$sce']
    function IVRM_TrainingApprovalController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache, $q, $sce) {

        $scope.showgrd = false;
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.search = "";
        $scope.search1 = "";
        $scope.currentPage = 1;
        $scope.currentPage1 = 1;
        $scope.currentPage2 = 1;
        $scope.itemsPerPage = 15;
        $scope.itemsPerPage1 = 10;
        $scope.itemsPerPage2 = 10;
        $scope.obj = {};

        //====== loadadata 
        $scope.loadgrid = function () {
            var pageid = 2;
            apiService.getURI("IVRTM_Training/onloaddataRequest/", pageid).then(function (promise) {
                $scope.aprovedlist = promise.aprovedlist;
                $scope.griddata = promise.griddata;

            });
        };

        $scope.minutesdetails = function (item) {
            $scope.MOM_Minutes = item.moM_Minutes
        };

        //===========================ADD==================================
        $scope.employeenameschange = [{ id: 'sell1' }];
        $scope.addNewDetail = function () {
            var newItemNo = $scope.employeenameschange.length + 1;
            if (newItemNo <= 30) {
                $scope.employeenameschange.push({ 'id': 'sell1' + newItemNo });
            }
        };
        $scope.removeNewDetail = function (index, data) {
            var newItemNo = $scope.employeenameschange.length - 1;
            $scope.employeenameschange.splice(index, 1);
            if (data.hrmE_Id > 0) {
                $scope.DeleteDocumentDataOthers(data);
            }
        };
        //==============================
        //===========================ADD files==================================
        $scope.documentListOtherDetails = [{ id: 'sell11' }];
        $scope.addNewDocumentOtherDetail = function () {
            var newItemNo = $scope.documentListOtherDetails.length + 1;
            if (newItemNo <= 30) {
                $scope.documentListOtherDetails.push({ 'id': 'sell11' + newItemNo });
            }
        };

        $scope.removeNewDocumentOtherDetail = function (index, data) {
            var newItemNo = $scope.documentListOtherDetails.length - 1;
            $scope.documentListOtherDetails.splice(index, 1);
            if (data.hrmE_Id > 0) {
                $scope.DeleteDocumentDataOthers(data);
            }
        };
        $scope.selectFileforUploadzdOtherDetail = function (input, document) {
            $scope.ldr = true;
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
            // We can send more data to server using append         
            //formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Upload_ISMAttachment_new", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (d !== "PathNotFound") {
                        data.FilePath = d[0].path;
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




        //==============================
        $scope.visitorlist = function (obj, hrmE_Id) {
            obj.MOMPRT_ParticipantsName = "";
            obj.MOMPRT_Designation = "";
            obj.MOMPRT_Email = "";
            obj.MOMPRT_MobileNo = "";
            angular.forEach($scope.emplist, function (rr) {
                if (obj.hrmE_Id.hrmE_Id == rr.hrmE_Id) {
                    obj.MOMPRT_ParticipantsName = rr.hrmE_EmployeeFirstName;
                    obj.MOMPRT_Designation = rr.hrmdeS_DesignationName;
                    obj.MOMPRT_Email = rr.hrmE_EmailId;
                    obj.MOMPRT_MobileNo = rr.hrmE_MobileNo;
                }
            })
        }


        $scope.allvisitor = [];
        $scope.trainingdetails = function (obj) {
            $scope.IVRMTT_Id = obj.IVRMTT_Id;
            $scope.IVRMTMT_Id = obj.IVRMTMT_Id;
            $scope.IVRMTT_EmployeeName = obj.IVRMTT_EmployeeName;
            $scope.IVRMTT_EmployeeId = obj.IVRMTT_EmployeeId;
            $scope.IVRMTT_EmployeeEmail = obj.IVRMTT_EmployeeEmail;
            $scope.IVRMTT_EmployeePhone = obj.IVRMTT_EmployeePhone;
            $scope.IVRMTT_ClientName = obj.IVRMTT_ClientName;
            $scope.IVRMTT_ClientURL = obj.IVRMTT_ClientURL;
            $scope.IVRMTT_TentetiveDate = new Date(obj.IVRMTT_TentetiveDate);
            $scope.TentetiveDate = new Date(obj.IVRMTT_TentetiveDate);
            $scope.IVRMTT_TentetiveStartTime = moment(obj.IVRMTT_TentetiveStartTime, 'HH:mm a').format();
            $scope.IVRMTT_TentetiveEndTime = moment(obj.IVRMTT_TentetiveEndTime, 'HH:mm a').format();
            $scope.TentetiveStartTime = moment(obj.IVRMTT_TentetiveStartTime, 'HH:mm a').format();
            $scope.TentetiveEndTime = moment(obj.IVRMTT_TentetiveEndTime, 'HH:mm a').format();
            //$scope.IVRMTT_TentetiveStartTime = obj.IVRMTT_TentetiveStartTime;
            //$scope.IVRMTT_TentetiveEndTime = obj.IVRMTT_TentetiveEndTime;
            $scope.IVRMTT_TrainingMode = obj.IVRMTT_TrainingMode;
            $scope.IVRMTT_Remarks = obj.IVRMTT_Remarks;
            $scope.IVRMTT_Status = obj.IVRMTT_Status;
            $scope.IVRMTT_Feedback = obj.IVRMTT_Feedback;
            $scope.IVRMTMT_TrainerName = obj.IVRMTMT_TrainerName;
            $scope.IVRMTMT_TrainerId = obj.IVRMTMT_TrainerId;
            $scope.IVRMTMT_TrainerEmail = obj.IVRMTMT_TrainerEmail;
            $scope.IVRMTT_TrainerPhone = obj.IVRMTT_TrainerPhone;
            $scope.IVRMTMT_Status = obj.IVRMTMT_Status;
            $scope.IVRMTMT_Feedback = obj.IVRMTMT_Feedback;
        };
        //////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\


        $scope.validatemax = function (maxdata) {

            var dsttimee = $scope.MOM_StartTime;
            $scope.sresult = $filter('date')(dsttimee, 'HH:mm:ss a');
            $scope.eresult = $filter('date')(maxdata, 'HH:mm:ss a');
            var startTime = moment($scope.sresult, "HH:mm:ss a");
            var endTime = moment($scope.eresult, "HH:mm:ss a");
            var duration = moment.duration(endTime.diff(startTime));
            var hours = parseInt(duration.asHours());
            var minutes = parseInt(duration.asMinutes()) - hours * 60;
            var finlrst = hours + ":" + minutes;

            $scope.tmin = new Date($scope.today);
            $scope.tmin.setHours(hours);
            $scope.tmin.setMinutes(minutes);
            $scope.tmax = new Date($scope.today);
            $scope.tmax.setHours(hours);
            $scope.tmax.setMinutes(minutes);

            $scope.ttst = new Date($scope.today);
            $scope.ttst.setHours(hours);
            $scope.ttst.setMinutes(minutes);

            $scope.htmax = new Date($scope.today);
            $scope.htmax.setHours(hours);
            $scope.htmax.setMinutes(minutes);

            if (maxdata >= new Date($scope.MOM_StartTime)) {
                $scope.totimemax = maxdata;
                var hh = $scope.totimemax.getHours();
                var mm = $scope.totimemax.getMinutes();
                $scope.max = maxdata;

                $scope.max.setMinutes(hh);
                $scope.max.setMinutes(mm);
            }
            else {
                $scope.MOM_EndTime = 0;
            }
        };
        $scope.validateTomintime = function (timedata) {

            $scope.totime = timedata;
            var hh = $scope.totime.getHours();
            var mm = $scope.totime.getMinutes();
            $scope.min = timedata;
            $scope.min.setHours(hh);
            $scope.min.setMinutes(mm);
            $scope.minlnc = timedata;
            $scope.minlnc.setHours(hh);
            $scope.minlnc.setMinutes(mm);
            $scope.MOM_EndTime = 0;
        };
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.mindate = new Date();
        $scope.submitted = false;
        $scope.submitted1 = false;

        $scope.saveData = function () {

            if ($scope.myForm.$valid) {

                var obj = {
                    "IVRMTT_Id": $scope.IVRMTT_Id,
                    "IVRMTT_Remarks": $scope.TrainerRemarks,
                    "IVRMTT_TentetiveStartTime": $filter('date')($scope.TentetiveStartTime, "HH:mm"),
                    "IVRMTT_TentetiveEndTime": $filter('date')($scope.TentetiveEndTime, "HH:mm"),
                    "TrainingDate": $scope.TentetiveDate,
                    "status": $scope.radiomodal,
                    "IVRMTT_TrainingMode": $scope.trainingmode
                };

                apiService.create("IVRTM_Training/saveData", obj).then(function (promise) {

                    if (promise.retrunMsg === 'Update') {
                        swal("Record Saved Successfully");
                        $state.reload();
                    }
                    else if (promise.retrunMsg === 'duplicate') {
                        swal("Record already exist");
                        $state.reload();
                    }
                    else if (promise.retrunMsg === "false") {
                        swal("Failed to save record");
                    }
                    else {
                        swal("Sorry...something went wrong");
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };



        $scope.objdata = [];
        $scope.viewdocument = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.objdata = obj;

            apiService.create("MOMUpdate/viewuploadflies", obj).then(function (promise) {
                if (promise !== null) {
                    $scope.getfiledetails = promise.getfiledetails;
                    if (promise.getfiledetails !== null && promise.getfiledetails.length > 0) {
                        $scope.uploaddocfiles = promise.getfiledetails;
                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            if (dd.momfL_FilePath !== null) {
                                var img = dd.momfL_FilePath;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.momfL_FilePath;
                                }
                            }

                        });
                    } else {
                        $('#popup11').modal('hide');
                        swal("No Documents Found");
                    }
                }
            });
        };

        $scope.ViewDetails = function (user) {

            var data = {
                "MOM_Id": user.moM_Id
            };

            apiService.create("MOMUpdate/viewuploadflies/", data).then(function (promise) {

                if (promise.getParticipentsdetails.length > 0 && promise.getParticipentsdetails !== null) {
                    $scope.getParticipentsdetails = promise.getParticipentsdetails;
                    $('#participants').modal('show');
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