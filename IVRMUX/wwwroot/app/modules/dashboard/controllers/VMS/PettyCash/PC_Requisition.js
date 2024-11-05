(function () {
    'use strict';
    angular.module('app').controller('PC_RequisitionController', PC_RequisitionController)

    PC_RequisitionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q', '$sce']
    function PC_RequisitionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q, $sce) {

        $scope.submitted = false;
        $scope.totalgrid = [];
        $scope.PCREQTN_Date = new Date();
        $scope.editflag = false;
        $scope.maxdate = new Date();
        $scope.editadd = false;
        $scope.vehicledocuupload = {};

        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";

            var pageid = 2;
            apiService.getURI("PC_Requisition/onloaddata", pageid).then(function (promise) {
                $scope.getdept = promise.getdept;
                $scope.getparticulars = promise.getparticulars;
                $scope.getloaddata = promise.getloaddata;
                $scope.getapprovedindent = promise.getapprovedindent;

                angular.forEach($scope.getloaddata, function (d) {
                    d.approvedflag = false;
                    angular.forEach($scope.getapprovedindent, function (dd) {
                        if (d.pcreqtN_Id === dd.pcreqtN_Id) {
                            d.approvedflag = true;
                        }
                    });
                });
            });
        };

        $scope.onchangedept = function () {
            $scope.totalgrid = [];
            $scope.totalgrid = [{ id: 'pcreq1' }];
            var data = {
                "HRMD_Id": $scope.HRMD_Id.hrmD_Id
            };
            apiService.create("PC_Requisition/onchangedept", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getemp = promise.getemp;

                }

            });
        };

        $scope.onchangeparticuar = function (user, index) {
            for (var k = 0; k < $scope.totalgrid.length; k++) {
                var roll = parseInt(user.pcmparT_Id);
                var arryind = $scope.totalgrid.indexOf($scope.totalgrid[k]);
                console.log(arryind);
                if (arryind !== index) {
                    var rollindex = parseInt($scope.totalgrid[k].PCMPART_Id.pcmparT_Id);
                    if (rollindex === roll) {
                        swal("Particular Already Exists");
                        $scope.totalgrid[index].PCMPART_Id = "";
                        $scope.totalgrid[index].amount = "";
                        $scope.totalgrid[index].remarks = "";
                        break;
                    }
                }
            }
        };

        $scope.onchangeamount = function (userobj) {
            $scope.PCREQTN_TotAmount = 0.00;
            $scope.PCREQTN_TotAmounttemp = 0.00;

            angular.forEach($scope.totalgrid, function (dd) {
                if (dd.amount !== undefined && dd.amount !== null && dd.amount !== "") {
                    $scope.PCREQTN_TotAmount = $scope.PCREQTN_TotAmount + parseFloat(dd.amount);
                    $scope.PCREQTN_TotAmounttemp = $scope.PCREQTN_TotAmounttemp + parseFloat(dd.amount);
                }
            });
        };

        $scope.saverecord = function () {
            if ($scope.myForm.$valid) {

                $scope.savePC_Requisition_DetailsDTO = [];

                angular.forEach($scope.totalgrid, function (ddd) {
                    if (ddd.remarks === undefined || ddd.remarks === null || ddd.remarks === "") {
                        ddd.remarks = "";
                    }
                    $scope.savePC_Requisition_DetailsDTO.push({
                        PCMPART_Id: ddd.PCMPART_Id.pcmparT_Id,
                        PCMPART_ParticularName: ddd.PCMPART_Id.pcmparT_ParticularName,
                        PCREQTNDET_Amount: parseFloat(ddd.amount), PCREQTNDET_Remarks: ddd.remarks, PCREQTNDET_Id: ddd.pcreqtndeT_Id
                    });

                });

                $scope.uploaddocments = [];

                angular.forEach($scope.vehicledocuupload, function (dd) {
                    if (dd.TRVCTD_FileLocation !== undefined && dd.TRVCTD_FileLocation !== null && dd.TRVCTD_FileLocation !== "") {
                        // $scope.uploaddocments.push(dd);
                        $scope.uploaddocments.push({ PCREQTNUP_FileName: dd.TRVCTD_FileName, PCREQTNUP_FileLocation: dd.TRVCTD_FileLocation, PCREQTNUP_Id: dd.TRVCTD_Id });
                    }
                });

                var data = {
                    "PCREQTN_Id": $scope.PCREQTN_Id,
                    "HRMD_Id": $scope.HRMD_Id.hrmD_Id,
                    "HRME_Id": $scope.HRME_Id.hrmE_Id,
                    "PCREQTN_Purpose": $scope.PCREQTN_Purpose,
                    "PCREQTN_TotAmount": $scope.PCREQTN_TotAmount,
                    "PCREQTN_Date": new Date($scope.PCREQTN_Date).toDateString(),
                    "PC_Requisition_DetailsDTO": $scope.savePC_Requisition_DetailsDTO,
                    uploaddocments: $scope.uploaddocments
                };

                apiService.create("PC_Requisition/saverecord", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.message === "Duplicate") {
                            swal("Record Already Exists");
                        } else if (promise.message === "Add") {
                            if (promise.returnval === true) {
                                swal("Record Saved Successfully");
                            } else {
                                swal("Failed To Save Record");
                            }
                        } else if (promise.message === "Update") {
                            if (promise.returnval === true) {
                                swal("Record Updated Successfully");
                            } else {
                                swal("Failed To Update Record");
                            }
                        } else if (promise.message === "Error") {
                            swal("Failed To Save/ Update Record");
                        } else {
                            swal("Something Went Wrong Contact Administrator");
                        }
                        $state.reload();
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.EditData = function (user) {
            var data = {
                "PCREQTN_Id": user.pcreqtN_Id
            };
            apiService.create("PC_Requisition/EditData", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.geteditdetails !== null && promise.geteditdetails.length > 0) {
                        $scope.geteditdept = promise.geteditdept;
                        $scope.HRMD_Id = $scope.geteditdept[0];

                        $scope.getemp = promise.geteditemp;
                        $scope.geteditsavedemp = promise.geteditsavedemp;
                        $scope.HRME_Id = $scope.geteditsavedemp[0];

                        $scope.PCREQTN_Purpose = promise.geteditdetails[0].pcreqtN_Purpose;
                        $scope.PCREQTN_Date = new Date(promise.geteditdetails[0].pcreqtN_Date);
                        $scope.PCREQTN_TotAmount = promise.geteditdetails[0].pcreqtN_TotAmount;
                        $scope.PCREQTN_TotAmounttemp = promise.geteditdetails[0].pcreqtN_TotAmount;
                        $scope.PCREQTN_Id = promise.geteditdetails[0].pcreqtN_Id;

                        $scope.geteditsavedparticulars = promise.geteditsavedparticulars;

                        $scope.totalgrid = [];

                        angular.forEach($scope.geteditsavedparticulars, function (dd) {
                            $scope.totalgrid.push({
                                PCMPART_Id: dd, amount: dd.pcreqtndeT_Amount, remarks: dd.pcreqtndeT_Remarks,
                                pcreqtndeT_Id: dd.pcreqtndeT_Id, editfalguser: true, editadduser: true
                            });
                        });

                        $scope.vehicledocuupload = [];
                        if (promise.documentdetails != null && promise.documentdetails.length > 0) {
                            angular.forEach(promise.documentdetails, function (ee) {
                                var img = ee.pcreqtnuP_FileLocation;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                $scope.filetype = lastelement;
                                $scope.vehicledocuupload.push({ TRVCTD_FileName: ee.pcreqtnuP_FileName, TRVCTD_FileLocation: ee.pcreqtnuP_FileLocation, PCREQTN_Id: ee.pcreqtN_Id, PCREQTNUP_Id: ee.pcreqtnuP_Id, filetype: $scope.filetype })
                                //$scope.vehicledocuupload.push({ PCREQTNUP_FileName: ee.pcreqtnuP_FileName, PCREQTNUP_FileLocation: ee.pcreqtnuP_FileLocation, PCREQTN_Id: ee.pcreqtN_Id, PCREQTNUP_Id: ee.pcreqtnuP_Id, filetype: $scope.filetype })

                            })
                        }

                        $scope.editflag = true;
                        $scope.editadd = true;
                    }
                }

            });
        };




        $scope.uploadeddocuemnt = function (user) {
            $scope.getviewdocdata = [];
            var data = {
                "PCREQTN_Id": user.pcreqtN_Id
            }
            apiService.create("PC_Requisition/EditData", data).then(function (promise) {
                if (promise.documentdetails.length > 0) {


                    $scope.getviewdocdata = [];
                    if (promise.documentdetails != null && promise.documentdetails.length > 0) {
                        angular.forEach(promise.documentdetails, function (ee) {
                            var img = ee.pcreqtnuP_FileLocation;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            $scope.filetype = lastelement;
                           // $scope.getviewdata.push({ PCREQTNUP_FileName: ee.pcreqtnuP_FileName, PCREQTNUP_FileLocation: ee.pcreqtnuP_FileLocation, PCREQTN_Id: ee.pcreqtN_Id, PCREQTNUP_Id: ee.pcreqtnuP_Id, filetype: $scope.filetype })
                            $scope.getviewdocdata.push({ TRVCTD_FileName: ee.pcreqtnuP_FileName, TRVCTD_FileLocation: ee.pcreqtnuP_FileLocation, PCREQTN_Id: ee.pcreqtN_Id, PCREQTNUP_Id: ee.pcreqtnuP_Id, filetype: $scope.filetype })

                        })
                    }
                }
                else {
                    swal("No Record Found")
                    $('#documentlist').modal('hide');
                }
            })
        }


        $scope.deactiveY = function (user, SweetAlert) {


            var data = {
                "PCREQTN_Id": user.pcreqtN_Id
            };

            var dystring = "";
            if (user.pcreqtN_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (user.pcreqtN_ActiveFlg === false) {
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
                        apiService.create("PC_Requisition/deactiveY", data).then(function (promise) {
                            if (promise.message === "Mapped") {
                                swal("Record Is Already Mapped, So You Can Not Deactive The Record");
                            } else {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            }
                        });
                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }
                });
        };

        $scope.Viewdata = function (user) {
            $scope.PCREQTN_RequisitionNotemp = user.pcreqtN_RequisitionNo;
            $scope.viewapprovedflag = user.approvedflag;
            var data = {
                "PCREQTN_Id": user.pcreqtN_Id
            };
            apiService.create("PC_Requisition/Viewdata", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getviewdata = promise.getviewdata;
                }
            });
        };

        $scope.deactiveparticulars = function (user, SweetAlert) {
            var data = {
                "pcreqtndeT_Id": user.pcreqtndeT_Id,
                "PCREQTN_Id": user.pcreqtN_Id
            };

            var dystring = "";
            if (user.pcreqtndeT_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (user.pcreqtndeT_ActiveFlg === false) {
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
                        apiService.create("PC_Requisition/deactiveparticulars", data).then(function (promise) {
                            if (promise.message === "Mapped") {
                                swal("Record Is Already Mapped, So You Can Not Deactive The Record");
                            } else {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $scope.getviewdata = promise.getviewdata;
                            }
                        });
                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }
                });
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.totalgrid = [{ id: 'pcreq1' }];

        $scope.addNewsiblingguarditem = function () {
            var newItemNo = $scope.totalgrid.length + 1;
            if (newItemNo <= 50) {
                $scope.totalgrid.push({ 'id': 'pcreq' + newItemNo, editfalguser: false, editadduser: false });
            }
            console.log($scope.totalgrid);
            if ($scope.editflag === true) {
                $scope.editadd = false;
            }
        };

        $scope.removeNewsiblingguarditem = function (index) {
            var newItemNo = $scope.totalgrid.length - 1;
            $scope.totalgrid.splice(index, 1);

            if ($scope.totalgrid.length === 0) {
                //data
            }

            $scope.PCREQTN_TotAmount = 0.00;
            $scope.PCREQTN_TotAmounttemp = 0.00;
            angular.forEach($scope.totalgrid, function (dd) {
                if (dd.amount !== undefined && dd.amount !== null && dd.amount !== "") {
                    $scope.PCREQTN_TotAmount = $scope.PCREQTN_TotAmount + parseFloat(dd.amount);
                    $scope.PCREQTN_TotAmounttemp = $scope.PCREQTN_TotAmounttemp + parseFloat(dd.amount);
                }
            });
        };

        $scope.search = '';

        $scope.filterValue1 = function (obj) {
            return ($filter('date')(obj.pcreqtN_Date, 'dd/MM/yyyy').indexOf($scope.search) >= 0) ||
                (angular.lowercase(obj.employeename)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.pcreqtN_RequisitionNo)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.departmentname)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.pcreqtN_Purpose)).indexOf(angular.lowercase($scope.search)) >= 0;
        };






        //Over all  Documents Upload
        $scope.uploadvisitordocuments1 = [];
        $scope.uploadvisitordocuments = function (input, document) {

            $scope.uploadvisitordocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {
                //$scope.size = input.files[0].size;
                if (input.files[0].type === "image/jpeg")  // 2097152 bytes = 2MB 
                {
                    UploadVisitorDocPhoto(document);
                }
                else if (input.files[0].type === "video/mp4") {
                    UploadVisitorDocPhoto(document);
                }
                else if (input.files[0].type === "application/pdf") {
                    UploadVisitorDocPhoto(document);
                }
                else if (input.files[0].type === "application/msword") {
                    UploadVisitorDocPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    UploadVisitorDocPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel") {
                    UploadVisitorDocPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document,") {
                    UploadVisitorDocPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.presentationml.presentation") {
                    UploadVisitorDocPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-powerpoint") {
                    UploadVisitorDocPhoto(document);
                }
                else {
                    swal("Upload MP4, Pdf, Doc, Image Files Only");
                }
            }
        };
        function UploadVisitorDocPhoto(data) {
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadvisitordocuments1.length; i++) {
                formData.append("File", $scope.uploadvisitordocuments1[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/uploadvehicleimg", formData,
                {
                    withCredentials: true,
                    headers: {
                        'Content-Type': undefined
                    },
                    transformRequest: angular.identity,

                })
                .success(function (d) {
                    defer.resolve(d);
                    data.TRVCTD_FileLocation = d;
                    data.TRVCTD_FileName = $scope.filename;
                    $('#').attr('src', data.TRVCTD_FileLocation);
                    var img = data.TRVCTD_FileLocation;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    console.log("data.filetype : " + data.filetype);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.TRVCTD_FileLocation;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.onview = function (filepath, filename) {
            //var myPdfUrl = 'https://bdcampusstrg.blob.core.windows.net/files/6/LessonPlanner/1e55cab1-2e21-4878-802d-ad87b8507e6b.pdf';
            var imagedownload = filepath;
            $scope.content = "";
            var fileURL = "";
            var file = "";
            document.getElementById("pdfviewdd").innerHTML = "";
            $http.get(imagedownload, { responseType: 'arraybuffer' })
                .success(function (response) {
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);
                    $scope.content = $sce.trustAsResourceUrl(fileURL);
                    var htmlElements = '<embed id="pdfview" src="' + $scope.content + '"  style="width: 100%;" height="1000" type="application/pdf" />';
                    document.getElementById("pdfviewdd").innerHTML = htmlElements;
                    $('#showpdf').modal('show');
                });
        };

        $scope.showGuardianSign = function (data) {
            $('#preview').attr('src', data.vmmvvI_VisitorPhoto);
        };

        $scope.showmothersign = function (path) {
            $('#preview1').attr('src', path);
            //$('img').bind('contextmenu', function (e) {
            //    return false;
            //});          
            $('#myModalimg').modal('show');
        };


        $scope.showGuardianPhotonew = function (data) {
            $scope.view_videos = [];
            $scope.videoSources = [];
            $scope.preview1 = data.lpmtR_Resources;
            $scope.videdfd = data.lpmtR_Resources;
            $scope.movie = { src: data.lpmtR_Resources };
            $scope.movie1 = { src: data.lpmtR_Resources };
            $scope.view_videos.push({ id: 1, coeeV_Videos: data.lpmtR_Resources });
            console.log($scope.view_videos);
        };

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };


        var imagedownload = "";
        $scope.downloaddirectimage = function (data, idd, type) {

            var studentreg = idd;
            $scope.examstart_time = true;
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
                        download: studentreg + '.' + type
                    })[0].click();
                });
        };

        $scope.vehicledocuupload = [{ id: 'Teacher1' }];

        $scope.addNewsiblingguard = function () {
            var newItemNo = $scope.vehicledocuupload.length + 1;

            if (newItemNo <= 10) {
                $scope.vehicledocuupload.push({ 'id': 'Teacher' + newItemNo });
            }
            console.log($scope.vehicledocuupload);
        };

        $scope.removeNewsiblingguard = function (index) {
            var newItemNo = $scope.vehicledocuupload.length - 1;
            $scope.vehicledocuupload.splice(index, 1);

            if ($scope.vehicledocuupload.length === 0) {
                //data
            }
        };




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






    }
})();

