(function () {
    'use strict';
    angular.module('app').controller('NaacDocumentUploadController', NaacDocumentUploadController)

    NaacDocumentUploadController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$filter', '$timeout', 'Excel', '$sce']
    function NaacDocumentUploadController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $filter, $timeout, Excel, $sce) {

        $scope.obj = {};
        $scope.valued = "1";

        $scope.addnewbtn = true;
        $scope.remflg = false;

        $scope.delete = function (data) {
            data.nodes = [];
        };

        $scope.add = function (data) {
            var post = data.nodes.length + 1;
            var newName = data.name + '-' + post;
            data.nodes.push({ name: newName, nodes: [] });
        };

        $scope.array = [];

        $scope.ddfd = true;
        $scope.onload = function () {
            var pageid = 1;
            apiService.getURI("NaacDocumentUpload/onload", pageid).then(function (promise) {
                if (promise !== null) {
                    $scope.userRole = promise.userRole;

                    $scope.getparentidzero = promise.getparentidzero;
                    $scope.getalldata = promise.getalldata;

                    $scope.array = [];
                    $scope.getsavealldata = promise.getsavealldata;

                    $scope.per = promise.percentage;

                    angular.forEach($scope.getparentidzero, function (dd) {
                        $scope.temparra1d = [];
                        angular.forEach($scope.getalldata, function (ddd) {
                            if (ddd.naacsL_ParentId === dd.naacsL_Id) {
                                $scope.temparra1d.push(ddd);
                            }
                        });

                        angular.forEach($scope.temparra1d, function (sdd) {
                            angular.forEach($scope.getsavealldata, function (dds) {
                                if (sdd.naacsL_Id === dds.naacsL_Id) {
                                    //sdd.document_Path = dds.naacmsL_Uploadpath;
                                    sdd.NAACMSL_Status = dds.naacmsL_Status;
                                    sdd.NAACMSL_ConsultantRemarks = dds.naacmsL_ConsultantRemarks;
                                    sdd.NAACMSL_Status = dds.naacmsL_Status;
                                    sdd.coordinatorcomments = dds.naacmsL_ConsultantRemarks;
                                    sdd.usercomments = dds.naacmsL_Details;
                                    sdd.NAACMSL_CGPA = dds.naacmsL_CGPA;
                                    sdd.NAACMSL_Id = dds.naacmsL_Id;

                                }
                            });
                        });


                        $scope.temparray2d = [];
                        angular.forEach($scope.temparra1d, function (levelii) {
                            $scope.temparray2d = [];
                            angular.forEach($scope.getalldata, function (ddd) {
                                if (ddd.naacsL_ParentId === levelii.naacsL_Id) {
                                    $scope.temparray2d.push(ddd);
                                }
                            });

                            angular.forEach($scope.temparray2d, function (sddd) {
                                angular.forEach($scope.getsavealldata, function (ddds) {
                                    if (sddd.naacsL_Id === ddds.naacsL_Id) {
                                        //sddd.document_Path = ddds.naacmsL_Uploadpath;
                                        sddd.NAACMSL_Status = ddds.naacmsL_Status;
                                        sddd.NAACMSL_ConsultantRemarks = ddds.naacmsL_ConsultantRemarks;
                                        sddd.NAACMSL_Status = ddds.naacmsL_Status;
                                        sddd.coordinatorcomments = ddds.naacmsL_ConsultantRemarks;
                                        sddd.usercomments = ddds.naacmsL_Details;
                                        sddd.NAACMSL_CGPA = ddds.naacmsL_CGPA;
                                    }
                                });
                            });


                            levelii.temparray2 = $scope.temparray2d;
                        });

                        $scope.array.push({ NAACSL_Id: dd.naacsL_Id, NAACSL_SLNo: dd.naacsL_SLNo, NAACSL_SLNoDescription: dd.naacsL_SLNoDescription, temparra1: $scope.temparra1d });

                        console.log($scope.array);


                        var amt = 60;

                        $scope.countTo = amt;
                        $scope.countFrom = 0;

                    });

                    //$timeout(function () {
                    //    $scope.progressValue = amt;
                    //}, 10);

                    if ($scope.array !== null && $scope.array.length > 0) {
                        $timeout(function () {
                            $scope.getdata();
                        }, 500);
                        $scope.getdata();
                    }
                }
            });
        };

        $scope.getdata = function () {
            $scope.ddfd = true;
            console.log("==============================");
            console.log($scope.array);
            var tree = document.querySelectorAll('ul.tree a:not(:last-child)');
            for (var i = 0; i < tree.length; i++) {
                tree[i].addEventListener('click', function (e) {
                    var parent = e.target.parentElement;
                    var classList = parent.classList;
                    if (classList.contains("open")) {
                        classList.remove('open');
                        var opensubs = parent.querySelectorAll(':scope .open');
                        for (var i = 0; i < opensubs.length; i++) {
                            opensubs[i].classList.remove('open');
                        }
                    } else {
                        classList.add('open');
                    }
                });
            }
        };


        // *************** Get Upload Document List *************//
        $scope.getuploaddoc = function (ddd) {
            $scope.viewdocuments = [];
            apiService.create("NaacDocumentUpload/getuploaddoc", ddd).then(function (promise) {
                if (promise !== null) {
                    if (promise.getdocumentlist !== null && promise.getdocumentlist.length > 0) {
                        $scope.viewdocuments = promise.getdocumentlist;

                        angular.forEach($scope.viewdocuments, function (dd) {
                            dd.usercomments = "";
                        });

                        angular.forEach($scope.viewdocuments, function (dd) {
                            var img = dd.NAACMSLF_FilePath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;

                            if (lastelement === 'xlsx' || lastelement === 'xls' || lastelement === 'xlsm' || lastelement === 'docx' || lastelement === 'doc') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.NAACMSLF_FilePath;
                            }
                        });
                        $('#mymodalviewuploaddocument').modal('show');
                    } else {
                        swal("No Documents Found");
                    }
                }
            });
        };

        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
            $('#myModalimg').modal('show');
        };


        // ************* Delete Upload Documents ************ //

        $scope.deleteuploadfile = function (user) {

            swal({
                title: "Are you sure?",
                text: "Do you want to Delete this item?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,Delete it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        var data = {
                            "NAACMSLF_Id": user.NAACMSLF_Id
                        };
                        apiService.create("NaacDocumentUpload/deleteuploadfile", data).then(function (promise) {
                            if (promise !== null) {
                                if (promise.returnval === true) {
                                    swal("Record Deleted Successfully");
                                } else {
                                    swal("Failed To Delete The Record");
                                }
                                $('#mymodalviewuploaddocument').modal('hide');
                                $scope.onload();
                            }
                        });
                    }
                    else {
                        swal("Cancelled");
                        //  $state.reload();
                    }
                });

        };



        $scope.aaaaa = function (user) {
            var data = {
                "NAACSL_Id": user.NAACSL_Id,
                "NAACMSLF_Id": user.NAACMSLF_Id
            };

            apiService.create("NaacDocumentUpload/viewcomments", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.getcommentslist !== null && promise.getcommentslist.length > 0) {
                        $scope.viewcomments = promise.getcommentslist;
                        //jQuery.noConflict();
                        $('#mymodalviewcommentslist').modal('show');

                    } else {
                        swal("No Comments Found");
                    }
                }
            });
        };

        // *************** Save Uploaded File  *************//
        $scope.save = function (dd) {
            var k = 0;

            if ((dd.usercomments === undefined || dd.usercomments === null || dd.usercomments === "") &&
                (dd.document_Path === undefined || dd.document_Path === null || dd.document_Path === "")) {
                k = 1;
                swal("Upload The Documents Or Enter The Comments To Save The Details");
            }

            if (k === 0) {
                var uploadpath = dd.document_Path;
                var id = dd.naacsL_Id;
                var templatepath = dd.naascL_Template;

                var data = {
                    "NAACSL_Id": id,
                    "document_Path": uploadpath,
                    "comments": dd.usercomments,
                    "file_name": dd.file_name,
                    "NAACMSLF_Id": dd.NAACMSLF_Id
                };

                apiService.create("NaacDocumentUpload/save", data).then(function (promise) {

                    if (promise !== null) {
                        if (promise.returnval === true) {
                            swal("Record Saved Successfully");
                        } else {
                            swal("Failed To Save Record");
                        }
                        $scope.valued = "2";
                        $scope.onload();
                    }
                });
            }

        };
        var imagedownload = "";
        var docname = "";

        //************ Download Uploaded Files ***************//
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
                        download: studentreg
                    })[0].click();
                });
        };


        //**************** Save Comments File Wise *************//
        $scope.savecomments = function (user) {
            $scope.tempcomments = [];
            angular.forEach(user, function (dd) {
                if (dd.usercomments !== null && dd.usercomments !== null && dd.usercomments !== "") {
                    $scope.tempcomments.push({
                        NAACMSLF_FileName: dd.NAACMSLF_FileName, NAACMSLF_FilePath: dd.NAACMSLF_FilePath, usercomments: dd.usercomments,
                        NAACSL_Id: dd.NAACSL_Id, NAACMSLF_Id: dd.NAACMSLF_Id
                    });
                }
            });
            if ($scope.tempcomments !== null && $scope.tempcomments.length > 0) {
                var data = {
                    "temp_dto": $scope.tempcomments
                };

                apiService.create("NaacDocumentUpload/savecomments", data).then(function (promise) {

                    if (promise !== null) {
                        if (promise.returnval === true) {
                            swal("Comments Saved");
                        } else {
                            swal("Failed To Save Comments");
                        }

                        $scope.valued = "2";
                        $scope.onload();
                    }

                });
            } else {
                swal("Enter Atleast One Comment To Save The Details");
            }
        };

        $scope.user = {};
        //*************** View Comments Saved For File Wise ***************//
        $scope.viewcomments1 = function (user) {

            var data = {
                "NAACSL_Id": user.NAACSL_Id,
                "NAACMSLF_Id": user.NAACMSLF_Id
            };

            apiService.create("NaacDocumentUpload/viewcomments", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.getcommentslist !== null && promise.getcommentslist.length > 0) {
                        $scope.viewcomments = promise.getcommentslist;
                        //jQuery.noConflict();
                        $('#mymodalviewcommentslist').modal('show');

                    } else {
                        swal("No Comments Found");
                    }
                }
            });
        };


        $scope.closecommentslist = function () {
            //  $scope.viewcomments = "";          
        };


        //*************** Open General Comments ***************//
        $scope.addcomments = function (dd) {
            $scope.commentsid = dd.naacsL_Id;
            $scope.commentslno = dd.naacsL_SLNo;
            $scope.obj.generalcomments = "";
            //jQuery.noConflict();
            $('#mymodaladdcomments').modal('show');

        };

        //*************** Save General Comments ***************//
        $scope.savegeneralcommetns = function (obj) {
            var data = {
                "NAACMSLCO_Remarks": obj.generalcomments,
                "NAACSL_Id": $scope.commentsid
            };

            apiService.create("NaacDocumentUpload/savegeneralcommetns", data).then(function (promise) {

                if (promise !== null) {
                    if (promise.returnval === true) {
                        swal("Comments Saved Successfully");
                    } else {
                        swal("Failed To Save Comments");
                    }
                    $('#mymodaladdcomments').modal('hide');
                    $scope.valued = "2";
                    $scope.onload();
                }
            });
        };

        // *************** Get General Comments List *************//
        $scope.getcomments = function (ddd) {
            apiService.create("NaacDocumentUpload/getcomments", ddd).then(function (promise) {
                if (promise !== null) {
                    if (promise.getgeneralcommentslist !== null && promise.getgeneralcommentslist.length > 0) {
                        $scope.viewcomments = promise.getgeneralcommentslist;
                        //jQuery.noConflict();
                        $('#mymodalviewcommentslist').modal('show');
                    } else {
                        swal("No Comments Found");
                    }
                }
            });
        };

        // ****************** Add / Save / View Hyper Links To Criteria ********************** //

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.addhyperlink = function (ddd) {

            $scope.hyperlinkdetails = [{ id: 'mobilestd1', NAACMSLLK_LinkName: undefined, NAACMSLLK_LinkRemarks: undefined }];

            $scope.hyderlinkid = ddd.naacsL_Id;

            $scope.addflg = true;
            $('#mymodaladdhyperlinks').modal('show');
        };

        $scope.addNewMobile1std = function () {
            var newItemNostd = $scope.hyperlinkdetails.length + 1;
            if (newItemNostd <= 5) {
                $scope.hyperlinkdetails.push({ 'id': 'mobilestd1' + newItemNostd, NAACMSLLK_LinkName: undefined, NAACMSLLK_LinkRemarks: undefined });
            }
            if (newItemNostd === 4) {
                $scope.myForm1.$setPristine();
            }
        };

        $scope.removeNewMobile1std = function (index, curval1std) {
            var newItemNostd2 = $scope.hyperlinkdetails.length - 1;
            if (newItemNostd2 !== 0) {
                $scope.delmsrd = $scope.hyperlinkdetails.splice(newItemNostd2, 1);
            }
        };

        $scope.savehyperlinks = function (obj) {
            $scope.submitted = false;
            if ($scope.myForm1.$valid) {
                console.log(obj);

                var k = 0;
                angular.forEach(obj, function (dddddd) {
                    if (dddddd.NAACMSLLK_LinkName === undefined || dddddd.NAACMSLLK_LinkName === null || dddddd.NAACMSLLK_LinkName === "") {
                        k += 1;
                        swal("Kindly Enter The Link");
                    }
                });

                if (k === 0) {
                    $('#mymodaladdhyperlinks').modal('hide');
                    var data = {
                        "NAACSL_Id": $scope.hyderlinkid,
                        "temp_hyperlink_dto": obj
                    };
                    apiService.create("NaacDocumentUpload/savehyperlinks", data).then(function (promise) {
                        if (promise !== null) {
                            if (promise.returnval === true) {
                                swal("Record Saved Successfully");
                            } else {
                                swal("Failed To Save Record");
                            }
                        } else {
                            swal("Failed To Save Record");
                        }
                        $scope.onload();
                    });
                }
            } else {
                $scope.submitted = true;
            }
        };


        $scope.viewaddedhyperlink = function (obj) {
            $scope.viewhyperlinks = [];
            var data = {
                "NAACSL_Id": obj.naacsL_Id
            };
            apiService.create("NaacDocumentUpload/viewaddedhyperlink", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.viewhyperlinks !== null && promise.viewhyperlinks.length > 0) {
                        $scope.viewhyperlinks = promise.viewhyperlinks;
                        $('#mymodalviewhyperlinks').modal('show');
                    } else {
                        swal("No Records Found");
                    }
                } else {
                    swal("No Records Found");
                }
            });
        };

        $scope.deletehyperlink = function (dd) {

            swal({
                title: "Are you sure?",
                text: "Do you want to Delete this item?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,Delete it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        var data = {
                            "NAACMSLLK_Id": dd.naacmsllK_Id
                        };
                        apiService.create("NaacDocumentUpload/deletehyperlink", data).then(function (promise) {
                            if (promise !== null) {
                                if (promise.returnval === true) {
                                    swal("Record Deleted Successfully");
                                } else {
                                    swal("Failed To Delete The Record");
                                }
                                $('#mymodalviewhyperlinks').modal('hide');
                                $scope.onload();
                            }
                        });
                    }
                    else {
                        swal("Cancelled");
                    }
                });
        };


        $scope.showpdf = false;

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };

        $scope.downloadview = function (pdfview) {

            var imagedownload = pdfview;
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

            //$scope.pdfurl = pdfview;
            //$scope.showpdf = true;
            //$('#showpdf').modal('show');

        };

        $scope.backtoview = function () {
            $scope.showpdf = false;
        };


        // *************** Upload File Function *************//
        $scope.SelectedFileForUploadzd = [];

        $scope.selectFileforUploadzd = function (input, document) {

            $scope.SelectedFileForUploadzd = input.files;

            if (input.files && input.files[0]) {
                if ((input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    || input.files[0].type === "application/pdf" || input.files[0].type === "application/msword"
                    || input.files[0].type === "application/vnd.ms-excel" || input.files[0].type === "image/jpeg")) {
                    if (input.files[0].size <= 2097152) {
                        $scope.filename = input.files[0].name;
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            $('#' + document.naacsL_Id).attr('src', e.target.result);
                        };
                        reader.readAsDataURL(input.files[0]);
                        Uploadprofiled(document);
                    } else {
                        swal("File Size Should Be Less Than 2 MB"); // 2097152 bytes = 2MB 
                    }
                }
                else {
                    swal("Upload Only Pdf, Doc , Excel ANd JPEG File Only");
                    return;
                }
            }
        };

        function Uploadprofiled(data) {
            console.log(data);
            var formData = new FormData();
            for (var i = 0; i <= $scope.selectFileforUploadzd.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzd[i]);
            }
            formData.append("Id", data.naacsL_Id);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadnaacdocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    data.document_Path = d;
                    data.file_name = $scope.filename;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

            console.log(data);
        }
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



