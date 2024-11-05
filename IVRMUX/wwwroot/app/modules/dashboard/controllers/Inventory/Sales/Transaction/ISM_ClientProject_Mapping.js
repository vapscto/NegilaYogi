(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_ClientProject_Mapping', ISM_ClientProject_Mapping)

    ISM_ClientProject_Mapping.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q']
    function ISM_ClientProject_Mapping($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q) {

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";
        $scope.obj = {};
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        // ============================ng_click function for module (ALL)========================//
        $scope.searchchkbx = "";
        $scope.all_checkC = function () {
            var checkStatus = $scope.usercheckC;
            var count = 0;
            angular.forEach($scope.module_list, function (itm) {
                itm.selected = checkStatus;
                if (itm.selected == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
        }


        $scope.togchkbxC = function () {

            $scope.usercheckC = $scope.module_list.every(function (options) {
                return options.selected;
            });
        }

        //====================================clear data====================================//
        $scope.submitted = false;
        $scope.Clearid = function () {
            $state.reload();
        }
        $scope.isOptionsRequired = function () {
            return !$scope.module_list.some(function (options) {
                return options.selected;
            });
        }

        $scope.itemsPerPage = 5;
        $scope.currentPage = 1;
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        //==================load data=========================//
        $scope.submitted = false;
        $scope.loaddata = function () {
            var pageid = 2;

            apiService.getURI("ISM_ClientProject_Mapping/loaddata", pageid).then(function (promise) {

                $scope.client_list = promise.client_list;
                $scope.get_clentlist = promise.get_clentlist;
                $scope.get_department = promise.get_department;
            });
        }
        //=====================project=====================//
        $scope.getproject = function () {
            var data = {
                "ISMMCLT_Id": $scope.obj.ISMMCLT_Id.ismmclT_Id,
                "HRMD_Id": $scope.hrmD_Id
            }
            apiService.create("ISM_ClientProject_Mapping/getproject", data).then(function (promise) {
                $scope.module_list = "";
                $scope.project_list = promise.project_list;
            })
        }
        $scope.interacted2 = function (field) {
            return $scope.submitted;
        };
        //======module============================//
        $scope.getmodule = function (projectid, deptid) {

            var data = {
                "ISMMCLT_Id": $scope.obj.ISMMCLT_Id.ismmclT_Id,
                "ISMMPR_Id": $scope.obj.ISMMPR_Id.ismmpR_Id,
                "HRMD_Id": deptid
            }
            apiService.create("ISM_ClientProject_Mapping/getmodule", data).then(function (promise) {

                $scope.module_list = promise.module_list;
            })
        }
        //================================================save================================================//
        $scope.savedata = function (clint, proj) {

            if ($scope.myForm.$valid) {
                var modulesss = [];
                angular.forEach($scope.module_list, function (rr) {
                    if (rr.selected === true) {
                        modulesss.push(rr);
                    }
                });
                var data = {
                    "ISMMCLTPR_Id": $scope.ismmcltpR_Id,
                    "ISMMCLT_Id": $scope.obj.ISMMCLT_Id.ismmclT_Id,
                    "ISMMPR_Id": $scope.obj.ISMMPR_Id.ismmpR_Id,
                    "moduleidss": modulesss,
                    "ISMMCLTPR_ProposalRefNo": $scope.ISMMCLTPR_ProposalRefNo,
                    "ISMMCLTPR_ProposalSentDate": $scope.ISMMCLTPR_ProposalSentDate,
                    "ISMMCLTPR_DealClosureDate": $scope.ISMMCLTPR_DealClosureDate,
                    "ISMMCLTPR_MOURefNo": $scope.ISMMCLTPR_MOURefNo,
                    "ISMMCLTPR_MOUDate": $scope.ISMMCLTPR_MOUDate,
                    "ISMMCLTPR_MOURepresentedBy": $scope.ISMMCLTPR_MOURepresentedBy,
                    "ISMMCLTPR_MOUStartDate": $scope.ISMMCLTPR_MOUStartDate,
                    "ISMMCLTPR_MOUEndDate": $scope.ISMMCLTPR_MOUEndDate,
                    "ISMMCLTPR_NodalOfficerName": $scope.ISMMCLTPR_NodalOfficerName,
                    "ISMMCLTPR_NodalOfficerContactNo": $scope.ISMMCLTPR_NodalOfficerContactNo,
                    "ISMMCLTPR_NodalOfficerEmailId": $scope.ISMMCLTPR_NodalOfficerEmailId,
                    "ISMMCLTPR_ProjectDuration": $scope.ISMMCLTPR_ProjectDuration,
                    "ISMMCLTPR_TotalStudent": $scope.ISMMCLTPR_TotalStudent,
                    "ISMMCLTPR_CostPerStudent": $scope.ISMMCLTPR_CostPerStudent,
                    "ISMMCLTPR_EnhancementPerYr": $scope.ISMMCLTPR_EnhancementPerYr,
                    "ISMMCLTPR_WorkOrder": $scope.ISMMCLTPR_WorkOrder,
                };
                apiService.create("ISM_ClientProject_Mapping/savedata", data).then(function (promise) {
                    if (promise.msg == 'saved') {
                        swal("Data Saved Successfully...!!!");
                    }
                    else if (promise.msg == 'Failed') {
                        swal("Data Not Saved Successfully...!!!");
                    }
                    else if (promise.msg == 'updated') {
                        swal("Data Successfully Updated");
                    }
                    else {

                        swal("Data Already Exists");
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        }
        //=====================================================edit data================================//
        $scope.Editdata = function (user) {
            debugger;
            var data = {
                "ISMMCLTPR_Id": user.ismmcltpR_Id
            }
            apiService.create("ISM_ClientProject_Mapping/Editdata", data).then(function (promise) {
                debugger;
                if (promise.editlist.length > 0) {
                    $scope.module_list = promise.module_list;
                    $scope.client_list = promise.client_list;
                    $scope.project_list = promise.project_list;
                    $scope.hrmD_Id = promise.editlist[0].hrmD_Id;
                    $scope.ismmcltpR_Id = promise.editlist[0].ismmcltpR_Id;
                    $scope.obj = promise.editlist[0];
                    $scope.ISMMCLTPR_ProposalRefNo = promise.editlist[0].ismmcltpR_ProposalRefNo;
                    $scope.ISMMCLTPR_ProposalSentDate = new Date(promise.editlist[0].ismmcltpR_ProposalSentDate);
                    $scope.ISMMCLTPR_DealClosureDate = new Date(promise.editlist[0].ismmcltpR_DealClosureDate);
                    $scope.ISMMCLTPR_MOURefNo = promise.editlist[0].ismmcltpR_MOURefNo;
                    $scope.ISMMCLTPR_MOUDate = new Date(promise.editlist[0].ismmcltpR_MOUDate);
                    $scope.ISMMCLTPR_MOURepresentedBy = promise.editlist[0].ismmcltpR_MOURepresentedBy;
                    $scope.ISMMCLTPR_MOUStartDate = new Date(promise.editlist[0].ismmcltpR_MOUStartDate);
                    $scope.ISMMCLTPR_MOUEndDate = new Date(promise.editlist[0].ismmcltpR_MOUEndDate);
                    $scope.ISMMCLTPR_NodalOfficerName = promise.editlist[0].ismmcltpR_NodalOfficerName;
                    $scope.ISMMCLTPR_NodalOfficerContactNo = promise.editlist[0].ismmcltpR_NodalOfficerContactNo;
                    $scope.ISMMCLTPR_NodalOfficerEmailId = promise.editlist[0].ismmcltpR_NodalOfficerEmailId;
                    $scope.ISMMCLTPR_ProjectDuration = promise.editlist[0].ismmcltpR_ProjectDuration;
                    $scope.ISMMCLTPR_TotalStudent = promise.editlist[0].ismmcltpR_TotalStudent;
                    $scope.ISMMCLTPR_CostPerStudent = promise.editlist[0].ismmcltpR_CostPerStudent;
                    $scope.ISMMCLTPR_EnhancementPerYr = promise.editlist[0].ismmcltpR_EnhancementPerYr;
                    $scope.ISMMCLTPR_WorkOrder = promise.editlist[0].ismmcltpR_WorkOrder;

                    angular.forEach($scope.module_list, function (yy) {
                        angular.forEach(promise.editlist, function (uu) {
                            if (yy.ivrmM_Id == uu.ivrmM_Id) {
                                yy.selected = true;
                            }
                        })
                    })

                }
            })
        }

        //==========================================deactivate=====================================//
        $scope.clientDecative = function (usersem, SweetAlert) {

            var dystring = "";

            if (usersem.ismmcltpR_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (usersem.ismmcltpR_ActiveFlag == false) {
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
                        apiService.create("ISM_ClientProject_Mapping/clientDecative", usersem).
                            then(function (promise) {

                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d" + " Successfully!!!");
                                    $state.reload();
                                }
                                else {
                                    swal("Record Not " + dystring + "d" + " Successfull!!!");
                                    $state.reload();
                                }
                            })
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        }
        $scope.SelectedFileForUploadzdBOSBOE = [];
        $scope.selectFileforUploadzdBOSBOE = function (input) {
            $scope.SelectedFileForUploadzdBOSBOE = input.files;
            //$('#blah').removeAttr('src');
            if (input.files && input.files[0]) {
                var filename = input.files[0].name.toString();
                var nameArray = filename.split('.');
                $scope.extention = nameArray[nameArray.length - 1];
                if (($scope.extention === "JPEG" || $scope.extention === "jpg" || $scope.extention === "JPG" || $scope.extention === "png" || $scope.extention === "pdf") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#blah')
                            .attr('src', e.target.result);

                        $('#documentid') //hrmedS_Id
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadEmployeeDocumentBOSBOE();
                }
                else if ($scope.extention !== "JPEG" && $scope.extention !== "png" && $scope.extention !== "jpg" && $scope.extention !== "JPG" && $scope.extention !== "pdf") {
                    $('#documentid').removeAttr('src');
                    swal("Please Upload the valid file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    $('#documentid').removeAttr('src');
                    swal("Document size should be less than 2MB");
                    return;
                }
            }
        };

        function UploadEmployeeDocumentBOSBOE() {

            var formData = new FormData();
            for (var i = 0; i <= $scope.selectFileforUploadzdBOSBOE.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzdBOSBOE[i]);
            }

            formData.append("Id", 0);

            var sample1 = Object.fromEntries(formData.entries());

            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadnaacdocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (d !== "PathNotFound") {
                        $scope.hrelT_SupportingDocument = d;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#documentid').removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.showmodaldetailsBOSBOE = function () {
            //$('#preview').removeAttr('src');
            var filename = $scope.hrelT_SupportingDocument.toString();
            var nameArray = filename.split('.');
            $scope.extention = nameArray[nameArray.length - 1];
            if ($scope.extention === "jpg" || $scope.extention === "jpeg" || $scope.extention === "JPG" || $scope.extention === "JPEG" || $scope.extention === "png"
                || $scope.extention === "PNG") {
                $('#preview').attr('src', $scope.hrelT_SupportingDocument);
                //$('#myModal').modal('show');
            }
            else if ($scope.extention === "doc" || $scope.extention === "docx" || $scope.extention === "xls" || $scope.extention === "xlsx") {
                //$('#preview').removeAttr('src');
                $('#preview').attr('src', $scope.hrelT_SupportingDocument);
            }
            else if ($scope.extention === "pdf") {
                var imagedownload = $scope.hrelT_SupportingDocument;
                $scope.content = "";
                var fileURL = "";
                var file = "";
                $http.get(imagedownload, { responseType: 'arraybuffer' })
                    .success(function (response) {
                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);
                        $scope.content = $sce.trustAsResourceUrl(fileURL);
                        $('#showpdf').modal('show');
                    });
            }
        };
        $scope.zoomin = function () {
            var myImg = document.getElementById("preview");
            var currWidth = myImg.clientWidth;
            if (currWidth >= 750) {
                swal("Maximum zoom-in level reached.");
            } else {
                myImg.style.width = (currWidth + 50) + "px";
            }
        };

        $scope.zoomout = function () {
            var myImg = document.getElementById("preview");
            var currWidth = myImg.clientWidth;
            if (currWidth <= 400) {
                swal("Maximum zoom-out level reached.");
            } else {
                myImg.style.width = (currWidth - 50) + "px";
            }
        };
    }
})();
//======================================================================================================//
