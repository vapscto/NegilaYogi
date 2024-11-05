
(function () {
    'use strict';

    angular
        .module('app')
        .controller('Employee_MedicalRecordController', Employee_MedicalRecordController);

    Employee_MedicalRecordController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', '$sce']

    function Employee_MedicalRecordController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, $sce) {

        //var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        //var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));




        //-----------load data
        var id = 1;
        $scope.get_emp = [];
        $scope.get_emptwo = [];
        $scope.itemsPerPage = 10;
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        $scope.search = "";
        $scope.loaddata = function () {
            $scope.get_emp = [];
            apiService.getURI("Employee_MedicalRecord/Getdetails", id).
                then(function (promise) {

                    $scope.get_emp = promise.staffarray;
                    $scope.get_emptwo = promise.staffarray;
                    $scope.appliedgrid = promise.appliedgrid;
                })
        }
        $scope.HREMR_TestDate = new Date();
        $scope.pdfDetails = [];

        $scope.documentListOtherDetails11 = [];
        if ($scope.documentListOtherDetails != null) {
            angular.forEach($scope.documentListOtherDetails, function (qq) {
                if (qq.INTBFL_FilePath != null) {
                    $scope.documentListOtherDetails11.push({ INTBFL_FilePath: qq.INTBFL_FilePath, FileName: qq.FileName });
                }
            })
            $scope.filedoc = $scope.documentListOtherDetails;
        }
        if ($scope.checklink == true) {
            angular.forEach($scope.urldocumentlist, function (ee) {
                if (ee.INTBFL_FilePath != null) {
                    $scope.filedoc1.push({ INTBFL_FilePath: ee.INTBFL_FilePath, FileName: ee.INTBFL_FilePath });
                }
            })
        }
        if ($scope.filedoc1 != null || $scope.filedoc1 > 0) {
            angular.forEach($scope.filedoc1, function (ww) {
                $scope.filedoc.push(ww);
            })
        }

        //============Multiple file upload===========
        //===========================ADD==================================
        $scope.documentListOtherDetails = [{ id: 'document' }];
        $scope.addNewDocumentOtherDetail = function () {
            var newItemNo = $scope.documentListOtherDetails.length + 1;
            if (newItemNo <= 30) {
                $scope.documentListOtherDetails.push({ 'id': 'document' + newItemNo });
            }
        };
        $scope.attachementlist = [{ id: 'documentss' }];

        $scope.addNewDocumentOtherDetaildocumentss = function (option) {

           
           // option.HREMRF_FilePath = "";
            var newItemNo = $scope.attachementlist.length + 1;
           
            if (newItemNo <= 30) {
                $scope.attachementlist.push({ 'id': 'documentss' + newItemNo });
            }
        };
        $scope.removeNewDocumentOtherDetailss = function (index, data) {
            var newItemNo = $scope.attachementlist.length - 1;
            $scope.attachementlist.splice(index, 1);
          
        };

        $scope.removeNewDocumentOtherDetail = function (index, data) {
            var newItemNo = $scope.documentListOtherDetails.length - 1;
            $scope.documentListOtherDetails.splice(index, 1);
            
        };
        $scope.Deletedata = function (index, data) {
            var newItemNo = $scope.pdfDetails.length - 1;
            $scope.pdfDetails.splice(index, 1);
        }
        $scope.savedata = function () {
            $scope.pdfDetailsuploda = [];
            if ($scope.pdfDetails != null && $scope.pdfDetails.length > 0) {

                angular.forEach($scope.pdfDetails, function (ww) {

                    $scope.pdfDetailsuploda.push({
                        HRME_Id: ww.HRME_Id,
                        HREMR_TestName: ww.HREMR_TestName,
                        HREMR_Remarks: ww.HREMR_Remarks,
                        HREMR_TestDate: new Date(ww.HREMR_TestDatetwo).toDateString(),
                        filelistMedical: ww.documentlist
                    })

                })

                var data = {
                    "Medicallisttwo": $scope.pdfDetailsuploda,

                }

                apiService.create("Employee_MedicalRecord/savedetail", data).then(function (promise) {
                    if (promise.returnval == "save") {
                        swal("Record Save Successfully !  Duplicate Count     " + promise.count + "  !")
                    }
                    else if (promise.returnval == "notsave") {
                        swal("Record Not save  ! Duplicate Count  " + promise.count + " ");
                    }
                    else if (promise.returnval == "admin") {
                        swal("Please Contact Adminsistror  !");
                    }
                    $state.reload();
                
                });

            }

        }

        $scope.savedatatwo = function (HREMR_Idtwo) {
            $scope.pdfDetailsuploda = [];
            if ($scope.attachementlist != null && $scope.attachementlist.length > 0) {

                angular.forEach($scope.attachementlist, function (ww) {
                    if (ww.INTBFL_FilePath != null && ww.INTBFL_FilePath != "") {
                        $scope.pdfDetailsuploda.push({
                            HREMRF_FileName: ww.cc,
                            HREMRF_FilePath: ww.INTBFL_FilePath,


                        })
                    }                  
                })
                if ($scope.pdfDetailsuploda != null && $scope.pdfDetailsuploda.length > 0) {
                    var data = {
                        "filelistMedical": $scope.pdfDetailsuploda,
                        "HREMR_Id": HREMR_Idtwo,

                    }

                    apiService.create("Employee_MedicalRecord/savedetail", data).then(function (promise) {
                        if (promise.returnval == "save") {
                            swal("Record Save Successfully  !")
                        }
                        else if (promise.returnval == "notsave") {
                            swal("Record Not save  !  ");
                        }
                        else if (promise.returnval == "admin") {
                            swal("Please Contact Adminsistror  !");
                        }
                        $scope.loaddata();

                    });
                }
                else {
                    swal("Please Select Files  !");
                }
                

            }

        }


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

                        data.INTBFL_FilePath = d[0].path;
                        data.FileName = d[0].name;
                        data.cc = d[0].name;

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

        $scope.clear_Id = function () {
            $state.reload();
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.previewimg_url = function (url) {
            $scope.urlnew = url;
            $window.open($scope.urlnew)
        }
        $scope.AddtoCart = function () {
            $scope.documentListOtherDetailstemp = [];
            var HRME_EmpName = "";
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                if ($scope.documentListOtherDetails != null && $scope.documentListOtherDetails.length > 0) {

                    if ($scope.hrmE_Id.hrmE_Id > 0) {

                        angular.forEach($scope.get_emp, function (ww) {
                            if (ww.hrmE_Id == $scope.hrmE_Id.hrmE_Id) {
                                HRME_EmpName = ww.hrmE_EmployeeFirstName;
                            }
                        })

                        angular.forEach($scope.documentListOtherDetails, function (ww) {
                            if (ww.FileName != null && ww.FileName != "") {
                                $scope.documentListOtherDetailstemp.push({
                                    HREMRF_FilePath: ww.INTBFL_FilePath,
                                    HREMRF_FileName: ww.FileName,
                                })
                            }
                        })
                        if ($scope.documentListOtherDetailstemp != null && $scope.documentListOtherDetailstemp.length > 0) {
                            $scope.pdfDetails.push({
                                HRME_Id: $scope.hrmE_Id.hrmE_Id,
                                documentlist: $scope.documentListOtherDetailstemp,
                                hrmE_EmployeeFirstName: HRME_EmpName,
                                FileCount: $scope.documentListOtherDetailstemp.length,
                                HREMR_TestName: $scope.HREMR_TestName,
                                HREMR_TestDate: $scope.HREMR_TestDate,
                                HREMR_TestDatetwo: new Date($scope.HREMR_TestDate),
                                HREMR_Remarks: $scope.HREMR_Remarks,
                            })
                            $scope.documentListOtherDetails = [];
                            $scope.documentListOtherDetails = [{ id: 'document' }];
                            $scope.HREMR_Remarks = "";
                            $scope.HREMR_TestName = "";
                        }
                        else {
                            swal("Please select Atleast One  Pdf !")
                        }

                        //  $scope.get_emp = promise.staffarray;
                        // $scope.get_emptwo = promise.staffarray;


                    }
                    else {
                        swal("Please Select Employee !")
                    }
                }
            }
           
        }

        $scope.deactivate = function (obj) {            var mgs = "";            var confirmmgs = "";            if (obj.HREMR_ActiveFlag === false) {                mgs = "Activate";                confirmmgs = "Activated";            }            else {                mgs = "Deactivate";                confirmmgs = "Deactivated";            }            var data = {                "HREMR_Id": obj.HREMR_Id                        };            swal({                title: "Are You Sure",                text: "Do You Want To " + mgs + " Record?",                type: "warning",                showCancelButton: true,                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " It!",                cancelButtonText: "Cancel",                closeOnConfirm: false,                closeOnCancel: false            },                function (isConfirm) {                    if (isConfirm) {                        apiService.create("Employee_MedicalRecord/deactivate", data).then(function (promise) {                            if (promise.returnVal == true) {                                swal("Record " + confirmmgs + " " + "successfully", "", "success");                            }                            else {                                swal("Record " + mgs + " Failed", "", "error");                            }                            $state.reload();                        });                    }                    else {                        swal("Record " + mgs + " Cancelled");                    }                });        }; 
        //HREMR_Id = 2
         //-------view data-----------//
        $scope.viewData = function (option) {
            $scope.attachementlist = [];
            $scope.HREMR_Idtwo = option.HREMR_Id;
            
            var data = {
                "HREMR_Id": $scope.HREMR_Idtwo ,
                //"HRME_Id": option.HRME_Id
            }
            apiService.create("Employee_MedicalRecord/viewData", data)
                .then(function (promise) {
                    if (promise.attachementlist != null  &&  promise.attachementlist.length > 0) {
                        angular.forEach(promise.attachementlist, function (ww) {
                            $scope.attachementlist.push({
                                INTBFL_FilePath: ww.hremrF_FilePath,
                                FilePath: ww.hremrF_FilePath,
                                cc: ww.hremrF_FileName,
                                HREMRF_Attachment: ww.hremrF_FileName,
                            })
                        })
                    
                        $('#myModalCoverview').modal('show');
                   
                    }
                    else {
                        swal("No Data Found.")

                    }

                });
        };
      
    }
})();
