(function () {
    'use strict';
    angular.module('app').controller('NaacDocumentApproveProcessController', NaacDocumentApproveProcessController)

    NaacDocumentApproveProcessController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$filter', '$timeout', 'Excel','$sce']
    function NaacDocumentApproveProcessController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $filter, $timeout, Excel, $sce) {

        $scope.valued = "1";
        $scope.obj = {};
        $scope.delete = function (data) {
            data.nodes = [];
        };
        $scope.add = function (data) {
            var post = data.nodes.length + 1;
            var newName = data.name + '-' + post;
            data.nodes.push({ name: newName, nodes: [] });
        };

        $scope.array = [];      

        $scope.onload = function () {
            var pageid = 1;
            apiService.getURI("NaacDocumentUpload/onload", pageid).then(function (promise) {
                if (promise !== null) {
                    $scope.userRole = promise.userRole;
                    $scope.getparentidzero = promise.getparentidzero;
                    $scope.getalldata = promise.getalldata;

                    $scope.per = promise.percentage;

                    if ($scope.valued === "1") {
                        $scope.ddfd = false;
                    } else {
                        $scope.ddfd = true;
                    }
                    $scope.array = [];
                    $scope.getsavealldata = promise.getsavealldata;
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
                                    sdd.document_Path = dds.naacmsL_Uploadpath;
                                    sdd.NAACMSL_Status = dds.naacmsL_Status;
                                    sdd.coordinatorcomments = dds.naacmsL_ConsultantRemarks;
                                    sdd.usercomments = dds.naacmsL_Details;
                                    sdd.status = dds.naacmsL_Status;
                                    sdd.NAACMSL_CGPA = dds.naacmsL_CGPA;
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
                                        sddd.document_Path = ddds.naacmsL_Uploadpath;
                                        sddd.NAACMSL_Status = ddds.naacmsL_Status;
                                        sddd.coordinatorcomments = ddds.naacmsL_ConsultantRemarks;
                                        sddd.usercomments = ddds.naacmsL_Details;
                                        sddd.status = ddds.naacmsL_Status;
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

                        //$timeout(function () {
                        //    $scope.progressValue = amt;
                        //}, 200);

                        //setTimeout(function () {
                        //    $scope.getdata();
                        //}, 200);

                        //$timeout(function () {
                        //    $scope.getdata();
                        //}, 200);
                        //$scope.getdata();

                        //$scope.getdata();
                    });

                    if ($scope.array !== null && $scope.array.length > 0) {
                        $timeout(function () {
                            $scope.getdata();
                        }, 500);
                        // $scope.getdata();
                    }
                }
            });
        };

        $scope.getdata = function () {
            $scope.ddfd = true;
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


        $scope.saveapproved = function (dd) {
            if (dd.status !== null && dd.status !== undefined && dd.status !== "") {
                var uploadpath = dd.document_Path;
                var id = dd.naacsL_Id;
                var status = dd.status;
                var data = {
                    "NAACSL_Id": id,
                    "document_Path": uploadpath,
                    "status": status
                    //"coordinatorcomments": dd.coordinatorcomments
                };

                apiService.create("NaacDocumentUpload/saveapproved", data).then(function (promise) {

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
            } else {
                swal("Kindly Select Either Approved Or Rejected");
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
                            dd.filestatus = dd.NAACMSLF_FileStatus;
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

                        //jQuery.noConflict();
                        jQuery.noConflict();
                        jQuery('#mymodalviewuploaddocument').modal('show');
                       // $('#mymodalviewuploaddocument').modal('show');
                    } else {
                        swal("No Documents Found");
                    }
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
                       // $('#mymodalviewcommentslist').modal('show');
                        jQuery.noConflict();
                        jQuery('#mymodalviewcommentslist').modal('show');
                    } else {
                        swal("No Comments Found");
                    }
                }
            });
        };

        // *************** Save Uploaded File  *************//
        $scope.save = function (dd) {
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
                        swal("Failed Save Record");
                    }
                    $scope.valued = "2";
                    $scope.onload();
                }
            });
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
            var k = 0;
            angular.forEach(user, function (dd) {
                if (dd.filestatus === undefined || dd.filestatus === null || dd.filestatus === "") {
                    $scope.filenamenew = dd.NAACMSLF_FileName;
                    k += 1;
                    swal("Kindly Select The File Status For This Document " + $scope.filenamenew);
                    return;
                }
            });

            if (k === 0) {
                var data = {
                    "temp_dto": user
                };
                apiService.create("NaacDocumentUpload/savecommentscons", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnval === true) {
                            swal("Record Updated");
                        } else {
                            swal("Failed To Save Comments");
                        }
                        $scope.valued = "2";
                        $('#mymodalviewuploaddocument').modal('hide');
                        $scope.onload();
                    }
                });
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
                       // $('#mymodalviewcommentslist').modal('show');
                        jQuery.noConflict();
                        jQuery('#mymodalviewcommentslist').modal('show');

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
           // $('#mymodaladdcomments').modal('show');
            jQuery.noConflict();
            jQuery('#mymodaladdcomments').modal('show');

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
                        jQuery.noConflict();
                        jQuery('#mymodalviewcommentslist').modal('show');
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
            //$('#mymodaladdhyperlinks').modal('show');
            jQuery.noConflict();
            jQuery('#mymodaladdhyperlinks').modal('show');
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
                        //$('#mymodalviewhyperlinks').modal('show');
                        jQuery.noConflict();
                        jQuery('#mymodalviewhyperlinks').modal('show');
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

        $scope.getcgpa = function (add, score) {
            $scope.naacmsL_CGPA = score.NAACMSL_CGPA;
            $scope.NAACSL_Id = add;
            $scope.naacsL_Percentagenew = score.naacsL_Percentage;
            //$('#addcgpa').modal('show');
            jQuery.noConflict();
            jQuery('#addcgpa').modal('show');
        };

        $scope.saveCGPS = function (add) {
            if ($scope.naacmsL_CGPA !== null && $scope.naacmsL_CGPA !== undefined && $scope.naacmsL_CGPA !== "") {

                if (parseFloat($scope.naacmsL_CGPA) <= parseFloat($scope.naacsL_Percentagenew)) {
                    var data = {
                        "NAACSL_Id": $scope.NAACSL_Id,
                        "NAACMSL_CGPA": $scope.naacmsL_CGPA
                    };

                    apiService.create("NaacDocumentUpload/saveCGPA", data).then(function (promise) {
                        if (promise.returnval === true) {
                            swal("CGPA Saved Successfully");
                            //sdd.NAACMSL_CGPA = $scope.naacmsL_CGPA;
                        }
                        else {
                            swal("CGPA Not Saved");
                        }
                        $('#addcgpa').modal('hide');
                        $scope.onload();
                    });
                } else {
                    swal("Entered CGPA Greater-than " + $scope.naacsL_Percentagenew + ", It Should Be Less-than " + $scope.naacsL_Percentagenew+ "");
                }          
            }
            else {
                swal("Enter CGP Score");
            }
        };

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };

        $scope.showpdf = false;

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

        $scope.SelectedFileForUploadzd = [];
        $scope.selectFileforUploadzd = function (input, document) {
            $scope.SelectedFileForUploadzd = input.files;
            if (input.files && input.files[0]) {
                if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.naacsL_Id).attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofiled(document);
                }
                else if (input.files[0].type !== "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    swal("Kindly Upload The Excel File Only");
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
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.clickdd = function () {
            const st = {};

            st.flap = document.querySelector('#flap');
            st.toggle = document.querySelector('.toggle');

            st.choice1 = document.querySelector('#choice1');
            st.choice2 = document.querySelector('#choice2');

            st.flap.addEventListener('transitionend', () => {

                if (st.choice1.checked) {
                    st.toggle.style.transform = 'rotateY(-15deg)';
                    setTimeout(() => st.toggle.style.transform = '', 400);
                } else {
                    st.toggle.style.transform = 'rotateY(15deg)';
                    setTimeout(() => st.toggle.style.transform = '', 400);
                }

            });

            st.clickHandler = (e) => {

                if (e.target.tagName === 'LABEL') {
                    setTimeout(() => {
                        st.flap.children[0].textContent = e.target.textContent;
                    }, 250);
                }
            };

            document.addEventListener('DOMContentLoaded', () => {
                st.flap.children[0].textContent = st.choice2.nextElementSibling.textContent;
            });

            document.addEventListener('click', (e) => st.clickHandler(e));

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



