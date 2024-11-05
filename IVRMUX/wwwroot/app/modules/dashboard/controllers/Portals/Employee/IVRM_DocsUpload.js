(function () {
    'use strict';

    angular
        .module('app')
        .controller('IVRM_DocsUploadController', IVRM_DocsUploadController);

    IVRM_DocsUploadController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', '$sce']

    function IVRM_DocsUploadController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, $sce) {

        $scope.UploadFile = [];
        $scope.docs_temp = [];
        var id = 1;
        $scope.submitted = false;
        $scope.search = "";
        //--------for sorting....
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.loaddata = function () {
            $scope.itemsPerPage = 10;
            $scope.currentPage = 1;
            apiService.getURI("IVRM_DocsUpload/Getdetails", id).
                then(function (promise) {
                    $scope.hrmE_Id = promise.hrmE_Id;
                    $scope.studetiallist = promise.yearlist;
                    $scope.docsDetails = promise.docsDetails;
                })
        }

      

        //-----------upload file/photo.............
        $scope.uploadFile = function (input, document) {
            
            $scope.UploadFile = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" || input.files[0].type == "image/png" || input.files[0].type == "image/jpg" || input.files[0].type === "application/pdf" || input.files[0].type === "application/doc" || input.files[0].type === "application/docx" || input.files[0].type === "application/vnd.ms-excel" &&
                    input.files[0].size <= 2097152) {

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


       
        $scope.submitted = false;
        $scope.savedata = function () {
            
            if ($scope.myForm.$valid) {

                //-------file upload
                if ($scope.notice == undefined || $scope.notice == "") {
                    var data = {
                        "IDU_Id": $scope.idU_Id,
                        "ASMAY_Id": $scope.asmaY_Id,
                        "ASMCL_Id": $scope.asmcL_Id,
                        "ASMS_Id": $scope.asmS_Id,
                        "IDU_Type": $scope.idU_Type,
                        "IDU_Remarks": $scope.idU_Remarks,
                       // "IDU_Attachment": $scope.file_detail,
                        //"IDU_FilePath": att_file11,
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
                        "IDU_Id": $scope.idU_Id,
                        "ASMAY_Id": $scope.asmaY_Id,
                        "ASMCL_Id": $scope.asmcL_Id,
                        "ASMS_Id": $scope.asmS_Id,
                        "IDU_Type": $scope.idU_Type,
                        "IDU_Remarks": $scope.idU_Remarks,
                         "IDU_Attachment": $scope.file_detail,
                        "IDU_FilePath": att_file11,
                    }
                }
               


               

                apiService.create("IVRM_DocsUpload/savedetail", data).
                    then(function (promise) {
                        if (promise.returnval != null && promise.duplicate != null) {
                            if (promise.duplicate == false) {
                                if (promise.returnval == true) {
                                    if ($scope.idU_Id > 0) {
                                        swal('Record Update Successfully!');
                                    }
                                    else {
                                        swal('Record Saved Successfully!!!');
                                    }
                                }
                                else {
                                    if (promise.returnval == false) {
                                        if ($scope.idU_Id > 0) {
                                            swal('Record Not Update Successfully!');
                                        }
                                        else {
                                            swal('Record Not Saved Successfully!!!');
                                        }
                                    }
                                }

                            }
                            else {
                                swal("Record already Exist!");
                            }
                            $state.reload();
                        }
                        else {
                            swal("Kindly Contact Administrator!!!");
                        }

                    });
               
            }
            else {
                $scope.submitted = true;
            }

        };
        $scope.clear1 = function () {
            $state.reload();
        };


        //======Edit Record =====//
        $scope.editData = function (option) {
            
            var data = {
                "IDU_Id": option.idU_Id,
                "ASMAY_Id": option.asmaY_Id,
            }
            apiService.create("IVRM_DocsUpload/editData", data)
                .then(function (promise) {
                    
                    $scope.editlist = promise.editlist;
                    if (promise.editlist.length > 0) {

                        $scope.idU_Id = promise.editlist[0].idU_Id;

                        $scope.asmaY_Id = promise.editlist[0].asmaY_Id;
                        $scope.onyearchange($scope.asmaY_Id);

                        $scope.asmcL_Id = promise.editlist[0].asmcL_Id;
                        $scope.onclasschange($scope.asmcL_Id);

                        $scope.asmS_Id = promise.editlist[0].asmS_Id;

                        $scope.file_detail = promise.editlist[0].idU_Attachment;
                        $scope.idU_Type = promise.editlist[0].idU_Type;
                        $scope.idU_Remarks = promise.editlist[0].idU_Remarks;

                        $scope.idU_FilePath = promise.editlist[0].idU_FilePath;
                        $scope.notice = promise.editlist[0].idU_FilePath;

                        
                    }
                   

                })

        }
        //===========End Edit Record =======//



        $scope.Deactivate = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.idU_ActiveFlag === true) {
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

                        apiService.create("IVRM_DocsUpload/deactivate", employee).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal(confirmmgs + " " + "successfully.");
                                }
                                else {
                                    swal(confirmmgs + " " + " successfully");
                                }
                                $state.reload();
                            })

                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        }

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        //-----------------------------------ON Change
        $scope.onyearchange = function (ASMAY_Id) {
            $scope.sectionlist = [];
            $scope.asmcL_Id = "";
            $scope.sectionlist = [];
            $scope.asmS_Id = "";
            var data = {
                "HRME_Id": $scope.hrmE_Id,
                "ASMAY_Id": $scope.asmaY_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("IVRM_DocsUpload/get_classes", data).then(function (promise) {
                if (promise.classlist.length > 0) {
                    $scope.classlist = promise.classlist;
                }
                else {
                    swal('Data Not Available!');
                    $scope.asmcL_Id = "";
                }
            })
        }
        $scope.onclasschange = function (asmcL_Id) {
            $scope.sectionlist = [];
            $scope.asmS_Id = "";
            $scope.subjectlist = [];
            $scope.ismS_Id = "";

            var data = {
                "HRME_Id": $scope.hrmE_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id,
            }
            apiService.create("IVRM_DocsUpload/getsectiondata", data).
                then(function (promise) {
                    if (promise.sectionlist.length > 0) {
                        $scope.sectionlist = promise.sectionlist;
                    }
                     else {
                        swal('Data Not Available!');
                        $scope.asmS_Id = "";
                    }
                })
        }

    }
})();
